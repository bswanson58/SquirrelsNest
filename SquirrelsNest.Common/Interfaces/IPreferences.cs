namespace SquirrelsNest.Common.Interfaces {
    public interface IPreferences<T> where T : new() {
        T       Current { get; }
        void    Save( T preferences );
    }
}
