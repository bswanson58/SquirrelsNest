using Microsoft.Extensions.DependencyInjection;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public static class ProviderRegistration {
        public static void AddEntityProviders( this IServiceCollection services ) {
            services.AddScoped<IAssociationProvider, SnAssociationProvider>();
            services.AddScoped<IComponentProvider, SnComponentProvider>();
            services.AddScoped<IIssueProvider, SnIssueProvider>();
            services.AddScoped<IIssueTypeProvider, SnIssueTypeProvider>();
            services.AddScoped<IProjectProvider, SnProjectProvider>();
            services.AddScoped<IReleaseProvider, SnReleaseProvider>();
            services.AddScoped<IUserProvider, SnUserProvider>();
            services.AddScoped<IUserDataProvider, SnUserDataProvider>();
            services.AddScoped<IWorkflowStateProvider, SnWorkflowStateProvider>();
        }
    }
}
