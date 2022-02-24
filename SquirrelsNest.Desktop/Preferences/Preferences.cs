namespace SquirrelsNest.Desktop.Preferences {
    public interface IPreferences<T> where T : new() {
        T   Current { get; set; }
    }

    public class Preferences<T> : IPreferences<T> where T : new() {
        private readonly IPreferencesManager<T>     mPreferences;

        public Preferences( IPreferencesManager<T> preferences ) {
            mPreferences = preferences;
        }

        public T Current {
            get => mPreferences.Load();
            set => mPreferences.Save( value );
        }
    }
}
