using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;

namespace SquirrelsNest.EfDb.Support {
    internal class SnDatabaseInitializer : IDatabaseInitializer {
        private readonly SquirrelsNestDbContext mContext;
        private readonly IUserProvider          mUserProvider;
        private readonly IConfiguration         mConfiguration;


        public SnDatabaseInitializer( SquirrelsNestDbContext context, IConfiguration configuration, IUserProvider userProvider ) {
            mContext = context;
            mConfiguration = configuration;
            mUserProvider = userProvider;
        }

        public async Task<Either<Error, Unit>> InitializeDatabase() {
            try {
                await mContext.Database.MigrateAsync();

                var users = await mUserProvider.GetUsers();

                var result = await users.BindAsync( async list => {
                    if(!list.Any()) {
                        var user = new SnUser( mConfiguration["DefaultAdmin:Email"], mConfiguration["DefaultAdmin:Email"]);
                        var result = await mUserProvider.AddUser( user );

                        return result.Map( _ => Unit.Default );
                    }

                    return users.Map( _ => Unit.Default );
                });

                return result;
            }
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }
    }
}
