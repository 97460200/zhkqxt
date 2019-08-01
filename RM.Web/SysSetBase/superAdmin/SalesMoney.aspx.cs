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
namespace RM.Web.SysSetBase.superAdmin
{
    public partial class SalesMoney : System.Web.UI.Page
    {
        public String hotelTreeHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Hdhoteladmin.Value = Request.QueryString["AdminHotelid"];
                bool blHotelTree = false;//是否有多分店权限 多店显示酒店树
                string HotelId = "";//如果只有一家店 默认的酒店ID



                StringBuilder strHtml = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT id,name,AdminHotelid FROM  Hotel_Admin WHERE AdminHotelid= @AdminHotelid");
                SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", Hdhoteladmin.Value) };
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string hotelName = dt.Rows[i]["name"].ToString();
                        string ahid = dt.Rows[i]["AdminHotelid"].ToString();//预设可管理多个酒店用
                        strHtml.Append("<dd class='down'>");
                        strHtml.AppendFormat("<b AdminHotelId = '{0}' HotelId = '0'>{1}</b>", ahid, hotelName);
                        //创建子节点
                        strHtml.Append(HotelTreeHelper.GetTreeNode(ahid, 1, "", out blHotelTree, out HotelId));
                        strHtml.Append("</dd>");
                    }
                }

                hotelTreeHtml = strHtml.ToString();


                HotelTree.Visible = blHotelTree;
                htHotelTree.Value = blHotelTree.ToString();
                hdHotelId.Value = HotelId;
            }

        }
    }
}