using System.Threading.Tasks;
using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Ui.Store;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class CreateProjectTemplateSuccessEffect : Effect<CreateProjectTemplateSuccess> {
        private readonly UiFacade   mUiFacade;

        public CreateProjectTemplateSuccessEffect( UiFacade uiFacade ) {
            mUiFacade = uiFacade;
        }

        public override async Task HandleAsync( CreateProjectTemplateSuccess action, IDispatcher dispatcher ) {
            var message = $"The project template named '{action.Template.TemplateName}' was successfully created!";

            await mUiFacade.DisplayMessage( "Success", message );
        }
    }
}
