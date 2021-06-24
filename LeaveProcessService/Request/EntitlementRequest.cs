using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveProcessService.Request
{
    public class EntitlementRequest
    {
        public string employeeIds { get; set; }
        public int year  { get; set; }
    }
}
