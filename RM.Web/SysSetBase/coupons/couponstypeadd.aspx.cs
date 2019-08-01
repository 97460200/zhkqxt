using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Busines;
using System.Text;
using System.Data;
using System.Collections;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.coupons
{
    public partial class couponstypeadd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["AdminHotelid"] != null)
                {
                    Hdhoteladmin.Value = Request.QueryString["AdminHotelid"].ToString();
                }
                else
                {
                    Hdhoteladmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                }
                
                if (Request["ID"] != null)
                {
                    int Id = Convert.ToInt32(Request["ID"]);
                    string sql = string.Format("select * from CouponType where ID={0}", Id);
                    DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                    if (ds != null && ds.Rows.Count > 0)
                    {
                        txtKindName.Value = ds.Rows[0]["Name"].ToString();
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
            Hashtable hss = new Hashtable();
            hss["Name"] = txtKindName.Value;
            
            hss["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
            if (Request["ID"] != null)
            {
                //修改
                int a = DataFactory.SqlDataBase().UpdateByHashtable("CouponType", "ID", Request["ID"], hss);
                if (a > 0)
                {

                    ShowMsgHelper.AlertReloadClose("修改成功！", "ListGrid()");
                    CommonMethod.Base_Log("卡券管理", "CouponType", "AdminHotelid:" + hss["AdminHotelid"], "修改卡券类型信息", "修改卡券类型信息");//操作记录

                }
                else
                {
                    ShowMsgHelper.AlertReloadClose("修改失败！", "ListGrid()");
                }
            }
            else
            {
                //添加
                int a = DataFactory.SqlDataBase().InsertByHashtable("CouponType", hss);
                if (a > 0)
                {
                    ShowMsgHelper.AlertReloadClose("添加成功！", "ListGrid()");
                    CommonMethod.Base_Log("卡券管理", "CouponType", "AdminHotelid:" + hss["AdminHotelid"], "添加卡券类型信息", "添加卡券类型信息");//操作记录

                }
                else
                {
                    ShowMsgHelper.AlertReloadClose("添加失败！", "ListGrid()");
                }
            }
        }
    }
}