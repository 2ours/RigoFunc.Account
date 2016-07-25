using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RigoFunc.Account.Services {
    public class IdentityServerAccessTokenProvider : IAccessTokenProvider {
        private readonly ILogger _logger;
        private readonly HttpContext _httpContext;
        private readonly ApiOptions _options;

        public IdentityServerAccessTokenProvider(IHttpContextAccessor contextAccessor,
 IOptions<ApiOptions> options,
 ILoggerFactory loggerFactory) {
            _httpContext = contextAccessor.HttpContext;
            _logger = loggerFactory.CreateLogger(nameof(IdentityServerAccessTokenProvider));
            _options = options.Value;
        }

        public virtual async Task<IResponse> RequestTokenAsync(string userName, string password) {
            var endpoint = $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host.Value}/connect/token";

            _logger.LogInformation($"token_endpoint: {endpoint}");

            string clientId = _httpContext.Request.Headers[ApiConstants.ClientId];
            if (string.IsNullOrWhiteSpace(clientId)) {
                clientId = _options.DefaultClientId;
            }
            string clientSecret = _httpContext.Request.Headers[ApiConstants.ClientSecret];
            if (string.IsNullOrWhiteSpace(clientSecret)) {
                clientSecret = _options.DefaultClientSecret;
            }
            string scope = _httpContext.Request.Headers[ApiConstants.Scope];
            if (string.IsNullOrWhiteSpace(scope)) {
                scope = _options.DefaultScope;
            }

            var client = new TokenClient(endpoint, clientId, clientSecret);
            var response = await client.RequestResourceOwnerPasswordAsync(userName, password, scope);

            return ApiResponse.FromTokenResponse(response);
        }
    }
}