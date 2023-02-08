using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.ProjectTemplates;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class CreateProjectTemplateEffect : Effect<CreateProjectTemplateAction> {
        private readonly IDialogService mDialogService;

        public CreateProjectTemplateEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( CreateProjectTemplateAction action, IDispatcher dispatcher ) {
            var request = new CreateProjectTemplateRequest( action.Project );
            var parameters = new DialogParameters { { nameof( CreateProjectTemplateDialog.Request ), request } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<CreateProjectTemplateDialog>( "Create Project Template", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Canceled ) &&
               ( dialogResult.Data is CreateProjectTemplateRequest templateRequest )) {
                dispatcher.Dispatch( new CreateProjectTemplateSubmit( templateRequest ));
            }
        }
    }
}
