using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Busines;
using RM.Common.DotNetCode;
using System.Text;
using System.Data;

namespace RM.Web
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                if (Request["AdminHotelid"] != null && Request["AdminHotelid"].Trim() != "")
                {
                    AdminHotelid.Value = Request["AdminHotelid"];
                    string sql = string.Format(@"select s.id,ha.type,ha.num from dbo.Hotel_Admin ha INNER JOIN hotel s ON ha.AdminHotelid=s.AdminHotelid
                    where ha.AdminHotelid=@AdminHotelid");
                    SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", AdminHotelid.Value)};
                    DataTable sql1s = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
                    if (sql1s != null && sql1s.Rows.Count > 0)
                    {
                        if (sql1s.Rows[0]["type"].ToString() == "0")
                        {                         
                            Response.Redirect(string.Format(@"Reservation/HotelDetails.aspx?AdminHotelid={0}&hotelid={1}", AdminHotelid.Value, sql1s.Rows[0]["id"]));
                        }
                        else
                        {
                            Response.Redirect(string.Format(@"Reservation/HotelList.aspx?AdminHotelid={0}", AdminHotelid.Value));                       
                        }
                    }
                    else
                    {
                        Response.Redirect("/Error/ErrorPage.aspx");
                    }
                }
                else
                {
                    Response.Redirect("/Error/ErrorPage.aspx");
                }
            }
        }
    }
}