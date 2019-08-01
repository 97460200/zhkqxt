using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.App_Code;
using RM.Common.DotNetBean;
using System.Text;
using RM.Busines;
using System.Data;


namespace RM.Web.SysSetBase.statement
{
    public partial class SummaryReport : System.Web.UI.Page
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
           

            txtStartTime.Value = DateTime.Now.ToString("yyyy-MM");
            txtEndTime.Value = DateTime.Now.ToString("yyyy-MM");
        }
    }
}