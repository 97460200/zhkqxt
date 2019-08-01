using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Busines;
using System.Text;
using System.Data;

namespace RM.Web.SysSetBase.GuestRoom
{
    public partial class kefang : System.Web.UI.Page
    {
        public string hydjHtml = "";
        public string yearHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }

        private void bind()
        {
            adminhotelid.Value = "1";//RequestSession.GetSessionUser().AdminHotelid.ToString();

            string sql = string.Format(@"select id,num from moday where AdminHotelid=@AdminHotelid");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid.Value)};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (ds != null && ds.Rows.Count > 0)
            {
                rili.Value = ds.Rows[0]["num"].ToString();
            }

            DataTable dt = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                hydjHtml += "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["jb"].ToString() == "5")
                    {
                        hydjHtml += "<b  class='active' hydj=" + dt.Rows[i]["code"] + ">" + dt.Rows[i]["LevelName"] + "</b>";
                    }
                    else
                    {
                        hydjHtml += "<b hydj=" + dt.Rows[i]["code"] + ">" + dt.Rows[i]["LevelName"] + "</b>";
                    }
                }
            }


            int years = DateTime.Now.Year;// 取当前时间年               

            //循环
            for (int i = 2017; i <= years + 1; i++)
            {
                if (years == i)
                {
                    yearHtml += "<option selected='selected' value=" + i + ">" + i + "</option>";
                }
                else {
                    yearHtml += "<option value=" + i + ">" + i + "</option>";
                }
            }
            year.Value = years.ToString();
            yuefen.Value = DateTime.Now.Month.ToString();
        }
    }
}