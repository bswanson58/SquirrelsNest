namespace SquirrelsNest.Common.Interfaces {
    public interface IApplicationConstants {
        string  ApplicationName { get; }
        string  CompanyName { get; }

        string  ConfigurationDirectory { get; }
        string  DatabaseDirectory { get; }
        string  LogFileDirectory { get; }
        string  TemplateDirectory { get; }

        string  DatabaseFileName { get; }
        string  ProjectTemplateExtension { get; }
    }
}
