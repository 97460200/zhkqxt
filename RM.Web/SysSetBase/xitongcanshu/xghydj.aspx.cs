using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;
using RM.Common.DotNetUI;
using System.Text;
using System.Data;
using RM.Busines;
using System.Collections;
using System.IO;
using RM.Web.App_Code;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.xitongcanshu
{
    public partial class xghydj : System.Web.UI.Page
    {
        private static string imagepath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["ID"] != null)
                {
                    hid.Value = Request["ID"].ToString();
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("SELECT  hylxcode ,  hylxname FROM hy_hylxbmb ");
                DataTable dt = DataFactory.SqlDataBase(Request["AdminHotelid"]).GetDataTableBySQL(sb);
                if (dt != null && dt.Rows.Count > 0) 
                {
                    if (hid.Value == "")
                    {
                        this.DDLgghy.Items.Add(new ListItem("公众号会员等级", "W"));
                    }
                    for (int i = 0; i<dt.Rows.Count;i++ )
                    {
                        this.DDLgghy.Items.Add(new ListItem(dt.Rows[i]["hylxname"].ToString(), dt.Rows[i]["hylxcode"].ToString().Trim()));
                    }
                }
                bind();
            }
        }

        void bind()
        {
            if (Request["ID"] != null)
            {
                string sql = string.Format(@"SELECT  hylxcode ,  hylxname , zk ,  bz ,jb , jfbz , mrscm , wx_url,wx_sm FROM dbo.hy_hylxbmb  where id=@id and AdminHotelid=@AdminHotelid ");
                SqlParam[] parmAdd2 = new SqlParam[] { 
                                     new SqlParam("@id",Request["id"]),
                new SqlParam("@AdminHotelid",Request["AdminHotelid"])};
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd2);
                if (ds != null && ds.Rows.Count > 0)
                {
                    hyname.Text = ds.Rows[0]["hylxname"].ToString();
                    hykje.Text = ds.Rows[0]["zk"].ToString().Split('.')[0];
                    sm.InnerText = ds.Rows[0]["wx_sm"].ToString();
                    DDLgghy.SelectedValue = ds.Rows[0]["hylxcode"].ToString().Trim();
                    hyjb.SelectedValue = ds.Rows[0]["jb"].ToString().Trim();
                    if (ds.Rows[0]["wx_url"].ToString() != "")
                    {
                        imgPicture.Src = "/upload/image/" + ds.Rows[0]["wx_url"].ToString();
                        imgPicture1.Src = "/upload/image/" + ds.Rows[0]["wx_url"].ToString();
                        hfImage.Value = ds.Rows[0]["wx_url"].ToString();
                        imgPicture.Style.Add("display", "block");
                    }
                    else
                    {
                        imgPicture.Src = "";
                        imgPicture.Style.Add("display", "none");
                    }
                }

            }
            else
            {
                btnSubmit.Text = "保存";
            }
        }
        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            string filename = "";
            if (hfImage.Value != imagepath)
            {
                filename = fuPicture.FileName;
                if (!string.IsNullOrEmpty(filename))
                {
                    string type = (filename.Substring(filename.LastIndexOf(".") + 1)).ToLower(); //得到文件的后缀
                    if (type == "jpg" || type == "gif" || type == "bmp" || type == "png")
                    {
                        string name1 = Guid.NewGuid().ToString();
                        string name = name1.ToString() + "." + type;
                        string fpath = HttpContext.Current.Server.MapPath("~\\upload\\image\\") + name;
                        fuPicture.SaveAs(fpath); //将文件保存到fpath这个路径里面
                        filename = name;
                        //原记录中有图片信息。
                        if (!String.IsNullOrEmpty(imagepath))
                        {
                            //删除原来的旧图片。
                            FileInfo file;
                            file = new FileInfo(Server.MapPath("..") + "\\..\\upload\\image\\" + imagepath);
                            file.Delete();
                        }
                    }
                    else
                    {
                        ShowMsgHelper.Alert_Error("不支持该格式的图标！支持jpg、gif、bmp、png！");
                        return;
                    }
                }
            }

            ht["hylxcode"] = DDLgghy.SelectedValue.Trim();
            ht["hylxname"] = hyname.Text;
            //ht["zk"] = hykje.Text == "" ? "0" : hykje.Text;
            ht["wx_url"] = hfImage.Value;
            ht["wx_sm"] = sm.InnerText;
            ht["jb"] = hyjb.SelectedValue;
            ht["sort"] = hyjb.SelectedValue;
            ht["AdminHotelid"] = Request["AdminHotelid"];

            if (DDLgghy.SelectedValue == "W")
            {
               int a = DataFactory.SqlDataBase().InsertByHashtable("hy_hylxbmb", ht);
               if (a > 0)
               {
                   Hashtable hts = new Hashtable();
                   hts["hylxcode"] = DDLgghy.SelectedValue;
                   hts["hylxname"] = hyname.Text;
                   hts["mrscm"] = "WXVIP";
                   hts["AdminHotelid"] = Request["AdminHotelid"];
                   hts["jb"] = hyjb.SelectedValue;
                   hts["sort"] = hyjb.SelectedValue;
                   int b = DataFactory.SqlDataBase(Request["AdminHotelid"]).InsertByHashtable("hy_hylxbmb", hts);
                   if (b > 0)
                   {
                       ShowMsgHelper.OpenClose("添加成功！");
                   }
                   else 
                   {
                       ShowMsgHelper.Alert_Error("同步至国光失败！");
                   }
               }
               else 
               {
                   ShowMsgHelper.Alert_Error("添加失败！");
               }
            }
            else 
            {
                bool val = DataFactory.SqlDataBase().Submit_AddOrEdit("hy_hylxbmb", "ID", hid.Value, ht);
                if (val)
                {                   
                    Hashtable hts = new Hashtable();
                    hts["AdminHotelid"] = Request["AdminHotelid"];
                    hts["jb"] = hyjb.SelectedValue;
                    hts["sort"] = hyjb.SelectedValue;
                    bool vals = DataFactory.SqlDataBase(Request["AdminHotelid"]).Submit_AddOrEdit("hy_hylxbmb", "hylxcode", DDLgghy.SelectedValue, hts);
                    if (vals)
                    {
                        ShowMsgHelper.OpenClose("编辑成功！");
                    }
                    else
                    {
                        ShowMsgHelper.Alert_Error("同步至国光失败！");
                    }
                }
                else 
                {
                    ShowMsgHelper.Alert_Error("编辑失败！");
                }
            }
        }
    }
}