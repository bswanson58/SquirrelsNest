using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.Transfer.Export;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.Controllers {
    [ApiController]
    [Route( "/transfer" )]
    public class TransferController : ControllerBase {
        private readonly IProjectProvider   mProjectProvider;
        private readonly IExportManager     mExportManager;

        public TransferController( IExportManager exportManager, IProjectProvider projectProvider ) {
            mExportManager = exportManager;
            mProjectProvider = projectProvider;
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
    }
}
