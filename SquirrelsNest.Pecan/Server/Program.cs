using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SquirrelsNest.Pecan.Server.Database;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Dto;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices( builder.Services, builder.Configuration );

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
    });

    services.AddIdentity<IdentityUser, IdentityRole>( options => {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<PecanDbContext>();

    services.AddScoped<IDbContext, PecanDbContext>();
    services.AddEntityProviders();

    services.AddValidatorsFromAssemblyContaining<CreateProjectInputValidator>();
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
