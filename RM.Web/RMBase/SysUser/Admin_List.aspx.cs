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
    public partial class Admin_List : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Hdhoteladmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                CommonMethod.BindCredentials(Credentials_Type);
            }
        }
    }
}