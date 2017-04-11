using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("TalentoDB", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public virtual System.Data.Entity.DbSet<Tag> Tags { get; set; }

        public virtual System.Data.Entity.DbSet<Position> Positions { get; set; }

        public virtual System.Data.Entity.DbSet<PositionLog> PositionLogs { get; set; }

        public virtual System.Data.Entity.DbSet<Candidate> Candidates { get; set; }

        public virtual System.Data.Entity.DbSet<TcsCandidate> TcsCandidates { get; set; }

        public virtual System.Data.Entity.DbSet<NonTcsCandidate> NonTcsCandidates { get; set; }

        public virtual System.Data.Entity.DbSet<PositionCandidates> PositionsCandidates { get; set; }

        public virtual System.Data.Entity.DbSet<FileBlob> FileBlobs { get; set; }

    }
}
