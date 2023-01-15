using System.Collections.Generic;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.ProjectTemplates.Actions {
    public class RequestProjectTemplatesAction {
    }

    public class RequestProjectTemplatesSuccess {
        public  List<SnProjectTemplate> Templates { get; }

        public RequestProjectTemplatesSuccess( List<SnProjectTemplate> templates ) {
            Templates = templates;
        }
    }

    public class RequestProjectTemplatesFailure : FailureAction {
        public RequestProjectTemplatesFailure( string message )
            : base( message ) { }
    }
}
