using NFine.Code;
using NFine.Domain.IRepository.BusinessManage;
using NFine.Domain.ViewModel;
using NFine.Repository.BusinessManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.BusinessManage
{
    public class ViewAllCandidateApp
    {
        private IViewAllCandidateRepository service = new ViewAllCandidateRespository();
        
        public List<ViewAllCandidate> GetList(string eventId)
        {
            string sql = "SELECT * FROM F_ViewAllCandidate where F_EventId = '" + eventId + "'";
            return service.FindList(sql);
        }
    }
}
