using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMS_API_PROJECT.Models
{
    public class Account
    {
        [Key]  //Indicates the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto-increment
        public int ACC_ID { get; set; }

        [Column("ACCOUNT_ID")]
        public int ACCOUNT_NO { get; set; }

        [Column("ACCOUNT_NAME")]
        public string ACCOUNT_NAME { get; set; }
        [Column("ACC_TYPE")]
        public string ACC_TYPE { get; set; }

        [Column("BALANCE")]
        public decimal BALANCE { get; set; }

        [Column("OPNENING_BALANCE")]
        public decimal OPNENING_BALANCE { get; set; }

        [Column("STATUS")]
        public int STATUS { get; set; }

        [Column("CREATED_AT")]
        public DateTime CREATED_AT { get; set; }

        [Column("CREATED_BY")]
        public int CREATED_BY { get; set; }

        public virtual ICollection<ChartOfAccount> ChartOfAccounts { get; set; }
    }
}