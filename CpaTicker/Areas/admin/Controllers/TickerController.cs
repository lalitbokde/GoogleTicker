using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.admin.Controllers
{
    public class TickerController /*: Controller*/ : BaseController
    {
        private ICpaTickerRepository repo;

        public TickerController()
        {
            this.repo = new EFCpatickerRepository();
        }

        public ActionResult Index()
        {
            int userid = repo.GetCurrentUserId();
            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);
            return View(repo.GetTickers());
        }

        public ActionResult Create()
        {
            ViewBag.TickerView = 7;
            ViewBag.Campaigns = repo.GetUserCampaigns().Where(c => c.Status == Status.Active);
            return View(CPAHelper.GetDefaultTicker());
        }

        [HttpPost]
        public ActionResult Create(int[] cp, string speed, TickerView[] views, string all, bool dir, Ticker tk)
        {
            TickerView view = 0;
            if (views != null)
            {
                foreach (var item in views)
                {
                    view = view | item;
                }
            }

            //Ticker t = new Ticker 
            //{ 
            //    All = all != null, 
            //    View = view, 
            //    Speed = int.Parse(speed), 
            //    UserId = repo.GetCurrentUserId(), 
            //    Direction = dir 
            //};

            tk.All = all != null;
            tk.View = view;
            tk.Speed = int.Parse(speed);
            tk.UserId = repo.GetCurrentUserId();
            tk.Direction = dir;

            using (var scope = new TransactionScope())
            {
                repo.AddTicker(tk);

                if (cp != null)
                {
                    foreach (var item in cp)
                    {
                        //repo.AddTickerCampaign(tk.TickerId, int.Parse(item));

                        repo.AddTickerElement(new TickerElement()
                        {
                            CampaignId = item,
                            TickerId = tk.TickerId,
                        });

                    }
                }

                scope.Complete();
            }



            return RedirectToAction("index");
        }

        //
        // GET: /admin/Ticker/Edit/5

        public ActionResult Edit(int id)
        {
            var ticker = repo.FindTicker(id);
            if (ticker == null)
            {
                return HttpNotFound();
            }

            ViewBag.Ticker = ticker;
            return View(repo.GetEditTickerVieModel(id, ticker.All));
        }

        //
        // POST: /admin/Ticker/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, /*string[] cp,*/ string speed, TickerView[] views, /*string all,*/ bool dir,
            string BackgroundColor, string CampaignColor, string ImpressionColor, string ClickColor, string ConversionColor)
        {
            //bool allchecked = all != null;
            //if (!allchecked)
            //{
            //    repo.DeleteTickerCampaigns(id);

            //    if (cp != null)
            //    {
            //        foreach (var item in cp)
            //            repo.AddTickerCampaign(id, int.Parse(item));
            //    }
            //}

            var ticker = repo.FindTicker(id);
            TickerView view = 0;
            if (views != null)
            {
                foreach (var item in views)
                {
                    view = view | item;
                }
            }
            ticker.Speed = int.Parse(speed);
            ticker.View = view;
            //ticker.All = allchecked;
            ticker.Direction = dir;

            ticker.BackgroundColor = BackgroundColor;
            ticker.CampaignColor = CampaignColor;
            ticker.ImpressionColor = ImpressionColor;
            ticker.ClickColor = ClickColor;
            ticker.ConversionColor = ConversionColor;

            repo.UpdateTicker(ticker);

            return RedirectToAction("edit", ticker.TickerId);
            //ViewBag.Message = "The ticker have been updated! Go back to ";
            //ViewBag.CurrentSpeed = ticker.Speed * 100;
            //ViewBag.TickerView = ticker.View;
            //return View(repo.GetEditTickerVieModel(id));
        }

        //
        // GET: /admin/Ticker/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    repo.DeleteTicker(id);
        //    return RedirectToAction("Index");
        //}

        //public ActionResult Settings()
        //{
        //    var model = repo.GetTickerSettings() ?? CPAHelper.GetDefaultTickerSettings();
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Settings(TickerSetting ts)
        //{
        //    if (ts.UserId != 0)
        //    {
        //        // update
        //        repo.UpdateTickerSettings(ts);
        //    }
        //    else
        //    {
        //        try // this is because sometimes it give duplicate key
        //        {
        //            // create
        //            repo.AddTickerSetting(ts);
        //        }
        //        catch { }
        //    }
        //    return View();
        //}

        ////
        //// POST: /admin/Ticker/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        public ActionResult Elements(int id)
        {
            var ticker = repo.FindTicker(id);
            if (ticker == null)
            {
                return HttpNotFound();
            }

            // fill viewbag
            //var customerid = repo.GetCurrentCustomerId();
            var user = repo.GetCurrentUser();
            ViewBag.IsAdmin = !user.AffiliateId.HasValue;

            ViewBag.Campaigns = repo.GetUserCampaigns(user).Where(c => c.Status == Status.Active).AsEnumerable().Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
            });

            if (!user.AffiliateId.HasValue)
            {
                ViewBag.Affiliates = repo.GetCustomerActiveAffiliates(user.CustomerId).AsEnumerable().Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = string.Format("{0} - {1}", a.AffiliateId, a.Company)
                }).OrderBy(u => u.Value);
            }


            return View(ticker);
        }

        [HttpPost]
        public ActionResult Elements(int id, int? campaignid, int? affiliateid)
        {
            // add the new ticker element
            if (campaignid.HasValue || affiliateid.HasValue)
            {
                repo.AddTickerElement(new TickerElement()
                {
                    AffiliateId = affiliateid,
                    CampaignId = campaignid,
                    TickerId = id,
                });
            }


            return RedirectToAction("elements", new { id = id });
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}
