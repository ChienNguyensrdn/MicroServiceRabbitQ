using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LeaveProcessService.Entities
{    public class Entitlement
    {
        [Required]
        public int employeeId  { get; set; }
        public string employeeCode {get;set;}
        public string employeeName {get;set;}
        public string titleName {get;set;}
        public string orgName {get;set;}
        public decimal? curHave {get;set;}
    }
}
