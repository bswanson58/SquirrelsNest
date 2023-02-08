using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.Transfer.Export;
using SquirrelsNest.Core.Transfer.Import;

namespace SquirrelsNest.Service.Controllers {
    [ApiController]
    [Route( "/transfer" )]
    public class TransferController : ControllerBase {
        private readonly IProjectProvider   mProjectProvider;
        private readonly IExportManager     mExportManager;
        private readonly IImportManager     mImportManager;
        private readonly IUserProvider      mUserProvider;

        public TransferController( IExportManager exportManager, IImportManager importManager,
                                   IProjectProvider projectProvider,  IUserProvider userProvider ) {
            mExportManager = exportManager;
            mProjectProvider = projectProvider;
            mImportManager = importManager;
            mUserProvider = userProvider;
        }

        private async Task<Either<Error, SnUser>> GetUser() {
            var users = await mUserProvider.GetUsers();

            return users.Map( userList => userList.FirstOrDefault( SnUser.Default ));
        }

        [HttpGet( "export" )]
//        [Authorize( Policy = PolicyNames.AdminPolicy )]
        public async Task<Stream> ExportProject( string projectId ) {
            var entityId = EntityId.For( projectId );
            var project = await entityId.MapAsync( id => mProjectProvider.GetProject( id ));
            var parameters = project.Map( p => new ExportParameters( p, true ));
            var projectStream = await parameters.BindAsync( p => mExportManager.StreamProject( p ));

            return projectStream.Match( s => s, e => new MemoryStream( Encoding.UTF8.GetBytes( e.Message )));
        }

        [HttpPost( "import" )]
//        [Authorize( Policy = PolicyNames.AdminPolicy )]
        public async Task<IActionResult> ImportProject( string projectName, IFormFile file ) {
            var importParameters = new ImportParameters( projectName );
            var user = await GetUser();
            var project = await user.BindAsync( u => mImportManager.ImportProject( file.OpenReadStream(), importParameters, u ));

            return project.Match( _ => Ok(), e => StatusCode( 500, e.Message ) as IActionResult );
        }
    }
}
