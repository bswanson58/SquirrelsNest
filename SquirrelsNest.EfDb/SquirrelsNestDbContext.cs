using Microsoft.EntityFrameworkCore;
using SquirrelsNest.EfDb.Dto;
using SquirrelsNest.EfDb.Support;

namespace SquirrelsNest.EfDb {
    internal class SquirrelsNestDbContext : DbContext {
        public  DbSet<DbComponent>      Components { get; set; }
        public  DbSet<DbIssue>          Issues { get; set; }
        public  DbSet<DbIssueType>      IssueTypes { get; set; }
        public  DbSet<DbProject>        Projects { get; set; }
        public  DbSet<DbRelease>        Releases { get; set; }
        public  DbSet<DbUser>           Users { get; set; }
        public  DbSet<DbWorkflowState>  States { get; set; }

        protected override void ConfigureConventions( ModelConfigurationBuilder configurationBuilder ) {
            base.ConfigureConventions( configurationBuilder );

            configurationBuilder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");

            configurationBuilder.Properties<DateOnly?>()
                .HaveConversion<NullableDateOnlyConverter>()
                .HaveColumnType("date");
        }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder ) {
            optionsBuilder.UseSqlServer( @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=SquirrelsNestDB" );

            // Working with immutable entities will require entities to be replaced for updates and the
            // application will not be changing the entities that are retrieved (and would be tracked).
            optionsBuilder.UseQueryTrackingBehavior( QueryTrackingBehavior.NoTracking );
        }
    }
}
