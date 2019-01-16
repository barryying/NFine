using System;

namespace NFine.Domain.Entity.BusinessManage
{
    public class EventEntity : IEntity<EventEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }        
        public string F_EnCode { get; set; }
        public string F_Name { get; set; }
        public DateTime? F_ApplyStartTime { get; set; }
        public DateTime? F_ApplyEndTime { get; set; }
        public DateTime? F_VoteStartTime { get; set; }
        public DateTime? F_VoteEndTime { get; set; }
        public int? F_VoteEndDuration { get; set; }
        public string F_Mode { get; set; }
        public bool? F_RankingListIsShow { get; set; }
        public bool? F_CandidateListIsShow { get; set; }
        public bool? F_EndIsShow { get; set; }
        public string F_Remark { get; set; }
        public string F_ApplyIsNeedFollow { get; set; }
        public string F_ApplyAuditType { get; set; }
        public int? F_ApplyMinNumber { get; set; }
        public int? F_ApplyNumberPerAll { get; set; }
        public int? F_ApplyNumberPerDayAndPerson { get; set; }
        public int? F_ApplyNumberPerPerson { get; set; }
        public int? F_ApplyVoteTimeDuration { get; set; }
        public int? F_ApplyGiftMaxNumberPerPerson { get; set; }
        public bool? F_ApplyVoteIsRemindMsg { get; set; }
        public bool? F_ApplyVoteIsAnonymous { get; set; }
        public bool? F_ApplyVoteIsToSelf { get; set; }
        public bool? F_ApplyVoteIsValidation { get; set; }
        public string F_ApplyVoteValidID { get; set; }
        public string F_ApplyVoteValidKey { get; set; }
        public int? F_ApplyAutoLockDuration { get; set; }
        public int? F_ApplyAutoLockVoteNum { get; set; }
        public int? F_ApplyAutoLockTime { get; set; }
        public string F_ApplyRegionLimit { get; set; }
        public string F_ApplyRegionLimitAPIKey { get; set; }
        public string F_VoteCarouselIDs { get; set; }
        public string F_VoteSuccPictureID { get; set; }
        public string F_Link { get; set; }
        public bool? F_VoteSuccPictureIsToGift { get; set; }
        public string F_VoteRules { get; set; }
        public string F_VotePrizeIntroDuction { get; set; }
        public string F_PageShowSortType { get; set; }
        public int? F_PageVirtualViewnumber { get; set; }
        public int? F_PageRankingListMaxnumber { get; set; }
        public bool? F_PageIsShowGiftDetail { get; set; }
        public string F_PageNerverFollowNotice { get; set; }
        public string F_PageRollNotice { get; set; }
        public string F_PageBackgroundSong { get; set; }
        public string F_PageColorCustom { get; set; }
        public string F_PageColorTheme { get; set; }
        public int? F_PageBorderEffectsNum { get; set; }
        public int? F_PageBorderEffectsDuration { get; set; }
        public string F_PageBorderEffectsPictureID { get; set; }
        public string F_PageFloatingEffects { get; set; }
        public int? F_ApplyPictureMinNum { get; set; }
        public int? F_ApplyPicturemaxNum { get; set; }
        public string F_ApplyName { get; set; }
        public string F_ApplyPhone { get; set; }
        public string F_ApplyDeclaration { get; set; }
        public string F_ApplyDetail { get; set; }
        public string F_ApplyNotice { get; set; }
        public bool? F_GiftIsShowList { get; set; }
        public string F_GiftPerOne { get; set; }
        public string F_GiftUnit { get; set; }
        public string F_GiftID { get; set; }
        public bool? F_Status { get; set; }
        public int? F_ViewNumber { get; set; }
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
