using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.business;
using System.Text;
using System.Net;
using System.IO;
using RM.Common.DotNetConfig;

namespace RM.Web.WX_SET
{
    public partial class SetMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (true)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["AdminHotelid"]))
                {
                    hdAdminHotelid.Value = Request.QueryString["AdminHotelid"];
                }
            }
        }

        protected void btnCreateMenu_Click(object sender, EventArgs e)
        {
            if (hdAdminHotelid.Value == "")
            {
                lblResult.Text = "结果：操作失败!请带集团酒店id参数(AdminHotelid)";
                return;
            }
            string postUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken(hdAdminHotelid.Value));
            string menuInfo = hdMenu.Value;
            lblResult.Text = "结果：" + PostWebRequest(postUrl, menuInfo);
        }
        private string PostWebRequest(string postUrl, string menuInfo)
        {
            string returnValue = string.Empty;
            try
            {
                byte[] byteData = Encoding.UTF8.GetBytes(menuInfo);
                Uri uri = new Uri(postUrl);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteData.Length;
                //定义Stream信息
                Stream stream = webReq.GetRequestStream();
                stream.Write(byteData, 0, byteData.Length);
                stream.Close();
                //获取返回信息
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                returnValue = streamReader.ReadToEnd();
                //关闭信息
                streamReader.Close();
                response.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.ToString();
            }
            return returnValue;
        }

    }
}