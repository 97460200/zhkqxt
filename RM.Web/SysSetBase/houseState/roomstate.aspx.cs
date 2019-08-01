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
using RM.Web.App_Code;

namespace RM.Web.SysSetBase.houseState
{
    public partial class roomstate : PageBase
    {
        public string hydjHtml = "";
        public string yearHtml = "";
        public String hotelTreeHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtWeekDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                bind();
            }
        }

        private void bind()
        {
            string HotelId = "";//如果只有一家店 默认的酒店ID
            hotelTreeHtml = HotelTreeHelper.RoomTree(1, out HotelId);
            hdHotelId.Value = HotelId;
            hdAdminHotelId.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();

            DataTable dt = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                hydjHtml += "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        hydjHtml += "<b  class='active' dataid=" + dt.Rows[i]["code"] + ">" + dt.Rows[i]["LevelName"] + "</b>";
                    }
                    else
                    {
                        hydjHtml += "<b dataid=" + dt.Rows[i]["code"] + ">" + dt.Rows[i]["LevelName"] + "</b>";
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
                else
                {
                    yearHtml += "<option value=" + i + ">" + i + "</option>";
                }
            }
            selMonth.Value = DateTime.Now.Month.ToString();
        }
    }
}