using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RigoFunc.Account.UnitTests.Supports {
    public class ApplicationUser : IdentityUser<int> {
        public ApplicationUser(string userName) {
            this.UserName = userName;
        }
    }
}
