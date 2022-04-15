using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core;
using SquirrelsNest.Core.Preferences;
using SquirrelsNest.EfDb;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.Service;
using SquirrelsNest.Service.Database;
using SquirrelsNest.Service.Filters;
using SquirrelsNest.Service.Issues;
using SquirrelsNest.Service.Projects;
using SquirrelsNest.Service.Users;

const string    corsPolicy = "corsPolicy";
const string    apiEndpoint = "/api";

var appBuilder = WebApplication.CreateBuilder( args );

// Use AutoFac as the container
appBuilder.Host.UseServiceProviderFactory( new AutofacServiceProviderFactory() );
appBuilder.Host.ConfigureContainer<ContainerBuilder>( ConfigureDependencies );

ConfigureServices( appBuilder.Services, appBuilder.Configuration );

var app = appBuilder.Build();

ConfigureMiddleware( app );
ConfigureEndpoints( app );

app.Run();

void ConfigureDependencies( ContainerBuilder builder ) {
    builder
        .RegisterModule<CoreModule>()
        .RegisterModule<EfDbModule>()
        .RegisterModule<ServiceModule>();

    builder.RegisterType<Preferences<EfDatabaseConfiguration>>().As<IPreferences<EfDatabaseConfiguration>>();
}

void ConfigureServices( IServiceCollection services, ConfigurationManager configuration ) {
    services.AddHttpContextAccessor();

    services.AddControllers(options => {
        options.Filters.Add( typeof( ExceptionFilter ));
        options.Filters.Add(typeof( BadRequestParser ));
    }).ConfigureApiBehaviorOptions( BadRequestsBehavior.Parse );

    services.AddDbContext<ServiceDbContext>( options =>
        options.UseSqlServer( configuration.GetConnectionString( "ServiceConnection" )));
    
    services.AddIdentity<IdentityUser, IdentityRole>( options => {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddEntityFrameworkStores<ServiceDbContext>()
        .AddDefaultTokenProviders();

    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

    services.AddAuthentication( options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer( options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( configuration["JwtKey"] ) ),
                ClockSkew = TimeSpan.Zero
            };
        } );

    services.AddAuthorization( options => {
        options.AddPolicy( "IsAdmin", policy => {
            policy.RequireClaim( "role", "admin" );
            policy.AuthenticationSchemes.Add( JwtBearerDefaults.AuthenticationScheme );
        });
    });

    services.AddAuthorization( options => {
        options.AddPolicy( "IsUser", policy => {
            policy.RequireClaim( "role", "user" );
            policy.AuthenticationSchemes.Add( JwtBearerDefaults.AuthenticationScheme );
        });
    });

    services
        .AddGraphQLServer()
        .RegisterDbContext<ServiceDbContext>()
        .AddAuthorization()
        .AddQueryType()
        .AddTypeExtension<ProjectQuery>()
        .AddTypeExtension<IssueQuery>()
        .AddTypeExtension<Authentication>()
        .AddMutationType()
        .AddTypeExtension<IssueMutations>()
        .AddFiltering()
        .AddSorting();

    services.AddCors( options => options
            .AddPolicy( corsPolicy, builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
}

void ConfigureMiddleware( IApplicationBuilder serviceBuilder ) {
    serviceBuilder.UseCors( corsPolicy );

    serviceBuilder.UseAuthentication();
    serviceBuilder.UseAuthorization();
}

void ConfigureEndpoints( IEndpointRouteBuilder routeBuilder ) {
    routeBuilder.MapControllers();

    routeBuilder.MapGraphQL( apiEndpoint );

    routeBuilder.MapGet( "/", context => {
        context.Response.Redirect( apiEndpoint, true );

        return Task.CompletedTask;
    } );
}
