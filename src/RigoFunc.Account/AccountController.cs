using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Love.Net.Core;
using RigoFunc.Account.Models;
using RigoFunc.Account.Services;

namespace RigoFunc.Account {
    [Route("api/[controller]")]
    public class AccountController {
        private readonly IAccountService _service;
        private readonly InvokeErrorDescriber _errorDescriber;
        
        public AccountController(IAccountService service, InvokeErrorDescriber describer = null) {
            _service = service;
            _errorDescriber = describer ?? new InvokeErrorDescriber();
        }

        [HttpGet]
        public async Task<User> Get([FromQuery]FindUserModel model) {
            if (model == null || (model.Id == null && string.IsNullOrEmpty(model.PhoneNumber))) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.GetAsync(model);
        }

        [HttpPost("[action]")]
        public async Task<bool> Lockout([FromBody]LockoutModel model) {
            if (model == null) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.LockoutAsync(model);
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<bool> Create([FromBody]RegisterModel model) {
            if (model == null) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.CreateAsync(model);
        }

        [HttpPost("[action]")]
        public async Task<IResponse> Register([FromBody]RegisterModel model) {
            if (model == null) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.RegisterAsync(model);
        }

        [HttpPost("[action]")]
        public async Task<bool> SendCode([FromBody]SendCodeModel model) {
            if (model == null) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.SendCodeAsync(model);
        }

        [HttpPost("[action]")]
        public async Task<IResponse> Login([FromBody]LoginModel model) {
            if (model == null) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.LoginAsync(model);
        }

        [HttpPost("[action]")]
        public async Task<IResponse> VerifyCode([FromBody]VerifyCodeModel model) {
            if (model == null) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.VerifyCodeAsync(model);
        }

        [HttpPost("[action]")]
        public async Task<bool> ChangePassword([FromBody]ChangePasswordModel model) {
            if (model == null) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.ChangePasswordAsync(model);
        }

        [HttpPost("[action]")]
        public async Task<IResponse> ResetPassword([FromBody]ResetPasswordModel model) {
            if (model == null) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.ResetPasswordAsync(model);
        }

        [HttpPost("[action]")]
        public async Task<bool> Update([FromBody]User user) {
            if (user == null) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.UpdateAsync(user);
        }
    }
}
