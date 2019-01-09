using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Domain.IRepository.BusinessManage;
using NFine.Domain.ViewModel;
using NFine.Repository.BusinessManage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

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
            var list = service.FindList(expression, pagination);
            var list2 = service.GetGiftList();
            return list.Where(a => list2.Exists(t => a.F_Id.Contains(t.F_Id))).ToList();
        }

        public List<GiftListEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
        }

        public List<GiftListEntity> GetTodayList()
        {
            string time = DateTime.Now.ToShortDateString();
            DateTime time1 = Convert.ToDateTime(time + " 0:00:00");  // 数字前 记得 加空格
            DateTime time2 = Convert.ToDateTime(time + " 23:59:59");
            //expression = expression.And(t => t.UpdateTime > time1 & t.UpdateTime < time2);
            return service.IQueryable().Where(t => t.F_CreatorTime > time1 & t.F_CreatorTime < time2).ToList();
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

        //SqlDependency dependency = new SqlDependency();
        //private void Update()
        //{
        //    //此处 要注意 不能使用*  表名要加[dbo]  否则会出现一直调用执行 OnChange
        //    string sql = "SELECT F_ID,[F_CandidateID],[F_GiftID],[F_Money],[F_PaymentStatus] FROM [dbo].[Sys_GiftList]";

        //    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
        //    //必须要执行一下command
        //    service.FindEntity(sql);
        //    Console.WriteLine(dependency.HasChanges);
        //}



        ////update insert delete都会进入
        //private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        //{
        //    Console.WriteLine("onchange方法中：" + dependency.HasChanges);
        //    Console.WriteLine("数据库数据发生变化" + DateTime.Now);
        //    //这里要再次调用
        //    Update();
        //}
        
        public void SubmitForm(string keyValue, string money, string giftid, string paymentstatus, string ip, string openid)
        {
            GiftListEntity giftListEntity = new GiftListEntity();
            giftListEntity.F_CandidateID = keyValue;
            giftListEntity.F_GiftID = giftid;
            giftListEntity.F_Money = decimal.Parse(money);
            giftListEntity.F_PaymentStatus = paymentstatus;
            giftListEntity.F_IP = ip;
            giftListEntity.F_OPENID = openid;
            giftListEntity.F_CreatorTime = DateTime.Now;
            giftListEntity.F_CreatorUserId = openid;

            giftListEntity.Create();
            service.Insert(giftListEntity);
        }
        
        public List<StatisticMoneyModel> GetStatisticMoney()
        {
            var statisticMoneyModel = new List<StatisticMoneyModel>();
            string sql = "SELECT * from Sys_GiftList";
            List<GiftListEntity> giftListentity = service.FindList(sql);
            var sums = from temp in giftListentity
                        where temp.F_PaymentStatus.Equals("3")
                        group temp by temp.F_CreatorTime.ToDateString() into g
                        select new { DateStr = g.Key, Count = g.Sum(temp => temp.F_Money) };

            foreach (var sum in sums)
                statisticMoneyModel.Add(new StatisticMoneyModel { DateStr = sum.DateStr.ToString(), CountMoney = sum.Count });
             return statisticMoneyModel;
        }
    }
}
