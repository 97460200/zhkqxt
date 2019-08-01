using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RM.Web.Frame
{
    public partial class MainAgentIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //DataTable dtlogin = user_idao.UserLogin(user_Account.Trim(), userPwd.Trim());
                //if (dtlogin != null)
                //{ 

                //}
                spTopUserName.InnerHtml = this.MenuTitle.InnerHtml = Request.Cookies["dlagent_COOKIE"]["User_Name"].ToString() + " [一级代理]";
            }
        }
    }
}