using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LeaveProcessService.Request
{
    public class ApproveRequest
    {
        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "* Part numbers must be between 1 and 50 character in length,List ID split ,")]
        [Description("Part numbers must be between 1 and 50 character in length,List ID split ,")]
        [DefaultValue("34,5,6")]
        public string leavesheetIds { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "List status aprove code in {APPROVE1,APPROVE2,RESET,REJECT}")]
        [DisplayName("List status aprove code in {APPROVE1,APPROVE2,RESET,REJECT}")]
        [DefaultValue("APPROVE1")]
        public string statusCode { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "User Login")]
        [DisplayName("User Login")]
        public string userName { get; set; }
    }
}
