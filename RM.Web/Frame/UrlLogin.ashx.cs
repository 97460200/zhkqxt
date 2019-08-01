using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RM.Common.DotNetEncrypt;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Busines;
using System.Data;
using System.Text;
using System.Web.SessionState;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using System.Collections;
using RM.Common.DotNetUI;
using RM.Common.DotNetJson;
namespace RM.Web.Frame
{
    /// <summary>
    /// UrlLogin 的摘要说明
    /// </summary>
    public class UrlLogin : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Action = context.Request["action"];                      //提交动作
            switch (Action.ToLower())
            {
                case "login":
                    login(context);
                    break;
                case "jizhumima":
                    jizhumima(context);
                    break;
                case "menu":
                    Menu(context);
                    break;
                default:
                    break;
            }

        }
        public void login(HttpContext context)
        {
            string user_Account = context.Request["user_Account"];          //账户
            string userPwd = context.Request["userPwd"];                    //密码
            string jizhu = context.Request["jizhu"]; //是否记住密码
            string fhz = "5";
            string sql = string.Format(@"SELECT id,User_Account,User_Pwd,User_Name,User_ID FROM dbo.ABase_UserInfo WHERE DeleteMark=1 and User_Account=@User_Account and  User_Pwd=@User_Pwd");
            SqlParam[] pamm = new SqlParam[] { 
                                     new SqlParam("@User_Account", user_Account),
                                     new SqlParam("@User_Pwd",Md5Helper.MD5(userPwd,32))  };
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), pamm);
            if (ds != null)
            {
                if (ds.Rows.Count > 0)
                {
                    RequestSession.ClearAllSession();
                    IPScanerHelper objScan = new IPScanerHelper();
                    objScan.DataPath = context.Server.MapPath("/Themes/IPScaner/QQWry.Dat");
                    objScan.IP = RequestHelper.GetIP();
                    Hashtable hs = new Hashtable();
                    hs["SYS_LOGINLOG_IP"] = RequestHelper.GetIP();
                    hs["SYS_LOGINLOG_TIME"] = DateTime.Now;
                    hs["User_Account"] = user_Account;
                    hs["SYS_LOGINLOG_STATUS"] = 1;
                    hs["OWNER_address"] = objScan.IPLocation();
                    hs["Base_UserInfo_ID"] = ds.Rows[0]["User_ID"].ToString();
                    DataFactory.SqlDataBase().InsertByHashtable("ABase_SysLoginlog", hs);

                    HttpCookie cookie = new HttpCookie("admin_cookie");
                    cookie.Expires = DateTime.Now.AddDays(30);
                    if (jizhu == "1")
                    {
                        cookie.Values.Add("UserName", user_Account);
                        cookie.Values.Add("PassWord", userPwd);
                        context.Response.AppendCookie(cookie);
                    }
                    else
                    {
                        cookie.Values.Clear();
                    }

                    HttpCookie cookies = new HttpCookie("dladmin_COOKIE");
                    cookies.Expires = DateTime.Now.AddDays(1);
                    cookies.Values.Add("User_ID", ds.Rows[0]["User_ID"].ToString());
                    cookies.Values.Add("User_Account", user_Account);
                    cookies.Values.Add("User_Name", ds.Rows[0]["User_Name"].ToString());
                    cookies.Values.Add("User_Pwd", ds.Rows[0]["User_Pwd"].ToString());
                    context.Response.AppendCookie(cookies);
                    fhz = "3";
                }
                else
                {
                    fhz = "4";//账户或者密码有错误！
                }
            }
            else
            {
                fhz = "5";
            }

            context.Response.Write(fhz);//服务连接不上！
        }

        public void jizhumima(HttpContext context)
        {
            string str = "";
            if (context.Request.Cookies["admin_cookie"] != null)
            {
                str = context.Request.Cookies["admin_cookie"]["UserName"] + "&" + context.Request.Cookies["admin_cookie"]["PassWord"];
            }
            if (str != "")
            {

            }
            else
            {
                str = "0";
            }
            context.Response.Write(str);
            return;
        }

        public void Menu(HttpContext context)
        {
            RM_System_IDAO sys_idao = new RM_System_Dal();
            string UserId = context.Request.Cookies["dladmin_COOKIE"]["User_ID"];
            string strMenus = JsonHelper.DataTableToJson(sys_idao.GetMenuHtmlA(UserId), "MENU");
            context.Response.Write(strMenus);
            return;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}