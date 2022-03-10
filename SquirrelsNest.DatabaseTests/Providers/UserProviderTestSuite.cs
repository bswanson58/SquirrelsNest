using System;
using System.Linq;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using Xunit;

namespace SquirrelsNest.DatabaseTests.Providers {
    public abstract class UserProviderTestSuite : BaseProviderTestSuite {
        protected abstract IDbUserProvider CreateSut();

        [Fact]
        public async void UserCanBeStored() {
            using var sut = CreateSut();
            var user = new SnUser( EntityId.Default, String.Empty, "loginName", "User Name", "email" );

            var result = await sut.AddUser( user );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding a user" ));
            result.IsRight.Should().BeTrue( "users should be addable" );
        }

        [Fact]
        public async void NewUserCanBeRetrieved() {
            var user = new SnUser( "User", "email" );
            using var sut = CreateSut();

            await sut.AddUser( user );
            var result = await sut.GetUser( user.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a user" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( user, option => option.Excluding( e => e.DbId ), "retrieved user should match stored user" ));
        }

        [Fact]
        public async void UserShouldUpdateSuccessfully() {
            var user = new SnUser( "user", "email" );
            using var sut = CreateSut();

            sut.AddUser( user ).Result.Do( e => user = e );
            user = user.With( displayName: "user name" );
            var result = await sut.UpdateUser( user );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating user" ));
        }

        [Fact]
        public async void UserShouldBeUpdated() {
            var user = new SnUser( "user", "email" );
            using var sut = CreateSut();

            sut.AddUser( user ).Result.Do( e => user = e );
            user = user.With( displayName: "different name" );
            // ReSharper disable once AccessToDisposedClosure
            await sut.UpdateUser( user );
            var result = await sut.GetUser( user.EntityId );

            result.IfLeft( error => error.Should().BeNull( "updating user should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( user, "retrieved user should match update" ));
        }

        [Fact]
        public async void UserCanBeDeleted() {
            var user = new SnUser( "user1", "email" );
            using var sut = CreateSut();

            sut.AddUser( user ).Result.Do( e => user = e );
            var result = await sut.DeleteUser( user );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete user" ));
            result.IsRight.Should().BeTrue( "user should be deleted without error" );
        }

        [Fact]
        public async void EmptyDatabaseReturnsEmptyUserList() {
            using var sut = CreateSut();

            var result = await sut.GetUsers();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving empty list" ));
            result.IfRight( list => list.Length().Should().Be( 0, "list should be empty" ));
        }

        [Fact]
        public async void UsersCanBeListed() {
            using var sut = CreateSut();

            await sut.AddUser( new SnUser( "one", "email" ));
            await sut.AddUser( new SnUser( "two", "email" ));
            await sut.AddUser( new SnUser( "three", "email" ));
            await sut.AddUser( new SnUser( "four", "email" ));

            var result = await sut.GetUsers();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting user list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 users were added" ));
        }
    }
}