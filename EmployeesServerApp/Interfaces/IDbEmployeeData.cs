using System.Collections.Generic;
using EmployeesServerApp.Entities;

namespace EmployeesServerApp.Interfaces
{
    public interface IDbEmployeeData
    {
       List<Employee> GetEmployees();
       List<Employee> GetEmployee(string firstName, string middleName, string lastName);
       int AddEmployee(string firstName, string middleName, string lastName, string payment_rate, string payment_type_id);
       decimal GetEmployeesTotalSalaryByMonth();
       Employee GetEmployeesWithHighestTimeBasePayment();
    }
}
