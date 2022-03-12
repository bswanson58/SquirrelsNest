using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Database;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    [Collection(nameof(SequentialCollection))]
    public class UserProjectProviderTests : BaseProviderTests {
        private IProjectProvider CreateSut() {
            return new ProjectProvider( mAssociationProvider, mIssueProvider, mProjectProvider, mComponentProvider, mIssueTypeProvider,
                mReleaseProvider, mStateProvider );
        }

        [Fact]
        public async void AddingProjectWithAssociation() {
            var users = await CreateSomeUsers( 2 );
            var sut = CreateSut();

            var addResult = await sut.AddProject( new SnProject( "project 1", "P1" ), users[1]);

            addResult.IfLeft( error => error.Should().BeNull( "creating project associated with user" ));
        }

        [Fact]
        public async void AddingProjectAddsAssociation() {
            var users = await CreateSomeUsers( 3 );
            var sut = CreateSut();
            
            await sut.AddProject( new SnProject( "Project Name", "PN" ), users[0]);
            var associationList = await mAssociationProvider.GetAssociations( users[0]);

            associationList.IfLeft( error => error.Should().BeNull( "retrieving association list" ));
            associationList.IfRight( list => list.Length().Should().Be( 1, "association should be created for user" ));
        }

        [Fact]
        public async void DeletingProjectShouldDeleteAssociation() {
            var users = await CreateSomeUsers( 2 );
            var sut = CreateSut();

            var result = await ( await sut.AddProject( new SnProject( "My Project", "MP" ), users[0]))
                .BindAsync( project => sut.DeleteProject( project, users[0]));
            var associationList = await mAssociationProvider.GetAssociations( users[0]);

            result.IfLeft( error => error.Should().BeNull( "deleting project" ));
            associationList.IfLeft( error => error.Should().BeNull( "retrieving associations" ));
            associationList.IfRight( list => list.Length().Should().Be( 0, "association should be deleted" ));
        }

        [Fact]
        public async void ProjectsForUserCanBeRetrieved() {
            var users = await CreateSomeUsers( 3 );
            var sut = CreateSut();

            await sut.AddProject( new SnProject( "Project 1", "P1" ), users[0]);
            await sut.AddProject( new SnProject( "Project 2", "P2" ), users[1]);
            await sut.AddProject( new SnProject( "Project 3", "P3" ), users[0]);
            await sut.AddProject( new SnProject( "Project 4", "P4" ), users[1]);
            await sut.AddProject( new SnProject( "Project 5", "P5" ), users[0]);

            var projectList = await sut.GetProjects( users[1]);

            projectList.IfLeft( error => error.Should().BeNull( "retrieving projects by user" ));
            projectList.IfRight( list => list.Length().Should().Be( 2, "getting projects for user" ));
        }
    }
}
