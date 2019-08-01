using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.business;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using RM.Web.Lib;
using RM.Common.DotNetUI;
using System.Collections;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.IO;
using RM.Common.DotNetBean;

namespace RM.Web.SysSetBase.memInfo
{
    public partial class ReceiptRules : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdAdminHotelid.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
                SELECT  *  FROM    ReceiptRules 
                WHERE   AdminHotelid =@AdminHotelid 
                ");
                SqlParam[] param = new SqlParam[] {   
                                            new SqlParam("@AdminHotelid", hdAdminHotelid.Value)};
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["RulesContent"] != null && dt.Rows[0]["RulesContent"].ToString()!="")
                    {
                      txtRulesContent.Value= dt.Rows[0]["RulesContent"].ToString();
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
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT  *  FROM    ReceiptRules 
            WHERE   AdminHotelid =@AdminHotelid 
            ");
            SqlParam[] param = new SqlParam[] {   
                                            new SqlParam("@AdminHotelid", hdAdminHotelid.Value)};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)//更新
            {
                Hashtable hs = new Hashtable();
                hs["RulesContent"] = txtRulesContent.Value;
                DataFactory.SqlDataBase().UpdateByHashtable("ReceiptRules", "ID", dt.Rows[0]["ID"].ToString(), hs);
                ShowMsgHelper.OpenClose("修改成功！");
            }
            else
            {
                Hashtable hs = new Hashtable();
                hs["RulesContent"] = txtRulesContent.Value;
                hs["Adminhotelid"] = hdAdminHotelid.Value;
                DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("ReceiptRules", hs);
                ShowMsgHelper.OpenClose("添加成功！");
            }
        }


    
    }
}