using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.Core.Environment {
    internal class ApplicationConstants : IApplicationConstants {
        public string  ApplicationName          => "SquirrelsNest";
        public string  CompanyName              => "Secret Squirrel Software";

        public string  ConfigurationDirectory   => "Configuration";
        public string  TemplateDirectory        => "Templates";
        public string  DatabaseDirectory        => "Database";
        public string  LogFileDirectory         => "Logs";

        public string  DatabaseFileName         => "SquirrelsNestDB";
        public string  ProjectTemplateExtension => ".pt";
    }
}
