using System.Web.Mvc;
using NFine.Code;
using NFine.Domain.Entity;
using NFine.Application.BusinessManage;
using NFine.Domain.Entity.BusinessManage;
using System;
using NFine.Application.SystemSecurity;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Application;

namespace NFine.Web.Areas.MicroEvent.Controllers
{
    public class CandidateController : ControllerBase
    {
        //
        // GET: /MicroEvent/Candidate/

        private CandidateApp candidateApp = new CandidateApp();

        public ActionResult GenLink()
        {
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.GenLink查看选手链接",
                F_Type = DbLogType.Visit.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "点击了查看选手链接",
            });
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
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.Disabled选手审核不通过",
                F_Type = DbLogType.Update.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "修改了选手: " + candidateEntity.F_Id + "  的 F_AuditIsOK: 由‘true’改为了‘false’。",
            });
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
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.Enabled选手审核通过",
                F_Type = DbLogType.Update.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "修改了选手: " + candidateEntity.F_Id + "  的 F_AuditIsOK: 由‘false’改为了‘true’。",
            });
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
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.SetStar设为今日之星",
                F_Type = DbLogType.Update.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "修改了选手: " + candidateEntity.F_Id + "  的 IsTodayStar: 由‘false’改为了‘true’。",
            });
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
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.CancelStar去除今日之星",
                F_Type = DbLogType.Update.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "修改了选手: " + candidateEntity.F_Id + "  的 IsTodayStar: 由‘true’改为了‘false’。",
            });
            return Success("已去除今日之星。");
        }

        [HttpPost]
        //[AllowAnonymous]
        // /MicroEvent/Candidate/Vote?kevalue=f488b366-287d-40b2-bc64-c42254e634bb
        public ActionResult Vote(string keyValue, int? votenumber)
        {
            string discription = "";
            //string isNormalVote = "1";
            CandidateEntity candidateEntity = candidateApp.GetForm(keyValue);
            //if(votenumber.IsEmpty())
            //{
            //    candidateEntity.F_VoteNumber += 1;
            //    isNormalVote = "2";
            //    discription = "前台修改了选手: " + candidateEntity.F_Id + "  的 F_VoteNumber: 增加了" + votenumber + "票。";
            //}
            //else
            //{
            //    candidateEntity.F_VoteNumber += votenumber;
            //    discription = "后台修改了选手: " + candidateEntity.F_Id + "  的 F_VoteNumber: 增加了" + votenumber + "票。";
            //}
            candidateApp.UpdateForm(candidateEntity);

            VoteEntity voteentity = new VoteEntity();
            VoteApp voteapp = new VoteApp();
            voteentity.F_Id = Common.GuId();
            voteentity.F_CandidateID = keyValue;
            voteentity.F_VoteType = "1";   //1后台投票 2前台投票
            voteentity.F_VoteNumber = votenumber;
            voteentity.F_IP = Net.Ip;
            voteentity.F_CreatorTime = DateTime.Now;
            voteentity.F_CreatorUserId = OperatorProvider.Provider.GetCurrent().UserId;
            voteapp.SubmitForm(voteentity, null);

            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.Vote投票成功",
                F_Type = DbLogType.Update.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = discription,
            });
            return Success("投票成功");
        }

        [HttpPost]
        [AllowAnonymous]
        // /MicroEvent/Candidate/Vote?kevalue=f488b366-287d-40b2-bc64-c42254e634bb
        public ActionResult AddViewNumber(string keyValue, int? viewnumber)
        {
            string discription = "";
            CandidateEntity candidateEntity = candidateApp.GetForm(keyValue);
            if (viewnumber.IsEmpty())
            {
                candidateEntity.F_ViewNumber += 1;
                discription = "前台修改了选手: " + candidateEntity.F_Id + "  的 F_ViewNumber: 增加了" + viewnumber + "浏览量。";
            }
            else
            {
                candidateEntity.F_ViewNumber += viewnumber;
                discription = "后台修改了选手: " + candidateEntity.F_Id + "  的 F_ViewNumber: 增加了" + viewnumber + "浏览量。";
            }
            candidateApp.UpdateForm(candidateEntity);
            //new LogApp().WriteDbLog(new LogEntity
            //{
            //    F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.AddViewNumber加浏览量成功",
            //    F_Type = DbLogType.Update.ToString(),
            //    F_Account = OperatorProvider.Provider.GetCurrent().UserId,
            //    F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
            //    F_Result = true,
            //    F_Description = discription,
            //});
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return Success("加浏览量成功");
        }

        [HttpGet]
        [AllowAnonymous]
        // /MicroEvent/Candidate/GetRankingList?keyvalue=aa51beeb-e55c-4a85-b1bc-1395eaa65c28
        public ActionResult GetRankingList(string keyvalue)
        {
            string callbackFunc = Request.QueryString["callback"];
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();

            var data = candidateApp.GetRankingList(keyvalue);
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return Content(callbackFunc + "(" + jss.Serialize(new
            {
                data = data
            }) + ")");
        }
    }
}
