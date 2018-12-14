using System.Web.Mvc;
using NFine.Code;
using NFine.Domain.Entity;
using NFine.Application.BusinessManage;
using NFine.Domain.Entity.BusinessManage;
using System;

namespace NFine.Web.Areas.MicroEvent.Controllers
{
    public class CandidateController : ControllerBase
    {
        //
        // GET: /MicroEvent/Candidate/

        private CandidateApp candidateApp = new CandidateApp();

        public ActionResult GenLink()
        {
            return View();
        }
        public ActionResult DataRecords()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var data = candidateApp.GetList();
            return Content(data.ToJson());
        }
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
            candidateEntity.F_AuditIsOK = true;
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

        [HttpPost]
        // /MicroEvent/Candidate/Vote?kevalue=f488b366-287d-40b2-bc64-c42254e634bb
        public ActionResult Vote(string keyValue, int? votenumber)
        {
            CandidateEntity candidateEntity = candidateApp.GetForm(keyValue);
            if(votenumber.IsEmpty())
                candidateEntity.F_VoteNumber += 1;
            else
                candidateEntity.F_VoteNumber += votenumber;
            candidateApp.UpdateForm(candidateEntity);

            VoteEntity voteentity = new VoteEntity();
            VoteApp voteapp = new VoteApp();
            voteentity.F_Id = Common.GuId();
            voteentity.F_CandidateID = keyValue;
            voteentity.F_VoteType = "1";   //后台投票
            voteentity.F_VoteNumber = votenumber;
            voteentity.F_IP = Net.Ip;
            voteentity.F_CreatorTime = DateTime.Now;
            voteentity.F_CreatorUserId = OperatorProvider.Provider.GetCurrent().UserId;
            voteapp.SubmitForm(voteentity, null);

            return Success("投票成功");
        }

        [HttpPost]
        [AllowAnonymous]
        // /MicroEvent/Candidate/Vote?kevalue=f488b366-287d-40b2-bc64-c42254e634bb
        public ActionResult AddViewNumber(string keyValue, int? viewnumber)
        {
            CandidateEntity candidateEntity = candidateApp.GetForm(keyValue);
            if (viewnumber.IsEmpty())
                candidateEntity.F_ViewNumber += 1;
            else
                candidateEntity.F_ViewNumber += viewnumber;
            candidateApp.UpdateForm(candidateEntity);
            return Success("加浏览量成功");
        }

        [HttpGet]
        [AllowAnonymous]
        // /MicroEvent/Candidate/GetRankingList?keyvalue=aa51beeb-e55c-4a85-b1bc-1395eaa65c28
        public ActionResult GetRankingList(string keyvalue)
        {
            var data = candidateApp.GetRankingList(keyvalue);
            return Content(data.ToJson());
        }
    }
}
