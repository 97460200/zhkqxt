using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Common;
using System.Collections;
using RM.Busines;
using System.Web.Script.Serialization;
using System.Text;
using Tencent;
using System.Data;
using RM.Common.DotNetCode;

namespace Api
{
    public class Callback
    {
        string log_Folder = "Callback";
        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void SendText(string token, string openid, string context)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> diInfo = new Dictionary<string, object>();
            diInfo.Add("touser", openid);//
            diInfo.Add("msgtype", "text");
            diInfo.Add("text", new Dictionary<string, string>
                    {
                        { "content", context},
                    });
            string menuInfo = serializer.Serialize(diInfo);
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token;
            TemplateMessage.PostWebRequest(postUrl, menuInfo);
        }

        private void GetToken(string appid, ref string Appsecret, ref string Ticket, ref string Token, ref string Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM Wx_PlatformInfo WHERE AppId = @AppId");
            SqlParam[] parm = new SqlParam[] { 
                new SqlParam("@AppId", appid)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm);
            if (dt != null && dt.Rows.Count > 0)
            {
                Appsecret = dt.Rows[0]["Appsecret"].ToString();
                Ticket = dt.Rows[0]["Ticket"].ToString();
                Token = dt.Rows[0]["Token"].ToString();
                Code = dt.Rows[0]["Code"].ToString();
            }
        }

