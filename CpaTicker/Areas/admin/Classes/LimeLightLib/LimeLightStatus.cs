using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes.LimeLightLib
{
    /// <summary>
    /// class to get reports from limelight using this class in limelightresponse actionresult
    /// </summary>
    public class LimeLightStatus
    {
        private NameValueCollection allfields;

        public NameValueCollection Allfields
        {
            get { return allfields; }
            set { allfields = value; }
        }

        public int ParentOrderId
        {
            get
            {
                return Convert.ToInt32(allfields["parent_order_id"]);
            }
            //set;
        }

        public int OrderId
        {
            get
            {
                return Convert.ToInt32(allfields["order_id"]);
            }
            //set;
        }

        //1 For Approvals, 0 For Declines
        public bool OrderStatus
        {
            get
            {
                return allfields["order_status"] == "1";
            }
            //set;
        }

        // temporal
        public string response {get;set;}


        public LimeLightStatus(Stream dataStream)
        {
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            this.response = responseFromServer;

            NameValueCollection query = HttpUtility.ParseQueryString(responseFromServer);

            this.allfields = query;
        }
    }
}