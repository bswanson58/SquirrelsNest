using System.Linq;
using SquirrelsNest.Service.Controllers.Dto;

namespace SquirrelsNest.Service.Support {
    public static class QueryableExtensions {
        public static IQueryable<T> Paginate<T>( this IQueryable<T> queryable, PageInfo pageInfo ) {
            return queryable
                .Skip(( pageInfo.Page - 1 ) * pageInfo.RecordsPerPage )
                .Take( pageInfo.RecordsPerPage );
        }
    }
}
