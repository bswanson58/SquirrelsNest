using System;
using FluentAssertions;
using SquirrelsNest.Common.Types;
using Xunit;

namespace SquirrelsNest.Common.Tests.Types {
    public class IssueIdTests {
        [Fact]
        public void NullId() {
            var sut = IssueId.For( String.Empty );

            sut.IsSome.Should().BeFalse( "Empty string should be an invalid issue ID." );
        }

        [Fact]
        public void WhiteSpaceId() {
            var sut = IssueId.For( " " );

            sut.IsNone.Should().BeTrue( "White space is not a valid issue ID" );
        }

        [Fact]
        public void ValidId() {
            var sut = IssueId.For( "Valid ID" );

            sut.IsSome.Should().BeTrue( "Any non white space string should be a valid issue ID" );
        }

        [Fact]
        public void HasValidValue() {
            var issueId = "ISSUE-1";
            var sut = IssueId.For( issueId );

            var _ = sut.Some( issue => issue.Value.Should().Be( issueId, "Issue ID should encapsulate value given" ));
        }

        [Fact]
        public void IssueCanBeString() {
            var issueId = "issue-1";
            var sut = IssueId.For( issueId );
            var sutIssueId = String.Empty;

            var _ = sut.Some( issue => sutIssueId = issue );

            sutIssueId.Should().BeEquivalentTo( sutIssueId, "IssueId should allow conversion to string." );
        }
    }
}
