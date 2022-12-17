using System;
using System.Collections.Generic;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto {
    public class GetProjectsResponse : BaseResponse {
        public List<SnProject>  Projects { get; }

        public GetProjectsResponse() {
            Projects = new List<SnProject>();
        }

        public GetProjectsResponse( List<SnProject> projects ) {
            Projects = projects;
        }

        public GetProjectsResponse( Exception ex ) :
            base( ex ) {
            Projects = new List<SnProject>();
        }
    }
}
