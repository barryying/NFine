using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Domain.IRepository.BusinessManage;
using NFine.Repository.BusinessManage;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.BusinessManage
{
    public class PictureApp
    {
        private IPictureRepository service = new PictureRepository();

        public List<PictureEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_UploadDate).ToList();
        }
        public PictureEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
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
        public void SubmitForm(PictureEntity pictureEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                pictureEntity.Modify(keyValue);
                new LogApp().WriteDbLog(new LogEntity
                {
                    F_ModuleName = "NFine.Application.BusinessManage.PictureApp.SubmitForm修改图片",
                    F_Type = DbLogType.Update.ToString(),
                    F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                    F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                    F_Result = true,
                    F_Description = "修改了图片: " + pictureEntity.F_Id,
                });
                service.Update(pictureEntity);
            }
            else
            {
                pictureEntity.Create();
                new LogApp().WriteDbLog(new LogEntity
                {
                    F_ModuleName = "NFine.Application.BusinessManage.PictureApp.SubmitForm添加图片",
                    F_Type = DbLogType.Update.ToString(),
                    F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                    F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                    F_Result = true,
                    F_Description = "添加了图片: " + pictureEntity.F_Id,
                });
                service.Insert(pictureEntity);
            }
        }

        private List<PictureEntity> GetList(string id, string uploadType)
        {
            List<string> urlList = new List<string>();
            string sql = "SELECT * from Sys_Picture where 1=1";
            if (uploadType == "1" || uploadType == "2" || uploadType == "3" || uploadType == "6")
            {
                sql += " and F_EventId = '" + id;
            }
            else if (uploadType == "4")
            {
                sql += " and F_CandidateId = '" + id;
            }
            else if (uploadType == "5")
            {
                sql += " and F_GiftID = '" + id;
            }
            sql = sql + "' and F_Type='" + uploadType + "'";
            List<PictureEntity> pictureentity = service.FindList(sql);
            return pictureentity;
        }

        public List<PictureEntity> GetImageList(string id, string uploadType)
        {
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Application.BusinessManage.PictureApp.GetImageList获取图片列表",
                F_Type = DbLogType.Visit.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "获取图片列表",
            });
            List<PictureEntity> pictureentity = GetList(id, uploadType);
            if (!pictureentity.IsEmpty())
            {
                return pictureentity;
            }
            else
                return null;
        }
        
        public Dictionary<string, string> GetImageUrl(string id, string uploadType)
        {
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Application.BusinessManage.PictureApp.GetImageUrl获取图片虚拟路径列表",
                F_Type = DbLogType.Visit.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "获取图片虚拟路径列表",
            });
            Dictionary<string, string> urlDic = new Dictionary<string, string>();
            List<PictureEntity> pictureentity = GetList(id, uploadType);
            if (!pictureentity.IsEmpty())
            {
                foreach(PictureEntity entity in pictureentity)
                {
                    urlDic.Add(entity.F_Id, "/" + entity.F_VirtualPath.Replace("\\", "/"));
                }
                return urlDic;
            }
            else
                return null;            
        }
    }
}
