using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
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
                service.Update(pictureEntity);
            }
            else
            {
                pictureEntity.Create();
                service.Insert(pictureEntity);
            }
        }

        private List<PictureEntity> GetList(string id, string uploadType)
        {
            List<string> urlList = new List<string>();
            string sql = "SELECT * from Sys_Picture ";
            if (uploadType == "1" || uploadType == "2" || uploadType == "3")
            {
                sql += "where F_EventId = '";
            }
            else if (uploadType == "4")
            {
                sql += "where F_CandidateId = '";
            }
            else if (uploadType == "5")
            {
                sql += "where F_GiftID = '";
            }
            sql = sql + id + "' and F_Type='" + uploadType + "'";
            List<PictureEntity> pictureentity = service.FindList(sql);
            return pictureentity;
        }

        public List<PictureEntity> GetImageList(string id, string uploadType)
        {
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
