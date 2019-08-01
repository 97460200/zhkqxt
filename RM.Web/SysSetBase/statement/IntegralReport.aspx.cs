using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;

namespace RM.Web.SysSetBase.statement
{
    public partial class IntegralReport : System.Web.UI.Page
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
            // 

            //AdminHotelid.Value = "1008337";

            txtStartTime.Value = DateTime.Now.ToString("yyyy-MM-01");
            txtEndTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}