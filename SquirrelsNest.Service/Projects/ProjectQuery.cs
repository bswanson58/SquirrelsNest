using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Dto;

namespace SquirrelsNest.Service.Projects {
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExtendObjectType(OperationTypeNames.Query)]
    public class ProjectQuery {
        private readonly IUserProvider      mUserProvider;
        private readonly IProjectProvider   mProjectProvider;

        public ProjectQuery( IUserProvider userProvider, IProjectProvider projectProvider ) {
            mUserProvider = userProvider;
            mProjectProvider = projectProvider;
        }

        private async Task<Either<Error, SnUser>> GetUser() {
            var users = await mUserProvider.GetUsers();

            return users.Map( userList => userList.FirstOrDefault( SnUser.Default ));
        }

        // ReSharper disable once UnusedMember.Global
        [UsePaging(MaxPageSize = 15, IncludeTotalCount = true)]
        [Authorize( Policy = "IsUser" )]
        public async Task<IEnumerable<ClProject>> AllProjects() {
            var user = await GetUser();
            var projects = await user.BindAsync( async u => await mProjectProvider.GetProjects( u ));
            var clProjects = projects.Map( list => list.Select( ProjectExtensions.ToCl ));

            return clProjects.Match( list => list, _ => new List<ClProject>());
        }
    }
}
