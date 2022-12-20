using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Server.Database.Support;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// The framework will set the DbSet properties appropriately:
#pragma warning disable CS8618 
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SquirrelsNest.Pecan.Server.Database {
    public interface IDbContext {
        DbSet<DbAssociation>        Associations { get; }
        DbSet<DbComponent>          Components { get; }
        DbSet<DbIssue>              Issues { get; }
        DbSet<DbIssueType>          IssueTypes { get; }
        DbSet<DbProject>            Projects { get; }
        DbSet<DbRelease>            Releases { get; }
        DbSet<DbUserData>           UserData { get; }
        DbSet<DbWorkflowState>      WorkflowStates { get; }
        DbSet<IdentityUser>         Users { get; }
    }

    public class PecanDbContext : IdentityDbContext<IdentityUser>, IDbContext {
        public  DbSet<DbAssociation>    Associations { get; set; }
        public  DbSet<DbComponent>      Components { get; set; }
        public  DbSet<DbIssue>          Issues { get; set; }
        public  DbSet<DbIssueType>      IssueTypes { get; set; }
        public  DbSet<DbProject>        Projects { get; set; }
        public  DbSet<DbRelease>        Releases { get; set; }
//        public  DbSet<DbUser>           Users { get; set; }
        public  DbSet<DbUserData>       UserData { get; set; }
        public  DbSet<DbWorkflowState>  WorkflowStates { get; set; }

        public PecanDbContext() { }

        public PecanDbContext( DbContextOptions<PecanDbContext> options ) :
            base( options ) { }

        protected override void ConfigureConventions( ModelConfigurationBuilder configurationBuilder ) {
            base.ConfigureConventions( configurationBuilder );

            configurationBuilder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType( "date" );

            configurationBuilder.Properties<DateOnly?>()
                .HaveConversion<NullableDateOnlyConverter>()
                .HaveColumnType( "date" );
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            base.OnModelCreating( modelBuilder );

            InitializeModelProperties( modelBuilder );

//            modelBuilder.ApplyConfiguration( new UserConfiguration());
        }

        private void InitializeModelProperties( ModelBuilder modelBuilder ) {
            modelBuilder.Entity<DbAssociation>( e => {
                e.Property( p => p.EntityId ).IsRequired();
                e.Property( p => p.AssociationId ).IsRequired();
                e.Property( p => p.OwnerId ).IsRequired();
            } );

            modelBuilder.Entity<DbComponent>( e => {
                e.Property( p => p.EntityId ).IsRequired();
                e.Property( p => p.ProjectId ).IsRequired();
                e.Property( p => p.Name ).IsRequired();
            } );

            modelBuilder.Entity<DbIssue>( e => {
                e.Property( p => p.EntityId ).IsRequired();
                e.Property( p => p.ProjectId ).IsRequired();
                e.Property( p => p.Title ).IsRequired();
            } );

            modelBuilder.Entity<DbIssueType>( e => {
                e.Property( p => p.EntityId ).IsRequired();
                e.Property( p => p.ProjectId ).IsRequired();
                e.Property( p => p.Name ).IsRequired();
            } );

            modelBuilder.Entity<DbProject>( e => {
                e.Property( p => p.EntityId ).IsRequired();
                e.Property( p => p.IssuePrefix ).IsRequired();
                e.Property( p => p.Name ).IsRequired();
            } );

            modelBuilder.Entity<DbRelease>( e => {
                e.Property( p => p.EntityId ).IsRequired();
                e.Property( p => p.ProjectId ).IsRequired();
                e.Property( p => p.Name ).IsRequired();
            } );

//            modelBuilder.Entity<DbUser>( e => {
//                e.Property( p => p.EntityId ).IsRequired();
//                e.Property( p => p.Name ).IsRequired();
//            } );

            modelBuilder.Entity<DbUserData>( e => {
                e.Property( p => p.EntityId ).IsRequired();
                e.Property( p => p.UserId ).IsRequired();
            } );

            modelBuilder.Entity<DbWorkflowState>( e => {
                e.Property( p => p.EntityId ).IsRequired();
                e.Property( p => p.ProjectId ).IsRequired();
                e.Property( p => p.Name ).IsRequired();
            } );
        }
    }
}
