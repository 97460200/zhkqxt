using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.App_Code;
using RM.Busines;
using RM.Common.DotNetBean;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;

using RM.Web.business;
using RM.Web.Lib;
using System.Collections;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using RM.Common.DotNetFile;


namespace RM.Web.RMBase.SysSetBase.sales
{
    public partial class MemberList : PageBase
    {
        public String hotelTreeHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //树形菜单
                Hdhoteladmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();

                //会员级别
                DataTable dtVip = CommonMethod.GetVip(Hdhoteladmin.Value);
                ddlSearch.DataSource = dtVip;
                ddlSearch.DataTextField = "LevelName";
                ddlSearch.DataValueField = "code";
                ddlSearch.DataBind();
                ListItem li = new ListItem("全部", "0");
                ddlSearch.Items.Insert(0, li);


                bool blHotelTree = false;//是否有多分店权限 多店显示酒店树
                string HotelId = "";//如果只有一家店 默认的酒店ID

                if (RequestSession.GetSessionUser().Hotelid.ToString() == "0")
                {
                    hotelTreeHtml = HotelTreeHelper.HotelTree(Hdhoteladmin.Value, 1, out blHotelTree, out HotelId);
                }
                else
                {
                    blHotelTree = false;
                    HotelId = RequestSession.GetSessionUser().Hotelid.ToString();
                }
                HotelTree.Visible = blHotelTree;
                htHotelTree.Value = blHotelTree.ToString();
                hdHotelId.Value = HotelId;
            }
        }



        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumit_Click(object sender, EventArgs e)
        {

            StringBuilder sql = new StringBuilder();
            List<SqlParam> ilistStr = new List<SqlParam>();
            sql.Append(@"SELECT  * FROM  V_Base_UserInfoRoles WHERE   1 = 1  and IsAdmin = '2'  and AdminHotelid = @AdminHotelid   and DeleteMark = '1' and User_Account!='sewa' ");
            ilistStr.Add(new SqlParam("@AdminHotelid", Hdhoteladmin.Value));
            if (hdHotelId.Value != "0" && hdHotelId.Value != "-1")
            {
                sql.Append(" AND hotelid = @hotelid");
                ilistStr.Add(new SqlParam("@hotelid", hdHotelId.Value));
            }
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, ilistStr.ToArray());
            //查询生成当前酒店用户二维码
            if (dt != null && dt.Rows.Count > 0)
            {
                List<string> listFJ = new List<string>();//保存附件路径
                List<string> listFJName = new List<string>();//保存附件名字
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string wjmc = dt.Rows[i]["User_Name"].ToString() + " - " + dt.Rows[i]["HotelNames"].ToString();
                    string url = Request.Url.ToString();
                    url = url.Substring(0, url.LastIndexOf("/"));
                    // <param name="Url">网页地址</param>
                    //<param name="BrowserWidth">页面宽度</param>
                    // <param name="BrowserHeight">页面高度</param>
                    // <param name="ThumbnailWidth">图像宽度</param>
                    //<param name="ThumbnailHeight">图像高度</param>
                    Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(url + "/code_imgnew03.aspx?AdminHotelid=" + dt.Rows[i]["AdminHotelid"] + "&User_ID=" + dt.Rows[i]["User_ID"], 540, 540, 540, 540); //宽高根据要获取快照的网页决定
                    string filename = "~/Upload/QRcode/" + wjmc + ".jpg";
                    string fpath = HttpContext.Current.Server.MapPath(filename);
                    m_Bitmap.Save(fpath, System.Drawing.Imaging.ImageFormat.Jpeg); //图片格式可以自由控制
                    try
                    {
                        listFJ.Add(fpath);
                        listFJName.Add(wjmc + ".jpg");
                    }
                    catch
                    {

                    }
                }
                string time = DateTime.Now.Ticks.ToString();
                ZipFileMain(listFJ.ToArray(), listFJName.ToArray(), Server.MapPath("~\\upload\\QRcode\\" + time + ".zip"), 6);//压缩文件
                string HotelName = "(" + dt.Rows[0]["HotelName"].ToString() + ")" + "员工推广码.zip";
                DownloadFile(Server.UrlEncode(HotelName), Server.MapPath("~\\upload\\QRcode\\" + time + ".zip"));

            }
        }




                /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumits_Click(object sender, EventArgs e)
        {

            StringBuilder sql = new StringBuilder();
            List<SqlParam> ilistStr = new List<SqlParam>();
            sql.Append(@"SELECT  * FROM  V_Base_UserInfoRoles WHERE   1 = 1  and IsAdmin = '2'  and AdminHotelid = @AdminHotelid   and DeleteMark = '1' and User_Account!='sewa' ");
            ilistStr.Add(new SqlParam("@AdminHotelid", Hdhoteladmin.Value));
            if (hdHotelId.Value != "0" && hdHotelId.Value != "-1")
            {
                sql.Append(" AND hotelid = @hotelid");
                ilistStr.Add(new SqlParam("@hotelid", hdHotelId.Value));
            }
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, ilistStr.ToArray());

            //查询生成当前酒店用户二维码
            if (dt != null && dt.Rows.Count > 0)
            {
                List<string> listFJ = new List<string>();//保存附件路径
                List<string> listFJName = new List<string>();//保存附件名字
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string wjmc = dt.Rows[i]["User_Name"].ToString() + " - " + dt.Rows[i]["HotelNames"].ToString();
                    string url = Request.Url.ToString();
                    url = url.Substring(0, url.LastIndexOf("/"));
                    // <param name="Url">网页地址</param>
                    //<param name="BrowserWidth">页面宽度</param>
                    // <param name="BrowserHeight">页面高度</param>
                    // <param name="ThumbnailWidth">图像宽度</param>
                    //<param name="ThumbnailHeight">图像高度</param>
                    Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(url + "/code_imgnew04.aspx?AdminHotelid=" + dt.Rows[i]["AdminHotelid"] + "&User_ID=" + dt.Rows[i]["User_ID"], 595, 842, 595, 842); //宽高根据要获取快照的网页决定
                    string filename = "~/Upload/QRcode/" + wjmc + ".jpg";
                    string fpath = HttpContext.Current.Server.MapPath(filename);
                    m_Bitmap.Save(fpath, System.Drawing.Imaging.ImageFormat.Jpeg); //图片格式可以自由控制
                    try
                    {
                        listFJ.Add(fpath);
                        listFJName.Add(wjmc + ".jpg");
                    }
                    catch
                    {

                    }
                }
                string time = DateTime.Now.Ticks.ToString();
                ZipFileMain(listFJ.ToArray(), listFJName.ToArray(), Server.MapPath("~\\upload\\QRcode\\" + time + ".zip"), 6);//压缩文件
                string HotelName = "(" + dt.Rows[0]["HotelName"].ToString() + ")" + "展牌.zip";
                DownloadFile(Server.UrlEncode(HotelName), Server.MapPath("~\\upload\\QRcode\\" + time + ".zip"));

            }
        
        }


        private void DownloadFile(string fileName, string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                File.Delete(filePath);//删除已下载文件
                Response.End();
            }
            catch
            {

            }
        }



        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileName">要压缩的所有文件（完全路径)</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="name">压缩后文件路径</param>
        /// <param name="Level">压缩级别</param>
        public void ZipFileMain(string[] filenames, string[] fileName, string name, int Level)
        {
            ZipOutputStream s = new ZipOutputStream(File.Create(name));
            Crc32 crc = new Crc32();
            //压缩级别
            s.SetLevel(Level); // 0 - store only to 9 - means best compression
            try
            {
                int m = 0;
                foreach (string file in filenames)
                {
                    //打开压缩文件
                    FileStream fs = File.OpenRead(file);//文件地址
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    //建立压缩实体
                    ZipEntry entry = new ZipEntry(fileName[m].ToString());//原文件名
                    //时间
                    entry.DateTime = DateTime.Now;
                    //空间大小
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);
                    s.Write(buffer, 0, buffer.Length);
                    m++;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                s.Finish();
                s.Close();
            }
        }



        #region 生成图片程序
        public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放    
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量    
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量    
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }


        public class ImageUtility
        {
            #region 合并用户QR图片和用户头像
            /// <summary>   
            /// 合并用户QR图片和用户头像   
            /// </summary>   
            /// <param name="qrImg">QR图片</param>   
            /// <param name="headerImg">用户头像</param>   
            /// <param name="n">缩放比例</param>   
            /// <returns></returns>   
            public Bitmap MergeQrImg(Bitmap qrImg, Bitmap headerImg, double n = 0.23)
            {
                int margin = 10;
                float dpix = qrImg.HorizontalResolution;
                float dpiy = qrImg.VerticalResolution;
                var _newWidth = (10 * qrImg.Width - 46 * margin) * 1.0f / 46;
                var _headerImg = ZoomPic(headerImg, _newWidth / headerImg.Width);
                //处理头像   
                int newImgWidth = _headerImg.Width + margin;
                Bitmap headerBgImg = new Bitmap(newImgWidth, newImgWidth);
                headerBgImg.MakeTransparent();
                Graphics g = Graphics.FromImage(headerBgImg);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                Pen p = new Pen(new SolidBrush(Color.White));
                //位置和大小
                Rectangle rect = new Rectangle(0, 0, newImgWidth - 1, newImgWidth - 1);
                using (GraphicsPath path = CreateRoundedRectanglePath(rect, 7))
                {
                    g.DrawPath(p, path);
                    g.FillPath(new SolidBrush(Color.White), path);
                }
                //画头像   
                Bitmap img1 = new Bitmap(_headerImg.Width, _headerImg.Width);
                Graphics g1 = Graphics.FromImage(img1);
                g1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g1.Clear(Color.Transparent);
                //Pen p1 = new Pen(new SolidBrush(Color.Gray));
                Pen p1 = new Pen(new SolidBrush(Color.White));
                Rectangle rect1 = new Rectangle(0, 0, _headerImg.Width - 1, _headerImg.Width - 1);
                using (GraphicsPath path1 = CreateRoundedRectanglePath(rect1, 8))
                {
                    g1.DrawPath(p1, path1);
                    TextureBrush brush = new TextureBrush(_headerImg);
                    g1.FillPath(brush, path1);
                }
                g1.Dispose();
                PointF center = new PointF((newImgWidth - _headerImg.Width) / 2, (newImgWidth - _headerImg.Height) / 2);
                g.DrawImage(img1, center.X, center.Y, _headerImg.Width, _headerImg.Height);
                g.Dispose();
                Bitmap backgroudImg = new Bitmap(qrImg.Width, qrImg.Height);
                backgroudImg.MakeTransparent();
                backgroudImg.SetResolution(dpix, dpiy);
                headerBgImg.SetResolution(dpix, dpiy);
                Graphics g2 = Graphics.FromImage(backgroudImg);
                g2.Clear(Color.Transparent);
                g2.DrawImage(qrImg, 0, 0);
                PointF center2 = new PointF((qrImg.Width - headerBgImg.Width) / 2, (qrImg.Height - headerBgImg.Height) / 2);
                g2.DrawImage(headerBgImg, center2);
                g2.Dispose();
                return backgroudImg;
            }
            #endregion

            #region 图形处理
            /// <summary>   
            /// 创建圆角矩形   
            /// </summary>   
            /// <param name="rect">区域</param>   
            /// <param name="cornerRadius">圆角角度</param>   
            /// <returns></returns>   
            private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
            {
                //下午重新整理下，圆角矩形   
                GraphicsPath roundedRect = new GraphicsPath();
                roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
                roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
                roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
                roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
                roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
                roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
                roundedRect.CloseFigure();
                return roundedRect;
            }
            /// <summary>   
            /// 图片按比例缩放   
            /// </summary>   
            private System.Drawing.Image ZoomPic(System.Drawing.Image initImage, double n)
            {
                //缩略图宽、高计算   
                double newWidth = initImage.Width;
                double newHeight = initImage.Height;
                newWidth = n * initImage.Width;
                newHeight = n * initImage.Height;
                //生成新图   
                //新建一个bmp图片   
                System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);
                //新建一个画板   
                System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);
                //设置质量   
                newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //置背景色   
                newG.Clear(Color.Transparent);
                //画图   
                newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                newG.Dispose();
                return newImage;
            }
            #endregion
        }
        #endregion






    }
}