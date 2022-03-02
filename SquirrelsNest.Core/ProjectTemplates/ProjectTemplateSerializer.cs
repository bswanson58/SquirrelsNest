using System.Text.Json;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.SomeHelp;
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
//            return Prelude.Try( () => mFileWriter.Load<ProjectTemplate>( TemplateFilePath( fileName ))).ToEither( Error.New );
            return mFileWriter.Load<ProjectTemplate>( TemplateFilePath( fileName ));
        }

        private Either<Error, Unit> Save<T>( string filePath, T settings ) {
            var x = Prelude.Try( () => {
                File.WriteAllText( filePath, JsonSerializer.Serialize( settings, new JsonSerializerOptions { WriteIndented = true }));

                return Unit.Default;
            });

            return x.ToEither( Error.New );
        }

        public Either<Error, Unit> SaveTemplate( ProjectTemplate template, string fileName ) {
            return mFileWriter.Save( TemplateFilePath( fileName ), template );
/*

            return Prelude.Try( () => {
                    mFileWriter.Save( TemplateFilePath( fileName ), template );

                    return Unit.Default;
                })
                .ToEither( Error.New );
*/        }
    }
}
