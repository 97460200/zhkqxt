using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;
using System.Data;
using RM.Common.DotNetUI;
using RM.Busines;
using RM.Common.DotNetBean;
using RM.Web.App_Code;
using System.Collections;
using System.IO;
using System.Text;
using RM.Common.DotNetCode;
namespace RM.Web.SysSetBase.xitongcanshu
{
    public partial class Template : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["AdminHotelid"] != null)
                {
                    hdAdminHotelId.Value = Request.QueryString["AdminHotelid"].ToString();
                }
                else
                {
                    hdAdminHotelId.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                }
                if (Request.QueryString["SelectType"] != null)
                {
                    hdhasTemplate.Value = Request.QueryString["SelectType"].ToString();
                }
            }
        }
    }
}