
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace elraee.Models
{
    public class context : IdentityDbContext<applicationUser, IdentityRole<int>, int>

    {
        public context() : base()
        {
        }

        public context(DbContextOptions options) : base(options)
        {

        }


        public DbSet<applicationUser> applicationUsers { get; set; }
        public DbSet<category> categories { get; set; }
        public DbSet<product> products { get; set; }
        public DbSet<imageAsString> images { get; set; }
        public DbSet<ContactInfo> contactInfo { get; set; }

        public DbSet<phoneNums> phoneNums { get; set; }

        public DbSet<relativeProducts> relativeProducts { get; set; }



    }
}
