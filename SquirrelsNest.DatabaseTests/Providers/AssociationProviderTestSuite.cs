using System;
using System.Linq;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using Xunit;

namespace SquirrelsNest.DatabaseTests.Providers {
    public abstract class AssociationProviderTestSuite : BaseProviderTestSuite {
        protected abstract IDbAssociationProvider CreateSut();

        [Fact]
        public async void AssociationCanBeStored() {
            using var sut = CreateSut();
            var association = new SnAssociation( EntityId.Default, String.Empty, EntityId.Default, EntityId.Default );

            var result = await sut.AddAssociation( association );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding an association" ));
            result.IsRight.Should().BeTrue( "associations should be addable" );
        }

        [Fact]
        public async void NewAssociationCanBeRetrieved() {
            var association = new SnAssociation( EntityId.Default, EntityId.Default );
            using var sut = CreateSut();

            await sut.AddAssociation( association );
            var result = await sut.GetAssociation( association.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving an association" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( association, option => option.Excluding( e => e.DbId ), "retrieved association should match stored association" ));
        }

        [Fact]
        public async void AssociationCanBeDeleted() {
            var association = new SnAssociation( EntityId.Default, EntityId.Default );
            using var sut = CreateSut();

            sut.AddAssociation( association ).Result.Do( e => association = e );
            var result = await sut.DeleteAssociation( association );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete an association" ));
            result.IsRight.Should().BeTrue( "association should be deleted without error" );
        }

        [Fact]
        public async void AssociationsCanBeListed() {
            using var sut = CreateSut();

            await sut.AddAssociation( new SnAssociation( SnUser.Default.EntityId, EntityId.Default ));
            await sut.AddAssociation( new SnAssociation( SnUser.Default.EntityId, EntityId.Default ));
            await sut.AddAssociation( new SnAssociation( SnUser.Default.EntityId, EntityId.Default ));

            var result = await sut.GetAssociations( SnUser.Default );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting an association list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 3, "3 associations were added" ));
        }

        [Fact]
        public async void AssociationDatabaseShouldBeEmpty() {
            using var sut = CreateSut();
            var result = await sut.GetAssociations( SnUser.Default );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting empty associations list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 0, "There should be no associations in the database" ));
        }
    }
}
