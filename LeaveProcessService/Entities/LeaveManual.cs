using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveProcessService.Entities
{
    [Table("AT_FML")]
    public class LeaveManual
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string CODE { get; set; }
        public string NAME_VN { get; set; }
        public string NAME_EN { get; set; }
        public string NOTE { get; set; }
        public string ACTFLG { get; set; }
        public bool IS_LEAVE { get; set; }
    }
}
