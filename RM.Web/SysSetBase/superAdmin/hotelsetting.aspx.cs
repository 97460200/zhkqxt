using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetBean;
using System.Data;
using RM.Busines;

namespace RM.Web.SysSetBase.superAdmin
{
    public partial class hotelsetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //加载服务选项
                if (Request.QueryString["AdminHotelid"] != null)
                {
                    AdminHotelid.Value = Request.QueryString["AdminHotelid"].ToString();
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("SELECT User_ID,User_Name,User_Account FROM dbo.Base_UserInfo WHERE AdminHotelid='{0}' AND IsAdmin=2 AND DeleteMark=1 ", AdminHotelid.Value);
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
                //this.DDLUserList.DataSource = dt;
                //this.DDLUserList.DataValueField = "User_ID";
                //this.DDLUserList.DataTextField = "User_Name";
                //this.DDLUserList.DataBind();
                this.DDLUserList.Items.Add(new ListItem("请选择人员", "0"));
                if (dt != null && dt.Rows.Count > 0) 
                {
                    for (int i = 0; i < dt.Rows.Count; i++) 
                    {
                        this.DDLUserList.Items.Add(new ListItem(dt.Rows[i]["User_Name"].ToString() + " - " + dt.Rows[i]["User_Account"].ToString(), dt.Rows[i]["User_ID"].ToString()));
                    }
                }
            }
        }
    }
}