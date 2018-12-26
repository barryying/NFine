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
        [AllowAnonymous]
        public ActionResult GetAllByEventID(string keyValue)
        {
            var data = viewallcandidateapp.GetAllByEventID(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAllByCandidateID(string keyValue)
        {
            var data = viewallcandidateapp.GetAllByCandidateID(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetMain(string keyValue)
        {
            var data = viewallcandidateapp.GetMain(keyValue);
            return Content(data.ToJson());
        }
    }
}