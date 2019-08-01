using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ThoughtWorks.QRCode.Codec;
using System.Text;
using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.sales
{
    public partial class checkewm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sjhm = Request["key"];//sjhm
            hdwxName.Value = Request["wxName"];//sjhm

            bind(sjhm);
        }

        private void bind(string sjhm)
        {
            //获取用户信息
            StringBuilder sqlKeHu = new StringBuilder();
            sqlKeHu.AppendFormat(@"SELECT * FROM dbo.hy_hyzlxxb WHERE sjhm='" + sjhm + "'");
            DataTable dt = DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).GetDataTableBySQL(sqlKeHu);

            if (dt.Rows.Count > 0)
            {
                string adminhotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();//adminhotelid
                string sql = string.Format(@"  SELECT (SELECT TOP 1 id FROM dbo.Hotel WHERE Hotel.AdminHotelid=@AdminHotelid)hotelid,Hotel_Admin.type FROM dbo.Hotel_Admin
                    where Hotel_Admin.AdminHotelid=@AdminHotelid");
                SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid)};
                DataTable sql1s = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
                string hotelid = "";//酒店ID
                string hotelType = "";//酒店类型
                if (sql1s != null && sql1s.Rows.Count > 0)
                {
                    hotelid = sql1s.Rows[0]["hotelid"].ToString();
                    hotelType = sql1s.Rows[0]["type"].ToString();
                }
                else
                {

                }

                if (dt.Rows[0]["fxurl"] != null && dt.Rows[0]["fxurl"].ToString() != "")
                {
                    fxsrc.Src = "~/QR_code/MemberQRCode/" + dt.Rows[0]["fxurl"] + ".jpg";
                }
                else
                {
                    //查询WeChatInfo，获取NOTIFY_URL
                    //获取用户信息
                    string NOTIFY_URL = "";
                    StringBuilder sqlInfo = new StringBuilder();
                    sqlInfo.AppendFormat(@"SELECT * FROM dbo.WeChatInfo WHERE AdminHotelid='" + adminhotelid + "'");
                    DataTable dtInfo = DataFactory.SqlDataBase().GetDataTableBySQL(sqlInfo);
                    if (dtInfo != null && dtInfo.Rows.Count > 0)
                    {
                        NOTIFY_URL = dtInfo.Rows[0]["NOTIFY_URL"].ToString();
                    }
                    else
                    {

                    }

                    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    qrCodeEncoder.QRCodeVersion = 0;//范围值是0-40
                    qrCodeEncoder.QRCodeScale = 4;
                    String data;
                    if (hotelType == "0")
                    {
                        data = "http://" + NOTIFY_URL + "/Reservation/HotelDetails.aspx?AdminHotelid=" + adminhotelid + "&hotelid=" + hotelid + "&sjhm=" + sjhm + "";
                    }
                    else
                    {
                        data = "http://" + NOTIFY_URL + "/Reservation/HotelList.aspx?AdminHotelid=" + adminhotelid;
                    }
                    System.Drawing.Bitmap image = qrCodeEncoder.Encode(data, Encoding.UTF8);
                    System.IO.MemoryStream MStream = new System.IO.MemoryStream();
                    string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + hotelid;

                    image.Save(Server.MapPath("~/QR_code/MemberQRCode/") + filename + ".jpg");
                    image.Save(MStream, System.Drawing.Imaging.ImageFormat.Gif);
                    fxsrc.Src = "~/QR_code/MemberQRCode/" + filename + ".jpg";
                    hdFxurl.Value = filename + ".jpg";
                    StringBuilder sqlUpdateKeHu = new StringBuilder();
                    sqlUpdateKeHu.AppendFormat(@"update Base_UserInfo set  fxurl='" + filename + "'  where User_Account='" + sjhm + "'");
                    DataTable dtUpdate = DataFactory.SqlDataBase().GetDataTableBySQL(sqlUpdateKeHu);
                }
            }
            else
            {
                return;
                //Response.Redirect("../member/MemberCenter.aspx");
            }
        }

    }
}