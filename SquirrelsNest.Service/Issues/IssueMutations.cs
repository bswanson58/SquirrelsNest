using System;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
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
        private readonly IProjectBuilder    mProjectBuilder;
        private readonly IIssueProvider     mIssueProvider;
        private readonly IIssueBuilder      mIssueBuilder;

        public IssueMutations( IUserProvider userProvider, IProjectProvider projectProvider, IProjectBuilder projectBuilder,
                               IIssueProvider issueProvider, IIssueBuilder issueBuilder ) {
            mUserProvider = userProvider;
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
            mIssueProvider = issueProvider;
            mIssueBuilder = issueBuilder;
        }

        private async Task<Either<Error, SnUser>> GetUser() {
            var users = await mUserProvider.GetUsers();

            return users.Map( userList => userList.FirstOrDefault( SnUser.Default ));
        }

        [Authorize( Policy = "IsUser" )]
        public async Task<AddIssuePayload> AddIssue( AddIssueInput issueInput ) {
            var user = await GetUser();
            var userId = EntityId.Default;

            user.IfRight( u => {
                userId = u.EntityId;
            });

            if( user.IsLeft ) {
                return new AddIssuePayload( "The user could not be determined" );
            }

            var projectId = EntityId.For( issueInput.ProjectId );
            if( projectId.IsNone ) {
                return new AddIssuePayload( "A proper project ID was not specified" );
            }

            var project = await projectId.MapAsync( id => mProjectProvider.GetProject( id ));
            var newIssue = project.Map( p => new SnIssue( issueInput.Title, p.NextIssueNumber, p.EntityId ))
                                  .Map( i => i.With( description: issueInput.Description ))
                                  .Map( i => i.With( assignedTo: userId ));

            var compositeProject = await project.BindAsync( p => mProjectBuilder.BuildCompositeProject( p ));
            if( compositeProject.IsLeft ) {
                return new AddIssuePayload( "The composite project could not be built for the issue to be added" );
            }

            newIssue = newIssue.Bind( issue => compositeProject.Map( composite => {
                var issueType = composite.IssueTypes.FirstOrDefault( it => it.EntityId.Value.Equals( issueInput.IssueTypeId ));

                return issueType != null ? issue.With( issueType ) : issue;
            }));

            var addedIssue = await newIssue.BindAsync( issue => mIssueProvider.AddIssue( issue ));

            if( addedIssue.IsRight ) {
                var result = await project.BindAsync( p => mProjectProvider.UpdateProject( p.WithNextIssueNumber()));

                if( result.IsLeft ) {
                    return result.Match( _ => new AddIssuePayload( String.Empty ), e => new AddIssuePayload( e ));
                }
            }

            var retValue = await addedIssue.BindAsync( issue => mIssueBuilder.BuildCompositeIssue( issue ));
               
            return retValue.Match( i => new AddIssuePayload( i ), e => new AddIssuePayload( e ));
        }

        [Authorize( Policy = "IsUser" )]
        public async Task<EditIssuePayload> EditIssue( EditIssueInput issue ) {
            var issueId = EntityId.For( issue.IssueId );
            if( issueId.IsNone ) {
                return new EditIssuePayload( "Invalid issue ID to be edited" );
            }

            var currentIssue = await issueId.MapAsync( id => mIssueProvider.GetIssue( id ));
            var currentProject = await currentIssue.BindAsync( i => mProjectProvider.GetProject( i.ProjectId ));
            var compositeProject = await currentProject.BindAsync( p => mProjectBuilder.BuildCompositeProject( p ));

            if( compositeProject.IsLeft ) {
                return new EditIssuePayload( "Composite project for edited issue could not be built" );
            }

            var updatedIssue = currentIssue.Map( newIssue => {
                var newTitle = String.IsNullOrWhiteSpace( issue.Title ) ? newIssue.Title : issue.Title;

                newIssue = newIssue.With( title: newTitle, description: issue.Description );

                compositeProject.Do( project => {
                    var user = project.Users.FirstOrDefault( u => u.EntityId.Value.Equals( issue.AssignedToId ));
                    if( user != null ) {
                        newIssue = newIssue.With( assignedTo:user.EntityId );
                    }

                    var it = project.IssueTypes.FirstOrDefault( i => i.EntityId.Value.Equals( issue.IssueTypeId ));
                    if( it != null ) {
                        newIssue = newIssue.With( it );
                    }

                    var comp = project.Components.FirstOrDefault( c => c.EntityId.Value.Equals( issue.ComponentId ));
                    if( comp != null ) {
                        newIssue = newIssue.With( comp );
                    }

                    var release = project.Releases.FirstOrDefault( r => r.EntityId.Value.Equals( issue.ReleaseId ));
                    if( release != null ) {
                        newIssue = newIssue.With( release );
                    }

                    var state = project.WorkflowStates.FirstOrDefault( s => s.EntityId.Value.Equals( issue.WorkflowStateId ));
                    if( state != null ) {
                        newIssue = newIssue.With( state );
                    }
                });

                return newIssue;
            });
            
            var updateResult = await updatedIssue.MapAsync( i => mIssueProvider.UpdateIssue( i ));

            if( updateResult.IsLeft ) {
                return new EditIssuePayload( "Issue edit failed" );
            }

            var compositeIssue = await updatedIssue.BindAsync( i => mIssueBuilder.BuildCompositeIssue( i ));

            return compositeIssue.Match( i => new EditIssuePayload( i ), e => new EditIssuePayload( e ));
        }
    }
}
