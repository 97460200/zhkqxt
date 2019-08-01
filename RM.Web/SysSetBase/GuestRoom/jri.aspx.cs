using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetBean;
using System.Data;
using RM.Busines;

namespace RM.Web.SysSetBase.GuestRoom
{
    public partial class jri : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                //SessionUser user = new SessionUser();
                //user.AdminHotelid = "1";
                //RequestSession.AddSessionUser(user);

                //获取节日价设置
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from Holiday where year(StartTime) = '{0}' and AdminHotelid='{1}'  order by ID desc",
                    DateTime.Now.Year, RequestSession.GetSessionUser().AdminHotelid);
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
                GV_mygvInfo.DataSource = dt;
                GV_mygvInfo.DataBind();
            }
        }
    }
}