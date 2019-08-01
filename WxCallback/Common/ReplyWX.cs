using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetCode;
using System.Web.Script.Serialization;
using RM.Common.DotNetConfig;
using Tencent;

namespace Common
{
    public class ReplyWX
    {
        #region ** 获取 AdminHotelid **
        public static string adminhotelid(string Original_ID)
        {
            string id = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT AdminHotelid FROM dbo.WeChatInfo WHERE Original_ID='{0}'", Original_ID);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
            if (dt != null && dt.Rows.Count > 0)
            {
                id = dt.Rows[0]["AdminHotelid"].ToString();
            }
            return id;

        }
        #endregion

        #region ** 拼接文本消息 **
        /// <summary>
        /// 拼接文本消息
        /// </summary>
        /// <param name="context">回复的消息内容</param>
        /// <param name="ToUserName">接收方帐号（收到的OpenID）</param>
        /// <param name="FromUserName">开发者微信号</param>
        /// <param name="CreateTime">消息创建时间 （整型）</param>
        /// <returns></returns>
        public static string text_xml(string context, string openId, string FromUserName, string CreateTime)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml><ToUserName><![CDATA[" + openId + "]]></ToUserName><FromUserName><![CDATA[" + FromUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + context + "]]></Content></xml> ");
            return sb.ToString();
        }
        #endregion

        #region ** 消息为文本回复 **

