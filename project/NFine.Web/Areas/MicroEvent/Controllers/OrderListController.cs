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

    }
}
