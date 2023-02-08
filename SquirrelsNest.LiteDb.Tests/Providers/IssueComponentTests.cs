using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NSubstitute;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Common.Values;
using SquirrelsNest.DatabaseTests.Support;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class IssueComponentTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;
        private readonly List<SnComponent>      mComponents;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public IssueComponentTests() {
            DateTimeProvider.SetProvider( new TestTimeProvider( new DateTime( 2020, 11, 30, 7, 17, 55 )));

            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            mComponents = new List<SnComponent>();

            DeleteDatabase();
        }

        private IssueProvider CreateSut() {
            AddSomeComponents();

            return new IssueProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        private void AddSomeComponents() {
            using var componentProvider = new ComponentProvider( new DatabaseProvider( mEnvironment, mConstants ));

            componentProvider.AddComponent( new SnComponent( "component 1" )).Do( component => mComponents.Add( component ));
            componentProvider.AddComponent( new SnComponent( "component 2" )).Do( component => mComponents.Add( component ));
            componentProvider.AddComponent( new SnComponent( "component 3" )).Do( component => mComponents.Add( component ));
        }

        [Fact]
        public void IssueIsCreatedWithDefaultComponent() {
            var sut = new SnIssue( "title", 1, EntityId.Default );

            sut.ComponentId.Should().BeEquivalentTo( EntityId.Default, "Issue should be created with a default component." );
        }

        [Fact]
        public void StoredNonComponentIssueReturnsNoComponent() {
            using var sut = CreateSut();
            var issue = new SnIssue( "title", 3, EntityId.Default );
            sut.AddIssue( issue );

            var results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.ComponentId.Should().BeEquivalentTo( EntityId.Default, "issue should have default component" ));
        }

        [Fact]
        public void IssueStoredWithComponentReturnsComponent() {
            using var sut = CreateSut();
            var issue = new SnIssue( "title", 3, EntityId.Default );
            sut.AddIssue( issue );

            var results = sut.GetIssue( issue.EntityId );
            results.IfRight( i => issue = i.With( mComponents[1]));
            sut.UpdateIssue( issue );
            results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.Should().BeEquivalentTo( issue, "issue should have updated component" ));
            results.IfRight( retrieved => retrieved.ComponentId.Should().BeEquivalentTo( mComponents[1].EntityId, "issue component should be equal to stored" ));
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
