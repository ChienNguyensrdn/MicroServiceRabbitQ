using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveProcessService.Request
{
    public class EntitlementRequest
    {
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage ="Max length 20")]
        public string leaveEOfficeId { get; set; }
        [Required]
        public int employeeId { get; set; }
        [Required]
        [DefaultValue("21062021")]
        public string leaveFrom { get; set; }
        [Required]
        [DefaultValue("21062021")]
        public string leaveTo { get; set; }
        [Required]
        public int manualId { get; set; }
        [Required]
        public int leaveTimeId { get; set; }
        public string leavePlace { get; set; }
        public string leaveReason { get; set; }
        [DefaultValue(false)]
        public Boolean isOffshore { get; set; }
        public string note { get; set; }
        [Required]
        public int leaveReplaceId { get; set; }
        [Required]
        public decimal balanceNow { get; set; }
        [DefaultValue("EOffice")]
        public string userLog { get; set; }
    }
}
