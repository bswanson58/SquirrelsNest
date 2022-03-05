using System.Diagnostics;
using SquirrelsNest.Common.Entities;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable MemberCanBeProtected.Global

namespace SquirrelsNest.EfDb.Dto {
    [DebuggerDisplay("{" + nameof( Version ) + "}")]
    internal class DbIssueType : DbBase {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set;}
        public  string      Description { get; set; }

        protected DbIssueType() {
            Name = String.Empty;
            Description = String.Empty;
            ProjectId = Common.Values.EntityId.Default;
        }

        public static DbIssueType From( SnIssueType issue ) {
            return new DbIssueType {
                EntityId = issue.EntityId,
                ProjectId = issue.ProjectId,
                Id = String.IsNullOrWhiteSpace( issue.DbId ) ? DbIdDefault() : DbIdCreate( issue.DbId ),
                Name = issue.Name,
                Description = issue.Description
            };
        }

        public SnIssueType ToEntity() {
            return new SnIssueType( EntityId, Id.ToString(), ProjectId, Name, Description );
        }
    }
}
