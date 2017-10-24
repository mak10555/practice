using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;

namespace EmployeeApiClient
{
   [DataContract]
    class EmployeeResponseTotalSum
    {
        [DataMember]
        public decimal TotalSalarySum;
    }
}
