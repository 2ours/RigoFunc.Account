using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Host.Models {
    public class ApplicationUser : IdentityUser {
        public ApplicationUser() { }

        public ApplicationUser(string userName) : this() {
            UserName = userName;
        }
    }
}
