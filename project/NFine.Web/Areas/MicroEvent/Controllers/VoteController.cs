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


        [HttpPost]
        [AllowAnonymous]
        // /MicroEvent/Vote/Vote?kevalue=f488b366-287d-40b2-bc64-c42254e634bb&ip=127.0.0.1&WXid=aaa&WXnick=bbb
        public ActionResult Vote(string keyValue, string ip = null, string WXid = null, string WXnick = null)
        {
            CandidateApp candidateApp = new CandidateApp();
            CandidateEntity candidateEntity = candidateApp.GetForm(keyValue);
            candidateEntity.F_VoteNumber += 1;
            candidateApp.UpdateForm(candidateEntity);

            VoteEntity voteentity = new VoteEntity();
            VoteApp voteapp = new VoteApp();
            voteentity.F_Id = Common.GuId();
            voteentity.F_CandidateID = keyValue;
            voteentity.F_VoteType = "2";     //微信投票
            voteentity.F_VoteNumber = 1;
            voteentity.F_IP = ip;
            voteentity.F_WX_id = WXid;
            voteentity.F_WX_Nick = WXnick;
            voteentity.F_CreatorTime = DateTime.Now;
            voteentity.F_CreatorUserId = OperatorProvider.Provider.GetCurrent().UserId;
            voteapp.SubmitForm(voteentity, null);

            //new LogApp().WriteDbLog(new LogEntity
            //{
            //    F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.Vote投票成功",
            //    F_Type = DbLogType.Update.ToString(),
            //    F_Account = OperatorProvider.Provider.GetCurrent().UserId,
            //    F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
            //    F_Result = true,
            //    F_Description = "前台给选手: " + candidateEntity.F_Id + " 投了1票。",
            //});
            return Success("投票成功");
        }

    }
}