using System;
using AspNetCore.Identity.CosmosDb.Extensions;
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
using SquirrelsNest.Pecan.Server.Features.Projects;
using SquirrelsNest.Pecan.Server.Features.ProjectTemplates;
using SquirrelsNest.Pecan.Server.Features.Transfer;
using SquirrelsNest.Pecan.Server.Models;
using SquirrelsNest.Pecan.Server.Support;
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

    var connectionString = configuration.GetConnectionString( "DatabaseConnection" ) ?? String.Empty;
    var databaseName = configuration.GetValue<string>( "DatabaseName" ) ?? "PecanDB";
    var setupDatabase = configuration.GetValue<string>( "SetupDatabase" ) ?? "false";
    
    if( bool.TryParse( setupDatabase, out var setup ) && setup ) {
        var builder1 = new DbContextOptionsBuilder<PecanDbContext>();

        builder1.UseCosmos( connectionString, databaseName );

        using (var dbContext = new PecanDbContext( builder1.Options )) {
            dbContext.Database.EnsureCreated();
        }
    }

    services.AddDbContext<PecanDbContext>( options =>
        options.UseCosmos( connectionString: connectionString, databaseName: databaseName ));

    services.AddScoped<IDbContext, PecanDbContext>();
    services.AddScoped<ICompositeIssueBuilder, CompositeIssueBuilder>();
    services.AddScoped<ICompositeProjectBuilder, CompositeProjectBuilder>();
    services.AddScoped<ITokenBuilder, TokenBuilder>();
    services.AddEntityProviders();

    services.AddScoped<IProjectTemplateManager, ProjectTemplateManager>();
    services.AddScoped<IStreamWriter, StreamWriter>();
    services.AddScoped<IExportManager, ExportManager>();
    services.AddScoped<IImportManager, ImportManager>();
    services.AddScoped<IUserService, UserService>();

    services.AddValidatorsFromAssemblyContaining<CreateProjectInputValidator>();
}

void ConfigureSecurity( IServiceCollection services, ConfigurationManager configuration ) {
    // AddIdentity must be called before AddAuthentication
    services.AddCosmosIdentity<PecanDbContext, DbUser, IdentityRole>(
            options => PasswordRequirements.LoadPasswordRequirements( configuration, options )
        )
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
