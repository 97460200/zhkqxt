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

namespace RM.Web.SysSetBase.sales
{
    public partial class code_img : System.Web.UI.Page
    {
        public string _img_codes = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = "";

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    id = Request["id"].ToString();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select User_ID,code_img,User_Name from Base_UserInfo where User_ID=@User_ID and DeleteMark = 1");
                    SqlParam[] param = new SqlParam[] { new SqlParam("@User_ID", id) };
                    DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                    if (ds != null && ds.Rows.Count > 0)
                    {
                        txtTitles.Text = ds.Rows[0]["User_Name"].ToString();
                        if (ds.Rows[0]["code_img"] != null && ds.Rows[0]["code_img"].ToString() != "" && ds.Rows[0]["code_img"].ToString() != "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=")
                        {
                            img_codes.Src = ds.Rows[0]["code_img"].ToString();
                            _img_codes = ds.Rows[0]["code_img"].ToString();
                        }
                        else
                        {
                            Hashtable hs = new Hashtable();
                            string img_name = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + TemplateMessage.Sen_img("3@" + id, id); //生成永久二维码      
                            img_codes.Src = img_name;
                            _img_codes = img_name;
                            hs["code_img"] = img_name;
                            DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", id, hs);
                        }
                    }
                    SetEWM();
                }
            }
        }

        public void SetEWM() 
        {
            string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT Pattern FROM Hotel_Admin  WHERE AdminHotelid='{0}' ",AdminHotelid);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0) 
            {
                if (dt.Rows[0][0].ToString().Trim() == "4") 
                {
                    zdyewm.Visible = false;
                    tgewm.Style.Add("width", "100%");
                    tgewm.Style.Add("text-align", "center");
                    tgewm.Style.Add("margin-right", "0");
                    EWM.Style.Add("padding-left", "0");
                    EWM.Style.Add("width", "100%");
                    //tgewm.Style.Add("margin-right", "auto");
                }
            }

        }

    }
}