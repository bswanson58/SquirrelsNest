using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.Service.Support {
    public class BaseGraphProvider {
        private     readonly IHttpContextAccessor   mContextAccessor;
        private     readonly IUserProvider          mUserProvider;
        protected   readonly IApplicationLog        mLog;

        protected BaseGraphProvider( IUserProvider userProvider, IHttpContextAccessor contextAccessor, IApplicationLog log ) {
            mUserProvider = userProvider;
            mContextAccessor = contextAccessor;
            mLog = log;
        }

        protected async Task<Either<Error, SnUser>> GetUser() {
            var users = await mUserProvider.GetUsers();
            var email = mContextAccessor.HttpContext?.User.Claims.FirstOrDefault( c => c.Type == "email" )?.Value;

            return email != null ? 
                users.Map( userList => userList.FirstOrDefault( u => u.Email.Equals( email ), SnUser.Default )) : 
                SnUser.Default;
        }
    }
}
