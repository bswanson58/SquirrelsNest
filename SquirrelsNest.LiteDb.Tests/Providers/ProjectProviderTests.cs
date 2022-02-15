using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using LiteDB;
using NSubstitute;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;
using SquirrelsNest.LiteDb.Tests.Database;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class ProjectProviderTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;
        private readonly DateTime               mTestTime;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public ProjectProviderTests() {
            mTestTime = new DateTime( 2020, 12, 17, 6, 33, 44 );

            DateTimeProvider.SetProvider( new TestTimeProvider( mTestTime ));

            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            DeleteDatabase();
        }

        private ProjectProvider CreateSut() {
            return new ProjectProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        [Fact]
        public void ProjectCanBeStored() {
            using var sut = CreateSut();
            var project = new SnProject( ObjectId.NewObjectId().ToString(), String.Empty, "Name", "description", DateTimeProvider.Instance.CurrentDate, "repository", "prefix", 1, 
                                         new List<SnRelease>() );

            var result = sut.AddProject( project );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding a project" ));
            result.IsRight.Should().BeTrue( "projects should be addable" );
        }

        [Fact]
        public void NewProjectCanBeRetrieved() {
            var project = new SnProject( "name", "prefix" );
            using var sut = CreateSut();

            sut.AddProject( project );
            var result = sut.GetProject( project.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a project" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( project, option => option.Excluding( e => e.DbId ),
                "retrieved project should match stored project" ));
        }

        [Fact]
        public void NewProjectShouldHaveCurrentInceptionDate() {
            var project = new SnProject( "name", "prefix" );
            using var sut = CreateSut();

            sut.AddProject( project );
            var result = sut.GetProject( project.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a project" ));
            result.Do( retrieved => retrieved.Inception.Should().Be( DateOnly.FromDateTime( mTestTime ), "inception time should match test time" ));
        }

        [Fact]
        public void ProjectShouldUpdateSuccessfully() {
            var project = new SnProject( "name", "prefix" );
            using var sut = CreateSut();

            sut.AddProject( project ).Do( p => project = p );
            project = project.With( description: "new description" );
            var result = sut.UpdateProject( project );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating project" ));
        }

        [Fact]
        public void ProjectShouldBeUpdated() {
            var project = new SnProject( "name", "prefix" );
            using var sut = CreateSut();

            sut.AddProject( project ).Do( p => project = p );
            project = project.With( description: "new description" );
            // ReSharper disable once AccessToDisposedClosure
            var result = sut.UpdateProject( project ).Bind( _ => sut.GetProject( project.EntityId ));

            result.IfLeft( error => error.Should().BeNull( "updating project should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( project, "retrieved project should match update" ));
        }

        [Fact]
        public void ProjectCanBeDeleted() {
            var project = new SnProject( "name", "prefix" );
            using var sut = CreateSut();

            sut.AddProject( project ).Do( p => project = p );
            var result = sut.DeleteProject( project );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete entity" ));
            result.IsRight.Should().BeTrue( "entity should be deleted without error" );
        }

        [Fact]
        public void ProjectsCanBeListed() {
            var project = new SnProject( "one", "prefix" );
            using var sut = CreateSut();

            sut.AddProject( project );
            sut.AddProject( project.With( name: "two" ));
            sut.AddProject( project.With( name: "three" ));
            sut.AddProject( project.With( name: "four" ));

            var result = sut.GetProjects();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred with getting project list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 projects were added" ));
        }

        private void DeleteDatabase() {
            if( File.Exists( DatabaseFile )) {
                File.Delete( DatabaseFile );
            }
        }

        public void Dispose() {
            DeleteDatabase();
        }
    }
}
