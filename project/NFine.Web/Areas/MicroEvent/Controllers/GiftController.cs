using NFine.Application;
using NFine.Application.BusinessManage;
using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.Entity.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.MicroEvent.Controllers
{
    public class GiftController : ControllerBase
    {
        //
        // GET: /MicroEvent/Gift/

        private GiftApp giftApp = new GiftApp();
        
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson(string keyValue)
        {
            var data = giftApp.GetTreeSelectJson(keyValue);
            //var treeList = new List<TreeSelectModel>();
            //foreach (GiftEntity item in data)
            //{
            //    TreeSelectModel treeModel = new TreeSelectModel();
            //    treeModel.id = item.F_Id;
            //    treeModel.text = item.F_Name;
            //    treeModel.parentId = item.F_ParentId;
            //    treeModel.data = item;
            //    treeList.Add(treeModel);
            //}
            //return Content(treeList.TreeSelectJson());
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = giftApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetList(string keyValue)
        {
            var data = giftApp.GetList(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = giftApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SubmitForm(GiftEntity giftEntity, string keyValue, string isClone, string eventId)
        {
            giftApp.SubmitForm(giftEntity, keyValue, isClone, eventId);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            giftApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Disabled(string keyValue)
        {
            GiftEntity giftEntity = new GiftEntity();
            giftEntity.F_Id = keyValue;
            giftEntity.F_EnabledMark = false;
            giftApp.UpdateForm(giftEntity);
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.Disabled禁用礼物",
                F_Type = DbLogType.Update.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "修改了礼物: " + giftEntity.F_Id + "  的 F_EnabledMark: 由‘true’改为了‘false’。",
            });
            return Success("礼物禁用成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Enabled(string keyValue)
        {
            GiftEntity giftEntity = new GiftEntity();
            giftEntity.F_Id = keyValue;
            giftEntity.F_EnabledMark = true;
            giftApp.UpdateForm(giftEntity);
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.Enabled启用礼物",
                F_Type = DbLogType.Update.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "修改了礼物: " + giftEntity.F_Id + "  的 F_EnabledMark: 由‘false’改为了‘true’。",
            });
            return Success("礼物启用成功。");
        }
    }
}
