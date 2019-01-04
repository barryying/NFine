using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.IRepository.BusinessManage;
using NFine.Repository.BusinessManage;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.BusinessManage
{
    public class GiftListInsertApp
    {
        private IGiftListInsertRepository service = new GiftListInsertRepository();

        public List<GiftListInsertEntity> GetOrderListInsert()
        {
            return service.IQueryable().ToList();
        }
        public bool DeleteTable()
        {
            string sql = "delete from Sys_GiftList_insert_temp;select * from Sys_GiftList_insert_temp";
            return service.FindList(sql).Count > 0 ? false : true;
        }
    }
}
