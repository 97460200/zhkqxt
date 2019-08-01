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
    public partial class zfzdycd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] != null)
            {
                string state = "";
                string AdminHotelid = "1";
                int Hotelid = 0;
                if (Request["state"] != null)
                {
                    state = Server.UrlDecode(Request["state"]);
                    string[] urlid = state.Split('?'); //dh.sewa-power.com/Reservation/HotelDetails.aspx?AdminHotelid=SEWA006637&hotelid=36 
                    string[] a = urlid[1].Split('&'); //AdminHotelid=SEWA006637&hotelid=36
                    string[] b = a[0].Split('='); //AdminHotelid=SEWA006637
                    string[] c = a[1].Split('='); //hotelid=36
                    AdminHotelid = b[1];
                    Hotelid = Convert.ToInt32(c[1]);
                }


                string code = Request["code"].ToString();
                string postUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
                postUrl = string.Format(postUrl, WxPayConfig.APPID(AdminHotelid, Hotelid), WxPayConfig.APPSECRET(AdminHotelid, Hotelid), code);
                string returnJason = GetJson(postUrl);
                //获取返回信息

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(returnJason);
                object value;
                string openid = "";
                if (json.TryGetValue("openid", out value))
                {
                    openid = value.ToString();
                    Session["zfzdyopenid" + AdminHotelid + Hotelid] = openid.ToString();
                    Session.Timeout = 60 * 60 * 24;
                }
                Log.Info("获取支付到店zfzdyopenid：" + openid + ",URL:", state);
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