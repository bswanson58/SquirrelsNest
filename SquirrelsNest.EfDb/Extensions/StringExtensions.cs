using System.Globalization;

namespace SquirrelsNest.EfDb.Extensions {
    internal static class StringExtensions {
        public static int ToDbId( this string source ) {
            if( String.IsNullOrWhiteSpace( source )) {
                return 0;
            }

            if( Int32.TryParse( source, NumberStyles.Any, CultureInfo.InvariantCulture, out var retValue )) {
                return retValue;
            }

            return 0;
        }
    }
}
