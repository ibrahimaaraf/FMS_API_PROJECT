using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS_API_PROJECT.Models
{
    public class FMS_Report
    {
        public int ACCOUNT_NO { get; set; }
        public string ACCOUNT_NAME { get; set; }
        public decimal OPNENING_BALANCE { get; set; }
        public string ACCOUNT_DESCRIPTION { get; set; }
        public string DOC_DATE { get; set; }
        public string TYPE { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal BALANCE { get; set; }
        public string FULL_NAME { get; set; }
    }
}