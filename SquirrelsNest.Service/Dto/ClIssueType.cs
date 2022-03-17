using System;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Service.Dto {
    public class ClIssueType : ClBase {
        public string ProjectId { get; }
        public string Name { get; }
        public string Description { get; }

        public ClIssueType( string id, string projectId, string name, string description ) :
            base( id ) {
            ProjectId = projectId;
            Name = name;
            Description = description;
        }

        private static ClIssueType ? mDefaultIssue;

        public static ClIssueType Default =>
            mDefaultIssue ??= new ClIssueType( EntityId.Default.Value, EntityId.Default.Value, "Unspecified", String.Empty );
    }

    public static class IssueTypeExtensions {
        public static ClIssueType From( this ClIssueType issue ) {
            return new ClIssueType( issue.Id, issue.ProjectId, issue.Name, issue.Description );
        }
    }

}
