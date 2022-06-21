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
using SquirrelsNest.Service.Support;
using SquirrelsNest.Service.Users;

const string    corsPolicy = "corsPolicy";
const string    apiEndpoint = "/api";
const string    jwtKey = "JwtKey";

var appBuilder = WebApplication.CreateBuilder( args );

// Use AutoFac as the container
appBuilder.Host.UseServiceProviderFactory( new AutofacServiceProviderFactory() );
appBuilder.Host.ConfigureContainer<ContainerBuilder>( ConfigureDependencies );

ConfigureServices( appBuilder.Services, appBuilder.Configuration );

var app = appBuilder.Build();

ConfigureMiddleware( app );
ConfigureEndpoints( app );

await SeedDatabase( app );

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
        options.UseSqlServer( configuration.GetConnectionString( "DatabaseConnection" )));
    
    services.AddIdentity<IdentityUser, IdentityRole>( options => {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.User.RequireUniqueEmail = true;
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
                IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( configuration[jwtKey] ) ),
                ClockSkew = TimeSpan.Zero
            };
        } );

    services.AddAuthorization( options => {
        options.AddPolicy( PolicyNames.AdminPolicy, policy => {
            policy.RequireClaim( ClaimValues.ClaimRole, ClaimValues.ClaimRoleAdmin );
            policy.AuthenticationSchemes.Add( JwtBearerDefaults.AuthenticationScheme );
        });
    });

    services.AddAuthorization( options => {
        options.AddPolicy( PolicyNames.UserPolicy, policy => {
            policy.RequireClaim( ClaimValues.ClaimRole, ClaimValues.ClaimRoleUser );
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

static async Task SeedDatabase( IHost host ) {
    var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
    var log = host.Services.GetService<ILog>();

    if(( scopeFactory != null ) &&
       ( log != null )) {
        using var scope = scopeFactory.CreateScope();

        var databaseSeeder = scope.ServiceProvider.GetService<DatabaseInitializer>();

        if( databaseSeeder != null ) {
            var initError = await databaseSeeder.InitializeDatabase();

            initError.IfLeft( error => {
                log.LogMessage( error.Message );
            });
        }
    }
}

