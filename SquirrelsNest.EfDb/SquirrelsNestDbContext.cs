﻿using Microsoft.EntityFrameworkCore;
using SquirrelsNest.EfDb.Dto;
using SquirrelsNest.EfDb.Support;

// The framework will set the DbSet properties appropriately:
#pragma warning disable CS8618 
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SquirrelsNest.EfDb {
    internal class SquirrelsNestDbContext : DbContext {
        public  DbSet<DbComponent>      Components { get; set; }
        public  DbSet<DbIssue>          Issues { get; set; }
        public  DbSet<DbIssueType>      IssueTypes { get; set; }
        public  DbSet<DbProject>        Projects { get; set; }
        public  DbSet<DbRelease>        Releases { get; set; }
        public  DbSet<DbUser>           Users { get; set; }
        public  DbSet<DbWorkflowState>  States { get; set; }

        public SquirrelsNestDbContext( DbContextOptions options ) :
            base( options  ) { }

        protected override void ConfigureConventions( ModelConfigurationBuilder configurationBuilder ) {
            base.ConfigureConventions( configurationBuilder );

            configurationBuilder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");

            configurationBuilder.Properties<DateOnly?>()
                .HaveConversion<NullableDateOnlyConverter>()
                .HaveColumnType("date");
        }
    }
}
