using CleaningManagement.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CleaningManagement.DAL.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCleaningManagementDal(this IServiceCollection services)
        {
            services.AddScoped<ICleaningPlanRepository, CleaningPlanRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
