using NFine.Data;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.IRepository.BusinessManage;

namespace NFine.Repository.BusinessManage
{
    public class EventRepository : RepositoryBase<EventEntity>, IEventRepository
    {
        public void SubmitCloneButton(EventEntity entity)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Insert(entity);
                db.Commit();
            }
        }
    }
}
