using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Common.DotNetBean;
using RM.Busines;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common.DotNetUI;
using RM.Web.App_Code;

namespace RM.Web.Frame
{
    public partial class SwitchHotel : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM V_Login WHERE User_Account = @User_Account AND User_Pwd = @User_Pwd");
            SqlParam[] param = new SqlParam[] {
                                     new SqlParam("@User_Account", RequestSession.GetSessionUser().UserAccount),
                                     new SqlParam("@User_Pwd", RequestSession.GetSessionUser().UserPwd)};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            rptAdminHotel.DataSource = dt;
            rptAdminHotel.DataBind();
        }

        public string GetDefaultType(string IsDefault, string AdminHotelid)
        {
            string rt = "";
            if (IsDefault.ToString() == "0")
            {
                rt = "<i class='mr'>默认</i>";
            }
            if (AdminHotelid == RequestSession.GetSessionUser().AdminHotelid.ToString())
            {
                rt += "<i class='dq'>当前</i>";
            }
            return rt;
        }

        public string GetButton(string IsDefault, string User_Id, string User_Account, string User_Pwd, string AdminHotelid)
        {
            //<a onclick="SwitchHotel('','','')">切换</a> <a onclick="DefaultHotel('')" class="w70 act">设为默认</a>
            string rtHtml = "";
            if (User_Id == RequestSession.GetSessionUser().UserId.ToString())
            {
                rtHtml += "<a class=\"act\">切换</a>";
            }
            else
            {
                rtHtml += string.Format("<a onclick=\"SwitchHotel('{0}','{1}','{2}')\">切换</a>", User_Account, User_Pwd, AdminHotelid);
            }

            if (IsDefault.ToString() == "0")
            {
                rtHtml += "<a class=\"w70 act\">设为默认</a>";
            }
            else
            {
                rtHtml += string.Format("<a onclick=\"DefaultHotel('{0}')\" class=\"w70\">设为默认</a>", User_Id);
            }
            return rtHtml;
        }

        protected void lbtDefault_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE Base_UserInfo SET IsDefault = 1 WHERE User_Account = @User_Account AND User_Pwd = @User_Pwd");
            SqlParam[] param = new SqlParam[] {
                                     new SqlParam("@User_Account", RequestSession.GetSessionUser().UserAccount),
                                     new SqlParam("@User_Pwd", RequestSession.GetSessionUser().UserPwd)};

            object obj = DataFactory.SqlDataBase().GetObjectValue(sb, param);


            sb = new StringBuilder();
            sb.Append("UPDATE Base_UserInfo SET IsDefault = 0 WHERE USER_ID = @USER_ID");
            param = new SqlParam[] {
                                     new SqlParam("@USER_ID", hdUserId.Value)};

            obj = DataFactory.SqlDataBase().GetObjectValue(sb, param);

            Bind();

        }

        protected void lbtSwitch_Click(object sender, EventArgs e)
        {
            SwitchLogin(hdUser_Account.Value, hdUser_Pwd.Value, hdAdminHotelid.Value);
        }

        private void SwitchLogin(string user_Account, string userPwd, string AdminHotelid)
        {
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();



            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select top 1 * from V_Login where ");
            strSql.Append("User_Account=@User_Account ");
            strSql.Append("and User_Pwd=@User_Pwd ");
            strSql.Append("and AdminHotelid=@AdminHotelid ");
            SqlParam[] para = {
                  new SqlParam("@AdminHotelid",AdminHotelid),
                  new SqlParam("@User_Account",user_Account),
                  new SqlParam("@User_Pwd",userPwd)};
            DataTable dtlogin = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
            if (dtlogin != null)
            {
                IPScanerHelper objScan = new IPScanerHelper();
                objScan.DataPath = Server.MapPath("/Themes/IPScaner/QQWry.Dat");
                objScan.IP = RequestHelper.GetIP();
                string OWNER_address = objScan.IPLocation();
                if (dtlogin.Rows.Count != 0)
                {

                    if (dtlogin.Rows[0]["DeleteMark"].ToString() == "1")
                    {
                        user_idao.SysLoginLog(1, 10, user_Account, "1", OWNER_address, dtlogin.Rows[0]["User_ID"].ToString(), AdminHotelid);
                        SessionUser user = new SessionUser();
                        user.UserId = dtlogin.Rows[0]["User_ID"].ToString();
                        user.UserAccount = dtlogin.Rows[0]["User_Account"].ToString();
                        user.UserName = dtlogin.Rows[0]["User_Name"].ToString() + "(" + dtlogin.Rows[0]["User_Account"].ToString() + ")";
                        user.UserPwd = dtlogin.Rows[0]["User_Pwd"].ToString();
                        user.AdminHotelid = dtlogin.Rows[0]["AdminHotelid"].ToString();
                        user.Hotelid = dtlogin.Rows[0]["Hotelid"].ToString();
                        user.HotelListId = dtlogin.Rows[0]["HotelListId"].ToString();
                        user.IsAdmin = dtlogin.Rows[0]["IsAdmin"].ToString();
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
                        Response.AppendCookie(cookies);

                        this.Response.Write("<script lanuage=javascript>top.location='/Frame/MainDefault.aspx'</script>");
                    }
                    else
                    {
                        user_idao.SysLoginLog(1, 10, user_Account, "2", OWNER_address, "", dtlogin.Rows[0]["AdminHotelid"].ToString());//账户被锁,联系管理员！
                    }
                }
                else
                {
                    user_idao.SysLoginLog(1, 10, user_Account, "0", OWNER_address, "", "");
                }
            }
        }
    }
}