using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS_API_PROJECT.Models
{
    public class Coa_List
    {
        public int ACC_ID { get; set; }                
        public int COA_ID { get; set; }                
        public int ACC_TYPE { get; set; }                
        public int ACCOUNT_NO { get; set; }         
        public string ACCOUNT_NAME { get; set; }       
        public string ACCOUNT_DESCRIPTION { get; set; }
        public decimal BALANCE { get; set; }          
        public decimal AMOUNT { get; set; }           
        public string TYPE { get; set; }
    }
}