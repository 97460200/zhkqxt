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
using RM.Busines.IDAO;
using RM.Busines.DAL;

namespace RM.Web.Frame
{
    public partial class MainIndex : APageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                //DataTable dtlogin = user_idao.UserLogin(user_Account.Trim(), userPwd.Trim());
                //if (dtlogin != null)
                //{ 

                //}
                spTopUserName.InnerHtml = this.MenuTitle.InnerHtml = Request.Cookies["dladmin_COOKIE"]["User_Name"].ToString()+" [超级管理员]";
            }
        }
    }
}