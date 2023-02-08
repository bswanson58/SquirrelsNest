using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SquirrelsNest.Service.Filters {
    public static class BadRequestsBehavior {
        public static void Parse( ApiBehaviorOptions options ) {
            options.InvalidModelStateResponseFactory = context => {
                var response = new List<string>();

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

                return new BadRequestObjectResult( response );
            };
        }
    }
}
