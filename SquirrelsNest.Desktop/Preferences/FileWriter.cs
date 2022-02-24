using System.IO;
using System.Text.Json;

namespace SquirrelsNest.Desktop.Preferences {
    public interface IFileWriter<T> {
        T       Load( string filePath );
        void    Save( string filePath, T toSave );
    }

    public class FileWriter<T> : IFileWriter<T> where T : new() {
        public T Load( string filePath ) => 
            JsonSerializer.Deserialize<T>( File.ReadAllText( filePath )) ?? new T();

        public void Save( string filePath, T settings ) => 
            File.WriteAllText( filePath, JsonSerializer.Serialize( settings ));
    }

}
