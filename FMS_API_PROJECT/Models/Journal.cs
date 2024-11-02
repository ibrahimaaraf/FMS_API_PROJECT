using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMS_API_PROJECT.Models
{
    public class Journal
    {
        [Key]  //Indicates the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto-increment
        public int ID { get; set; }

        [Column("ACC_ID")]
        public int ACC_ID { get; set; }

        [Column("ACC_TYPE")]
        public int ACC_TYPE { get; set; }

        [Column("COA_ID")]
        public int COA_ID { get; set; }

        [Column("BALANCE")]
        public decimal BALANCE { get; set; }

        public virtual Account Account { get; set; }
        public virtual ChartOfAccount ChartOfAccount { get; set; }

        [Column("AMOUNT")]
        public string AMOUNT { get; set; }

        [Column("DOC_DATE")]
        public string DOC_DATE { get; set; }

        [Column("STATUS")]
        public int STATUS { get; set; }

        [Column("CREATED_AT")]
        public DateTime CREATED_AT { get; set; }

        [Column("CREATED_BY")]
        public int CREATED_BY { get; set; }

        [Column("UPDATED_AT")]
        public DateTime UPDATED_AT { get; set; }

        [Column("UPDATED_BY")]
        public int UPDATED_BY { get; set; }
    }
}