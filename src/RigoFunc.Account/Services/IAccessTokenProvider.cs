using System.Threading.Tasks;

namespace RigoFunc.Account.Services {
    /// <summary>
    /// Provides the interface for providing token.
    /// </summary>
    public interface IAccessTokenProvider {
        /// <summary>
        /// Requests the token asynchronous.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>A <see cref="Task"/> represents the asynchronous operation, containing the <see cref="IResponse"/>.</returns>
        Task<IResponse> RequestTokenAsync(string userName, string password);
    }
}
