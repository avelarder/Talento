namespace Talento.Core.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Talento.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<Talento.Core.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Talento.Core.Data.ApplicationDbContext";
        }

        protected override void Seed(Talento.Core.Data.ApplicationDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);

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

            #region Admin User
            if (!context.Users.Any(u => u.UserName == "Admin@example.com"))
            {
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
            #endregion            

            #region ApplicationSettings

            var appSettings = new List<ApplicationSetting>
            {
                new ApplicationSetting {
                    ApplicationSettingId = 1,
                    SettingName = "Pagination",
                    ParameterName = "Status",
                    ParameterValue = "Enabled",
                    CreationDate = DateTime.Now,
                    CreatedBy = manager.FindByEmail("Admin@example.com")
                },
                new ApplicationSetting {
                    ApplicationSettingId = 2,
                    SettingName = "Pagination",
                    ParameterName = "PageSize",
                    ParameterValue = "10",
                    CreationDate = DateTime.Now.AddMinutes(-5),
                    CreatedBy = manager.FindByEmail("Admin@example.com")
                },
                new ApplicationSetting {
                    ApplicationSettingId = 3,
                    SettingName = "Pagination",
                    ParameterName = "PageSize1",
                    ParameterValue = "10",
                    CreationDate = DateTime.Now.AddMinutes(-7),
                    CreatedBy = manager.FindByEmail("Admin@example.com")
                },
                new ApplicationSetting {
                    ApplicationSettingId = 4,
                    SettingName = "Sorting",
                    ParameterName = "Status",
                    ParameterValue = "Enabled",
                    CreationDate = DateTime.Now.AddHours(-1),
                    CreatedBy = manager.FindByEmail("Admin@example.com")
                },

                new ApplicationSetting {
                    ApplicationSettingId = 5,
                    SettingName = "Sorting",
                    ParameterName = "SortBy",
                    ParameterValue = "CreationDate",
                    CreationDate = DateTime.Now.AddHours(-1).AddMinutes(-5),
                    CreatedBy = manager.FindByEmail("Admin@example.com")
                },
                new ApplicationSetting {
                    ApplicationSettingId = 6,
                    SettingName = "Filtering",
                    ParameterName = "DefaultSort",
                    ParameterValue = "DESC",
                    CreationDate = DateTime.Now.AddHours(-1).AddMinutes(-20),
                    CreatedBy = manager.FindByEmail("Admin@example.com")
                },
                new ApplicationSetting {
                    ApplicationSettingId = 7,
                    SettingName = "Filtering",
                    ParameterName = "Status",
                    ParameterValue = "Enabled",
                    CreationDate = DateTime.Now.AddHours(-2),
                    CreatedBy = manager.FindByEmail("Admin@example.com")
                },
                new ApplicationSetting {
                    ApplicationSettingId = 8,
                    SettingName = "Filtering",
                    ParameterName = "DefaultFilter",
                    ParameterValue = "All",
                    CreationDate = DateTime.Now.AddHours(-2).AddMinutes(-5),
                    CreatedBy = manager.FindByEmail("Admin@example.com")
                },
                new ApplicationSetting {
                    ApplicationSettingId = 9,
                    SettingName = "Files",
                    ParameterName = "DefaultName",
                    ParameterValue = "/[A-z]*_TIFF.(txt|pdf|doc)/g",
                    CreationDate = DateTime.Now.AddHours(-3),
                    CreatedBy = manager.FindByEmail("Admin@example.com")
                }
            };
            appSettings.ForEach(r => context.ApplicationSetting.AddOrUpdate(p => p.ApplicationSettingId, r));
            context.SaveChanges();
            #endregion
        }
    }
}