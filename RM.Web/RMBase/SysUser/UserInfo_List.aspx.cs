using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RM.Common.DotNetUI;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common.DotNetCode;
using System.Text;
using RM.Common.DotNetData;
using RM.Web.App_Code;
using RM.Busines;
using RM.Common.DotNetBean;

namespace RM.Web.RMBase.SysUser
{
    public partial class UserInfo_List : PageBase
    {   
        /// <summary>
        /// 获取用户权限
        /// </summary>
        private void GetUserMenus()
        {
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            hdMenus.Value = user_idao.InitUserRight(RequestSession.GetSessionUser().UserId.ToString(), "用户管理系统");//获取用户对应菜单的权限
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Hdhoteladmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                GetUserMenus();
            }

        }

        protected string substr(object str)
        {
            string msg;
            if (str != null && str.ToString() != "")
            {
                msg = "/Themes/advertise/" + str.ToString();
            }
            else
            {
                msg = "/Themes/Images/people.jpg";

            }
            return msg;
        }
    }
}