using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Shared.Dto.Auth;

namespace SquirrelsNest.Pecan.Server.Features.Auth {
    [Route(CreateUserInput.Route)]
    public class CreateUser : EndpointBaseAsync
        .WithRequest<CreateUserInput>
        .WithActionResult<CreateUserResponse> {

        private readonly IUserService                   mUserService;
        private readonly IValidator<CreateUserInput>    mValidator;

        public CreateUser( IValidator<CreateUserInput> validator, IUserService userService ) {
            mValidator = validator;
            mUserService = userService;
        }

        [HttpPost]
        public override async Task<ActionResult<CreateUserResponse>> HandleAsync(
                    [FromBody] CreateUserInput request, 
                    CancellationToken cancellationToken = new()) {
            try {
                var validation = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validation.IsValid ) {
                    return Ok( new CreateUserResponse( validation ));
                }

                var user = await mUserService.CreateUser( request.Email, request.Name, request.Password );

                if( user != null ) {
                    return Ok( new CreateUserResponse());
                }

                return Ok( new CreateUserResponse( false, "User could not be created" ));
            }
            catch( Exception ex ) {
                return Ok( new CreateUserResponse( ex ));
            }
        }
    }
}
