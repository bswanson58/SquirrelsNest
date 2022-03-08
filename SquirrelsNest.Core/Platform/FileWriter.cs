using System.Text.Json;
using LanguageExt;
using LanguageExt.Common;

namespace SquirrelsNest.Core.Platform {
    public interface IFileWriter {
        Either<Error, T>            Load<T>( string filePath ) where T : new();
        EitherAsync<Error, T>       LoadAsync<T>( string filePath ) where T : new();

        Either<Error, Unit>         Save<T>( string filePath, T toSave );
        EitherAsync<Error, Unit>    SaveAsync<T>( string filePath, T toSave );
    }

    public class FileWriter : IFileWriter {
        private readonly JsonSerializerOptions  mOptions;

        public FileWriter() {
            mOptions = new JsonSerializerOptions( JsonSerializerDefaults.Web );

            mOptions.Converters.Add(new DateOnlyConverter());
            mOptions.WriteIndented = true;
        }

        public Either<Error, T> Load<T>( string filePath ) where T : new() {
            if(!File.Exists( filePath )) {
                return new T();
            }

            return 
                Prelude.Try( () => JsonSerializer.Deserialize<T>( File.ReadAllText( filePath ), mOptions ) ?? new T())
                    .ToEither( Error.New );
        }

        public EitherAsync<Error, T> LoadAsync<T>( string filePath ) where T : new() {
            if(!File.Exists( filePath )) {
                return new T();
            }

            return 
                Prelude.TryAsync( async () => 
                        JsonSerializer.Deserialize<T>( await File.ReadAllTextAsync( filePath ).ConfigureAwait( false ), mOptions ) ?? new T())
                    .ToEither( error => Error.New( error ));
        }

        public Either<Error, Unit> Save<T>( string filePath, T toSave ) {
            return 
                Prelude.Try( () => {
                    File.WriteAllText( filePath, JsonSerializer.Serialize( toSave, mOptions ));

                    return Unit.Default;
                })
                .ToEither( Error.New );
        }

        public EitherAsync<Error, Unit> SaveAsync<T>( string filePath, T toSave ) {
            return 
                Prelude.TryAsync( async () => {
                        await File.WriteAllTextAsync( filePath, JsonSerializer.Serialize( toSave, mOptions )).ConfigureAwait( false );

                        return Unit.Default;
                    })
                    .ToEither( error => Error.New( error ));
        }
    }
}
