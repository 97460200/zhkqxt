using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RM.Web.RMBase.SysSetBase.sales
{
    public partial class OrderRecord1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           if (Request["UserId"] != null && Request["UserId"].Trim() != "")
           {
               hdUserId.Value = Request["UserId"].ToString();
           }
        }
    }
}