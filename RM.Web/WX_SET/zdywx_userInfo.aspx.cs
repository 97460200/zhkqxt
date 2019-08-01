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


namespace RM.Web.WX_SET
{
    public partial class zdywx_userInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["code"] != null)
                {
                    string state = "";
                    string AdminHotelid = "1";
                     int Hotelid = 0;
                    if (Request["state"] != null)
                    {
                        state = Server.UrlDecode(Request["state"]);
                    }

                    string code = Request["code"].ToString();
                    string postUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
                    postUrl = string.Format(postUrl, WxPayConfig.APPID(AdminHotelid, Hotelid), WxPayConfig.APPSECRET(AdminHotelid, Hotelid), code);

                    string returnJason = GetJson(postUrl);
                    //获取返回信息
                 
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(returnJason);
                    object value;
                    string openid;
                    if (json.TryGetValue("openid", out value))
                    {
                        openid = value.ToString();
                        Session["zdyopenid"] = openid.ToString();
                        Session["zdyopenid_user"] = openid.ToString();
                        Session.Timeout = 60 * 60 * 24;
                    }
                    string accessToken = "";
                    if (json.TryGetValue("access_token", out value))
                    {
                        accessToken = value.ToString();
                    }

                    string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", accessToken.Trim(), Session["zdyopenid_user"].ToString().Trim());
                    string data = GetJson(url);
                    Dictionary<string, object> json1 = (Dictionary<string, object>)serializer.DeserializeObject(data);
                    Session["zdyInfo"] = json1;
                    Response.Redirect(state, false);

                }
            }
            catch (Exception ee)
            {
                Log.Info("智订云获取用户信息报错：", ee.Message);
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