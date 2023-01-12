using System.Threading.Tasks;
using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Ui;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class DeleteProjectEffect : Effect<DeleteProjectAction> {
        private readonly IDispatcher    mDispatcher;
        private readonly UiFacade       mUiFacade;

        public DeleteProjectEffect( IDispatcher dispatcher, UiFacade uiFacade ) {
            mDispatcher = dispatcher;
            mUiFacade = uiFacade;
        }

        public override async Task HandleAsync( DeleteProjectAction action, IDispatcher dispatcher ) {
            var confirmation = await mUiFacade.ConfirmAction( "Confirm Deletion", 
                $"Are you sure you want to delete the Project titled '{action.Project.Name}'?" );

            if(!confirmation.Canceled ) {
                mDispatcher.Dispatch( new DeleteProjectSubmit( new DeleteProjectRequest( action.Project )));
            } 
        }
    }
}
