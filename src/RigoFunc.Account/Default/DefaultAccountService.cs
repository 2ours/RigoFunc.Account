using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityModel;
using Love.Net.Core;
using Newtonsoft.Json;
using RigoFunc.Account.Models;
using RigoFunc.Account.Services;
using RigoFunc.OAuth;
using RigoFunc.Utils;
using System.Collections.Generic;

namespace RigoFunc.Account.Default {
    /// <summary>
    /// Represents the default implementation of the <see cref="IAccountService"/> interface.
    /// </summary>
    /// <typeparam name="TUser">The type of the t user.</typeparam>
    internal class DefaultAccountService<TUser> : IAccountService where TUser : class {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;
        private readonly IUserFactory<TUser> _userFactory;
        private readonly ISmsSender _smsSender;
        private readonly ILogger<DefaultAccountService<TUser>> _logger;
        private readonly ApiOptions _options;
        private const string DefaultSecurityStamp = "022a9e42-9509-4aa6-8a0a-c34a1f405c61";
        private const string WChatLoginProvider = "wxLoginProvider";
        private IAccessTokenProvider _accessTokenProvider;
        private readonly InvokeErrorDescriber _errorDescriber;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAccountService{TUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="userFactory">The user factory.</param>
        /// <param name="smsSender">The SMS sender.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="options">The options.</param>
        /// <param name="accessTokenProvider"></param>
        /// <param name="describer">The error describer</param>
        public DefaultAccountService(UserManager<TUser> userManager,
            SignInManager<TUser> signInManager,
            IUserFactory<TUser> userFactory,
            ISmsSender smsSender,
            ILogger<DefaultAccountService<TUser>> logger,
            IOptions<ApiOptions> options,
            IAccessTokenProvider accessTokenProvider,
            InvokeErrorDescriber describer = null) {
            _userManager = userManager;
            _signInManager = signInManager;
            _userFactory = userFactory;
            _smsSender = smsSender;
            _accessTokenProvider = accessTokenProvider;
            _options = options.Value;
            _logger = logger;
            _errorDescriber = describer ?? new InvokeErrorDescriber();
        }

        /// <summary>
        /// Gets OAuth user by the specified user Id or Phone number asynchronous.
        /// </summary>
        /// <param name="model">The model contains the user Id or Phone number.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the get operation. Task result contains the found user.</returns>
        public async Task<OAuthUser> GetAsync(FindUserModel model) {
            TUser user;
            if (model.Id != null) {
                user = await _userManager.FindByIdAsync(model.Id.ToString());
            }
            else {
                user = await _userManager.FindByNameAsync(model.PhoneNumber);
            }

            if (user == null) {
                _logger.LogWarning($"cannot find the user by id or phone number: {JsonConvert.SerializeObject(model)}");

                return null;
            }

            var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

            if (_userManager.SupportsUserEmail) {
                var email = await _userManager.GetEmailAsync(user);
                if (!string.IsNullOrWhiteSpace(email)) {
                    userPrincipal.Identities.First().AddClaims(new[]
                    {
                        new Claim(JwtClaimTypes.Email, email),
                        new Claim(JwtClaimTypes.EmailVerified,
                            await _userManager.IsEmailConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
                    });
                }
            }

            if (_userManager.SupportsUserPhoneNumber) {
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (!string.IsNullOrWhiteSpace(phoneNumber)) {
                    userPrincipal.Identities.First().AddClaims(new[]
                    {
                        new Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
                        new Claim(JwtClaimTypes.PhoneNumberVerified,
                            await _userManager.IsPhoneNumberConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
                    });
                }
            }

            return OAuthUser.FromUser(userPrincipal);
        }

        /// <summary>
        /// Sets a flag indicating whether the specified user is locked out, as an asynchronous operation.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, the result contain a bool indicating whether succeeded.</returns>
        public async Task<bool> LockoutAsync(LockoutModel model) {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) {
                _errorDescriber.UserNotFoundByPhoneNumber(model.UserName).Throw();
            }

            var result = await _userManager.SetLockoutEnabledAsync(user, model.Enabled);
            if (!result.Succeeded) {
                _errorDescriber.UserLockoutFailure().Throw();
            }

            if (model.Enabled) {
                // lockout a year
                result = await _userManager.SetLockoutEndDateAsync(user, model.LockoutEnd ?? DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(365)));
            }

            return true;
        }

        /// <summary>
        /// Creates  a new user asynchronous. This method will not require end-user to provide the send code to their phone number and will send a random password to their phone.
        /// </summary>
        /// <param name="model">The register model.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, the result contain a bool indicating whether succeeded.</returns>
        public async Task<bool> CreateAsync(RegisterModel model) {
            var user = await _userManager.FindByNameAsync(model.UserName ?? model.PhoneNumber);
            if (user != null) {
                _errorDescriber.PhoneNumberHadBeenRegister(model.PhoneNumber).Throw();
            }

            user = _userFactory.CreateUser(model.UserName ?? model.PhoneNumber);

            // why md5 here? because we should force APP or web to MD5 their plain password
            var password = model.Password ?? $"{GenericUtil.UniqueKey(3)}@{model.Code ?? GenerateCode(model.PhoneNumber)}";
            var result = await _userManager.CreateAsync(user, GenericUtil.EncryptMD5(password));
            if (!result.Succeeded) {
                var error = _errorDescriber.RegisterNewUserFailure(model.PhoneNumber, model.Code);
                error.Details = HandleErrors(result);
                error.Throw();
            }

            // set phone number.
            if (_userManager.SupportsUserPhoneNumber) {
                result = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!result.Succeeded) {
                    var error = _errorDescriber.RegisterNewUserFailure(model.PhoneNumber, model.Code);
                    error.Details = HandleErrors(result);
                    error.Throw();
                }
            }

            InvokeResult invokeResult;
            if (string.IsNullOrWhiteSpace(_options.PasswordSmsTemplate)) {
                invokeResult = await _smsSender.SendSmsAsync(model.PhoneNumber, password);
            }
            else {
                invokeResult = await _smsSender.SendSmsAsync(_options.PasswordSmsTemplate, model.PhoneNumber, Tuple.Create("password", password));
            }

            if (!invokeResult.Succeeded) {
                var error = _errorDescriber.SendPasswordFailed();
                error.Details = new[] { invokeResult.Error };
                error.Throw();
            }

            return true;
        }

