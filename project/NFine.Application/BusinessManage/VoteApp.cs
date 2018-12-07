using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.IRepository.BusinessManage;
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
    }
}
