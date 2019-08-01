using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;
using Common;
using System.Collections;
using RM.Busines;

namespace WxCallback.Api
{
    /// <summary>
    /// wx_api 的摘要说明
    /// </summary>
    public class wx_api : IHttpHandler
    {

        string log_Folder = "wx_api";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"].ToString();
            JobLogs.Writer(log_Folder, action);
            switch (action)
            {
                case "get_union":
                    get_union(context);
                    break;
                case "get_tags":
                    get_tags(context);
                    break;
                case "set_tags_all"://所有关注粉丝全部设置为星标用户
                    set_tags_all(context);
                    break;
                default:
                    break;
            }
        }
        #region ** 获取微信用户信息 **
        private string get_data(string AdminHotelId, string openid)
        {
            string accessToken = TemplateMessage.GetAccessToken(AdminHotelId);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", accessToken.Trim(), openid);
            return GetJson(url);
        }

        private void get_union(HttpContext context)
        {
            string AdminHotelId = context.Request["AdminHotelId"].ToString();
            string HotelId = context.Request["HotelId"].ToString();
            string openid = context.Request["openid"].ToString();

            #region ** 获取 unionid **

            string data = get_data(AdminHotelId, openid);

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
                csInfo.Add("HotelId", HotelId);//
                string menuInfo = serializer.Serialize(csInfo);
                string postUrl = "http://zidinn.com/API/wxtoken.ashx?action=set_token";
                TemplateMessage.PostWebRequest(postUrl, menuInfo);
                data = get_data(AdminHotelId, openid);
                userInfo = (Dictionary<string, object>)serializer.DeserializeObject(data);
            }

            string subscribe = "";

            if (userInfo.TryGetValue("subscribe", out value))
            {
                subscribe = value.ToString();
            }

            if (subscribe == "" || subscribe == "0")//未关注
            {
                return;
            }

            string subscribe_time = "";//用户关注时间
            string qr_scene_str = "0";//二维码参数

            if (userInfo.TryGetValue("subscribe_time", out value))
            {
                subscribe_time = value.ToString();
            }
            if (userInfo.TryGetValue("qr_scene_str", out value))
            {
                qr_scene_str = value.ToString();
            }

            Hashtable ht = new Hashtable();
            ht["ToUserName"] = ApplicationHelper.GetWxPayConfig(AdminHotelId, "Original_ID", Convert.ToInt32(HotelId));
            ht["FromUserName"] = openid;
            ht["CreateTime"] = subscribe_time;
            ht["MsgType"] = "event";
            ht["Event"] = "SCAN";
            ht["EventKey"] = qr_scene_str;
            bool zizhu = (string.IsNullOrEmpty(qr_scene_str) || qr_scene_str.IndexOf("last_trade_no") >= 0);//是否为自主关注
            ht["Independent"] = zizhu;
            ht["Url"] = context.Request.Url.ToString();
            DataFactory.SqlDataBase().InsertByHashtable("Wx_CallbackLog", ht);

            string[] keys = qr_scene_str.Split('@');
            if (keys.Length < 2)
            {
                return;
            }
            string key_val = keys[1];
            switch (key_val)
            {
                case "3":  //扫描二维码回复事件 3
                    string userid = keys[2];
                    ReplyWX.ScanUserCode(openid, userid, "扫推广码[回调]");
                    break;
                default:
                    break;
            }
            #endregion

        }
        #endregion


        #region ** 获取微信用户信息 **

        private string get_tags_data(string AdminHotelId)
        {
            string accessToken = TemplateMessage.GetAccessToken(AdminHotelId);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/get?access_token={0}", accessToken.Trim());
            return GetJson(url);
        }
        private void get_tags(HttpContext context)
        {
            string AdminHotelId = context.Request["AdminHotelId"].ToString();
            string data = get_tags_data(AdminHotelId);
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
                data = get_tags_data(AdminHotelId);
                userInfo = (Dictionary<string, object>)serializer.DeserializeObject(data);
            }
        }
        #endregion

        #region ** 设置全部粉丝为星标用户 **

        private void set_tags_all(HttpContext context)
        {
            string AdminHotelId = context.Request["AdminHotelId"].ToString();
            string accessToken = TemplateMessage.GetAccessToken(AdminHotelId);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}", accessToken.Trim());
            string jsval = GetJson(url);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            UserInfo userInfo = serializer.Deserialize<UserInfo>(jsval);
            if (userInfo != null && userInfo.count > 0)
            {
                string[] userList = userInfo.data.openid;
                List<string> listOpenid = new List<string>();
                for (int i = 1; i < userList.Length + 1; i++)
                {
                    listOpenid.Add(userList[i - 1].ToString());
                    if (i % 50 == 0)
                    {
                        set_tags(accessToken, listOpenid);
                        listOpenid = new List<string>();
                    }
                    if (userList.Length == i)
                    {
                        set_tags(accessToken, listOpenid);
                    }
                }
                if (userInfo.total > 10000)
                {
                    next_openid(accessToken, userInfo.next_openid, 10000);
                }
            }
        }

        private void next_openid(string accessToken, string nextopenid, int total)
        {
            total += 10000;
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", accessToken.Trim(), nextopenid);
            string jsval = GetJson(url);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            UserInfo userInfo = serializer.Deserialize<UserInfo>(jsval);
            if (userInfo != null && userInfo.count > 0)
            {
                string[] userList = userInfo.data.openid;
                List<string> listOpenid = new List<string>();
                for (int i = 1; i < userList.Length + 1; i++)
                {
                    listOpenid.Add(userList[i - 1].ToString());
                    if (i % 50 == 0)
                    {
                        set_tags(accessToken, listOpenid);
                        listOpenid = new List<string>();
                    }
                    if (userList.Length == i)
                    {
                        set_tags(accessToken, listOpenid);
                    }
                }
                if (userInfo.total > total)
                {
                    next_openid(accessToken, userInfo.next_openid, total);
                }
            }
        }

        private string set_tags(string accessToken, List<string> listOpenid)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string openids = jss.Serialize(listOpenid);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/members/batchtagging?access_token={0}", accessToken);
            string menuInfo = "{\"openid_list\":" + openids + ",\"tagid\":2}";
            string val = TemplateMessage.PostWebRequest(url, menuInfo);
            JobLogs.Writer(log_Folder, "设置用户星标结果:" + val);
            return val;
        }



        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
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



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class UserInfo
    {
        public int total { get; set; }
        public int count { get; set; }
        public UserData data { get; set; }
        public string next_openid { get; set; }
    }

    public class UserData
    {
        public string[] openid { get; set; }
    }
}