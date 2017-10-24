using System;
using System.Runtime.Serialization.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;

namespace EmployeeApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.Unicode;

            RunProcess();
        }

        async static void RunProcess()
        {
            Console.WriteLine("Клиент для доступа к базе сотрудников");

            string last_name, first_name, middle_name, payment_type_id, payment_rate;
            Dictionary<string, string> parameters;

            Boolean condition = true;

            while (condition)
            {
                Console.WriteLine("[1]:    Добавить нового сотрудника.");
                Console.WriteLine("[2]:    Поиск сотрудника.");
                Console.WriteLine("[3]:    Вывести список сотрудников.");
                Console.WriteLine("[4]:    Вывести суммарную з/п всех сотрудников за месяц.");
                Console.WriteLine("[5]:    Вывести сотрудника с самой высокой почасовой ставкой.");
                Console.WriteLine("[Exit]: Завершить работу с приложением.");
                Console.WriteLine();

                string input = Console.ReadLine();
                Console.WriteLine();
                
                switch (input)
                {
                    case "1":
                        Console.WriteLine("Введите Фамилию: "); last_name = Console.ReadLine();
                        Console.WriteLine("Введите Имя: "); first_name = Console.ReadLine();
                        Console.WriteLine("Введите Отчество: "); middle_name = Console.ReadLine();
                        Console.WriteLine("Введите вид оплаты сотрудника: ");
                        Console.WriteLine("[1] Почасовая оплата."); Console.WriteLine("[2] Ежемесячная оплата."); payment_type_id = Console.ReadLine();
                        Console.WriteLine("Введите ставку з/п. (Учитывая вид оплаты): "); payment_rate = Console.ReadLine();

                        parameters = new Dictionary<string, string>
                        {
                            [nameof(first_name)] = first_name,
                            [nameof(last_name)] = last_name,
                            [nameof(middle_name)] = middle_name,
                            [nameof(payment_type_id)] = payment_type_id,
                            [nameof(payment_rate)] = payment_rate,
                        };

                        await PostWebResponseAsync("AddEmployee", parameters);

                        break;
                    case "2":
                        Console.WriteLine("Введите Фамилию: "); last_name = Console.ReadLine();
                        Console.WriteLine("Введите Имя: "); first_name = Console.ReadLine();
                        Console.WriteLine("Введите Отчество: "); middle_name = Console.ReadLine();

                        parameters = new Dictionary<string, string>
                        {
                            [nameof(first_name)] = first_name,
                            [nameof(last_name)] = last_name,
                            [nameof(middle_name)] = middle_name,
                        };

                        var searchedEmployees = GetWebResponseAsync<List<EmployeesResponseData>>("GetEmployee", parameters).Result;

                        if (searchedEmployees?.Count > 0)
                            foreach (var employee in searchedEmployees)
                            Console.WriteLine(employee.Id + "\t" + employee.AverageSalary.ToString("C", new CultureInfo("ru-RU")) + "\t" + employee.FullName);
                        else
                            Console.WriteLine("Сотрудник не найден");

                        break;
                    case "3":
                        var employees = GetWebResponseAsync<List<EmployeesResponseData>>("GetEmployees").Result;

                        foreach (var employee in employees.OrderByDescending(emp => emp.AverageSalary).ThenBy(emp => emp.FullName))
                        Console.WriteLine(employee.Id + "\t" + employee.AverageSalary.ToString("C", new CultureInfo("ru-RU")) + "\t" + employee.FullName);

                        SaveToXML(employees);

                        break;
                    case "4":
                        var res = GetWebResponseAsync<EmployeeResponseTotalSum>("GetTotalSalary").Result;

                        Console.WriteLine("Суммарная з/п: " + res.TotalSalarySum.ToString("C", new CultureInfo("ru-RU")));

                        break;
                    case "5":
                        var highestRateEmployee = GetWebResponseAsync<EmployeesResponseData>("GetEmployeeHighestTimeBasePayment").Result;

                         Console.WriteLine(highestRateEmployee.Id + "\t" + highestRateEmployee.AverageSalary.ToString("C", new CultureInfo("ru-RU")) + "\t" + highestRateEmployee.FullName);

                        break;
                    case "Exit":
                        condition = false;

                        break;
                    default:
                        Console.WriteLine("Введена некорректная команда");

                        break;
                }

                Console.WriteLine();
            }
        }
        static async Task<T> GetWebResponseAsync<T>(string method_name, Dictionary<string, string> dict = null)
        {
            Uri baseUri = new Uri("http://localhost:5000/api/");
            Uri targetUri = new Uri(baseUri, method_name);

            UriBuilder uriBuilder = new UriBuilder(targetUri);
            StringBuilder parameters = new StringBuilder();

            if (dict?.Count > 0)
            {
                foreach (var pair in dict)
                    parameters.Append(pair.Key + "=" + WebUtility.UrlEncode(pair.Value) + "&");
                parameters.Remove(parameters.Length - 1, 1);

                targetUri = new Uri(baseUri, method_name + "/?" + parameters.ToString());
            }

            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T));

                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(targetUri))
                using (HttpContent content = response.Content)
                {

                    var streamTask = content.ReadAsStreamAsync();

                    if (response.StatusCode != HttpStatusCode.OK)
                        return default(T);
                    else
                        return (T)serializer.ReadObject(await streamTask);
                }
            }
            catch (SerializationException ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }

        static async Task PostWebResponseAsync(string method_name, Dictionary<string, string> dict)
        {
            Uri baseUri = new Uri("http://localhost:5000/api/");
            Uri targetUri = new Uri(baseUri, method_name);

            UriBuilder uriBuilder = new UriBuilder(targetUri);
            StringBuilder parameters = new StringBuilder();
            
            var encodedContent = new FormUrlEncodedContent(dict);

            try
            {
                HttpClient client = new HttpClient();
                    HttpResponseMessage sResponse = await client.PostAsync(targetUri, encodedContent).ConfigureAwait(false);
                using (HttpResponseMessage response = await client.PostAsync(targetUri, encodedContent))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                        Console.WriteLine("Сотрудник был успешно добавлен.");
                    else
                        Console.WriteLine("Возникла ошибка при добавлении сотрудника.");
                }
            }
            catch (SerializationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void SaveToXML(List<EmployeesResponseData> list)
        {
            decimal avg_salary = list.Average(employeeData => employeeData.AverageSalary);

            var serializer = new XmlSerializer(list.GetType());

            using (FileStream fs = new FileStream("employees.xml", FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, list.Where(employeeData => employeeData.AverageSalary > avg_salary).ToList());

                Console.WriteLine("Xml-документ со списком сотрудников, чья заработная плата выше средней, был сохранен по пути: ");
                Console.WriteLine(fs.Name);
            }
        }
    }
}