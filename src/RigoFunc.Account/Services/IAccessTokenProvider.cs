using System.Threading.Tasks;

namespace RigoFunc.Account.Services {
    public interface IAccessTokenProvider {
        Task<IResponse> RequestTokenAsync(string userName, string password);
    }
}
