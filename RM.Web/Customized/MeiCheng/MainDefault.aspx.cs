using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.App_Code;
using RM.Common.DotNetBean;
using System.Text;
using System.Data;
using RM.Busines;

namespace RM.Web.Customized.MeiCheng
{
    public partial class MainDefault : PageBase
    {
        protected string PageTitle;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (RequestSession.GetSessionUser() == null)
                    {
                        this.Response.Write("<script lanuage=javascript>top.location='/Customized/MeiCheng/Login.aspx'</script>");
                        return;
                    }
                    hdHotelId.Value = RequestSession.GetSessionUser().Hotelid.ToString();
                    hdUserId.Value = RequestSession.GetSessionUser().UserId.ToString();
                    InitData();
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT id,name FROM Hotel_Admin WHERE AdminHotelid='{0}'", RequestSession.GetSessionUser().AdminHotelid);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
            if (dt != null && dt.Rows.Count > 0)
            {
                hotelName.InnerHtml = PageTitle = dt.Rows[0]["name"].ToString();
            }
            string Rolesname = RequestSession.GetSessionUser().RoleName.ToString();
            string un = RequestSession.GetSessionUser().UserName.ToString() + " [" + Rolesname + "]";
            this.MenuTitle.InnerHtml = this.spTopUserName.InnerHtml = un;
        }
    }
}