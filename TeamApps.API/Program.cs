using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TeamApps.API
{
    /// <summary>
    /// App Entry
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Host builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
