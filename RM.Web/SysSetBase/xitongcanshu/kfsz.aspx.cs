using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RM.Busines;
using System.Text;
using System.Collections;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.xitongcanshu
{
    public partial class kfsz : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                hdAdminHotelId.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                string sql = string.Format(@"SELECT a.*,b.RunningStatistics FROM Wx_function  a LEFT JOIN Hotel_Admin b ON a.AdminHotelid = b.AdminHotelid where a.AdminHotelid='{0}'", hdAdminHotelId.Value);
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds != null && ds.Rows.Count > 0)
                {

                    string[] sp = ds.Rows[0]["CheckOutRemind"].ToString().Split(':');
                    ddlCheckOutRemind.SelectedValue = sp[0].ToString();//退房/续住提醒时间（时）
                    ddlCheckOutReminds.SelectedValue = sp[1].ToString();//退房/续住提醒时间（分）

                    string[] sps = ds.Rows[0]["CheckOutTime"].ToString().Split(':');
                    ddlHours.SelectedValue = sps[0].ToString();//退房到时时间（时）
                    ddlMinutes.SelectedValue = sps[1].ToString();//退房到时时间（分）
                }
                Bind();
            }
        }

        private void Bind()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM dbo.Hotel_AdminParameter WHERE AdminHotelId = @AdminHotelId");
            SqlParam[] param = new SqlParam[] {
                new SqlParam("@AdminHotelId",hdAdminHotelId.Value)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                BusinessHour.Value = dt.Rows[0]["BusinessHour"].ToString();//酒店的营业时间（时）
                BusinessMinute.Value = dt.Rows[0]["BusinessMinute"].ToString();//酒店的营业时间（分）
                CashPledgeMoney.Value = dt.Rows[0]["CashPledgeMoney"].ToString();//押金金额
            }
        }
    }
}