using RigoFunc.OAuth;

namespace RigoFunc.Account.Models {
    public class UpdateUserClaimsModel : OAuthUser {
        /// <summary>
        /// 用户编号
        /// </summary>
        /// <remarks>
        /// For fixing readonly Id cannot deserialize issues
        /// </remarks>
        public new int Id { get; set; }
    }
}
