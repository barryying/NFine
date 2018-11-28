using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.MicroEvent.Controllers
{
    public class OrderListController : ControllerBase
    {
        //
        // GET: /MicroEvent/OrderList/

        [HttpGet]
        public ActionResult OrderList()
        {
            return View();
        }

    }
}
