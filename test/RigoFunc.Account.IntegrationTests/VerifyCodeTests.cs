using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Host;
using Host.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Love.Net.Core;

namespace RigoFunc.Account.IntegrationTests {
    public class VerifyCodeTests {
        private TestServer _server;
        private HttpClient _client;
        private Sender _sender;

        public VerifyCodeTests() {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
            _sender = _server.Host.Services.GetService<ISmsSender>() as Sender;
        }
        [Fact]
        public async Task SendCode_VerifyCode_Test() {
            var response = await _client.PostAsJsonAsync("api/Account/SendCode", new { phonenumber = "18121629620" });
            response.EnsureSuccessStatusCode();
            var s = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"code:{_sender.Code}");
            var responseMessage = await _client.PostAsJsonAsync("api/account/verifycode", new { phonenumber = "18121629620", code = _sender.Code });
            responseMessage.EnsureSuccessStatusCode();
            var r = await responseMessage.Content.ReadAsAsync<Rootobject>();
            Assert.NotNull(r);
            Assert.NotNull(r.accessToken);
        }
        public class Rootobject {
            public string accessToken { get; set; }
            public int expiresIn { get; set; }
            public string raw { get; set; }
            public string refreshToken { get; set; }
            public string tokenType { get; set; }
            public bool isError { get; set; }
            public string error { get; set; }
        }

    }
}
