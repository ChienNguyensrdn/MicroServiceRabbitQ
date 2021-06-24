using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveProcessService.Entities
{
    [Table("AT_LEAVESHEET_PH")]
    public class Leavesheet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Decimal ID { get; set; }
        public Decimal EMPLOYEE_ID { get; set; }
        public String LEAVE_NO { get; set; }
        public DateTime? LEAVE_FROM { get; set; }
        public DateTime? LEAVE_TO { get; set; }
        public int? IS_APPROVE { get; set; }
        public String NOTE { get; set; }
        public Decimal? DAYS_NON_SAL { get; set; }
        public Decimal? DAYS_SAL { get; set; }
        public Decimal? LEAVE_TIME_ID { get; set; }
        //noi nghi
        public String  LEAVE_PLACE { get; set; }
        //nghi trong nuoc hay nuoc ngoai
        public bool? IS_OFFSHORE { get; set; }
        // nguoi thay the 
        public Decimal? LEAVE_REPLACE_ID { get; set; }
        // ly do
        public String LEAVE_REASON { get; set; }
        //tra phep 
        public bool? IS_RETURN_DAY_OFF { get; set; }
        public List<LeavesheetDetail> LeavesheetDetails { get; set; }
    }
}
