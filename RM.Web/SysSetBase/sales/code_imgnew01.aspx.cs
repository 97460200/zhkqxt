using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Busines;
using System.Text;
using System.Data;
using System.Collections;
using RM.Web.business;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;
using System.Drawing;
using RM.Web.App_Code;
using RM.Common.DotNetFile;


using RM.Web.business;
using RM.Web.Lib;
using RM.Busines;
using System.Text;
using System.Collections;
using RM.Web.App_Code;
using RM.Common.DotNetCode;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
namespace RM.Web.SysSetBase.sales
{
    public partial class code_imgnew01 : System.Web.UI.Page
    {
        public string _img_codes = "";
        public string _code_imgnew05 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = "";

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    id = Request["id"].ToString();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select User_ID,code_img,User_Name,AdminHotelid from Base_UserInfo where User_ID=@User_ID and DeleteMark = 1");
                    SqlParam[] param = new SqlParam[] { new SqlParam("@User_ID", id) };
                    DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                    if (ds != null && ds.Rows.Count > 0)
                    {
                        Span_Name.InnerText  = Span_Names.InnerText= ds.Rows[0]["User_Name"].ToString();
                        Hdhoteladmin.Value = ds.Rows[0]["AdminHotelid"].ToString();
                        HdUser_ID.Value = ds.Rows[0]["User_ID"].ToString();
                        if (ds.Rows[0]["code_img"] != null && ds.Rows[0]["code_img"].ToString() != "" && ds.Rows[0]["code_img"].ToString() != "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=")
                        {
                            First_code.Src = ds.Rows[0]["code_img"].ToString();
                            Second_code.Src = ds.Rows[0]["code_img"].ToString();
                        }
                        else
                        {
                            Hashtable hs = new Hashtable();
                            string img_name = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + TemplateMessage.Sen_img("3@" + id, id); //生成永久二维码      
                            First_code.Src = img_name;
                            Second_code.Src = img_name;
                            hs["code_img"] = img_name;
                            DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", id, hs);
                        }
                        _code_imgnew05 = "code_imgnew05.aspx?AdminHotelid=" + Hdhoteladmin.Value + "&User_ID=" + HdUser_ID.Value + "";
                        //修改 
                        string sql = string.Format("select ID,Name,LOGO,CentreLogo,Extension,HotelNameCode,AdminHotelid from Hotel_Admin where AdminHotelid='{0}'", ds.Rows[0]["AdminHotelid"].ToString());
                        DataTable dss = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                        if (dss != null && dss.Rows.Count > 0)
                        {
                            //酒店LOGO
                            if (dss.Rows[0]["LOGO"] != null && dss.Rows[0]["LOGO"].ToString() != "")
                            {
                                HotelLogo.Src = "../../upload/image/SN" + dss.Rows[0]["LOGO"];
                            }

                            //酒店名称
                            if (dss.Rows[0]["Name"] != null && dss.Rows[0]["Name"].ToString() != "")
                            {
                                HdHotelName.Value = dss.Rows[0]["Name"].ToString();
                            }

                            //酒店名称
                            if (dss.Rows[0]["HotelNameCode"] != null && dss.Rows[0]["HotelNameCode"].ToString() != "")
                            {
                                lblHotelNameCode.Text = dss.Rows[0]["HotelNameCode"].ToString();
                            }

                            //酒店二维码LOGO
                            if (dss.Rows[0]["CentreLogo"] != null && dss.Rows[0]["CentreLogo"].ToString() != "")
                            {
                                First_codes.Src = "../../upload/image/" + dss.Rows[0]["CentreLogo"];
                                Second_codes.Src = "../../upload/image/" + dss.Rows[0]["CentreLogo"];
                            }

                            //推广说明
                            if (dss.Rows[0]["Extension"] != null && dss.Rows[0]["Extension"].ToString() != "")
                            {
                         
                                lblAdvertising.InnerHtml = dss.Rows[0]["Extension"].ToString();

                            }

                        }


                    }
                
                }
            }
        }

        
        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumit_Click(object sender, EventArgs e)
        {
            string url = Request.Url.ToString();
            url = url.Substring(0, url.LastIndexOf("/"));
            // <param name="Url">网页地址</param>
            //<param name="BrowserWidth">页面宽度</param>
            // <param name="BrowserHeight">页面高度</param>
            // <param name="ThumbnailWidth">图像宽度</param>
            //<param name="ThumbnailHeight">图像高度</param>
            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(url + "/code_imgnew03.aspx?AdminHotelid=" + Hdhoteladmin.Value + "&User_ID=" + HdUser_ID.Value + "", 540, 540, 540, 540); //宽高根据要获取快照的网页决定
            string PhotoName = Span_Name.InnerText + "-" + HdHotelName.Value + "员工推广码.jpg";
            string filename = "~/Upload/QRcode/" + PhotoName;
            string fpath = HttpContext.Current.Server.MapPath(filename);
            m_Bitmap.Save(fpath, System.Drawing.Imaging.ImageFormat.Jpeg); //图片格式可以自由控制
            DownloadFile(Server.UrlEncode(PhotoName), Server.MapPath(filename));
        }

                
        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumits_Click(object sender, EventArgs e)
        {
            string url = Request.Url.ToString();
            url = url.Substring(0, url.LastIndexOf("/"));
            // <param name="Url">网页地址</param>
            //<param name="BrowserWidth">页面宽度</param>
            // <param name="BrowserHeight">页面高度</param>
            // <param name="ThumbnailWidth">图像宽度</param>
            //<param name="ThumbnailHeight">图像高度</param>
            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(url + "/code_imgnew04.aspx?AdminHotelid=" + Hdhoteladmin.Value + "&User_ID=" + HdUser_ID.Value + "", 595, 842, 595, 842); //宽高根据要获取快照的网页决定
            string PhotoName = Span_Name.InnerText + "-" + HdHotelName.Value + "展牌.jpg";
            string filename = "~/Upload/QRcode/" + PhotoName;
            string fpath = HttpContext.Current.Server.MapPath(filename);
            m_Bitmap.Save(fpath, System.Drawing.Imaging.ImageFormat.Jpeg); //图片格式可以自由控制
            DownloadFile(Server.UrlEncode(PhotoName), Server.MapPath(filename));
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
                return;
            }
            catch
            {

            }
        }

    }
}