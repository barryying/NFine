using NFine.Data;
using NFine.Domain.Entity.BusinessManage;
using System.Collections.Generic;

namespace NFine.Domain.IRepository.BusinessManage
{
    public interface IGiftListRepository : IRepositoryBase<GiftListEntity>
    {
        List<GiftListEntity> GetGiftList();
    }
}
