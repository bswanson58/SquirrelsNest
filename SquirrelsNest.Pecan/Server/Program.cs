using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SquirrelsNest.Pecan.Server.Database;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Server.Features.Auth;
using SquirrelsNest.Pecan.Server.Features.Issues;
using SquirrelsNest.Pecan.Server.Models;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices( builder.Services, builder.Configuration );
ConfigureSecurity( builder.Services, builder.Configuration );

var app = builder.Build();

ConfigurePipeline( app );

app.Run();


void ConfigureServices( IServiceCollection services, ConfigurationManager configuration ) {
    services.AddControllersWithViews();
    services.AddRazorPages();

    services.AddDbContext<PecanDbContext>( options => {
        options.UseSqlServer( configuration.GetConnectionString( "DatabaseConnection" ));
#if DEBUG
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
#endif
    } );

    services.AddScoped<IDbContext, PecanDbContext>();
    services.AddScoped<ICompositeIssueBuilder, CompositeIssueBuilder>();
    services.AddScoped<ITokenBuilder, TokenBuilder>();
    services.AddEntityProviders();

    services.AddValidatorsFromAssemblyContaining<CreateProjectInputValidator>();
}

void ConfigureSecurity( IServiceCollection services, ConfigurationManager configuration ) {
    // AddIdentity must be called before AddAuthentication
    services.AddIdentity<DbUser, IdentityRole>( 
        options => PasswordRequirements.LoadPasswordRequirements( configuration, options ))
        .AddEntityFrameworkStores<PecanDbContext>()
        .AddDefaultTokenProviders();

//    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

    var jwtSettings = configuration.GetSection( JWTConstants.JwtConfigSettings );

    services.AddAuthentication( options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    } )
        .AddJwtBearer( options => {
            options.TokenValidationParameters = TokenBuilder.CreateTokenValidationParameters( jwtSettings );
        } );
}

void ConfigurePipeline( WebApplication webApp ) {
    if( webApp.Environment.IsDevelopment() ) {
        webApp.UseWebAssemblyDebugging();
    }
    else {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        webApp.UseHsts();
    }

    webApp.UseHttpsRedirection();
    webApp.UseBlazorFrameworkFiles();
    webApp.UseStaticFiles();

    webApp.UseRouting();

    webApp.MapRazorPages();
    webApp.MapControllers();
    webApp.MapFallbackToFile( "index.html" );

    webApp.UseAuthentication();
    webApp.UseAuthorization();
}
