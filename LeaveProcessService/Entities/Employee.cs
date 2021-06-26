using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveProcessService.Entities
{
    [Table("HU_EMPLOYEE")]
    public class Employee
    {
        public int Id { get; set; }
        public string employeeCode { get; set; }
        public string  fullName { get; set; }
        public Boolean is3B { get; set; }
        public DateTime? terLastDate { get; set; }
    }
}
