using NFine.Application.BusinessManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.StatisticalInfo.Controllers
{
    public class StatisticController : ControllerBase
    {
        private GiftListApp giftListApp = new GiftListApp();
        //
        // GET: /StatisticalInfo/Statistic/

        [HttpGet]
        public ActionResult TodayList()
        {
            var data = giftListApp.GetDatatistic("today");
            return View(data);
        }

        [HttpGet]
        public ActionResult WeekList()
        {
            var data = giftListApp.GetDatatistic("week");
            return View(data);
        }

        [HttpGet]
        public ActionResult MonthList()
        {
            var data = giftListApp.GetDatatistic("month");
            return View(data);
        }
    }
}
