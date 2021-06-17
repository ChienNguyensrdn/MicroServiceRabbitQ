using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace LeaveProcessService.Entities
{
    [Table("SNP_CR.OT_OTHER_LIST_TYPE")]
    public class OtherListType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string CODE { get; set; }
        public string NAME{ get; set; }
    }
}
