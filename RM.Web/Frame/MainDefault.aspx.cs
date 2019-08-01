using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;
using RM.Web.App_Code;
using RM.Common.DotNetCode;
using RM.Busines;
using System.Data;
using System.Text;
using System.Collections;

namespace RM.Web.Frame
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
                        this.Response.Write("<script lanuage=javascript>top.location='/Frame/Login.htm'</script>");
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
            try
            {
                DataFactory.ClearConnString(RequestSession.GetSessionUser().AdminHotelid.ToString(), Convert.ToInt32(RequestSession.GetSessionUser().Hotelid));
                //数据库连接状态
                if (DataFactory.CheckSqlIsOpen(RequestSession.GetSessionUser().AdminHotelid.ToString(), Convert.ToInt32(RequestSession.GetSessionUser().Hotelid)))
                {
                    zt.Attributes.Add("class", "ljqk zc");
                    ljzt.InnerText = "连接正常";
                    //ljzt.Style.Add("color", "#5FE390");
                    tk.InnerHtml = "";
                }
                else
                {
                    zt.Attributes.Add("class", "ljqk yc");
                    ljzt.InnerText = "连接异常";
                    HCheckSqlIsOpen.Value = "1";
                    //ljzt.Style.Add("color", "#FA6F72");
                }
            }
            catch { }

        }
    }
}