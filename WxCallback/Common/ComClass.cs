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

namespace WxCallback.Common
{
    public class ComClass
    {

        #region ******** 手机验证码是否有效 ********
        /// <summary>
        /// 手机验证码是否有效
        /// </summary>
        /// <param name="Phone">手机号码</param>
        /// <param name="Code">验证码</param>
        /// <returns></returns>
        public static bool CheckPhoneCode(string Phone, string Code)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT TOP 1 Code FROM SendRecord WHERE PhoneSubmit = @Phone AND Code = @Code AND CreateTime > DATEADD(mi,-5,GETDATE())");
            SqlParam[] param = new SqlParam[] {
                new SqlParam("@Phone", Phone),
                new SqlParam("@Code", Code)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region ******** 获取酒店名称 ********
        /// <summary>
        /// 获取酒店名称
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public static void GetHotelName(string hotelId, out string adminHotelId, out string hotelName)
        {
            adminHotelId = hotelName = "";
            StringBuilder sql = new StringBuilder();
            sql.Append("Select adminHotelId,name From hotel Where ID = @hotelId");
            SqlParam[] param = new SqlParam[] {
                new SqlParam("@hotelId", hotelId)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                adminHotelId = dt.Rows[0]["adminHotelId"].ToString();
                hotelName = dt.Rows[0]["name"].ToString();
            }
        }
        #endregion

        #region ** PostData **
        
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