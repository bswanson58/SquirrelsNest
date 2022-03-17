using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Dto;

namespace SquirrelsNest.Service.Issues {
    public class IssueQuery {
        private readonly IUserProvider      mUserProvider;
        private readonly IProjectProvider   mProjectProvider;
        private readonly IIssueProvider     mIssueProvider;

        public IssueQuery( IIssueProvider issueProvider, IProjectProvider projectProvider, IUserProvider userProvider ) {
            mIssueProvider = issueProvider;
            mProjectProvider = projectProvider;
            mUserProvider = userProvider;
        }

        private async Task<Either<Error, SnUser>> GetUser() {
            var users = await mUserProvider.GetUsers();

            return users.Map( userList => userList.FirstOrDefault( SnUser.Default ));
        }

        private async Task<Either<Error, SnProject>> GetProject() {
            var user = await GetUser();
            var projects = await user.BindAsync( async u => await mProjectProvider.GetProjects( u ));

            return projects.Map( projectList => projectList.FirstOrDefault( SnProject.Default ));
        }

        public async Task<IEnumerable<ClIssue>> GetIssues() {
            var project = await GetProject();
            var issues = await project.BindAsync( async p => await mIssueProvider.GetIssues( p ));
            var clIssues = issues.Map( list => list.Select( IssueExtensions.From ));

            return clIssues.Match( list => list, _ => new List<ClIssue>());
        }
    }
}
