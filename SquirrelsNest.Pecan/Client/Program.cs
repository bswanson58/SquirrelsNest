using System;
using System.Net.Http;
using FluentValidation;
using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using SquirrelsNest.Pecan.Client;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Shared.Dto;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

ConfigureRootComponents( builder.RootComponents );
ConfigureServices( builder.Services );

await builder.Build().RunAsync();


void ConfigureRootComponents( RootComponentMappingCollection root ) {
    root.Add<App>( "#app" );
    root.Add<HeadOutlet>( "head::after" );
}

void ConfigureServices( IServiceCollection services ) {
    services.AddHttpClient( "SquirrelsNest.Pecan.ServerAPI", 
        client => client.BaseAddress = new Uri( builder.HostEnvironment.BaseAddress ));
// Supply HttpClient instances that include access tokens when making requests to the server project
    services.AddScoped( sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient( "SquirrelsNest.Pecan.ServerAPI" ));

    services.AddScoped<ProjectFacade>();

    services.AddFluxor( options => options.ScanAssemblies( typeof( App ).Assembly ));

    services.AddValidatorsFromAssemblyContaining<CreateProjectInputValidator>();

    services.AddMudServices();
}