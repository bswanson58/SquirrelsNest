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
using SquirrelsNest.Core.CompositeBuilders;
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

        private Either<Error, SnIssue> UpdateIssue( SnIssue issue, CompositeProject project, UpdateIssueInput updates ) {
            var retValue = issue;

            foreach( var operation in updates.Operations ) {
                switch ( operation.Path ) {
                    case IssueUpdatePath.Title:
                        retValue = retValue.With( title: operation.Value );
                        break;

                    case IssueUpdatePath.Description:
                        retValue = retValue.With( description: operation.Value );
                        break;

                    case IssueUpdatePath.AssignedToId:
                        var user = project.Users.FirstOrDefault( u => u.EntityId.Value.Equals( operation.Value ));

                        if( user != null ) {
                            retValue = retValue.With( assignedTo: user.EntityId );
                        }
                        else {
                            return Error.New( "Invalid AssignedTo User" );
                        }

                        break;

                    case IssueUpdatePath.ComponentId:
                        var component = project.Components.FirstOrDefault( c => c.EntityId.Value.Equals( operation.Value ));

                        if( component != null ) {
                            retValue = retValue.With( component );
                        }
                        else {
                            return Error.New( "Invalid Component" );
                        }

                        break;

                    case IssueUpdatePath.IssueTypeId:
                        var issueType = project.IssueTypes.FirstOrDefault( i => i.EntityId.Value.Equals( operation.Value ));

                        if( issueType != null ) {
                            retValue = retValue.With( issueType );
                        }
                        else {
                            return Error.New( "Invalid Issue Type" );
                        }

                        break;

                    case IssueUpdatePath.ReleaseId:
                        var release = project.Releases.FirstOrDefault( r => r.EntityId.Value.Equals( operation.Value ));

                        if( release != null ) {
                            retValue = retValue.With( release );
                        }
                        else {
                            return Error.New( "Invalid Release" );
                        }

                        break;

                    case IssueUpdatePath.WorkflowStateId:
                        var state = project.WorkflowStates.FirstOrDefault( s => s.EntityId.Value.Equals( operation.Value ));

                        if( state != null ) {
                            retValue = retValue.With( state );
                        }
                        else {
                            return Error.New( "Invalid Workflow State" );
                        }

                        break;

                    default:
                        return Error.New( "Invalid Issue update path" );
                }
            }

            return retValue;
        }

        [Authorize( Policy = "IsUser" )]
        public async Task<UpdateIssuePayload> UpdateIssue( UpdateIssueInput updateInput ) {
            var issueId = EntityId.For( updateInput.IssueId );
            if( issueId.IsNone ) {
                return new UpdateIssuePayload( "Invalid issue ID to be updated" );
            }

            var currentIssue = await issueId.MapAsync( id => mIssueProvider.GetIssue( id ));
            var currentProject = await currentIssue.BindAsync( i => mProjectProvider.GetProject( i.ProjectId ));
            var compositeProject = await currentProject.BindAsync( p => mProjectBuilder.BuildCompositeProject( p ));
            var updatedIssue = compositeProject.Bind( p => currentIssue.Bind( i => UpdateIssue( i, p, updateInput )));
            var updateResult = await updatedIssue.MapAsync( i => mIssueProvider.UpdateIssue( i ));

            if( updateResult.IsLeft ) {
                return new UpdateIssuePayload( updateResult.LeftToList().FirstOrDefault());
            }

            var compositeIssue = await updatedIssue.BindAsync( i => mIssueBuilder.BuildCompositeIssue( i ));

            return compositeIssue.Match( i => new UpdateIssuePayload( i ), e => new UpdateIssuePayload( e ));
        }
    }
}
