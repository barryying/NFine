using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.IRepository.BusinessManage;
using NFine.Repository.BusinessManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.BusinessManage
{
    public class GiftListApp
    {
        private IGiftListRepository service = new GiftListRepository();

        public List<GiftListEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<GiftListEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_PaymentStatus.Contains(keyword));
            }
            if (!queryParam["timeType"].IsEmpty())
            {
                string timeType = queryParam["timeType"].ToString();
                DateTime startTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate();
                DateTime endTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate().AddDays(1);
                switch (timeType)
                {
                    case "1":
                        break;
                    case "2":
                        startTime = DateTime.Now.AddDays(-7);
                        break;
                    case "3":
                        startTime = DateTime.Now.AddMonths(-1);
                        break;
                    case "4":
                        startTime = DateTime.Now.AddMonths(-3);
                        break;
                    default:
                        break;
                }
                expression = expression.And(t => t.F_CreatorTime >= startTime && t.F_CreatorTime <= endTime);
            }
            return service.FindList(expression, pagination);
        }
        public List<GiftListEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
        }
        
        public GiftListEntity GetDatatistic(string keyValue)
        {
            string sql = "SELECT ";
            if (keyValue == "today")
            {
                sql += "NULLIF((SELECT Sum(F_Money) FROM Sys_GiftList WHERE CONVERT(varchar(100), F_CreatorTime, 111)=CONVERT(varchar(100), GETDATE(), 111)),0) AS TodayMoney,";
                sql += "NULLIF((SELECT Sum(F_Money) FROM Sys_GiftList WHERE CONVERT(varchar(100), F_CreatorTime, 111)=CONVERT(varchar(100), GETDATE()-1, 111)),0) AS YesterdayMoney,";
            }
            else if(keyValue == "week")
            {
                sql += "NULLIF((SELECT Sum(F_Money) FROM Sys_GiftList WHERE DATEPART(wk,F_CreatorTime)-DATEPART(wk,DATEADD(dd,-day(F_CreatorTime),F_CreatorTime))+1=DATEPART(wk,getdate())-DATEPART(wk,DATEADD(dd,-day(getdate()),getdate()))+1),0) AS TodayMoney,";
                sql += "NULLIF((SELECT Sum(F_Money) FROM Sys_GiftList WHERE DATEPART(wk,F_CreatorTime)-DATEPART(wk,DATEADD(dd,-day(F_CreatorTime),F_CreatorTime))+1=DATEPART(wk,getdate())-DATEPART(wk,DATEADD(dd,-day(getdate()),getdate()))),0) AS YesterdayMoney,";
            }
            else if(keyValue == "month")
            {
                sql += "NULLIF((SELECT Sum(F_Money) FROM Sys_GiftList WHERE month(F_CreatorTime)=month(GETDATE())),0) AS TodayMoney,";
                sql += "NULLIF((SELECT Sum(F_Money) FROM Sys_GiftList WHERE month(F_CreatorTime)=month(GETDATE())-1),0) AS YesterdayMoney,";
            }

            sql += "NULLIF((SELECT Sum(F_Money) FROM Sys_GiftList),0) AS TotalMoney,";
            sql += "NULLIF((SELECT count(*) FROM Sys_Event WHERE F_CreatorUserId = '" + OperatorProvider.Provider.GetCurrent().UserId + "'),0) AS TotalCount,";
            sql += "NULLIF((SELECT count(*) FROM Sys_Event WHERE F_CreatorUserId = '" + OperatorProvider.Provider.GetCurrent().UserId + "' AND F_VoteStartTime > GETDATE()),0) AS NotStartCount,";
            sql += "NULLIF((SELECT count(*) FROM Sys_Event WHERE F_CreatorUserId = '" + OperatorProvider.Provider.GetCurrent().UserId + "' AND F_VoteEndTime < GETDATE()),0) AS EndCount,";

            sql += "NULLIF((SELECT count(*) FROM Sys_Event WHERE F_CreatorUserId = '" + OperatorProvider.Provider.GetCurrent().UserId + "' AND F_VoteStartTime <= GETDATE() AND F_VoteEndTime >= GETDATE()),0) AS VotingCount,";
            sql += "NULLIF((SELECT count(*) FROM Sys_Event WHERE F_CreatorUserId = '" + OperatorProvider.Provider.GetCurrent().UserId + "' AND CONVERT(varchar(100), F_VoteStartTime, 111) = CONVERT(varchar(100), GETDATE(), 111)),0) AS VoteTodayStartCount,";
            sql += "NULLIF((SELECT count(*) FROM Sys_Event WHERE F_CreatorUserId = '" + OperatorProvider.Provider.GetCurrent().UserId + "' AND CONVERT(varchar(100), F_VoteEndTime, 111) = CONVERT(varchar(100), GETDATE(), 111)),0) AS VoteTodayEndCount,";

            sql += "NULLIF((SELECT Sum(F_VoteNumber) FROM Sys_Candidate WHERE F_CreatorUserId = '" + OperatorProvider.Provider.GetCurrent().UserId + "'),0) AS TotalVoteNumber,";
            sql += "NULLIF((SELECT Sum(F_ViewNumber) FROM Sys_Candidate WHERE F_CreatorUserId = '" + OperatorProvider.Provider.GetCurrent().UserId + "'),0) AS TotalViewNumber,";
            sql += "NULLIF((SELECT Sum(F_GiftNumber) FROM Sys_Candidate WHERE F_CreatorUserId = '" + OperatorProvider.Provider.GetCurrent().UserId + "'),0) AS TotalGiftNumber, ";
            sql += "F_ID,F_ParentId,F_CandidateID,F_GiftID,F_Money,F_PaymentStatus,F_IP,F_OPENID,F_SortCode,F_DeleteMark,F_EnabledMark,F_CreatorTime,F_CreatorUserId,F_LastModifyTime,F_LastModifyUserId,F_DeleteTime,F_DeleteUserId FROM Sys_GiftList";
            return service.FindList(sql).FirstOrDefault();
        }
    }
}
