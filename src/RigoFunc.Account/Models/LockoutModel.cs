using System;

namespace RigoFunc.Account.Models {
    public class LockoutModel {
        /// <summary>
        /// Gets or sets the user name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets a flag indicating whether the user is locked out or not.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="DateTimeOffset"/> after which the user lockout should end.
        /// </summary>
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
