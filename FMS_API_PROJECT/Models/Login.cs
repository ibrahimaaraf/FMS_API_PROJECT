using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMS_API_PROJECT.Models
{
    [Table("USER_INFO")]
    public class Login
    {
        [Key]  //Indicates the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto-increment
        public int ID { get; set; }

        [Column("USER_NAME")]
        public string USER_NAME { get; set; }

        [Column("PASSWORD")]
        public string PASSWORD { get; set; }

        [Column("FULL_NAME")]
        public string FULL_NAME { get; set; }

        [Column("USER_ID")]
        public string USER_ID { get; set; }

        [Column("EMAIL")]
        public string EMAIL { get; set; }

        [Column("PHONE")]
        public string PHONE { get; set; }

        [Column("STATUS")]
        public int? STATUS { get; set; } // Make STATUS nullable if required
    }
}