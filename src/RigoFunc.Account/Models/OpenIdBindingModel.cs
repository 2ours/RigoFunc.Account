namespace RigoFunc.Account.Models {
    /// <summary>
    /// Represents the DTO/ViewModel to binding the third-party open Id.
    /// </summary>
    public class OpenIdBindingModel {
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number.</value>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the open identifier.
        /// </summary>
        /// <value>The open identifier.</value>
        public string OpenId { get; set; }
    }
}
