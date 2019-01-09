using NFine.Application;
using NFine.Application.BusinessManage;
using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.BusinessManage;
using NFine.Domain.Entity.SystemSecurity;
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
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.GenLink查看活动链接",
                F_Type = DbLogType.Visit.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "点击了查看活动链接",
            });
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
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.Disabled禁用活动",
                F_Type = DbLogType.Update.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "修改了活动: " + eventEntity.F_Id + "  的 F_Status: 由‘true’改为了‘false’。",
            });
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
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.Enabled启用活动",
                F_Type = DbLogType.Update.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "修改了活动: " + eventEntity.F_Id + "  的 F_Status: 由‘false’改为了‘true’。",
            });
            return Success("活动启用成功。");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddViewNumber(string keyValue, int? viewnumber)
        {
            string discription = "";
            EventEntity eventEntity = eventApp.GetForm(keyValue);
            if (viewnumber.IsEmpty())
            {
                eventEntity.F_ViewNumber += 1;
                discription = "前台修改了选手: " + eventEntity.F_Id + "  的 F_ViewNumber: 增加了" + viewnumber + "浏览量。";
            }
            else
            {
                eventEntity.F_ViewNumber += viewnumber;
                discription = "后台修改了选手: " + eventEntity.F_Id + "  的 F_ViewNumber: 增加了" + viewnumber + "浏览量。";
            }
            eventApp.UpdateForm(eventEntity);
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.AddViewNumber加活动浏览量",
                F_Type = DbLogType.Update.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = discription,
            });
            return Success("加浏览量成功");
        }

        [HttpGet]
        [AllowAnonymous]
        // /MicroEvent/AddEvent/GetEventPrize?keyvalue=aa51beeb-e55c-4a85-b1bc-1395eaa65c28
        public ActionResult GetEventDataByType(string keyValue, string type)
        {
            string callbackFunc = Request.QueryString["callback"];
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();

            var data = eventApp.GetEventDataByType(keyValue,type);
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return Content(callbackFunc + "(" + jss.Serialize(new
            {
                data = data
            }) + ")");
        }
        
        [HttpPost]
        public ActionResult UploadImage(string keyvalue, string uploadType = "1", string pictureLink = "#")
        {
            PictureEntity pictureentity = new PictureEntity();
            pictureentity.F_Id = Common.GuId();
            if (keyvalue == "")
                keyvalue = pictureentity.F_Id;
            Image img = null;
            int towidth = 0;
            int toheight = 0;
            string result = "success";
            int maxFilesNumber = 5;
            if (uploadType == "4")
                maxFilesNumber = 20;
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase Files = Request.Files;//该集合是所有fileupload文件的集合。
                    List<string> listpictureids = new List<string>();
                    for (int i = 0; i < Files.Count; i++)
                    {
                        HttpPostedFileBase PostedFile = Files[i];
                        if (PostedFile.ContentLength > 0)
                        {
                            if(uploadType == "6")        //上传音频文件
                            {
                                string FileName = PostedFile.FileName;//文件名自行处理
                                string Dir = "Resource\\" + MappingDirName(uploadType) + "\\" + keyvalue + "\\";
                                if (!FileHelper.IsExistDirectory(Dir))
                                    FileHelper.CreateDir(Dir);

                                string savePath = Server.MapPath("/") + Dir + FileName;

                                //保存文件
                                PostedFile.SaveAs(savePath);

                                //写入Sys_Picture数据库
                                //PictureEntity pictureentity = new PictureEntity();
                                //pictureentity.F_Id = Common.GuId();
                                pictureentity.F_Type = "6";
                                pictureentity.F_EventId = keyvalue;
                                pictureentity.F_Link = "";
                                pictureentity.F_VirtualPath = "/" + (Dir + FileName).Replace("\\", "/");
                                pictureentity.F_VirtualPathSmall = "";
                                pictureentity.F_UploadDate = DateTime.Now;
                                pictureentity.F_CreatorTime = DateTime.Now;
                                pictureentity.F_CreatorUserId = OperatorProvider.Provider.GetCurrent().UserId;

                                pictureapp.SubmitForm(pictureentity, null);
                                new LogApp().WriteDbLog(new LogEntity
                                {
                                    F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.UploadFile上传背景音乐",
                                    F_Type = DbLogType.Update.ToString(),
                                    F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                                    F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                                    F_Result = true,
                                    F_Description = "上传了背景音乐: " + savePath,
                                });
                            }
                            else
                            {
                                string FileName = PostedFile.FileName;//文件名自行处理
                                                                      //大图文件夹
                                string Dir = "Resource\\Image\\" + MappingDirName(uploadType) + "\\" + keyvalue + "\\";
                                if (!FileHelper.IsExistDirectory(Dir))
                                    FileHelper.CreateDir(Dir);
                                //小图文件夹
                                string DirSmall = "Resource\\ImageSmall\\" + MappingDirName(uploadType) + "\\" + keyvalue + "\\";
                                if (!FileHelper.IsExistDirectory(DirSmall))
                                    FileHelper.CreateDir(DirSmall);

                                string savePath = Server.MapPath("/") + Dir + FileName;
                                string savePathSmall = Server.MapPath("/") + DirSmall + FileName;

                                #region 保存文件

                                //确保上传数量小于5
                                if (FileHelper.GetFileNames(Server.MapPath("/") + Dir).Length < maxFilesNumber)
                                {
                                    //保存大图文件
                                    PostedFile.SaveAs(savePath);
                                    new LogApp().WriteDbLog(new LogEntity
                                    {
                                        F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.UploadImage上传大图",
                                        F_Type = DbLogType.Update.ToString(),
                                        F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                                        F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                                        F_Result = true,
                                        F_Description = "上传了大图: " + savePath,
                                    });
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

                                    //写入Sys_Picture数据库
                                    pictureentity.F_Type = uploadType;
                                    if (uploadType == "1" || uploadType == "2" || uploadType == "3")
                                        pictureentity.F_EventId = keyvalue;
                                    else if (uploadType == "4")
                                        pictureentity.F_CandidateId = keyvalue;
                                    else if (uploadType == "5")
                                        pictureentity.F_GiftID = keyvalue;
                                    pictureentity.F_Link = pictureLink;
                                    pictureentity.F_VirtualPath = "/" + (Dir + FileName).Replace("\\", "/");
                                    pictureentity.F_VirtualPathSmall = "/" + (DirSmall + FileName).Replace("\\", "/");
                                    pictureentity.F_UploadDate = DateTime.Now;
                                    pictureentity.F_SmallSize = towidth.ToString() + "*" + toheight.ToString();
                                    pictureentity.F_CreatorTime = DateTime.Now;
                                    pictureentity.F_CreatorUserId = OperatorProvider.Provider.GetCurrent().UserId;

                                    pictureapp.SubmitForm(pictureentity, null);

                                    new LogApp().WriteDbLog(new LogEntity
                                    {
                                        F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.UploadImage上传小图",
                                        F_Type = DbLogType.Update.ToString(),
                                        F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                                        F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                                        F_Result = true,
                                        F_Description = "上传了小图: " + savePathSmall,
                                    });
                                    listpictureids.Add(pictureentity.F_Id);
                                }
                                else
                                    result = "full"; //满了5张
                            }
                        }
                    }
                    //写入Sys_Event数据库
                    //string ids = "";
                    //foreach(var id in listpictureids)
                    //{
                    //    ids += id + ",";
                    //}
                    //ids = ids.Substring(0, ids.Length - 1);
                    //EventEntity evententity = eventApp.GetForm(keyvalue);
                    //evententity.F_VoteCarouselIDs = ids;
                    //eventApp.SubmitForm(evententity, keyvalue,"0");
                }
            }
            catch (Exception ex)
            {
                result = "failed：" + ex.Message;
                new LogApp().WriteDbLog(new LogEntity
                {
                    F_ModuleName = "NFine.Web.Areas.MicroEvent.Controllers.UploadImage上传小图",
                    F_Type = DbLogType.Update.ToString(),
                    F_Account = OperatorProvider.Provider.GetCurrent().UserId,
                    F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                    F_Result = true,
                    F_Description = "上传失败" + ex.Message,
                });
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult DeleteImage(string key)
        {
            try
            {
                PictureEntity pictureentity = pictureapp.GetForm(key);
                if(key != "")
                {
                    pictureapp.DeleteForm(key);
                }
                string savePath = pictureentity.F_VirtualPath;
                string savePathSmall = pictureentity.F_VirtualPathSmall;
                FileHelper.DeleteFile(savePath);
                FileHelper.DeleteFile(savePathSmall);
                return Json("success");
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
        }
        
        [HttpPost]
        public ActionResult DeleteFile(string key)
        {
            try
            {
                PictureEntity pictureentity = pictureapp.GetForm(key);
                if (key != "")
                {
                    pictureapp.DeleteForm(key);
                }
                string savePath = pictureentity.F_VirtualPath;
                FileHelper.DeleteFile(savePath);
                return Json("success");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
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
                case "6":
                    DirName = "Song";
                    break;
                default:
                    break;
            }
            return DirName;
        }
        
        [HttpGet]
        [AllowAnonymous]
        // /MicroEvent/AddEvent/GetImageList?keyvalue=aa51beeb-e55c-4a85-b1bc-1395eaa65c28&uploadType=1
        public ActionResult GetImageList(string keyvalue, string uploadType)
        {
            string callbackFunc = Request.QueryString["callback"];
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();

            var data = pictureapp.GetImageList(keyvalue, uploadType);
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return Content(callbackFunc + "(" + jss.Serialize(new
            {
                data = data
            }) + ")");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        // /MicroEvent/AddEvent/GetImageUrl?keyvalue=aa51beeb-e55c-4a85-b1bc-1395eaa65c28&uploadType=1
        public ActionResult GetImageUrl(string keyvalue, string uploadType)
        {
            var data = pictureapp.GetImageUrl(keyvalue,uploadType);
            return Content(data.ToJson());
        }
    }
}
