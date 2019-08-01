using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using System.Text;
using System.Xml;
using Tencent;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using System.Web.Script.Serialization;
using System.Collections;

namespace WxCallback.Api
{
    /// <summary>
    /// WxCallback 的摘要说明
    /// </summary>
    public class WxCallback : IHttpHandler
    {
        string log_Folder = "WxCall";
        string sAppID = "wx1939f9ee65384f14";
        string sToken = "sewapower";
        string sEncodingAESKey = "Sd8AFrmKTlF2u5jbQl8vQEYJX57aALEz1OivuIUgD3r";
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                JobLogs.Writer(log_Folder, "------------------- 分割线 -------------------");
                context.Request.Url.ToString();
                JobLogs.Writer(log_Folder, "授权Url:" + context.Request.Url.ToString());
                System.IO.StreamReader stream = new System.IO.StreamReader(context.Request.InputStream, Encoding.UTF8);
                string postData = stream.ReadToEnd();
                stream.Close();
                JobLogs.Writer(log_Folder, "授权postData*************************************" + postData);


                string timestamp = context.Request.QueryString["timestamp"];//时间戳
                string nonce = context.Request.QueryString["nonce"];//随机数
                string encrypt_type = context.Request.QueryString["encrypt_type"];//加密类型，为aes
                string msg_signature = context.Request.QueryString["msg_signature"];//消息体签名，用于验证消息体的正确性

                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.LoadXml(postData);//读取xml字符串
                XmlElement root = doc.DocumentElement;

                string AppId = root.SelectSingleNode("AppId").InnerText;
                string original_text = "";

                WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
                int ret = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, postData, ref original_text);

                if (original_text == "")
                {
                    JobLogs.Writer(log_Folder, "--------授权解密失败" + ret + "--------");
                    context.Response.Write("error");
                    return;
                }
                JobLogs.Writer(log_Folder, "授权解密结果:" + original_text);


                doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.LoadXml(original_text);//读取xml字符串
                root = doc.DocumentElement;

                string InfoType = root.SelectSingleNode("InfoType").InnerText;
                JobLogs.Writer(log_Folder, "授权消息类型:" + InfoType);

