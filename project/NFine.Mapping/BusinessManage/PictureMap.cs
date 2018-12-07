using NFine.Domain.Entity.BusinessManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.BusinessManage
{
    public class PictureMap : EntityTypeConfiguration<PictureEntity>
    {
        public PictureMap()
        {
            this.ToTable("Sys_Picture");
            this.HasKey(t => t.F_Id);
        }
    }
}
