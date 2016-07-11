using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Host;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using RigoFunc.Utils;
using Xunit;
using Xunit.Abstractions;

namespace RigoFunc.Account.UnitTests {
    public class AccountControllerTests {
        private readonly ITestOutputHelper output;
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public AccountControllerTests(ITestOutputHelper output) {
            this.output = output;
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        private const string DefaultSecurityStamp = "022a9e42-9509-4aa6-8a0a-c34a1f405c61";
        byte[] securityStamp = Encoding.Unicode.GetBytes(DefaultSecurityStamp);

        //[Fact]
        public async Task RegisterTest() {
            var code = Rfc6238Service.GenerateCode(securityStamp, "18121629620");
            var value = new {
                username = "lotosbin@gmail.com",
                password = "Welcome123$",
                phonenumber = "18121629620",
                code = code
            };
            var responseMessage = await _client.PostAsJsonAsync("/api/account/register", value);
            var rootobject = await responseMessage.Content.ReadAsAsync<Rootobject>();
            Assert.NotNull(rootobject);
            Assert.NotNull(rootobject.accessToken);
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
