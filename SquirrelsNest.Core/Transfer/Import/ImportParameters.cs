namespace SquirrelsNest.Core.Transfer.Import {
    public class ImportParameters {
        public  string      ImportFilePath { get; }
        public  string      ProjectName { get; }

        public ImportParameters( string filePath, string projectName ) {
            ImportFilePath = filePath;
            ProjectName = projectName;
        }
    }
}
