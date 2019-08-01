using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using SQL;
using RM.Common.DotNetCode;

namespace RM.Web.Ajax
{
    /// <summary>
    /// UploadImg 的摘要说明
    /// </summary>
    public class UploadImg : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Action = context.Request["action"];                      //提交动作
            switch (Action)
            {
                case "UploadImg"://上传图片
                    string img_path = SaveImg(context);
                    context.Response.Write(img_path);
                    context.Response.End();
                    break;
                default:
                    break;
            }

            string type1 = context.Request["type"];
            string na = context.Request["name"];
            string SavePath = context.Request["path"];
            HttpFileCollection files = context.Request.Files;
            HttpPostedFile hpf = files["fileElem"];
            string ruen = "";

            if (type1.ToString() == "add")
            {
                string filename = hpf.FileName;
                string type = (filename.Substring(filename.LastIndexOf(".") + 1)).ToLower(); //得到文件的后缀
                if (type == "jpg" || type == "gif" || type == "bmp" || type == "png")
                {
                    string name1 = Guid.NewGuid().ToString();
                    string name = name1.ToString() + "." + type;
                    string fpath = HttpContext.Current.Server.MapPath("~\\upload\\" + SavePath + "\\") + name;
                    hpf.SaveAs(fpath);
                    ruen = name;
                }
                else
                {
                    ruen = "0";
                }
            }
            else
            {
                //删除
                FileInfo file;
                file = new FileInfo(HttpContext.Current.Server.MapPath("..") + "\\upload\\" + SavePath + "\\" + na);
                file.Delete();
                ruen = "1";
            }
            context.Response.Write(ruen);
        }
        private string SaveImg(HttpContext context)
        {
            string img_path = "";
            HttpPostedFile file = context.Request.Files["files"];
            string thumbnail = context.Request["thumbnail"];  //是否生成缩略图
            int thumbWidth = CommonHelper.GetInt(context.Request["width"]);  //缩略图 宽度
            int thumbHeight = CommonHelper.GetInt(context.Request["heigth"]);  //缩略图 高度
            string filename = file.FileName;
            string type = (filename.Substring(filename.LastIndexOf(".") + 1)).ToLower(); //得到文件的后缀
            if (type == "jpg" || type == "gif" || type == "bmp" || type == "png")
            {
                string name = Guid.NewGuid().ToString() + "." + type;
                string path = "/upload/image/" + DateTime.Now.ToString("yyyyMMdd") + "/";
                string fpath = HttpContext.Current.Server.MapPath(path);
                if (!System.IO.Directory.Exists(fpath))//如果不存在就创建file文件夹
                {
                    System.IO.Directory.CreateDirectory(fpath);
                }
                file.SaveAs(fpath + name);
                if (thumbnail == "true" && thumbWidth > 0 && thumbHeight > 0)
                {
                    CommonMethod.CreateThumbnail(fpath + name, fpath + "SN" + name, thumbWidth, thumbHeight, false);
                }

                img_path = path + name;
            }
            return img_path;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}