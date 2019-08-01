using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Busines;
using System.Text;

namespace RM.Web.SysSetBase.memInfo
{
    public partial class memcard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = string.Format(@"select payname,paytype from paytypes where isenble=1 and iddelete=0 and paytype IN (4,8,2,9,41,42,44,45) ");


                ddlSearch.DataSource = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                ddlSearch.DataTextField = "payname";
                ddlSearch.DataValueField = "paytype";
                ddlSearch.DataBind();
                ListItem li = new ListItem("全部类型", "0");
                ddlSearch.Items.Insert(0, li);
            }
        }
    }
}