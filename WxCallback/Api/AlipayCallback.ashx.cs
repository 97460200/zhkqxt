using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using System.Collections;
using System.Net;
using System.IO;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Alipay;
using System.Web.Script.Serialization;

namespace WxCallback.Api
{
    /// <summary>
    /// AlipayCallback 的摘要说明
    /// </summary>
    public class AlipayCallback : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string app_id = context.Request["app_id"];
            string app_auth_code = context.Request["app_auth_code"];

            Hashtable ht = new Hashtable();
            ht["App_id"] = app_id;
            ht["App_auth_code"] = app_auth_code;

            string serverUrl = Config.serverUrl;
            string merchant_private_key = Config.merchant_private_key;
            string format = "json";
            string version = Config.version;
            string signType = Config.signtype;
            string alipay_public_key = Config.alipay_public_key;
            string charset = Config.charset;

            IAopClient client = new DefaultAopClient(serverUrl, app_id, merchant_private_key, format, version, signType, alipay_public_key, charset, false);

            AlipayOpenAuthTokenAppRequest request = new AlipayOpenAuthTokenAppRequest();
            request.BizContent = "{" +
            "    \"grant_type\":\"authorization_code\"," +
            "    \"code\":\"" + app_auth_code + "\"" +
            "  }";
            AlipayOpenAuthTokenAppResponse response = client.Execute(request);

            //IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "app_id", "merchant_private_key", "json", "1.0", "RSA2", "alipay_public_key", "GBK", false);
            //AlipayOpenAuthTokenAppRequest request = new AlipayOpenAuthTokenAppRequest();
            //request.BizContent = "{" +
            //"\"grant_type\":\"authorization_code或者refresh_token\"," +
            //"\"code\":\"1cc19911172e4f8aaa509c8fb5d12F56\"," +
            //"\"refresh_token\":\"201509BBdcba1e3347de4e75ba3fed2c9abebE36\"" +
            //"  }";
            //AlipayOpenAuthTokenAppResponse response = client.execute(request);
            //Console.WriteLine(response.Body);

            if (response != null)
            {
                if (!string.IsNullOrEmpty(response.AppAuthToken))
                {
                    ht["AppAuthToken"] = response.AppAuthToken;
                }
                if (!string.IsNullOrEmpty(response.AppRefreshToken))
                {
                    ht["AppRefreshToken"] = response.AppRefreshToken;
                }
                if (!string.IsNullOrEmpty(response.AuthAppId))
                {
                    ht["AuthAppId"] = response.AuthAppId;
                }
                if (!string.IsNullOrEmpty(response.ExpiresIn))
                {
                    ht["ExpiresIn"] = response.ExpiresIn;
                }
                if (!string.IsNullOrEmpty(response.ReExpiresIn))
                {
                    ht["ReExpiresIn"] = response.ReExpiresIn;
                }
                if (!string.IsNullOrEmpty(response.UserId))
                {
                    ht["UserId"] = response.UserId;
                }
                string body = response.Body;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                alipay alipayInfo = serializer.Deserialize<alipay>(body);
                if (alipayInfo != null && alipayInfo.alipay_open_auth_token_app_response != null && alipayInfo.alipay_open_auth_token_app_response.tokens != null && alipayInfo.alipay_open_auth_token_app_response.tokens.Count() > 0)
                {
                    token info = alipayInfo.alipay_open_auth_token_app_response.tokens[0];
                    ht["AppAuthToken"] = info.app_auth_token;
                    ht["AppRefreshToken"] = info.app_refresh_token;
                    ht["AuthAppId"] = info.auth_app_id;
                    ht["ExpiresIn"] = info.expires_in;
                    ht["ReExpiresIn"] = info.re_expires_in;
                    ht["UserId"] = info.user_id;
                }
            }

            DataFactory.SqlDataBase().InsertByHashtable("Alipay_PlatformUser", ht);

            context.Response.Write("success");
            return;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class alipay
    {
        public response alipay_open_auth_token_app_response { get; set; }
        public string sign { get; set; }
    }
    public class response
    {
        public response alipay_open_auth_token_app_response { get; set; }
        public string code { get; set; }
        public string msg { get; set; }
        public token[] tokens { get; set; }
    }
    public class token
    {
        public string app_auth_token { get; set; }
        public string app_refresh_token { get; set; }
        public string auth_app_id { get; set; }
        public string expires_in { get; set; }
        public string re_expires_in { get; set; }
        public string user_id { get; set; }
    }
}