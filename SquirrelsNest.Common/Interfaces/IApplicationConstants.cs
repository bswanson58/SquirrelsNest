namespace SquirrelsNest.Common.Interfaces {
    public interface IApplicationConstants {
        string  ApplicationName { get; }
        String  CompanyName { get; }

        String  ConfigurationDirectory { get; }
        String  DatabaseDirectory { get; }
        String  DatabaseFileName { get; }
        String  LogFileDirectory { get; }
    }
}
