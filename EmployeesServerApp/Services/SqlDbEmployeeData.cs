using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EmployeesServerApp.Entities;
using EmployeesServerApp.Interfaces;
using System.Linq;
using System.Net;

namespace EmployeesServerApp
{
    public class SqlDbEmployeeData : IDbEmployeeData
    {
        readonly string _connectionString;

        public SqlDbEmployeeData(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection");
        public List<Employee> GetEmployees()
        {
            string proc_name = "dbo.employees_get_all";

            List<Employee> emp_list = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(proc_name, connection))
                {
                    connection.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            Employee employee;
                            if (reader.GetFieldValue<int>(4) == 1)
                            {
                                employee = new EmployeeSalary(
                                    reader.GetFieldValue<int>(0),
                                    reader.GetFieldValue<decimal>(5),
                                    reader.GetFieldValue<string>(1),
                                    reader.IsDBNull(2) ? null : reader.GetFieldValue<string>(2),
                                    reader.GetFieldValue<string>(3)
                                    );

                            }
                            else
                            {
                                employee = new EmployeeWage(
                                    reader.GetFieldValue<int>(0),
                                    reader.GetFieldValue<decimal>(5),
                                    reader.GetFieldValue<string>(1),
                                    reader.IsDBNull(2) ? null : reader.GetFieldValue<string>(2),
                                    reader.GetFieldValue<string>(3)
                                    );
                            }

                            emp_list.Add(employee);
                        }

                }

            return emp_list;
        }
        public List<Employee> GetEmployee(string firstName, string middleName, string lastname)
        {

            string proc_name = new StringBuilder()
                                    .Append("exec dbo.employees_get_by_name ")
                                    .Append(String.IsNullOrEmpty(firstName) ? "null" : "\"" + firstName + "\"").Append(",")
                                    .Append(String.IsNullOrEmpty(middleName) ? "null" : "\"" + middleName + "\"").Append(",")
                                    .Append(String.IsNullOrEmpty(lastname) ? "null" : "\"" + lastname + "\"")
                                    .ToString();

            List<Employee> emp_list = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(proc_name, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            Employee employee;
                            if (reader.GetFieldValue<int>(4) == 1)
                            {
                                employee = new EmployeeSalary(
                                    reader.GetFieldValue<int>(0),
                                    reader.GetFieldValue<decimal>(5),
                                    reader.GetFieldValue<string>(1),
                                    reader.GetFieldValue<string>(2),
                                    reader.GetFieldValue<string>(3)
                                    );

                            }
                            else
                            {
                                employee = new EmployeeWage(
                                    reader.GetFieldValue<int>(0),
                                    reader.GetFieldValue<decimal>(5),
                                    reader.GetFieldValue<string>(1),
                                    reader.GetFieldValue<string>(2),
                                    reader.GetFieldValue<string>(3)
                                    );
                            }

                            emp_list.Add(employee);
                        }

                }

            return emp_list;
        }
        public int AddEmployee(string firstName, string middleName, string lastname, string paymentRate, string paymentTypeId)
        {

            string proc_name = new StringBuilder()
                                    .Append("exec dbo.employees_add ")
                                    .Append(String.IsNullOrEmpty(firstName) ? "null" : "\"" + firstName + "\"").Append(",")
                                    .Append(String.IsNullOrEmpty(middleName) ? "null" : "\"" + middleName + "\"").Append(",")
                                    .Append(String.IsNullOrEmpty(lastname) ? "null" : "\"" + lastname + "\"").Append(",")
                                    .Append(String.IsNullOrEmpty(paymentTypeId) ? "null" : paymentTypeId).Append(",")
                                    .Append(String.IsNullOrEmpty(paymentRate) ? "null" : paymentRate)
                                    .ToString();

            int result = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(proc_name, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    result = cmd.ExecuteNonQuery();
                }

            return result;
        }
        public decimal GetEmployeesTotalSalaryByMonth() => GetEmployees().Sum(employee => employee.GetAverageSalary());
        public Employee GetEmployeesWithHighestTimeBasePayment() => GetEmployees().Where(employee => employee is EmployeeWage).OrderByDescending(employee => employee.PaymentRate).FirstOrDefault();
    }
}