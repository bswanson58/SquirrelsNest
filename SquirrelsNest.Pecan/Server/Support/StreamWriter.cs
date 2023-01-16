using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Server.Support {
    public interface IStreamWriter {
        T           Load<T>( Stream stream ) where T : new();
        Task<T>     LoadAsync<T>( Stream stream ) where T : new ();

        void        Save<T>( Stream stream, T toSave );
        Task        SaveAsync<T>( Stream stream, T toSave );
    }

    public class StreamWriter : IStreamWriter {
        private readonly JsonSerializerOptions  mOptions;

        public StreamWriter() {
            mOptions = new JsonSerializerOptions( JsonSerializerDefaults.Web );

            mOptions.Converters.Add( new DateOnlyConverter());
            mOptions.WriteIndented = true;
        }

        public T Load<T>( Stream stream ) where T : new() =>
            JsonSerializer.Deserialize<T>( stream, mOptions ) ?? new T();

        public async Task<T> LoadAsync<T>( Stream stream ) where T : new() =>
            await JsonSerializer.DeserializeAsync<T>( stream, mOptions ) ?? new T();

        public void Save<T>( Stream stream, T toSave ) {
            var streamWriter = new System.IO.StreamWriter( stream );

            streamWriter.Write( JsonSerializer.Serialize( toSave, mOptions ));
            streamWriter.Flush();
        }

        public async Task SaveAsync<T>( Stream stream, T toSave ) {
            var streamWriter = new System.IO.StreamWriter( stream );

            await streamWriter.WriteAsync( JsonSerializer.Serialize( toSave, mOptions )).ConfigureAwait( false );
            await streamWriter.FlushAsync();
        }
    }
}
