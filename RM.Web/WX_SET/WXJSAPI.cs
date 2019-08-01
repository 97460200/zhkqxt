using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.Web.Lib;
using System.IO;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using RM.Common.DotNetConfig;
using RM.Web.business;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetCode;
using System.Data;

namespace RM.Web.WX_SET
{
    public class WXJSAPI
    {
        #region 微信接口
        /// <summary>
        /// 获取jsapi_ticket
        /// jsapi_ticket是公众号用于调用微信JS接口的临时票据。
        /// 正常情况下，jsapi_ticket的有效期为7200秒，通过access_token来获取。
        /// 由于获取jsapi_ticket的api调用次数非常有限，频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket 。
        /// </summary>
        /// <param name="access_token">BasicAPI获取的access_token,也可以通过TokenHelper获取</param>
        /// <returns></returns>
        public static string GetTickect(string AdminHotelid)
        {
            string tickect = string.Empty;
            if (string.IsNullOrEmpty(AdminHotelid))
            {
                return tickect;
            }
            if (AdminHotelid == "1004613")
            {
                AdminHotelid = "1";
            }
            int Hotelid = 0;
            string Tickect_Key = "Tickect_Key" + AdminHotelid + "_" + Hotelid;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM WX_Tickect WHERE AdminHotelId = @AdminHotelId AND HotelId = @HotelId");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@AdminHotelId", AdminHotelid),
                new SqlParam("@HotelId", Hotelid)
            };
            string tickect_id = "";
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                tickect_id = dt.Rows[0]["Id"].ToString();
                DateTime tickectTime = Convert.ToDateTime(dt.Rows[0]["CreateTime"].ToString()).AddHours(1);
                if (DateTime.Now < tickectTime)
                {
                    tickect = dt.Rows[0]["Tickect"].ToString();
                    if (tickect.Length > 20)
                    {
                        return tickect;
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string accessToken = TemplateMessage.GetAccessToken(AdminHotelid);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", accessToken);
            string data = GetJson(url);
            Dictionary<string, object> json1 = (Dictionary<string, object>)serializer.DeserializeObject(data);

            object value;
            if (json1.TryGetValue("ticket", out value))
            {
                tickect = value.ToString();
            }
            string ReturnInfo = "";
            if (string.IsNullOrEmpty(tickect))
            {
                Hashtable ht_token = new Hashtable();
                ht_token["Access_Token"] = "";
                ht_token["ReturnInfo"] = "获取ticket失败,重置Access_Token";
                DataFactory.SqlDataBase().Submit_AddOrEdit("WX_Access_Token", "AdminHotelId", AdminHotelid, ht_token);
                if (json1.TryGetValue("errcode", out value))
                {
                    string errcode = value.ToString();
                }
                ReturnInfo = "获取ticket失败";
            }
            Hashtable ht = new Hashtable();
            ht["AdminHotelId"] = AdminHotelid;
            ht["HotelId"] = Hotelid;
            ht["Tickect_Key"] = Tickect_Key;
            ht["Tickect"] = tickect;
            ht["ReturnInfo"] = ReturnInfo;
            ht["CreateTime"] = DateTime.Now;
            DataFactory.SqlDataBase().Submit_AddOrEdit("WX_Tickect", "Id", tickect_id, ht);
            return tickect;
        }

        public static string GetSignature(string jsapi_ticket, string noncestr, string timestamp, string url, out string mess)
        {

            string tmpStr = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url;
            mess = tmpStr;
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");

            return tmpStr.ToLower();

        }

        //访问微信url并返回微信信息
        protected static string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
            string returnText = wc.DownloadString(url);
            if (returnText.Contains("errcode"))
            {
                //可能发生错误
            }
            return returnText;
        }

        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {

            //为了通过证书验证，总是返回true

            return true;

        }

        //创建随机字符串  
        public static string createNonceStr()
        {
            int length = 16;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string str = "";
            Random rad = new Random();
            for (int i = 0; i < length; i++)
            {
                str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
            }
            return str;
        }
        /**
          * 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
           * @return 时间戳
          */
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion





        #region  //获取星期几,判断是周末还是平日价
        /// <summary>
        /// 获取星期几，返回整形
        /// </summary>
        /// <param name="y"></param>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string getWeek_S(DateTime dt)
        {
            string w = "";
            string weekstr = dt.DayOfWeek.ToString();
            switch (weekstr)
            {
                case "Monday": w = "周一"; break;
                case "Tuesday": w = "周二"; break;
                case "Wednesday": w = "周三"; break;
                case "Thursday": w = "周四"; break;
                case "Friday": w = "周五"; break;
                case "Saturday": w = "周六"; break;
                case "Sunday": w = "周日"; break;
            }
            return w;
        }

        /// <summary>
        /// 获取星期几，返回整形
        /// </summary>
        /// <param name="y"></param>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int getWeek(DateTime dt)
        {
            int w = 0;
            string weekstr = dt.DayOfWeek.ToString();

            switch (weekstr)
            {
                case "Monday": w = 0; break;
                case "Tuesday": w = 1; break;
                case "Wednesday": w = 2; break;
                case "Thursday": w = 3; break;
                case "Friday": w = 4; break;
                case "Saturday": w = 5; break;
                case "Sunday": w = 6; break;
            }
            return w;
        }

        /// <summary>
        /// 获取星期几，返回整形
        /// </summary>
        /// <param name="y"></param>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int getStartdateWeek(DateTime dt)
        {
            int w = 0;
            string weekstr = dt.DayOfWeek.ToString();
            switch (weekstr)
            {
                case "Monday": w = 1; break;
                case "Tuesday": w = 2; break;
                case "Wednesday": w = 3; break;
                case "Thursday": w = 4; break;
                case "Friday": w = 5; break;
                case "Saturday": w = 6; break;
                case "Sunday": w = 0; break;
            }
            return w;
        }

        public static int getEnddateWeek(DateTime dt)
        {
            int w = 0;
            string weekstr = dt.DayOfWeek.ToString();
            switch (weekstr)
            {
                case "Monday": w = 6; break;
                case "Tuesday": w = 5; break;
                case "Wednesday": w = 4; break;
                case "Thursday": w = 3; break;
                case "Friday": w = 2; break;
                case "Saturday": w = 1; break;
                case "Sunday": w = 7; break;
            }
            return w;
        }
        /// <summary>
        /// 当前日期
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int getzhoumo(string time, string AdminHotelid)
        {
            int week = getWeek(Convert.ToDateTime(time));
            int num = 0;

            //if (week == 4)
            //{
            //    num = 1;
            //}

            //if (week == 5)
            //{
            //    num = 1;
            //}

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT ID FROM weekend WHERE adminhotelid=@AdminHotelid AND weekend=@week ");
            SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",AdminHotelid ),
                                     new SqlParam("@week",week.ToString() )};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                num = 1;
            }

            return num;
        }

        public static int getzhoumos(string time)
        {
            int week = getWeek(Convert.ToDateTime(time));
            int num = 0;
            num = week;
            return num;
        }


        #endregion
    }
}