using System;
using System.Linq;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Common.Values;
using Xunit;

namespace SquirrelsNest.DatabaseTests.Providers {
    public abstract class ProjectProviderTestSuite : BaseProviderTestSuite {
        private readonly DateTime   mTestTime;

        protected ProjectProviderTestSuite() {
            mTestTime = new DateTime( 2020, 12, 17, 6, 33, 44 );

            DateTimeProvider.SetProvider( new TestTimeProvider( mTestTime ));
        }

        protected abstract IDbProjectProvider CreateSut();

        [Fact]
        public async void ProjectCanBeStored() {
            using var sut = CreateSut();
            var project = new SnProject( EntityId.Default, String.Empty, "Name", "description", DateTimeProvider.Instance.CurrentDate, "repository", "prefix", 1 );

            var result = await sut.AddProject( project );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding a project" ));
            result.IsRight.Should().BeTrue( "projects should be addable" );
        }

        [Fact]
        public async void NewProjectCanBeRetrieved() {
            var project = new SnProject( "name", "prefix" );
            using var sut = CreateSut();

            await sut.AddProject( project );
            var result = await sut.GetProject( project.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a project" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( project, option => option.Excluding( e => e.DbId ),
                "retrieved project should match stored project" ));
        }

        [Fact]
        public async void NewProjectShouldHaveCurrentInceptionDate() {
            var project = new SnProject( "name", "prefix" );
            using var sut = CreateSut();

            await sut.AddProject( project );
            var result = await sut.GetProject( project.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a project" ));
            result.Do( retrieved => retrieved.Inception.Should().Be( DateOnly.FromDateTime( mTestTime ), "inception time should match test time" ));
        }

        [Fact]
        public async void ProjectShouldUpdateSuccessfully() {
            var project = new SnProject( "name", "prefix" );
            using var sut = CreateSut();

            sut.AddProject( project ).Result.Do( p => project = p );
            project = project.With( description: "new description" );
            var result = await sut.UpdateProject( project );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating project" ));
        }

        [Fact]
        public async void ProjectShouldBeUpdated() {
            var project = new SnProject( "name", "prefix" );
            using var sut = CreateSut();

            sut.AddProject( project ).Result.Do( p => project = p );
            project = project.With( description: "new description" );
            // ReSharper disable once AccessToDisposedClosure
            await sut.UpdateProject( project );
            var result = await sut.GetProject( project.EntityId );

            result.IfLeft( error => error.Should().BeNull( "updating project should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( project, "retrieved project should match update" ));
        }

        [Fact]
        public async void ProjectCanBeDeleted() {
            var project = new SnProject( "name", "prefix" );
            using var sut = CreateSut();

            sut.AddProject( project ).Result.Do( p => project = p );
            var result = await sut.DeleteProject( project );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete entity" ));
            result.IsRight.Should().BeTrue( "entity should be deleted without error" );
        }

        [Fact]
        public async void ProjectsCanBeListed() {
            var project = new SnProject( "one", "prefix" );
            using var sut = CreateSut();

            await sut.AddProject( project );
            await sut.AddProject( project.With( name: "two" ));
            await sut.AddProject( project.With( name: "three" ));
            await sut.AddProject( project.With( name: "four" ));

            var result = await sut.GetProjects();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred with getting project list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 projects were added" ));
        }
    }
}