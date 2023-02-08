using System;
using System.Linq;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using Xunit;

namespace SquirrelsNest.DatabaseTests.Providers {
    public abstract class UserDataProviderTestSuite : BaseProviderTestSuite {
        protected abstract IDbUserDataProvider CreateSut();

        [Fact]
        public async void UserDataCanBeStored() {
            using var sut = CreateSut();
            var data = new SnUserData( EntityId.Default, String.Empty, EntityId.Default, UserDataType.LastProject, String.Empty );

            var result = await sut.AddData( data );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding user data" ));
            result.IsRight.Should().BeTrue( "user data should be addable" );
        }

        [Fact]
        public async void NewDataCanBeRetrieved() {
            var data = new SnUserData( SnUser.Default.EntityId, UserDataType.LastProject, "last project" );
            using var sut = CreateSut();

            await sut.AddData( data );
            var result = await sut.GetData( SnUser.Default, UserDataType.LastProject );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving user data" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( data, option => option.Excluding( e => e.DbId ), "retrieved user data should match stored data" ));
        }

        [Fact]
        public async void UserDataCanBeDeleted() {
            var data = new SnUserData( SnUser.Default.EntityId, UserDataType.IssueListFormat, "whatever" );
            using var sut = CreateSut();

            sut.AddData( data ).Result.Do( e => data = e );
            var result = await sut.DeleteData( data );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete user data" ));
            result.IsRight.Should().BeTrue( "user data should be deleted without error" );
        }

        [Fact]
        public async void UserDataCanBeListed() {
            using var sut = CreateSut();

            await sut.AddData( new SnUserData( SnUser.Default.EntityId, UserDataType.IssueListFormat, "options" ));
            await sut.AddData( new SnUserData( SnUser.Default.EntityId, UserDataType.LastProject, "project data" ));
            await sut.AddData( new SnUserData( SnUser.Default.EntityId, UserDataType.Unknown, String.Empty ));

            var result = await sut.GetData( SnUser.Default );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting user data list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 3, "3 user data instances were added" ));
        }

        [Fact]
        public async void DataDatabaseShouldBeEmpty() {
            using var sut = CreateSut();
            var result = await sut.GetData( SnUser.Default );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting empty user data list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 0, "There should be no user data in the database" ));
        }
    }
}
