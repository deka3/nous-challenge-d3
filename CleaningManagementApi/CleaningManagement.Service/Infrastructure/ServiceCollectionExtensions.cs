using CleaningManagement.DAL.Infrastructure;
using CleaningManagement.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleaningManagement.Service.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCleaningManagementService(this IServiceCollection services)
        {
            services.AddCleaningManagementDal();
            services.AddScoped<ICleaningManagementService, CleaningManagementService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
