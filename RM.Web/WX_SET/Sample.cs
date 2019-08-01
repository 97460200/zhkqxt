using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Text;
using System.Xml;
using System.Web.Security;
using RM.Web.Lib;
using System.Collections;
using RM.Busines;

namespace RM.Web.WX_SET
{
    public class Sample
    {
        public Page page { get; set; }
        public Sample(Page page)
        {
            this.page = page;
        }
        const string Token = "sewapower";

        internal void ProcessNotify()
        {
            try
            {
                if (page.Request.HttpMethod == "POST")
                {
                    string weixin = "";
                    weixin = PostInput();//获取xml数据
                    //Log.Info("微信关注返回内容", weixin);
                    if (!string.IsNullOrEmpty(weixin))
                    {
                        ResponseMsg(weixin);//调用消息适配器
                    }
                }
                else
                {
                    Valid();
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                page.Response.Write("error");
                page.Response.End();
            }
        }

        #region 获取post请求数据
        /// <summary>
        /// 获取post请求数据
        /// </summary>
        /// <returns></returns>
        private string PostInput()
        {
            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            return Encoding.UTF8.GetString(b);
        }
        #endregion


        #region 消息类型适配器
        private void ResponseMsg(string weixin)// 服务器响应微信请求
        {
            string resxml = "";
            try
            {
                //XmlDocument doc = new XmlDocument();
                SafeXmlDocument doc = new SafeXmlDocument();
                doc.LoadXml(weixin);//读取xml字符串
                XmlElement root = doc.DocumentElement;
                ExmlMsg xmlMsg = GetExmlMsg(root);
                string messageType = xmlMsg.MsgType;//获取收到的消息类型。文本(text)，图片(image)，语音等。

                switch (messageType)
                {
                    //当消息为文本时
                    case "text":
                        string con = xmlMsg.Content.Trim();
                        resxml = Reply.reply(con, xmlMsg.ToUserName, xmlMsg.FromUserName, xmlMsg.CreateTime);
                        break;
                    case "event":
                        if (!string.IsNullOrEmpty(xmlMsg.EventName) && (xmlMsg.EventName == "subscribe" || xmlMsg.EventName == "SCAN"))
                        {
                            Hashtable ht = new Hashtable();
                            ht["ToUserName"] = xmlMsg.ToUserName;
                            ht["FromUserName"] = xmlMsg.FromUserName;
                            ht["CreateTime"] = xmlMsg.CreateTime;
                            ht["MsgType"] = xmlMsg.MsgType;
                            ht["Event"] = xmlMsg.EventName;
                            ht["EventKey"] = xmlMsg.EventKey;
                            bool zizhu = (string.IsNullOrEmpty(xmlMsg.EventKey) || xmlMsg.EventKey.IndexOf("last_trade_no") >= 0);//是否为自主关注
                            ht["Independent"] = zizhu;
                            ht["Url"] = page.Request.Url.ToString();
                            DataFactory.SqlDataBase().InsertByHashtable("Wx_CallbackLog", ht);

                            resxml = "error";
                            if (zizhu)
                            {
                                if (xmlMsg.ToUserName.ToString() == "gh_4305dc154099")
                                {
                                    //升维商户平台自己关注推送信息
                                    resxml = Reply.Automatic_SewaFocus(xmlMsg.ToUserName, xmlMsg.FromUserName, xmlMsg.CreateTime);
                                }
                                else
                                {
                                    resxml = Reply.AutonomyFollow(xmlMsg.ToUserName, xmlMsg.FromUserName, xmlMsg.CreateTime);//自主关注
                                }
                                break;
                            }

                            string[] keys = xmlMsg.EventKey.Split('@');
                            if (keys.Length < 2)
                            {
                                break;
                            }
                            switch (xmlMsg.EventName)
                            {
                                case "subscribe"://用户未关注时，进行关注后的事件推送
                                    if (keys[1] == "codeimg") //账单二维码 1、类型 2、Adminhotelid 3、账单号
                                    {
                                        resxml = Reply.zd_subscribe(xmlMsg.ToUserName, keys[3], xmlMsg.FromUserName, xmlMsg.CreateTime, keys[2]);
                                    }
                                    else if (keys[1] == "3")  //扫描二维码回复事件 3
                                    {
                                        string userid = keys[2];
                                        resxml = Reply.ScanUserCode(xmlMsg.ToUserName, userid, xmlMsg.FromUserName, xmlMsg.CreateTime);
                                    }
                                    else if (keys[1] == "4")  //扫描酒店/门店二维码回复事件 
                                    {
                                        resxml = Reply.UnHotelFocusWelfare(xmlMsg.ToUserName, keys[2], xmlMsg.FromUserName, xmlMsg.CreateTime);
                                    }
                                    break;
                                case "SCAN"://用户已关注时的事件推送

                                    if (keys[1] == "codeimg") //账单二维码
                                    {
                                        resxml = Reply.zd_subscribe(xmlMsg.ToUserName, keys[3], xmlMsg.FromUserName, xmlMsg.CreateTime, keys[2]);
                                    }
                                    else if (keys[1] == "3")  //扫描二维码回复事件 3
                                    {
                                        string userid = keys[2];
                                        resxml = Reply.ScanUserCode(xmlMsg.ToUserName, userid, xmlMsg.FromUserName, xmlMsg.CreateTime);
                                        //   Log.Info("已关注(扫描推广二维码)：", ToUserName);
                                        //resxml = Reply.FocusWelfare(xmlMsg.ToUserName, xmlMsg.EventKey.ToString().Split('@')[2], xmlMsg.FromUserName, xmlMsg.CreateTime);
                                    }
                                    else if (keys[1] == "4")  //扫描酒店/门店二维码回复事件 3
                                    {
                                        resxml = Reply.HotelFocusWelfare(xmlMsg.ToUserName, keys[2], xmlMsg.FromUserName, xmlMsg.CreateTime);
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
                            ht["Url"] = page.Request.Url.ToString();
                            DataFactory.SqlDataBase().InsertByHashtable("Wx_CallbackLog", ht);

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
                Log.Info("消息类型适配器ResponseMsg()", ex.Message);
            }
            // Log.Info("返回给微信内容:", "------" + resxml + "------");
            page.Response.Write(resxml);
            page.Response.End();
        }
        #endregion


        #region 将datetime.now 转换为 int类型的秒
        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        private int converDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        #endregion

        #region 验证微信签名 保持默认即可
        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        private bool CheckSignature()
        {
            string signature = page.Request.QueryString["signature"].ToString();
            string timestamp = page.Request.QueryString["timestamp"].ToString();
            string nonce = page.Request.QueryString["nonce"].ToString();
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Valid()
        {
            string echoStr = page.Request.QueryString["echoStr"].ToString();
            Log.Info("微信发送的Token验证", echoStr);
            if (CheckSignature())
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    Log.Info("微信发送的Token验证", "成功");
                    page.Response.Write(echoStr);
                    page.Response.End();
                }
            }
        }
        #endregion



        #region 接收的消息实体类 以及 填充方法
        private class ExmlMsg
        {
            /// <summary>
            /// 本公众账号
            /// </summary>
            public string ToUserName { get; set; }
            /// <summary>
            /// 用户账号
            /// </summary>
            public string FromUserName { get; set; }
            /// <summary>
            /// 发送时间戳
            /// </summary>
            public string CreateTime { get; set; }
            /// <summary>
            /// 发送的文本内容
            /// </summary>
            public string Content { get; set; }
            /// <summary>
            /// 消息的类型
            /// </summary>
            public string MsgType { get; set; }
            /// <summary>
            /// 事件名称
            /// </summary>
            public string EventName { get; set; }
            /// <summary>
            /// 事件KEY值
            /// </summary>
            public string EventKey { get; set; }
            /// <summary>
            /// 二维码的ticket
            /// </summary>
            public string Ticket { get; set; }

        }


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
                catch (Exception ex)
                {
                    Log.Info("接收事件推送(EventKey)", ex.Message);
                }
                //try
                //{
                //    xmlMsg.Ticket = root.SelectSingleNode("Ticket").InnerText;
                //}
                //catch (Exception ex)
                //{
                //    Log.Info("接收事件推送(Ticket)", ex.Message);
                //}
            }
            return xmlMsg;
        }
        #endregion
    }
}