using System;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using Xunit;

namespace SquirrelsNest.Common.Tests.Entities {
    public class SnProjectTests {
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

            sut.Should().BeEquivalentTo( original, options => options.Excluding( e => e.Name ));
            sut.Name.Should().Be( "new name" );
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

            sut.Should().BeEquivalentTo( original, options => options.Excluding( e => e.IssuePrefix ));
            sut.IssuePrefix.Should().Be( "new prefix" );
        }

        [Fact]
        public void ProjectCanHaveNextIssueNumberChanged() {
            var original = new SnProject( "Name","prefix" ).With( nextIssueNumber: 100 );

            var sut = original.With( nextIssueNumber: 200 );

            sut.Should().BeEquivalentTo( original, options => options.Excluding( e => e.NextIssueNumber ));
            sut.NextIssueNumber.Should().Be( 200 );
        }
    }
}
