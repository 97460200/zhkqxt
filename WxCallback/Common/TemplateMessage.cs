using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using System.Net;
using System.IO;

namespace Common
{
    public class TemplateMessage
    {
        #region ** 获取Token **
        public static string GetAccessToken(string AdminHotelid)
        {
            string accessToken = string.Empty;
            if (string.IsNullOrEmpty(AdminHotelid))
            {
                return accessToken;
            }
            if (AdminHotelid == "1004613")
            {
                AdminHotelid = "1";
            }
            int Hotelid = 0;
            string Token_Key = "Token_Key" + AdminHotelid + "_" + Hotelid;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM WX_Access_Token WHERE AdminHotelId = @AdminHotelId AND HotelId = @HotelId");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@AdminHotelId", AdminHotelid),
                new SqlParam("@HotelId", Hotelid)
            };
            string token_id = "";
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                token_id = dt.Rows[0]["Id"].ToString();
                DateTime tokenTime = Convert.ToDateTime(dt.Rows[0]["CreateTime"].ToString()).AddHours(1);
                if (DateTime.Now < tokenTime)
                {
                    accessToken = dt.Rows[0]["Access_Token"].ToString();
                    if (accessToken.Length > 20)
                    {
                        return accessToken;
                    }
                }
            }
            return accessToken;
        }
        #endregion

        #region ** post提交  **
        public static string PostWebRequest(string postUrl, string menuInfo)
        {
            string returnValue = string.Empty;
            try
            {
                byte[] byteData = Encoding.UTF8.GetBytes(menuInfo);
                Uri uri = new Uri(postUrl);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteData.Length;
                //定义Stream信息
                Stream stream = webReq.GetRequestStream();
                stream.Write(byteData, 0, byteData.Length);
                stream.Close();
                //获取返回信息
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                returnValue = streamReader.ReadToEnd();
                //关闭信息
                streamReader.Close();
                response.Close();
                stream.Close();
            }
            catch (Exception ex)
            {

            }
            return returnValue;
        }
        #endregion

    }
}