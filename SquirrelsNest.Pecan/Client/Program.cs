using System;
using System.Net.Http;
using Blazored.LocalStorage;
using FluentValidation;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using SquirrelsNest.Pecan.Client;
using SquirrelsNest.Pecan.Client.Auth.Store;
using SquirrelsNest.Pecan.Client.Auth.Support;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Client.Gravatar;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Client.Issues.Support;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Client.ProjectTemplates.Store;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Client.Ui;
using SquirrelsNest.Pecan.Client.Ui.Store;
using SquirrelsNest.Pecan.Client.UserData;
using SquirrelsNest.Pecan.Client.UserData.Store;
using SquirrelsNest.Pecan.Client.Users.Store;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

ConfigureRootComponents( builder.RootComponents );
ConfigureServices( builder.Services );

await builder.Build().RunAsync();


void ConfigureRootComponents( RootComponentMappingCollection root ) {
    root.Add<App>( "#app" );
    root.Add<HeadOutlet>( "head::after" );
}

void ConfigureServices( IServiceCollection services ) {
    services.AddHttpClient( HttpClientNames.Authenticated, 
                            client => client.BaseAddress = new Uri( builder.HostEnvironment.BaseAddress ))
        .AddHttpMessageHandler<JwtTokenHandler>();
    services.AddScoped( sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient( HttpClientNames.Authenticated ));
    services.AddScoped<JwtTokenHandler>();

    services.AddHttpClient( HttpClientNames.Anonymous,
                            client => client.BaseAddress = new Uri( builder.HostEnvironment.BaseAddress ));

    services.AddScoped( sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient( HttpClientNames.Anonymous ));

    services.AddScoped<ITokenRefresher, JwtTokenRefresher>();
    services.AddScoped<ITokenExpirationChecker, TokenExpirationChecker>();

    services.AddScoped<IResponseStatusHandler, ResponseStatusHandler>();
    services.AddScoped<IAuthenticatedHttpHandler, AuthenticatedHttpHandler>();
    services.AddScoped<IAnonymousHttpHandler, AnonymousHttpHandler>();

    services.AddScoped<IAppStartup, AppStartup>();
    services.AddScoped<IUserDataService, UserDataService>();
    services.AddScoped<AuthFacade>();
    services.AddScoped<IssueFacade>();
    services.AddScoped<ProjectFacade>();
    services.AddScoped<ProjectTemplatesFacade>();
    services.AddScoped<UiFacade>();
    services.AddScoped<UserFacade>();
    services.AddScoped<UserDataFacade>();
    services.AddScoped<AnnouncementHandler>();

    services.AddScoped<IIssueRetriever, IssueRetriever>();

    services.AddScoped<IGravatarClient, GravatarClient>();

    services.AddFluxor( options => options.ScanAssemblies( typeof( App ).Assembly ));

    services.AddValidatorsFromAssemblyContaining<CreateProjectInputValidator>();

    services.AddBlazoredLocalStorage();
    services.AddBlazorDownloadFile();

    services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
    services.AddScoped<IAuthInformation, AuthInformation>();
    services.AddAuthorizationCore();

    services.AddMudServices();
}