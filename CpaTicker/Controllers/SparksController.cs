using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Models;
using System.Text;

namespace CpaTicker.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SparksController : ApiController
    {
        public readonly ICpaTickerRepository repo = null;

        public SparksController()
        {
            this.repo = new EFCpatickerRepository();
        }

        //GET -api/sparks/get
        public IEnumerable<Spark> Get()
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);
            var tzi = repo.FindTimeZoneInfo(null, customer.TimeZone);

            var count = 12;

            var fdate = new DateTime(DateTime.Today.AddDays(1 - count).Ticks, DateTimeKind.Unspecified);
            var tdate = new DateTime(DateTime.Today.AddDays(1).AddSeconds(-1).Ticks, DateTimeKind.Unspecified);

            var ufdate = new DateTimeOffset(fdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(tdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            //var report = repo.DailyReport(fdate, tdate, up, tzi);

            //var report = repo.DailyReport(fdate, tdate, up.CustomerId, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId);
            //var pagename = "onlySparks";
            //var loaddependancy = repo.GetNotifyHourlyRptData(up.CustomerId, pagename);
            var report = repo.RunQuery<DailyView>("EXEC [Spark] {0}, {1}, {2}, {3}, {4}, {5}",
                up.CustomerId, ufdate, utdate, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, up.AffiliateId);

            var result = new List<Spark>();
            foreach (var item in report)
            {               
                result.Add(new Spark
                {                    
                    Clicks = item.Clicks,
                    Conversions = item.Conversions,
                    Revenue = item.Revenue,
                });
            }

            return result;

        }
    }
}
