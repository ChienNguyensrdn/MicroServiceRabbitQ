using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace LeaveProcessService.Entities
{
    [Table("SNP_CR.AT_TIME_MANUAL")]
    public class LeaveSymbol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string CODE { get; set; }
        public string NAME{ get; set; }
        public int  MORNING_ID { get; set; }
        public int  AFTERNOON_ID { get; set; }
    }
}
