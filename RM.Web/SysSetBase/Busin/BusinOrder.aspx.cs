using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetCode;
using SQL;
using System.Data;
using RM.Common.DotNetUI;
using RM.Busines;
using RM.Common.DotNetBean;
using RM.Web.App_Code;

namespace RM.Web.SysSetBase.Busin
{
    public partial class BusinOrder : System.Web.UI.Page
    {
        public String hotelTreeHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {


                Hdhoteladmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                bool blHotelTree = false;//是否有多分店权限 多店显示酒店树
                string HotelId = "";//如果只有一家店 默认的酒店ID

                if (RequestSession.GetSessionUser().Hotelid.ToString() == "0")
                {
                    hotelTreeHtml = HotelTreeHelper.HotelTree(Hdhoteladmin.Value, 1, out blHotelTree, out HotelId);
                }
                else
                {
                    blHotelTree = false;
                    HotelId = RequestSession.GetSessionUser().Hotelid.ToString();
                }
                HotelTree.Visible = blHotelTree;
                htHotelTree.Value = blHotelTree.ToString();
                hdHotelId.Value = HotelId;
            }
        }


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string hotelid = txtSearch.Value;
            string type = txtSearch.Value;
            string start = txtSearch.Value;
            string end = txtSearch.Value;
            string content = txtSearch.Value;
            StringBuilder strSql = new StringBuilder(@"select OrderNumber 订单编号,BusinessName  营业点,OrderTime 下单时间,
              Contact 联系人, ContactPhone as 手机号码,Number as 人数,Address as 所在位置, (case State  when 1 then '已确认' when 2 then '未确认' when 3 then '已取消'  end)状态
              from V_BookOrder  where 1 = 1  and DeleteMark=1  ");
            strSql.Append("  and  AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");
            if (hotelid != null && hotelid != "" && hotelid != "-1")
            {
                strSql.Append("  and  HotelId='" + hotelid + "' ");
            }
            if (type != null && type != "")
            {
                strSql.Append("  and  BusinessId='" + type + "' ");
            }
            if (start != null && start != "" && end != null && end != "")
            {
                string sql = end + " 23:59:59";
                strSql.AppendFormat(" and OrderTime>='{0} 00:00:00' and OrderTime<='{1}'", start, sql);
            }
            else
            {
                if (start != null && start != "")
                {
                    strSql.AppendFormat(" and OrderTime>='{0} 00:00:00'", start);
                }
                else if (end != null && end != "")
                {
                    strSql.AppendFormat("and OrderTime<='{0} 23:59:59'", end);
                }
            }

            if (content != "" && content != null)
            {
                strSql.AppendFormat(" and (OrderNumber like '{0}' or BusinessName like '{0}' or Contact like '{0}' or ContactPhone like '{0}' or Address like '{0}' )", "%" + content + "%");
            }

            strSql.AppendFormat("order by AddTime desc");

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql);

            ExcelHelper.ExportExcel(dt, "营业点订单管理");
        }
    }
}