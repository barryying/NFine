using NFine.Domain.Entity.BusinessManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.BusinessManage
{
    public class GiftListInsertMap : EntityTypeConfiguration<GiftListInsertEntity>
    {
        public GiftListInsertMap()
        {
            this.ToTable("Sys_GiftList_insert_temp");
            this.HasKey(t => t.F_Id);
        }
    }
}
