﻿using System.IO;
using NSubstitute;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.DatabaseTests.Providers;
using SquirrelsNest.DatabaseTests.Support;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class UserProviderTests : UserProviderTestSuite {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public UserProviderTests() {
            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );
        }

        protected override IDbUserProvider CreateSut() {
            return new UserProviderAsync( new DatabaseProvider( mEnvironment, mConstants ));
        }


        protected override void DeleteDatabase() {
            if( File.Exists( DatabaseFile )) {
                File.Delete( DatabaseFile );
            }
        }
    }
}
