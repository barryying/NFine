using NFine.Data;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.IRepository.BusinessManage;

namespace NFine.Repository.BusinessManage
{
    public class GiftRepository : RepositoryBase<GiftEntity>, IGiftRepository
    {
        public void SubmitCloneButton(GiftEntity entity)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Insert(entity);
                db.Commit();
            }
        }
    }
}
