﻿using System;
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
    public class ReleaseProviderTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;
        private readonly DateTime               mTestTime;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public ReleaseProviderTests() {
            mTestTime = new DateTime( 2022, 2, 16, 8, 46, 55 );
            DateTimeProvider.SetProvider( new TestTimeProvider( mTestTime ));

            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            DeleteDatabase();
        }

        private ReleaseProvider CreateSut() {
            return new ReleaseProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        [Fact]
        public void ReleaseCanBeStored() {
            using var sut = CreateSut();
            var release = new SnRelease( ObjectId.NewObjectId().ToString(), String.Empty, "project ID", "Name", "Description", "Repository Label", DateTimeProvider.Instance.CurrentDate );

            var result = sut.AddRelease( release );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding a release" ));
            result.IsRight.Should().BeTrue( "releases should be addable" );
        }

        [Fact]
        public void NewReleaseCanBeRetrieved() {
            var release = new SnRelease( "Release" );
            using var sut = CreateSut();

            sut.AddRelease( release );
            var result = sut.GetRelease( release.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a release" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( release, option => option.Excluding( e => e.DbId ), "retrieved release should match stored release" ));
        }

        [Fact]
        public void NewReleaseShouldHaveCurrentEntryDate() {
            var release = new SnRelease( "Release" );
            using var sut = CreateSut();

            sut.AddRelease( release );
            var result = sut.GetRelease( release.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a release" ));
            result.Do( retrieved => retrieved.ReleaseDate.Should().Be( DateOnly.FromDateTime( mTestTime ), "release date should match test time" ));
        }

        [Fact]
        public void ReleaseShouldUpdateSuccessfully() {
            var release = new SnRelease( "release" );
            using var sut = CreateSut();

            sut.AddRelease( release ).Do( e => release = e );
            release = release.With( description: "new description" );
            var result = sut.UpdateRelease( release );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating release" ));
        }

        [Fact]
        public void ReleaseShouldBeUpdated() {
            var release = new SnRelease( "release" );
            using var sut = CreateSut();

            sut.AddRelease( release ).Do( e => release = e );
            release = release.With( description: "new description" );
            // ReSharper disable once AccessToDisposedClosure
            var result = sut.UpdateRelease( release ).Bind( _ => sut.GetRelease( release.EntityId ));

            result.IfLeft( error => error.Should().BeNull( "updating release should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( release, "retrieved release should match update" ));
        }

        [Fact]
        public void ReleaseCanBeDeleted() {
            var release = new SnRelease( "release title" );
            using var sut = CreateSut();

            sut.AddRelease( release ).Do( e => release = e );
            var result = sut.DeleteRelease( release );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete release" ));
            result.IsRight.Should().BeTrue( "release should be deleted without error" );
        }

        [Fact]
        public void ReleasesCanBeListed() {
            var release = new SnRelease( "one" );
            using var sut = CreateSut();

            sut.AddRelease( release );
            sut.AddRelease( release.With( name: "two" ));
            sut.AddRelease( release.With( name: "three" ));
            sut.AddRelease( release.With( name: "four" ).With( description: "description" ));

            var result = sut.GetReleases();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting release list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 releases were added" ));
        }

        [Fact]
        public void ReleasesCanBeListedByProject() {
            var project1 = new SnProject( "project1", "P1" );
            var project2 = new SnProject( "project2", "P2" );
            var release1 = new SnRelease( "release 1" ).For( project1 );
            var release2 = new SnRelease( "release 2" ).For( project1 ).With( description: "description2" );
            var release3 = new SnRelease( "release 3" ).For( project2 ).With( description: "description3" );
            var release4 = new SnRelease( "release 4" ).For( project1 );
            using var sut = CreateSut();

            sut.AddRelease( release1 );
            sut.AddRelease( release2 );
            sut.AddRelease( release3 );
            sut.AddRelease( release4 );

            var result = sut.GetReleases( project1 );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting issue type list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 3, "3 releases are associated with project1" ));
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
