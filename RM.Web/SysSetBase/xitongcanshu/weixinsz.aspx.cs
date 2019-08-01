using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.App_Code;
using System.Net;
using System.IO;
using System.Data;
using System.Text;
using RM.Busines;
using System.Collections;
using RM.Common.DotNetUI;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;

namespace RM.Web.SysSetBase.xitongcanshu
{
    public partial class weixinsz : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();
                bind(AdminHotelid);
            }
        }

        public void bind(string HoleID)
        {
            string sql = string.Format(@"SELECT * FROM WeChatList where AdminHotelid='" + HoleID + "' ");
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            if (ds != null && ds.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(ds.Rows[0]["logo"].ToString()))
                {
                    logo.Src = "/Themes/Images/" + ds.Rows[0]["logo"].ToString();
                    
                }
                
                if (!string.IsNullOrEmpty(ds.Rows[0]["ewm"].ToString()))
                {
                    ewm.Src = "/Themes/Images/" + ds.Rows[0]["ewm"].ToString();
                    
                }
                
                name.InnerText = ds.Rows[0]["name"].ToString();
                subject.InnerText = ds.Rows[0]["subject"].ToString();
                introduce.InnerText = ds.Rows[0]["introduce"].ToString();
                adminname.InnerText = ds.Rows[0]["adminname"].ToString();
                starttime.InnerText = Convert.ToDateTime(ds.Rows[0]["starttime"]).ToString("yyyy年MM月dd日");
                endtime.InnerText = Convert.ToDateTime(ds.Rows[0]["endtime"]).ToString("yyyy年MM月dd日");


                
            }
        }
    }
}