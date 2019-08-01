using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using System.Text;
using RM.Common.DotNetConfig;
using System.Collections;
using RM.Web.Lib;
using RM.Web.App_Code;
using System.Web.Script.Serialization;
using RM.Web.business;

namespace RM.Web.WX_SET
{
    public static class Reply
    {
        /// <summary>
        /// 智订云商户平台
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <param name="FromUserName"></param>
        /// <param name="CreateTime"></param>
        /// <returns></returns>
        public static string Automatic_SewaFocus(string ToUserName, string FromUserName, string CreateTime)
        {
            string url = ConfigHelper.GetAppSettings("Url");
            string resxml = "";
            //resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles><item><Title><![CDATA[欢迎关注升维商户平台]]></Title><Description><![CDATA[点击进入营销中心，获取专属推广二维码，了解营销政策，并查看获奖记录或领取奖金。]]></Description><PicUrl><![CDATA[" + url + "/App_Themes/default/images/tuisong.jpg]]></PicUrl><Url><![CDATA[" + url + "/Melt/Sale_bonus.aspx]]></Url></item></Articles></xml> ";
            resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles><item><Title><![CDATA[了解“智订云商户平台”]]></Title><Description><![CDATA[为酒店商户提供贴心服务，助你轻松玩转智订云]]></Description><PicUrl><![CDATA[" + url + "/App_Themes/default/images/lj.png]]></PicUrl><Url><![CDATA[" + url + "/Melt/ljpt.htm]]></Url></item></Articles></xml> ";
            return resxml;
        }



        /// <summary>
        /// 公众号关注自动回复事件
        /// </summary>
        /// <param name="ToUserName">开发者微信号</param>
        /// <param name="FromUserName">自己的OPENID</param>
        /// <param name="CreateTime">消息创建时间 （整型）</param>
        public static string Automatic_Focus(string ToUserName, string FromUserName, string CreateTime)
        {
            string resxml = "";
            string context = "欢迎关注！";
            resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + context + "]]></Content></xml> ";

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT  w.AdminHotelid ,
                    h.Name ,
                    h.type ,
                    ISNULL(( SELECT TOP 1
                                    ImgFile
                             FROM   Photo
                             WHERE  [type] = '9'
                                    AND AdminHotelid = h.AdminHotelid
                             ORDER BY hotelid,ID ASC
                           ), '') images,
                           r.content,
                           r.photo,
                           r.bt,
                           r.url
            FROM    dbo.WeChatInfo w
                    INNER JOIN dbo.Hotel_Admin h ON w.AdminHotelid = h.AdminHotelid
                    LEFT JOIN Reply_attention r ON r.AdminHotelid = h.AdminHotelid 
            WHERE   w.Original_ID = @Original_ID
                    AND h.AdminHotelid <> '1'
            ");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@Original_ID",ToUserName.Trim() )
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                string AdminHotelid = dt.Rows[0]["AdminHotelid"].ToString();
                string url = HttpContext.Current.Request.Url.ToString();
                url = url.Substring(0, url.LastIndexOf("/"));

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
            return resxml;
        }



