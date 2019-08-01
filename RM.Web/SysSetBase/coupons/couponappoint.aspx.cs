using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SQL;

using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Busines;
using System.Text;
using System.Collections;
using RM.Common.DotNetBean;
using RM.Web.RMBase;
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.coupons
{
    public partial class couponappoint : System.Web.UI.Page
    {
        public string _yxMembergrade = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                IsyxMembergrade("0");
                B_sj.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public void IsyxMembergrade(string MembergradeId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtjb = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            if (dtjb != null && dtjb.Rows.Count > 0)
            {
                sb.Append("<label val='0'>全部会员</label>");
                for (int i = 0; i < dtjb.Rows.Count; i++)
                {
                    sb.Append(string.Format("<label class='{0}' val='{2}'>{1}</label>", loadMembergrade(dtjb.Rows[i]["code"].ToString(), MembergradeId.ToString()), dtjb.Rows[i]["LevelName"].ToString(), dtjb.Rows[i]["code"].ToString()));
                }
            }
            _yxMembergrade = sb.ToString();
        }

        string loadMembergrade(string wxdj, string MembergradeId)
        {
            if (MembergradeId == "0")
            {
                return "checked";
            }
            string[] Membergrade = MembergradeId.Split(',');

            for (int j = 0; j < Membergrade.Length; j++)
            {
                if (wxdj.ToString() == Membergrade[j].ToString())
                {
                    return "checked";
                }
                else
                {
                    //return "";
                }
            }
            return "";
        }

        /// <summary>
        /// 添加，修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumit_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht["dxlx"] = hddxlx.Value;
            ht["xfcs"] = hdxfcs.Value;
            ht["hydj"] = hdhydj.Value;
            ht["xfjg"] = hdxfjg.Value;
            ht["sjhm"] = sjhm.Value;
            if (hdxfjg.Value == "0")
            {

            }
            else if (hdxfjg.Value == "1")
            {
                ht["StartData"] = DateTime.Now.AddMonths(-1);
                ht["EndData"] = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (hdxfjg.Value == "2")
            {
                ht["StartData"] = DateTime.Now.AddMonths(-2);
                ht["EndData"] = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (hdxfjg.Value == "3")
            {
                ht["StartData"] = DateTime.Now.AddMonths(-3);
                ht["EndData"] = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (hdxfjg.Value == "4")
            {
                ht["StartData"] = DateTime.Now.AddMonths(-4);
                ht["EndData"] = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (hdxfjg.Value == "5")
            {
                ht["StartData"] = StartData.Value;
                ht["EndData"] = EndData.Value;
            }

            ht["zssj"] = B_sj.Value;
            
            ht["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();

            int x = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("GiftCoupons", ht);
            
            AlertReloadClose("ok", "ListGrid()");
            
            
        }

        public static void AlertReloadClose(string message, string method)
        {
            ExecuteScript(string.Format("var i1 = parent.window.frames['MattersEdit']; var val = i1.document.getElementById('zsdx'); $(val).show();OpenClose();showTipsMsg('{0}','2500','4');", message, method));
        }

        public static void ExecuteScript(string scriptBody)
        {
            string scriptKey = "Somekey";
            Page p = HttpContext.Current.Handler as Page;
            p.ClientScript.RegisterStartupScript(typeof(string), scriptKey, scriptBody, true);
        }
    }
}