        public string Analysis(HttpContext context, string jsonDate, string appid)
        {
            string resxml = "";
            try
            {
                JobLogs.Writer(log_Folder, "------------------- 分割线 -------------------");
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.LoadXml(jsonDate);//读取xml字符串
                XmlElement root = doc.DocumentElement;

                ExmlMsg xmlMsg = GetExmlMsg(root);
                string messageType = xmlMsg.MsgType;//获取收到的消息类型。文本(text)，图片(image)，语音等。

                string openid = xmlMsg.FromUserName;
                switch (messageType)
                {
                    //当消息为文本时
                    case "text":
                        string con = xmlMsg.Content.Trim();

                        //自动化测试的专用测试公众号
                        if (con == "TESTCOMPONENT_MSG_TYPE_TEXT")
                        {
                            resxml = ReplyWX.text_xml("TESTCOMPONENT_MSG_TYPE_TEXT_callback", openid, xmlMsg.ToUserName, xmlMsg.CreateTime);
                        }
                        else
                        {
                            #region **

                            string[] Contents = con.Split(':');
                            if (Contents.Length == 2 && Contents[0] == "QUERY_AUTH_CODE")
                            {
                                string auth = Contents[0];
                                string code = Contents[1];

                                string Appsecret = "";
                                string Ticket = "";
                                string Token = "";
                                string Code = "";

                                GetToken(appid, ref Appsecret, ref Ticket, ref Token, ref Code);//获取Token

                                JobLogs.Writer(log_Folder, "Token111111111111" + Token);
                                if (Token == "")
                                {
                                    appid = "wx1939f9ee65384f14";
                                    GetToken(appid, ref Appsecret, ref Ticket, ref Token, ref Code);
                                }
                                if (Token != "")
                                {
                                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                                    Dictionary<string, object> csInfo = new Dictionary<string, object>();
                                    csInfo.Add("component_appid", appid);//
                                    csInfo.Add("authorization_code", code);//
                                    string menuInfo = serializer.Serialize(csInfo);
                                    string postUrl = "https://api.weixin.qq.com/cgi-bin/component/api_query_auth?component_access_token=" + Token;
                                    string val = TemplateMessage.PostWebRequest(postUrl, menuInfo);

                                    JobLogs.Writer(log_Folder, "返回结果：" + val);
                                    Dictionary<string, object> dToken = (Dictionary<string, object>)serializer.DeserializeObject(val);
                                    if (dToken != null && dToken.Count == 1)
                                    {
                                        Dictionary<string, object> dInfo = dToken["authorization_info"] as Dictionary<string, object>;
                                        string access_token = "";
                                        object value;
                                        if (dInfo.TryGetValue("authorizer_access_token", out value))
                                        {
                                            access_token = value.ToString();
                                            JobLogs.Writer(log_Folder, "access_token:" + access_token);
                                            SendText(access_token, openid, code + "_from_api");
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        break;
                    case "event":
                        if (!string.IsNullOrEmpty(xmlMsg.EventName) && (xmlMsg.EventName == "subscribe" || xmlMsg.EventName == "SCAN"))
                        {
                            break;
                            JobLogs.Writer("event", jsonDate);//写入日志
                            Hashtable ht = new Hashtable();
                            ht["ToUserName"] = xmlMsg.ToUserName;
                            ht["FromUserName"] = xmlMsg.FromUserName;
                            ht["CreateTime"] = xmlMsg.CreateTime;
                            ht["MsgType"] = xmlMsg.MsgType;
                            ht["Event"] = xmlMsg.EventName;
                            ht["EventKey"] = xmlMsg.EventKey;
                            bool zizhu = (string.IsNullOrEmpty(xmlMsg.EventKey) || xmlMsg.EventKey.IndexOf("last_trade_no") >= 0);//是否为自主关注
                            ht["Independent"] = zizhu;
                            ht["Url"] = context.Request.Url.ToString();
                            DataFactory.SqlDataBase().InsertByHashtable("Wx_CallbackLog", ht);

                            string AdminHotelId = ApplicationHelper.GetHotelTweetsInfo(xmlMsg.ToUserName).Rows[0]["AdminHotelid"].ToString();
                            set_user_tag(AdminHotelId, openid);//设置为星标用户

                            if (zizhu)
                            {
                                //if (xmlMsg.ToUserName.ToString() == "gh_4305dc154099")
                                //{
                                //    //关注智订云推送信息
                                //    resxml = ReplyWX.ZDY_Reply(xmlMsg.ToUserName, xmlMsg.FromUserName, xmlMsg.CreateTime);
                                //}
                                //else
                                //{
                                //    resxml = ReplyWX.AutonomyFollow(xmlMsg.ToUserName, xmlMsg.FromUserName, xmlMsg.CreateTime);//自主关注
                                //}
                                break;
                            }

                            string[] keys = xmlMsg.EventKey.Split('@');
                            if (keys.Length < 2)
                            {
                                break;
                            }

                            string key_val = keys[1];
                            switch (xmlMsg.EventName)
                            {
                                case "subscribe"://用户未关注时，进行关注后的事件推送
                                    switch (key_val)
                                    {
                                        case "3":  //扫描二维码回复事件 3
                                            string userid = keys[2];
                                            ReplyWX.ScanUserCode(xmlMsg.FromUserName, userid, "扫推广码[第三方]");
                                            break;
                                        case "4":   //扫描酒店/门店二维码回复事件
                                            ReplyWX.ScanHotelCode(xmlMsg.ToUserName, keys[2], xmlMsg.FromUserName, xmlMsg.CreateTime);
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case "SCAN"://用户已关注时的事件推送
                                    switch (key_val)
                                    {
                                        case "3":  //扫描二维码回复事件 3
                                            string userid = keys[2];
                                            ReplyWX.ScanUserCode(xmlMsg.FromUserName, userid, "扫推广码[第三方]");
                                            break;
                                        case "4":   //扫描酒店/门店二维码回复事件
                                            ReplyWX.ScanHotelCode(xmlMsg.ToUserName, keys[2], xmlMsg.FromUserName, xmlMsg.CreateTime);
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case "scancode_push"://用户已关注时的事件推送
                                    switch (key_val)
                                    {
                                        case "3":  //扫描二维码回复事件 3
                                            string userid = keys[2];
                                            ReplyWX.ScanUserCode(xmlMsg.FromUserName, userid, "扫推广码[第三方]");
                                            break;
                                        case "4":   //扫描酒店/门店二维码回复事件
                                            ReplyWX.ScanHotelCode(xmlMsg.ToUserName, keys[2], xmlMsg.FromUserName, xmlMsg.CreateTime);
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (xmlMsg.EventName == "unsubscribe")//取消订阅
                        {
                            Hashtable ht = new Hashtable();
                            ht["ToUserName"] = xmlMsg.ToUserName;
                            ht["FromUserName"] = xmlMsg.FromUserName;
                            ht["CreateTime"] = xmlMsg.CreateTime;
                            ht["MsgType"] = xmlMsg.MsgType;
                            ht["Event"] = xmlMsg.EventName;
                            ht["EventKey"] = xmlMsg.EventKey;
                            ht["Independent"] = "0";
                            ht["Url"] = context.Request.Url.ToString();
                            DataFactory.SqlDataBase().InsertByHashtable("Wx_CallbackLog", ht);
                        }
                        else if (xmlMsg.EventName == "CLICK")//点击菜单拉取消息时的事件推送
                        {
                            string click_key = xmlMsg.EventKey;
                            if (click_key == "wgz")//默认菜单 点击是重新获取一下 菜单权限
                            {
                                JobLogs.Writer(log_Folder, "------------------默认菜单 点击是重新获取一下 菜单权限-------------------:");
                                string AdminHotelId = ApplicationHelper.GetHotelTweetsInfo(xmlMsg.ToUserName).Rows[0]["AdminHotelid"].ToString();
                                set_user_tag(AdminHotelId, openid);

                                //string yd_yrl = string.Format("http://www.zidinn.com/Reservation/HotelList.aspx?AdminHotelid={0}", AdminHotelId);
                                //resxml = ReplyWX.text_xml("<a href='" + yd_yrl + "'>酒店预订</a>", openid, xmlMsg.ToUserName, xmlMsg.CreateTime);
                            }
                        }
                        break;
                    case "image"://2 回复图片消息
                        break;
                    case "voice"://回复语音消息
                        break;
                    case "vedio"://回复视频消息
                        break;
                    case "music"://回复音乐消息
                        break;
                    case "news"://回复图文消息
                        break;
                    case "location"://上报地理位置事件
                        break;
                    case "link":
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                resxml = ex.Message;
            }
            return resxml;
        }

        #region ** 设置用户星标 **

        private string set_tags(string AdminHotelId, string openid)
        {
            string accessToken = TemplateMessage.GetAccessToken(AdminHotelId);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/members/batchtagging?access_token={0}", accessToken.Trim());
            string menuInfo = "{\"openid_list\":[\"" + openid + "\"],\"tagid\":2}";
            string val = TemplateMessage.PostWebRequest(url, menuInfo);
            JobLogs.Writer(log_Folder, "设置用户星标结果:" + val);
            return val;
        }
        private void set_user_tag(string AdminHotelId, string openid)
        {

            JobLogs.Writer(log_Folder, "设置用户星标");
            string data = set_tags(AdminHotelId, openid);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> userInfo = (Dictionary<string, object>)serializer.DeserializeObject(data);
            object value = null;
            string errcode = "";
            if (userInfo.TryGetValue("errcode", out value))
            {
                errcode = value.ToString();
            }
            if (errcode == "40001")
            {
                Hashtable htToken = new Hashtable();
                htToken["Access_Token"] = "";
                DataFactory.SqlDataBase().UpdateByHashtable("WX_Access_Token", "AdminHotelId", AdminHotelId, htToken);

                Dictionary<string, object> csInfo = new Dictionary<string, object>();
                csInfo.Add("AdminHotelId", AdminHotelId);//
                csInfo.Add("HotelId", "0");//
                string menuInfo = serializer.Serialize(csInfo);
                string postUrl = "http://zidinn.com/API/wxtoken.ashx?action=set_token";
                TemplateMessage.PostWebRequest(postUrl, menuInfo);
                data = set_tags(AdminHotelId, openid);
                userInfo = (Dictionary<string, object>)serializer.DeserializeObject(data);
            }

            string accessToken = TemplateMessage.GetAccessToken(AdminHotelId);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/trymatch?access_token={0}", accessToken.Trim());
            string cdInfo = "{\"user_id\":\"" + openid + "\"}";
            string val = TemplateMessage.PostWebRequest(url, cdInfo);
            JobLogs.Writer(log_Folder, "测试个性化菜单匹配结果:" + val);

        }

        #endregion

        private ExmlMsg GetExmlMsg(XmlElement root)
        {
            ExmlMsg xmlMsg = new ExmlMsg()
            {
                FromUserName = root.SelectSingleNode("FromUserName").InnerText,
                ToUserName = root.SelectSingleNode("ToUserName").InnerText,
                CreateTime = root.SelectSingleNode("CreateTime").InnerText,
                MsgType = root.SelectSingleNode("MsgType").InnerText,
                EventName = "",
                EventKey = ""
            };
            if (xmlMsg.MsgType.Trim().ToLower() == "text")
            {
                xmlMsg.Content = root.SelectSingleNode("Content").InnerText;
            }
            else if (xmlMsg.MsgType.Trim().ToLower() == "event")
            {
                xmlMsg.EventName = root.SelectSingleNode("Event").InnerText;
                try
                {
                    if (root.GetElementsByTagName("EventKey").Count > 0)
                    {
                        xmlMsg.EventKey = root.SelectSingleNode("EventKey").InnerText;
                    }
                    //if (root.GetElementsByTagName("Ticket").Count > 0)
                    //{
                    //    xmlMsg.Ticket = root.SelectSingleNode("Ticket").InnerText;
                    //}
                }
                catch
                {

                }
            }
            return xmlMsg;
        }
    }
}