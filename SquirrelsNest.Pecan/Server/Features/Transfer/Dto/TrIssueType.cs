using System;
using System.Diagnostics;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Transfer.Dto {
    [DebuggerDisplay("{" + nameof( Version ) + "}")]
    internal class TrIssueType : TrBase {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set;}
        public  string      Description { get; set; }

        public TrIssueType() {
            Name = String.Empty;
            Description = String.Empty;
            ProjectId = EntityIdentifier.Default;
        }

        public static TrIssueType From( SnIssueType issue ) {
            return new TrIssueType {
                EntityId = issue.EntityId,
                ProjectId = issue.ProjectId,
                Name = issue.Name,
                Description = issue.Description
            };
        }

        public SnIssueType ToEntity() {
            return new SnIssueType( EntityId, ProjectId, Name, Description );
        }
    }
}
