namespace SquirrelsNest.Common.Interfaces {
    public interface ILog {
        void	LogException( string message, Exception exception );
        void	LogMessage( string message );
    }

    public interface IApplicationLog : ILog {
        void	ApplicationStarting();
        void	ApplicationExiting();
    }
}
