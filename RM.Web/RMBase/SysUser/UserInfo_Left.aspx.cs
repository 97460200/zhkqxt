using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using RM.Common.DotNetData;
using RM.Busines;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Web.App_Code;
using RM.Common.DotNetBean;

namespace RM.Web.RMBase.SysUser
{
    public partial class UserInfo_Left : PageBase
    {
        public StringBuilder strHtml = new StringBuilder();
        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        string adminHotelid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                adminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();
                InitInfo();
            }
        }
        /// <summary>
        /// 酒店树
        /// </summary>
        public void InitInfo()
        {
            string hotelName = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT id,name FROM  dbo.Hotel_Admin WHERE AdminHotelid='{0}'", adminHotelid);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
            if (dt.Rows.Count > 0 && dt != null)
            {
                hotelName = dt.Rows[0]["name"].ToString();
            }

            strHtml.Append("<li>");
            strHtml.Append("<div>" + hotelName + "");
            strHtml.Append("<span style='display:none'>" + adminHotelid + "</span></div>");
            //创建子节点
            strHtml.Append(GetTreeNode());
            strHtml.Append("</li>");
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTreeNode()
        {
            DataTable dtHotel = new DataTable();
            string sqls = string.Format(@"select id,name from Hotel where 1=1 and AdminHotelid='{0}' order by sort asc", adminHotelid);
            dtHotel = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls));
            StringBuilder sb_TreeNode = new StringBuilder();
            sb_TreeNode.Append("<ul>");
            foreach (DataRow drv in dtHotel.Rows)
            {
                sb_TreeNode.Append("<li>");
                sb_TreeNode.Append("<div>" + drv["name"] + "");
                sb_TreeNode.Append("<span style='display:none'>" + drv["id"].ToString() + "</span></div>");
                sb_TreeNode.Append("</li>");
            }
            sb_TreeNode.Append("</ul>");

            return sb_TreeNode.ToString();
        }
    }
}