using System.Text.Json;
using LanguageExt;
using LanguageExt.Common;

namespace SquirrelsNest.Core.Platform {
    public interface IFileWriter {
        Either<Error, T>        Load<T>( string filePath ) where T : new();
        Either<Error, Unit>     Save<T>( string filePath, T toSave );
    }

    public class FileWriter : IFileWriter {
        public Either<Error, T> Load<T>( string filePath ) where T : new() {
            if(!File.Exists( filePath )) {
                return new T();
            }

            return 
                Prelude.Try( () => JsonSerializer.Deserialize<T>( File.ReadAllText( filePath )) ?? new T())
                    .ToEither( Error.New );
        }

        public Either<Error, Unit> Save<T>( string filePath, T toSave ) {
            return 
                Prelude.Try( () => {
                    File.WriteAllText( filePath, JsonSerializer.Serialize( toSave, new JsonSerializerOptions { WriteIndented = true }));

                    return Unit.Default;
                })
                .ToEither( Error.New );
        }
    }
}
