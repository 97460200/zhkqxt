using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using RM.Common.DotNetBean;

namespace RM.Web.App_Code
{
    public class APageBase : Page
    {
        public APageBase()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 检测是匿名访问还是经过登陆访问
        /// </summary>
        protected virtual bool CheckSession()
        {
            if (HttpContext.Current.Request.Cookies["dladmin_COOKIE"] != null)
            {
                if (Request.Cookies["dladmin_COOKIE"]["User_ID"] == null)
                {
                    this.Response.Write("<script lanuage=javascript>top.location='/Frame/adminLogin.htm'</script>");
                    return false;
                }    
            }
            else
            {
                this.Response.Write("<script lanuage=javascript>top.location='/Frame/adminLogin.htm'</script>");
                return false;
            }
            return true;
        }

        protected override void OnInit(EventArgs e)
        {
            //    //检测是匿名访问还是经过登陆访问
            this.CheckSession();

            base.OnInit(e);
        }
    }
}