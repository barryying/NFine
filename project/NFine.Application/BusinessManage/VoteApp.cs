using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
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
