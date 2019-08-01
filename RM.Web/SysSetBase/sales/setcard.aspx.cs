using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;
using System.Data;
using System.IO;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System.Text;
using RM.Common.DotNetBean;
using RM.Web.business;


namespace RM.Web.SysSetBase.sales
{
    public partial class setcard : System.Web.UI.Page
    {
        private static string imagepath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //加载服务选项
                if (Request.QueryString["AdminHotelid"] != null)
                {
                    //修改 
                    string sql = string.Format("select ID,LOGO,CentreLogo,Extension,HotelNameCode,AdminHotelid from Hotel_Admin where AdminHotelid='{0}'", Request["AdminHotelid"]);
                    DataTable dss = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                    if (dss != null && dss.Rows.Count > 0)
                    {
                        //酒店LOGO
                        if (dss.Rows[0]["LOGO"] != null && dss.Rows[0]["LOGO"].ToString() != "")
                        {
                            HotelLogo.Src = "../../upload/image/SN" + dss.Rows[0]["LOGO"];
                        }

                        if (dss.Rows[0]["HotelNameCode"] != null && dss.Rows[0]["HotelNameCode"].ToString() != "")
                        {
                            txtHotelNameCode.Value = lblHotelNameCode.Text = dss.Rows[0]["HotelNameCode"].ToString();
                        }

                        string AdminHotelid = Hdhoteladmin.Value = dss.Rows[0]["AdminHotelid"].ToString();
                        string Hotel_Code = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + TemplateMessage.Sen_Hotelimg("5@" + AdminHotelid, AdminHotelid, AdminHotelid); //生成永久二维码    
                        if (Hotel_Code.Length > 51) 
                        {
                            HotelCode.Src = Hotel_Code;
                        }

                        //推广图片
                        if (dss.Rows[0]["CentreLogo"] != null && dss.Rows[0]["CentreLogo"].ToString() != "")
                        {
                            imgPicture.Style.Add("display", "block");
                            imgFristPicture.Style.Add("display", "block");
                            imgPicture.Src = "../../upload/image/SN" + dss.Rows[0]["CentreLogo"];
                            imgFristPicture.Src = "../../upload/image/SN" + dss.Rows[0]["CentreLogo"];
                            hfImage.Value = imagepath = dss.Rows[0]["CentreLogo"].ToString();
                            photo.Style.Add("display", "");
                        }
                        else
                        {
                            photo.Style.Add("display", "none");
                        }

                        //推广说明
                        if (dss.Rows[0]["Extension"] != null && dss.Rows[0]["Extension"].ToString() != "")
                        {
                            txtExtension.Value = dss.Rows[0]["Extension"].ToString();
                            lblAdvertising.InnerHtml = dss.Rows[0]["Extension"].ToString();
                              
                        }

                    }

                }
          
            }
        }





        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumit_Click(object sender, EventArgs e)
        {
            Hashtable hs = new Hashtable();
            //获取图片路径
            if (hfImage.Value != imagepath)
            {
                string filename = fuPicture.FileName;
                if (!string.IsNullOrEmpty(filename))
                {
                    string type = (filename.Substring(filename.LastIndexOf(".") + 1)).ToLower(); //得到文件的后缀
                    if (type == "jpg" || type == "gif" || type == "bmp" || type == "png")
                    {
                        string name1 = Guid.NewGuid().ToString();
                        string name = name1.ToString() + "." + type;
                        string fpath = HttpContext.Current.Server.MapPath("~\\upload\\image\\") + name;
                        fuPicture.SaveAs(fpath); //将文件保存到fpath这个路径里面

                        CommonMethod.CreateThumbnail(fpath, HttpContext.Current.Server.MapPath("~\\upload\\image\\") + "SN" + name, 160, 160, false);

                        //创建缩略图
                        hs["CentreLogo"] = name;
                        //原记录中有图片信息。
                        if (!String.IsNullOrEmpty(imagepath))
                        {
                            //删除原来的旧图片。
                            FileInfo file;
                            file = new FileInfo(Server.MapPath("..") + "\\..\\upload\\image\\" + imagepath);
                            file.Delete();

                            file = new FileInfo(Server.MapPath("..") + "\\..\\upload\\image\\" + "SN" + imagepath);
                            file.Delete();
                        }
                    }
                    else
                    {
                        CommonMethod.Alert(this.Page, "不支持该格式的图标！支持jpg、gif、bmp、png");
                        return;
                    }
                }
            }

            hs["Extension"] = this.txtExtension.Value;
            hs["HotelNameCode"] = this.txtHotelNameCode.Value;

            if (Request.QueryString["AdminHotelid"] != null)
            {
                //修改
                if (DataFactory.SqlDataBase().UpdateByHashtable("Hotel_Admin", "AdminHotelid", Hdhoteladmin.Value, hs) > 0)
                {
                    ShowMsgHelper.Alert("编辑成功！");


                    Response.Redirect("setcard.aspx?AdminHotelid=" + Hdhoteladmin.Value + "", false); 
           
                }
                else
                {
            
                    ShowMsgHelper.Alert_Error("编辑失败！");
               
                }
            }
     
        }

    }
}