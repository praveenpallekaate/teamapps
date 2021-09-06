using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// API config
    /// </summary>
    public class Startup
    {
        private const string AllowOrigin = "AllowOrigins";

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// ctor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets config
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    AllowOrigin,
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });

            services.AddControllers();

            services.AddTransient<AppLookupRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<ApplicationRepository>();
            services.AddTransient<ResourceRepository>();
            services.AddTransient<IAppLookupManagement, AppLookupManagement>();
            services.AddTransient<IUserManagement, UserManagement>();
            services.AddTransient<IApplicationManagement, ApplicationManagement>();
            services.AddTransient<IResourceManagement, ResourceManagement>();

            services.AddSwaggerGen();

            // Map appsettings config to class
            services.Configure<AppSettings>(Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(AllowOrigin);

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Teams API v1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
