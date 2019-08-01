using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetCode;
using SQL;
using System.Data;
using RM.Common.DotNetUI;
using RM.Busines;
using RM.Common.DotNetBean;
using RM.Web.App_Code;

namespace RM.Web.SysSetBase.coupons
{
    public partial class couponstypelist : System.Web.UI.Page
    {
        public String hotelTreeHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Hdhoteladmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                
            }
        }
    }
}