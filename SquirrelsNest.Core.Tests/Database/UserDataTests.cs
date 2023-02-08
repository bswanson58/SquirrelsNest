using System;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.Database;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    internal class UsersLastProject {
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string   ProjectName { get; set; }

        public UsersLastProject() {
            ProjectName = String.Empty;
        }

        public UsersLastProject( string projectName ) {
            ProjectName = projectName;
        }
    }

    [Collection(nameof(SequentialCollection))]
    public class UserDataTests : BaseProviderTests {

        private IUserData CreateSut() {
            return new UserData( new UserDataProvider( mUserDataProvider ));
        }

        [Fact]
        public async void UserDataCanBeSaved() {
            var users = await CreateSomeUsers( 1 );
            var sut = CreateSut();

            var result = await sut.Save( users[0], UserDataType.LastProject, new UsersLastProject( "project name" ));

            result.IfLeft( error => error.Should().BeNull( "initial save of user data" ));
        }

        [Fact]
        public async void UserDataCanBeReSaved() {
            var users = await CreateSomeUsers( 1 );
            var sut = CreateSut();

            var result = await ( await sut.Save( users[0], UserDataType.LastProject, new UsersLastProject( "project name" )))
                .BindAsync( async _ => await sut.Save( users[0], UserDataType.LastProject, new UsersLastProject( "updated project" )));

            result.IfLeft( error => error.Should().BeNull( "subsequent save of user data" ));
        }

        [Fact]
        public async void UserDataCanBeLoaded() {
            var users = await CreateSomeUsers( 1 );
            var sut = CreateSut();
            var userData = new UsersLastProject( "my project" );

            await sut.Save( users[0], UserDataType.LastProject, userData );
            var result = await sut.Load<UsersLastProject>( users[0], UserDataType.LastProject );

            result.IfLeft( error => error.Should().BeNull( "initial save of user data" ));
            result.IfRight( data => data.Should().BeEquivalentTo( userData, "loading user data" ));
        }

        [Fact]
        public async void InitialUserDataLoadsDefault() {
            var users = await CreateSomeUsers( 1 );
            var sut = CreateSut();
            var userData = new UsersLastProject();

            var result = await sut.Load<UsersLastProject>( users[0], UserDataType.LastProject );

            result.IfLeft( error => error.Should().BeNull( "initial save of user data" ));
            result.IfRight( data => data.Should().BeEquivalentTo( userData, "loading initial user data" ));
        }
    }
}
