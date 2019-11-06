using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Core;

namespace Minenetred.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .WriteTo.File("Logs.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
               .CreateLogger();
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, "Fatal error");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseStartup<Startup>();
    }
}