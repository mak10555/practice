using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using EmployeesServerApp.Interfaces;
using EmployeesServerApp.Services;

namespace EmployeesServerApp
{
    class Startup
    {
        IConfiguration configuration;

        public Startup()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var conn = configuration.GetConnectionString("DefaultConnection");

            services.AddSingleton(provider => configuration);

            services.AddSingleton<IDbEmployeeData, SqlDbEmployeeData>();

            services.AddSingleton<IEmployeeResponse, EmployeResponse>();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IEmployeeResponse responseConfiguration)
        {

            app.Map("/api",
                api =>
                {
                api.Map("/GetEmployees", responseConfiguration.GetEmployees);
                api.Map("/GetEmployee",
                    api11 =>
                    {
                        api11.MapWhen(
                            context => context.Request.Query.ContainsKey("first_name") &&
                                       context.Request.Query.ContainsKey("last_name"),
                            responseConfiguration.GetEmployee);
                    });
                api.Map("/AddEmployee",
                     api12 =>
                     {
                        api12.MapWhen(
                            context => context.Request.Form.ContainsKey("first_name") &&
                                       context.Request.Form.ContainsKey("middle_name") &&
                                       context.Request.Form.ContainsKey("last_name") &&
                                       context.Request.Form.ContainsKey("payment_rate") &&
                                       context.Request.Form.ContainsKey("payment_type_id"),
                            responseConfiguration.AddEmployee);
                    });
                api.Map("/GetTotalSalary", responseConfiguration.GetEmployeesTotalSalaryByMonth);
                api.Map("/GetEmployeeHighestTimeBasePayment", responseConfiguration.GetEmployeesWithHighestTimeBasePayment);
                });

        }
    }
}
