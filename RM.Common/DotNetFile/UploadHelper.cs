using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace RM.Common.DotNetFile
{
    /// <summary>
    /// 文件上传帮助类
    /// </summary>
    public class UploadHelper
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="filleupload">上传文件控件</param>
        /// <returns></returns>
        public static string FileUpload(string path, FileUpload filleupload)
        {
            try
            {
                bool fileOk = false;
                //取得文件的扩展名,并转换成小写
                string fileExtension = System.IO.Path.GetExtension(filleupload.FileName).ToLower();
                //文件格式
                string[] allowExtension = { ".xls", ".doc", ".docx", ".rar", ".zip", ".rar", ".ios", ".jpg", ".png", "bmp", ".gif", ".txt" };
                if (filleupload.HasFile)
                {
                    //对上传的文件的类型进行一个个匹对
                    for (int i = 0; i < allowExtension.Length; i++)
                    {
                        if (fileExtension == allowExtension[i])
                        {
                            fileOk = true;
                            break;
                        }
                    }
                }
                //如果符合条件，则上传
                if (fileOk)
                {
                    //if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
                    //{
                    //    Directory.CreateDirectory(path);
                    //}
                    if (!FileHelper.IsExistFile(path + filleupload.FileName))
                    {
                        int Size = filleupload.PostedFile.ContentLength / 1024 / 1024;
                        if (Size > 10)
                        {
                            return "上传失败,文件过大";
                        }
                        else
                        {
                            string fpath = HttpContext.Current.Server.MapPath(path) + filleupload.FileName;
                            filleupload.PostedFile.SaveAs(fpath);
                            return "上传成功";
                        }
                    }
                    else
                    {
                        return "上传失败,文件已存在";
                    }

                }
                else
                {
                    return "不支持【" + fileExtension + "】文件格式";
                }
            }
            catch (Exception)
            {
                return "上传失败";
            }
        }


        /// <summary>
        /// 文件上传(导入)
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="filleupload">上传文件控件</param>
        /// <returns></returns>
        public static string Inport(FileUpload filleupload, out string fullPath)
        {
            fullPath = "";
            try
            {
                bool fileOk = false;
                //取得文件的扩展名,并转换成小写
                string fileExtension = System.IO.Path.GetExtension(filleupload.FileName).ToLower();
                //文件格式
                string[] allowExtension = { ".xls", ".xlsx" };
                if (filleupload.HasFile)
                {
                    //对上传的文件的类型进行一个个匹对
                    for (int i = 0; i < allowExtension.Length; i++)
                    {
                        if (fileExtension == allowExtension[i])
                        {
                            fileOk = true;
                            break;
                        }
                    }
                }
                if (fileOk)
                {
                    int Size = filleupload.PostedFile.ContentLength / 1024 / 1024;
                    if (Size > 10)
                    {
                        return "上传失败,文件过大";
                    }
                    else
                    {
                        string fpath = fullPath = HttpContext.Current.Server.MapPath("~\\Themes\\Upload\\") + "Inport" + fileExtension;
                        filleupload.PostedFile.SaveAs(fpath);
                        return "上传成功";
                    }

                }
                else
                {
                    return "不支持【" + fileExtension + "】文件格式";
                }
            }
            catch (Exception)
            {
                return "上传失败";
            }
        }

        /// <summary>
        /// 上传单张图片
        /// </summary>
        /// <param name="fu"></param>
        /// <param name="savePath"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool UploadImg(FileUpload fu, string savePath, out string name)
        {
            //上传主题图
            name = "";
            bool sta = false;
            string filename = fu.FileName;
            string type = (filename.Substring(filename.LastIndexOf(".") + 1)).ToLower(); //得到文件的后缀
            if (type == "jpg" || type == "gif" || type == "bmp" || type == "png")
            {
                string name1 = Guid.NewGuid().ToString();
                name = name1.ToString() + "." + type;
                string fpath = HttpContext.Current.Server.MapPath("~\\Themes\\Upload\\Images\\" + savePath + "\\") + name;
                fu.SaveAs(fpath); //将文件保存到fpath这个路径里面
                sta = true;
            }

            return sta;
        }

        /// <summary>
        /// 上传图片函数
        /// </summary>
        /// <param name="fuPic"></param>
        /// <returns></returns>
        public static string uploadPicture(FileUpload fuPic)
        {
            string name = "";
            string fileName = fuPic.FileName;
            if (!string.IsNullOrEmpty(fileName))
            {
                string type = (fileName.Substring(fileName.LastIndexOf(".") + 1)).ToLower();
                if (type == "jpg" || type == "gif" || type == "bmp" || type == "png")
                {
                    name = Guid.NewGuid().ToString();
                    name += "." + type;
                    string fpath = HttpContext.Current.Server.MapPath("~\\Themes\\Upload\\Images\\") + name;
                    fuPic.SaveAs(fpath);

                }
                else
                {
                    return "不支持该格式的图片！支持jpg、gif、bmp、png";
                }
            }
            return name;
        }

        /// <summary>
        /// 任意角度旋转
        /// </summary>
        /// <param name="bmp">原始图Bitmap</param>
        /// <param name="angle">旋转角度</param>
        /// <param name="bkColor">背景色</param>
        /// <returns>输出Bitmap</returns>
        public static Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 2;
            int h = bmp.Height + 2;
            PixelFormat pf;
            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }
            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);
            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();
            tmp.Dispose();
            return dst;
        }


    }
}
