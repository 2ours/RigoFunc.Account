namespace RigoFunc.Account.Models {
    /// <summary>
    /// Represents the DTO/ViewModel to verify the code.
    /// </summary>
    public class VerifyCodeModel {
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
