using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.Platform;
using SquirrelsNest.Core.Transfer.Dto;
using SquirrelsNest.Core.Validators;

namespace SquirrelsNest.Core.Transfer.Import {
    internal class ImportManager : IImportManager {
        private readonly IIssueProvider         mIssueProvider;
        private readonly IComponentProvider     mComponentProvider;
        private readonly IIssueTypeProvider     mIssueTypeProvider;
        private readonly IWorkflowStateProvider mStateProvider;
        private readonly IReleaseProvider       mReleaseProvider;
        private readonly IProjectProvider       mProjectProvider;
        private readonly IUserProvider          mUserProvider;
        private readonly IFileWriter            mFileWriter;

        public ImportManager( IIssueProvider issueProvider, IComponentProvider componentProvider, IIssueTypeProvider issueTypeProvider,
                              IWorkflowStateProvider workflowStateProvider, IReleaseProvider releaseProvider,
                              IProjectProvider projectProvider, IUserProvider userProvider, IFileWriter fileWriter ) {
            mIssueProvider = issueProvider;
            mComponentProvider = componentProvider;
            mIssueTypeProvider = issueTypeProvider;
            mStateProvider = workflowStateProvider;
            mReleaseProvider = releaseProvider;
            mProjectProvider = projectProvider;
            mUserProvider = userProvider;
            mFileWriter = fileWriter;
        }

        private async Task<Either<Error, TransferEntities>> CreateComponents( TransferEntities entities ) {
            foreach( var component in entities.Components ) {
                var result = await mComponentProvider.AddComponent( component.ToEntity());

                if( result.IsLeft ) {
                    return result.Map( _ => entities );
                }
            }

            return entities;
        }

        private async Task<Either<Error, TransferEntities>> CreateIssueTypes( TransferEntities entities ) {
            foreach( var issueType in entities.IssueTypes ) {
                var result = await mIssueTypeProvider.AddIssue( issueType.ToEntity());

                if( result.IsLeft ) {
                    return result.Map( _ => entities );
                }
            }

            return entities;
        }

        private async Task<Either<Error, TransferEntities>> CreateWorkflowStates( TransferEntities entities ) {
            foreach( var state in entities.States ) {
                var result = await mStateProvider.AddState( state.ToEntity());

                if( result.IsLeft ) {
                    return result.Map( _ => entities );
                }
            }

            return entities;
        }

        private async Task<Either<Error, TransferEntities>> CreateReleases( TransferEntities entities ) {
            foreach( var release in entities.Releases ) {
                var result = await mReleaseProvider.AddRelease( release.ToEntity());

                if( result.IsLeft ) {
                    return result.Map( _ => entities );
                }
            }

            return entities;
        }

        private TransferEntities TranslateIssueUsers( TransferEntities entities, IReadOnlyList<TrUser> currentUsers ) {
            var updatedIssues = new List<TrIssue>();

            foreach( var issue in entities.Issues ) {
                var entryUser = entities.Users.FirstOrDefault( u => u.EntityId.Equals( issue.EnteredById ), TrUser.Default );
                var newEntryUser = currentUsers.FirstOrDefault( u => u.Email.Equals( entryUser.Email ), TrUser.Default );

                var assignedUser = entities.Users.FirstOrDefault( u => u.EntityId.Equals( issue.AssignedToId ), TrUser.Default );
                var newAssignedUser = currentUsers.FirstOrDefault( u => u.Email.Equals( assignedUser.Email ), TrUser.Default );

                updatedIssues.Add( issue.With( enteredBy: newEntryUser.EntityId, assignedTo: newAssignedUser.EntityId ));
            }

            return entities.With( updatedIssues );
        }

        private async Task<Either<Error, TransferEntities>> AssimilateUsers( TransferEntities entities ) {
            var existingUsers = await mUserProvider.GetUsers();

            return await existingUsers.BindAsync( async list => {
                var userList = list.ToList();
                var resultingUsers = new List<TrUser>();

                foreach( var user in entities.Users ) {
                    var existingUser = userList.FirstOrDefault( u => u.Email.Equals( user.Email ));

                    if( existingUser == null ) {
                        var result = await mUserProvider.AddUser( user.ToEntity());

                        if( result.IsLeft ) {
                            return result.Map( _ => entities );
                        }

                        resultingUsers.Add( user );
                    }
                    else {
                        resultingUsers.Add( TrUser.From( existingUser ));
                    }
                }

                return TranslateIssueUsers( entities, resultingUsers );
            });
        }

        private async Task<Either<Error, TransferEntities>> CreateIssues( TransferEntities entities ) {
            foreach( var issue in entities.Issues ) {
                var result = await mIssueProvider.AddIssue( issue.ToEntity());

                if( result.IsLeft ) {
                    return result.Map( _ => entities );
                }
            }

            return entities;
        }

        private async Task<Either<Error, SnProject>> CreateProject( TransferEntities entities, ImportParameters parameters, SnUser forUser ) {
            var project = entities.Project.ToEntity().With( name: parameters.ProjectName );

            return await mProjectProvider.AddProject( project, forUser );
        }

        private Option<Error> ValidateImportParameters( ImportParameters parameters ) {
            var validator = new ImportParametersValidator();
            var validationResults = validator.Validate( parameters );

            if(!validationResults.IsValid ) {
                return Error.New( 0, validationResults.ToString());
            }

            return Option<Error>.None;
        }

        public async Task<Either<Error, SnProject>> ImportProject( ImportParameters parameters, SnUser forUser ) {
            var parameterErrors = ValidateImportParameters( parameters );

            if( parameterErrors.IsSome ) {
                return parameterErrors.First();
            }

            var imported = await mFileWriter.LoadAsync<TransferEntities>( parameters.ImportFilePath );

            return await imported.BindAsync( CreateComponents )
                .BindAsync( CreateIssueTypes )
                .BindAsync( CreateWorkflowStates )
                .BindAsync( CreateReleases )
                .BindAsync( AssimilateUsers )
                .BindAsync( CreateIssues )
                .BindAsync( e => CreateProject( e, parameters, forUser ));
        }

        public async Task<Either<Error, SnProject>> ImportProject( Stream stream, ImportParameters parameters, SnUser forUser ) {
            var imported = await mFileWriter.LoadAsync<TransferEntities>( stream );
            var existingProject = await imported.BindAsync( async e => {
                var projectId = EntityId.For( e.Project.EntityId );
                var project = await projectId.MapAsync( id => mProjectProvider.GetProject( id ));

                return project.IsRight ? 
                    Prelude.Left<Error, TransferEntities>( Error.New( "Imported project ID conflicts with existing project." )) :
                    e;
            });

            return await existingProject.BindAsync( CreateComponents )
                .BindAsync( CreateIssueTypes )
                .BindAsync( CreateWorkflowStates )
                .BindAsync( CreateReleases )
                .BindAsync( AssimilateUsers )
                .BindAsync( CreateIssues )
                .BindAsync( e => CreateProject( e, parameters, forUser ));
        }
    }
}
