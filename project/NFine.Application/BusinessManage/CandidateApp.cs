using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity;
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
using System.Threading.Tasks;

namespace NFine.Application.BusinessManage
{
    public class CandidateApp
    {
        private ICandidateRepository service = new CandidateRepository();

        public List<CandidateEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<CandidateEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["eventId"].IsEmpty())
            {
                string eventId = queryParam["eventId"].ToString();
                expression = expression.And(t => t.F_EventID.Contains(eventId));
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_Name.Contains(keyword));
                expression = expression.Or(t => t.F_Phone.Contains(keyword));
                expression = expression.Or(t => t.F_Introduction.Contains(keyword));
            }
            if (!queryParam["timeType"].IsEmpty())
            {
                string timeType = queryParam["timeType"].ToString();
                DateTime currentTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate();
                switch (timeType)
                {
                    case "1":    //全部选手
                        break;
                    case "2":    //待审核
                        expression = expression.And(t => t.F_AuditIsOK == false);
                        break;
                    case "3":    //已审核
                        expression = expression.And(t => t.F_AuditIsOK == true);
                        break;
                    default:
                        break;
                }
            }
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Application.BusinessManage.CandidateApp.GetList选手管理",
                F_Type = DbLogType.Visit.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "访问了选手管理页面",
            });
            return service.FindList(expression, pagination);
        }

        public List<CandidateEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
        }
        
        public CandidateEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void UpdateForm(CandidateEntity candidateEntity)
        {
            service.Update(candidateEntity);
        }

        public void DeleteForm(string keyValue)
        {
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Application.BusinessManage.CandidateApp.DeleteForm删除选手",
                F_Type = DbLogType.Delete.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "删除了选手: " + keyValue,
            });
            //if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            //{
            //    throw new Exception("删除失败！操作的对象包含了下级数据。");
            //}
            //else
            //{
            service.Delete(t => t.F_Id == keyValue);
            //}
        }
        public void SubmitForm(CandidateEntity candidateEntity, string keyValue, string eventID)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                candidateEntity.Modify(keyValue);
                new LogApp().WriteDbLog(new LogEntity
                {
                    F_ModuleName = "NFine.Application.BusinessManage.CandidateApp.SubmitForm修改选手",
                    F_Type = DbLogType.Update.ToString(),
                    F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                    F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                    F_Result = true,
                    F_Description = "修改了选手: " + candidateEntity.F_Id,
                });
                service.Update(candidateEntity);
            }
            else
            {
                if (!eventID.IsEmpty())
                {
                    candidateEntity.F_EventID = eventID;
                }
                candidateEntity.F_VoteNumber = 0;
                candidateEntity.F_GiftNumber = 0;
                candidateEntity.F_ViewNumber = 0;
                candidateEntity.F_VirtualHeat = 0;
                candidateEntity.F_AuditIsOK = true;
                candidateEntity.Create();
                new LogApp().WriteDbLog(new LogEntity
                {
                    F_ModuleName = "NFine.Application.BusinessManage.CandidateApp.SubmitForm添加选手",
                    F_Type = DbLogType.Create.ToString(),
                    F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                    F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                    F_Result = true,
                    F_Description = "添加了选手: " + candidateEntity.F_Id,
                });
                service.Insert(candidateEntity);
            }
        }
        public List<CandidateEntity> GetRankingList(string eventId)
        {
            //new LogApp().WriteDbLog(new LogEntity
            //{
            //    F_ModuleName = "NFine.Application.BusinessManage.CandidateApp.GetRankingList查询活动排行榜接口",
            //    F_Type = DbLogType.Visit.ToString(),
            //    F_Account = OperatorProvider.Provider.GetCurrent().UserId,
            //    F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
            //    F_Result = true,
            //    F_Description = "访问了活动: " + eventId + "的活动排行榜接口",
            //});
            if (eventId != "")
            {
                IEventRepository eventservice = new EventRepository();
                string sql1 = "SELECT F_PageRankingListMaxnumber,* from Sys_Event where F_ID='" + eventId + "'";
                List<EventEntity> evententity = eventservice.FindList(sql1);
                if (!evententity.IsEmpty())
                {
                    if (evententity[0].F_PageRankingListMaxnumber != null)
                    {
                        string sql2 = "select top " + evententity[0].F_PageRankingListMaxnumber.ToString() + " * from Sys_Candidate where F_AuditIsOK=1  and F_EventID='" + eventId + "' order by F_VoteNumber DESC";
                        return service.FindList(sql2);
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}
