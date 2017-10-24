using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesServerApp.Entities
{
    public abstract class Employee
    {
        public decimal PaymentRate { get; protected set; }
        public int Id { get; protected set; }
        public string FirstName { get; protected set; }
        public string MiddleName { get; protected set; }
        public string LastName { get; protected set; }
        public abstract decimal GetAverageSalary();
    }
}
