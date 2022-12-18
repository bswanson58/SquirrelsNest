using System;

namespace SquirrelsNest.Pecan.Shared.Dto {
    public class CreateProjectInput {
        public  string      Name { get; set; }
        public  string      Description { get; set; }
        public  string      IssuePrefix { get; set; }

        public CreateProjectInput() {
            Name = String.Empty;
            Description = String.Empty;
            IssuePrefix = String.Empty;
        }
    }
}