        public static string HotelImage(string AdminHotelid)
        {
            string Image = "";
            string sql = string.Format(@"SELECT ID FROM Hotel WHERE AdminHotelid='{0}'", AdminHotelid);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            if (ds != null && ds.Rows.Count > 0)
            {
                string sqls = string.Format(@"select ID, ImgFile from Photo where PID='{0}' and [type]='9' and AdminHotelid='{1}' order by ID asc", ds.Rows[0]["ID"].ToString(), AdminHotelid);
                DataTable dss = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls));
                if (dss != null && dss.Rows.Count > 0)
                {
                    Image = dss.Rows[0]["ImgFile"].ToString();
                }
            }
            return Image;
        }
        /// <summary>
        /// 扫描员工二维码回复事件
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <param name="Phone"></param>
        /// <param name="FromUserName"></param>
        /// <param name="CreateTime"></param>
        /// <returns></returns>
        public static string ScanUserCode(string ToUserName, string Phone, string FromUserName, string CreateTime)
        {
            string resxml = "";
            string context = "欢迎关注！";
            resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + context + "]]></Content></xml> ";

            StringBuilder sb = new StringBuilder();
//            sb.Append(@"
//            SELECT  w.AdminHotelid ,
//                    h.Name ,
//                    h.type ,
//                    ISNULL(( SELECT TOP 1
//                                    ImgFile
//                             FROM   Photo
//                             WHERE  [type] = '9'
//                                    AND AdminHotelid = h.AdminHotelid
//                             ORDER BY hotelid,ID ASC
//                           ), '') images
//            FROM    dbo.WeChatInfo w
//                    INNER JOIN dbo.Hotel_Admin h ON w.AdminHotelid = h.AdminHotelid
//            WHERE   w.Original_ID = @Original_ID
//                    AND h.AdminHotelid <> '1'
//            ");
            sb.Append(@"
            SELECT  w.AdminHotelid ,
                    h.Name ,
                    h.type ,
                    ISNULL(( SELECT TOP 1
                                    ImgFile
                             FROM   Photo
                             WHERE  [type] = '9'
                                    AND AdminHotelid = h.AdminHotelid
                             ORDER BY hotelid,ID ASC
                           ), '') images,
                           r.content,
                           r.photo,
                           r.bt,
                           r.url
            FROM    dbo.WeChatInfo w
                    INNER JOIN dbo.Hotel_Admin h ON w.AdminHotelid = h.AdminHotelid
                    LEFT JOIN Reply_attention r ON r.AdminHotelid = h.AdminHotelid 
            WHERE   w.Original_ID = @Original_ID
                    AND h.AdminHotelid <> '1'
            ");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@Original_ID",ToUserName.Trim() )
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                string AdminHotelid = dt.Rows[0]["AdminHotelid"].ToString();
                string url = HttpContext.Current.Request.Url.ToString();
                url = url.Substring(0, url.LastIndexOf("/"));

                //判断是否成为会员
                bool isVip = DataFactory.SqlDataBase().IsExist("MemberInfo", "Openid", FromUserName) > 0;

                //添加扫码记录 判断是永久还是临时
                CommonMethod.MemberSource(AdminHotelid, FromUserName, Phone, isVip);
                //分销员二维码
                CommonMethod.D_MemberSource(AdminHotelid, FromUserName, Phone, isVip);
                if (isVip)
                {
                    //推送文字订房、充值
                    string reservation = url + "/Reservation/HotelList.aspx?AdminHotelid=" + AdminHotelid;
                    context = @"尊贵的会员，快快点击“<a href='" + reservation + "'>订房</a>”进行客房预订吧！";
                    resxml = automatic_return(context, ToUserName, FromUserName, CreateTime);
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



        /// <summary>
        /// 未关注执行事件：客户扫码酒店推广二维码
        /// </summary>
        /// <param name="ToUserName">开发者微信号</param>
        /// <param name="Phone">手机号码</param>
        /// <param name="FromUserName">自己的OPENID</param>
        /// <param name="CreateTime">消息创建时间 （整型）</param>
        public static string UnHotelFocusWelfare(string ToUserName, string Hotelid, string FromUserName, string CreateTime)
        {
            string resxml = "";
            string context = "欢迎关注公众号！";
            resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + context + "]]></Content></xml> ";

            string sql = string.Format(@"SELECT  w.AdminHotelid,h.Name,h.type FROM dbo.WeChatInfo w INNER JOIN dbo.Hotel_Admin h ON w.AdminHotelid=h.AdminHotelid where  w.Original_ID=@Original_ID and h.AdminHotelid<>'1' ");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@Original_ID",ToUserName.Trim() )};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (dt != null && dt.Rows.Count > 0)
            {
                //建立关系
                string AdminHotelid = dt.Rows[0]["AdminHotelid"].ToString();
                Log.Info("未关注(建立关系)：", AdminHotelid);


                //推送酒店信息
                string url = ConfigHelper.GetAppSettings("Url");
                string tpurl = url + "/Marketing/images/fuli.jpg";
                string wyrul = url + "/Marketing/register.aspx?AdminHotelid=" + AdminHotelid;

                //判断是否成为会员
                bool isVip = false;
                //添加扫码记录
                CommonMethod.HotelSource(AdminHotelid, FromUserName, Hotelid, out isVip);
                if (isVip)
                {
                    //推送文字订房、充值
                    string reservation = url + "/Reservation/HotelList.aspx?AdminHotelid=" + AdminHotelid;

                    string recharge = url + "/Vipcard/MemCart.aspx?AdminHotelid=" + AdminHotelid;
                    context = @"尊贵的会员，快快点击“<a href='" + reservation + "'>订房</a>”进行客房预订吧！";
                    resxml = automatic_return(context, ToUserName, FromUserName, CreateTime);

                }
                else
                {
                    //推送福利消息
                    context = "欢迎关注" + dt.Rows[0]["Name"] + "，点击领取福利";
                    string contextes = "欢迎关注" + dt.Rows[0]["Name"] + "，点击进去领取福利";
                    resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles><item><Title><![CDATA[" + context + "]]></Title><Description><![CDATA[" + contextes + "]]></Description><PicUrl><![CDATA[" + tpurl + "]]></PicUrl><Url><![CDATA[" + wyrul + "]]></Url></item></Articles></xml> ";


                    ////判断是否开启活动  Wx_function(功能开启设置表)
                    string sqls4 = string.Format(@"select Wx_Marketing from Wx_function where Wx_Marketing=1 and AdminHotelid=@AdminHotelid");
                    SqlParam[] parmAdd4 = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", dt.Rows[0]["AdminHotelid"])       };
                    DataTable dt4 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls4), parmAdd4);
                    if (dt4 != null && dt4.Rows.Count > 0)
                    {
                        context = "欢迎关注" + dt.Rows[0]["Name"] + "！";
                        string contexts = "欢迎关注" + dt.Rows[0]["Name"] + "，点击进去领取福利";
                        resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles><item><Title><![CDATA[" + context + "]]></Title><Description><![CDATA[" + contexts + "]]></Description><PicUrl><![CDATA[" + tpurl + "]]></PicUrl><Url><![CDATA[" + wyrul + "]]></Url></item></Articles></xml> ";

                    }
                    else
                    {
                        resxml = Automatic_Focus(ToUserName, FromUserName, CreateTime);
                    }
                }

            }

            return resxml;
        }


        /// <summary>
        /// 已关注执行事件：客户扫码酒店推广二维码
        /// </summary>
        /// <param name="ToUserName">开发者微信号</param>
        /// <param name="Phone">手机号码</param>
        /// <param name="FromUserName">自己的OPENID</param>
        /// <param name="CreateTime">消息创建时间 （整型）</param>
        public static string HotelFocusWelfare(string ToUserName, string Hotelid, string FromUserName, string CreateTime)
        {
            string url = ConfigHelper.GetAppSettings("Url");
            string resxml = "";
            string context = "欢迎关注！";
            resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + context + "]]></Content></xml> ";

            string sql = string.Format(@"SELECT  w.AdminHotelid,h.type, h.Name FROM dbo.WeChatInfo w INNER JOIN dbo.Hotel_Admin h ON w.AdminHotelid=h.AdminHotelid where  w.Original_ID=@Original_ID and h.AdminHotelid<>'1'");
            SqlParam[] parmAdd2 = new SqlParam[] { 
                                     new SqlParam("@Original_ID",ToUserName.Trim() )};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd2);
            if (dt != null && dt.Rows.Count > 0)
            {
                string AdminHotelid = dt.Rows[0]["AdminHotelid"].ToString();
                string tpurl = url + "/Marketing/images/fuli.jpg";
                string wyrul = url + "/Marketing/register.aspx?AdminHotelid=" + dt.Rows[0]["AdminHotelid"];
                context = "欢迎关注" + dt.Rows[0]["Name"] + "！";
                resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + context + "]]></Content></xml> ";


                ////判断是否开启活动  Wx_function(功能开启设置表)
                string sqls4 = string.Format(@"select Wx_Marketing from Wx_function where Wx_Marketing=1 and AdminHotelid=@AdminHotelid");
                SqlParam[] parmAdd4 = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", dt.Rows[0]["AdminHotelid"])       };
                DataTable dt4 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls4), parmAdd4);
                if (dt4 != null && dt4.Rows.Count > 0)
                {
                    context = "欢迎关注" + dt.Rows[0]["Name"] + "！";
                    string contexts = "欢迎关注" + dt.Rows[0]["Name"] + "，点击进去领取福利";
                    resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles><item><Title><![CDATA[" + context + "]]></Title><Description><![CDATA[" + contexts + "]]></Description><PicUrl><![CDATA[" + tpurl + "]]></PicUrl><Url><![CDATA[" + wyrul + "]]></Url></item></Articles></xml> ";

                }
                else
                {
                    resxml = Automatic_Focus(ToUserName, FromUserName, CreateTime);
                }

                Log.Info("已关注(添加扫码记录 判断是永久还是临时)：", FromUserName);
                //添加扫码记录 判断是永久还是临时

                //判断是否成为会员
                bool isVip = false;
                //添加扫码记录 判断是永久还是临时
                CommonMethod.HotelSource(AdminHotelid, FromUserName, Hotelid, out isVip);
                if (isVip)
                {
                    //推送文字订房、充值
                    string reservation = url + "/Reservation/HotelList.aspx?AdminHotelid=" + AdminHotelid;

                    //string recharge = url + "/Vipcard/MemCart.aspx?AdminHotelid=" + AdminHotelid;//充值链接
                    context = @"尊贵的会员，快快点击“<a href='" + reservation + "'>订房</a>”进行客房预订吧！";
                    resxml = automatic_return(context, ToUserName, FromUserName, CreateTime);
                }

                Log.Info("已关注(完成----------)：", FromUserName);

            }

            return resxml;
        }


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

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT  w.AdminHotelid ,
                    h.Name ,
                    h.type ,
                    ISNULL(( SELECT TOP 1
                                    ImgFile
                             FROM   Photo
                             WHERE  [type] = '9'
                                    AND AdminHotelid = h.AdminHotelid
                             ORDER BY hotelid,ID ASC
                           ), '') images,
                           r.content,
                           r.photo,
                           r.bt,
                           r.url
            FROM    dbo.WeChatInfo w
                    INNER JOIN dbo.Hotel_Admin h ON w.AdminHotelid = h.AdminHotelid
                    LEFT JOIN Reply_attention r ON r.AdminHotelid = h.AdminHotelid 
            WHERE   w.Original_ID = @Original_ID
                    AND h.AdminHotelid <> '1'
            ");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@Original_ID",ToUserName.Trim() )
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
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
                    try {
                        contexts = dt.Rows[0]["content"].ToString() == "" ? contexts : dt.Rows[0]["content"].ToString();
                        context = dt.Rows[0]["bt"].ToString() == "" ? context : dt.Rows[0]["bt"].ToString();
                        wyrul = dt.Rows[0]["url"].ToString() == "" ? wyrul : dt.Rows[0]["url"].ToString();
                        picUrl = dt.Rows[0]["photo"].ToString() == "" ? picUrl : url+"/upload/Reply/" + dt.Rows[0]["photo"].ToString();
                    }
                    catch { }
                    

                    resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles><item><Title><![CDATA[" + context + "]]></Title><Description><![CDATA[" + contexts + "]]></Description><PicUrl><![CDATA[" + picUrl + "]]></PicUrl><Url><![CDATA[" + wyrul + "]]></Url></item></Articles></xml> ";
                }
            }
            return resxml;
        }

        /// <summary>
        /// 拼接text内容XML
        /// </summary>
        /// <param name="context">内容</param>
        /// <param name="ToUserName">开发者微信号</param>
        /// <param name="FromUserName">自己的OPENID</param>
        /// <param name="CreateTime">消息创建时间 （整型）</param>
        /// <returns></returns>
        public static string text_xml(string context, string ToUserName, string FromUserName, string CreateTime)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + context + "]]></Content></xml> ");
            return sb.ToString();
        }
        ///// <summary>
        ///// 已关注执行事件：客户扫码员工推广二维码
        ///// </summary>
        ///// <param name="ToUserName">开发者微信号</param>
        ///// <param name="Phone">手机号码</param>
        ///// <param name="FromUserName">自己的OPENID</param>
        ///// <param name="CreateTime">消息创建时间 （整型）</param>
        //public static string FocusWelfare(string ToUserName, string Phone, string FromUserName, string CreateTime)
        //{
        //    string url = ConfigHelper.GetAppSettings("Url");
        //    string resxml = "";
        //    string context = "欢迎关注！";

        //    string sql = string.Format(@"SELECT  w.AdminHotelid,h.type, h.Name FROM dbo.WeChatInfo w INNER JOIN dbo.Hotel_Admin h ON w.AdminHotelid=h.AdminHotelid where  w.Original_ID=@Original_ID and h.AdminHotelid<>'1'");
        //    SqlParam[] parmAdd2 = new SqlParam[] { 
        //                             new SqlParam("@Original_ID",ToUserName.Trim() )};
        //    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd2);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        string AdminHotelid = dt.Rows[0]["AdminHotelid"].ToString();
        //        //推送文字订房、充值
        //        string reservation = url + "/Reservation/HotelList.aspx?AdminHotelid=" + AdminHotelid;
        //        string zc = url + "/Members/Register.aspx?AdminHotelid=" + AdminHotelid;//注册
        //        context = @"尊贵的客人，快快点击“<a href='" + reservation + "'>订房</a>”进行客房预订吧！点击“<a href='" + zc + "'>注册</a>”会员可享受专属会员权益哦！";
        //        resxml = automatic_return(context, ToUserName, FromUserName, CreateTime);

        //        bool isVip = false;
        //        //建立关系
        //        //添加扫码记录 判断是永久还是临时
        //        CommonMethod.MemberSource(AdminHotelid, FromUserName, Phone, out isVip);

        //    }
        //    return resxml;
        //}

        /// <summary>
        /// 拼接XML
        /// </summary>
        /// <param name="context">内容</param>
        /// <param name="ToUserName">开发者微信号</param>
        /// <param name="FromUserName">自己的OPENID</param>
        /// <param name="CreateTime">消息创建时间 （整型）</param>
        /// <returns></returns>
        public static string automatic_return(string context, string ToUserName, string FromUserName, string CreateTime)
        {
            string resxml = "";
            resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + context + "]]></Content></xml> ";
            return resxml;
        }

        /// <summary>
        /// 文字回复
        /// </summary>
        /// <param name="context">回复内容</param>
        /// <param name="ToUserName">开发者微信号</param>
        /// <param name="FromUserName">自己的OPENID</param>
        /// <param name="CreateTime">消息创建时间 （整型）</param>
        /// <param name="Adminhotelid">酒店ID</param>
        public static string automatic_hf(string context, string ToUserName, string FromUserName, string CreateTime, string Adminhotelid)
        {
            string resxml = "";

            if (context == "")
            {
                //Reply_attention(微信首次关注回复表)
                string sqls = string.Format(@"select id,type,media_id,content,photo from Reply_attention where 1=1 and Adminhotelid='{0}'", Adminhotelid);
                DataTable dss = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls));
                if (dss != null && dss.Rows.Count > 0)
                {
                    if (dss.Rows[0]["type"].ToString() == "1")
                    {  //文字回复
                        resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + dss.Rows[0]["content"].ToString() + "]]></Content></xml> ";
                    }
                    else if (dss.Rows[0]["type"].ToString() == "2") //图片回复
                    {
                        resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + dss.Rows[0]["media_id"].ToString() + "]]></MediaId></Image></xml> ";
                    }
                    return resxml;
                }
            }
            resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + context + "]]></Content></xml> ";

            return resxml;

        }


        /// <summary>
        /// 账单扫码关注事件
        /// </summary>
        /// <param name="ToUserName">开发者微信号</param>
        /// <param name="EventKey">父级参数openid，扫描的用户</param>
        /// <param name="FromUserName">自己的OPENID</param>
        /// <param name="CreateTime">消息创建时间 （整型）</param>
        /// <param name="Adminhotelid">酒店ID</param>
        /// <returns></returns>
        public static string zd_subscribe(string ToUserName, string EventKey, string FromUserName, string CreateTime, string Adminhotelid)
        {
            string resxml = "";

            string sql1 = string.Format(@"select 1 from SY_KRXFMXB where zh=@zh");
            SqlParam[] param = new SqlParam[] { 
                           new SqlParam("@zh", EventKey)};
            DataTable ds1 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql1), param);
            if (ds1 != null && ds1.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"SELECT id,ggddh,code_url from Scanning_code where ggddh=@zh and ISscanning=0");
                SqlParam[] param1 = new SqlParam[] { 
                           new SqlParam("@zh", EventKey)};
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param1);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string context = cj(FromUserName, EventKey, Adminhotelid);
                    automatic_hf(context, ToUserName, FromUserName, CreateTime, Adminhotelid);
                }
                else
                {
                    string context = "该二维码已经扫描过了！";
                    automatic_hf(context, ToUserName, FromUserName, CreateTime, Adminhotelid);
                }
            }
            else
            {
                string context = "该账单二维码已失效！";
                automatic_hf(context, ToUserName, FromUserName, CreateTime, Adminhotelid);
            }
            return resxml;
        }


        ///// <summary>
        ///// 用户扫码订房事件
        ///// </summary>
        ///// <param name="ToUserName">开发者微信号</param>
        ///// <param name="EventKey">餐厅餐桌ID</param>
        ///// <param name="FromUserName">自己的OPENID</param>
        /////  <param name="FromUserName">消息创建时间（整型）</param>
        ///// <returns></returns>
        //private string Scan_code(string ToUserName, string EventKey, string FromUserName, string CreateTime, string type)
        //{
        //    string resxml = "";
        //    string id = EventKey;
        //    string sql = string.Format(@"SELECT  id , sort , number ,isdelete , code_img ,  Catering_id FROM WX_board where id='{0}' and isdelete=1 ", id);
        //    DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
        //    if (ds != null && ds.Rows.Count > 0)
        //    {
        //        string context = wxhy(FromUserName, id);
        //        Hashtable hs = new Hashtable();
        //        hs["board_id"] = id;
        //        hs["CateringId"] = ds.Rows[0]["Catering_id"];
        //        DataFactory.SqlDataBase().InsertByHashtable("WX_boardList", hs);

        //        resxml = automatic_hf(context, ToUserName, FromUserName, CreateTime);

        //    }
        //    else
        //    {
        //        resxml = automatic_hf("该二维码已失效", ToUserName, FromUserName, CreateTime);
        //    }
        //    return resxml;
        //}

        /// <summary>
        /// 关键字回复
        /// </summary>
        /// <param name="text">关键字</param>
        /// <param name="ToUserName">开发者微信号</param>
        /// <param name="FromUserName">自己的OPENID</param>
        /// <param name="CreateTime">消息创建时间（整型）</param>
        /// <returns></returns>
        public static string reply(string text, string ToUserName, string FromUserName, string CreateTime)
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
                    resxml = automatic_hf(ds.Rows[0]["content"].ToString(), ToUserName, FromUserName, CreateTime, AdminHotelid);
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
                        resxml = automatic_hf(dss.Rows[0]["content"].ToString(), ToUserName, FromUserName, CreateTime, AdminHotelid);
                    }
                    else if (dss.Rows[0]["type"].ToString() == "2") //图片回复
                    {
                        resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + CreateTime + "</CreateTime><MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + dss.Rows[0]["media_id"].ToString() + "]]></MediaId></Image></xml> ";
                    }
                }

            }

            return resxml;
        }


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




        /// <summary>
        /// 抽奖返回数据
        /// </summary>
        /// <param name="openid">用户微信ID</param>
        /// <returns></returns>
        private static string cj(string openid, string zh, string adminhotelid)
        {
            string context = "";

            try
            {
                string sql = string.Format(@"SELECT Winning_prompt , not_Winning_prompt ,Winning_promptss,rate,sums,num,Draw_num FROM Draw_Code  where isEnble=1 and  AdminHotelid='{0}' ", adminhotelid);
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds != null && ds.Rows.Count > 0)
                {
                    context = ds.Rows[0]["not_Winning_prompt"].ToString();//未中奖提示语

                    int sums = 0; //每人最多允许抽奖总次数
                    int num = 0;  //每天最多抽多少次数
                    int zjcsnum = 0; //每个最多允许中奖次数

                    Random ran = new Random();
                    int RandKey = ran.Next(0, 100);

                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    string dates = DateTime.Now.ToString();

                    sums = Convert.ToInt32(ds.Rows[0]["sums"]);
                    num = Convert.ToInt32(ds.Rows[0]["num"]);
                    zjcsnum = Convert.ToInt32(ds.Rows[0]["Draw_num"]);

                    int sumss = 0;//统计一共抽过多少次
                    string sqls1 = string.Format(@"select count(1) from Prize_draw where openid='{0}' and   AdminHotelid='{1}'", openid, adminhotelid);  //统计历史一共抽过多少次奖
                    DataTable dss1 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls1));
                    if (dss1 != null && dss1.Rows.Count > 0)
                    {
                        sumss = Convert.ToInt32(dss1.Rows[0][0]);
                    }

                    int daysum = 0; //今天抽过多少次
                    string daysql = string.Format(@"select count(1) from Prize_draw where openid='{0}' and CAST(addtime AS DATE)='{1}' and   AdminHotelid='{2}'", openid, date, adminhotelid);  //统计今天一共抽过多少次奖
                    DataTable daydss = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(daysql));
                    if (daydss != null && daydss.Rows.Count > 0)
                    {
                        daysum = Convert.ToInt32(daydss.Rows[0][0]);
                    }

                    if (sums > sumss)  //设置总次数大于历史次数
                    {
                        if (num > daysum)  //设置每天次数大于今天抽奖次数
                        {
                            string sqlzjcs = string.Format(@"select count(1) from Prize_draw where openid='{0}' and results=1 and   AdminHotelid='{1}'", openid, adminhotelid);
                            DataTable zjcsdss = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlzjcs));
                            if (zjcsdss != null && zjcsdss.Rows.Count > 0)
                            {
                                string sqlcj = string.Format(@"update Scanning_code set ISscanning=1 where ggddh='{0}' and   AdminHotelid='{1}'", zh, adminhotelid);
                                DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcj));

                                int zjcs = Convert.ToInt32(zjcsdss.Rows[0][0]);

                                if (zjcsnum > zjcs)  //允许中奖次数
                                {
                                    if (Convert.ToInt32(ds.Rows[0]["rate"]) * 100 > RandKey) //是否中奖
                                    {
                                        string rad = "";
                                        string sqls = string.Format(@"SELECT id,ratess FROM Prize_code WHERE deleteMark=1 and sums>0 and   AdminHotelid='{0}'", adminhotelid);
                                        DataTable dss = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls));
                                        if (dss != null && dss.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dss.Rows.Count; i++)
                                            {
                                                for (int s = 0; s < Convert.ToDouble(dss.Rows[i]["ratess"]) * 100; s++)
                                                {
                                                    rad += dss.Rows[i]["id"] + ",";
                                                }
                                            }

                                            string[] sjrad = rad.Split(',');

                                            for (int i = 0; i <= sjrad.Length - 1; i++)
                                            {
                                                if (RandKey == i)
                                                {
                                                    rad = sjrad[i];
                                                }
                                            }
                                        }

                                        int couponid = 0;
                                        string sqlpr = string.Format(@"select c.ID,c.Par,c.biginTime,c.endinTime ,p.name  from Prize_code p INNER JOIN dbo.Coupon c ON p.couponid=c.id
  WHERE p.deleteMark=1 and p.id='{0}' and p.AdminHotelid='{1}'", rad, adminhotelid);
                                        DataTable dspr = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlpr));
                                        if (dspr != null && dspr.Rows.Count > 0)
                                        {
                                            context = dspr.Rows[0]["name"].ToString();
                                            couponid = Convert.ToInt32(dspr.Rows[0]["id"]);
                                        }

                                        Hashtable hs = new Hashtable();
                                        hs["openid"] = openid;
                                        hs["addtime"] = dates;
                                        hs["results"] = 1;
                                        hs["PrizeId"] = rad;
                                        hs["couponId"] = couponid;
                                        hs["ggddh"] = zh;
                                        hs["AdminHotelid"] = adminhotelid;
                                        DataFactory.SqlDataBase().InsertByHashtable("Prize_draw", hs);

                                        string sqlup = string.Format(@"update Prize_code set sums=sums-1 where id='{0}' and  AdminHotelid='{1}'", rad, adminhotelid);
                                        DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(sqlup));

                                        string sqlwx = string.Format(@"select lsh from hy_hyzlxxb where carid='{0}'", openid);
                                        DataTable dswx = DataFactory.SqlDataBase(adminhotelid).GetDataTableBySQL(new StringBuilder(sqlwx));
                                        if (dswx != null && dswx.Rows.Count > 0)
                                        {

                                            if (dspr != null && dspr.Rows.Count > 0)
                                            {
                                                Hashtable hss = new Hashtable();
                                                hss["Clientid"] = dswx.Rows[0]["lsh"];
                                                hss["CouponID"] = dspr.Rows[0]["ID"];
                                                hss["Par"] = dspr.Rows[0]["Par"];
                                                hss["isDelete"] = 0;
                                                hss["isReceive"] = 0;
                                                hss["biginTime"] = dspr.Rows[0]["biginTime"];
                                                hss["endinTime"] = dspr.Rows[0]["endinTime"];
                                                hss["type"] = "抽奖赠券";
                                                hss["AdminHotelid"] = adminhotelid;
                                                DataFactory.SqlDataBase().InsertByHashtable("ClientCoupon", hss);
                                            }
                                        }
                                        string url = ConfigHelper.GetAppSettings("Url") + "/Members/MemberCantre.aspx?AdminHotelid=" + adminhotelid;
                                        context = "<a href='" + url + "'>" + ds.Rows[0]["Winning_prompt"].ToString() + "获得" + context + "点击领取</a>";//未中奖提示语
                                    }
                                    else
                                    {
                                        Hashtable hs = new Hashtable();
                                        hs["openid"] = openid;
                                        hs["addtime"] = dates;
                                        hs["results"] = 0;
                                        hs["PrizeId"] = 0;
                                        hs["ggddh"] = zh;
                                        hs["AdminHotelid"] = adminhotelid;
                                        DataFactory.SqlDataBase().InsertByHashtable("Prize_draw", hs);

                                    }
                                }
                                else
                                {
                                    Hashtable hs = new Hashtable();
                                    hs["openid"] = openid;
                                    hs["addtime"] = dates;
                                    hs["results"] = 0;
                                    hs["PrizeId"] = 0;
                                    hs["ggddh"] = zh;
                                    hs["AdminHotelid"] = adminhotelid;
                                    DataFactory.SqlDataBase().InsertByHashtable("Prize_draw", hs);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ee)
            {
                Log.Info("抽奖错误信息", ee.Message);
            }

            return context;
        }

    }
}