        public static string Text_Reply(string text, string ToUserName, string FromUserName, string CreateTime)
        {
            string AdminHotelid = adminhotelid(ToUserName);
            if (AdminHotelid == "")
            {
                return "";
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("select id,name,media_id,content,code_img,Reply_id,type from Replylist where AdminHotelid=@AdminHotelid and  name like @name ");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@AdminHotelid",AdminHotelid),
                new SqlParam("@name","%" + text + "%")
            };

            string resxml = "";
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
            if (ds != null && ds.Rows.Count > 0)
            {
                if (ds.Rows[0]["type"].ToString() == "1")
                {  //文字回复
                    resxml = text_xml(ds.Rows[0]["content"].ToString(), ToUserName, FromUserName, CreateTime);
                }
                else if (ds.Rows[0]["type"].ToString() == "2") //图片回复
                {
                    resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + ds.Rows[0]["media_id"].ToString() + "]]></MediaId></Image></xml> ";
                }

                if (ds.Rows[0]["name"].ToString() == text)
                {
                    Hashtable hs = new Hashtable();
                    hs["Reply_id"] = ds.Rows[0]["Reply_id"].ToString();
                    hs["ReplyList_id"] = ds.Rows[0]["id"].ToString();
                    hs["type"] = 2; //全匹配
                    hs["AdminHotelid"] = AdminHotelid;
                    DataFactory.SqlDataBase().InsertByHashtable("Reply_user", hs);
                }
                else
                {
                    Hashtable hs = new Hashtable();
                    hs["Reply_id"] = ds.Rows[0]["Reply_id"].ToString();
                    hs["ReplyList_id"] = ds.Rows[0]["id"].ToString();
                    hs["type"] = 1; //模糊匹配
                    hs["AdminHotelid"] = AdminHotelid;
                    DataFactory.SqlDataBase().InsertByHashtable("Reply_user", hs);
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select id,type,media_id,content,photo from Reply_news where 1=1 and AdminHotelid='{0}'", AdminHotelid);

                DataTable dss = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
                if (dss != null && dss.Rows.Count > 0)
                {
                    if (dss.Rows[0]["type"].ToString() == "1")
                    {  //文字回复
                        resxml = text_xml(dss.Rows[0]["content"].ToString(), ToUserName, FromUserName, CreateTime);
                    }
                    else if (dss.Rows[0]["type"].ToString() == "2") //图片回复
                    {
                        resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + dss.Rows[0]["media_id"].ToString() + "]]></MediaId></Image></xml> ";
                    }
                }
            }
            return resxml;
        }

        #endregion

        #region **关注智订云推送信息**
        /// <summary>
        /// 关注智订云推送信息
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <param name="FromUserName"></param>
        /// <param name="CreateTime"></param>
        /// <returns></returns>
        public static string ZDY_Reply(string ToUserName, string FromUserName, string CreateTime)
        {
            string url = "http://www.zidinn.com";
            string resxml = "";
            resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles><item><Title><![CDATA[了解“智订云商户平台”]]></Title><Description><![CDATA[为酒店商户提供贴心服务，助你轻松玩转智订云]]></Description><PicUrl><![CDATA[" + url + "/App_Themes/default/images/lj.png]]></PicUrl><Url><![CDATA[" + url + "/Melt/ljpt.htm]]></Url></item></Articles></xml> ";
            return resxml;
        }

        #endregion

        #region ** 自主关注 **


        /// <summary>
        /// 自主关注
        /// </summary>
        /// <param name="ToUserName">开发者微信号</param>
        /// <param name="FromUserName">自己的OPENID</param>
        /// <param name="CreateTime">消息创建时间 （整型）</param>
        public static string AutonomyFollow(string ToUserName, string FromUserName, string CreateTime)
        {
            string resxml = "";
            string context = "欢迎关注！";
            DataTable dt = ApplicationHelper.GetHotelTweetsInfo(ToUserName.Trim());
            if (dt != null && dt.Rows.Count > 0)
            {
                string AdminHotelid = dt.Rows[0]["AdminHotelid"].ToString();
                string url = "http://www.zidinn.com";

                //判断是否成为会员
                bool isVip = DataFactory.SqlDataBase().IsExist("MemberInfo", "Openid", FromUserName) > 0;

                if (isVip)
                {
                    //推送文字订房、充值
                    string reservation = url + "/Reservation/HotelList.aspx?AdminHotelid=" + AdminHotelid;
                    context = @"尊贵的会员，快快点击“<a href='" + reservation + "'>订房</a>”进行客房预订吧！";
                    resxml = text_xml(context, ToUserName, FromUserName, CreateTime);
                }
                else
                {
                    string picUrl = url + "/Marketing/images/fuli.jpg";
                    string images = dt.Rows[0]["images"].ToString();
                    if (images != "")
                    {
                        picUrl = url + "/upload/photo/SN" + images;
                    }
                    string wyrul = url + "/Reservation/HotelList.aspx?AdminHotelid=" + AdminHotelid;
                    context = "欢迎关注" + dt.Rows[0]["Name"] + "！";
                    string contexts = "倾听感动，分享喜悦，“" + dt.Rows[0]["Name"] + "微管家”与您24小时贴身相伴。立即点击预订吧！";
                    //自定义关注图文推送
                    try
                    {
                        contexts = dt.Rows[0]["content"].ToString() == "" ? contexts : dt.Rows[0]["content"].ToString();
                        context = dt.Rows[0]["bt"].ToString() == "" ? context : dt.Rows[0]["bt"].ToString();
                        wyrul = dt.Rows[0]["url"].ToString() == "" ? wyrul : dt.Rows[0]["url"].ToString();
                        picUrl = dt.Rows[0]["photo"].ToString() == "" ? picUrl : url + "/upload/Reply/" + dt.Rows[0]["photo"].ToString();
                    }
                    catch { }
                    resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles><item><Title><![CDATA[" + context + "]]></Title><Description><![CDATA[" + contexts + "]]></Description><PicUrl><![CDATA[" + picUrl + "]]></PicUrl><Url><![CDATA[" + wyrul + "]]></Url></item></Articles></xml> ";
                }
            }
            return resxml;
        }

        #endregion

        #region**扫描员工二维码回复事件**

        /// <summary>
        /// 扫描员工二维码回复事件
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <param name="Phone"></param>
        /// <param name="FromUserName"></param>
        /// <param name="CreateTime"></param>
        /// <returns></returns>
        public static string ScanUserCode(string openid, string user_ID, string source)
        {
            string resxml = "";

            //添加扫码记录 判断是永久还是临时
            MemberSource(openid, user_ID, source);

            return resxml;
        }


        /// <summary>
        /// 添加扫码记录 
        /// </summary>
        /// <param name="AdminHotelid"></param>
        /// <param name="openid"></param>
        /// <param name="fjPhone"></param>
        /// <returns></returns>
        public static void MemberSource(string openid, string user_ID, string Source)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT  a.User_ID ,
                    a.AdminHotelid ,
                    a.User_Account ,
                    a.HotelId ,
                    ISNULL(a.openid, '') openid ,
                    b.name HotelName
            FROM    Base_UserInfo a
                    LEFT JOIN dbo.Hotel b ON a.hotelid = b.ID
            WHERE    a.User_Account = @User_Account
                          OR a.User_ID = @User_ID                     
            ");
            SqlParam[] parm_user = new SqlParam[] { 
                new SqlParam("@User_ID",user_ID),
                new SqlParam("@User_Account",user_ID)
            };
            DataTable dt_user = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm_user);

