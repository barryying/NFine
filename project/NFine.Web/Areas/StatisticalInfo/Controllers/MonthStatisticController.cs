using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.StatisticalInfo.Controllers
{
    public class MonthStatisticController : ControllerBase
    {
        //
        // GET: /StatisticalInfo/MonthStatistic/

        [HttpGet]
        public ActionResult MonthList()
        {
            return View();
        }

    }
}
