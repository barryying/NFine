using NFine.Application.BusinessManage;
using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.MicroEvent.Controllers
{
    public class AddEventController : ControllerBase
    {
        private EventApp eventApp = new EventApp();
        //
        // GET: /MicroEvent/AddEvent/

        public ActionResult AddEvent()
        {
            return View();
        }
        public ActionResult GenLink()
        {
            return View();
        }

        //[HttpGet]
        //[HandlerAjaxOnly]
        //public ActionResult GetTreeSelectJson()
        //{
        //    var data = eventApp.GetList();
        //    var treeList = new List<TreeSelectModel>();
        //    foreach (EventEntity item in data)
        //    {
        //        TreeSelectModel treeModel = new TreeSelectModel();
        //        treeModel.id = item.F_Id;
        //        treeModel.text = item.F_Name;
        //        treeModel.parentId = item.F_ParentId;
        //        treeModel.data = item;
        //        treeList.Add(treeModel);
        //    }
        //    return Content(treeList.TreeSelectJson());
        //}
        //[HttpGet]
        //[HandlerAjaxOnly]
        //public ActionResult GetTreeJson()
        //{
        //    var data = eventApp.GetList();
        //    var treeList = new List<TreeViewModel>();
        //    foreach (EventEntity item in data)
        //    {
        //        TreeViewModel tree = new TreeViewModel();
        //        bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
        //        tree.id = item.F_Id;
        //        tree.text = item.F_Name;
        //        tree.value = item.F_EnCode;
        //        tree.parentId = item.F_ParentId;
        //        tree.isexpand = true;
        //        tree.complete = true;
        //        tree.hasChildren = hasChildren;
        //        treeList.Add(tree);
        //    }
        //    return Content(treeList.TreeViewJson());
        //}
        //[HttpGet]
        //[HandlerAjaxOnly]
        //public ActionResult GetTreeGridJson(string keyword)
        //{
        //    var data = eventApp.GetList();
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        data = data.TreeWhere(t => t.F_Name.Contains(keyword));
        //    }
        //    var treeList = new List<TreeGridModel>();
        //    foreach (EventEntity item in data)
        //    {
        //        TreeGridModel treeModel = new TreeGridModel();
        //        bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
        //        treeModel.id = item.F_Id;
        //        treeModel.isLeaf = hasChildren;
        //        treeModel.parentId = item.F_ParentId;
        //        treeModel.expanded = hasChildren;
        //        treeModel.entityJson = item.ToJson();
        //        treeList.Add(treeModel);
        //    }
        //    return Content(treeList.TreeGridJson());
        //}

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = eventApp.GetList(pagination, queryJson),
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
            var data = eventApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SubmitForm(EventEntity eventEntity, string keyValue, string isClone)
        {
            eventApp.SubmitForm(eventEntity, keyValue, isClone);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            eventApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Disabled(string keyValue)
        {
            EventEntity eventEntity = new EventEntity();
            eventEntity.F_Id = keyValue;
            eventEntity.F_Status = false;
            eventApp.UpdateForm(eventEntity);
            return Success("活动禁用成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Enabled(string keyValue)
        {
            EventEntity eventEntity = new EventEntity();
            eventEntity.F_Id = keyValue;
            eventEntity.F_Status = true;
            eventApp.UpdateForm(eventEntity);
            return Success("活动启用成功。");
        }

        [HttpGet]
        // /MicroEvent/AddEvent/GetEventPrize?kevalue=aa51beeb-e55c-4a85-b1bc-1395eaa65c28
        public ActionResult GetEventPrize(string kevalue)
        {            
            var data = eventApp.GetEventPrize(kevalue);
            return Content(data);
        }
    }
}
