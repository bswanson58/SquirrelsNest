using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto {
    public class GetProjectsResponse : BaseResponse {
        public List<SnProject>  Projects { get; }

        [JsonConstructor]
        public GetProjectsResponse( bool succeeded, string message, List<SnProject> projects ) :
            base( succeeded, message ) {
            Projects = projects;
        }

        public GetProjectsResponse() {
            Projects = new List<SnProject>();
        }

        public GetProjectsResponse( List<SnProject> projects ) {
            Projects = new List<SnProject>( projects );
        }

        public GetProjectsResponse( Exception ex ) :
            base( ex ) {
            Projects = new List<SnProject>();
        }
    }
}
