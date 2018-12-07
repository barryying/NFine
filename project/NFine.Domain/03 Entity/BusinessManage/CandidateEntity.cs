using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity
{
    public class CandidateEntity : IEntity<CandidateEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }
        public string F_EnCode { get; set; }
        public string F_Name { get; set; }
        public string F_SimpleSpelling { get; set; }
        public string F_PictureIDs { get; set; }
        public int? F_VoteNumber { get; set; }
        public int? F_GiftNumber { get; set; }
        public int? F_VirtualHeat { get; set; }
        public string F_Introduction { get; set; }
        public bool? F_AuditIsOK { get; set; }
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
