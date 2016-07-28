namespace RigoFunc.Account.Models {
    /// <summary>
    /// Represents the view model/DTO to change password.
    /// </summary>
    public class ChangePasswordModel {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        /// <value>The old password.</value>
        /// <remarks>
        /// For security reason, the app/web MUST MD5 the plain text password.
        /// </remarks>
        public string OldPassword { get; set; }
        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>The new password.</value>
        /// <remarks>
        /// For security reason, the app/web MUST MD5 the plain text password.
        /// </remarks>
        public string NewPassword { get; set; }
    }
}
