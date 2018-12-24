using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.BusinessManage
{
    public class GiftListEntity : IEntity<GiftListEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }
        public string F_CandidateID { get; set; }
        public string F_GiftID { get; set; }
        public decimal? F_Money { get; set; }
        public string F_PaymentStatus { get; set; }
        public string F_IP { get; set; }
        public string F_OPENID { get; set; }
        public string F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public decimal? TodayMoney { get; set; }
        public decimal? YesterdayMoney { get; set; }
        public decimal? TotalMoney { get; set; }
        public int? TotalCount { get; set; }
        public int? NotStartCount { get; set; }
        public int? EndCount { get; set; }
        public int? VotingCount { get; set; }
        public int? VoteTodayStartCount { get; set; }
        public int? VoteTodayEndCount { get; set; }
        public int? TotalVoteNumber { get; set; }
        public int? TotalViewNumber { get; set; }
        public int? TotalGiftNumber { get; set; }
    }
}
