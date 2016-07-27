using System.Net.Http;
using System.Threading.Tasks;
using Host;
using Host.Models;
using Love.Net.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RigoFunc.Account.Models;
using Xunit;
using Xunit.Abstractions;

namespace RigoFunc.Account.IntegrationTests {
    public class LoginTests {
        private readonly ITestOutputHelper _output;
        private TestServer _server;
        private HttpClient _client;
        private Sender _sender;
        public LoginTests(ITestOutputHelper output) {
            _output = output;
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
            _sender = _server.Host.Services.GetService<ISmsSender>() as Sender;
        }
        [Fact]
        public async Task Login_Test() {
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
                var r = JsonConvert.DeserializeObject<RegisterTests.Rootobject>(s1);

                Assert.NotNull(r);
                Assert.NotNull(r.accessToken);
            }
            //main
            {
                var responseMessage = await _client.PostAsJsonAsync("api/account/login", new RegisterModel() {
                    UserName = "18121629620",
                    Password = "Welcome123$"
                });
                responseMessage.EnsureSuccessStatusCode();
                var s1 = await responseMessage.Content.ReadAsStringAsync();
                _output.WriteLine(s1);
                var r = JsonConvert.DeserializeObject<RegisterTests.Rootobject>(s1);

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
