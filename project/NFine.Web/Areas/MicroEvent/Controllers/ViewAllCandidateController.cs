using NFine.Application.BusinessManage;
using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.MicroEvent.Controllers
{
    public class ViewAllCandidateController : ControllerBase
    {
        private ViewAllCandidateApp viewallcandidateapp = new ViewAllCandidateApp();

        [HttpGet]
        public ActionResult GetAll(string eventId)
        {
            var data = viewallcandidateapp.GetList(eventId);
            return Content(data.ToJson());
        }
    }
}