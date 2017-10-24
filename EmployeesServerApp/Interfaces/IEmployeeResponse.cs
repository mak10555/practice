using Microsoft.AspNetCore.Builder;

namespace EmployeesServerApp.Interfaces
{
    public interface IEmployeeResponse
    {
        void GetEmployees(IApplicationBuilder appBuilder);
        void GetEmployee(IApplicationBuilder appBuilder);
        void AddEmployee(IApplicationBuilder appBuilder);
        void GetEmployeesTotalSalaryByMonth(IApplicationBuilder appBuilder);
        void GetEmployeesWithHighestTimeBasePayment(IApplicationBuilder appBuilder);
    }
}
