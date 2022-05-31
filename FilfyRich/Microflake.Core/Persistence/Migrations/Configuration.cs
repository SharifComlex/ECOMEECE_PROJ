using Microflake.Core.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Microflake.Core.Persistence.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {

            if (!context.Roles.Any(r => r.Name == "SuperAdmin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "SuperAdmin" };

                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "User"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "User" };

                manager.Create(role);
            }
            if (!context.Currencies.Any(r => r.Name == "Saudi Ryal"))
            {
                var entity = new Currency();
                entity.Name = "Saudi Ryal";
                entity.Simbal = "SAR";
                entity.Currency_Rate = 1;
                context.Currencies.Add(entity);
                context.SaveChanges();
            }
            if (!context.Currencies.Any(r => r.Name == "Euro"))
            {
                var entity = new Currency();
                entity.Name = "Euro";
                entity.Simbal = "€";
                entity.Currency_Rate = 1;
                context.Currencies.Add(entity);
                context.SaveChanges();
            }
            if (!context.Currencies.Any(r => r.Name == "Canadian Dollar"))
            {
                var entity = new Currency();
                entity.Name = "Canadian Dollar";
                entity.Simbal = "CAD";
                entity.Currency_Rate = 1;
                context.Currencies.Add(entity);
                context.SaveChanges();
            }
            if (!context.Currencies.Any(r => r.Name == "US Dollar"))
            {
                var entity = new Currency();
                entity.Name = "US Dollar";
                entity.Simbal = "USD";
                entity.Currency_Rate = 1;
                context.Currencies.Add(entity);
                context.SaveChanges();
            }
            if (!context.Currencies.Any(r => r.Name == "Pound"))
            {
                var entity = new Currency();
                entity.Name = "Pound";
                entity.Simbal = "£";
                entity.Currency_Rate = 1;
                context.Currencies.Add(entity);
                context.SaveChanges();
            }
            if (!context.Currencies.Any(r => r.Name == "Pakistani Rupee"))
            {
                var entity = new Currency();
                entity.Name = "Pakistani Rupee";
                entity.Simbal = "PKR";
                entity.Base_Currency = true;
                entity.Currency_Rate = 1;
                context.Currencies.Add(entity);
                context.SaveChanges();
            }
            if (!context.Users.Any(u => u.UserName == "admin@ecap.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "admin@ecap.com", Email = "admin@ecap.com", Status = true };

                manager.Create(user, "Tms.123456");
                manager.AddToRole(user.Id, "SuperAdmin");
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
