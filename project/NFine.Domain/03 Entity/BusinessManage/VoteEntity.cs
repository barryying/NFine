using System;

namespace NFine.Domain.Entity.BusinessManage
{
    public class VoteEntity : IEntity<VoteEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_VoteType { get; set; }    // 1 代表后台投票   2代表微信投票
        public int? F_VoteNumber { get; set; }
        public string F_ParentId { get; set; }
        public string F_CandidateID { get; set; }
        public string F_WX_Nick { get; set; }
        public string F_WX_id { get; set; }
        public string F_IP { get; set; }
        public string F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
    }
}
