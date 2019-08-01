using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;

namespace RM.Web.SysSetBase.points
{
    public partial class points : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdAdminHotelid.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
            }
        }
    }
}