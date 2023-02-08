using System;
using System.Linq;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using Xunit;

namespace SquirrelsNest.DatabaseTests.Providers {
    public abstract class ComponentProviderTestSuite : BaseProviderTestSuite {
        protected abstract IDbComponentProvider CreateSut();

        [Fact]
        public async void ComponentCanBeStored() {
            using var sut = CreateSut();
            var component = new SnComponent( EntityId.Default, String.Empty, EntityId.Default, "Name", "Description" );

            var result = await sut.AddComponent( component );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding a component" ));
            result.IsRight.Should().BeTrue( "components should be addable" );
        }

        [Fact]
        public async void NewComponentCanBeRetrieved() {
            var component = new SnComponent( "Component" );
            using var sut = CreateSut();

            await sut.AddComponent( component );
            var result = await sut.GetComponent( component.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a component" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( component, option => option.Excluding( e => e.DbId ), "retrieved component should match stored component" ));
        }

        [Fact]
        public async void ComponentShouldUpdateSuccessfully() {
            var component = new SnComponent( "component" );
            using var sut = CreateSut();

            sut.AddComponent( component ).Result.Do( e => component = e );
            component = component.With( description: "new description" );
            var result = await sut.UpdateComponent( component );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating component" ));
        }

        [Fact]
        public async void ComponentShouldBeUpdated() {
            var component = new SnComponent( "component" );
            using var sut = CreateSut();

            sut.AddComponent( component ).Result.Do( e => component = e );
            component = component.With( description: "new description" );
            await sut.UpdateComponent( component );
            var result = await sut.GetComponent( component.EntityId );

            result.IfLeft( error => error.Should().BeNull( "updating component should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( component, "retrieved component should match update" ));
        }

        [Fact]
        public async void ComponentCanBeDeleted() {
            var component = new SnComponent( "component title" );
            using var sut = CreateSut();

            sut.AddComponent( component ).Result.Do( e => component = e );
            var result = await sut.DeleteComponent( component );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete component" ));
            result.IsRight.Should().BeTrue( "component should be deleted without error" );
        }

        [Fact]
        public async void ComponentsCanBeListed() {
            var component = new SnComponent( "one" );
            using var sut = CreateSut();

            await sut.AddComponent( component );
            await sut.AddComponent( component.With( name: "two" ));
            await sut.AddComponent( component.With( name: "three" ));
            await sut.AddComponent( component.With( name: "four" ).With( description: "description" ));

            var result = await sut.GetComponents();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting component list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 components were added" ));
        }

        [Fact]
        public async void ComponentsCanBeListedByProject() {
            var project1 = new SnProject( "project1", "P1" );
            var project2 = new SnProject( "project2", "P2" );
            var component1 = new SnComponent( "component 1" ).For( project1 );
            var component2 = new SnComponent( "component 2" ).For( project1 ).With( description: "description2" );
            var component3 = new SnComponent( "component 3" ).For( project2 ).With( description: "description3" );
            var component4 = new SnComponent( "component 4" ).For( project1 );
            using var sut = CreateSut();

            await sut.AddComponent( component1 );
            await sut.AddComponent( component2 );
            await sut.AddComponent( component3 );
            await sut.AddComponent( component4 );

            var result = await sut.GetComponents( project1 );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting component list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 3, "3 components are associated with project1" ));
        }

        [Fact]
        public async void ComponentDatabaseShouldBeEmpty() {
            using var sut = CreateSut();
            var result = await sut.GetComponents();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting empty components list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 0, "There should be no components in the database" ));
        }
    }
}
