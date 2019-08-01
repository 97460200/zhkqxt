using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;
using RM.Web.App_Code;

namespace RM.Web.SysSetBase.member
{
    public partial class member : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SessionUser user = new SessionUser();
            //user.AdminHotelid = "1";
            //RequestSession.AddSessionUser(user);
            if (!IsPostBack) 
            {
                AdminHotelid.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                //Member3.Attributes["src"] = "/RMBase/SysParameter/Member3.aspx?HoleID=" + AdminHotelid.Value; 
            }
        }
    }
}