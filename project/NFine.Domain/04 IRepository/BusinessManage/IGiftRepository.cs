using NFine.Data;
using NFine.Domain.Entity.BusinessManage;

namespace NFine.Domain.IRepository.BusinessManage
{
    public interface IGiftRepository : IRepositoryBase<GiftEntity>
    {
        void SubmitCloneButton(GiftEntity entity);
    }
}
