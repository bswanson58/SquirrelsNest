﻿using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Server.Database.Entities;

// The framework will set the DbSet properties appropriately:
#pragma warning disable CS8618 
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SquirrelsNest.Pecan.Server.Database {
    public class PecanDbContext : DbContext {
        public  DbSet<DbAssociation>    Associations { get; set; }
        public  DbSet<DbComponent>      Components { get; set; }
        public  DbSet<DbIssue>          Issues { get; set; }
        public  DbSet<DbIssueType>      IssueTypes { get; set; }
        public  DbSet<DbProject>        Projects { get; set; }
        public  DbSet<DbRelease>        Releases { get; set; }
        public  DbSet<DbUser>           Users { get; set; }
        public  DbSet<DbUserData>       UserData { get; set; }
        public  DbSet<DbWorkflowState>  WorkflowStates { get; set; }

        public PecanDbContext() { }

        public PecanDbContext( DbContextOptions<PecanDbContext> options ) :
            base( options ) { }

        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            base.OnModelCreating( modelBuilder );

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

            modelBuilder.Entity<DbUser>( e => {
                e.Property( p => p.EntityId ).IsRequired();
                e.Property( p => p.Name ).IsRequired();
            } );

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
