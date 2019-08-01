using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;
using System.Text;
using System.Data;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common.DotNetData;
using RM.Web.App_Code;
using RM.Common.DotNetCode;
using RM.Busines;
using System.Data.SqlClient;
using System.Collections;
using RM.Common.DotNetUI;

namespace RM.Web.Frame
{
    public partial class HomeIndex : APageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spTopUserName.InnerHtml = Request.Cookies["dladmin_COOKIE"]["User_Name"].ToString();

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
                if (ht["Theme".ToUpper()].ToString().Length > 11)
                {
                    string a = ht["Theme".ToUpper()].ToString().Substring(0, 3);
                    string b = ht["Theme".ToUpper()].ToString().Substring(7, 4);
                    Theme.InnerText = a + "****" + b;
                }
            }
        }

        protected void lbClearApplication_Click(object sender, EventArgs e)
        {
            ApplicationHelper.ClearAllApplication();
        }
    }
}