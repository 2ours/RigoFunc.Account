using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Host;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace RigoFunc.Account.IntergrationTests {
    public class VerifyCodeTests {
        private TestServer _server;
        private HttpClient _client;

        public VerifyCodeTests() {
            _server = new TestServer(new WebHostBuilder()
    .UseStartup<Startup>());
            _client = _server.CreateClient();

        }
        [Fact]
        public async Task SendCode_VerifyCode_Test() {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.PostAsJsonAsync("api/Account/SendCode", new { phonenumber = "18121629620" });
            response.EnsureSuccessStatusCode();
            var s = await response.Content.ReadAsStringAsync();

            var responseMessage = await _client.PostAsJsonAsync("api/account/verifycode", new { phonenumber = "18121629620", code = "123456" });
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
