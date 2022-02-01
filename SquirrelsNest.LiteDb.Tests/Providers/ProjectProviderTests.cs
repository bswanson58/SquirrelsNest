using System;
using System.IO;
using AutoMapper;
using FluentAssertions;
using LiteDB;
using NSubstitute;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;
using SquirrelsNest.LiteDb.Tests.Database;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class ProjectProviderTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;
        private readonly IMapper                mMapper;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public ProjectProviderTests() {
            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            mMapper = new MapperConfiguration( cfg => cfg.AddMaps( typeof( DbMappingProfile ))).CreateMapper();

            DeleteDatabase();
        }

        private IProjectProvider CreateSut() {
            return new ProjectProvider( new DatabaseProvider( mEnvironment, mConstants ), mMapper );
        }

        [Fact]
        public void ProjectCanBeStored() {
            using var sut = CreateSut();
            var project = new SnProject( ObjectId.NewObjectId().ToString(), "Name", "description", DateOnly.Parse( "01/31/2022" ), "repository", "prefix", 1 );

            var result = sut.AddProject( project );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding a project" ));
            result.IsRight.Should().BeTrue( "projects should be addable" );
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
