using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveProcessService.Request
{
    public class ValidateLeavesheet
    {
        [Required]
        public Decimal? Id { get; set; }
        [Required]
        public Decimal employeeId { get; set; }
        [Required]
        [StringLength(8,MinimumLength =8)]
        [DefaultValue("21062021")]
        public String leaveFrom { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 8)]
        [DefaultValue("21062021")]
        public String leaveTo { get; set; }
        [Required]
        public Decimal leaveType { get; set; }
        [Required]
        public Decimal leaveTime { get; set; }
    }
}
