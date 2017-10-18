using System;
using System.Collections.Generic;
using System.Linq;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;

namespace CpaTicker.Helper
{
    public class TickerHelper
    {
        private readonly ICpaTickerRepository Repo;

        public TickerHelper(ICpaTickerRepository repo)
        {
            Repo = repo;
        }

        public List<TickerItem> TickerItems(DateTime? fromdate, DateTime? todate, int? offset, int? affiliateid, string subid, UserProfile user)
        {
            // if the user is a customer then filter data by that affiliate
            if (user.AffiliateId.HasValue)
            {
                affiliateid = user.AffiliateId;
            }

            fromdate = fromdate ?? DateTime.Today;
            todate = todate ?? DateTime.Today.AddDays(1);

            var fdate = new DateTime(fromdate.Value.Ticks, DateTimeKind.Unspecified);
            var tdate = new DateTime(todate.Value.Ticks, DateTimeKind.Unspecified);
            TimeSpan toffset;
            if (offset.HasValue)
            {
                toffset = new TimeSpan(offset.Value, 0, 0);
            }
            else
            {
                var customer = Repo.GetCurrentCustomer(user.CustomerId);
                var tzi = TimeZoneInfo.FindSystemTimeZoneById(customer.TimeZone);
                // toffset = tzi.GetUtcOffset(fdate);
                toffset = tzi.GetUtcOffset(DateTime.Now);
            }

            //var ufdate = new DateTimeOffset(fdate, toffset).UtcDateTime;
            //var utdate = new DateTimeOffset(tdate, toffset).UtcDateTime;
            DateTime ufdate = new DateTimeOffset(fromdate.Value.Ticks, toffset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(todate.Value.Ticks, toffset).UtcDateTime;

            var list = Repo.BuildTicker(ufdate, utdate, user.CustomerId, user.UserId, affiliateid, subid).ToList();
            return list;
        }


    }
}