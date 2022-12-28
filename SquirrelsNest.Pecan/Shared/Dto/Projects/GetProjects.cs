using SquirrelsNest.Pecan.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Shared.Dto.Projects {
    public class GetProjectsResponse : BaseResponse {
        public List<SnCompositeProject> Projects { get; }

        [JsonConstructor]
        public GetProjectsResponse( bool succeeded, string message, List<SnCompositeProject> projects ) :
            base( succeeded, message ) {
            Projects = projects;
        }

        public GetProjectsResponse() {
            Projects = new List<SnCompositeProject>();
        }

        public GetProjectsResponse( List<SnCompositeProject> projects ) {
            Projects = new List<SnCompositeProject>( projects );
        }

        public GetProjectsResponse( Exception ex ) :
            base( ex ) {
            Projects = new List<SnCompositeProject>();
        }
    }
}
