using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.chongzhi
{
    public partial class pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["Money"] != null && Request["Money"].Trim() != "")
                {
                    price.InnerHtml = Request["Money"].ToString();
                }
                if (Request["Type"] != null && Request["Type"].Trim() != "")
                {
                    if (Request["Type"].ToString() == "1")
                    {
                        PayType.InnerHtml = "维护费充值";
                    }
                    else if (Request["Type"].ToString() == "2")
                    {
                        PayType.InnerHtml = "服务费充值";
                    
                    }
                    else if (Request["Type"].ToString() == "3")
                    {
                        PayType.InnerHtml = "奖金充值";
                    }
                }
                
                orders.InnerHtml = "CZ" + CommonHelper.CreateNo();
                rlrq.InnerHtml = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
        }
    }
}