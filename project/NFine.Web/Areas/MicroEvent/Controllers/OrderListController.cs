using NFine.Application.BusinessManage;
using NFine.Code;
using System.Web.Mvc;

namespace NFine.Web.Areas.MicroEvent.Controllers
{
    public class OrderListController : ControllerBase
    {
        //
        // GET: /MicroEvent/OrderList/

        [HttpGet]
        public ActionResult OrderList()
        {
            return View();
        }

        private GiftListApp giftlistApp = new GiftListApp();
        private GiftListInsertApp giftlistinsertApp = new GiftListInsertApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = giftlistApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTodayListCount()
        {
            var data = giftlistApp.GetTodayList();
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOrderListInsert()
        {
            var data = giftlistinsertApp.GetOrderListInsert();
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult DeleteTable()
        {
            var data = giftlistinsertApp.DeleteTable();
            return Content(data.ToJson());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SubmitForm(string keyvalue, string money, string giftid, string paymentstatus, string ip, string openid)
        {
            string callbackFunc = Request.QueryString["callback"];
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();

            giftlistApp.SubmitForm(keyvalue, money, giftid, paymentstatus, ip, openid);
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return Content(callbackFunc + "(" + jss.Serialize(new
            {
                data = Success("操作成功。")
            }) + ")");
        }
    }
}
