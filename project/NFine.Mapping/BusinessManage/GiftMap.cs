using NFine.Domain.Entity.BusinessManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.BusinessManage
{
    public class GiftMap : EntityTypeConfiguration<GiftEntity>
    {
        public GiftMap()
        {
            this.ToTable("Sys_Gift");
            this.HasKey(t => t.F_Id);
        }
    }
}
