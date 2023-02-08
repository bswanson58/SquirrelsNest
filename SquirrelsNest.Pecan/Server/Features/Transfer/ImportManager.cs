using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Server.Features.Auth;
using SquirrelsNest.Pecan.Server.Features.Projects;
using SquirrelsNest.Pecan.Server.Features.Transfer.Dto;
using SquirrelsNest.Pecan.Server.Support;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Transfer {
    public interface IImportManager {
        Task<SnCompositeProject>    ImportProject( Stream fromStream, ImportProjectRequest projectParameters,
                                                   SnUser user, CancellationToken cancellationToken );
    }

    public class ImportManager : IImportManager {
        private readonly IIssueProvider             mIssueProvider;
        private readonly IIssueTypeProvider         mIssueTypeProvider;
        private readonly IWorkflowStateProvider     mStateProvider;
        private readonly IReleaseProvider           mReleaseProvider;
        private readonly IProjectProvider           mProjectProvider;
        private readonly IUserProvider              mUserProvider;
        private readonly IComponentProvider         mComponentProvider;
        private readonly ICompositeProjectBuilder   mProjectBuilder;
        private readonly IUserService               mUserService;
        private readonly IStreamWriter              mStreamReader;

        public ImportManager( IIssueProvider issueProvider, IIssueTypeProvider issueTypeProvider,
                              IWorkflowStateProvider stateProvider, IReleaseProvider releaseProvider,
                              IProjectProvider projectProvider, IUserProvider userProvider,
                              IComponentProvider componentProvider, ICompositeProjectBuilder projectBuilder,
                              IStreamWriter streamReader, IUserService userService ) {
            mIssueProvider = issueProvider;
            mIssueTypeProvider = issueTypeProvider;
            mStateProvider = stateProvider;
            mReleaseProvider = releaseProvider;
            mProjectProvider = projectProvider;
            mUserProvider = userProvider;
            mComponentProvider = componentProvider;
            mProjectBuilder = projectBuilder;
            mUserService = userService;
            mStreamReader = streamReader;
        }

        private async Task CreateComponents( TransferEntities entities, TransferMap transferMap ) {
            foreach( var component in entities.Components ) {
                transferMap.Add( component,
                    await mComponentProvider.Create( component.ToNewEntity( transferMap.Project )));

            }
        }

        private async Task CreateIssueTypes( TransferEntities entities, TransferMap transferMap ) {
            foreach( var issueType in entities.IssueTypes ) {
                transferMap.Add( issueType,
                    await mIssueTypeProvider.Create( issueType.ToNewEntity( transferMap.Project )));
            }
        }

        private async Task CreateWorkflowStates( TransferEntities entities, TransferMap transferMap ) {
            foreach( var state in entities.WorkflowStates ) {
                transferMap.Add( state,
                    await mStateProvider.Create( state.ToNewEntity( transferMap.Project )));
            }
        }

        private async Task CreateReleases( TransferEntities entities, TransferMap transferMap ) {
            foreach( var release in entities.Releases ) {
                transferMap.Add( release,
                    await mReleaseProvider.Create( release.ToNewEntity( transferMap.Project )));
            }
        }

        private async Task CreateIssues( TransferEntities entities, TransferMap transferMap ) {
            foreach( var issue in entities.Issues ) {
                await mIssueProvider.Create( transferMap.Convert( issue ));
            }
        }

        private async Task AssimilateUsers( TransferEntities entities, TransferMap transferMap ) {
            var userList = await mUserProvider.GetAll();

            foreach( var user in entities.Users ) {
                var existingUser = userList.FirstOrDefault( u => u.Email.Equals( user.Email ));

                if( existingUser == null ) {
                    var createdUser = await mUserService.CreateUser( user.Email, user.DisplayName );

                    if( createdUser != null ) {
                        transferMap.Add( user, createdUser );
                    }
                }
                else {
                    transferMap.Add( user, existingUser );
                }
            }
        }

        private async Task<SnProject> CreateProject( TransferEntities entities, ImportProjectRequest parameters ) {
            var project = entities.Project
                .ToNewEntity()
                .With( name: parameters.Name, description: parameters.Description, issuePrefix: parameters.IssuePrefix );

            return await mProjectProvider.Create( project );
        }

        public async Task<SnCompositeProject> ImportProject( Stream fromStream, ImportProjectRequest projectParameters,
                                                             SnUser user, CancellationToken cancellationToken ) {
            var importEntities = await mStreamReader.LoadAsync<TransferEntities>( fromStream );
            var transferMap = new TransferMap( await CreateProject( importEntities, projectParameters ));

            await CreateComponents( importEntities, transferMap );
            await CreateIssueTypes( importEntities, transferMap );
            await CreateWorkflowStates( importEntities, transferMap );
            await CreateReleases( importEntities, transferMap );
            await AssimilateUsers( importEntities, transferMap );
            await CreateIssues( importEntities, transferMap );

            return await mProjectBuilder.BuildComposite( transferMap.Project, cancellationToken );
        }
    }
}
