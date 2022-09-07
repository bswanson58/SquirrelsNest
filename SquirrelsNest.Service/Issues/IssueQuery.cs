using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.CompositeBuilders;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.Issues {
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExtendObjectType(OperationTypeNames.Query)]
    public class IssueQuery {
        private readonly IUserProvider          mUserProvider;
        private readonly IProjectProvider       mProjectProvider;
        private readonly IIssueBuilder          mIssueBuilder;
        private readonly IIssueProvider         mIssueProvider;
        private readonly IHttpContextAccessor   mContextAccessor;

        public IssueQuery( IIssueBuilder issueBuilder, IIssueProvider issueProvider, IProjectProvider projectProvider, 
                           IUserProvider userProvider, IHttpContextAccessor contextAccessor ) {
            mIssueProvider = issueProvider;
            mProjectProvider = projectProvider;
            mUserProvider = userProvider;
            mIssueBuilder = issueBuilder;
            mContextAccessor = contextAccessor;
        }

        private async Task<Either<Error, SnUser>> GetUser() {
            var users = await mUserProvider.GetUsers();
            var email = mContextAccessor.HttpContext?.User.Claims.FirstOrDefault( c => c.Type == "email" )?.Value;

            return email != null ? 
                users.Map( userList => userList.FirstOrDefault( u => u.Email.Equals( email ), SnUser.Default )) : 
                SnUser.Default;
        }

        private async Task<Either<Error, SnProject>> GetProject( Option<EntityId> id ) {
            var i = id.ToEither( new Error());

            return await i.BindAsync( async projectId => {
                var user = await GetUser();
                var projects = await user.BindAsync( async u => await mProjectProvider.GetProjects( u ));

                return projects.Map( projectList => projectList.FirstOrDefault( p => p.EntityId.Equals( projectId ), SnProject.Default ));
            });
        }

        private async Task<Either<Error, IEnumerable<CompositeIssue>>> BuildIssues( IEnumerable<SnIssue> issues ) {
            var retValue = new List<CompositeIssue>();

            foreach( var issue in issues ) {
                var result = await mIssueBuilder.BuildCompositeIssue( issue );

                if( result.IsLeft ) {
                    return result.Map( _ => Enumerable.Empty<CompositeIssue>());
                }

                result.IfRight( composite => retValue.Add( composite ));
            }

            return retValue;
        }

        // ReSharper disable once UnusedMember.Global
        [UseOffsetPaging(MaxPageSize = 30, IncludeTotalCount = true)]
        [UseFiltering]
        [UseSorting]
        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<IEnumerable<ClIssue>> IssueList([ID(nameof(ClProjectBase))] string projectId ) {
            var entityId = EntityId.For( projectId );
            var project = await GetProject( entityId );
            var issues = await project.BindAsync( async p => await mIssueProvider.GetIssues( p ));
            var cpIssues = await issues.BindAsync( BuildIssues );
            var clIssues = cpIssues.Map( list => list.Select( IssueExtensions.ToCl ));

            return clIssues.Match( list => list, _ => new List<ClIssue>());
        }
    }
}
