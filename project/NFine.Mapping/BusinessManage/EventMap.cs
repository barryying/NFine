using NFine.Domain.Entity.BusinessManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.BusinessManage
{
    public class EventMap : EntityTypeConfiguration<EventEntity>
    {
        public EventMap()
        {
            this.ToTable("Sys_Event");
            this.HasKey(t => t.F_Id);
        }
    }
}
