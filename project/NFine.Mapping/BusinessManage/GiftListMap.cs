using NFine.Domain.Entity.BusinessManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.BusinessManage
{
    public class GiftListMap : EntityTypeConfiguration<GiftListEntity>
    {
        public GiftListMap()
        {
            this.ToTable("Sys_GiftList");
            this.HasKey(t => t.F_Id);
        }
    }
}
