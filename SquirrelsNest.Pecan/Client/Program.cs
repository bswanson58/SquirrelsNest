using System;
using System.Net.Http;
using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SquirrelsNest.Pecan.Client;
using SquirrelsNest.Pecan.Client.Projects;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>( "#app" );
builder.RootComponents.Add<HeadOutlet>( "head::after" );

builder.Services.AddHttpClient( "SquirrelsNest.Pecan.ServerAPI", 
    client => client.BaseAddress = new Uri( builder.HostEnvironment.BaseAddress ));
// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped( sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient( "SquirrelsNest.Pecan.ServerAPI" ));

builder.Services.AddScoped( typeof( ProjectFacade ));

builder.Services.AddFluxor( options => options.ScanAssemblies( typeof( App ).Assembly ));

await builder.Build().RunAsync();
