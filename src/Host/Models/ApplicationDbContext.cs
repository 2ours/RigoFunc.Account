using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Host.Models {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext() {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {

        }
    }
}
