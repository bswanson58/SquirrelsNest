using System;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.Types;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Dto.Mutations;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.Users {
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class UserMutations {
        private readonly IUserProvider  mUserProvider;

        public UserMutations( IUserProvider userProvider ) {
            mUserProvider = userProvider;
        }

        private async Task<Either<Error, SnUser>> CreateUser( AddUserInput user ) {
            var dbUser = await mUserProvider.GetUser( user.Email );

            if( dbUser.IsLeft ) {
                var newUser = new SnUser( user.LoginName, user.Email ).With( displayName: user.Name );

                return await mUserProvider.AddUser( newUser );
            }

            return dbUser;
        }

        [Authorize( Policy = PolicyNames.AdminPolicy )]
        public async Task<AddUserPayload> AddUser( AddUserInput userInput,
                                                  [FromServices] UserManager<IdentityUser> userManager ) {
            var existingUser = await userManager.FindByEmailAsync( userInput.Email );

            if( existingUser != null ) {
                return new AddUserPayload( "User with requested email already exists." );
            }

            var user = new IdentityUser( userInput.LoginName ) { Email = userInput.Email };
            var result = await userManager.CreateAsync( user );

            if( result.Succeeded ) {
                result = await userManager.AddPasswordAsync( user, userInput.Password );
            }

            if(!result.Succeeded ) {
                return new AddUserPayload( result.ToString());
            }

            var newUser = await CreateUser( userInput );

            return newUser.Match( u => new AddUserPayload( u.ToCl()), e => new AddUserPayload( e ));
        }


        [Authorize( Policy = PolicyNames.AdminPolicy )]
        public async Task<DeleteUserPayload> DeleteUser( DeleteUserInput deleteInput,
                                                        [FromServices] UserManager<IdentityUser> userManager ) {
            var existingUser = await userManager.FindByEmailAsync( deleteInput.Email );

            if( existingUser == null ) {
                return new DeleteUserPayload( "User does not exist with that email." );
            }

            var deleteResult = await userManager.DeleteAsync( existingUser );

            if(!deleteResult.Succeeded ) {
                return new DeleteUserPayload( deleteResult.ToString());
            }

            var userList = await mUserProvider.GetUsers();
            var user = default( SnUser );

            userList.Do( list => 
                user = list.FirstOrDefault( u => u.Email.Equals( deleteInput.Email, StringComparison.InvariantCultureIgnoreCase )));

            if( user != null ) {
                var result = await mUserProvider.DeleteUser( user );

                return result.Match( _ => new DeleteUserPayload( user.ToCl()), e => new DeleteUserPayload( e ));
            }

            return new DeleteUserPayload( "SnUser does not exist" );
        }
    }
}
