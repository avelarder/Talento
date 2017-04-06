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
                new Tag { Name = "SQL"},
                new Tag { Name = "PMP"}
            };
            tags.ForEach(r => context.Tags.AddOrUpdate(p => p.Name, r));
            context.SaveChanges();

            var positions = new List<Position>
            {
                new Position
                {
                    Id = 1,
                    Title = "Programador .Net",
                    Owner = manager.FindByEmail("Pmuser1@example.com"),
                    Area="IT",
                    RGS="",
                    Status = Status.Open,
                    CreationDate = Convert.ToDateTime("2017-03-15T13:45:30"),
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="Napoleon bonaparte",
                    Description = "Full-stack .Net developer with knowledge in everything.",
                    Tags = new List<Tag>{ context.Tags.Find(".Net") },
                    LastOpenedBy = manager.FindByEmail("Pmuser1@example.com"),
                    LastOpenedDate = Convert.ToDateTime("2017-03-15T13:45:30")
                },
                new Position
                {
                    Id = 2,
                    Title = "Project Manager",
                    Owner = manager.FindByEmail("Tluser1@example.com"),
                    Area="BPO",
                    RGS="",
                    Status = Status.Closed,
                    CreationDate = Convert.ToDateTime("2017-03-22T09:43:30"),
                    PortfolioManager = manager.FindByEmail("Pmuser2@example.com"),
                    EngagementManager ="Alejandro Magno",
                    Description = "Project manager with Mode-God on",
                    Tags = new List<Tag>{ context.Tags.Find("PMP") },
                    LastOpenedBy = manager.FindByEmail("Tluser1@example.com"),
                    LastOpenedDate = Convert.ToDateTime("2017-03-22T09:43:30"),
                    LastClosedBy = manager.FindByEmail("Pmuser1@example.com"),
                    LastClosedDate = Convert.ToDateTime("2017-03-30T18:01:30")
                },
                new Position
                {
                    Id = 3,
                    Title = "Java Programmer",
                    Owner = manager.FindByEmail("Tluser1@example.com"),
                    Area="BPO",
                    RGS="",
                    Status = Status.Canceled,
                    CreationDate = Convert.ToDateTime("2017-02-15T11:13:30"),
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="Anibal Barca",
                    Description = "Java programmer. Not Javascript!!!!",
                    Tags = new List<Tag>{ context.Tags.Find("Java") },
                    LastOpenedBy = manager.FindByEmail("Tluser1@example.com"),
                    LastOpenedDate = Convert.ToDateTime("2017-02-15T11:13:30"),
                    LastCancelledBy = manager.FindByEmail("Tluser1@example.com"),
                    LastCancelledDate = Convert.ToDateTime("2017-02-22T13:58:30")
                },
            };
            positions.ForEach(r => context.Positions.AddOrUpdate(p => p.Id, r));
            context.SaveChanges();

            var positionLogs = new List<PositionLog>
            {
                new PositionLog
                {
                    Action = Entities.Action.Create,
                    ActualStatus = Status.Open,
                    User = manager.FindByEmail("Pmuser1@example.com"),
                    Position = context.Positions.Find(1),
                    Date = Convert.ToDateTime("2017-03-15T13:45:30")
                },
                new PositionLog
                {
                    Action = Entities.Action.Create,
                    ActualStatus = Status.Open,
                    User = manager.FindByEmail("Tluser1@example.com"),
                    Position = context.Positions.Find(2),
                    Date = Convert.ToDateTime("2017-03-22T09:43:30")
                },
                new PositionLog
                {
                    Action = Entities.Action.Edit,
                    PreviousStatus = Status.Open,
                    ActualStatus = Status.Closed,
                    User = manager.FindByEmail("Pmuser1@example.com"),
                    Position = context.Positions.Find(2),
                    Date = Convert.ToDateTime("2017-03-30T18:01:30")
                },
                new PositionLog
                {
                    Action = Entities.Action.Create,
                    ActualStatus = Status.Open,
                    User = manager.FindByEmail("Tluser1@example.com"),
                    Position = context.Positions.Find(3),
                    Date = Convert.ToDateTime("2017-02-15T11:13:30"),
                },
                new PositionLog
                {
                    Action = Entities.Action.Edit,
                    PreviousStatus = Status.Open,
                    ActualStatus = Status.Canceled,
                    User = manager.FindByEmail("Tluser1@example.com"),
                    Position = context.Positions.Find(3),
                    Date = Convert.ToDateTime("2017-02-22T13:58:30")
                },
            };
            positionLogs.ForEach(r => context.PositionLogs.AddOrUpdate(p => p.Id, r));
            context.SaveChanges();
        }
    }
}
