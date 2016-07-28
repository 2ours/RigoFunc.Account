namespace RigoFunc.Account.Models {
    /// <summary>
    /// Represents the view model/DTO to find/search user.
    /// </summary>
    public class FindUserModel {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int? Id { get; set; }
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number.</value>
        public string PhoneNumber { get; set; }
    }
}
