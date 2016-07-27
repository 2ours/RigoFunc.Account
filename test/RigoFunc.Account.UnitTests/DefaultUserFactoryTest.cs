using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RigoFunc.Account.Default;
using RigoFunc.Account.Services;
using Xunit;

namespace RigoFunc.Account.UnitTests {
    public class UsePropertyCreateUser {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UseCtorCreateUser : UsePropertyCreateUser {
        public UseCtorCreateUser(string userName) {
            CtorUsed = true;
            UserName = userName;
        }

        public bool CtorUsed { get; set; }
    }

    public class ThrowExceptionUser {
        public ThrowExceptionUser() {
            UserName = "privateSet";
        }

        public string UserName { get; }
    }

    public class DefaultUserFactoryTest {
        [Fact]
        public void CreateUser_UsePropery_Test() {
            var user = new DefaultUserFactory<UsePropertyCreateUser>().CreateUser("xUnit");

            Assert.NotNull(user);
            Assert.Equal("xUnit", user.UserName);
        }

        [Fact]
        public void CreateUser_UseCtor_Test() {
            var user = new DefaultUserFactory<UseCtorCreateUser>().CreateUser("xUnit");

            Assert.NotNull(user);
            Assert.Equal("xUnit", user.UserName);
            Assert.True(user.CtorUsed);
        }

        [Fact]
        public void Throw_Exception_Test() {
            Assert.Throws<NotSupportedException>(() => { new DefaultUserFactory<ThrowExceptionUser>().CreateUser("xUnit"); });
        }
    }
}
