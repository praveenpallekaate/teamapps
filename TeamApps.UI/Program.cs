using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TeamApps.UI
{
    /// <summary>
    /// Entry
    /// </summary>
    public class Program
    {
        /// <summary>
        /// App entry
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("app");
            builder.Services.AddScoped<UserContext>();

            builder.Services.AddHttpClient<IUserService, UserService>(client => client.BaseAddress = new Uri(builder.Configuration["APIUrl"]));
            builder.Services.AddHttpClient<ITeamAppsService, TeamAppsService>(client => client.BaseAddress = new Uri(builder.Configuration["APIUrl"]));
            builder.Services.AddHttpClient<IResourceService, ResourceService>(client => client.BaseAddress = new Uri(builder.Configuration["APIUrl"]));

            // Added Httpclient scoped is only for Matblazor table
            // as httpclient factory is used as above for services
            builder.Services.AddScoped<HttpClient>();

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            });

            await builder.Build().RunAsync();
        }
    }
}
