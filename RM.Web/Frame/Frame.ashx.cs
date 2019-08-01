using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using System.Data;
using RM.Common.DotNetBean;
using RM.Common.DotNetJson;
using System.Collections;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using System.Text;
using RM.Busines;
using RM.Common.DotNetEncrypt;


namespace RM.Web.Frame
{
    /// <summary>
    /// Frame 的摘要说明
    /// </summary>
    public class Frame : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string Action = context.Request["action"];                      //提交动作
            switch (Action)
            {
                case "login":
                    PwdLogin(context);
                    break;
                case "code_login":
                    CodeLogin(context);
                    break;
                case "retrieve_pwd":
                    Retrieve_Pwd(context);
                    break;
                case "sms_code":
                    SmsCode(context);
                    break;
                case "jizhumima":
                    string str = "0";
                    HttpCookie cookie = context.Request.Cookies["USER_COOKIE"];
                    if (cookie != null)
                    {
                        str = cookie["UserName"] + "&" + cookie["PassWord"] + "&" + cookie["DLAdminHotelid"];
                    }
                    context.Response.Write(str);
                    break;
                case "Menu":
                    RM_System_IDAO sys_idao = new RM_System_Dal();
                    string UserId = RequestSession.GetSessionUser().UserId.ToString();//用户ID
                    string strMenus = JsonHelper.DataTableToJson(sys_idao.GetMenuHtml(UserId), "MENU");
                    context.Response.Write(strMenus);
                    break;
                case "CheckSqlIsOpen":
                    try
                    {
                        //数据库连接状态
                        if (DataFactory.CheckSqlIsOpen(RequestSession.GetSessionUser().AdminHotelid.ToString(), Convert.ToInt32(RequestSession.GetSessionUser().Hotelid)))
                        {
                            StringBuilder sql = new StringBuilder();
                            sql.AppendFormat("SELECT id,ErrorNum FROM dbo.Set_Association WHERE hotelid={0} ", RequestSession.GetSessionUser().Hotelid);
                            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (Convert.ToInt32(dt.Rows[0]["ErrorNum"]) > 0)
                                {
                                    Hashtable ht = new Hashtable();
                                    ht["ErrorNum"] = 0;
                                    DataFactory.SqlDataBase().UpdateByHashtable("Set_Association", "id", dt.Rows[0]["id"].ToString(), ht);




                                }
                            }



                            context.Response.Write("1");
                        }
                        else
                        {
                            context.Response.Write("0");
                        }
                    }
                    catch { }

                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 密码登录
        /// </summary>
        /// <param name="context"></param>
        private void PwdLogin(HttpContext context)
        {
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            RM_System_IDAO sys_idao = new RM_System_Dal();
            IPScanerHelper objScan = new IPScanerHelper();
            string user_Account = context.Request["user_Account"];          //账户
            string userPwd = context.Request["userPwd"];                    //密码
            string jizhu = context.Request["jizhu"];
            DataTable dtlogin = user_idao.UserLogin(user_Account.Trim(), userPwd.Trim());
            if (dtlogin != null)
            {
                objScan.DataPath = context.Server.MapPath("/Themes/IPScaner/QQWry.Dat");
                objScan.IP = RequestHelper.GetIP();
                string OWNER_address = objScan.IPLocation();
                if (dtlogin.Rows.Count != 0)
                {
                    string AdminHotelid = dtlogin.Rows[0]["AdminHotelid"].ToString();
                    LoginInfo(context, dtlogin, OWNER_address);

                    #region ** 记住密码cookie **
                    HttpCookie cookie = context.Request.Cookies["USER_COOKIE"];
                    if (cookie == null)
                    {
                        cookie = new HttpCookie("USER_COOKIE");
                    }
                    cookie.Values.Clear();
                    cookie.Expires = DateTime.Now.AddDays(30);
                    if (jizhu == "1")
                    {
                        cookie.Values.Add("UserName", user_Account);
                        cookie.Values.Add("PassWord", userPwd);
                        cookie.Values.Add("DLAdminHotelid", AdminHotelid);
                        context.Response.AppendCookie(cookie);
                    }
                    else
                    {
                        TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                        cookie.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
                        context.Response.AppendCookie(cookie);
                    }
                    #endregion

                    context.Response.Write("3");//验证成功
                    return;

                }
                else
                {
                    user_idao.SysLoginLog(1, 1, user_Account, "0", OWNER_address, "", "");
                    context.Response.Write("4");//账户或者密码有错误！
                    return;
                }
            }
            else
            {
                context.Response.Write("5");//服务连接不上！
            }
        }

