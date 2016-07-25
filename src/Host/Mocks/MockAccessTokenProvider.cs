using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using RigoFunc.Account;
using RigoFunc.Account.Services;

namespace Host.Mocks {
    public class MockAccessTokenProvider : IAccessTokenProvider {
        public Task<IResponse> RequestTokenAsync(string userName, string password) {
            TokenResponse response = new TokenResponse(JsonConvert.SerializeObject(new { access_token = "test" }));
            return Task.FromResult(ApiResponse.FromTokenResponse(response));
        }
    }
}
