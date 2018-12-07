using System;

namespace NFine.Domain.Entity.BusinessManage
{
    public class GiftEntity : IEntity<GiftEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }
        public string F_EventID { get; set; }
        public string F_EnCode { get; set; }
        public string F_Name { get; set; }
        public string F_PictureID { get; set; }
        public float? F_Price { get; set; }
        public int? F_VoteNumber { get; set; }
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
