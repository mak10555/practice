using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace EmployeesServerApp.Entities
{
    public sealed class EmployeeWage : Employee
    {
        public EmployeeWage(int id, decimal salaryRate, string firstName, string middleName, string lastName)
        {
            Id = id;
            PaymentRate = salaryRate;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }
        public override decimal GetAverageSalary()
                 => 20.8m * 8m * PaymentRate;
    }
}
