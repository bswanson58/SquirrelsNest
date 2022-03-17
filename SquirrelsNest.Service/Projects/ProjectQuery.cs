using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Dto;

namespace SquirrelsNest.Service.Projects {
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

        public async Task<IEnumerable<ClProject>> GetProjects() {
            var user = await GetUser();
            var projects = await user.BindAsync( async u => await mProjectProvider.GetProjects( u ));
            var clProjects = projects.Map( list => list.Select( ProjectExtensions.From ));

            return clProjects.Match( list => list, _ => new List<ClProject>());
        }
    }
}
