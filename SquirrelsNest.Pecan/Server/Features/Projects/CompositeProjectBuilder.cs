﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Projects {
    public interface ICompositeProjectBuilder {
        Task<SnCompositeProject>    BuildComposite( SnProject forProject, CancellationToken token );
    }

    public class CompositeProjectBuilder : ICompositeProjectBuilder {
        private readonly IComponentProvider     mComponentProvider;
        private readonly IIssueTypeProvider     mIssueTypeProvider;
        private readonly IWorkflowStateProvider mWorkflowStateProvider;
        private readonly IReleaseProvider       mReleaseProvider;
        private readonly IUserProvider          mUserProvider;

        public CompositeProjectBuilder( IComponentProvider componentProvider, IIssueTypeProvider issueTypeProvider,
                                        IWorkflowStateProvider workflowStateProvider, IReleaseProvider releaseProvider,
                                        IUserProvider userProvider ) {
            mComponentProvider = componentProvider;
            mIssueTypeProvider = issueTypeProvider;
            mWorkflowStateProvider = workflowStateProvider;
            mReleaseProvider = releaseProvider;
            mUserProvider = userProvider;
        }

        public async Task<SnCompositeProject> BuildComposite( SnProject forProject, CancellationToken token = new () ) =>
            new ( forProject,
                await mComponentProvider.GetAll( forProject ).ToListAsync( token ),
                await mIssueTypeProvider.GetAll( forProject ).ToListAsync( token ),
                await mWorkflowStateProvider.GetAll( forProject ).ToListAsync( token ),
                await mReleaseProvider.GetAll( forProject ).ToListAsync( token ),
                new List<SnUser>( await mUserProvider.GetAll()));
    }
}
