using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetUI;
using System.Text;
using System.Data;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;
using RM.Common.DotNetData;
using RM.Web.App_Code;
using RM.Common.DotNetEncrypt;

namespace RM.Web.RMBase.SysUser
{
    public partial class UserEntry : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["UserId"]))
                {
                    hdUserId.Value = Request.QueryString["UserId"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["name"]))
                {
                    hdUserName.Value = Request.QueryString["name"];
                }
            }
        }
    }
}