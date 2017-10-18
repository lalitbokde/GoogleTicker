using CpaTicker.Areas.admin.Classes.LimeLightLib;
using CpaTicker.Areas.admin.Classes.SecurityLib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CpaTicker.Areas.admin.Classes.Helpers
{
    public static class LimeLightHelper
    {
        /// <summary>
        /// Throws exception if errors were found
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="productid"></param>
        /// <param name="customer_email">Email of the customer</param>
        /// <returns></returns>
        public static LimeLightLib.LimeLightResponse NewLimeLightOrder(ICpaTickerRepository repo, Customer customer, string customer_email, int productid, string ipaddress, int? master_order_id = null)
        {
            // set the new order to limelight
            // make neworder call and gets the response
            LimeLightRequest request = new LimeLightRequest();
            NameValueCollection n = new NameValueCollection();

            // changes possible values
            n.Add("username", CpaTickerConfiguration.LimeLightUsername);
            n.Add("password", CpaTickerConfiguration.LimeLightPassword);
            n.Add("productId", productid.ToString()); // CpaTickerConfiguration.LimeLightProductId(customer.Level).ToString()
            n.Add("campaignId", CpaTickerConfiguration.LimeLightCampaignId.ToString());
            n.Add("shippingId", CpaTickerConfiguration.LimeLightShippingId.ToString());

            n.Add("method", "NewOrder");
            n.Add("firstName", customer.FirstName);
            n.Add("lastName", customer.LastName);
            n.Add("phone", customer.Phone);
            n.Add("email", customer_email);
            //n.Add("ipAddress", LimeLightRequest.LocalIPAddress());
            n.Add("ipAddress", ipaddress);

            if (master_order_id.HasValue)
            {
                n.Add("master_order_id", master_order_id.ToString());
            }

            //if (CpaTickerConfiguration.LimeLightTesting)
            //{
            //    n.Add("creditCardType", "visa"); // sc.GetCardType.ToLower()
            //    n.Add("creditCardNumber", "1444444444444440"); //  sc.CardNumber
            //}
            //else
            //{
            SecureCard sc = new SecureCard(customer.CreditCardData);
            n.Add("creditCardType", sc.GetCardType.ToLower());
            n.Add("creditCardNumber", sc.CardNumber);
            //}

            n.Add("expirationDate", sc.ExpiryMonth + sc.ExpiryYear); // expirationDate MMYY
            n.Add("CVV", sc.CVV);
            n.Add("shippingAddress1", customer.Address);
            n.Add("shippingCity", customer.City);
            n.Add("shippingState", customer.State);
            n.Add("shippingZip", customer.Zip);
            n.Add("shippingCountry", customer.Country.CountryAbbreviation);
            n.Add("billingSameAsShipping", "yes");
            n.Add("tranType", "Sale");
            request.PostData = n;

            string url = "https://www.transactsafely.com/admin/transact.php";

            var response = request.GetResponse(url);

            // log limelight response
            repo.AddLimeLightLog(new LimeLightLog 
            { 
                DateTime = DateTime.UtcNow,
                Request = n.ToString(),
                Response = response.allfields.ToString(),
            });

            if (response.errorFound() && !CpaTickerConfiguration.LimeLightTesting)
                throw new LimeLightException("Sorry, your credit card has been declined.");

            return response;
        }

        public static LimeLightLib.LimeLightResponse CancelLimeLightOrder(ICpaTickerRepository repo, int orderid)
        {
            LimeLightRequest request = new LimeLightRequest();
            NameValueCollection n = new NameValueCollection();
            n.Add("username", CpaTickerConfiguration.LimeLightUsername);
            n.Add("password", CpaTickerConfiguration.LimeLightPassword);
            n.Add("method", "order_update_recurring");

            n.Add("order_id", orderid.ToString());
            n.Add("status", "stop");

            request.PostData = n;
            var url = "https://www.transactsafely.com/admin/membership.php";
            var response = request.GetResponse(url);

            // log limelight response
            repo.AddLimeLightLog(new LimeLightLog
            {
                DateTime = DateTime.UtcNow,
                Request = n.ToString(),
                Response = response.allfields.ToString(),
            });

            return response;
        }

        public static LimeLightLib.LimeLightResponse UpdateLimeLightOrder(ICpaTickerRepository repo, int orderid, Customer customer, string customer_email, SecureCard customercard, int productid)
        {
            LimeLightRequest request = new LimeLightRequest();
            NameValueCollection n = new NameValueCollection();
            n.Add("username", CpaTickerConfiguration.LimeLightUsername);
            n.Add("password", CpaTickerConfiguration.LimeLightPassword);
            n.Add("method", "order_update");
            n.Add("order_ids", string.Format("{0},{0},{0},{0},{0},{0},{0},{0},{0},{0},{0},{0}", orderid));
            n.Add("actions", "first_name,last_name,email,phone,shipping_address1,shipping_city,shipping_zip,shipping_state,cc_number,cc_expiration_date,shipping_country,next_rebill_product");


            n.Add("values", string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                            customer.FirstName, customer.LastName,
                            customer_email, customer.Phone, HttpUtility.UrlPathEncode(string.Format("\"{0}\"", customer.Address)),
                            HttpUtility.UrlPathEncode(string.Format("\"{0}\"", customer.City)), customer.Zip, customer.State,
                            customercard.CardNumber, customercard.ExpiryMonth + customercard.ExpiryYear,
                            customer.Country.CountryAbbreviation, productid));


            request.PostData = n;
            var url = "https://www.transactsafely.com/admin/membership.php";
            var response = request.GetResponse(url);

            // log limelight response
            repo.AddLimeLightLog(new LimeLightLog
            {
                DateTime = DateTime.UtcNow,
                Request = n.ToString(),
                Response = response.allfields.ToString(),
            });

            string response_codes = response.response_code();
            string[] codes = response_codes.Split(',');
            foreach (var item in codes)
            {

                // 410-  API user: (api_username) has reached the limit of requests per minute: (limit) for method: (method_name)
                // 343 - Data Element Has Same Value As Value Passed No Update done (Information ONLY, but still a success)
                // 100 - Success
                // 350 – Invalid order Id supplied

                if (item.Equals("410"))
                {
                    throw new LimeLightException("410");
                }

                if (!(item.Equals("343") || item.Equals("100")))
                {
                    if (!CpaTickerConfiguration.LimeLightTesting)
                    {
                         var errorMessage = string.Format("Sorry, your billing options has not been updated. {0} {1}", LimeLightCodes.GetOrderUpdateResponseCode()[item], orderid);
                         throw new LimeLightException(errorMessage);
                    }
                   

                }
            }

            return response;
        }

        /*==================================================================================*/

        public static async Task<LimeLightLib.LimeLightResponse> NewLimeLightOrderAsync(Customer customer, string customer_email, int productid)
        {
            // set the new order to limelight
            // make neworder call and gets the response
            LimeLightRequest request = new LimeLightRequest();
            NameValueCollection n = new NameValueCollection();

            // changes possible values
            n.Add("username", CpaTickerConfiguration.LimeLightUsername);
            n.Add("password", CpaTickerConfiguration.LimeLightPassword);
            n.Add("productId", productid.ToString()); // CpaTickerConfiguration.LimeLightProductId(customer.Level).ToString()
            n.Add("campaignId", CpaTickerConfiguration.LimeLightCampaignId.ToString());
            n.Add("shippingId", CpaTickerConfiguration.LimeLightShippingId.ToString());

            n.Add("method", "NewOrder");
            n.Add("firstName", customer.FirstName);
            n.Add("lastName", customer.LastName);
            n.Add("phone", customer.Phone);
            n.Add("email", customer_email);
            n.Add("ipAddress", LimeLightRequest.LocalIPAddress());

            //if (CpaTickerConfiguration.LimeLightTesting)
            //{
            //    n.Add("creditCardType", "visa"); // sc.GetCardType.ToLower()
            //    n.Add("creditCardNumber", "1444444444444440"); //  sc.CardNumber
            //}
            //else
            //{
            SecureCard sc = new SecureCard(customer.CreditCardData);
            n.Add("creditCardType", sc.GetCardType.ToLower());
            n.Add("creditCardNumber", sc.CardNumber);
            //}

            n.Add("expirationDate", sc.ExpiryMonth + sc.ExpiryYear); // expirationDate MMYY
            n.Add("CVV", sc.CVV);
            n.Add("shippingAddress1", customer.Address);
            n.Add("shippingCity", customer.City);
            n.Add("shippingState", customer.State);
            n.Add("shippingZip", customer.Zip);
            n.Add("shippingCountry", customer.Country.CountryAbbreviation);
            n.Add("billingSameAsShipping", "yes");
            n.Add("tranType", "Sale");
            request.PostData = n;

            string url = "https://www.transactsafely.com/admin/transact.php";

            var response = await request.GetResponseAsync(url);

            //if (response.errorFound())
            //    throw new LimeLightException("Sorry, your credict card have been decline.");

            return response;
        }

        public static async Task<LimeLightLib.LimeLightResponse> CancelLimeLightOrderAsync(int orderid)
        {
            LimeLightRequest request = new LimeLightRequest();
            NameValueCollection n = new NameValueCollection();
            n.Add("username", CpaTickerConfiguration.LimeLightUsername);
            n.Add("password", CpaTickerConfiguration.LimeLightPassword);
            n.Add("method", "order_update_recurring");

            n.Add("order_id", orderid.ToString());
            n.Add("status", "stop");

            request.PostData = n;
            var url = "https://www.transactsafely.com/admin/membership.php";
            var response = await request.GetResponseAsync(url);

            return response;
        }

        public static async Task<LimeLightLib.LimeLightResponse> UpdateLimeLightOrderAsync(int orderid, Customer customer, string customer_email, SecureCard customercard, int productid)
        {
            LimeLightRequest request = new LimeLightRequest();
            NameValueCollection n = new NameValueCollection();
            n.Add("username", CpaTickerConfiguration.LimeLightUsername);
            n.Add("password", CpaTickerConfiguration.LimeLightPassword);
            n.Add("method", "order_update");
            n.Add("order_ids", string.Format("{0},{0},{0},{0},{0},{0},{0},{0},{0},{0},{0},{0}", orderid));
            n.Add("actions", "first_name,last_name,email,phone,shipping_address1,shipping_city,shipping_zip,shipping_state,cc_number,cc_expiration_date,shipping_country,next_rebill_product");


            n.Add("values", string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                            customer.FirstName, customer.LastName,
                            customer_email, customer.Phone, HttpUtility.UrlPathEncode(string.Format("\"{0}\"", customer.Address)),
                            HttpUtility.UrlPathEncode(string.Format("\"{0}\"", customer.City)), customer.Zip, customer.State,
                            customercard.CardNumber, customercard.ExpiryMonth + customercard.ExpiryYear,
                            customer.Country.CountryAbbreviation, productid));


            request.PostData = n;
            var url = "https://www.transactsafely.com/admin/membership.php";
            var response = await request.GetResponseAsync(url);

            //string response_codes = response.response_code();
            //string[] codes = response_codes.Split(',');
            //foreach (var item in codes)
            //{
            //    // 343 - Data Element Has Same Value As Value Passed No Update done (Information ONLY, but still a success)
            //    // 100 - Success
            //    // 350 – Invalid order Id supplied
            //    if (!(item.Equals("343") || item.Equals("100")))
            //    {
            //        var errorMessage = string.Format("Sorry, your billing options has not been updated. {0} {1}", LimeLightCodes.GetOrderUpdateResponseCode()[item], orderid);
            //        throw new LimeLightException(errorMessage);

            //    }
            //}

            return response;
        }

        
    }
}