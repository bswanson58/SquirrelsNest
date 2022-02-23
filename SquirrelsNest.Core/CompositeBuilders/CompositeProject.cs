using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.CompositeBuilders {
    public class CompositeProject {
        public  SnProject                       Project { get; }
        public  IReadOnlyList<SnIssueType>      IssueTypes { get; }

        public CompositeProject( SnProject project, IEnumerable<SnIssueType> issueTypes ) {
            Project = project;
            IssueTypes = new List<SnIssueType>( issueTypes );
        }
    }
}
