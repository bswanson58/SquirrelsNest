using System;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Common.Values;
using Xunit;

namespace SquirrelsNest.Common.Tests.Entities {
    public class SnIssueTests {
        private readonly DateTime   mTestTime = new DateTime( 2000, 11, 30, 9, 45, 50 );

        public SnIssueTests() {
            DateTimeProvider.SetProvider( new TestTimeProvider( mTestTime ));
        }

        [Fact]
        public void IssueCannotBeCreatedWithEmptyTitle() {
            Assert.Throws<ApplicationException>(() => new SnIssue( String.Empty, 1, EntityId.Default ));
        }

        [Fact]
        public void IssueCanBeCreatedWithTitle() {
            var sut = new SnIssue( "Title", 2, EntityId.Default );

            sut.Should().NotBeNull( "SnIssue should be creatable" );
        }

        [Fact]
        public void IssueShouldBeCreatedWIthCurrentTime() {
            var sut = new SnIssue( "title", 4, EntityId.Default);

            sut.EntryDate.Should().Be( DateOnly.FromDateTime( mTestTime ), "Issue inception should be current date" );
        }

        [Fact]
        public void IssueCanHaveTitleChanged() {
            var original = new SnIssue( "title", 7, EntityId.Default );

            var sut = original.With( title: "new title" );

            sut.Should().BeEquivalentTo( original, options => options.Excluding( e => e.Title ));
            sut.Title.Should().Be( "new title" );
        }

        [Fact]
        public void IssueCanHaveDescriptionChanged() {
            var original = new SnIssue( "title", 4, EntityId.Default );

            var sut = original.With( description: "new description" );

            sut.Should().BeEquivalentTo( original, options => options.Excluding( e => e.Description ));
            sut.Description.Should().Be( "new description" );
        }
    }
}
