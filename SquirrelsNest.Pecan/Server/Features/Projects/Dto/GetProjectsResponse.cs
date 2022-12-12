using System.Collections.Generic;
using SquirrelsNest.Pecan.Server.Models.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Projects.Dto {
    public class GetProjectsResponse {
        public List<SnProject>  Projects { get; }

        public GetProjectsResponse( IEnumerable<SnProject> list ) {
            Projects = new List<SnProject>( list );
        }
    }
}
