using LiteDB;

namespace SquirrelsNest.LiteDb.Dto {
    internal class DbProject : DbBase {
        public  string      Name { get; set; }
        public  string      Description { get; set; }
        public  DateOnly    Inception { get; set; }
        public  string      RepositoryUrl { get; set; }
        public  string      IssuePrefix { get; set; }
        public  int         NextIssueNumber { get; set; }

        public DbProject() {
            Name = String.Empty;
            Description = String.Empty;
            Inception = DateOnly.FromDateTime( DateTime.Now );
            RepositoryUrl = String.Empty;
            IssuePrefix = String.Empty;
            NextIssueNumber = 1;
        }
    }
}