            if (dt_user != null && dt_user.Rows.Count > 0)
            {
                string AdminHotelid = dt_user.Rows[0]["AdminHotelid"].ToString();
                string userid = dt_user.Rows[0]["User_ID"].ToString();
                string User_Account = dt_user.Rows[0]["User_Account"].ToString();
                string HotelId = dt_user.Rows[0]["HotelId"].ToString();
                string userOpenid = dt_user.Rows[0]["openid"].ToString();
                string HotelName = dt_user.Rows[0]["HotelName"].ToString();

                Hashtable Temporaryht = new Hashtable();
                Temporaryht["name"] = "";
                Temporaryht["Phone"] = "";
                Temporaryht["openid"] = openid;
                Temporaryht["AdminHotelid"] = AdminHotelid;
                Temporaryht["HotelId"] = HotelId;
                Temporaryht["HotelName"] = HotelName;
                Temporaryht["TGType"] = "2";
                Temporaryht["TGMember"] = userid;
                Temporaryht["fjPhone"] = User_Account;
                Temporaryht["Source"] = Source;
                Temporaryht["AddTime"] = DateTime.Now.AddSeconds(-30);
                int w = DataFactory.SqlDataBase().InsertByHashtable("MemberSource", Temporaryht);
            }
        }


        /// <summary>
        /// 扫码成功 发送员工通知
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="userid"></param>
        /// <param name="userOpenid"></param>
        /// <param name="openid"></param>
        /// <param name="isVip"></param>
        private static void RemindStaff(string hotelId, string userid, string userOpenid, string openid, bool isVip)
        {
            bool isOthers = false;

            //查询客人第一次扫描的员工id
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT TOP 1 TGMember
            FROM    MemberSource
            WHERE   HotelId = @HotelId
                    AND openid = @openid
                    AND LEN(TGMember) > 4
            ORDER BY AddTime ASC
            ");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@HotelId",hotelId),
                new SqlParam("@openid",openid)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (userid != dt.Rows[0]["TGMember"].ToString())
                {
                    isOthers = true;
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> csInfo = new Dictionary<string, object>();

            csInfo.Add("touser", userOpenid);//微信id
            csInfo.Add("template_id", ApplicationHelper.GetAdminTemplateId("关注成功通知"));//推送模板id
            string url = "";
            string title = "恭喜您又有一位客人扫描您的推广二维码！";
            string remark = "";

            if (isVip)//客人是国光会员
            {
                if (isOthers)//之前已经扫过他人推广码
                {
                    remark = "客人已与他人绑定关系，客人本次微网消费您将获得奖金";
                }
                else//未扫过他人推广码
                {
                    remark = "客人已和您绑定永久关系";
                }
            }
            else//还未注册未会员
            {
                if (isOthers)//之前已经扫过他人推广码
                {
                    remark = "客人已与他人绑定关系，客人本次微网消费您将获得奖金";
                }
                else//未扫过他人推广码
                {
                    //remark = "客人还未注册，请提醒客人注册会员，才能与您绑定永久关系";
                    remark = "客人还未注册，请提醒客人注册会员，才能查看订单详情及获得积分";
                }
            }

            csInfo.Add("url", url);//点击跳转地址

            #region ****** 参数信息 ******

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("first", new Dictionary<string, object>
                    {
                        { "value", title},
                        { "color", "#000" }
                    });
            //
            data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value", "******"},
                        { "color", "#000" }
                    });
            //
            data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value", DateTime.Now.ToString("yyyy-MM-dd HH:mm")},
                        { "color", "#000" }
                    });

            data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", remark},
                        { "color", "#000" }
                    });
            #endregion
            csInfo.Add("data", data);
            string menuInfo = serializer.Serialize(csInfo);
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken("1"));
            TemplateMessage.PostWebRequest(postUrl, menuInfo);
        }


        #endregion

        #region **扫描酒店/门店二维码回复事件**

        /// <summary>
        /// 扫描酒店/门店二维码回复事件
        /// </summary>
        /// <param name="ToUserName">开发者微信号</param>
        /// <param name="Phone">手机号码</param>
        /// <param name="FromUserName">自己的OPENID</param>
        /// <param name="CreateTime">消息创建时间 （整型）</param>
        public static string ScanHotelCode(string ToUserName, string Hotelid, string FromUserName, string CreateTime)
        {
            string resxml = "";
            //添加扫码记录 
            HotelSource(FromUserName, Hotelid);
            return resxml;
        }
        /// <summary>
        /// 添加扫酒店二维码记录
        /// </summary>
        /// <param name="AdminHotelid"></param>
        /// <param name="openid"></param>
        /// <param name="fjPhone"></param>
        /// <returns></returns>
        public static void HotelSource(string openid, string Hotelid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT AdminHotelid,name FROM Hotel WHERE ID=@ID");
            SqlParam[] parm_hotel = new SqlParam[] {
                new SqlParam("@ID",Hotelid)
            };
            DataTable dt_hotel = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm_hotel);
            if (dt_hotel != null && dt_hotel.Rows.Count > 0)
            {
                string AdminHotelid = dt_hotel.Rows[0]["AdminHotelid"].ToString();
                string name = dt_hotel.Rows[0]["name"].ToString();
                Hashtable Temporaryht = new Hashtable();
                Temporaryht["name"] = "";
                Temporaryht["Phone"] = "";
                Temporaryht["openid"] = openid;
                Temporaryht["AdminHotelid"] = AdminHotelid;
                Temporaryht["HotelId"] = Hotelid;
                Temporaryht["HotelName"] = name;
                Temporaryht["TGType"] = "1";
                Temporaryht["TGMember"] = "";
                Temporaryht["fjPhone"] = "";
                Temporaryht["Source"] = "扫酒店二维码[第三方]";
                Temporaryht["AddTime"] = DateTime.Now.AddSeconds(-30);
                DataFactory.SqlDataBase().InsertByHashtable("MemberSource", Temporaryht);
            }
        }

        #endregion

        #region **扫描 押金 二维码回复事件**

        /// <summary>
        /// 扫描 押金二维码回复事件
        /// </summary>
        /// <param name="ToUserName">开发者微信号</param>
        /// <param name="Phone">手机号码</param>
        /// <param name="FromUserName">自己的OPENID</param>
        /// <param name="CreateTime">消息创建时间 （整型）</param>
        public static string ScanPledgeMoneyCode(string EventName, string ToUserName, string FromUserName, string CreateTime)
        {
            string resxml = "";
            string context = "欢迎关注！";

            DataTable dt = ApplicationHelper.GetHotelTweetsInfo(ToUserName.Trim());
            if (dt != null && dt.Rows.Count > 0)
            {
                string AdminHotelid = dt.Rows[0]["AdminHotelid"].ToString();
                string url = "http://www.zidinn.com";

                //添加扫码记录 
                PledgeMoneySource(AdminHotelid, FromUserName);
                if (EventName == "subscribe")
                {
                    SendNews(AdminHotelid, ToUserName, FromUserName);
                }

                string reservation = url + "/Reservation/Deposit.aspx?AdminHotelid=" + AdminHotelid;
                context = @"<a href='" + reservation + "'>前往微信付押金</a>";
                resxml = text_xml(context, ToUserName, FromUserName, CreateTime);
            }
            return resxml;
        }
        /// <summary>
        /// 添加扫押金二维码记录
        /// </summary>
        /// <param name="AdminHotelid"></param>
        /// <param name="openid"></param>
        /// <param name="fjPhone"></param>
        /// <returns></returns>
        public static void PledgeMoneySource(string AdminHotelid, string openid)
        {
            Hashtable Temporaryht = new Hashtable();
            Temporaryht["name"] = "";
            Temporaryht["Phone"] = "";
            Temporaryht["openid"] = openid;
            Temporaryht["AdminHotelid"] = AdminHotelid;
            Temporaryht["HotelId"] = 0;
            Temporaryht["HotelName"] = "";
            Temporaryht["TGType"] = "3";
            Temporaryht["TGMember"] = "";
            Temporaryht["fjPhone"] = "";
            Temporaryht["Source"] = "扫押金二维码";
            Temporaryht["AddTime"] = DateTime.Now;
            DataFactory.SqlDataBase().InsertByHashtable("MemberSource", Temporaryht);
        }

        /// <summary>
        /// 发送客服图片消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void SendNews(string AdminHotelid, string ToUserName, string FromUserName)
        {
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
            postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken(AdminHotelid));
            string menuInfo = SendNewsContent(AdminHotelid, ToUserName, FromUserName);

            if (menuInfo != "")
            {
                string sqljosn = TemplateMessage.PostWebRequest(postUrl, menuInfo);
                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(sqljosn);
            }
        }

        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <returns></returns>
        public static string SendNewsContent(string AdminHotelid, string ToUserName, string FromUserName)
        {
            string context = "欢迎关注！";
            DataTable dt = ApplicationHelper.GetHotelTweetsInfo(ToUserName.Trim());
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                string url = ConfigHelper.GetAppSettings("Url");
                string picUrl = url + "/Marketing/images/fuli.jpg";
                string images = dt.Rows[0]["images"].ToString();
                if (images != "")
                {
                    picUrl = url + "/upload/photo/SN" + images;
                }
                string wyrul = url + "/Reservation/HotelList.aspx?AdminHotelid=" + AdminHotelid;
                context = "欢迎关注" + dt.Rows[0]["Name"] + "！";
                string contexts = "倾听感动，分享喜悦，“" + dt.Rows[0]["Name"] + "微管家”与您24小时贴身相伴。立即点击预订吧！";

                //自定义关注图文推送
                try
                {
                    contexts = dt.Rows[0]["content"].ToString() == "" ? contexts : dt.Rows[0]["content"].ToString();
                    context = dt.Rows[0]["bt"].ToString() == "" ? context : dt.Rows[0]["bt"].ToString();
                    wyrul = dt.Rows[0]["url"].ToString() == "" ? wyrul : dt.Rows[0]["url"].ToString();
                    picUrl = dt.Rows[0]["photo"].ToString() == "" ? picUrl : url + "/upload/Reply/" + dt.Rows[0]["photo"].ToString();
                }
                catch { }

                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", FromUserName); //用户openid
                sb.AppendFormat("\"msgtype\":\"{0}\",", "news");//	发送文本格式
                sb.Append("\"news\":{");
                sb.Append("\"articles\":[{"); //文字描述
                sb.AppendFormat("\"title\":\"{0}\",", context); //标题
                sb.AppendFormat("\"description\":\"{0}\",", contexts); //文字描述
                sb.AppendFormat("\"url\":\"{0}\",", wyrul); //跳转链接路径
                sb.AppendFormat("\"picurl\":\"{0}\"", picUrl); //图片路径
                sb.Append("}]");
                sb.Append("}");
                sb.Append("}");
            }
            return sb.ToString();

        }


        #endregion

    }
}