using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;
using System.Collections;
using RM.Common.DotNetEncrypt;
using RM.Busines;
using RM.Web.App_Code;
using RM.Common.DotNetUI;

namespace RM.Web.RMBase.SysUser
{
    public partial class UpdateUserInfo : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdUserId.Value = RequestSession.GetSessionUser().UserId.ToString();
                ddlUser_Role.Items.Add(new ListItem(RequestSession.GetSessionUser().RoleName.ToString()));
                EditData();
            }
        }

        private void EditData()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_UserInfo", "User_ID", hdUserId.Value);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
            }
        }


        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            if (User_Name.Value.ToLower() == "")
            {
                User_Name.Focus();
                errorMsg.InnerHtml = "请输入姓名！";
                return;
            }
            if (User_Account.Value.ToLower() == "")
            {
                User_Name.Focus();
                errorMsg.InnerHtml = "请输入账号/手机号码！";
                return;
            }
            int i = 0;
            Hashtable ht_User = new Hashtable();
            ht_User["User_Name"] = User_Name.Value;
            ht_User["User_Account"] = User_Account.Value;
            ht_User["User_Sex"] = User_Sex.Value;
            i = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", RequestSession.GetSessionUser().UserId.ToString(), ht_User);

            if (i > 0)
            {
                Session.Abandon();  //取消当前会话
                Session.Clear();    //清除当前浏览器所以Session
                Response.Write("<script>alert('修改成功,请重新登录');top.location.href='/Frame/login.aspx'</script>");
            }
            else
            {
                errorMsg.InnerHtml = "修改登录密码失败";
            }
        }
    }
}