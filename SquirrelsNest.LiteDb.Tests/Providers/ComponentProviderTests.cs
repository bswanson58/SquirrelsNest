using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using LiteDB;
using NSubstitute;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;
using SquirrelsNest.LiteDb.Tests.Database;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class ComponentProviderTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public ComponentProviderTests() {
            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            DeleteDatabase();
        }

        private ComponentProvider CreateSut() {
            return new ComponentProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        [Fact]
        public void ComponentCanBeStored() {
            using var sut = CreateSut();
            var component = new SnComponent( EntityId.Default, ObjectId.NewObjectId().ToString(), EntityId.Default, "Name", "Description" );

            var result = sut.AddComponent( component );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding a component" ));
            result.IsRight.Should().BeTrue( "components should be addable" );
        }

        [Fact]
        public void NewComponentCanBeRetrieved() {
            var component = new SnComponent( "Component" );
            using var sut = CreateSut();

            sut.AddComponent( component );
            var result = sut.GetComponent( component.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a component" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( component, option => option.Excluding( e => e.DbId ), "retrieved component should match stored component" ));
        }

        [Fact]
        public void ComponentShouldUpdateSuccessfully() {
            var component = new SnComponent( "component" );
            using var sut = CreateSut();

            sut.AddComponent( component ).Do( e => component = e );
            component = component.With( description: "new description" );
            var result = sut.UpdateComponent( component );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating component" ));
        }

        [Fact]
        public void ComponentShouldBeUpdated() {
            var component = new SnComponent( "component" );
            using var sut = CreateSut();

            sut.AddComponent( component ).Do( e => component = e );
            component = component.With( description: "new description" );
            // ReSharper disable once AccessToDisposedClosure
            var result = sut.UpdateComponent( component ).Bind( _ => sut.GetComponent( component.EntityId ));

            result.IfLeft( error => error.Should().BeNull( "updating component should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( component, "retrieved component should match update" ));
        }

        [Fact]
        public void ComponentCanBeDeleted() {
            var component = new SnComponent( "component title" );
            using var sut = CreateSut();

            sut.AddComponent( component ).Do( e => component = e );
            var result = sut.DeleteComponent( component );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete component" ));
            result.IsRight.Should().BeTrue( "component should be deleted without error" );
        }

        [Fact]
        public void ComponentsCanBeListed() {
            var component = new SnComponent( "one" );
            using var sut = CreateSut();

            sut.AddComponent( component );
            sut.AddComponent( component.With( name: "two" ));
            sut.AddComponent( component.With( name: "three" ));
            sut.AddComponent( component.With( name: "four" ).With( description: "description" ));

            var result = sut.GetComponents();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting component list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 components were added" ));
        }

        [Fact]
        public void ComponentsCanBeListedByProject() {
            var project1 = new SnProject( "project1", "P1" );
            var project2 = new SnProject( "project2", "P2" );
            var component1 = new SnComponent( "component 1" ).For( project1 );
            var component2 = new SnComponent( "component 2" ).For( project1 ).With( description: "description2" );
            var component3 = new SnComponent( "component 3" ).For( project2 ).With( description: "description3" );
            var component4 = new SnComponent( "component 4" ).For( project1 );
            using var sut = CreateSut();

            sut.AddComponent( component1 );
            sut.AddComponent( component2 );
            sut.AddComponent( component3 );
            sut.AddComponent( component4 );

            var result = sut.GetComponents( project1 );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting component list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 3, "3 components are associated with project1" ));
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
