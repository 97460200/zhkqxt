using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;

namespace RM.Web.SysSetBase.GuestRoom
{
    public partial class manfang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                adminhotelid.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                //SessionUser user = new SessionUser();
                //user.AdminHotelid = "1";
                //RequestSession.AddSessionUser(user);
            }
        }
    }
}