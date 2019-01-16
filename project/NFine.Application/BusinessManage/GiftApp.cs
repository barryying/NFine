using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Domain.IRepository.BusinessManage;
using NFine.Repository.BusinessManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.BusinessManage
{
    public class GiftApp
    {
        private IGiftRepository service = new GiftRepository();

        public List<GiftEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<GiftEntity>();
            var queryParam = queryJson.ToJObject();
            //if (!queryParam["eventId"].IsEmpty())
            //{
            //    string eventId = queryParam["eventId"].ToString();
            //    expression = expression.And(t => t.F_EventID.Equals(eventId));
            //}
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_Name.Contains(keyword));
            }
            if (!queryParam["timeType"].IsEmpty())
            {
                string timeType = queryParam["timeType"].ToString();
                switch (timeType)
                {
                    case "1":    //全部礼物
                        break;
                    default:
                        break;
                }
            }
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Application.BusinessManage.GiftApp.GetList礼物设置页面",
                F_Type = DbLogType.Visit.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "访问了礼物设置页面",
            });
            return service.FindList(expression, pagination);
        }

        public List<GiftEntity> GetTreeSelectJson(string eventId)
        {
            if (eventId != "")
                return service.IQueryable().Where(t => t.F_EventID == eventId && t.F_EnabledMark == true).OrderBy(t => t.F_CreatorTime).ToList();
            else
                return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
        }

        public List<GiftEntity> GetList(string eventId)
        {
            if(eventId != "")
                return service.IQueryable().Where(t => t.F_EventID == eventId).OrderBy(t => t.F_CreatorTime).ToList();
            else
                return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
        }

        public GiftEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void UpdateForm(GiftEntity entity)
        {
            service.Update(entity);
        }

        public void DeleteForm(string keyValue)
        {
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Application.BusinessManage.DeleteForm删除礼物",
                F_Type = DbLogType.Delete.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "删除了礼物: " + keyValue,
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
        public void SubmitForm(GiftEntity entity, string keyValue, string isClone, string eventId)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (isClone == "1")
                {
                    SubmitCloneButton(entity,eventId);
                }
                else
                {
                    entity.Modify(keyValue);
                    new LogApp().WriteDbLog(new LogEntity
                    {
                        F_ModuleName = "NFine.Application.BusinessManage.SubmitForm修改礼物",
                        F_Type = DbLogType.Update.ToString(),
                        F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                        F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                        F_Result = true,
                        F_Description = "修改了礼物: " + entity.F_Id,
                    });
                    service.Update(entity);
                }
            }
            else
            {
                entity.Create();
                if (!eventId.IsEmpty())
                {
                    entity.F_EventID = eventId;
                }
                entity.F_EnabledMark = true;
                new LogApp().WriteDbLog(new LogEntity
                {
                    F_ModuleName = "NFine.Application.BusinessManage.SubmitForm添加礼物",
                    F_Type = DbLogType.Create.ToString(),
                    F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                    F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                    F_Result = true,
                    F_Description = "添加了礼物: " + entity.F_Id,
                });
                service.Insert(entity);
            }
        }
        public void SubmitCloneButton(GiftEntity entity,string eventId)
        {
            var data = this.GetList("");
            entity.F_Id = Common.GuId();
            entity.F_CreatorTime = DateTime.Now;
            entity.F_CreatorUserId = OperatorProvider.Provider.GetCurrent().UserId;
            entity.F_EnabledMark = true;
            if (!eventId.IsEmpty())
            {
                entity.F_EventID = eventId;
            }
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Application.BusinessManage.SubmitCloneButton复制添加礼物",
                F_Type = DbLogType.Create.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "复制添加礼物: " + entity.F_Id,
            });
            service.SubmitCloneButton(entity);
        }
    }
}
