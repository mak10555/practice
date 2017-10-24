using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Builder;

namespace EmployeesServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var host = new WebHostBuilder()
                                .UseKestrel()
                                .UseUrls("http://+:5000/")
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseIISIntegration()
                                .UseStartup<Startup>()
                                .Build();

            return host;
        }
    }
}