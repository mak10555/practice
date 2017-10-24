using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using EmployeesServerApp.Entities;

namespace EmployeesServerApp.Models
{
    [DataContract]
    public class EmployeesResponseData
    {
        public EmployeesResponseData()
        {

        }
        public EmployeesResponseData(Employee employee)
        {
            Id = employee.Id;
            AverageSalary = employee.GetAverageSalary();
            FullName = employee.LastName + " " + employee.FirstName + " " + employee.MiddleName;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public decimal AverageSalary { get; set; }
    }
}
