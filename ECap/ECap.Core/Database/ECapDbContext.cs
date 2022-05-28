using ECap.Core.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECap.Core.Database
{
    public class ECapDbContext : IdentityDbContext<AdminUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Config.Database.ConnectionString);
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
