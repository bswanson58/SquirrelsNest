using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace SquirrelsNest.Service.Support {
    public static class HttpContextExtensions {
        public static async Task InsertParametersPaginationInHeader<T>( this HttpContext httpContext, IQueryable<T> queryable ) {
            if( httpContext == null ) {
                throw new ArgumentNullException( nameof( httpContext ));
            }

            double count = await queryable.CountAsync();

            httpContext.Response.Headers.Add( "totalAmountOfRecords", count.ToString( CultureInfo.InvariantCulture ));
        }
    }
}
