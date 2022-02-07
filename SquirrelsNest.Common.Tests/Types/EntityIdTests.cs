using System;
using FluentAssertions;
using SquirrelsNest.Common.Values;
using Xunit;

namespace SquirrelsNest.Common.Tests.Types {
    public class EntityIdTests {
        [Fact]
        public void NullId() {
            var sut = EntityId.For( String.Empty );

            sut.IsSome.Should().BeFalse( "Empty string should be an invalid entity ID." );
        }

        [Fact]
        public void WhiteSpaceId() {
            var sut = EntityId.For( " " );

            sut.IsNone.Should().BeTrue( "White space is not a valid entity ID" );
        }

        [Fact]
        public void ValidId() {
            var sut = EntityId.For( "Valid ID" );

            sut.IsSome.Should().BeTrue( "Any non white space string should be a valid entity ID" );
        }

        [Fact]
        public void HasValidValue() {
            var entityId = "entity-1";
            var sut = EntityId.For( entityId );

            var _ = sut.Some( entity => entity.Value.Should().Be( entityId, "entity ID should encapsulate value given" ));
        }

        [Fact]
        public void EntityCanBeString() {
            var entityId = "entity-1";
            var sut = EntityId.For( entityId );
            var sutEntityId = String.Empty;

            var _ = sut.Some( entity => sutEntityId = entity );

            sutEntityId.Should().BeEquivalentTo( sutEntityId, "EntityId should allow conversion to string." );
        }
    }
}