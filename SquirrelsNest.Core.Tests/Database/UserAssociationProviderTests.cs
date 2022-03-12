using FluentAssertions;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Database;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    [Collection(nameof(SequentialCollection))]
    public class UserAssociationProviderTests : BaseProviderTests {

        private IUserProvider CreateSut() {
            return new UserProvider( mIssueProvider, mUserProvider, mAssociationProvider );
        }

        [Fact]
        public async void AssociationCanBeAdded() {
            var users = await CreateSomeUsers( 2 );
            var projects = await CreateSomeProjects( 2 );
            var sut = CreateSut();

            var result = await sut.AddAssociation( users[1], projects[0]);

            result.IfLeft( error => error.Should().BeNull( "adding an associated project" ));
        }

        [Fact]
        public async void AddedUserAssociationCanBeRetrieved() {
            var users = await CreateSomeUsers( 3 );
            var projects = await CreateSomeProjects( 2 );
            var sut = CreateSut();

            await sut.AddAssociation( users[0], projects[0]);
            await sut.AddAssociation( users[1], projects[1]);
            await sut.AddAssociation( users[0], projects[1]);

            var associationList = await mAssociationProvider.GetAssociations( users[0]);

            associationList.IfLeft( error => error.Should().BeNull( "retrieving associations" ));
            associationList.IfRight( list => list.Length().Should().Be( 2, "2 associations were added" ));
        }

        [Fact]
        public async void AddedProjectAssociationCanBeRetrieved() {
            var users = await CreateSomeUsers( 3 );
            var projects = await CreateSomeProjects( 2 );
            var sut = CreateSut();

            await sut.AddAssociation( users[0], projects[0]);
            await sut.AddAssociation( users[1], projects[1]);
            await sut.AddAssociation( users[0], projects[1]);

            var associationList = await mAssociationProvider.GetAssociations( projects[1].EntityId );

            associationList.IfLeft( error => error.Should().BeNull( "retrieving associations" ));
            associationList.IfRight( list => list.Length().Should().Be( 2, "2 associations were added" ));
        }

        [Fact] async void AssociationsCanBeDeleted() {
            var users = await CreateSomeUsers( 3 );
            var projects = await CreateSomeProjects( 2 );
            var sut = CreateSut();

            await sut.AddAssociation( users[1], projects[0]);
            await sut.AddAssociation( users[0], projects[1]);
            var result = await sut.DeleteAssociation( users[0], projects[1]);
            var associationList = await mAssociationProvider.GetAssociations( users[0]);

            result.IfLeft( error => error.Should().BeNull( "deleting association" ));
            associationList.IfLeft( error => error.Should().BeNull( "retrieving associations" ));
            associationList.IfRight( list => list.Length().Should().Be( 0, "association should be deleted" ));
        }
    }
}
