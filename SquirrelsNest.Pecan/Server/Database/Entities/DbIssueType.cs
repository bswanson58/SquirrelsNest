﻿using System;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public class DbIssueType : DbEntityBase<DbIssueType> {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set; }
        public  string      Description { get; set; }

        public DbIssueType() {
            ProjectId = String.Empty;
            Name = String.Empty;
            Description = String.Empty;
        }

        public DbIssueType( SnIssueType issueType ) :
            base( issueType.EntityId ) {
            ProjectId = issueType.ProjectId;
            Name = issueType.Name;
            Description = issueType.Description;
        }

        public static DbIssueType From( SnIssueType issueType ) => new DbIssueType( issueType );

        public SnIssueType ToEntity() => new SnIssueType( EntityId, ProjectId, Name, Description );

        public override void UpdateFrom( DbIssueType from ) {
            ProjectId = from.ProjectId;
            Name = from.Name;
            Description = from.Description;
        }
    }
}
