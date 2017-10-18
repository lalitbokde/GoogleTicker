using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CpaTicker.Hubs
{
    public class TrickerHub : Hub
    {
        public static void Show(string pagename,string ConnectionId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TrickerHub>();
            if (pagename == "onlySparks")
            {
                context.Clients.Client(ConnectionId).displaySparks();
            }
            else if (pagename == "Hourly")
            {
                context.Clients.Client(ConnectionId).displayhourlyreport();
            }
            else if (pagename == "Affiliate")
            {
                context.Clients.Client(ConnectionId).displayaffiliatereport();
            }
            else if (pagename == "campaign")
            {
                context.Clients.Client(ConnectionId).displayCampaign();
            }
            else if(pagename == "ticker")
            {
                context.Clients.Client(ConnectionId).displayTicker();
            }
            else if (pagename == "ConversionStatus")
            {
                context.Clients.Client(ConnectionId).displayconversionstatusreport();
            }
            else if (pagename == "CTRReport")
            {
                context.Clients.Client(ConnectionId).displayCTRreport();
            }
            
                
           
            
        }
    }
}