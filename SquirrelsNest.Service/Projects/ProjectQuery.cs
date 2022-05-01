using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.CompositeBuilders;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Service.Dto;

namespace SquirrelsNest.Service.Projects {
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExtendObjectType(OperationTypeNames.Query)]
    public class ProjectQuery {
        private readonly IUserProvider      mUserProvider;
        private readonly IProjectProvider   mProjectProvider;
        private readonly IProjectBuilder    mProjectBuilder;

        public ProjectQuery( IUserProvider userProvider, IProjectProvider projectProvider, IProjectBuilder projectBuilder ) {
            mUserProvider = userProvider;
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
        }

        private async Task<Either<Error, SnUser>> GetUser() {
            var users = await mUserProvider.GetUsers();

            return users.Map( userList => userList.FirstOrDefault( SnUser.Default ));
        }

        private async Task<Either<Error, List<CompositeProject>>> BuildComposites( IEnumerable<SnProject> projects ) {
            var retValue = new List<CompositeProject>();

            foreach( var project in projects ) {
                var composite = await mProjectBuilder.BuildCompositeProject( project );

                if( composite.IsLeft ) {
                    return composite.Map( _ => new List<CompositeProject>());
                }

                composite.Do( c => retValue.Add( c ));

            }
            return retValue;
        }

        // ReSharper disable once UnusedMember.Global
        [UseOffsetPaging(MaxPageSize = 10, IncludeTotalCount = true)]
        [UseFiltering]
        [UseSorting]
        [Authorize( Policy = "IsUser" )]
        public async Task<IEnumerable<ClProject>> ProjectList() {
            var user = await GetUser();
            var projects = await user.BindAsync( async u => await mProjectProvider.GetProjects( u ));
            var composites = await projects.BindAsync( BuildComposites );
            var clProjects = composites.Map( list => list.Select( ProjectExtensions.ToCl ));

            return clProjects.Match( list => list, _ => new List<ClProject>());
        }
    }
}
