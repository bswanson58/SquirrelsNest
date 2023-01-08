using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Shared.Dto;

namespace SquirrelsNest.Pecan.Server.Database.Support {
    public class PagedList<T> : List<T> {
        public  PageInformation     PageInformation { get; }

        private PagedList( List<T> items, int count, PageRequest pageRequest ) {
            PageInformation = new PageInformation( pageRequest, count );

            AddRange( items );
        }

        public static async Task<PagedList<T>> CreatePagedList( IQueryable<T> source, PageRequest pageRequest, 
                                                                CancellationToken token ) {
            var count = source.Count();
            var items = await source
                .Skip(( pageRequest.PageNumber - 1 ) * pageRequest.MaxPageSize )
                .Take( pageRequest.MaxPageSize )
                .ToListAsync( token );

            return new PagedList<T>( items, count, pageRequest );
        }
    }
}