                switch (InfoType)
                {
                    case "component_verify_ticket"://推送component_verify_ticket协议

                        string CreateTime = root.SelectSingleNode("CreateTime").InnerText;
                        string Ticket = root.SelectSingleNode("ComponentVerifyTicket").InnerText;

                        StringBuilder sb = new StringBuilder();
                        sb.Append(@"SELECT * FROM Wx_PlatformInfo WHERE AppId = @AppId");
                        SqlParam[] param = new SqlParam[] { 
                            new SqlParam("@AppId", AppId)
                        };
                        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                        if (dt == null || dt.Rows.Count < 1)
                        {
                            context.Response.Write("success");
                            return;
                        }
                        string data_Ticket = dt.Rows[0]["Ticket"].ToString();
                        string token = dt.Rows[0]["Token"].ToString();
                        update_code(AppId, token);
                        if (data_Ticket == Ticket)
                        {
                            JobLogs.Writer(log_Folder, "data_Ticket================================================Ticket");
                            context.Response.Write("success");
                            return;
                        }
                        string Appsecret = dt.Rows[0]["Appsecret"].ToString();
                        update_token(AppId, Appsecret, Ticket);
                        break;
                    case "authorized"://授权成功通知
                        authorized(root);
                        break;
                    case "unauthorized"://取消授权通知
                        unauthorized(root);
                        break;
                    case "updateauthorized"://授权更新通知
                        updateauthorized(root);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                context.Response.Write("success");
                return;
            }
            context.Response.Write("success");
            return;
        }

        private void authorized(XmlElement root)
        {
            string AppId = root.SelectSingleNode("AppId").InnerText;
            string CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            //string InfoType = root.SelectSingleNode("InfoType").InnerText;
            string AuthorizerAppid = root.SelectSingleNode("AuthorizerAppid").InnerText;
            string AuthorizationCode = root.SelectSingleNode("AuthorizationCode").InnerText;
            string AuthorizationCodeExpiredTime = root.SelectSingleNode("AuthorizationCodeExpiredTime").InnerText;
            string PreAuthCode = root.SelectSingleNode("PreAuthCode").InnerText;
            Hashtable ht = new Hashtable();
            ht["AppId"] = AppId;
            ht["CreateTime"] = CreateTime;
            ht["InfoType"] = "authorized";
            ht["AuthorizerAppid"] = AuthorizerAppid;
            ht["AuthorizationCode"] = AuthorizationCode;
            ht["AuthorizationCodeExpiredTime"] = AuthorizationCodeExpiredTime;
            ht["PreAuthCode"] = PreAuthCode;
            DataFactory.SqlDataBase().InsertByHashtable("Wx_PlatformUser", ht);
        }

        private void unauthorized(XmlElement root)
        {
            string AppId = root.SelectSingleNode("AppId").InnerText;
            string CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            string AuthorizerAppid = root.SelectSingleNode("AuthorizerAppid").InnerText;
            Hashtable ht = new Hashtable();
            ht["AppId"] = AppId;
            ht["CreateTime"] = CreateTime;
            ht["InfoType"] = "unauthorized";
            DataFactory.SqlDataBase().UpdateByHashtable("Wx_PlatformUser", "AuthorizerAppid", AuthorizerAppid, ht);
        }

        private void updateauthorized(XmlElement root)
        {
            string AppId = root.SelectSingleNode("AppId").InnerText;
            string CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            //string InfoType = root.SelectSingleNode("InfoType").InnerText;
            string AuthorizerAppid = root.SelectSingleNode("AuthorizerAppid").InnerText;
            string AuthorizationCode = root.SelectSingleNode("AuthorizationCode").InnerText;
            string AuthorizationCodeExpiredTime = root.SelectSingleNode("AuthorizationCodeExpiredTime").InnerText;
            string PreAuthCode = root.SelectSingleNode("PreAuthCode").InnerText;
            Hashtable ht = new Hashtable();
            ht["AppId"] = AppId;
            ht["CreateTime"] = CreateTime;
            ht["AuthorizationCode"] = AuthorizationCode;
            ht["AuthorizationCodeExpiredTime"] = AuthorizationCodeExpiredTime;
            ht["PreAuthCode"] = PreAuthCode;
            DataFactory.SqlDataBase().UpdateByHashtable("Wx_PlatformUser", "AuthorizerAppid", AuthorizerAppid, ht);
        }


        private void update_token(string appId, string appsecret, string ticket)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> csInfo = new Dictionary<string, object>();
            csInfo.Add("component_appid", appId);//
            csInfo.Add("component_appsecret", appsecret);
            csInfo.Add("component_verify_ticket", ticket);
            string menuInfo = serializer.Serialize(csInfo);
            string postUrl = "https://api.weixin.qq.com/cgi-bin/component/api_component_token";
            string val = TemplateMessage.PostWebRequest(postUrl, menuInfo);

            Dictionary<string, object> dToken = (Dictionary<string, object>)serializer.DeserializeObject(val);
            string component_access_token = "";
            object value;
            if (dToken.TryGetValue("component_access_token", out value))
            {
                component_access_token = value.ToString();
                Hashtable ht = new Hashtable();
                ht["Token"] = component_access_token;
                ht["Ticket"] = ticket;
                ht["CreateTime"] = DateTime.Now;
                DataFactory.SqlDataBase().UpdateByHashtable("Wx_PlatformInfo", "AppId", appId, ht);
            }
        }

        private void update_code(string appId, string token)
        {
            if (token.Length < 10)
            {
                return;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> csInfo = new Dictionary<string, object>();
            csInfo.Add("component_appid", appId);//
            string menuInfo = serializer.Serialize(csInfo);

            string postUrl = "https://api.weixin.qq.com/cgi-bin/component/api_create_preauthcode?component_access_token=" + token;
            string val = TemplateMessage.PostWebRequest(postUrl, menuInfo);

            Dictionary<string, object> dToken = (Dictionary<string, object>)serializer.DeserializeObject(val);
            string pre_auth_code = "";
            object value;
            if (dToken.TryGetValue("pre_auth_code", out value))
            {
                pre_auth_code = value.ToString();
                Hashtable ht = new Hashtable();
                ht["Code"] = pre_auth_code;
                DataFactory.SqlDataBase().UpdateByHashtable("Wx_PlatformInfo", "AppId", appId, ht);
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}