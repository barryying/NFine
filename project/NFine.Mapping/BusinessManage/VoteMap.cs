using NFine.Domain.Entity.BusinessManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.BusinessManage
{
    public class VoteMap : EntityTypeConfiguration<VoteEntity>
    {
        public VoteMap()
        {
            this.ToTable("Sys_Vote");
            this.HasKey(t => t.F_Id);
        }
    }
}
