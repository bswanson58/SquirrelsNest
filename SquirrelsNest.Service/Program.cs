using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core;
using SquirrelsNest.Core.Preferences;
using SquirrelsNest.EfDb;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Issues;
using SquirrelsNest.Service.Projects;

var appBuilder = WebApplication.CreateBuilder( args );

// Use AutoFac as the container
appBuilder.Host.UseServiceProviderFactory( new AutofacServiceProviderFactory());
appBuilder.Host.ConfigureContainer<ContainerBuilder>( ConfigureDependencies );

ConfigureConfiguration( appBuilder.Configuration );
ConfigureServices( appBuilder.Services );

var app = appBuilder.Build();

ConfigureMiddleware( app, app.Services );
ConfigureEndpoints( app, app.Services );

app.Run();

void ConfigureDependencies( ContainerBuilder builder ) {
    builder
        .RegisterModule<CoreModule>()
        .RegisterModule<EfDbModule>();

    builder.RegisterType<Preferences<EfDatabaseConfiguration>>().As<IPreferences<EfDatabaseConfiguration>>();
}

void ConfigureConfiguration( ConfigurationManager configuration ) {
}

void ConfigureServices( IServiceCollection services ) {
    services
        .AddGraphQLServer()
        .AddQueryType()
        .AddTypeExtension<ProjectQuery>()
        .AddTypeExtension<IssueQuery>()
        .AddType<ClProject>()
        .AddType<ClIssue>()
        .AddFiltering()
        .AddSorting();
}

void ConfigureMiddleware( IApplicationBuilder serviceBuilder, IServiceProvider services ) {
}

void ConfigureEndpoints( IEndpointRouteBuilder routeBuilder, IServiceProvider services ) {
    routeBuilder.MapGraphQL( "/api" );

    routeBuilder.MapGet( "/", context => {
        context.Response.Redirect( "/api", true );

        return Task.CompletedTask;
    });
}
