using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.Transfer.Export {
    public class ExportParameters {
        public  SnProject   Project { get; }
        public  bool        IncludeCompletedIssues { get; }
        public  string      ExportFilePath { get; }

        public ExportParameters( SnProject forProject, bool includeCompletedIssues, string toFilePath ) {
            Project = forProject;
            IncludeCompletedIssues = includeCompletedIssues;
            ExportFilePath = toFilePath;
        }

        public ExportParameters( SnProject forProject, bool includeCompletedIssues ) :
            this( forProject, includeCompletedIssues, String.Empty) {
        }
    }
}
