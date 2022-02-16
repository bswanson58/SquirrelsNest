using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
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
    public class ProjectReleasesTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;
        private readonly List<SnRelease>        mReleases;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public ProjectReleasesTests() {
            DateTimeProvider.SetProvider( new TestTimeProvider( new DateTime( 2020, 11, 30, 7, 17, 55 )));

            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            mReleases = new List<SnRelease>();

            DeleteDatabase();
        }

        private ProjectProvider CreateSut() {
            AddSomeReleases();

            return new ProjectProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        private void AddSomeReleases() {
            using var releaseProvider = new ReleaseProvider( new DatabaseProvider( mEnvironment, mConstants ));

            releaseProvider.AddRelease( new SnRelease( "release 1" )).Do( release => mReleases.Add( release ));
            releaseProvider.AddRelease( new SnRelease( "release 2" )).Do( release => mReleases.Add( release ));
            releaseProvider.AddRelease( new SnRelease( "release 3" )).Do( release => mReleases.Add( release ));
        }

        [Fact]
        public void ProjectCanAddRelease() {
            using var sut = CreateSut();
            var project = new SnProject( "project name", "prefix" );
            var release = mReleases[1];
            project = project.WithReleaseAdded( release );

            sut.AddProject( project ).Do( p => project = p );
            var result = sut.GetProject( project.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message}" ));
            result.IfRight( p => p.Should().BeEquivalentTo( project, "Project can have release added" ));
            result.IfRight( p => p.Releases.Count.Should().Be( 1, "Project should have 1 release" ));
        }

        [Fact]
        public void ProjectCanHaveMultipleReleases() {
            using var sut = CreateSut();
            var project = new SnProject( "project name", "prefix" );
            var release1 = mReleases[0];
            var release2 = mReleases[1];
            project = project.WithReleaseAdded( release1 );
            project = project.WithReleaseAdded( release2 );

            sut.AddProject( project ).Do( p => project = p );
            var result = sut.GetProject( project.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message}" ));
            result.IfRight( p => p.Should().BeEquivalentTo( project, "Project can have multiple releases added" ));
            result.IfRight( p => p.Releases.Count.Should().Be( 2, "Project should have 2 releases" ));
        }

        [Fact]
        public void ProjectCannotHaveSameReleaseAdded() {
            AddSomeReleases();
            var project = new SnProject( "project name", "prefix" );
            var release1 = mReleases[0];

            project = project.WithReleaseAdded( release1 );

            Assert.Throws<ApplicationException>(() => project = project.WithReleaseAdded( release1 ));
        }

        [Fact]
        public void ProjectCanHaveReleaseDeleted() {
            using var sut = CreateSut();
            var project = new SnProject( "project name", "prefix" );
            var release1 = mReleases[1];
            var release2 = mReleases[0];
            project = project.WithReleaseAdded( release1 );
            project = project.WithReleaseAdded( release2 );

            sut.AddProject( project ).Do( p => project = p );
            project = project.WithReleaseRemoved( project.Releases.First());
            sut.UpdateProject( project );
            var result = sut.GetProject( project.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message}" ));
            result.IfRight( p => p.Should().BeEquivalentTo( project, "Project can have release deleted" ));
            result.IfRight( p => p.Releases.Count.Should().Be( 1, "Project should have 1 release" ));
        }

        [Fact]
        public void ProjectThrowsIfCanNotRemoveRelease() {
            using var sut = CreateSut();
            var project = new SnProject( "project name", "prefix" );
            var release1 = mReleases[1];
            var release2 = mReleases[2];
            project = project.WithReleaseAdded( release1 );
            project = project.WithReleaseAdded( release2 );

            // Adding the project updates the release with database identifiers.
            sut.AddProject( project ).Do( p => project = p );

            Assert.Throws<ApplicationException>(() => project.WithReleaseRemoved( release1 ));
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
