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
using SquirrelsNest.Service.Database;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Filters;
using SquirrelsNest.Service.Issues;
using SquirrelsNest.Service.Projects;

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
        .RegisterModule<EfDbModule>();

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
    
    services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ServiceDbContext>()
        .AddDefaultTokenProviders();

    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

    services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
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
        options.AddPolicy( "IsAdmin", policy => policy.RequireClaim( "role", "admin" ));
    });

    services
        .AddGraphQLServer()
        .AddQueryType()
        .AddTypeExtension<ProjectQuery>()
        .AddTypeExtension<IssueQuery>()
        .AddType<ClProject>()
        .AddType<ClIssue>()
        .AddFiltering()
        .AddSorting();

    services.AddCors( options => options
            .AddPolicy( corsPolicy, builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader() ) );
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
