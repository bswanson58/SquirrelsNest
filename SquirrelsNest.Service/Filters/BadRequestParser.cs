using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SquirrelsNest.Service.Filters {
    public class BadRequestParser : IActionFilter {
        public void OnActionExecuted( ActionExecutedContext context ) {
            var result = context.Result as IStatusCodeActionResult;
            if( result == null ) {
                return;
            }

            var statusCode = result.StatusCode;
            if( statusCode == 400 ) {
                var response = new List<string>();

                if( context.Result is BadRequestObjectResult badRequestObjectResult ) {
                    if( badRequestObjectResult.Value is string) {
                        response.Add( badRequestObjectResult.Value.ToString() ?? string.Empty );
                    }
                    else if( badRequestObjectResult.Value is IEnumerable<IdentityError> errors ) {
                        foreach( var error in errors ) {
                            response.Add( error.Description );
                        }
                    }
                    else {
                        foreach( var key in context.ModelState.Keys ) {
                            if(!String.IsNullOrWhiteSpace( key )) {
                                var errorList = context.ModelState[key]?.Errors;

                                if( errorList != null ) {
                                    foreach( var error in errorList ) {
                                        response.Add( $"{key}: {error.ErrorMessage}" );
                                    }
                                }
                            }
                        }
                    }

                }

                context.Result = new BadRequestObjectResult( response );
            }
        }

        public void OnActionExecuting( ActionExecutingContext context ) {
        }
    }
}
