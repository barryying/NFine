using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System.Web.Mvc;

namespace NFine.Web.Areas.MicroEvent.Controllers
{
    public class DiamondVoteController : ControllerBase
    {
        //
        // GET: /MicroEvent/DiamondVote/

        [HttpGet]
        public ActionResult EventList()
        {
            return View();
        }
    }
}
