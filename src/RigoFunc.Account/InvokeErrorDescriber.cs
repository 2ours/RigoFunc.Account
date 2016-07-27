using Love.Net.Core;

namespace RigoFunc.Account {
    public class InvokeErrorDescriber {
        /// <summary>
        /// Returns the default <see cref="InvokeError"/>.
        /// </summary>
        /// <returns>The default <see cref="InvokeError"/>,</returns>
        public virtual InvokeError DefaultError() => new InvokeError {
            Code = nameof(DefaultError),
            Message = Resources.DefaultError
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the bad argument.
        /// </summary>
        /// <returns>An <see cref="InvokeError"/> indicating the bad argument.</returns>
        public virtual InvokeError BadArgument() => new InvokeError {
            Code = nameof(BadArgument),
            Message = Resources.BadArgument
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the code is invalid or timeout.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>An <see cref="InvokeError"/> indicating the cod is invalid or timeout.</returns>
        public virtual InvokeError CodeInvalidOrTimeout(string code) => new InvokeError {
            Code = nameof(CodeInvalidOrTimeout),
            Message = string.Format(Resources.CodeInvalidOrTimeout, code)
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the phone number had been register.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns>An <see cref="InvokeError"/> indicating the phone number had been register.</returns>
        public virtual InvokeError PhoneNumberHadBeenRegister(string phoneNumber) => new InvokeError {
            Code = nameof(PhoneNumberHadBeenRegister),
            Message = string.Format(Resources.PhoneNumberHadBeenRegister, phoneNumber)
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the phone number or the password error.
        /// </summary>
        /// <returns>An <see cref="InvokeError"/> indicating the phone number or the password error.</returns>
        public virtual InvokeError PhoneNumberOrPasswordError() => new InvokeError {
            Code = nameof(PhoneNumberOrPasswordError),
            Message = Resources.PhoneNumberOrPasswordError
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the phone number had been register.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="code">The code.</param>
        /// <returns>An <see cref="InvokeError"/> indicating the phone number had been register.</returns>
        public virtual InvokeError RegisterNewUserFailure(string phoneNumber, string code) => new InvokeError {
            Code = nameof(RegisterNewUserFailure),
            Message = string.Format(Resources.RegisterNewUserFailure, phoneNumber, code)
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the reset password failure.
        /// </summary>
        /// <returns>An <see cref="InvokeError"/> indicating the reset password failure.</returns>
        public virtual InvokeError ResetPasswordFailure() => new InvokeError {
            Code = nameof(ResetPasswordFailure),
            Message = Resources.ResetPasswordFailure
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the send code failure.
        /// </summary>
        /// <returns>An <see cref="InvokeError"/> indicating the send code failure.</returns>
        public virtual InvokeError SendCodeFailure() => new InvokeError {
            Code = nameof(SendCodeFailure),
            Message = Resources.SendCodeFailure
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the send initial password failure.
        /// </summary>
        /// <returns>An <see cref="InvokeError"/> indicating the send initial password failure.</returns>
        public virtual InvokeError SendPasswordFailed() => new InvokeError {
            Code = nameof(SendPasswordFailed),
            Message = Resources.SendPasswordFailed
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the user lock failure.
        /// </summary>
        /// <returns>An <see cref="InvokeError"/> indicating the user lock failure.</returns>
        public virtual InvokeError UserLockoutFailure() => new InvokeError {
            Code = nameof(UserLockoutFailure),
            Message = Resources.UserLockoutFailure
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the user login failure because is lockout.
        /// </summary>
        /// <returns>An <see cref="InvokeError"/> indicating the user login failure because is lockout.</returns>
        public virtual InvokeError UserLoginFailureBecauseIsLockout() => new InvokeError {
            Code = nameof(UserLoginFailureBecauseIsLockout),
            Message = Resources.UserLoginFailureBecauseIsLockout
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the phone number had been register.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="code">The code.</param>
        /// <returns>An <see cref="InvokeError"/> indicating the phone number had been register.</returns>
        public virtual InvokeError UserLoginFailureWithPhoneAndCode(string phoneNumber, string code) => new InvokeError {
            Code = nameof(UserLoginFailureWithPhoneAndCode),
            Message = string.Format(Resources.UserLoginFailureWithPhoneAndCode, phoneNumber, code)
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the user not found by the specified Id.
        /// </summary>
        /// <param name="id">The user Id.</param>
        /// <returns>An <see cref="InvokeError"/> indicating the user not found by the specified Id.</returns>
        public virtual InvokeError UserNotFoundById(string id) => new InvokeError {
            Code = nameof(UserNotFoundById),
            Message = string.Format(Resources.UserNotFoundById, id)
        };


        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating user not found by the specified phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns>An <see cref="InvokeError"/> indicating the user not found by the specified phone number.</returns>
        public virtual InvokeError UserNotFoundByPhoneNumber(string phoneNumber) => new InvokeError {
            Code = nameof(UserNotFoundByPhoneNumber),
            Message = string.Format(Resources.UserNotFoundByPhoneNumber, phoneNumber)
        };

        /// <summary>
        /// Returns an <see cref="InvokeError"/> indicating the user update failure.
        /// </summary>
        /// <returns>An <see cref="InvokeError"/> indicating the user update failure.</returns>
        public virtual InvokeError UserUpdateFailure() => new InvokeError {
            Code = nameof(UserUpdateFailure),
            Message = Resources.UserUpdateFailure
        };
    }
}
