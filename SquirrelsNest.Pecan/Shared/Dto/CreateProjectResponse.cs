using System;
using System.Text.Json.Serialization;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto {
    public class CreateProjectResponse : BaseResponse {
        public  SnProject ?     Project { get; }

        [JsonConstructor]
        public CreateProjectResponse( bool succeeded, string message, SnProject project ) :
            base( succeeded, message ) {
            Project = project;
        }

        public CreateProjectResponse( SnProject project ) {
            Project = project;
        }

        public CreateProjectResponse( Exception ex ) :
            base( ex ) {
            Project = null;
        }
    }
}
