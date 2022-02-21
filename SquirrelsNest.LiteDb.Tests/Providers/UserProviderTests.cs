using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using LiteDB;
using NSubstitute;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;
using SquirrelsNest.LiteDb.Tests.Database;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class UserProviderTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public UserProviderTests() {
            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            DeleteDatabase();
        }

        private UserProvider CreateSut() {
            return new UserProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        [Fact]
        public void UserCanBeStored() {
            using var sut = CreateSut();
            var user = new SnUser( ObjectId.NewObjectId().ToString(), String.Empty, "loginName", "User Name" );

            var result = sut.AddUser( user );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding a user" ));
            result.IsRight.Should().BeTrue( "users should be addable" );
        }

        [Fact]
        public void NewUserCanBeRetrieved() {
            var user = new SnUser( "User" );
            using var sut = CreateSut();

            sut.AddUser( user );
            var result = sut.GetUser( user.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a user" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( user, option => option.Excluding( e => e.DbId ), "retrieved user should match stored user" ));
        }

        [Fact]
        public void UserShouldUpdateSuccessfully() {
            var user = new SnUser( "user" );
            using var sut = CreateSut();

            sut.AddUser( user ).Do( e => user = e );
            user = user.With( displayName: "user name" );
            var result = sut.UpdateUser( user );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating user" ));
        }

        [Fact]
        public void UserShouldBeUpdated() {
            var user = new SnUser( "user" );
            using var sut = CreateSut();

            sut.AddUser( user ).Do( e => user = e );
            user = user.With( displayName: "different name" );
            // ReSharper disable once AccessToDisposedClosure
            var result = sut.UpdateUser( user ).Bind( _ => sut.GetUser( user.EntityId ));

            result.IfLeft( error => error.Should().BeNull( "updating user should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( user, "retrieved user should match update" ));
        }

        [Fact]
        public void UserCanBeDeleted() {
            var user = new SnUser( "user1" );
            using var sut = CreateSut();

            sut.AddUser( user ).Do( e => user = e );
            var result = sut.DeleteUser( user );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete user" ));
            result.IsRight.Should().BeTrue( "user should be deleted without error" );
        }

        [Fact]
        public void UsersCanBeListed() {
            using var sut = CreateSut();

            sut.AddUser( new SnUser( "one" ));
            sut.AddUser( new SnUser( "two" ));
            sut.AddUser( new SnUser( "three" ));
            sut.AddUser( new SnUser( "four" ));

            var result = sut.GetUsers();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting user list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 users were added" ));
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
