namespace Talento.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using Talento.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Talento.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            CommandTimeout = 10000;
        }

        protected override void Seed(Talento.Models.ApplicationDbContext context)
        {
            var roles = new List<ApplicationRole>
            {
                new ApplicationRole{ Name = "Admin"},
                new ApplicationRole{ Name = "PM"},
                new ApplicationRole{ Name = "RMG"},
                new ApplicationRole{ Name = "TAG"},
                new ApplicationRole{ Name = "TL"}
            };
            roles.ForEach(r => context.Roles.AddOrUpdate(p => p.Name, r));
            context.SaveChanges();

            if (!context.Users.Any(u => u.UserName == "Admin@example.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var passwordHash = new PasswordHasher();
                string password = passwordHash.HashPassword("Admin@123456");

                var user = new ApplicationUser
                {
                    UserName = "Admin@example.com",
                    Email = "Admin@example.com",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult resultCreate = manager.Create(user);
                if (resultCreate.Succeeded == false)
                {
                    throw new Exception(resultCreate.Errors.First());
                }

                IdentityResult resultAddToRole = manager.AddToRole(user.Id, "Admin");
                if (resultAddToRole.Succeeded == false)
                {
                    throw new Exception(resultAddToRole.Errors.First());
                }
            }

            context.SaveChanges();
        }
    }
}
