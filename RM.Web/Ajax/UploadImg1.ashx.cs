using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using RM.Common.DotNetCode;

namespace RM.Web.Ajax
{
    /// <summary>
    /// UploadImg1 的摘要说明
    /// </summary>
    public class UploadImg1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                string type1 = context.Request["type"];
                string na = context.Request["name"];
                string SavePath = context.Request["path"];
                int optimize = CommonHelper.GetInt(context.Request["optimize"]);
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
                        string sn_path = HttpContext.Current.Server.MapPath("~\\upload\\" + SavePath + "\\") + "SN" + name;

                        CommonMethod.CreateThumbnail(fpath, sn_path, 680, 510, false);

                        int ImgSize = hpf.ContentLength / 1024;//此处取得的文件大小的单位是byte
                        if (ImgSize > 800 && optimize == 1)//转换为kb 图片大于800KB
                        {
                            CommonMethod.CreateThumbnail(sn_path, HttpContext.Current.Server.MapPath("~\\upload\\" + SavePath + "\\") + name, 680, 510, false);
                        }
                        ruen = name;
                    }
                }
                else
                {//删除
                    FileInfo file;
                    file = new FileInfo(HttpContext.Current.Server.MapPath("..") + "\\upload\\" + SavePath + "\\" + na);
                    file.Delete();

                    file = new FileInfo(HttpContext.Current.Server.MapPath("..") + "\\upload\\" + SavePath + "\\" + "SN" + na);
                    file.Delete();
                    ruen = "1";
                }
                context.Response.Write(ruen);
            }
            catch (Exception)
            {
                context.Response.Write("0");
            }
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