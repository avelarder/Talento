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
                new Tag { Name = "PMP"},
                new Tag { Name = "Haskell"},
                new Tag { Name = "Lamda Calculus"},
                new Tag { Name = "Object Orientated Programming"},
                new Tag { Name = "Madness"},
            };
            tags.ForEach(r => context.Tags.AddOrUpdate(p => p.Name, r));
            context.SaveChanges();

            #region PositionsSeed

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
                    LastOpenedDate = Convert.ToDateTime("2017-03-15T13:45:30"),
                    Logs = new List<Log>
                    {
                        new Log
                        {
                            Action = Entities.Action.Create,
                            ActualStatus = Status.Open,
                            User = manager.FindByEmail("Pmuser1@example.com"),
                            Description = string.Format("Position Created by {0} at {1}", "Pmuser1@example.com", Convert.ToDateTime("2017-03-15T13:45:30").ToShortDateString()),
                            Date = Convert.ToDateTime("2017-03-15T13:45:30")
                        }
                    }
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
                    LastClosedDate = Convert.ToDateTime("2017-03-30T18:01:30"),
                    Logs =  new List<Log>
                    {
                        new Log
                        {
                            Action = Entities.Action.Create,
                            ActualStatus = Status.Open,
                            Description = string.Format("Position Created by {0} at {1}", "Tluser1@example.com", Convert.ToDateTime("2017-03-22T09:43:30").ToShortDateString()),
                            User = manager.FindByEmail("Tluser1@example.com"),
                            Date = Convert.ToDateTime("2017-03-22T09:43:30")
                        }
                    }
                },
                new Position
                {
                    Id = 3,
                    Title = "Java Programmer",
                    Owner = manager.FindByEmail("Tluser1@example.com"),
                    Area="BPO",
                    RGS="",
                    Status = Status.Cancelled,
                    CreationDate = Convert.ToDateTime("2017-02-15T11:13:30"),
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="Anibal Barca",
                    Description = "Java programmer. Not Javascript!!!!",
                    Tags = new List<Tag>{ context.Tags.Find("Java") },
                    LastOpenedBy = manager.FindByEmail("Tluser1@example.com"),
                    LastOpenedDate = Convert.ToDateTime("2017-02-15T11:13:30"),
                    LastCancelledBy = manager.FindByEmail("Tluser1@example.com"),
                    LastCancelledDate = Convert.ToDateTime("2017-02-22T13:58:30"),
                    Logs = new List<Log>
                    {
                          new Log
                        {
                            Action = Entities.Action.Create,
                            ActualStatus = Status.Open,
                            Description = string.Format("Position Created by {0} at {1}", "Tluser1@example.com", Convert.ToDateTime("2017-02-15T11:13:30").ToShortDateString()),
                            User = manager.FindByEmail("Tluser1@example.com"),
                            Date = Convert.ToDateTime("2017-02-15T11:13:30")
                        },
                        new Log
                        {
                            Action = Entities.Action.Edit,
                            PreviousStatus = Status.Open,
                            ActualStatus = Status.Closed,
                            Description = string.Format("Position Edited by {0} at {1}", "Pmuser1@example.com", Convert.ToDateTime("2017-03-30T18:01:30").ToShortDateString()),
                            User = manager.FindByEmail("Pmuser1@example.com"),
                            Date = Convert.ToDateTime("2017-03-30T18:01:30")
                        }
                    }
                },
                new Position
                {
                    Id = 4,
                    Title = "Some cool Programmer",
                    Owner = manager.FindByEmail("Tluser1@example.com"),
                    Area="Dev",
                    RGS="",
                    Status = Status.Open,
                    CreationDate = Convert.ToDateTime("2017-01-03T11:13:30"),
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="THE Engagement Manager",
                    Description = "Yeah, you just have to be cool",
                    Tags = new List<Tag>{},
                    LastOpenedBy = manager.FindByEmail("Tluser1@example.com"),
                    LastOpenedDate = Convert.ToDateTime("2017-01-03T11:13:30"),
                    Logs = new List<Log>
                    {
                        new Log
                        {
                            Action = Entities.Action.Create,
                            ActualStatus = Status.Open,
                            Description = string.Format("Position Created by {0} at {1}", "Tluser1@example.com", Convert.ToDateTime("2017-01-03T11:13:30").ToShortDateString()),
                            User = manager.FindByEmail("Tluser1@example.com"),
                            Date = Convert.ToDateTime("2017-01-03T11:13:30")
                        }
                    }
                },
                new Position
                {
                    Id = 5,
                    Title = "Haskell Programmer",
                    Owner = manager.FindByEmail("Tluser1@example.com"),
                    Area="CrazyDevs",
                    RGS="",
                    Status = Status.Open,
                    CreationDate = Convert.ToDateTime("2017-04-03T12:13:30"),
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="El payaso plim plim",
                    Description = "Lambda Programming skills",
                    Tags = new List<Tag>{ context.Tags.Find("Haskell"),context.Tags.Find("Madness"), },
                    LastOpenedBy = manager.FindByEmail("Tluser1@example.com"),
                    LastOpenedDate = Convert.ToDateTime("2017-04-03T12:13:30"),
                    Logs = new List<Log>
                    {
                        new Log
                        {
                            Action = Entities.Action.Create,
                            ActualStatus = Status.Open,
                            Description = string.Format("Position Created by {0} at {1}", "Tluser1@example.com", Convert.ToDateTime("2017-04-03T12:13:30").ToShortDateString()),
                            User = manager.FindByEmail("Tluser1@example.com"),
                            Date = Convert.ToDateTime("2017-04-03T12:13:30")
                        }
                    }
                },
                new Position
                {
                    Id = 6,
                    Title = "Crazy Dev",
                    Owner = manager.FindByEmail("Tluser1@example.com"),
                    Area="Dev",
                    RGS="",
                    Status = Status.Open,
                    CreationDate = Convert.ToDateTime("2017-03-15T11:13:30"),
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="NoOne InParticular",
                    Description = "Go for poloymoprhic types!!",
                    Tags = new List<Tag>{ context.Tags.Find("Madness") },
                    LastOpenedBy = manager.FindByEmail("Tluser1@example.com"),
                    LastOpenedDate = Convert.ToDateTime("2017-03-15T11:13:30"),
                    Logs = new List<Log>
                    {
                       new Log
                        {
                            Action = Entities.Action.Create,
                            ActualStatus = Status.Open,
                            Description = string.Format("Position Created by {0} at {1}", "Tluser1@example.com", Convert.ToDateTime("2017-03-15T11:13:30").ToShortDateString()),
                            User = manager.FindByEmail("Tluser1@example.com"),
                            Date = Convert.ToDateTime("2017-03-15T11:13:30")
                        }
                    }
                },
                new Position
                {
                    Id = 7,
                    Title = "Second dev",
                    Owner = manager.FindByEmail("Tluser1@example.com"),
                    Area="Dev",
                    RGS="",
                    Status = Status.Open,
                    CreationDate = Convert.ToDateTime("2017-02-19T11:13:30"),
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="Anibal Barca",
                    Description = "We need another dev for pair programming, the previous one left its partner alone :(",
                    Tags = new List<Tag>{ context.Tags.Find("Objet Orientated Programming") },
                    LastOpenedBy = manager.FindByEmail("Tluser1@example.com"),
                    LastOpenedDate = Convert.ToDateTime("2017-02-19T11:13:30"),
                    Logs = new List<Log>
                    {
                        new Log
                        {
                            Action = Entities.Action.Edit,
                            PreviousStatus = Status.Open,
                            ActualStatus = Status.Closed,
                            Description = string.Format("Position Closed by {0} at {1}", "Pmuser1@example.com", Convert.ToDateTime("2017-03-30T18:01:30").ToShortDateString()),
                            User = manager.FindByEmail("Pmuser1@example.com"),
                            Date = Convert.ToDateTime("2017-03-30T18:01:30")
                        }
                    }
                },

                new Position
                {
                    Id = 8,
                    Title = "Pair Programming",
                    Owner = manager.FindByEmail("Tluser1@example.com"),
                    Area="Dev",
                    RGS="",
                    Status = Status.Closed,
                    CreationDate = Convert.ToDateTime("2017-02-09T11:13:30"),
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="Alejandro Magno",
                    Description = "You will work with a pair.",
                    Tags = new List<Tag>{ context.Tags.Find("Objet Orientated Programming") },
                    LastOpenedBy = manager.FindByEmail("Tluser1@example.com"),
                    LastOpenedDate = Convert.ToDateTime("2017-02-09T11:13:30"),
                    LastClosedBy = manager.FindByEmail("Tluser1@example.com"),
                    LastClosedDate = Convert.ToDateTime("2017-03-01T14:08:12"),
                    Logs = new List<Log>
                    {
                        new Log
                        {
                            Action = Entities.Action.Edit,
                            PreviousStatus = Status.Open,
                            ActualStatus = Status.Closed,
                            User = manager.FindByEmail("Pmuser1@example.com"),
                            Description = string.Format("Position Closed by {0} at {1}", "Pmuser1@example.com", Convert.ToDateTime("2017-03-30T18:01:30").ToShortDateString()),
                            Date = Convert.ToDateTime("2017-03-30T18:01:30")
                        }
                    }
                },
                new Position
                {
                    Id = 9,
                    Title = "Pair Programming",
                    Owner = manager.FindByEmail("Tluser1@example.com"),
                    Area="Dev",
                    RGS="",
                    Status = Status.Closed,
                    CreationDate = Convert.ToDateTime("2017-02-15T11:13:30"),
                    PortfolioManager = manager.FindByEmail("Pmuser1@example.com"),
                    EngagementManager ="Anibal Barca",
                    Description = "You will work with a pair.",
                    Tags = new List<Tag>{ context.Tags.Find("Objet Orientated Programming") },
                    LastOpenedBy = manager.FindByEmail("Pmuser1@example.com"),
                    LastOpenedDate = Convert.ToDateTime("2017-07-22T13:58:30"),
                    LastClosedBy = manager.FindByEmail("Pmuser1@example.com"),
                    LastClosedDate = Convert.ToDateTime("2017-05-22T08:58:30"),
                    Logs = new List<Log>() {
                        new Log
                        {
                            Action = Entities.Action.Create,
                            ActualStatus = Status.Open,
                            User = manager.FindByEmail("Tluser1@example.com"),
                            Description = string.Format("Position Opened by {0} at {1}", "Tluser1@example.com", Convert.ToDateTime("2017-02-15T11:13:30").ToShortDateString()),
                            Date = Convert.ToDateTime("2017-02-15T11:13:30")
                        },
                        new Log
                        {
                            Action = Entities.Action.Edit,
                            PreviousStatus = Status.Open,
                            ActualStatus = Status.Closed,
                            User = manager.FindByEmail("Pmuser1@example.com"),
                            Description = string.Format("Position Opened by {0} at {1}", "Pmuser1@example.com", Convert.ToDateTime("2017-03-22T13:58:30").ToShortDateString()),
                            Date = Convert.ToDateTime("2017-03-22T13:58:30")
                        },
                        new Log
                        {
                            Action = Entities.Action.Edit,
                            PreviousStatus = Status.Closed,
                            ActualStatus = Status.Open,
                            User = manager.FindByEmail("Pmuser1@example.com"),
                            Description = string.Format("Position Opened by {0} at {1}", "Pmuser1@example.com", Convert.ToDateTime("2017-04-22T11:58:30").ToShortDateString()),
                            Date = Convert.ToDateTime("2017-04-22T11:58:30")
                        },
                        new Log
                        {
                            Action = Entities.Action.Edit,
                            PreviousStatus = Status.Open,
                            ActualStatus = Status.Open,
                            User = manager.FindByEmail("Pmuser2@example.com"),
                            Description = string.Format("Position Opened by {0} at {1}", "Pmuser2@example.com", Convert.ToDateTime("2017-04-22T12:50:00").ToShortDateString()),
                            Date = Convert.ToDateTime("2017-04-22T12:50:00")
                        },
                        new Log
                        {
                            Action = Entities.Action.Edit,
                            PreviousStatus = Status.Open,
                            ActualStatus = Status.Open,
                            User = manager.FindByEmail("Pmuser1@example.com"),
                            Description = string.Format("Position Opened by {0} at {1}", "Pmuser1@example.com", Convert.ToDateTime("2017-04-22T12:52:00").ToShortDateString()),
                            Date = Convert.ToDateTime("2017-04-22T12:52:00")
                        },
                        new Log
                        {
                            Action = Entities.Action.Edit,
                            PreviousStatus = Status.Open,
                            ActualStatus = Status.Closed,
                            User = manager.FindByEmail("Pmuser1@example.com"),
                            Description = string.Format("Position Closed by {0} at {1}", "Pmuser1@example.com", Convert.ToDateTime("2017-05-22T08:58:30").ToShortDateString()),
                            Date = Convert.ToDateTime("2017-05-22T08:58:30")
                        },
                        new Log
                        {
                            Action = Entities.Action.Edit,
                            PreviousStatus = Status.Closed,
                            ActualStatus = Status.Open,
                            User = manager.FindByEmail("Pmuser1@example.com"),
                            Description = string.Format("Position Opened by {0} at {1}", "Pmuser1@example.com", Convert.ToDateTime("2017-07-22T13:58:30").ToShortDateString()),
                            Date = Convert.ToDateTime("2017-07-22T13:58:30")
                        },
                        new Log
                        {
                            Action = Entities.Action.Edit,
                            PreviousStatus = Status.Open,
                            ActualStatus = Status.Open,
                            Description = string.Format("Position Opened by {0} at {1}", "Pmuser1@example.com", Convert.ToDateTime("2017-07-22T14:00:30").ToShortDateString()),
                            User = manager.FindByEmail("Pmuser1@example.com"),
                            Date = Convert.ToDateTime("2017-07-22T14:00:30")
                        }
                    }
                },
            };
            positions.ForEach(r => context.Positions.AddOrUpdate(p => p.Id, r));
            context.SaveChanges();

            #endregion

            #region tcscandidates

            var tcsCandidates = new List<Candidate>
            {
               new Candidate{
                   Id = 1,
                   Email = "Candidate1@example.com",
                   Name = "Example Employee 1",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-10T12:30:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.Accepted,
                   IsTcsEmployee = true
               },
               new Candidate{
                   Id = 2,
                   Email = "Candidate2@example.com",
                   Name = "Example Employee 2",
                   Competencies ="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-20T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.Accepted
               },
               new Candidate{
                   Id = 3,
                   Email = "Candidate3@example.com",
                   Name = "Example Employee 3",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-02-09T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.Accepted
               },
               new Candidate{
                   Id = 4,
                   Email = "Candidate4@example.com",
                   Name = "Example Employee 4",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-04-11T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.Accepted
               },
               new Candidate{
                   Id = 5,
                   Email = "Candidate5@example.com",
                   Name = "Example Employee 5",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-01-16T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.Accepted
               },
               new Candidate{
                   Id = 6,
                   Email = "Candidate6@example.com",
                   Name = "Example Employee 6",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-01-17T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.Accepted
               },
               new Candidate{
                   Id = 7,
                   Email = "Candidate7@example.com",
                   Name = "Example Employee 7",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-02-15T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.Accepted
               },
               new Candidate{
                   Id = 8,
                   Email = "Candidate8@example.com",
                   Name = "Example Employee 8",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-15T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.Accepted
               },
               new Candidate{
                   Id = 9,
                   Email = "Candidate9@example.com",
                   Name = "Example Employee 9",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-04-11T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.Accepted
               },
               new Candidate{
                   Id = 10,
                   Email = "Candidate0@example.com",
                   Name = "Example Employee 0",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-02-15T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.Accepted
               }
            };

            tcsCandidates.ForEach(r => context.Candidates.AddOrUpdate(p => p.Id, r));
            context.SaveChanges();

            #endregion tcscandidates

            #region nontcscandidates

            var nontcsCandidates = new List<Candidate>
            {
               new Candidate{
                   Id = 11,
                   Email = "Candidate11@example.com",
                   Name = "Example Employee 11",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-01T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.New,
                   IsTcsEmployee = false
               },
               new Candidate{
                   Id = 12,
                   Email = "Candidate12@example.com",
                   Name = "Example Employee 12",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-04T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.New,
                   IsTcsEmployee = false
               },
               new Candidate{
                   Id = 13,
                   Email = "Candidate13@example.com",
                   Name = "Example Employee 13",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-06T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.New,
                   IsTcsEmployee = false
               },
               new Candidate{
                   Id = 14,
                   Email = "Candidate14@example.com",
                   Name = "Example Employee 14",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-09T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.New,
                   IsTcsEmployee = false
               },
               new Candidate{
                   Id = 15,
                   Email = "Candidate15@example.com",
                   Name = "Example Employee 15",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-12T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.New,
                   IsTcsEmployee = false
               },
               new Candidate{
                   Id = 16,
                   Email = "Candidate16@example.com",
                   Name = "Example Employee 16",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-17T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.New,
                   IsTcsEmployee = false
               },
               new Candidate{
                   Id = 17,
                   Email = "Candidate17@example.com",
                   Name = "Example Employee 17",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-15T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.New,
                   IsTcsEmployee = false
               },
               new Candidate{
                   Id = 18,
                   Email = "Candidate18@example.com",
                   Name = "Example Employee 18",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-20T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.New,
                   IsTcsEmployee = false
               },
               new Candidate{
                   Id = 19,
                   Email = "Candidate19@example.com",
                   Name = "Example Employee 19",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-22T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.New,
                   IsTcsEmployee = false
               },
               new Candidate{
                   Id = 20,
                   Email = "Candidate00@example.com",
                   Name = "Example Employee 00",
                   Competencies="Comma separated competencies",
                   Description = "This is a description",
                   CratedOn = Convert.ToDateTime("2017-03-27T13:45:30"),
                   CreatedBy = manager.FindByEmail("Pmuser1@example.com"),
                   Status = CandidateStatus.New,
                   IsTcsEmployee = false
               }
            };

            nontcsCandidates.ForEach(r => context.Candidates.AddOrUpdate(p => p.Id, r));
            context.SaveChanges();

            #endregion tcscandidates

            #region Candidates in positions
            Position position1 = context.Positions.Find(1);
            List<int> listId = new List<int> { 1, 2, 3, 4, 5 };
            List<Candidate> candidatesPosition1 = context.Candidates.Where(candidate => listId.Contains(candidate.Id)).ToList();

            position1.Candidates = candidatesPosition1;
            context.SaveChanges();

            Position position2 = context.Positions.Find(2);
            listId = new List<int> { 6 };
            List<Candidate> candidatesPosition2 = context.Candidates.Where(candidate => listId.Contains(candidate.Id)).ToList();

            position2.Candidates = candidatesPosition2;
            context.SaveChanges();

            Position position6 = context.Positions.Find(6);
            listId = new List<int> {7, 8, 9, 10, 11, 12, 13, 14, 15 };
            List<Candidate> candidatesPosition6 = context.Candidates.Where(candidate => listId.Contains(candidate.Id)).ToList();

            position6.Candidates = candidatesPosition6;
            context.SaveChanges();

            Position position5 = context.Positions.Find(5);
            listId = new List<int> { 16, 17, 18, 19, 20 };
            List<Candidate> candidatesPosition5 = context.Candidates.Where(candidate => listId.Contains(candidate.Id)).ToList();

            position5.Candidates = candidatesPosition5;
            context.SaveChanges();
            #endregion
        }
    }
}
