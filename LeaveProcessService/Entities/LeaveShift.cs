using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace LeaveProcessService.Entities
{
    [Table("OT_OTHER_LIST")]
    public class LeaveShift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string CODE { get; set; }
        public string NAME_VN { get; set; }
        public string NAME_EN { get; set; }
        public int TYPE_ID { get; set; }
    }
}
