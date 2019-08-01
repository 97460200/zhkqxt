using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.App_Code;
using RM.Common.DotNetBean;
using System.Text;
using System.Data;
using RM.Busines;
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.houseState
{
    public partial class hoSeEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtEndTime.Value = DateTime.Now.ToString("yyyy-MM-dd");

                if (!string.IsNullOrEmpty(Request.QueryString["HotelId"]))
                {
                    hdHotelId.Value = Request.QueryString["HotelId"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["RoomId"]))
                {
                    hdRoomId.Value = Request.QueryString["RoomId"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["RuleId"]))
                {
                    hdRuleId.Value = Request.QueryString["RuleId"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["VipCode"]))
                {
                    hdVipCode.Value = Request.QueryString["VipCode"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["DateRange"]))
                {
                    txtStartTime.Value = Request.QueryString["DateRange"];
                    txtEndTime.Value = Request.QueryString["DateRange"];
                }
                hdWeek.Value = Convert.ToDateTime(txtStartTime.Value).DayOfWeek.ToString();
                Bind();
                BindVip();
            }
        }
        private void Bind()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT  a.Name ,
                    b.name HotelName
            FROM    Guestroom a
                    LEFT JOIN Hotel b ON a.HotelID = b.ID
            WHERE   a.ID = @RoomId");
            SqlParam[] param = new SqlParam[] {
                                     new SqlParam("@RoomId", hdRoomId.Value)};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                dTitle.InnerHtml = dt.Rows[0]["Name"].ToString() + "(" + dt.Rows[0]["HotelName"].ToString() + ")";
            }
        }


        public string vipHtml = "";
        public string jgHtml = "";
        public string jfHtml = "";
        private void BindVip()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT replace(hylxcode, ' ', '') as code,hylxname as LevelName FROM hy_hylxbmb where replace(hylxcode, ' ', '')=@VipCode and AdminHotelid=@AdminHotelid order by sort desc");
            SqlParam[] param = new SqlParam[] {
                                new SqlParam("@VipCode", hdVipCode.Value),
                                new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).GetDataTableBySQL(sql, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    vipHtml += "<span>" + dt.Rows[i]["LevelName"] + "</span>";
                    jgHtml += "<span><input type='text' valtype='price' vip_code='" + dt.Rows[i]["code"] + "' />元</span>";
                    jfHtml += "<span><input type='text' valtype='integral' vip_code='" + dt.Rows[i]["code"] + "' />分</span>";
                }
            }
        }
    }
}