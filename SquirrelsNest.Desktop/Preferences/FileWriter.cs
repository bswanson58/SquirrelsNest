using System.IO;
using System.Text.Json;

namespace SquirrelsNest.Desktop.Preferences {
    public interface IFileWriter {
        T       Load<T>( string filePath ) where T : new();
        void    Save<T>( string filePath, T toSave );
    }

    public class FileWriter : IFileWriter {
        public T Load<T>( string filePath ) where T: new() {
            if(!File.Exists( filePath )) {
                return new T();
            }

            return JsonSerializer.Deserialize<T>( File.ReadAllText( filePath )) ?? new T();
        } 

        public void Save<T>( string filePath, T settings ) => 
            File.WriteAllText( filePath, JsonSerializer.Serialize( settings ));
    }
}
