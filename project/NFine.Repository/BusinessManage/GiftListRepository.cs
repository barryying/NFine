using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.IRepository.BusinessManage;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace NFine.Repository.BusinessManage
{
    public class GiftListRepository : RepositoryBase<GiftListEntity>, IGiftListRepository
    {
        public List<GiftListEntity> GetGiftList()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from Sys_GiftList where F_CandidateID in(
                            select F_ID from Sys_Candidate where F_EventID in( 
                            select F_ID from Sys_Event where F_CreatorUserId=@id
                            ))");
            DbParameter[] parameter =
            {
                 new SqlParameter("@id",OperatorProvider.Provider.GetCurrent().UserId)
            };
            return this.FindList(strSql.ToString(), parameter);
        }
    }
}
