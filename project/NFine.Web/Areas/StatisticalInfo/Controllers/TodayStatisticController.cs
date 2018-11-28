using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.StatisticalInfo.Controllers
{
    public class TodayStatisticController : ControllerBase
    {
        //
        // GET: /StatisticalInfo/TodayStatistic/

        [HttpGet]
        public ActionResult TodayList()
        {
            return View();
        }

    }
}
