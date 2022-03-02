using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Platform;

namespace SquirrelsNest.Core.ProjectTemplates {
    internal interface IProjectTemplateSerializer {
        IEnumerable<string>             ScanTemplates( string pattern );
        Either<Error, ProjectTemplate>  LoadTemplate( string fileName );
        Either<Error, Unit>             SaveTemplate( ProjectTemplate template, string fileName );
    }

    internal class ProjectTemplateSerializer : IProjectTemplateSerializer {
        private readonly IApplicationConstants  mApplicationConstants;
        private readonly IEnvironment           mEnvironment;
        private readonly IFileWriter            mFileWriter;

        public ProjectTemplateSerializer( IFileWriter fileWriter, IEnvironment environment, IApplicationConstants applicationConstants ) {
            mFileWriter = fileWriter;
            mEnvironment = environment;
            mApplicationConstants = applicationConstants;
        }

        private string TemplateFilePath( string templateFile ) => 
            Path.ChangeExtension( 
                Path.Combine( mEnvironment.TemplateDirectory(), templateFile ), 
                mApplicationConstants.ProjectTemplateExtension );

        public IEnumerable<string> ScanTemplates( string pattern ) {
            return Directory.EnumerateFiles( mEnvironment.TemplateDirectory(), pattern );
        }

        public Either<Error, ProjectTemplate> LoadTemplate( string fileName ) {
            return mFileWriter.Load<ProjectTemplate>( TemplateFilePath( fileName ));
        }

        public Either<Error, Unit> SaveTemplate( ProjectTemplate template, string fileName ) {
            return mFileWriter.Save( TemplateFilePath( fileName ), template );
        }
    }
}
