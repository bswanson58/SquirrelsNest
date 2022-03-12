using System.Linq;
using FluentAssertions;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Database;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    [Collection(nameof(SequentialCollection))]
    public class ProjectProviderTests : BaseProviderTests {

        private IProjectProvider CreateSut() {
            return new ProjectProvider( mAssociationProvider, mIssueProvider, mProjectProvider, mComponentProvider, mIssueTypeProvider,
                                        mReleaseProvider, mStateProvider );
        }

        [Fact]
        public async void ComponentsAreDeletedWithProject() {
            var projects = await CreateSomeProjects( 3 );
            var sut = CreateSut();
            await CreateSomeComponents( 3, projects[1]);
            await CreateSomeComponents( 2 );

            var result = await sut.DeleteProject( projects[1]);
            var list = await mComponentProvider.GetComponents( projects[1]);

            result.IfLeft( error => error.Should().BeNull( "error deleting the project" ));
            list.IfLeft( error => error.Should().BeNull( "error retrieving components" ));
            list.IfRight( comps => comps.Count().Should().Be( 0, "deleting a project should have deleted all components"  ));
        }

        [Fact]
        public async void IssueTypesAreDeletedWithProject() {
            var projects = await CreateSomeProjects( 3 );
            var sut = CreateSut();
            await CreateSomeIssueTypes( 5, projects[1]);
            await CreateSomeComponents( 2 );

            var result = await sut.DeleteProject( projects[1]);
            var list = await mIssueTypeProvider.GetIssues( projects[1]);

            result.IfLeft( error => error.Should().BeNull( "error deleting the project" ));
            list.IfLeft( error => error.Should().BeNull( "error retrieving issue types" ));
            list.IfRight( comps => comps.Count().Should().Be( 0, "deleting a project should have deleted all issue types"  ));
        }

        [Fact]
        public async void StatesAreDeletedWithProject() {
            var projects = await CreateSomeProjects( 3 );
            var sut = CreateSut();
            await CreateSomeStates( 2, projects[1]);
            await CreateSomeStates( 3 );
            await CreateSomeComponents( 2 );

            var result = await sut.DeleteProject( projects[1]);
            var list = await mStateProvider.GetStates( projects[1]);

            result.IfLeft( error => error.Should().BeNull( "error deleting the project" ));
            list.IfLeft( error => error.Should().BeNull( "error retrieving workflow states" ));
            list.IfRight( comps => comps.Count().Should().Be( 0, "deleting a project should have deleted all workflow states"  ));
        }

        [Fact]
        public async void ReleasesAreDeletedWithProject() {
            var projects = await CreateSomeProjects( 3 );
            var sut = CreateSut();
            await CreateSomeReleases( 1, projects[1]);
            await CreateSomeReleases( 3 );
            await CreateSomeComponents( 2 );

            var result = await sut.DeleteProject( projects[1]);
            var list = await mReleaseProvider.GetReleases( projects[1]);

            result.IfLeft( error => error.Should().BeNull( "error deleting the project" ));
            list.IfLeft( error => error.Should().BeNull( "error retrieving project releases" ));
            list.IfRight( comps => comps.Count().Should().Be( 0, "deleting a project should have deleted all releases"  ));
        }

        [Fact]
        public async void IssuesAreDeletedWithProject() {
            var projects = await CreateSomeProjects( 3 );
            var sut = CreateSut();
            await CreateSomeIssues( 7, projects[1]);
            await CreateSomeReleases( 3, projects[2]);
            await CreateSomeComponents( 2, projects[0]);

            var result = await sut.DeleteProject( projects[1]);
            var list = await mIssueProvider.GetIssues( projects[1]);

            result.IfLeft( error => error.Should().BeNull( "error deleting the project" ));
            list.IfLeft( error => error.Should().BeNull( "error retrieving project issues" ));
            list.IfRight( comps => comps.Count().Should().Be( 0, "deleting a project should have deleted all issues"  ));
        }
    }
}
