using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;
using System.Web.Security;
using System.Collections;
using RM.Common.DotNetEncrypt;
using RM.Busines;

namespace RM.Web.Frame
{
    public partial class UdWpd2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                if (Session["Update_PWD_phone"] == null) 
                {
                    Response.Redirect("/Frame/Login.htm");
                }
            }
        }
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht["User_Pwd"] = Md5Helper.MD5(txtPwd.Text.Trim(), 32);
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "Theme", Request.QueryString["txtPhone"].ToString(), ht);
            if (IsOk)
            {
                CommonMethod.AlertAndRedirect(this.Page, "设置新密码成功，请牢记您的新密码！", "/Frame/Login.htm");
            }
            else 
            {
                CommonMethod.AlertAndRedirect(this.Page, "设置新密码时发生错误，请稍候再试！", Request.Url.PathAndQuery);
            }
            //Base_UserInfoBll bll = new Base_UserInfoBll();
            //WhereOperator wo = new WhereOperator();
            //Base_UserInfoInfo info = new Base_UserInfoInfo();
            //info.User_Pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(this.txtPwd.Text.Trim(), "MD5");
            //// info.ID =Convert.ToInt32(RequestSession.GetSessionUser().UserId.ToString());  //当前用户ID
            //wo.Add("Theme", Request.QueryString["txtPhone"].ToString(), TypeForOperators.Equal);
            //string msg;
            //if (bll.UpdateBase_UserInfo(info, wo, out msg) > 0)
            //    CommonMethod.AlertAndRedirect(this.Page, "设置新密码成功，请牢记您的新密码！", "/Frame/Login.htm");
            //else
            //    CommonMethod.AlertAndRedirect(this.Page, "设置新密码时发生错误，请稍候再试！", Request.Url.PathAndQuery);

        }
    }
}