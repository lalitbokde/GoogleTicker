using System.Web.Http.Description;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CpaTicker.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LogsController : ApiController
    {

        public readonly ICpaTickerRepository repo = null;

        public LogsController()
        {
            this.repo = new EFCpatickerRepository();
        }
        
        // GET api/logs/get
        public IEnumerable<Log> Get(int? page = null)
        {
            int _page = page ?? 1;
            int itemsPerPage = 20;
            var skip = (_page - 1) * itemsPerPage;

            return repo.Logs()
                .OrderByDescending(l => l.Date)
                .Skip(skip)
                .Take(itemsPerPage)
                ;
                //.Select(c => new LogModel()
                //{
                //    Date = c.Date,
                //    Level = c.Level,
                //    LineNumber = 
        }

        
    }
}
