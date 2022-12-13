using System.Collections.Generic;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto {
    public class GetProjectsResponse {
        public List<SnProject>  Projects { get; }

        public GetProjectsResponse( List<SnProject> projects ) {
            Projects = projects;
        }
    }
}
