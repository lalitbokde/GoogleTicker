using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Helper;

namespace CpaTicker.Controllers
{
    public class TickersController : ApiController
    {
        private ICpaTickerRepository repo;

        public TickersController()
        {
            this.repo = new EFCpatickerRepository();
        }

        /// <summary>
        /// campaign details
        /// </summary>
        /// <param name="apiKey">user’s api key (required)</param>
        /// <param name="fromdate">campaign details from datetime(not required)</param>
        /// <param name="todate">campaign details until datetime(not required)</param>
        /// <param name="offset">user timzone offset from UTC(not required)</param>
        /// <param name="affiliateid">user's affiliate id(not required)</param>
        /// <param name="subid"> click's sub value(not required)</param>
        /// <returns>
        /// list of campaign detail
        /// </returns>
        [HttpGet]
        //GET -api/tickers/api
        public IEnumerable<TickerItem> Api(string apiKey = "", DateTime? fromdate = null, DateTime? todate = null, int? offset = null, int? affiliateid = null, string subid = null)
        {

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Api key cann't be null or empty."));
            }

            Guid apiKeyAsGuid;
            if (!Guid.TryParse(apiKey, out apiKeyAsGuid))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Invalid api key, it is not a Guid."));
            }

            var user = repo.GetUserFromAPIKey(apiKey);
            if (user == null || !user.OrderId.HasValue)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "User not found using the api key."));
            }

            var list = new TickerHelper(repo).TickerItems(fromdate, todate, offset, affiliateid, subid, user);
            return list;
        }

    }
}
