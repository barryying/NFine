using NFine.Application.BusinessManage;
using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.MicroEvent.Controllers
{
    public class AddEventController : ControllerBase
    {
        private EventApp eventApp = new EventApp();
        private PictureApp pictureapp = new PictureApp();
        //
        // GET: /MicroEvent/AddEvent/

        public ActionResult AddEvent()
        {
            return View();
        }
        public ActionResult GenLink()
        {
            return View();
        }

        //[HttpGet]
        //[HandlerAjaxOnly]
        //public ActionResult GetTreeSelectJson()
        //{
        //    var data = eventApp.GetList();
        //    var treeList = new List<TreeSelectModel>();
        //    foreach (EventEntity item in data)
        //    {
        //        TreeSelectModel treeModel = new TreeSelectModel();
        //        treeModel.id = item.F_Id;
        //        treeModel.text = item.F_Name;
        //        treeModel.parentId = item.F_ParentId;
        //        treeModel.data = item;
        //        treeList.Add(treeModel);
        //    }
        //    return Content(treeList.TreeSelectJson());
        //}
        //[HttpGet]
        //[HandlerAjaxOnly]
        //public ActionResult GetTreeJson()
        //{
        //    var data = eventApp.GetList();
        //    var treeList = new List<TreeViewModel>();
        //    foreach (EventEntity item in data)
        //    {
        //        TreeViewModel tree = new TreeViewModel();
        //        bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
        //        tree.id = item.F_Id;
        //        tree.text = item.F_Name;
        //        tree.value = item.F_EnCode;
        //        tree.parentId = item.F_ParentId;
        //        tree.isexpand = true;
        //        tree.complete = true;
        //        tree.hasChildren = hasChildren;
        //        treeList.Add(tree);
        //    }
        //    return Content(treeList.TreeViewJson());
        //}
        //[HttpGet]
        //[HandlerAjaxOnly]
        //public ActionResult GetTreeGridJson(string keyword)
        //{
        //    var data = eventApp.GetList();
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        data = data.TreeWhere(t => t.F_Name.Contains(keyword));
        //    }
        //    var treeList = new List<TreeGridModel>();
        //    foreach (EventEntity item in data)
        //    {
        //        TreeGridModel treeModel = new TreeGridModel();
        //        bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
        //        treeModel.id = item.F_Id;
        //        treeModel.isLeaf = hasChildren;
        //        treeModel.parentId = item.F_ParentId;
        //        treeModel.expanded = hasChildren;
        //        treeModel.entityJson = item.ToJson();
        //        treeList.Add(treeModel);
        //    }
        //    return Content(treeList.TreeGridJson());
        //}

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = eventApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = eventApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SubmitForm(EventEntity eventEntity, string keyValue, string isClone)
        {
            eventApp.SubmitForm(eventEntity, keyValue, isClone);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            eventApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Disabled(string keyValue)
        {
            EventEntity eventEntity = new EventEntity();
            eventEntity.F_Id = keyValue;
            eventEntity.F_Status = false;
            eventApp.UpdateForm(eventEntity);
            return Success("活动禁用成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Enabled(string keyValue)
        {
            EventEntity eventEntity = new EventEntity();
            eventEntity.F_Id = keyValue;
            eventEntity.F_Status = true;
            eventApp.UpdateForm(eventEntity);
            return Success("活动启用成功。");
        }

        [HttpGet]
        [AllowAnonymous]
        // /MicroEvent/AddEvent/GetEventPrize?keyvalue=aa51beeb-e55c-4a85-b1bc-1395eaa65c28
        public ActionResult GetEventPrize(string keyValue)
        {
            var data = eventApp.GetEventPrize(keyValue);
            return Content(data);
        }
        
        [HttpPost]
        public ActionResult UploadImage(string keyvalue, string uploadType = "1", string pictureLink = "#")
        {
            Image img = null;
            int towidth = 0;
            int toheight = 0;
            string result = "success";
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase Files = Request.Files;//该集合是所有fileupload文件的集合。
                    for (int i = 0; i < Files.Count; i++)
                    {
                        HttpPostedFileBase PostedFile = Files[i];
                        if (PostedFile.ContentLength > 0)
                        {
                            string FileName = PostedFile.FileName;//文件名自行处理
                            //大图文件夹
                            string Dir = "Resource\\Image\\" + MappingDirName(uploadType) + "\\";
                            if (!FileHelper.IsExistDirectory(Dir))
                                FileHelper.CreateDir(Dir);
                            //小图文件夹
                            string DirSmall = "Resource\\ImageSmall\\" + MappingDirName(uploadType) + "\\";
                            if (!FileHelper.IsExistDirectory(DirSmall))
                                FileHelper.CreateDir(DirSmall);

                            string savePath = Server.MapPath("/") + Dir + FileName;
                            string savePathSmall = Server.MapPath("/") + DirSmall + FileName;

                            #region 保存文件

                            //保存大图文件
                            PostedFile.SaveAs(savePath);
                            img = Image.FromFile(savePath);

                            //保存小图文件
                            int nW = 400;
                            int nH = 400;
                            if ((img.Width < nW) && (img.Height < nH))
                            {
                                if (savePathSmall.EndsWith(".jpg"))
                                {
                                    savePathSmall = savePathSmall.Remove(savePathSmall.LastIndexOf(".")) + ".jpg";
                                    img.Save(savePathSmall, ImageFormat.Jpeg);
                                }
                                else
                                {
                                    img.Save(savePathSmall, ImageFormat.Jpeg);
                                }
                            }
                            else
                            {
                                towidth = nW;
                                toheight = nH;
                                int x = 0;
                                int y = 0;
                                int ow = img.Width;
                                int oh = img.Height;
                                if (ow > oh)
                                {
                                    toheight = (oh * nW) / ow;
                                    y = (nH - toheight) / 2;
                                }
                                else
                                {
                                    towidth = (ow * nH) / oh;
                                    x = (nW - towidth) / 2;
                                }
                                Image bmp = new Bitmap(towidth, toheight);
                                Graphics g = Graphics.FromImage(bmp);
                                g.InterpolationMode = InterpolationMode.High;
                                g.SmoothingMode = SmoothingMode.HighQuality;
                                g.Clear(Color.White);
                                g.DrawImage(img, new Rectangle(0, 0, towidth, toheight), new Rectangle(0, 0, ow, oh), GraphicsUnit.Pixel);

                                if (savePathSmall.EndsWith(".jpg"))
                                {
                                    savePathSmall = savePathSmall.Remove(savePathSmall.LastIndexOf(".")) + ".jpg";
                                    bmp.Save(savePathSmall, ImageFormat.Jpeg);
                                }
                                else
                                {
                                    bmp.Save(savePathSmall, ImageFormat.Jpeg);
                                }
                                if (bmp != null)
                                {
                                    bmp.Dispose();
                                }
                            }
                            if (img != null)
                            {
                                img.Dispose();
                            }
                            #endregion

                            //写入数据库
                            PictureEntity pictureentity = new PictureEntity();
                            pictureentity.F_Id = Common.GuId();
                            pictureentity.F_Type = uploadType;
                            pictureentity.F_Link = pictureLink;
                            pictureentity.F_VirtualPath = Dir + FileName;
                            pictureentity.F_UploadDate = DateTime.Now;
                            pictureentity.F_SmallSize = towidth.ToString() + "*" + toheight.ToString(); 
                            pictureentity.F_CreatorTime = DateTime.Now;
                            pictureentity.F_CreatorUserId = OperatorProvider.Provider.GetCurrent().UserId;

                            //pictureapp.SubmitForm(pictureentity, null);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result = "failed：" + ex.Message;
            }
            return Json(result);
        }

        private string MappingDirName(string uploadType)
        {
            string DirName = "CarouselPicture";
            // 1表示轮播图 CarouselPicture    2表示广告图片 AdvertisementPicture 3表示边框特效图片 BorderEffectsPicture 
            // 4表示选手图片 CandidatePicture 5表示礼物图片 GiftPicture
            switch (uploadType)
            {
                case "1":
                    DirName = "CarouselPicture";
                    break;
                case "2":
                    DirName = "AdvertisementPicture";
                    break;
                case "3":
                    DirName = "BorderEffectsPicture";
                    break;
                case "4":
                    DirName = "CandidatePicture";
                    break;
                case "5":
                    DirName = "GiftPicture";
                    break;
                default:
                    break;
            }
            return DirName;
        }
    }
}
