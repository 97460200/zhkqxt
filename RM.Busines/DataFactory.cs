using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.DataBase;
using PDA_Service.DataBase.DataBase.SqlServer;
using RM.Common.DotNetConfig;
using System.Data;
using RM.Common.DotNetCode;
using System.Collections;
using System.Data.SqlClient;
using System.Threading;
using System.Web;


namespace RM.Busines
{
    /// <summary>
    /// 连接数据库服务工厂
    /// </summary>
    public class DataFactory
    {
        /// <summary>
        /// 链接本地数据库
        /// </summary>
        /// <returns></returns>
        public static IDbHelper SqlDataBase()
        {
            return new SqlServerHelper(ConfigHelper.GetAppSettings("SqlServer_RM_DB"));
        }

        /// <summary>
        /// 酒店数据连接
        /// </summary>
        /// <param name="adminHotelId"></param>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public static IDbHelper SqlDataBase(string adminHotelId, int hotelId)
        {
            string ConnectionString = GetConnString(adminHotelId, hotelId);
            return new SqlServerHelper(ConnectionString);
        }

        public static IDbHelper SqlDataBase(string adminHotelId, string hotelId)
        {
            string ConnectionString = GetConnString(adminHotelId, CommonHelper.GetInt(hotelId));
            return new SqlServerHelper(ConnectionString);
        }

        public static IDbHelper SqlDataBase(string adminHotelId)
        {
            string ConnectionString = GetConnString(adminHotelId, 0);
            return new SqlServerHelper(ConnectionString);
        }

        #region ** 获取酒店数据连接 **
        public static string GetConnString(string adminHotelId, int hotelId)
        {
            string dataVal = "";
            string key_name = "ConnString_" + adminHotelId + hotelId;
            HttpContext rq = HttpContext.Current;

            if (rq.Application[key_name] != null)
            {
                dataVal = rq.Application[key_name].ToString();
            }

            if (dataVal == "")
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ConnString FROM Set_Association where AdminHotelid = @AdminHotelid and hotelid = @HotelId ");
                SqlParam[] parm = new SqlParam[] { 
                    new SqlParam("@HotelId", hotelId),
                    new SqlParam("@AdminHotelid", adminHotelId)
                };
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dataVal = dt.Rows[0]["ConnString"].ToString();
                    if (dataVal != "")
                    {
                        rq.Application[key_name] = dataVal;
                    }
                }
            }
            return dataVal;
        }

        /// <summary>
        /// 清理 全部酒店数据连接
        /// </summary>
        public static void ClearConnString()
        {
            HttpContext rq = HttpContext.Current;
            for (int i = 0; i < rq.Application.Keys.Count; i++)
            {
                string key_name = rq.Application.Keys[i];
                if (key_name.IndexOf("ConnString_") >= 0)
                {
                    rq.Application.Remove(key_name);
                }
            }
        }

        /// <summary>
        /// 清理 指定酒店数据连接
        /// </summary>
        /// <param name="adminHotelId">集团id</param>
        /// <param name="hotelId">酒店id</param>
        public static void ClearConnString(string adminHotelId, int hotelId = 0)
        {
            HttpContext rq = HttpContext.Current;
            string key_name = "ConnString_" + adminHotelId + hotelId;
            rq.Application.Remove(key_name);
        }

        #endregion

        #region ** 酒店数据库是否能连接 **

        static bool Return = true;
        static string Message = "3秒内未连接成功";
        static AutoResetEvent sleepSynchro = new AutoResetEvent(false);
        /// <summary>
        /// 验证数据库是否能连接
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static bool CheckSqlIsOpen(string adminHotelId, int hotelId)
        {
            string ConnectionString = GetConnString(adminHotelId, hotelId);
            Thread controlThread = new Thread(new ParameterizedThreadStart(ConnOpen));
            controlThread.Start((object)ConnectionString);
            if (!sleepSynchro.WaitOne(3000, false))
            {
                Log_SqlOpenIsError(ConnectionString, adminHotelId, hotelId, 1, Message);
                return false;
            }
            else if (!Return)
            {
                Log_SqlOpenIsError(ConnectionString, adminHotelId, hotelId, 2, Message);
                return false;
            }
            return true;
        }

        static void ConnOpen(object sqlconn)
        {
            SqlConnection conn = new SqlConnection(sqlconn.ToString());
            try
            {
                conn.Open();
                Return = true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                Return = false;
            }
            finally
            {
                conn.Close();
                sleepSynchro.Set();
            }

        }

        //保存数据库连不上的记录
        static void Log_SqlOpenIsError(string ConnectionString, string AdminHotelid, int hotelid, int type, string Message)
        {
            try
            {
                Hashtable ht = new Hashtable();
                ht["ConnectionString"] = ConnectionString;
                ht["Message"] = Message;
                ht["AdminHotelid"] = AdminHotelid;
                ht["hotelid"] = hotelid;
                ht["type"] = type;
                ht["AddTime"] = DateTime.Now;
                DataFactory.SqlDataBase().InsertByHashtable("Log_SqlOpenIsError", ht);
            }
            catch { }
        }

        #endregion

    }
}
