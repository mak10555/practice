using System.Runtime.Serialization;

namespace EmployeeApiClient
{
    [DataContract]
    public class EmployeesResponseData
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public decimal AverageSalary { get; set; }
    }
}