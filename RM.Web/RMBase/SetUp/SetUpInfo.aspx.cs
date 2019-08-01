using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using System.Text;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SetUp
{
    public partial class SetUpInfo : APageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = string.Format(@"SELECT Name FROM V_Hotel_Admin where AdminHotelid=@AdminHotelid");
                SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", Request["AdminHotelid"])};
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
                if (ds != null && ds.Rows.Count > 0)
                {
                    if (Request["Hotelid"].ToString() != "0") 
                    {
                        Menber.Visible = false; Sms.Visible = false; Function.Visible = false;
                        SetDefault.Visible = false; SetMemCode.Visible = false; Setbonus.Visible = false; SetMenu.Visible = false;

                        WeChat.Attributes["class"] = "Tabsel sharetabsliact";  
                    }

                    hotelname.InnerText = ds.Rows[0][0].ToString() + GetHotelName(Request["Hotelid"]);
                }
            }
        }



        public string GetHotelName(string Hotelid)
        {
            string HotelName = "";
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT name FROM Hotel WHERE  ID='" + Hotelid + "'");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0)//获取分店名称
            {
                HotelName = " - " + dt.Rows[0]["name"].ToString();
            }
            return HotelName;
        }
    }
}