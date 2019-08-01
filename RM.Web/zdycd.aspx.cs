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

namespace RM.Web
{
    public partial class zdycd : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string session_zhi;
            if (Request.QueryString["code"] != null)
            {
                string state = "";
                string AdminHotelid = "";
                if (Request["state"] != null)
                {
                    state = Server.UrlDecode(Request["state"]);
                    string[] urlid = state.Split('?'); //dh.sewa-power.com/Reservation/HotelDetails.aspx?AdminHotelid=SEWA006637&hotelid=36 
                    string[] a = urlid[1].Split('&'); //AdminHotelid=SEWA006637&hotelid=36
                    string[] b = a[0].Split('='); //AdminHotelid=SEWA006637

                    AdminHotelid = b[1];
                }

                session_zhi = "openid" + AdminHotelid;
                string code = Request["code"].ToString();
                string postUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
                postUrl = string.Format(postUrl, WxPayConfig.APPID(AdminHotelid), WxPayConfig.APPSECRET(AdminHotelid), code);
                string returnJason = GetJson(postUrl);
                //获取返回信息

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(returnJason);
                object value;
                string openid = "";
                if (json.TryGetValue("openid", out value))
                {
                    openid = value.ToString();
                    Session[session_zhi] = openid.ToString();
                    //Session["openid_user"] = openid.ToString();
                    Session.Timeout = 60 * 60 * 24;
                }
                
                string accessToken = "";
                if (json.TryGetValue("access_token", out value))
                {
                    accessToken = value.ToString();
                }

                #region ** 获取 unionid **
                string session_unionid = "unionid" + AdminHotelid;
                string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", accessToken.Trim(), openid);
                string data = GetJson(url);
                Dictionary<string, object> dUnionid = (Dictionary<string, object>)serializer.DeserializeObject(data);
                if (dUnionid != null)
                {
                    string unionid = "0";
                    if (dUnionid.TryGetValue("unionid", out value))
                    {
                        unionid = value.ToString();
                    }
                    Session[session_unionid] = unionid;
                }

                #endregion

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
            Log.Info("infoerr", returnText);
            return returnText;
        }



    }
}