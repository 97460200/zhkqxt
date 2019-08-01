using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using System.Collections;
using RM.Common.DotNetUI;

namespace RM.Web.SysSetBase.superAdmin
{
    public partial class ConditionMaintainMoney : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["AdminHotelid"]))
                {
                    hdAdminHotelid.Value = Request.QueryString["AdminHotelid"];
                    Bind();
                }
            }
        }

        private void Bind()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TotalNumber,MoreNumber,MoreProportion,MoreMaintainMoney,MoreMaintainProportion FROM dbo.MarketingConfigure WHERE AdminHotelId = @AdminHotelId");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@AdminHotelId",hdAdminHotelid.Value)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                TotalNumber.Value = dt.Rows[0]["TotalNumber"].ToString();
                MoreNumber.Value = dt.Rows[0]["MoreNumber"].ToString();
                MoreProportion.Value = dt.Rows[0]["MoreProportion"].ToString();
                MaintainMoney.Value = dt.Rows[0]["MoreMaintainMoney"].ToString();
                MaintainProportion.Value = dt.Rows[0]["MoreMaintainProportion"].ToString();
                if (MaintainProportion.Value != "0")
                {
                    hdMaintain.Value = "0";
                }
            }
        }

        protected void btnSumit_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht["TotalNumber"] = TotalNumber.Value;
            ht["MoreNumber"] = MoreNumber.Value;
            ht["MoreProportion"] = MoreProportion.Value;
            ht["MoreMaintainMoney"] = MaintainMoney.Value;
            ht["MoreMaintainProportion"] = MaintainProportion.Value;

            int i = DataFactory.SqlDataBase().UpdateByHashtable("MarketingConfigure", "AdminHotelId", hdAdminHotelid.Value, ht);
            if (i > 0)
            {
                ShowMsgHelper.OpenClose("操作成功！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }
        }
    }
}