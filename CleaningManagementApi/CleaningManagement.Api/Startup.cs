using CleaningManagement.Api.Authentification;
using CleaningManagement.DAL;
using CleaningManagement.DAL.Infrastructure;
using CleaningManagement.Service.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CleaningManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CleaningManagementDbContext>(opt => opt.UseInMemoryDatabase("CleaningContext"));

            services.AddCleaningManagementService();
            services.AddControllers();
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            string authentificationName = "BasicAuthentification";
            services.AddAuthentication(authentificationName)
                    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(authentificationName, null);

            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1",
                            new OpenApiInfo()
                            {
                                Version = "v1",
                                Title = "Cleaning plan API",
                                Description = "Cleaning plan management API",
                            });

                o.AddSecurityDefinition("basic", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Basic authorization header."
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[0]
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "Cleaning plan API v1");
                o.RoutePrefix = string.Empty;
            });

            var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<CleaningManagementDbContext>();

            DbSeeder.Seed(dbContext);
        }
    }
}
