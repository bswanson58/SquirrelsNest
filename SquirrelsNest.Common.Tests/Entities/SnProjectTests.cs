using System;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Platform;
using Xunit;

namespace SquirrelsNest.Common.Tests.Entities {
    public class SnProjectTests {
        private readonly DateTime   mTestTime = new DateTime( 2000, 11, 30, 9, 45, 50 );

        public SnProjectTests() {
            DateTimeProvider.SetProvider( new TestTimeProvider( mTestTime ));
        }

        [Fact]
        public void ProjectCannotBeCreatedWithEmptyName() {
            Assert.Throws<ApplicationException>(() => new SnProject( String.Empty, "prefix" ));
        }

        [Fact]
        public void ProjectCannotBeCreatedWithEmptyIssuePrefix() {
            Assert.Throws<ApplicationException>(() => new SnProject( "a name", String.Empty ));
        }

        [Fact]
        public void ProjectCanBeCreatedWithName() {
            var sut = new SnProject( "Name", "prefix" );

            sut.Should().NotBeNull( "SnProject should be creatable" );
        }

        [Fact]
        public void ProjectCanHaveNameChanged() {
            var original = new SnProject( "Name","prefix" );

            var sut = original.With( name: "new name" );

            sut.Should().BeEquivalentTo( original, options => options.Excluding( e => e.Name ).Excluding( e => e.DebugName ));
            sut.Name.Should().Be( "new name" );
        }

        [Fact]
        public void ProjectShouldBeCreatedWIthCurrentTime() {
            var sut = new SnProject( "name", "prefix" );

            sut.Inception.Should().Be( DateOnly.FromDateTime( mTestTime ), "project inception should be current date" );
        }

        [Fact]
        public void ProjectCanHaveDescriptionChanged() {
            var original = new SnProject( "Name","prefix" );

            var sut = original.With( description: "new description" );

            sut.Should().BeEquivalentTo( original, options => options.Excluding( e => e.Description ));
            sut.Description.Should().Be( "new description" );
        }

        [Fact]
        public void ProjectCanHaveRepositoryChanged() {
            var original = new SnProject( "Name","prefix" );

            var sut = original.With( repository: "new repository url" );

            sut.Should().BeEquivalentTo( original, options => options.Excluding( e => e.RepositoryUrl ));
            sut.RepositoryUrl.Should().Be( "new repository url" );
        }

        [Fact]
        public void ProjectCanHaveIssuePrefixChanged() {
            var original = new SnProject( "Name","prefix" );

            var sut = original.With( issuePrefix: "new prefix" );

            sut.Should().BeEquivalentTo( original, options => options.Excluding( e => e.IssuePrefix ).Excluding( e => e.DebugName ));
            sut.IssuePrefix.Should().Be( "new prefix" );
        }

        [Fact]
        public void ProjectCanHaveNextIssueNumberChanged() {
            var original = new SnProject( "Name","prefix" );

            var sut = original.WithNextIssueNumber();

            sut.Should().BeEquivalentTo( original, options => options.Excluding( e => e.NextIssueNumber ));
            sut.NextIssueNumber.Should().Be( original.NextIssueNumber + 1 );
        }
    }
}