        /// <summary>
        /// Registers a new user asynchronous.
        /// </summary>
        /// <param name="model">The register model.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the register operation. Task result contains the register response.</returns>
        public async Task<IResponse> RegisterAsync(RegisterModel model) {
            var user = await _userManager.FindByNameAsync(model.UserName ?? model.PhoneNumber);
            if (user != null) {
                _errorDescriber.PhoneNumberHadBeenRegister(model.PhoneNumber).Throw();
            }

            if (!ValidateCode(model.Code, model.PhoneNumber)) {
                _errorDescriber.CodeInvalidOrTimeout(model.Code).Throw();
            }

            user = _userFactory.CreateUser(model.UserName ?? model.PhoneNumber);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) {
                var error = _errorDescriber.RegisterNewUserFailure(model.PhoneNumber, model.Code);
                error.Details = HandleErrors(result);
                error.Throw();
            }

            // set phone number.
            if (_userManager.SupportsUserPhoneNumber) {
                result = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!result.Succeeded) {
                    var error = _errorDescriber.RegisterNewUserFailure(model.PhoneNumber, model.Code);
                    error.Details = HandleErrors(result);
                    error.Throw();
                }
            }

            // sign in
            await _signInManager.SignInAsync(user, isPersistent: false);

            return await _accessTokenProvider.RequestTokenAsync(model.PhoneNumber, model.Password);
        }

        /// <summary>
        /// Logins with the specified model asynchronous.
        /// </summary>
        /// <param name="model">The login model.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the login operation.</returns>
        public async Task<IResponse> LoginAsync(LoginModel model) {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded) {
                if (result == SignInResult.LockedOut) {
                    _errorDescriber.UserLoginFailureBecauseIsLockout().Throw();
                }
                _errorDescriber.PhoneNumberOrPasswordError().Throw();
            }

            return await _accessTokenProvider.RequestTokenAsync(model.UserName, model.Password);
        }

        /// <summary>
        /// Sends the specified code asynchronous.
        /// </summary>
        /// <param name="model">The send code model.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the send operation.</returns>
        public async Task<bool> SendCodeAsync(SendCodeModel model) {
            string code;
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null) {
                code = GenerateCode(model.PhoneNumber);
            }
            else {
                code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            }

            InvokeResult result;
            if (string.IsNullOrWhiteSpace(_options.CodeSmsTemplate)) {
                result = await _smsSender.SendSmsAsync(model.PhoneNumber, code);
            }
            else {
                result = await _smsSender.SendSmsAsync(_options.CodeSmsTemplate, model.PhoneNumber, Tuple.Create("code", code));
            }

            if (!result.Succeeded) {
                var error = _errorDescriber.SendCodeFailure();
                error.Details = new[] { result.Error };
                error.Throw();
            }

            return true;
        }

        /// <summary>
        /// Verifies the specified code asynchronous.
        /// </summary>
        /// <param name="model">The verify code model.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the verify operation.</returns>
        public async Task<IResponse> VerifyCodeAsync(VerifyCodeModel model) {
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null) {
                if (!ValidateCode(model.Code, model.PhoneNumber)) {
                    _errorDescriber.CodeInvalidOrTimeout(model.Code).Throw();
                }

                var password = $"{GenericUtil.UniqueKey(3)}@{model.Code}";

                user = _userFactory.CreateUser(model.PhoneNumber);
                // why md5 here? because we should force APP or web to MD5 their plain password
                var result = await _userManager.CreateAsync(user, GenericUtil.EncryptMD5(password));
                if (!result.Succeeded) {
                    var error = _errorDescriber.RegisterNewUserFailure(model.PhoneNumber, model.Code);
                    error.Details = HandleErrors(result);
                    error.Throw();
                }

                // set phone number.
                if (_userManager.SupportsUserPhoneNumber) {
                    result = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                    if (!result.Succeeded) {
                        var error = _errorDescriber.RegisterNewUserFailure(model.PhoneNumber, model.Code);
                        error.Details = HandleErrors(result);
                        error.Throw();
                    }
                }

                InvokeResult invokeResult;
                if (string.IsNullOrWhiteSpace(_options.PasswordSmsTemplate)) {
                    invokeResult = await _smsSender.SendSmsAsync(model.PhoneNumber, password);
                }
                else {
                    invokeResult = await _smsSender.SendSmsAsync(_options.PasswordSmsTemplate, model.PhoneNumber, Tuple.Create("password", password));
                }
                if (!invokeResult.Succeeded) {
                    var error = _errorDescriber.SendPasswordFailed();
                    error.Details = new[] { invokeResult.Error };
                    error.Throw();
                }

                // sign in
                await _signInManager.SignInAsync(user, isPersistent: false);

                return await _accessTokenProvider.RequestTokenAsync(model.PhoneNumber, GenericUtil.EncryptMD5(password));
            }
            else {
                if (await _userManager.IsLockedOutAsync(user)) {
                    _errorDescriber.UserLoginFailureBecauseIsLockout().Throw();
                }

                if (!await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.Code, model.PhoneNumber)) {
                    _errorDescriber.CodeInvalidOrTimeout(model.Code).Throw();
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                var token = await _accessTokenProvider.RequestTokenAsync(model.PhoneNumber, model.Code);
                if (token.IsError) {
                    _errorDescriber.PhoneNumberOrPasswordError().Throw();
                }

                return token;
            }
        }

        /// <summary>
        /// Changes the password for the specified user asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the change operation.</returns>
        public async Task<bool> ChangePasswordAsync(ChangePasswordModel model) {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) {
                _errorDescriber.UserNotFoundByPhoneNumber(model.UserName).Throw();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded) {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }

            HandleErrors(result);

            return false;
        }

        /// <summary>
        /// Resets the password for specified user asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the reset operation.</returns>
        public async Task<IResponse> ResetPasswordAsync(ResetPasswordModel model) {
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null) {
                _errorDescriber.UserNotFoundByPhoneNumber(model.PhoneNumber).Throw();
            }

            if (!await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.Code, model.PhoneNumber)) {
                _errorDescriber.CodeInvalidOrTimeout(model.Code).Throw();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
            if (!result.Succeeded) {
                var error = _errorDescriber.ResetPasswordFailure();
                error.Details = HandleErrors(result);
                error.Throw();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return await _accessTokenProvider.RequestTokenAsync(model.PhoneNumber, model.Password);
        }

        /// <summary>
        /// Updates the specified user asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the reset operation.</returns>
        public async Task<bool> UpdateAsync(UpdateUserClaimsModel model) {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null) {
                _errorDescriber.UserNotFoundById(model.Id.ToString()).Throw();
            }

            var claims = model.ToClaims();
            // remove
            var result = await _userManager.RemoveClaimsAsync(user, claims);
            // add again
            result = await _userManager.AddClaimsAsync(user, model.ToClaims());
            if (!result.Succeeded) {
                var error = _errorDescriber.UserUpdateFailure();
                error.Details = HandleErrors(result);
                error.Throw();
            }

            return true;
        }

        /// <summary>
        /// Binds the open ID for the specified user asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the bind operation.</returns>
        public async Task<bool> BindAsync(OpenIdBindingModel model) {
            var user = await _userManager.FindByLoginAsync(WChatLoginProvider, model.OpenId);
            if (user != null) {
                _logger.LogWarning($"Phone Number {model.PhoneNumber} had bind open id {model.OpenId}");

                // find by phone number
                var byPhone = await _userManager.FindByNameAsync(model.PhoneNumber);

                var userId1 = await _userManager.GetUserIdAsync(user);
                var userId2 = await _userManager.GetUserIdAsync(byPhone);
                if (userId1 == userId2) {
                    return true;
                }
            }

            user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null) {
                _errorDescriber.UserNotFoundByPhoneNumber(model.PhoneNumber).Throw();
            }
            var info = new UserLoginInfo(WChatLoginProvider, model.OpenId, "weixin");
            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded) {
                _logger.LogError(result.ToString());
                return false;
            }

            // add a claim for this user.
            result = await _userManager.AddClaimAsync(user, new Claim(OAuthClaimTypes.WChat, model.OpenId));
            if (!result.Succeeded) {
                _logger.LogError($"User: {model.PhoneNumber} add claim error: {result.ToString()}");
            }

            return true;
        }

        /// <summary>
        /// Logins with the specified model asynchronous.
        /// </summary>
        /// <param name="model">The login model.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the login operation.</returns>
        public async Task<IResponse> LoginAsync(OpenIdLoginModel model) {
            var user = await _userManager.FindByLoginAsync(WChatLoginProvider, model.OpenId);
            if (user == null) {
                new InvokeError {
                    Code = "OpenIdLogin",
                    Message = "cannot login with weixin open id."
                }.Throw();
            }

            // cannot login if the user had been lockedout
            if (await _userManager.IsLockedOutAsync(user)) {
                _errorDescriber.UserLoginFailureBecauseIsLockout().Throw();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            var response = await _accessTokenProvider.RequestTokenAsync(userName, code);
            if (response.IsError) {
                new InvokeError {
                    Code = "OpenIdLogin",
                    Message = response.Error
                }.Throw();
            }

            return response;
        }

        internal string GenerateCode(string phoneNumber) {
            var securityStamp = Encoding.Unicode.GetBytes(DefaultSecurityStamp);
            return Rfc6238Service.GenerateCode(securityStamp, phoneNumber).ToString(CultureInfo.InvariantCulture);
        }

        internal bool ValidateCode(string token, string phoneNumber) {
            var securityStamp = Encoding.Unicode.GetBytes(DefaultSecurityStamp);
            int code;
            if (securityStamp != null && int.TryParse(token, out code)) {
                if (Rfc6238Service.ValidateCode(securityStamp, code, phoneNumber)) {
                    return true;
                }
            }
            _logger.LogWarning(8, $"ValidateCode() failed for phone {phoneNumber}.");
            return false;
        }

        private IEnumerable<InvokeError> HandleErrors(IdentityResult result, string throwMsg = null) {
            var list = new List<InvokeError>();
            foreach (var error in result.Errors) {
                list.Add(new InvokeError {
                    Code = error.Code,
                    Message = error.Description
                });
            }

            return list;
        }
    }
}
