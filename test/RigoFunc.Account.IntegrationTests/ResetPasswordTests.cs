using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Host;
using Host.Models;
using Love.Net.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RigoFunc.Account.Models;
using RigoFunc.Account.Services;
using Xunit.Abstractions;

namespace RigoFunc.Account.IntegrationTests {
    public class ResetPasswordTests {
        private TestServer _server;
        private HttpClient _client;
        private Sender _sender;
        private readonly ITestOutputHelper _output;

        public ResetPasswordTests(ITestOutputHelper output) {
            _output = output;
            _server = new TestServer(new WebHostBuilder()
    .UseStartup<Startup>());
            _client = _server.CreateClient();
            _sender = _server.Host.Services.GetService<ISmsSender>() as Sender;
        }

        [Fact]
        public async Task SendCode_ResetPassword_Test() {
            //prepare
            {
                var response = await _client.PostAsJsonAsync("api/Account/SendCode", new { phonenumber = "18121629620" });
                response.EnsureSuccessStatusCode();
                var s = await response.Content.ReadAsStringAsync();
                _output.WriteLine($"code:{_sender.Code}");

                var responseMessage = await _client.PostAsJsonAsync("api/account/register", new RegisterModel() {
                    UserName = "18121629620",
                    PhoneNumber = "18121629620",
                    Code = _sender.Code,
                    Password = "Welcome123$"
                });
                responseMessage.EnsureSuccessStatusCode();
                var s1 = await responseMessage.Content.ReadAsStringAsync();
                _output.WriteLine(s1);
                var r = JsonConvert.DeserializeObject<Rootobject>(s1);

                Assert.NotNull(r);
                Assert.NotNull(r.accessToken);

            }

            //
            {

                var response = await _client.PostAsJsonAsync("api/Account/SendCode", new { phonenumber = "18121629620" });
                response.EnsureSuccessStatusCode();
                var s = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"code:{_sender.Code}");

                var responseMessage = await _client.PostAsJsonAsync("api/account/resetpassword", new { phonenumber = "18121629620", code = _sender.Code, password = "Welcome123$" });
                responseMessage.EnsureSuccessStatusCode();
                var r = await responseMessage.Content.ReadAsAsync<VerifyCodeTests.Rootobject>();

                Assert.NotNull(r);
                Assert.NotNull(r.accessToken);
            }
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
