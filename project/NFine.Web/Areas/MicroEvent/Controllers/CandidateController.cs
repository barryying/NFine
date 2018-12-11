using System.Web.Mvc;
using NFine.Code;
using NFine.Domain.Entity;
using NFine.Application.BusinessManage;

namespace NFine.Web.Areas.MicroEvent.Controllers
{
    public class CandidateController : ControllerBase
    {
        //
        // GET: /MicroEvent/Candidate/

        private CandidateApp candidateApp = new CandidateApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = candidateApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = candidateApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SubmitForm(CandidateEntity candidateEntity, string keyValue, string eventID)
        {
            candidateApp.SubmitForm(candidateEntity, keyValue, eventID);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            candidateApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Disabled(string keyValue)
        {
            CandidateEntity candidateEntity = new CandidateEntity();
            candidateEntity.F_Id = keyValue;
            candidateEntity.F_AuditIsOK = false;
            candidateApp.UpdateForm(candidateEntity);
            return Success("选手审核不通过。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Enabled(string keyValue)
        {
            CandidateEntity candidateEntity = new CandidateEntity();
            candidateEntity.F_Id = keyValue;
            candidateEntity.IsTodayStar = true;
            candidateApp.UpdateForm(candidateEntity);
            return Success("选手审核通过。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SetStar(string keyValue)
        {
            CandidateEntity candidateEntity = new CandidateEntity();
            candidateEntity.F_Id = keyValue;
            candidateEntity.IsTodayStar = true;
            candidateApp.UpdateForm(candidateEntity);
            return Success("已设为今日之星。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult CancelStar(string keyValue)
        {
            CandidateEntity candidateEntity = new CandidateEntity();
            candidateEntity.F_Id = keyValue;
            candidateEntity.IsTodayStar = false;
            candidateApp.UpdateForm(candidateEntity);
            return Success("已去除今日之星。");
        }
    }
}
