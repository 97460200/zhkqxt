using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.App_Code;
using RM.Common.DotNetBean;

namespace RM.Web.SysSetBase.sell
{
    public partial class sell : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }

        private void bind()
        {
            AdminHotelid.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
        }
    }
}