using SquirrelsNest.Pecan.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Shared.Dto.Projects {
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
