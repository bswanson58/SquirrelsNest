using System;
using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class AddProjectEffect : Effect<AddProjectAction> {
        private readonly IDialogService mDialogService;

        public AddProjectEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( AddProjectAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters { { nameof( AddProjectDialog.Request ), new CreateProjectRequest() } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<AddProjectDialog>( "New Project Parameters", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Canceled ) &&
               ( dialogResult.Data is CreateProjectRequest request )) {
                if(!String.IsNullOrWhiteSpace( request.ProjectTemplateName )) {
                    dispatcher.Dispatch( new AddProjectFromTemplateSubmitAction( request ));
                }
                else {
                    dispatcher.Dispatch( new AddProjectSubmitAction( request ));
                }
            }
        }
    }
}
