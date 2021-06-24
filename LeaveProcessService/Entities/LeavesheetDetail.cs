using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveProcessService.Entities
{
    [Table("AT_LEAVESHEET")]
    public class LeavesheetDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Decimal ID { get; set; }
        public DateTime? WORKINGDAY { get; set; }
        public Decimal? MORNING_ID { get; set; }
        public Decimal? AFTERNOON_ID { get; set; }
        public Decimal? MANUAL_ID { get; set; }
        public Decimal? LEAVE_TIME_ID { get; set; }
        public Decimal? IS_RETURN_DAY_OFF { get; set; }
        public DateTime? RETURN_DAY_OFF_DATE { get; set; }
        public Decimal? LEAVE_ID_PH { get; set; }
        public Leavesheet Leavesheet { get; set; }
        public Decimal? LEAVE_DAYS { get; set; }
    }
}
