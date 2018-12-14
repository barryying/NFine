using NFine.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.BusinessManage
{
    public class ViewAllCandidateMap : EntityTypeConfiguration<ViewAllCandidate>
    {
        public ViewAllCandidateMap()
        {
            this.ToTable("F_ViewAllCandidate");
            this.HasKey(t => t.CandidateID);
        }
    }
}
