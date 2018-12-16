using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.IRepository.BusinessManage;
using NFine.Repository.BusinessManage;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NFine.Application.BusinessManage
{
    public class EventApp
    {
        private IEventRepository service = new EventRepository();

        public List<EventEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<EventEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_Name.Contains(keyword));
                expression = expression.Or(t => t.F_ApplyName.Contains(keyword));
            }
            if (!queryParam["timeType"].IsEmpty())
            {
                string timeType = queryParam["timeType"].ToString();
                DateTime currentTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate();
                switch (timeType)
                {
                    case "1":    //全部活动
                        break;
                    case "2":    //报名中
                        expression = expression.And(t => t.F_ApplyStartTime <= currentTime && t.F_ApplyEndTime >= currentTime);
                        break;
                    case "3":    //进行中
                        expression = expression.And(t => t.F_VoteStartTime <= currentTime && t.F_VoteEndTime >= currentTime);
                        break;
                    case "4":    //已结束
                        expression = expression.And(t => t.F_VoteEndTime < currentTime);
                        break;
                    case "5":    //今日开始
                        expression = expression.And(t => t.F_VoteStartTime == currentTime);
                        break;
                    case "6":    //今日结束
                        expression = expression.And(t => t.F_VoteEndTime == currentTime);
                        break;
                    case "7":    //未开始
                        expression = expression.And(t => t.F_VoteStartTime > currentTime);
                        break;
                    case "8":    //待审核
                        expression = expression.And(t => t.F_EnabledMark == false);
                        break;
                    default:
                        break;
                }
            }
            return service.FindList(expression, pagination);
        }
        //public List<EventEntity> GetList(Pagination pagination, string keyword)
        //{
        //    var expression = ExtLinq.True<EventEntity>();
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        expression = expression.And(t => t.F_Name.Contains(keyword));
        //        expression = expression.Or(t => t.F_ApplyName.Contains(keyword));
        //    }
        //    return service.FindList(expression, pagination);
        //}
        public List<EventEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
        }
        public EventEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void UpdateForm(EventEntity eventEntity)
        {
            service.Update(eventEntity);
        }

        public void DeleteForm(string keyValue)
        {
            //if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            //{
            //    throw new Exception("删除失败！操作的对象包含了下级数据。");
            //}
            //else
            //{
                service.Delete(t => t.F_Id == keyValue);
            //}
        }
        public void SubmitForm(EventEntity eventEntity, string keyValue, string isClone)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (isClone == "1")
                {
                    SubmitCloneButton(eventEntity);
                }
                else
                {
                    eventEntity.Modify(keyValue);
                    service.Update(eventEntity);
                }
            }
            else
            {
                eventEntity.Create();
                service.Insert(eventEntity);
            }
        }
        public void SubmitCloneButton(EventEntity eventEntity)
        {
            var data = this.GetList();
            eventEntity.F_Id = Common.GuId();
            eventEntity.F_CreatorTime = DateTime.Now;
            eventEntity.F_CreatorUserId = OperatorProvider.Provider.GetCurrent().UserId;
            service.SubmitCloneButton(eventEntity);
        }

        public string GetEventPrize(string eventId)
        {
            IEventRepository eventservice = new EventRepository();
            string sql = "SELECT F_VoteRules,F_VotePrizeIntroDuction,* from Sys_Event where F_ID='" + eventId + "'";
            List<EventEntity> evententity = eventservice.FindList(sql);

            string result = "";
            if (!evententity.IsEmpty())
            {
                if (evententity[0].F_VotePrizeIntroDuction != null)
                {
                    result += evententity[0].F_VotePrizeIntroDuction.ToString();
                    return result;
                }
                else if (evententity[0].F_VoteRules != null)
                {
                    result += evententity[0].F_VoteRules.ToString();
                    return result;
                }
                else
                    return "";
            }
            else
                return "";
        }
    }
}
