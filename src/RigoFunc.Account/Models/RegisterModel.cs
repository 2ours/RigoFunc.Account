namespace RigoFunc.Account.Models {
    /// <summary>
    /// Represents the DTO/ViewModel to register a new user
    /// </summary>
    public class RegisterModel {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }
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
