using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Database;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    [Collection(nameof(SequentialCollection))]
    public class UserDataProviderTests : BaseProviderTests {

        private IUserDataProvider CreateSut() {
            return new UserDataProvider( mUserDataProvider, mUserProvider );
        }

        [Fact]
        public async void UserDataCanBeSaved() {
            var users = await CreateSomeUsers( 2 );
            var sut = CreateSut();

            var results = await sut.SaveData( new SnUserData( users[1].EntityId, UserDataType.LastProject, "some project data" ));

            results.IfLeft( error => error.Should().BeNull( "saving user data" ));
        }

        [Fact]
        public async void DefaultUserDataCanBeLoaded() {
            var users = await CreateSomeUsers( 1 );
            var sut = CreateSut();

            var result = await sut.LoadData( users[0], UserDataType.LastProject );

            result.IfLeft( error => error.Should().BeNull( "loading user data" ));
            result.IfRight( data => data.Should().BeEquivalentTo( SnUserData.Default, "default data should be default instance" ));
        }

        [Fact]
        public async void UserDataCanBeLoaded() {
            var users = await CreateSomeUsers( 1 );
            var sut = CreateSut();
            var userData = new SnUserData( users[0].EntityId, UserDataType.IssueListFormat, "list format" );

            await sut.SaveData( userData );
            var result = await sut.LoadData( users[0], UserDataType.IssueListFormat );

            result.IfLeft( error => error.Should().BeNull( "loading user data" ));
            result.IfRight( data => data.Should().BeEquivalentTo( userData, options => options.Excluding( d => d.DbId ),
                "loaded data should be equivalent" ));
        }

        [Fact]
        public async void AllUserDataCanBeLoaded() {
            var users = await CreateSomeUsers( 3 );
            var sut = CreateSut();

            await sut.SaveData( new SnUserData( users[0].EntityId, UserDataType.LastProject, "last project 0" ));
            await sut.SaveData( new SnUserData( users[1].EntityId, UserDataType.LastProject, "last project 1" ));
            await sut.SaveData( new SnUserData( users[0].EntityId, UserDataType.IssueListFormat, "list format" ));
            var results = await sut.GetData( users[0]);

            results.IfLeft( error => error.Should().BeNull( "getting all user data" ));
            results.IfRight( data => data.Length().Should().Be( 2, "getting user data" ));
        }

        [Fact]
        public async void InitialUserDataShouldBeEmpty() {
            var users = await CreateSomeUsers( 2 );
            var sut = CreateSut();

            var results = await sut.GetData( users[1]);

            results.IfLeft( error => error.Should().BeNull( "getting all user data" ));
            results.IfRight( data => data.Length().Should().Be( 0, "user should not have initial data" ));
        }
    }
}
