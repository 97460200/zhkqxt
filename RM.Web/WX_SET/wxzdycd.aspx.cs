using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using RM.Web.Lib;
using RM.Web.business;

namespace RM.Web.WX_SET
{
    public partial class wxzdycd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] != null)
            {
                string state = "";

                if (Request["state"] != null)
                {
                    state = Server.UrlDecode(Request["state"]);
                }

                string code = Request["code"].ToString();
                string postUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
                postUrl = string.Format(postUrl, WxPayConfig.APPID("1", 0), WxPayConfig.APPSECRET("1", 0), code);
                string returnJason = GetJson(postUrl);
                //获取返回信息

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(returnJason);
                object value;
                string openid = "";
                if (json.TryGetValue("openid", out value))
                {
                    openid = value.ToString();
                    Session["zdyopenid"] = openid.ToString();
                    Session.Timeout = 60 * 60 * 24;
                }
                Log.Info("获取智订云openid：" + openid + ",URL:", state);
                Response.Redirect(state, false);
            }

        }
        //访问微信url并返回微信信息
        protected string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);
            if (returnText.Contains("errcode"))
            {
                //可能发生错误
            }
            return returnText;
        }

    }
}