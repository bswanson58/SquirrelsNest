using LanguageExt;
using MoreLinq;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Platform;

namespace SquirrelsNest.Core.ProjectTemplates {
    internal class ProjectCreator : IProjectCreator {
        private readonly IApplicationConstants  mAppConstants;
        private readonly IEnvironment           mEnvironment;
        private readonly IFileWriter            mFileWriter;
        private readonly ILog                   mLog;

        public ProjectCreator( IFileWriter fileWriter, IEnvironment environment, ILog log, IApplicationConstants appConstants ) {
            mFileWriter = fileWriter;
            mEnvironment = environment;
            mLog = log;
            mAppConstants = appConstants;
        }

        public IEnumerable<IProjectTemplate> GetAvailableTemplates() {
            var retValue = new List<ProjectTemplateFile>();
            
            ScanTemplates( $"*{mAppConstants.ProjectTemplateExtension}" )
                .ForEach( fileName => LoadTemplate( fileName )
                    .Match( Succ: template => retValue.Add( template ), 
                            Fail: exception => mLog.LogException( $"Loading project template file from: '{fileName}'", exception )));

            return retValue;
        }

        private IEnumerable<string> ScanTemplates( string pattern ) {
            return Directory.EnumerateFiles( mEnvironment.TemplateDirectory(), pattern );
        }

        private Try<ProjectTemplateFile> LoadTemplate( string fileName ) {
            return Prelude.Try( () => mFileWriter.Load<ProjectTemplateFile>( fileName ));
        }

        public void CreateProject( IProjectTemplate fromTemplate ) {
            throw new NotImplementedException();
        }
    }
}
