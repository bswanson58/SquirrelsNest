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
        public  string  Name { get; set; }
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
        public new Either<Error, Unit> DeleteEntity( TestEntity entity ) => base.DeleteEntity( entity );
        public new Either<Error, Unit> UpdateEntity( TestEntity entity ) => base.UpdateEntity( entity );

        public new Either<Error, TestEntity> GetEntityById( ObjectId id ) => base.GetEntityById( id );
        public new Either<Error, TestEntity> FindEntity( string expression ) => base.FindEntity( expression );
        public new Either<Error, ILiteQueryable<TestEntity>> GetList() => base.GetList();
    }

    [Collection(nameof(SequentialCollection))]
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
            return new TestEntityProvider( new DatabaseProvider( mEnvironment, mConstants ));
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
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            result.Match( retrieved => retrieved.Should().BeEquivalentTo( entity, "retrieved entity should be equivalent to stored entity." ),
                          error => error.Should().BeNull( "entity retrieval caused an error" ));
        }

        [Fact]
        public void NonExistingEntityShouldReturnError() {
            var entity = new TestEntity( "One", 1 );
            using var sut = CreateSut();
            sut.InsertEntity( entity );
            entity = new TestEntity( "Two", 2 );

            var result = sut.GetEntityById( entity.Id );

            result.IsLeft.Should().BeTrue( "entity should not be retrievable" );
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            result.Match( retrieved => retrieved.Should().NotBeEquivalentTo( entity, "retrieved entity should not be found." ),
                          error => error.Should().NotBeNull( "non retrieved entity should return an error" ));
        }

        [Fact]
        public void EntityCanBeDeleted() {
            var entity = new TestEntity( "One", 1 );
            using var sut = CreateSut();
            sut.InsertEntity( entity );

            var result = sut.DeleteEntity( entity );

            result.IsRight.Should().BeTrue( "entry should be successfully deleted" );
        }

        [Fact]
        public void EntityCanNotBeRetrieved() {
            var entity = new TestEntity( "One", 1 );
            using var sut = CreateSut();
            sut.InsertEntity( entity );
            sut.DeleteEntity( entity );

            var result = sut.GetEntityById( entity.Id );

            result.IsLeft.Should().BeTrue( "deleted entity should not be retrieved" );
        }

        [Fact]
        public void EntryCanBeUpdatedSuccessfully() {
            var entity = new TestEntity( "One", 1 );
            using var sut = CreateSut();
            sut.InsertEntity( entity );
            entity.Name = "Two";

            var result = sut.UpdateEntity( entity );

            result.IsRight.Should().BeTrue( "entry should be successfully updated" );
        }

        [Fact]
        public void UpdatedEntryShouldBeChanged() {
            var entity = new TestEntity( "One", 1 );
            using var sut = CreateSut();
            sut.InsertEntity( entity );
            entity.Name = "Two";
            sut.UpdateEntity( entity );

            var result = sut.GetEntityById( entity.Id );

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            result.Match( retrieved => retrieved.Should().BeEquivalentTo( entity, "updated entity should match" ),
                          error => error.Should().BeNull( "there should be no error" ));
        }

        [Fact]
        public void EmptyDatabaseShouldReturnSuccessfulList() {
            using var sut = CreateSut();

            var result = sut.GetList();

            result.IsRight.Should().BeTrue( "an empty list should return without error" );
        }

        [Fact]
        public void EmptyDatabaseShouldReturnEmptyList() {
            using var sut = CreateSut();

            var result = sut.GetList();

            result.Match( list => list.Count().Should().Be( 0, "an empty database should return an empty list" ),
                          error => error.Should().BeNull( "an empty database should not return an error retrieving a list" ));
        }

        [Fact]
        public void SingleRecordShouldBeReturnedInList() {
            var entity = new TestEntity( "One", 1 );
            using var sut = CreateSut();
            sut.InsertEntity( entity );

            var result = sut.GetList();

            result.Match( list => list.Count().Should().Be( 1, "a single record database should return a list of one" ),
                          error => error.Should().BeNull( "should not return an error retrieving a list" ));
        }

        [Fact]
        public void SingleRecordShouldBeReturnedAndMatch() {
            var entity = new TestEntity( "One", 1 );
            using var sut = CreateSut();
            sut.InsertEntity( entity );

            var result = sut.GetList();

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            result.Match( list => list.First().Should().BeEquivalentTo( entity, "a single record database should return an equivalent entity" ),
                          error => error.Should().BeNull( "should not return an error retrieving a list" ));
        }

        [Fact]
        public void MultipleRecordsShouldBeReturned() {
            using var sut = CreateSut();
            var entity = new TestEntity( "One", 1 );

            sut.InsertEntity( entity );
            entity = new TestEntity( "Two", 2 );
            sut.InsertEntity( entity );
            entity = new TestEntity( "Three", 3 );
            sut.InsertEntity( entity );

            var result = sut.GetList();

            result.Match( list => list.Count().Should().Be( 3, "multiple records should returned from a database" ),
                          error => error.Should().BeNull( "should not return an error retrieving a list" ));
        }

        [Fact]
        public void EntityCanBeFoundBeExpression() {
            using var sut = CreateSut();
            var entity1 = new TestEntity( "One", 1 );
            sut.InsertEntity( entity1 );
            var entity2 = new TestEntity( "Two", 2 );
            sut.InsertEntity( entity2 );
            var entity3 = new TestEntity( "Three", 3 );
            sut.InsertEntity( entity3 );

            var result = sut.FindEntity( LiteDB.Query.EQ( nameof( TestEntity.Name ), "Two" ));

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            result.Match( retrieved => retrieved.Should().BeEquivalentTo( entity2, "found entity should be equivalent to stored entity" ),
                          error => error.Should().BeNull( "should not return an error finding an entity" ));
        }

        [Fact]
        public void GetListShouldBeQueryable() {
            using var sut = CreateSut();
            var entity1 = new TestEntity( "One", 1 );
            sut.InsertEntity( entity1 );
            var entity2 = new TestEntity( "Two", 2 );
            sut.InsertEntity( entity2 );
            var entity3 = new TestEntity( "Three", 3 );
            sut.InsertEntity( entity3 );

            var results = sut.GetList();

            results.IfRight( queryable => queryable.Where( e => e.Number.Equals( 2 )).First().Should().BeEquivalentTo( entity2, "query should select entity" ));
            results.IfLeft( error => error.Should().BeNull( "and error should not occur for a query" ));
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
