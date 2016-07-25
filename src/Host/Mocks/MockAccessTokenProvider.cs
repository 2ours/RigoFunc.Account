using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RigoFunc.Account;
using RigoFunc.Account.Services;

namespace Host.Mocks {
    public class MockAccessTokenProvider : AccessTokenProvider {
        public MockAccessTokenProvider(IHttpContextAccessor contextAccessor, IOptions<ApiOptions> options, ILoggerFactory loggerFactory) : base(contextAccessor, options, loggerFactory) {
        }
        public override Task<IResponse> RequestTokenAsync(string userName, string password) {
            TokenResponse response = new TokenResponse(JsonConvert.SerializeObject(new { access_token = "test" })) {
            };
            return Task.FromResult(ApiResponse.FromTokenResponse(response));
        }

    }
}
