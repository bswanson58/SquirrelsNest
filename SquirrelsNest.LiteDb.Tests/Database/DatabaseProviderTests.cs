using System;
using System.IO;
using FluentAssertions;
using NSubstitute;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.LiteDb.Database;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Database {
    public class DatabaseProviderTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public DatabaseProviderTests() {
            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Test.DB" );

            DeleteDatabase();
        }

        private IDatabaseProvider CreateSut() {
            return new DatabaseProvider( mEnvironment, mConstants );
        }

        [Fact]
        public void CanCreateDatabase() {
            using var sut = CreateSut();
            var database = sut.GetDatabase();

            database.IsRight.Should().BeTrue( "database should be created." );
            File.Exists( DatabaseFile ).Should().BeTrue( "database file should exist." );
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
