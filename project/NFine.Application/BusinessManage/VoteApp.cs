using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Domain.IRepository.BusinessManage;
using NFine.Domain.ViewModel;
using NFine.Repository.BusinessManage;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.BusinessManage
{
    public class VoteApp
    {
        private IVoteRepository service = new VoteRepository();
        //private string currentuserid = "";
        public List<VoteEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<VoteEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_IP.Contains(keyword));
                expression = expression.Or(t => t.F_WX_id.Contains(keyword));
            }
            if (!queryParam["timeType"].IsEmpty())
            {
                string timeType = queryParam["timeType"].ToString();
                switch (timeType)
                {
                    case "3":    //全部投票
                        break;
                    case "2":    //正常投票
                    case "1":    //后台投票
                        expression = expression.And(t => t.F_VoteType == timeType);
                        break;
                    default:
                        break;
                }
            }
            //if (!OperatorProvider.Provider.GetCurrent().IsSystem)
            //{
            //    currentuserid = OperatorProvider.Provider.GetCurrent().UserId;
            //    expression = expression.And(t => t.F_CreatorUserId == currentuserid);
            //}

            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Application.BusinessManage.VoteApp.GetList投票记录",
                F_Type = DbLogType.Visit.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "访问了投票记录页面",
            });
            return service.FindList(expression, pagination);
        }
        public List<VoteEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
        }

        public VoteEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            //if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            //{
            //    throw new Exception("删除失败！操作的对象包含了下级数据。");
            //}
            //else
            //{
            service.Delete(t => t.F_Id == keyValue);
            //}
        }
        public void SubmitForm(VoteEntity voteEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                voteEntity.Modify(keyValue);
                service.Update(voteEntity);
            }
            else
            {
                voteEntity.Create();
                service.Insert(voteEntity);
            }
        }

        public List<VoteEntity> GetList(string candidateId)
        {
            string sql = "SELECT * FROM Sys_Vote where F_CandidateID = '" + candidateId + "' and F_VoteType=2";
            return service.FindList(sql);
        }

        public List<StatisticVoteNumberModel> GetStatisticVoteNumber()
        {
            var statisticVoteNumberModel = new List<StatisticVoteNumberModel>();
            string sql = "SELECT * from Sys_Vote";
            List<VoteEntity> voteEntity = service.FindList(sql);
            var sums = from temp in voteEntity
                       orderby temp.F_CreatorTime descending
                       group temp by temp.F_CreatorTime.ToDateString() into g
                       select new { DateStr = g.Key, Count = g.Sum(temp => temp.F_VoteNumber) };

            foreach (var sum in sums)
                statisticVoteNumberModel.Add(new StatisticVoteNumberModel { DateStr = sum.DateStr.ToString(), CountVoteNumber = sum.Count });
            return statisticVoteNumberModel;
        }
    }
}
