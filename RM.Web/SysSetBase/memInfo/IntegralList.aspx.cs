using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RM.Web.SysSetBase.memInfo
{
    public partial class IntegralList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["lsh"]))
                {
                    hdMemberId.Value = Request.QueryString["lsh"].ToString();
                }
                else
                {

                }
            }
        }
    }
}