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
        
        public List<ViewAllCandidate> GetAllByEventID(string eventId)
        {
            string sql = "SELECT * FROM F_ViewAllCandidate where F_AuditIsOK=1 and F_EventId='" + eventId + "'";
            return service.FindList(sql);
        }

        public List<ViewAllCandidate> GetAllByCandidateID(string candidateId)
        {
            string sql = "SELECT * FROM F_ViewAllCandidate where CandidateId='" + candidateId + "'";
            return service.FindList(sql);
        }

        public List<ViewAllCandidate> GetMain(string eventId)
        {//COUNT(*) as CandidateTotalNumber,SUM(F_VoteNumber) as VoteTotalNumber,
            string sql = "SELECT EventViewTotalNumber, F_VoteStartTime, F_VoteEndTime, CandidateTotalNumber, VoteTotalNumber FROM (SELECT EventViewNumber as EventViewTotalNumber, F_VoteStartTime, F_VoteEndTime, COUNT(*) as CandidateTotalNumber, SUM(F_VoteNumber) as VoteTotalNumber FROM F_ViewAllCandidate where F_EventID = '" + eventId + "'group by EventViewNumber, F_VoteStartTime, F_VoteEndTime) as A";
            return service.FindList(sql);
        }
    }
}
