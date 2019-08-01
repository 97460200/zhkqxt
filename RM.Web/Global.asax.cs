using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using RM.Common.DotNetCode;
using System.Diagnostics;
using System.IO;
using System.Text;
using RM.Common.DotNetConfig;
//using RM.Web.RMBase.SysReportForms.StatisticalTable;
using System.Threading;
using System.Net;
using System.Data;
using RM.Busines;

namespace RM.Web
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// 创建系统异常日志
        /// </summary>
        protected LogHelper Logger = new LogHelper("Global");
        void Application_Start(object sender, EventArgs e)
        {
            // 计算人数
            Application.Lock();
            Application["CurrentUsers"] = 0;
            Application.UnLock();

            #region 定时器 检测订单状态同步程序是否启动
            System.Timers.Timer ot_timer = new System.Timers.Timer(300000);//每隔5分钟执行一次
            ot_timer.Elapsed += new System.Timers.ElapsedEventHandler(OrderState);
            //ot_timer.Interval = 1000;//一秒
            ot_timer.Enabled = true;
            #endregion

            //#region 定时器 自动重启IIS
            //System.Timers.Timer myTimer = new System.Timers.Timer(60000);
            //myTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            //myTimer.Interval = 1000;//
            //myTimer.Enabled = true;
            //#endregion
        }

        #region ** 检测订单状态同步程序是否启动 **
        private void OrderState(object source, System.Timers.ElapsedEventArgs e)
        {
            this.SynchroOrderState();
        }
        /// <summary>
        /// 同步程序是否启动
        /// </summary>
        private void SynchroOrderState()
        {
            try
            {
                Logger.WriteLog("检测订单状态同步程序是否启动");

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT  * FROM dbo.Timedtime WHERE   Timed_time > DATEADD(mi, -5, GETDATE())");
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
                if (dt != null && dt.Rows.Count == 0)
                {
                    string url = "http://39.108.182.4:6203/api/SqlIsOpen/CheckTimedTask"; //URL
                    string json = "{\"AdminHotelId\":\"1\"}";
                    byte[] postData = Encoding.UTF8.GetBytes(json);
                    WebClient client = new WebClient();
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    client.Headers.Add("ContentLength", postData.Length.ToString());
                    byte[] data_ret_bytes = client.UploadData(url, "POST", postData);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("同步程序是否启动:" + ex.Message);
            }
        }
        #endregion


        /// <summary>
        /// 错误处理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_Error(object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError().GetBaseException();
            string error = objErr.Message + "";
            Server.ClearError();
            Application["error"] = error;
            //Response.Redirect("~/Error/ErrorPage.aspx");
        }
        void Session_Start(object sender, EventArgs e)
        {
            // 计算人数
            Application.Lock();
            Application["CurrentUsers"] = CommonHelper.GetInt(Application["CurrentUsers"]) + 1;
            Application.UnLock();
        }
        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["CurrentUsers"] = CommonHelper.GetInt(Application["CurrentUsers"]) - 1;
            Application.UnLock();
        }
        /// <summary>
        /// 定时器触发事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            this.RestartIIS();
        }
        /// <summary>
        /// 自动重启IIS
        /// </summary>
        private void RestartIIS()
        {
            if (ConfigHelper.GetAppSettings("IsRestartIIS").Equals("true"))//判断是否自动重启IIS
            {
                if (DateTime.Now.ToString("HH:mm:ss").Equals(ConfigHelper.GetAppSettings("RestartIISTime")))
                {
                    Logger.WriteLog("自动重启IIS时间到了");
                }
            }
        }
    }
}