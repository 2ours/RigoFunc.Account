using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RigoFunc.Account.Services;
using RigoFunc.Account.UnitTests.Supports;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Love.Net.Core;

namespace RigoFunc.Account.UnitTests.Services {
    public class DefaultAccountServiceTests {
        private DefaultAccountService<ApplicationUser> target;
        private readonly ITestOutputHelper _output;

        public DefaultAccountServiceTests(ITestOutputHelper output) {
            this._output = output;
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=unittest;Trusted_Connection=True;MultipleActiveResultSets=true"));

            services.AddIdentity<ApplicationUser, IdentityRole<int>>(options => {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext, int>()
            .AddDefaultTokenProviders();
            services.UseDefaultAccountService<ApplicationUser>();
            services.AddTransient<IEmailSender, Sender>();
            services.AddTransient<ISmsSender, Sender>();
            var serviceProvider = services.BuildServiceProvider();
            target = (DefaultAccountService<ApplicationUser>)serviceProvider.GetService<IAccountService>();
        }
        string phoneNumber = "18121629620";
        //[Fact]
        public void GenerateCodeTest() {
            var code = target.GenerateCode(phoneNumber);
            _output.WriteLine(code);
            Assert.True(false);
        }
        //[Fact]
        public void ValidateCodeTest() {
            var code = "805486";
            var validateCode = target.ValidateCode(code, phoneNumber);
            Assert.True(validateCode);
        }
    }
}
