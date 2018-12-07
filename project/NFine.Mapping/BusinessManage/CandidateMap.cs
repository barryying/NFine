using NFine.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.BusinessManage
{
    public class CandidateMap : EntityTypeConfiguration<CandidateEntity>
    {
        public CandidateMap()
        {
            this.ToTable("Sys_Candidate");
            this.HasKey(t => t.F_Id);
        }
    }
}
