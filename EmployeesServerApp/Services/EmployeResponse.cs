using System.Text;
using EmployeesServerApp.Interfaces;
using EmployeesServerApp.Models;
using Microsoft.AspNetCore.Builder;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace EmployeesServerApp.Services
{
    public class EmployeResponse : IEmployeeResponse
    {
        IDbEmployeeData _dbData;

        public EmployeResponse(IDbEmployeeData dbData)
        {
            _dbData = dbData;
        }

        public void GetEmployees(IApplicationBuilder app)
        {
            var res = (from emp in _dbData.GetEmployees()
                       let sal = emp.GetAverageSalary()
                       let fullname = emp.LastName + " " + emp.FirstName + " " + emp.MiddleName
                       orderby sal descending
                       select new EmployeesResponseData { Id = emp.Id, FullName = fullname, AverageSalary = sal }).ToList();

            string result = ConvertToJson(res);

            app.Run(async context =>
            {
                if (res.Count == 0)
                    context.Response.StatusCode = 204;

                await context.Response.WriteAsync(
                    result
                    );
            });
        }

        public void GetEmployee(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var res = (from emp in _dbData.GetEmployee(context.Request.Query["first_name"], context.Request.Query["middle_name"], context.Request.Query["last_name"])
                           let sal = emp.GetAverageSalary()
                           let fullname = emp.LastName + " " + emp.MiddleName + " " + emp.LastName
                           select new EmployeesResponseData { Id = emp.Id, FullName = fullname, AverageSalary = sal }).ToList();

                string result = ConvertToJson(res);

                if (res.Count == 0)
                    context.Response.StatusCode = 204;

                await context.Response.WriteAsync(
                    result
                    );
            });
        }

        public void AddEmployee(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                
                int res = _dbData.AddEmployee(
                     
                                                context.Request.Form["first_name"],
                                                context.Request.Form["middle_name"],
                                                context.Request.Form["last_name"],
                                                context.Request.Form["payment_rate"],
                                                context.Request.Form["payment_type_id"]);

                if (res == 0)
                    context.Response.StatusCode = 404;

                await context.Response.WriteAsync(
                "{ list: [] }"
                );
            });
        }

        public void GetEmployeesTotalSalaryByMonth(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                decimal TotalSalarySum = _dbData.GetEmployeesTotalSalaryByMonth();

                string result = ConvertToJson(TotalSalarySum);

                NumberFormatInfo nfi = new CultureInfo("en-US").NumberFormat;

                nfi.NumberDecimalSeparator = ".";

                await context.Response.WriteAsync(
                            "{\"" + nameof(TotalSalarySum) + "\":\"" + TotalSalarySum.ToString(nfi) + "\"}"
                            );
            });
        }

        public void GetEmployeesWithHighestTimeBasePayment(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                EmployeesResponseData curEmployeeResponse = new EmployeesResponseData(_dbData.GetEmployeesWithHighestTimeBasePayment());

                if (curEmployeeResponse == null)
                    context.Response.StatusCode = 204;
                
                string result = ConvertToJson(curEmployeeResponse);

                await context.Response.WriteAsync(
                    result
                    );
                
            });
        }

        private string ConvertToJson<T>(T serialazableObject)
        {
            MemoryStream ms = new MemoryStream();

            DataContractJsonSerializer ser = new DataContractJsonSerializer(
                typeof(T)
                    );

            ser.WriteObject(ms, serialazableObject);

            byte[] json = ms.ToArray();

            string result = /*'\uFEFF' + */Encoding.UTF8.GetString(json, 0, json.Length);

            return result;
        }

    }
}
