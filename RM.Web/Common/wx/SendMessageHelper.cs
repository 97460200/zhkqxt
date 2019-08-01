using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using RM.Web.business;

namespace Common
{
    public class SendMessageHelper
    {
        #region  ** 发送文本消息 **
        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="openid">用户openid</param>
        /// <param name="context">内容</param>
        public static void Send_Text(string token, string openid, string context)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> diInfo = new Dictionary<string, object>();
            diInfo.Add("touser", openid);//
            diInfo.Add("msgtype", "text");
            diInfo.Add("text", new Dictionary<string, string>
                    {
                        {"content", context}
                    });
            string menuInfo = serializer.Serialize(diInfo);
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token;
            TemplateMessage.PostWebRequest(postUrl, menuInfo);
        }
        #endregion

        #region  ** 发送图片消息 **
        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="openid">用户openid</param>
        /// <param name="media_id">图片Id</param>
        public static void Send_Image(string token, string openid, string media_id)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> diInfo = new Dictionary<string, object>();
            diInfo.Add("touser", openid);//
            diInfo.Add("msgtype", "image");
            diInfo.Add("image", new Dictionary<string, string>
                    {
                        { "media_id", media_id}
                    });
            string menuInfo = serializer.Serialize(diInfo);
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token;
            TemplateMessage.PostWebRequest(postUrl, menuInfo);
        }
        #endregion

        #region  ** 发送语音消息 **
        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="openid">用户openid</param>
        /// <param name="media_id">语音Id</param>
        public static void Send_Voice(string token, string openid, string media_id)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> diInfo = new Dictionary<string, object>();
            diInfo.Add("touser", openid);//
            diInfo.Add("msgtype", "voice");
            diInfo.Add("voice", new Dictionary<string, string>
                    {
                        { "media_id", media_id}
                    });
            string menuInfo = serializer.Serialize(diInfo);
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token;
            TemplateMessage.PostWebRequest(postUrl, menuInfo);
        }
        #endregion


        #region  ** 发送图文消息（点击跳转到图文消息页面） **
        /// <summary>
        /// 发送图文消息（点击跳转到图文消息页面）
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="openid">用户openid</param>
        /// <param name="media_id">图文消息Id</param>
        public static void Send_Mpnews(string token, string openid, string media_id)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> diInfo = new Dictionary<string, object>();
            diInfo.Add("touser", openid);//
            diInfo.Add("msgtype", "mpnews");
            diInfo.Add("mpnews", new Dictionary<string, string>
                    {
                        { "media_id", media_id}
                    });
            string menuInfo = serializer.Serialize(diInfo);
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token;
            TemplateMessage.PostWebRequest(postUrl, menuInfo);
        }
        #endregion

        #region  ** 发送图文消息（点击跳转到外链） **

        /// <summary>
        /// 发送图文消息（点击跳转到外链）
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="openid">用户openid</param>
        /// <param name="title">标题</param>
        /// <param name="description">内容描述</param>
        /// <param name="url">点击跳转链接</param>
        /// <param name="picurl">图片链接</param>
        public static void Send_News(string token, string openid, string title, string description, string url, string picurl)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> diInfo = new Dictionary<string, object>();
            diInfo.Add("touser", openid);//
            diInfo.Add("msgtype", "news");
            List<object> articles = new List<object>();
            articles.Add(new Dictionary<string, string>
                    {
                        { "title", title},
                        { "description", description},
                        { "url", url},
                        { "picurl", picurl}
                    });
            diInfo.Add("news", new Dictionary<string, object>
                    {
                        { "articles", articles}
                    });
            string menuInfo = serializer.Serialize(diInfo);
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token;
            TemplateMessage.PostWebRequest(postUrl, menuInfo);
        }
        #endregion
    }
}