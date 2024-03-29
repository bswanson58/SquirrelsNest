﻿using System.Text.Json;
using LanguageExt;
using LanguageExt.Common;

namespace SquirrelsNest.Core.Platform {
    public interface IFileWriter {
        Either<Error, T>            Load<T>( string filePath ) where T : new();
        EitherAsync<Error, T>       LoadAsync<T>( string filePath ) where T : new();
        Either<Error, T>            Load<T>( Stream stream ) where T : new();
        EitherAsync<Error, T>       LoadAsync<T>( Stream stream ) where T : new ();

        Either<Error, Unit>         Save<T>( string filePath, T toSave );
        EitherAsync<Error, Unit>    SaveAsync<T>( string filePath, T toSave );
        Either<Error, Unit>         Save<T>( Stream stream, T toSave );
        EitherAsync<Error, Unit>    SaveAsync<T>( Stream stream, T toSave );
    }

    public class FileWriter : IFileWriter {
        private readonly JsonSerializerOptions  mOptions;

        public FileWriter() {
            mOptions = new JsonSerializerOptions( JsonSerializerDefaults.Web );

            mOptions.Converters.Add( new DateOnlyConverter());
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
                        JsonSerializer.Deserialize<T>( await File.ReadAllTextAsync( filePath ).ConfigureAwait( false ), mOptions ) ?? new T() )
                    .ToEither( error => Error.New( error ) );
        }

        public Either<Error, T> Load<T>( Stream stream ) where T : new() {
            return 
                Prelude.Try( () => JsonSerializer.Deserialize<T>( stream, mOptions ) ?? new T())
                    .ToEither( Error.New );
        }

        public EitherAsync<Error, T> LoadAsync<T>( Stream stream ) where T : new() {
            return
                Prelude.TryAsync( async () =>
                        await JsonSerializer.DeserializeAsync<T>( stream, mOptions ) ?? new T())
                    .ToEither( error => Error.New( error ));
        }

        public Either<Error, Unit> Save<T>( string filePath, T toSave ) {
            return
                Prelude.Try( () => {
                    File.WriteAllText( filePath, JsonSerializer.Serialize( toSave, mOptions ));

                    return Unit.Default;
                } )
                .ToEither( Error.New );
        }

        public EitherAsync<Error, Unit> SaveAsync<T>( string filePath, T toSave ) {
            return
                Prelude.TryAsync( async () => {
                    await File.WriteAllTextAsync( filePath, JsonSerializer.Serialize( toSave, mOptions )).ConfigureAwait( false );

                    return Unit.Default;
                } )
                    .ToEither( error => Error.New( error ));
        }

        public Either<Error, Unit> Save<T>( Stream stream, T toSave ) {
            return
                Prelude.Try( () => {
                    var streamWriter = new StreamWriter( stream );

                    streamWriter.Write( JsonSerializer.Serialize( toSave, mOptions ));
                    streamWriter.Flush();

                    return Unit.Default;
                } )
                    .ToEither( Error.New );
        }

        public EitherAsync<Error, Unit> SaveAsync<T>( Stream stream, T toSave ) {
            return
                Prelude.TryAsync( async () => {
                    var streamWriter = new StreamWriter( stream );

                    await streamWriter.WriteAsync( JsonSerializer.Serialize( toSave, mOptions )).ConfigureAwait( false );
                    await streamWriter.FlushAsync();

                    return Unit.Default;
                } )
                    .ToEither( error => Error.New( error ));
        }
    }
}
