using System;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.Types;
using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Service.Dto.Mutations;

namespace SquirrelsNest.Service.Issues {
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class IssueMutations {
        private readonly IUserProvider      mUserProvider;
        private readonly IProjectProvider   mProjectProvider;
        private readonly IIssueProvider     mIssueProvider;
        private readonly IIssueBuilder      mIssueBuilder;

        public IssueMutations( IUserProvider userProvider, IProjectProvider projectProvider, 
                               IIssueProvider issueProvider, IIssueBuilder issueBuilder ) {
            mUserProvider = userProvider;
            mProjectProvider = projectProvider;
            mIssueProvider = issueProvider;
            mIssueBuilder = issueBuilder;
        }

        private async Task<Either<Error, SnUser>> GetUser() {
            var users = await mUserProvider.GetUsers();

            return users.Map( userList => userList.FirstOrDefault( SnUser.Default ));
        }

        public async Task<AddIssuePayload> AddIssue( AddIssueInput issue ) {
            var user = await GetUser();
            var userId = EntityId.Default;

            user.IfRight( u => {
                userId = u.EntityId;
            });

            if( user.IsLeft ) {
                return new AddIssuePayload( "The user could not be determined" );
            }

            var projectId = EntityId.For( issue.ProjectId );

            if( projectId.IsNone ) {
                return new AddIssuePayload( "A proper project ID was not specified" );
            }

            var project = await projectId.MapAsync( id => mProjectProvider.GetProject( id ));
            var newIssue = project.Map( p => new SnIssue( issue.Title, p.NextIssueNumber, p.EntityId ))
                                  .Map( i => i.With( description: issue.Description ))
                                  .Map( i => i.With( assignedTo: userId ));
            var addedIssue = await newIssue.BindAsync( i => mIssueProvider.AddIssue( i ));

            if( addedIssue.IsRight ) {
                var result = await project.BindAsync( p => mProjectProvider.UpdateProject( p.WithNextIssueNumber()));

                if( result.IsLeft ) {
                    return result.Match( _ => new AddIssuePayload( String.Empty ), e => new AddIssuePayload( e ));
                }
            }

            var retValue = await addedIssue.BindAsync( i => mIssueBuilder.BuildCompositeIssue( i ));
               
            return retValue.Match( i => new AddIssuePayload( i ), e => new AddIssuePayload( e ));
        }
    }
}
