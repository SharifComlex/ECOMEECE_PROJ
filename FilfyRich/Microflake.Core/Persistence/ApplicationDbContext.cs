using Microflake.Core.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Microflake.Core.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext():base(Config.ConnectionString)
        {

        }
        
      public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Whislist> Whislists { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ContactUs> ContactUss { get; set; }
        
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<OrderDetals> OrderDetals { get; set; }

        public DbSet<CustomCategory> CustomCategories { get; set; }
        public DbSet<CustomColor> CustomColors { get; set; }
        public DbSet<CustomItem> CustomItems { get; set; }
        public DbSet<CustomVariation> CustomVariations { get; set; }


        public DbSet<Product> Products { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
