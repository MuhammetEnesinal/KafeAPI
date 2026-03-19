using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Persistence.Context.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppIdentityUser,AppIdentityRole,string>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppIdentityUser>(entity =>
            {
                entity.ToTable("Users");
            });
            builder.Entity<AppIdentityRole>(entity =>
            {
                entity.ToTable("Roles");
            });
        }




    }
}
