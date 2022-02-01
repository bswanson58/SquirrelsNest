namespace SquirrelsNest.Common.Entities {
    public class SnProject : EntityBase {
        public  string      Name { get; }
        public  string      Description { get; }
        public  DateOnly    Inception { get; }
        public  string      RepositoryUrl { get; }
        public  string      IssuePrefix { get; }
        public  int         NextIssueNumber { get; }

        public SnProject( string entityId, string name, string description, DateOnly inception, string repository, string issuePrefix, int nextIssueNumber ) :
            base( entityId ) {
            Name = name;
            Description = description;
            Inception = inception;
            RepositoryUrl = repository;
            IssuePrefix = issuePrefix;
            NextIssueNumber = nextIssueNumber;
        }
    }
}
