using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;
using System.Data;
using RM.Busines;
using System.Text;
using System.Collections;
using RM.Common.DotNetUI;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SetUp
{
    public partial class ParameterWeixins : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string AdminHotelid = hdAdminHotelid.Value= RequestSession.GetSessionUser().AdminHotelid.ToString();
            //tab4
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT Wx_Menu FROM Wx_function WHERE AdminHotelid='{0}' ",AdminHotelid);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            if(dt!=null&&dt.Rows.Count>0)
            {
                //公众号底部菜单权限
                if (dt.Rows[0][0].ToString() != "1")
                {
                    tab4.Visible = false;
                }
                else 
                {
                    tab4.Visible = true;
                }
            }

        }
    }
}