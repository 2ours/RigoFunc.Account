namespace RigoFunc.Account.Models {
    /// <summary>
    /// Represents the DTO/ViewModel to reset the password.
    /// </summary>
    public class ResetPasswordModel {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks>
        /// For security reason, the app/web MUST MD5 the plain text password.
        /// </remarks>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number.</value>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }
    }
}
