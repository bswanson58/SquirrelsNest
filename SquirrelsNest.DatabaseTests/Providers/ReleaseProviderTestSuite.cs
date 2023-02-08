using System;
using System.Linq;
using FluentAssertions;
using LanguageExt;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Common.Values;
using Xunit;

namespace SquirrelsNest.DatabaseTests.Providers {
    public abstract class ReleaseProviderTestSuite : BaseProviderTestSuite {
        private readonly DateTime   mTestTime;

        protected ReleaseProviderTestSuite() {
            mTestTime = new DateTime( 2020, 12, 17, 6, 33, 44 );

            DateTimeProvider.SetProvider( new TestTimeProvider( mTestTime ));
        }

        protected abstract IDbReleaseProvider CreateSut();
        
        [Fact]
        public async void ReleaseCanBeStored() {
            using var sut = CreateSut();
            var release = new SnRelease( EntityId.Default, String.Empty, "project ID", "Name", "Description", "Repository Label", DateTimeProvider.Instance.CurrentDate );

            var result = await sut.AddRelease( release );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding a release" ));
            result.IsRight.Should().BeTrue( "releases should be addable" );
        }

        [Fact]
        public async void NewReleaseCanBeRetrieved() {
            var release = new SnRelease( "Release" );
            using var sut = CreateSut();

            await sut.AddRelease( release );
            var result = await sut.GetRelease( release.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a release" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( release, option => option.Excluding( e => e.DbId ), "retrieved release should match stored release" ));
        }

        [Fact]
        public async void NewReleaseShouldHaveCurrentEntryDate() {
            var release = new SnRelease( "Release" );
            using var sut = CreateSut();

            await sut.AddRelease( release );
            var result = await sut.GetRelease( release.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a release" ));
            result.Do( retrieved => retrieved.ReleaseDate.Should().Be( DateOnly.FromDateTime( mTestTime ), "release date should match test time" ));
        }

        [Fact]
        public async void ReleaseShouldUpdateSuccessfully() {
            var release = new SnRelease( "release" );
            using var sut = CreateSut();

            sut.AddRelease( release ).Result.Do( e => release = e );
            release = release.With( description: "new description" );
            var result = await sut.UpdateRelease( release );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating release" ));
        }

        [Fact]
        public async void ReleaseShouldBeUpdated() {
            var release = new SnRelease( "release" );
            using var sut = CreateSut();

            sut.AddRelease( release ).Result.Do( e => release = e );
            release = release.With( description: "new description" );
            // ReSharper disable once AccessToDisposedClosure
            var result = await sut.UpdateRelease( release ).Bind( _ => sut.GetRelease( release.EntityId ));

            result.IfLeft( error => error.Should().BeNull( "updating release should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( release, "retrieved release should match update" ));
        }

        [Fact]
        public async void ReleaseCanBeDeleted() {
            var release = new SnRelease( "release title" );
            using var sut = CreateSut();

            sut.AddRelease( release ).Result.Do( e => release = e );
            var result = await sut.DeleteRelease( release );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete release" ));
            result.IsRight.Should().BeTrue( "release should be deleted without error" );
        }

        [Fact]
        public async void ReleasesCanBeListed() {
            var release = new SnRelease( "one" );
            using var sut = CreateSut();

            await sut.AddRelease( release );
            await sut.AddRelease( release.With( name: "two" ));
            await sut.AddRelease( release.With( name: "three" ));
            await sut.AddRelease( release.With( name: "four" ).With( description: "description" ));

            var result = await sut.GetReleases();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting release list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 releases were added" ));
        }

        [Fact]
        public async void ReleasesCanBeListedByProject() {
            var project1 = new SnProject( "project1", "P1" );
            var project2 = new SnProject( "project2", "P2" );
            var release1 = new SnRelease( "release 1" ).For( project1 );
            var release2 = new SnRelease( "release 2" ).For( project1 ).With( description: "description2" );
            var release3 = new SnRelease( "release 3" ).For( project2 ).With( description: "description3" );
            var release4 = new SnRelease( "release 4" ).For( project1 );
            using var sut = CreateSut();

            await sut.AddRelease( release1 );
            await sut.AddRelease( release2 );
            await sut.AddRelease( release3 );
            await sut.AddRelease( release4 );

            var result = await sut.GetReleases( project1 );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting issue type list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 3, "3 releases are associated with project1" ));
        }

    }
}