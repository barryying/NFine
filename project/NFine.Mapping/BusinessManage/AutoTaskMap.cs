using NFine.Domain.Entity.BusinessManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.BusinessManage
{
    public class AutoTaskMap : EntityTypeConfiguration<AutoTaskEntity>
    {
        public AutoTaskMap()
        {
            this.ToTable("Sys_AutoTask");
            this.HasKey(t => t.F_Id);
        }
    }
}
