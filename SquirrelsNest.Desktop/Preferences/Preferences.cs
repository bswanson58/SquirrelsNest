using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.Desktop.Preferences {
    public class Preferences<T> : IPreferences<T> where T : new() {
        private readonly IPreferencesHandler     mPreferences;

        public Preferences( IPreferencesHandler preferences ) {
            mPreferences = preferences;
        }

        public T Current => mPreferences.Load<T>();

        public void Save( T preferences ) {
            mPreferences.Save( preferences );
        }
    }
}
