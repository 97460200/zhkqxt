using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using System.Collections;
using RM.Web.business;

namespace RM.Web.SysSetBase.pay
{
    public partial class PledgeMoneyCode : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(Request["AdminHotelid"]))
            {
                hdAdminHotelId.Value = Request["AdminHotelid"].ToString();
                Bind();
            }
        }


        private void Bind()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT a.PledgeMoneyCode,b.CentreLogo FROM dbo.Hotel_AdminParameter a LEFT JOIN dbo.Hotel_Admin b ON a.AdminHotelId = b.AdminHotelid WHERE a.AdminHotelId = @AdminHotelId");
            SqlParam[] param = new SqlParam[] {
                new SqlParam("@AdminHotelId",hdAdminHotelId.Value)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["PledgeMoneyCode"] != null && dt.Rows[0]["PledgeMoneyCode"].ToString() != "")
                {
                    imgPledgeMoneyCode.Src = dt.Rows[0]["PledgeMoneyCode"].ToString();
                }
                else
                {
                    string img_name = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + TemplateMessage.Sen_Hotelimg("PledgeMoney@" + hdAdminHotelId.Value, hdAdminHotelId.Value, ""); //生成永久二维码      
                    imgPledgeMoneyCode.Src = img_name;
                    Hashtable ht = new Hashtable();
                    ht["PledgeMoneyCode"] = img_name;
                    DataFactory.SqlDataBase().UpdateByHashtable("Hotel_AdminParameter", "AdminHotelId", hdAdminHotelId.Value, ht);
                }

                //酒店二维码LOGO
                if (dt.Rows[0]["CentreLogo"] != null && dt.Rows[0]["CentreLogo"].ToString() != "")
                {
                    First_codes.Src = "/upload/image/" + dt.Rows[0]["CentreLogo"];
                }

            }
        }

    }
}