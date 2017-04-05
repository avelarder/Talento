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
            AutomaticMigrationsEnabled = false;
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

            #region PM User1
            if (!context.Users.Any(u => u.UserName == "Pmuser1@example.com"))
            {
                var passwordHash = new PasswordHasher();
                string password = passwordHash.HashPassword("Pmuser1@123456");

                var user = new ApplicationUser
                {
                    UserName = "Pmuser1@example.com",
                    Email = "Pmuser1@example.com",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult resultCreate = manager.Create(user);
                if (resultCreate.Succeeded == false)
                {
                    throw new Exception(resultCreate.Errors.First());
                }

                IdentityResult resultAddToRole = manager.AddToRole(user.Id, "PM");
                if (resultAddToRole.Succeeded == false)
                {
                    throw new Exception(resultAddToRole.Errors.First());
                }
            }
            context.SaveChanges();
            #endregion

            #region PM User2
            if (!context.Users.Any(u => u.UserName == "Pmuser2@example.com"))
            {
                var passwordHash = new PasswordHasher();
                string password = passwordHash.HashPassword("Pmuser2@123456");

                var user = new ApplicationUser
                {
                    UserName = "Pmuser2@example.com",
                    Email = "Pmuser2@example.com",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult resultCreate = manager.Create(user);
                if (resultCreate.Succeeded == false)
                {
                    throw new Exception(resultCreate.Errors.First());
                }

                IdentityResult resultAddToRole = manager.AddToRole(user.Id, "PM");
                if (resultAddToRole.Succeeded == false)
                {
                    throw new Exception(resultAddToRole.Errors.First());
                }
            }
            context.SaveChanges();
            #endregion

            #region TL User1
            if (!context.Users.Any(u => u.UserName == "Tluser1@example.com"))
            {
                var passwordHash = new PasswordHasher();
                string password = passwordHash.HashPassword("Tluser1@123456");

                var user = new ApplicationUser
                {
                    UserName = "Tluser1@example.com",
                    Email = "Tluser1@example.com",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult resultCreate = manager.Create(user);
                if (resultCreate.Succeeded == false)
                {
                    throw new Exception(resultCreate.Errors.First());
                }

                IdentityResult resultAddToRole = manager.AddToRole(user.Id, "TL");
                if (resultAddToRole.Succeeded == false)
                {
                    throw new Exception(resultAddToRole.Errors.First());
                }
            }
            context.SaveChanges();
            #endregion

            var tags = new List<Tag>
            {
                new Tag { Name = ".Net"},
                new Tag { Name = "Java"},
                new Tag { Name = "SQL"}
            };
            tags.ForEach(r => context.Tags.AddOrUpdate(p => p.Name, r));
            context.SaveChanges();

            var positions = new List<Position>
            {
                new Position
                {
                    Title = "Programador .Net",
                    Owner = manager.FindByEmail("Pmuser1@example.com"),
                    Area="IT",
                    RGS="",
                    Status = Status.Open,
                    CreationDate = DateTime.Now,
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="Il Padrino",
                    Description = "Add description here",
                    Tags = new List<Tag>{ context.Tags.Find(".Net") }
                },

                 new Position
                {
                    Title = "Programador java",
                    Owner = manager.FindByEmail("Pmuser2@example.com"),
                    Area="IT",
                    RGS="",
                    Status = Status.Canceled,
                    CreationDate = DateTime.Now.AddDays(-30),
                    EngagementManager ="El PropioEM",
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    Description = "Here is the description",
                    Tags = new List<Tag>{ context.Tags.Find("Java") }
                },

                  new Position
                {
                    Title = "Project Manager",
                    Owner = manager.FindByEmail("Pmuser1@example.com"),
                    Area="IT",
                    RGS="",
                    Status = Status.Closed,
                    CreationDate = DateTime.Now.AddDays(-6),
                    EngagementManager ="La Carito",
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    Description = "Hear iz de thescriction",
                    Tags = new List<Tag>{ context.Tags.Find(".Net") }
                },

                  new Position
                {
                    Title = "Programador php",
                    Owner = manager.FindByEmail("Tluser1@example.com"),
                    Area="IT",
                    RGS="",
                    Status = Status.Removed,
                    CreationDate = DateTime.Now.AddDays(-1),
                    EngagementManager ="Il Padrino",
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    Description = "Add description here",
                    Tags = new List<Tag>{ context.Tags.Find("SQL") }
                },

                  new Position
                {
                    Title = "Programador SQL",
                    Owner = manager.FindByEmail("Tluser1@example.com"),
                    Area="IT",
                    RGS="",
                    Status = Status.Open,
                    CreationDate = DateTime.Now.AddDays(-3),
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="Engagement Manager",
                    Description = "Add description here",
                    Tags = new List<Tag>{ context.Tags.Find("SQL") }
                },

                  new Position
                {
                    Title = "Programador .Net",
                    Owner = manager.FindByEmail("Pmuser1@example.com"),
                    Area="IT",
                    RGS="",
                    Status = Status.Closed,
                    CreationDate = DateTime.Now.AddDays(-4),
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="Il Padrino",
                    Description = "Add description here",
                    Tags = new List<Tag>{ context.Tags.Find(".Net") }
                },

                    new Position
                {
                    Title = "Programador Java",
                    Owner = manager.FindByEmail("Pmuser1@example.com"),
                    Area="IT",
                    RGS="",
                    Status = Status.Removed,
                    CreationDate = DateTime.Now,
                    EngagementManager ="Claudia",
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    Description = "Add description here",
                    Tags = new List<Tag>{ context.Tags.Find(".Net") }
                },
            };
            positions.ForEach(r => context.Positions.AddOrUpdate(p => p.Id, r));
            context.SaveChanges();

            var positionLogs = new List<PositionLog>
            {
                new PositionLog
                {
                    Action = Entities.Action.Create,
                    PreviousStatus = Status.Closed,
                    ActualStatus = Status.Open,
                    User = manager.FindByEmail("Admin@example.com"),
                    Position = context.Positions.Find(1),
                    Date = DateTime.Today
                },
                new PositionLog
                {
                    Action = Entities.Action.Delete,
                    PreviousStatus = Status.Open,
                    ActualStatus = Status.Removed,
                    User = manager.FindByEmail("Admin@example.com"),
                    Position = context.Positions.Find(1),
                    Date = DateTime.Today.AddMinutes(10)
                },
                new PositionLog
                {
                    Action = Entities.Action.Create,
                    PreviousStatus = Status.Closed,
                    ActualStatus = Status.Open,
                    User = manager.FindByEmail("Pmuser1@example.com"),
                    Position = context.Positions.Find(2),
                    Date = DateTime.Today.AddMinutes(60)
                },
                new PositionLog
                {
                    Action = Entities.Action.ChangeStatus,
                    PreviousStatus = Status.Open,
                    ActualStatus = Status.Canceled,
                    User = manager.FindByEmail("Pmuser1@example.com"),
                    Position = context.Positions.Find(2),
                    Date = DateTime.Today.AddMinutes(90)
                },
                new PositionLog
                {
                    Action = Entities.Action.Create,
                    PreviousStatus = Status.Closed,
                    ActualStatus = Status.Open,
                    User = manager.FindByEmail("Pmuser2@example.com"),
                    Position = context.Positions.Find(3),
                    Date = DateTime.Today.AddMinutes(90)
                },
                new PositionLog
                {
                    Action = Entities.Action.Edit,
                    PreviousStatus = Status.Open,
                    ActualStatus = Status.Open,
                    User = manager.FindByEmail("Pmuser2@example.com"),
                    Position = context.Positions.Find(3),
                    Date = DateTime.Today.AddMinutes(100)
                }
            };
            positionLogs.ForEach(r => context.PositionLogs.AddOrUpdate(p => p.Id, r));
            context.SaveChanges();
        }
    }
}
