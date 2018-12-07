using System;

namespace NFine.Domain.Entity.BusinessManage
{
    public class PictureEntity : IEntity<PictureEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }
        public string F_Type { get; set; }
        public string F_Link { get; set; }
        public string F_VirtualPath { get; set; }
        public DateTime? F_UploadDate { get; set; }
        public string F_SmallSize { get; set; }
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
