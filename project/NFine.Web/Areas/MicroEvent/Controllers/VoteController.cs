using NFine.Application;
using NFine.Application.BusinessManage;
using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.Entity.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.MicroEvent.Controllers
{
    public class VoteController : ControllerBase
    {
        private VoteApp voteapp = new VoteApp();

        [HttpGet]
        public ActionResult GetAll(string candidateId)
        {
            var data = voteapp.GetList(candidateId);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = voteapp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [AllowAnonymous]
        // /MicroEvent/Vote/Vote?kevalue=f488b366-287d-40b2-bc64-c42254e634bb&ip=127.0.0.1&WXid=aaa&WXnick=bbb
        public ActionResult Vote(string keyvalue, string ip = null, string WXid = null, string WXnick = null)
        {
            string callbackFunc = Request.QueryString["callback"];
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            try
            {
                CandidateApp candidateApp = new CandidateApp();
                CandidateEntity candidateEntity = candidateApp.GetForm(keyvalue);
                candidateEntity.F_VoteNumber += 1;
                candidateApp.UpdateForm(candidateEntity);

                VoteEntity voteentity = new VoteEntity();
                VoteApp voteapp = new VoteApp();
                voteentity.F_Id = Common.GuId();
                voteentity.F_CandidateID = keyvalue;
                voteentity.F_VoteType = "2";     //微信投票
                voteentity.F_VoteNumber = 1;
                voteentity.F_IP = ip;
                voteentity.F_WX_id = WXid;
                voteentity.F_WX_Nick = WXnick;
                voteentity.F_CreatorTime = DateTime.Now;
                voteentity.F_CreatorUserId = WXnick;
                voteapp.SubmitForm(voteentity, null);
                
                return Content(callbackFunc + "(" + jss.Serialize(new
                {
                    data = Success("投票成功。")
                }) + ")");
                //new LogApp().WriteDbLog(new LogEntity
                //{
                //    F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.Vote投票成功",
                //    F_Type = DbLogType.Update.ToString(),
                //    F_Account = "前台请求接口",
                //    F_NickName = "前台",
                //    F_Result = true,
                //    F_Description = "前台给选手: " + candidateEntity.F_Id + " 投了1票。",
                //});
            }
            catch(Exception ex)
            {
                return Content(callbackFunc + "(" + jss.Serialize(new
                {
                    data = Error("投票失败：" + ex.Message)
                }) + ")");
            }
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetStatisticVoteNumber()
        {
            var data = voteapp.GetStatisticVoteNumber();
            return Content(data.ToJson());
        }
    }
}