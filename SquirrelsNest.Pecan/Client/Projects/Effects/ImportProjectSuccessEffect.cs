using System.Threading.Tasks;
using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Ui.Store;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class ImportProjectSuccessEffect : Effect<ImportProjectSuccess> {
        private readonly UiFacade   mUiFacade;

        public ImportProjectSuccessEffect( UiFacade uiFacade ) {
            mUiFacade = uiFacade;
        }

        public override async Task HandleAsync( ImportProjectSuccess action, IDispatcher dispatcher ) {
            await mUiFacade.DisplayMessage( "Import Project", "The project was successfully imported." );
        }
    }
}
