using System;
using System.IO;
using FluentAssertions;
using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using NSubstitute;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Database {
    internal class TestEntity : DbBase {
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  string  Name { get; }
        public  int     Number {  get; }
        // ReSharper restore MemberCanBePrivate.Global

        // ReSharper disable once MemberCanBePrivate.Global
        public TestEntity( ObjectId id, string name, int number ) :
            base( id ) {
            Name = name;
            Number = number;
        }

        public TestEntity( string name, int number ) :
            this( ObjectId.NewObjectId(), name, number ) {
        }
    }

    internal class TestEntityProvider : EntityProvider<TestEntity> {
        internal TestEntityProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, "TestEntities" ) { }

        protected override void InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<TestEntity>().Id( e => e.Id );
        }

        public new Either<Error, Unit> InsertEntity( TestEntity entity ) => base.InsertEntity( entity );
        public new Either<Error, Option<TestEntity>> GetEntityById( ObjectId id ) => base.GetEntityById( id );
    }

    public class EntityProviderTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public EntityProviderTests() {
            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Test.DB" );

            DeleteDatabase();
        }

        private TestEntityProvider CreateSut() {
            var databaseProvider = new DatabaseProvider( mEnvironment, mConstants );

            return new TestEntityProvider( databaseProvider );
        }

        [Fact]
        public void CanAddEntity() {
            var entity = new TestEntity( "One", 1 );
            using var sut = CreateSut();

            var result = sut.InsertEntity( entity );

            result.IsRight.Should().BeTrue( "entity should be inserted without error" );
        }

        [Fact]
        public void CanRetrieveEntity() {
            var entity = new TestEntity( "One", 1 );
            using var sut = CreateSut();

            sut.InsertEntity( entity );
            var result = sut.GetEntityById( entity.Id );

            result.IsRight.Should().BeTrue( "entity should be retrievable" );
            result.Match( noError => noError.Match( retrieved => retrieved.Should().BeEquivalentTo( entity, "retrieved entity should be equivalent to stored entity." ),
                                                    () => true.Should().BeFalse( "entity was not retrieved" )),
                          error => error.Should().BeNull( "entity retrieval caused an error" ));
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
