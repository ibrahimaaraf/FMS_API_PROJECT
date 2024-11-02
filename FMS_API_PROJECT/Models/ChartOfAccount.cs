using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMS_API_PROJECT.Models
{
    [Table("CHART_ACC")]
    public class ChartOfAccount
    {
        [Key]  //Indicates the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto-increment
        public int COA_ID { get; set; }

        [Column("ACC_ID")]
        public int ACC_ID { get; set; }

        [Column("ACC_TYPE")]
        public int ACC_TYPE { get; set; }

        public virtual Account Account { get; set; }

        [Column("ACCOUNT_DESCRIPTION")]
        public string ACCOUNT_DESCRIPTION { get; set; }

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