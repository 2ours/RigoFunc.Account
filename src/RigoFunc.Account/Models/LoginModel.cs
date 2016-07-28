namespace RigoFunc.Account.Models {
    /// <summary>
    /// Represents the view model/DTO to login.
    /// </summary>
    public class LoginModel {
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
        /// Gets or sets a value indicating whether [remember me].
        /// </summary>
        /// <value><c>true</c> if [remember me]; otherwise, <c>false</c>.</value>
        public bool RememberMe { get; set; }
    }
}
