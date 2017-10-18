using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CpaTicker.Areas.admin.Classes.LimeLightLib
{
    class LimeLightRequest
    {
        public NameValueCollection PostData { get; set; }

        private string GetPostData()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < PostData.Count; i++)
            {
                sb.AppendFormat("{0}={1}&", PostData.Keys[i], PostData[i]);
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public LimeLightResponse GetResponse(string url)
        {
            // Create a request using a URL that can receive a post. 
            var request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            string postData = GetPostData();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
           

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            WebResponse response = request.GetResponse();
            
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            NameValueCollection query = HttpUtility.ParseQueryString(responseFromServer);

            var result = new LimeLightResponse() { allfields = query  };
            
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return result;

        }

        public async Task<LimeLightResponse> GetResponseAsync(string url)
        {
            // Create a request using a URL that can receive a post. 
            var request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            string postData = GetPostData();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = await request.GetRequestStreamAsync();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            WebResponse response = await request.GetResponseAsync();

            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = await reader.ReadToEndAsync();

            NameValueCollection query = HttpUtility.ParseQueryString(responseFromServer);

            var result = new LimeLightResponse() { allfields = query };

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return result;

        }

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
    }
}
