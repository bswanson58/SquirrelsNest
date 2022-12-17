using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SquirrelsNest.Pecan.Server.Database;
using SquirrelsNest.Pecan.Server.Database.DataProviders;

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

    services.AddEntityProviders();
}

void ConfigurePipeline( WebApplication app ) {
    if( app.Environment.IsDevelopment() ) {
        app.UseWebAssemblyDebugging();
    }
    else {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile( "index.html" );
}