        private void LoginInfo(HttpContext context, DataTable dtlogin, string OWNER_address)
        {
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            string AdminHotelid = dtlogin.Rows[0]["AdminHotelid"].ToString();

            string user_Account = dtlogin.Rows[0]["User_Account"].ToString();          //账户

            user_idao.SysLoginLog(1, 1, user_Account, "1", OWNER_address, dtlogin.Rows[0]["User_ID"].ToString(), AdminHotelid);

            SessionUser user = new SessionUser();
            user.UserId = dtlogin.Rows[0]["User_ID"].ToString();
            user.UserAccount = dtlogin.Rows[0]["User_Account"].ToString();
            user.UserName = dtlogin.Rows[0]["User_Name"].ToString();
            user.UserPwd = dtlogin.Rows[0]["User_Pwd"].ToString();
            user.AdminHotelid = dtlogin.Rows[0]["AdminHotelid"].ToString();
            user.Hotelid = dtlogin.Rows[0]["Hotelid"].ToString();
            user.HotelListId = dtlogin.Rows[0]["HotelListId"].ToString();
            string IsAdmin = dtlogin.Rows[0]["IsAdmin"].ToString();
            user.IsAdmin = IsAdmin;
            string roleName = "";
            if (IsAdmin == "0")
            {
                roleName = "管理员";
            }
            else if (IsAdmin == "1")
            {
                roleName = "管理员";
            }
            else
            {
                roleName = dtlogin.Rows[0]["Roles_Name"].ToString();
            }
            user.RoleName = roleName;

            RequestSession.ClearAllSession();
            RequestSession.AddSessionUser(user);

            HttpCookie cookies = new HttpCookie("LoginUser_CK");
            cookies.Values.Clear();
            cookies.Expires = DateTime.Now.AddDays(1);
            cookies.Values.Add("User_ID", user.UserId.ToString());
            cookies.Values.Add("User_Account", user.UserAccount.ToString());
            cookies.Values.Add("AdminHotelid", user.AdminHotelid.ToString());
            cookies.Values.Add("Hotelid", user.Hotelid.ToString());
            cookies.Values.Add("User_Name", user.UserName.ToString());
            cookies.Values.Add("UserPwd", user.UserPwd.ToString());
            cookies.Values.Add("HotelListId", user.HotelListId.ToString());
            cookies.Values.Add("IsAdmin", user.IsAdmin.ToString());
            cookies.Values.Add("RoleName", user.RoleName.ToString());
            context.Response.AppendCookie(cookies);
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="context"></param>
        private void SmsCode(HttpContext context)
        {
            string phone = context.Request["phone"];    //手机号码
            int num = CommonHelper.GetInt(context.Request["num"]);    //类型 1 登录验证,5忘记密码

            if (ValidateUtil.IsValidMobile(phone) && num > 0)
            {
                context.Response.Write("0");
            }
            Random ran_int = new Random();
            string code = ran_int.Next(1000, 9999).ToString();
            if (CommonMethod.SendSms(phone, code, num, "1"))
                context.Response.Write("1");
            else
                context.Response.Write("0");
        }

        /// <summary>
        /// 手机 验证码登录
        /// </summary>
        /// <param name="context"></param>
        private void CodeLogin(HttpContext context)
        {
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            RM_System_IDAO sys_idao = new RM_System_Dal();
            IPScanerHelper objScan = new IPScanerHelper();
            string phone = context.Request["phone"];          //账户
            string code = context.Request["code"];            //验证码

            StringBuilder sql_code = new StringBuilder();
            sql_code.Append("SELECT TOP 1 Code FROM SendRecord WHERE PhoneSubmit = @Phone AND Code = @Code AND CreateTime > DATEADD(mi,-5,GETDATE())");
            SqlParam[] param_code = new SqlParam[] {
                new SqlParam("@Phone", phone),
                new SqlParam("@Code", code)
            };
            DataTable dt_code = DataFactory.SqlDataBase().GetDataTableBySQL(sql_code, param_code);
            if (dt_code != null && dt_code.Rows.Count > 0)
            {
                DataTable dtlogin = user_idao.PhoneLogin(phone.Trim());
                if (dtlogin != null && dtlogin.Rows.Count > 0)
                {
                    objScan.DataPath = context.Server.MapPath("/Themes/IPScaner/QQWry.Dat");
                    objScan.IP = RequestHelper.GetIP();
                    string OWNER_address = objScan.IPLocation();
                    LoginInfo(context, dtlogin, OWNER_address);
                    context.Response.Write("1");//验证成功
                    return;
                }
                else
                {
                    context.Response.Write("10012");//手机号不存在！
                }
            }
            else
            {
                context.Response.Write("10011");//验证码错误或超时！
            }
        }


        /// <summary>
        /// 手机 验证码登录
        /// </summary>
        /// <param name="context"></param>
        private void Retrieve_Pwd(HttpContext context)
        {
            string phone = context.Request["phone"];          //账户
            string code = context.Request["code"];                    //验证码
            string pwd = context.Request["pwd"];                    //新密码

            StringBuilder sql_code = new StringBuilder();
            sql_code.Append("SELECT TOP 1 Code FROM SendRecord WHERE PhoneSubmit = @Phone AND Code = @Code AND CreateTime > DATEADD(mi,-5,GETDATE())");
            SqlParam[] param_code = new SqlParam[] { 
                new SqlParam("@Phone", phone),
                new SqlParam("@Code", code)
            };
            DataTable dt_code = DataFactory.SqlDataBase().GetDataTableBySQL(sql_code, param_code);
            if (dt_code != null && dt_code.Rows.Count > 0)
            {
                StringBuilder sql_user = new StringBuilder();
                sql_user.Append("SELECT User_ID FROM dbo.Base_UserInfo WHERE User_Account = @User_Account AND DeleteMark = 1");
                SqlParam[] param_user = new SqlParam[] { 
                    new SqlParam("@User_Account", phone)
                };
                DataTable dt_user = DataFactory.SqlDataBase().GetDataTableBySQL(sql_user, param_user);
                if (dt_user != null && dt_user.Rows.Count > 0)
                {
                    Hashtable ht = new Hashtable();
                    ht["User_Pwd"] = Md5Helper.MD5(pwd, 32);
                    DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", dt_user.Rows[0]["User_ID"].ToString(), ht);
                    context.Response.Write("1");//修改成功
                }
                else
                {
                    context.Response.Write("10012");//手机号不存在！
                }
            }
            else
            {
                context.Response.Write("10011");//验证码错误或超时！
            }
        }



        /// <summary>
        /// 同一账号不能同时登陆
        /// </summary>
        /// <param name="context"></param>
        /// <param name="User_Account">账户</param>
        /// <returns></returns>
        public bool Islogin(HttpContext context, string User_Account)
        {
            //将Session转换为Arraylist数组
            //ArrayList list = context.Session["GLOBAL_USER_LIST"] as ArrayList;
            //if (list == null)
            //{
            //    list = new ArrayList();
            //}
            //for (int i = 0; i < list.Count; i++)
            //{
            //    if (User_Account == (list[i] as string))
            //    {
            //        //已经登录了，提示错误信息 
            //        return false; ;
            //    }
            //}
            ////将用户信息添加到list数组中
            //list.Add(User_Account);
            ////将数组放入Session
            //context.Session.Add("GLOBAL_USER_LIST", list);
            return true;
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