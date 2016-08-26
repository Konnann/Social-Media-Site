using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ATHNN.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
           : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<ATHNN.Models.Post> Posts { get; set; }

        public System.Data.Entity.DbSet<ATHNN.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<ATHNN.Models.Tag> Tags { get; set; }
    }
}