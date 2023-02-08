using System;
using System.Globalization;
using System.Reflection;

// from: https://rmauro.dev/add-build-time-to-your-csharp-assembly/
//
// Requires the following entry in the .csproj file:
//
// <Project Sdk="Microsoft.NET.Sdk">
//    <!--existing tags-->
//
//    <PropertyGroup>
//       <SourceRevisionId>build$([System.DateTime]::Now.ToString("yyyy-MM-ddTHH:mm:ss:fffZ"))</SourceRevisionId>     <-- this line
//       <SourceRevisionId>build$([System.DateTime]::UtcNow.ToString("yyyy-MM-ddTHH:mm:ss:fffZ"))</SourceRevisionId>  <-- or this line for UTC time
//    </PropertyGroup>
//    
//    <!--existing tags-->
//
// </Project>

namespace SquirrelsNest.Desktop.Platform {
    public static class ApplicationInformation {
        public static DateTime GetLinkerTime( Assembly assembly ) {
            const string buildVersionMetadataPrefix = "+build";

            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            if( attribute?.InformationalVersion != null ) {
                var value = attribute.InformationalVersion;
                var index = value.IndexOf( buildVersionMetadataPrefix, StringComparison.Ordinal );

                if( index > 0 ) {
                    value = value[( index + buildVersionMetadataPrefix.Length )..];

                    return DateTime.ParseExact( value, "yyyy-MM-ddTHH:mm:ss:fffZ", CultureInfo.InvariantCulture );
                }
            }

            return default;
        }

        public static Version ? GetVersion( Assembly assembly ) => assembly.GetName().Version;
    }
}
