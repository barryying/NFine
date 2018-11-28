using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.StatisticalInfo.Controllers
{
    public class WeekStatisticController : ControllerBase
    {
        //
        // GET: /StatisticalInfo/WeekStatistic/

        [HttpGet]
        public ActionResult WeekList()
        {
            return View();
        }

    }
}
