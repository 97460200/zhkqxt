using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.App_Code;
using RM.Common.DotNetBean;
using RM.Busines;
using RM.Common.DotNetUI;
using System.Collections;

namespace RM.Web.Frame
{
    public partial class HomeDefault : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (RequestSession.GetSessionUser() == null)
                {
                    this.Response.Write("<script lanuage=javascript>top.location='/Frame/Login.htm'</script>");
                    return;
                }
                hdHotelId.Value = RequestSession.GetSessionUser().Hotelid.ToString();
                if (hdHotelId.Value == "")
                {
                    hdHotelId.Value = "44";
                }
                hdUserId.Value = RequestSession.GetSessionUser().UserId.ToString();
                InitData();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_UserInfo", "User_ID", RequestSession.GetSessionUser().UserId.ToString());
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
                //if (ht["Theme".ToUpper()].ToString().Length > 11)
                //{
                //    string a = ht["Theme".ToUpper()].ToString().Substring(0, 3);
                //    string b = ht["Theme".ToUpper()].ToString().Substring(7, 4);
                //    Theme.InnerText = a + "****" + b;
                //}
            }

            string Rolesname = RequestSession.GetSessionUser().RoleName.ToString();
            string un = RequestSession.GetSessionUser().UserName.ToString() + " [" + Rolesname + "]";
            this.spTopUserName.InnerHtml = un;
        }
    }
}