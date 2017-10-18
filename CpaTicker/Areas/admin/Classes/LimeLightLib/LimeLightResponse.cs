using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpaTicker.Areas.admin.Classes.LimeLightLib
{
    public class LimeLightResponse
    {
        /// <summary>
        /// Get if there has been errors from newordercall
        /// </summary>
        /// <returns></returns>
        public bool errorFound()
        { 
            return allfields["errorFound"].Equals("1");
        }
        /// <summary>
        /// Get the errormessage from neworder if the call fails
        /// </summary>
        /// <returns></returns>
        public string errorMessage()
        {
            return allfields["errorMessage"];
        }
        public string responseCode()
        {
            return allfields["responseCode"];
        }


        /// <summary>
        /// Get the response codes from order_update
        /// </summary>
        /// <returns></returns>
        public string response_code()
        {
            return allfields["response_code"];
        }

            
        public bool ValidateResponseCodes()
        {
            return true;
        }

        
        public int OrderId()
        {
            return Convert.ToInt32(allfields["orderId"]);
        }

        public System.Collections.Specialized.NameValueCollection allfields { get; set; }
    }
}
