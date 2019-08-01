using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetUI;
using System.Text;
using System.Data;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;
using RM.Common.DotNetData;
using RM.Web.App_Code;
using RM.Common.DotNetEncrypt;

namespace RM.Web.RMBase.SysUserAdmin
{
    public partial class UserInfo_Edit : APageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("ABase_UserInfo", "User_ID", Request.Cookies["dladmin_COOKIE"]["User_ID"].ToString());
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);


                string sqls = string.Format(@"select name from Hotel_Admin where 1=1 and id='{0}' order by sort asc", ht["hotelid".ToUpper()]);
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls));
                if (dt != null && dt.Rows.Count > 0)
                {
                    hotelname.InnerText = dt.Rows[0]["name"].ToString();
                    hotelid.Visible = true;
                }
            }
        }


        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht["User_Account"] = this.User_Account.Text.Trim();
            ht["User_Name"] = this.User_Name.Text.Trim();
            ht["User_Sex"] = User_Sex.Value;
            ht["Email"] = Email.Value;
            ht["Title"] = Title.Value;
            ht["Theme"] = Theme.Value;
            ht["User_Remark"] = User_Remark.Value;
            ht["ModifyDate"] = DateTime.Now;
            ht["ModifyUserId"] = Request.Cookies["dladmin_COOKIE"]["User_ID"].ToString();
            ht["ModifyUserName"] = Request.Cookies["dladmin_COOKIE"]["User_Name"].ToString();
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("ABase_UserInfo", "User_ID", Request.Cookies["dladmin_COOKIE"]["User_ID"].ToString(), ht);
            if (IsOk)
            {
                ShowMsgHelper.AlertClose("操作成功！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }
        }

    }
}