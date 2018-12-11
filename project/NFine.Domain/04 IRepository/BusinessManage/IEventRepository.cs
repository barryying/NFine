using NFine.Data;
using NFine.Domain.Entity.BusinessManage;

namespace NFine.Domain.IRepository.BusinessManage
{
    public interface IEventRepository : IRepositoryBase<EventEntity>
    {
        void SubmitCloneButton(EventEntity entity);
    }
}
