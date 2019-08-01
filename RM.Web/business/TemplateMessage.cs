using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using SQL;
using System.Data;
using RM.Web.Lib;
using RM.Busines;
using RM.Common.DotNetCode;
using RM.Common.DotNetConfig;
using System.Collections;

namespace RM.Web.business
{
    public class TemplateMessage
    {
        //访问微信url并返回微信信息
        public static string GetJson(string url)
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

        public static string PostWebRequest(string postUrl, string menuInfo)
        {
            string returnValue = string.Empty;
            try
            {
                byte[] byteData = Encoding.UTF8.GetBytes(menuInfo);
                Uri uri = new Uri(postUrl);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);

                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteData.Length;
                //定义Stream信息
                Stream stream = webReq.GetRequestStream();
                stream.Write(byteData, 0, byteData.Length);
                stream.Close();
                //获取返回信息
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                returnValue = streamReader.ReadToEnd();

                //关闭信息
                streamReader.Close();
                response.Close();
                stream.Close();
            }
            catch (Exception ex)
            {

            }
            return returnValue;
        }


        public static string HttpUploadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);

            //请求头部信息
            StringBuilder sbHeader =
                new StringBuilder(
                    string.Format(
                        "Content-Disposition:form-data;name=\"media\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n",
                        fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();

            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }

        /// <summary>
        /// 删除素材图片
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public static string delete_img(string media_id, string AdminHotelid)
        {
            ///从缓存读取accesstoken
            string Access_token = GetAccessToken(AdminHotelid);
            string posturl = "https://api.weixin.qq.com/cgi-bin/material/del_material?access_token=" + Access_token;

            //POST数据例子： POST数据例子：{"media_id":MEDIA_ID}       

            string postData = "{\"media_id\":\"" + media_id + "\"}";

            string res = PostWebRequest(posturl, postData);

            if (res.Contains("errcode"))
            {
                //开始解析json串，使用前需要引用Newtonsoft.json.dll文件
                LitJson.JsonData data = LitJson.JsonMapper.ToObject(res);

                if (data["errcode"].ToString().Equals("0"))
                {
                    //返回成功处理
                }
            }
            return "";
        }

        public static string GetAccessToken(string AdminHotelid)
        {
            string accessToken = string.Empty;
            if (string.IsNullOrEmpty(AdminHotelid))
            {
                return accessToken;
            }
            if (AdminHotelid == "1004613")
            {
                AdminHotelid = "1";
            }
            int Hotelid = 0;
            string Token_Key = "Token_Key" + AdminHotelid + "_" + Hotelid;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM WX_Access_Token WHERE AdminHotelId = @AdminHotelId AND HotelId = @HotelId");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@AdminHotelId", AdminHotelid),
                new SqlParam("@HotelId", Hotelid)
            };
            string token_id = "";
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                token_id = dt.Rows[0]["Id"].ToString();
                DateTime tokenTime = Convert.ToDateTime(dt.Rows[0]["CreateTime"].ToString()).AddHours(1);
                if (DateTime.Now < tokenTime)
                {
                    accessToken = dt.Rows[0]["Access_Token"].ToString();
                    if (accessToken.Length > 20)
                    {
                        return accessToken;
                    }
                }
            }
            string getUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            getUrl = string.Format(getUrl, WxPayConfig.APPID(AdminHotelid, Hotelid), WxPayConfig.APPSECRET(AdminHotelid, Hotelid));
            Uri uri = new Uri(getUrl);
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);
            webReq.Method = "POST";

            //获取返回信息
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string returnJason = streamReader.ReadToEnd();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(returnJason);
            object value;
            if (json.TryGetValue("access_token", out value))
            {
                accessToken = value.ToString();
            }
            string returnInfo = "";
            if (string.IsNullOrEmpty(accessToken))
            {
                if (json.TryGetValue("errcode", out value))
                {
                    string errcode = value.ToString();
                }
                returnInfo = returnJason;
            }
            Hashtable ht = new Hashtable();
            ht["AdminHotelId"] = AdminHotelid;
            ht["HotelId"] = Hotelid;
            ht["Token_Key"] = Token_Key;
            ht["Access_Token"] = accessToken;
            ht["ReturnInfo"] = returnInfo;
            ht["CreateTime"] = DateTime.Now;
            DataFactory.SqlDataBase().Submit_AddOrEdit("WX_Access_Token", "Id", token_id, ht);

            return accessToken;
        }


        /// <summary>
        /// 发送客服图片消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string SenImage(string id, string AdminHotelid)
        {
            string show_qrcode_url = "";
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));
            string menuInfo = getnews(id, AdminHotelid);

            if (menuInfo != "")
            {
                string sqljosn = TemplateMessage.PostWebRequest(postUrl, menuInfo);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(sqljosn);
            }

            return show_qrcode_url;
        }

        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <returns></returns>
        public static string getnews(string id, string AdminHotelid)
        {
            string url = ConfigHelper.GetAppSettings("Url");
            string HotelName = GetHotelName(AdminHotelid);
            string title = "恭喜您成为" + HotelName + "的会员";
            string description = "恭喜您成为" + HotelName + "的会员，点击进去领取福利";
            string URL = url + "/Marketing/reward.aspx?AdminHotelid=" + AdminHotelid.ToString();
            string PIC_URL = url + "/Marketing/images/fuli.jpg";
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", id); //用户openid
            sb.AppendFormat("\"msgtype\":\"{0}\",", "news");//	发送文本格式
            sb.Append("\"news\":{");
            sb.Append("\"articles\":[{"); //文字描述
            sb.AppendFormat("\"title\":\"{0}\",", title); //标题
            sb.AppendFormat("\"description\":\"{0}\",", description); //文字描述
            sb.AppendFormat("\"url\":\"{0}\",", URL); //跳转链接路径
            sb.AppendFormat("\"picurl\":\"{0}\"", PIC_URL); //图片路径
            sb.Append("}]");
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }



        public static string GetHotelName(string AdminHotelid)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(string.Format("select Name from Hotel_Admin where 1=1 and AdminHotelid='" + AdminHotelid + "' "));
            DataTable dtStr = DataFactory.SqlDataBase().GetDataTableBySQL(sqlStr);
            if (dtStr != null && dtStr.Rows.Count > 0)
            {
                return dtStr.Rows[0]["Name"].ToString();
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 发送客服文本消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string Sentext(string id, string context, string AdminHotelid)
        {
            Log.Info("发送文本消息：", "openid:" + id + ",context:" + context);
            string show_qrcode_url = "";
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));
            string menuInfo = gettext(id, context);
            if (menuInfo != "")
            {
                string sqljosn = TemplateMessage.PostWebRequest(postUrl, menuInfo);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(sqljosn);
                Log.Info("发送文本消息：", "openid:" + id + ",结果:" + context);
            }
            return show_qrcode_url;
        }
        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <returns></returns>
        public static string gettext(string id, string context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", id); //用户openid
            sb.AppendFormat("\"msgtype\":\"{0}\",", "text");//	发送文本格式
            sb.Append("\"text\":{");
            sb.AppendFormat("\"content\":\"{0}\",", context); //文字描述
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 发送客服文本消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string Sentexts(string id, string context, string AdminHotelid)
        {
            string fhz = "1";
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
            postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken(AdminHotelid));
            string menuInfo = gettext(id, context);

            if (menuInfo != "")
            {
                string sqljosn = PostWebRequest(postUrl, menuInfo);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(sqljosn);
                object value;
                if (json.TryGetValue("errcode", out value))
                {
                    fhz = value.ToString();
                }
            }

            return fhz;
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <returns></returns>
        public static string gettexts(string id, string context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", id); //用户openid
            sb.AppendFormat("\"msgtype\":\"{0}\",", "text");//	发送文本格式
            sb.Append("\"text\":{");
            sb.AppendFormat("\"content\":\"{0}\",", context); //文字描述
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }


        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Sen(string order, string type, string sjhm = "", string gly = "")
        {
            string AdminHotelid = "";
            string openid = "";

            string sql = string.Format(@"select id,AdminHotelid,openid from Reservation where OrderNum='{0}'", order);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            StringBuilder sb = new StringBuilder();
            if (ds != null && ds.Rows.Count > 0)
            {
                AdminHotelid = ds.Rows[0]["AdminHotelid"].ToString();
                openid = ds.Rows[0]["openid"].ToString();
                //是否接收推送消息
                StringBuilder Is_Push = new StringBuilder();
                Is_Push.AppendFormat(@"SELECT id FROM Set_Is_Push WHERE openid=@Carid AND AdminHotelid=@AdminHotelid AND Is_Push=0");
                SqlParam[] parmAdd = new SqlParam[] { 
                                 new SqlParam("@Carid", openid),
                                 new SqlParam("@AdminHotelid", AdminHotelid)};
                DataTable ds1 = DataFactory.SqlDataBase().GetDataTableBySQL(Is_Push, parmAdd);

                if (ds1 != null && ds1.Rows.Count > 0)
                {
                    //不接收结束
                    return;
                }

            }
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));
            string menuInfo = "";

            if (type == "4")
            {
                //订单支付成功通知推送给客户
                menuInfo = getNewFKCG(order);
            }
            else if (type == "12")
            {
                menuInfo = get_transfers(order, sjhm);
            }

            else if (type == "13")
            {
                menuInfo = gettfxx(order);
            }
            else if (type == "14")
            {
                menuInfo = getrzxx(order);
            }
            else if (type == "15")
            {
                menuInfo = getrzyxx(order);//推送退房通知
            }
            else if (type == "16")
            {
                //获得奖金
                menuInfo = getrzyxxs(order);
            }
            else if (type == "70")
            {
                //会员卡快捷支付收款通知
                menuInfo = gethykkjzfsktz(order);
            }

            if (menuInfo != "")
            {
                PostWebRequest(postUrl, menuInfo);
            }
        }


        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void SenPayment(string order, string type, string AdminHotelid = "")
        {
            
            string openid = "";

            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));
            string menuInfo = "";

            
           if (type == "70")
            {
                //会员卡快捷支付收款通知
                menuInfo = gethykkjzfsktz(order);
            }

            if (menuInfo != "")
            {
                PostWebRequest(postUrl, menuInfo);
            }
        }


        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Sens(string order, string type, string sjhm = "", string gly = "")
        {
            string AdminHotelid = "";
            string sql = string.Format(@"select id,AdminHotelid from ServiceOrder where OrderNum='{0}'", order);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            StringBuilder sb = new StringBuilder();
            if (ds != null && ds.Rows.Count > 0)
            {
                AdminHotelid = ds.Rows[0]["AdminHotelid"].ToString();
            }
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));
            string menuInfo = "";

            if (type == "1")
            {
                //预订服务订单推送
                menuInfo = getfwddts(order);
            }
            else if (type == "2")
            {
                menuInfo = getfwqxddts(order);
            }
            Log.Info("//获取返回信息11-----menuInfo", menuInfo);
            if (menuInfo != "")
            {
                PostWebRequest(postUrl, menuInfo);
            }
        }

        /// <summary>
        ///点餐发送模板消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void FoodSen(string order, string type, string sjhm = "", string gly = "")
        {
            string AdminHotelid = "";
            string sql = string.Format(@"select id,AdminHotelid from FoodOrder where OrderNum='{0}'", order);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            StringBuilder sb = new StringBuilder();
            if (ds != null && ds.Rows.Count > 0)
            {
                AdminHotelid = ds.Rows[0]["AdminHotelid"].ToString();
            }
            if (gly == "1")
            {
                AdminHotelid = "1";
            }
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));
            string menuInfo = "";

            if (type == "1")
            {
                //预订点餐订单推送
                menuInfo = getdcddts(order);
            }
            else if (type == "2")
            {
                menuInfo = getdcqxddts(order);
            }
            else if (type == "3")
            {
                menuInfo = getdcygts(order);
            }
            Log.Info("//获取返回信息11-----menuInfo", menuInfo);
            if (menuInfo != "")
            {
                PostWebRequest(postUrl, menuInfo);
            }
        }


        /// <summary>
        ///商品发送模板消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ProductSen(string order, string type, string sjhm = "", string gly = "")
        {
            string AdminHotelid = "";
            string sql = string.Format(@"select id,AdminHotelid from ProductOrder where OrderNum='{0}'", order);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            StringBuilder sb = new StringBuilder();
            if (ds != null && ds.Rows.Count > 0)
            {
                AdminHotelid = ds.Rows[0]["AdminHotelid"].ToString();
            }
            if (gly == "1")
            {
                AdminHotelid = "1";
            }
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));
            string menuInfo = "";

            if (type == "1")
            {
                //预订商品订单推送
                menuInfo = getspddts(order);//商品订单支付成功
            }
            else if (type == "2")
            {
                menuInfo = getspqxddts(order);//商品订单取消
            }
            else if (type == "3")
            {
                menuInfo = getspygts(order);
            }
            Log.Info("//获取返回信息11-----menuInfo", menuInfo);
            if (menuInfo != "")
            {
                PostWebRequest(postUrl, menuInfo);
            }
        }



        /// <summary>
        ///帮助中心发送模板消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void HelpSen(string FeedBackId, string OpenId, string type)
        {
            string AdminHotelid = "1";
            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));
            string menuInfo = "";
            if (type == "1")
            {
                //预订商品订单推送
                menuInfo = gethelp(FeedBackId, OpenId);//商品订单支付成功
            }
            Log.Info("//获取返回信息11-----menuInfo", menuInfo);
            if (menuInfo != "")
            {
                PostWebRequest(postUrl, menuInfo);
            }
        }
        #region 商品订单付款成功通知
        /// <summary>
        /// 商品订单付款成功通知
        /// </summary>
        /// <returns></returns>
        public static string gethelp(string FeedBackId, string OpenId)
        {
            string sql = string.Format(@"SELECT * FROM dbo.FeedBackInfo where ID='{0}'", FeedBackId);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {

                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", OpenId.ToString());
                sb.AppendFormat("\"template_id\":\"{0}\",", "PPQP2agyXqmvTkrvFHueal55Xwyt0Xspmtw02d0vWhk");
                sb.AppendFormat("\"url\":\"{0}\",", "http://www.zidinn.com/Members/help/MessageRecordDetail.aspx?ID=" + dt.Rows[0]["ID"].ToString());

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您好！客户在智订云平台有新留言了，赶快看看吧\\n客户单位：" + dt.Rows[0]["UnitOrHotel"].ToString() + "\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //客户名称
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["Name"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //留言内容
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["FeedBackContent"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //留言时间
                string sTime = Convert.ToDateTime(dt.Rows[0]["AddTime"]).ToString("yyyy-MM-dd HH:mm");
                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", sTime);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                sb.Append("\"remark\":{");
                sb.Append("\"value\":\"请及时处理\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");

                sb.Append("}");
                sb.Append("}");
            }
            return sb.ToString();
        }
        #endregion


        #region 完成商品订单成功通知员工
        /// <summary>
        /// 完成商品订单成功通知员工
        /// </summary>
        /// <returns></returns>
        public static string getspygts(string order)
        {

            string sql = string.Format(@"select s.id,MemberId,ProductName,OrderNum,zip,PayPrice,PayType,OpenId,s.PayTime,s.AddTime,s.AdminHotelid,Mobile,s.name,Hotelid,h.Address,h.name AS hotelname,s.StaffId,s.ServicingMoney,s.EstimateMoney,s.DeliveryTime from ProductOrder s,Hotel h WHERE s.Hotelid=h.ID and s.OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {
                string User_Openid = "";
                string User_Name = "";
                StringBuilder sqls = new StringBuilder();
                sqls.AppendFormat("SELECT User_Name,openid FROM Base_UserInfo WHERE User_ID='{0}'", dt.Rows[0]["StaffId"].ToString());
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sqls);
                if (ds.Rows.Count > 0)
                {
                    User_Openid = ds.Rows[0]["openid"].ToString();
                    User_Name = ds.Rows[0]["User_Name"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", User_Openid);
                sb.AppendFormat("\"template_id\":\"{0}\",", "EzmktI6VY7vDOsE99EKzBce572xgMUg6pUUoGT83nCI");
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri("1", 0) + "//Melt/Sale_bonus.aspx");

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"客人[" + ReplaceWithSpecialChar(dt.Rows[0]["Name"].ToString()) + "]扫码预定了[" + dt.Rows[0]["hotelname"].ToString() + "]的商品\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //订单号
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //酒店名称
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["hotelname"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //下单时间
                string sTime = Convert.ToDateTime(dt.Rows[0]["DeliveryTime"]).ToString("yyyy-MM-dd HH:mm");
                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", sTime);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");



                string Remark = "智订云获得" + dt.Rows[0]["ServicingMoney"].ToString() + "元维护费,员工[" + User_Name + "]获得" + dt.Rows[0]["EstimateMoney"].ToString() + "元点餐奖金";

                sb.Append("\"remark\":{");
                sb.AppendFormat("\"value\":\"{0}\",", Remark.ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");

                sb.Append("}");
                sb.Append("}");



            }
            return sb.ToString();
        }
        #endregion

        #region 商品订单付款成功通知
        /// <summary>
        /// 商品订单付款成功通知
        /// </summary>
        /// <returns></returns>
        public static string getspddts(string order)
        {
            string sql = string.Format(@"select s.id,MemberId,ProductName,OrderNum,zip,PayPrice,PayType,OpenId,s.PayTime,s.AddTime,s.AdminHotelid,Mobile,s.name,Hotelid,h.Address from ProductOrder s,Hotel h WHERE s.Hotelid=h.ID and s.OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {

                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", dt.Rows[0]["OpenId"].ToString());
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("商品订单支付成功", dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri(dt.Rows[0]["AdminHotelid"].ToString(), Convert.ToInt32(dt.Rows[0]["HotelId"].ToString())) + "/Stores/Productorderdetails.aspx?AdminHotelid=" + dt.Rows[0]["AdminHotelid"].ToString() + "&Hotelid=" + dt.Rows[0]["Hotelid"].ToString() + "&Id=" + dt.Rows[0]["id"].ToString());

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您的订单已正式提交并支付成功，点击查看详情\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //订单号
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //时间
                string sTime = Convert.ToDateTime(dt.Rows[0]["PayTime"]).ToString("yyyy年MM月dd日");
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", sTime);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //订单金额
                sb.Append("\"keyword3\":{");
                if (dt.Rows[0]["PayType"].ToString() == "32")
                {
                    sb.AppendFormat("\"value\":\"积分抵扣{0}分\",", dt.Rows[0]["zip"]);
                }
                else if (dt.Rows[0]["PayType"].ToString() == "5")
                {
                    sb.AppendFormat("\"value\":\"卡券抵扣{0}元\",", dt.Rows[0]["zip"]);
                }
                else
                {
                    sb.AppendFormat("\"value\":\"{0}元\",", dt.Rows[0]["PayPrice"]);
                }
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                string Remark = "感谢您的预订";
                sb.Append("\"remark\":{");
                sb.Append("\"value\":\"商品信息：" + dt.Rows[0]["ProductName"].ToString() + "\\n酒店地址："
                    + dt.Rows[0]["Address"].ToString() + "\\n"
                    + Remark.ToString() + "\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");


                sb.Append("}");
                sb.Append("}");

            }
            return sb.ToString();
        }
        #endregion

        #region 商品取消订单通知
        /// <summary>
        /// 商品取消订单通知
        /// </summary>
        /// <returns></returns>
        public static string getspqxddts(string order)
        {
            string sql = string.Format(@"select s.id,MemberId,ProductName,OrderNum,zip,PayPrice,PayType,OpenId,s.CancelTime,s.AdminHotelid,s.SubmissionTime,Mobile,s.name,Hotelid,h.Address,h.phone from ProductOrder s,Hotel h WHERE s.Hotelid=h.ID and s.OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {

                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", dt.Rows[0]["OpenId"].ToString());
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("商品订单取消", dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri(dt.Rows[0]["AdminHotelid"].ToString(), Convert.ToInt32(dt.Rows[0]["HotelId"].ToString())) + "/Stores/Productorderdetails.aspx?AdminHotelid=" + dt.Rows[0]["AdminHotelid"].ToString() + "&Hotelid=" + dt.Rows[0]["Hotelid"].ToString() + "&Id=" + dt.Rows[0]["id"].ToString());

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您的订单已取消，点击查看详情\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //客户名称
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["name"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //订单编号
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //订单金额
                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["PayPrice"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //下单时间
                string SubmissionTime = Convert.ToDateTime(dt.Rows[0]["SubmissionTime"]).ToString("yyyy年MM月dd日");
                sb.Append("\"keyword4\":{");
                sb.AppendFormat("\"value\":\"{0}\",", SubmissionTime);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //取消时间
                string CancelTime = Convert.ToDateTime(dt.Rows[0]["CancelTime"]).ToString("yyyy年MM月dd日");
                sb.Append("\"keyword5\":{");
                sb.AppendFormat("\"value\":\"{0}\",", CancelTime);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                sb.Append("\"remark\":{");
                sb.Append("\"value\":\" \",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");


                sb.Append("}");
                sb.Append("}");


            }
            return sb.ToString();
        }
        #endregion


        #region 完成点餐订单成功通知员工
        /// <summary>
        /// 完成点餐订单成功通知员工
        /// </summary>
        /// <returns></returns>
        public static string getdcygts(string order)
        {

            string sql = string.Format(@"select s.id,MemberId,RestaurantName,FoodName,OrderNum,zip,PayPrice,PayType,OpenId,s.PayTime,s.AddTime,s.AdminHotelid,Mobile,s.name,Hotelid,h.Address,h.name AS hotelname,s.StaffId,s.ServicingMoney,s.EstimateMoney,s.UnUseTime from FoodOrder s,Hotel h WHERE s.Hotelid=h.ID and s.OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {
                string User_Openid = "";
                string User_Name = "";
                StringBuilder sqls = new StringBuilder();
                sqls.AppendFormat("SELECT User_Name,openid FROM Base_UserInfo WHERE User_ID='{0}'", dt.Rows[0]["StaffId"].ToString());
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sqls);
                if (ds.Rows.Count > 0)
                {
                    User_Openid = ds.Rows[0]["openid"].ToString();
                    User_Name = ds.Rows[0]["User_Name"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", User_Openid);
                sb.AppendFormat("\"template_id\":\"{0}\",", "55KGsByf5Oh6UjR2LYj3QVi5TiQLGz4fGeBM2rQjQME");
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri("1", 0) + "//Melt/Sale_bonus.aspx");

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"客人[" + ReplaceWithSpecialChar(dt.Rows[0]["Name"].ToString()) + "]扫码预定了[" + dt.Rows[0]["RestaurantName"].ToString() + "]的菜品\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //订单号
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //酒店名称
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["hotelname"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //配送地址
                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["Address"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //送餐时间
                string sTime = "";
                if (dt.Rows[0]["UnUseTime"] != null && dt.Rows[0]["UnUseTime"].ToString() != "")
                {
                   sTime= Convert.ToDateTime(dt.Rows[0]["UnUseTime"]).ToString("yyyy-MM-dd HH:mm");
                }
                else 
                {
                    sTime = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm");
                }

                sb.Append("\"keyword4\":{");
                sb.AppendFormat("\"value\":\"{0}\",", sTime);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //支付金额
                sb.Append("\"keyword5\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["PayPrice"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                string Remark = "智订云获得" + dt.Rows[0]["ServicingMoney"].ToString() + "元维护费,员工[" + User_Name + "]获得" + dt.Rows[0]["EstimateMoney"].ToString() + "元点餐奖金";

                sb.Append("\"remark\":{");
                sb.AppendFormat("\"value\":\"{0}\",", Remark.ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");

                sb.Append("}");
                sb.Append("}");


            }
            return sb.ToString();
        }
        #endregion

        #region 点餐订单付款成功通知
        /// <summary>
        /// 点餐订单付款成功通知
        /// </summary>
        /// <returns></returns>
        public static string getdcddts(string order)
        {
            string sql = string.Format(@"select s.id,MemberId,RestaurantName,FoodName,OrderNum,zip,PayPrice,PayType,OpenId,s.PayTime,s.AddTime,s.AdminHotelid,Mobile,s.name,Hotelid,h.Address from FoodOrder s,Hotel h WHERE s.Hotelid=h.ID and s.OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {

                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", dt.Rows[0]["OpenId"].ToString());
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("点餐订单支付成功", dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri(dt.Rows[0]["AdminHotelid"].ToString(), Convert.ToInt32(dt.Rows[0]["HotelId"].ToString())) + "/Stores/Foodorderdetails.aspx?AdminHotelid=" + dt.Rows[0]["AdminHotelid"].ToString() + "&Hotelid=" + dt.Rows[0]["Hotelid"].ToString() + "&Id=" + dt.Rows[0]["id"].ToString());

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您的订单已正式提交并支付成功，点击查看详情\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //订单号
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //订单金额
                sb.Append("\"keyword2\":{");
                if (dt.Rows[0]["PayType"].ToString() == "2")
                {
                    sb.AppendFormat("\"value\":\"积分抵扣{0}分\",", dt.Rows[0]["zip"]);
                }
                else if (dt.Rows[0]["PayType"].ToString() == "5")
                {
                    sb.AppendFormat("\"value\":\"卡券抵扣{0}元\",", dt.Rows[0]["zip"]);
                }
                else
                {
                    sb.AppendFormat("\"value\":\"{0}元\",", dt.Rows[0]["PayPrice"]);
                }
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //时间
                string sTime = Convert.ToDateTime(dt.Rows[0]["PayTime"]).ToString("yyyy年MM月dd日");
                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", sTime);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //订单详情
                sb.Append("\"keyword4\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["FoodName"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                sb.Append("\"remark\":{");
                sb.AppendFormat("\"value\":\"餐厅地址：" + dt.Rows[0]["Address"].ToString() + "\\n感谢您的预订\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");

                sb.Append("}");
                sb.Append("}");

            }
            return sb.ToString();
        }
        #endregion

        #region 点餐取消订单通知
        /// <summary>
        /// 点餐取消订单通知
        /// </summary>
        /// <returns></returns>
        public static string getdcqxddts(string order)
        {
            string sql = string.Format(@"select s.id,MemberId,RestaurantName,FoodName,OrderNum,zip,PayPrice,PayType,OpenId,s.CancelTime,s.AdminHotelid,s.SubmissionTime,Mobile,s.name,Hotelid,h.Address,h.phone from FoodOrder s,Hotel h WHERE s.Hotelid=h.ID and s.OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {

                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", dt.Rows[0]["OpenId"].ToString());
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("点餐订单取消", dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri(dt.Rows[0]["AdminHotelid"].ToString(), Convert.ToInt32(dt.Rows[0]["HotelId"].ToString())) + "/Stores/Foodorderdetails.aspx?AdminHotelid=" + dt.Rows[0]["AdminHotelid"].ToString() + "&Hotelid=" + dt.Rows[0]["Hotelid"].ToString() + "&Id=" + dt.Rows[0]["id"].ToString());

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您的订单已取消，点击查看详情\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //客户名称
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["name"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //订单编号
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //订单金额
                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["PayPrice"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //下单时间
                string SubmissionTime = Convert.ToDateTime(dt.Rows[0]["SubmissionTime"]).ToString("yyyy年MM月dd日");
                sb.Append("\"keyword4\":{");
                sb.AppendFormat("\"value\":\"{0}\",", SubmissionTime);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //取消时间
                string CancelTime = Convert.ToDateTime(dt.Rows[0]["CancelTime"]).ToString("yyyy年MM月dd日");
                sb.Append("\"keyword5\":{");
                sb.AppendFormat("\"value\":\"{0}\",", CancelTime);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                sb.Append("\"remark\":{");
                sb.Append("\"value\":\" \",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");


                sb.Append("}");
                sb.Append("}");


            }
            return sb.ToString();
        }
        #endregion


        /// <summary>
        /// 商户平台发送模板消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Senshpt(string openid, string type, string sjhm = "", string name = "", string money = "")
        {
            string AdminHotelid = "1";

            string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));
            string menuInfo = "";

            if (type == "1")
            {
                //营销充值
                menuInfo = getyxcz(openid, sjhm, name, money);
            }

            Log.Info("//获取商户平台发送模板消息返回信息-----menuInfo", menuInfo);
            if (menuInfo != "")
            {
                PostWebRequest(postUrl, menuInfo);
            }
        }

        #region 	营销充值
        /// <summary>
        /// 营销充值成功通知
        /// </summary>
        /// <returns></returns>
        public static string getyxcz(string openid, string sjhm = "", string name = "", string money = "")
        {


            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (openid != "")
            {

                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", openid);
                //sb.AppendFormat("\"touser\":\"{0}\",", getCarid(dt.Rows[0]["MemberId"].ToString(), dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"template_id\":\"{0}\",", "mb9O4rryem5rvs9cUc4eifIWuoR3hHw5tK1k0u2JmGc");
                sb.AppendFormat("\"url\":\"{0}\",", "");

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"[智订云]尊敬的用户：您的智订云酒店营销系统营销费账单已结清并恢复服务。请登录系统查看及管理。\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //门店名称
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", name);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");



                //联系电话
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", sjhm);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //时间
                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", time);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                sb.Append("\"remark\":{");
                sb.Append("\"value\":\" \",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");


                sb.Append("}");
                sb.Append("}");

            }
            return sb.ToString();
        }
        #endregion


        #region 	排房信息
        /// <summary>
        /// 订单付款成功通知
        /// </summary>
        /// <returns></returns>
        public static string gettfxx(string order)
        {
            string sql = string.Format(@"select id,MemberId,RoomType,OrderNum,zip,TomePrice,PayType,AdminHotelid,Mobile,name,HotelId,BeginTime,EndTime,openid from Reservation where OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {

                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", dt.Rows[0]["openid"].ToString());
                //sb.AppendFormat("\"touser\":\"{0}\",", getCarid(dt.Rows[0]["MemberId"].ToString(), dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("排房信息", dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri(dt.Rows[0]["AdminHotelid"].ToString(), Convert.ToInt32(dt.Rows[0]["HotelId"].ToString())) + "/Reservation/OrderWith.aspx?AdminHotelid=" + dt.Rows[0]["AdminHotelid"].ToString() + "&Id=" + order);

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您好，您预订的客房已排房，欢迎准时入住、期待您的光临\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //酒店名称
                sb.Append("\"hotelName\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");



                //客房名称
                sb.Append("\"roomName\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["RoomType"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //抵店日期
                sb.Append("\"date\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["BeginTime"]);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                sb.Append("\"remark\":{");
                sb.Append("\"value\":\"订单编号: " + dt.Rows[0]["OrderNum"].ToString() + "\\n入住房号："
                    + dt.Rows[0]["zip"].ToString() + "\\n订单金额："
                    + dt.Rows[0]["MemberId"].ToString() + "\\n会员卡号："
                    + dt.Rows[0]["MemberId"].ToString() + "\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");


                sb.Append("}");
                sb.Append("}");

            }
            return sb.ToString();
        }
        #endregion



        #region 	入住信息
        /// <summary>
        /// 订单付款成功通知
        /// </summary>
        /// <returns></returns>
        public static string getrzxx(string order)
        {
            string sql = string.Format(@"select id,MemberId,RoomType,OrderNum,zip,TomePrice,PayType,AdminHotelid,Mobile,name,HotelId,BeginTime,EndTime,openid,RoomNum from Reservation where OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {
                string AdminHotelid = dt.Rows[0]["AdminHotelid"].ToString();
                sb.Append("{");
                //sb.AppendFormat("\"touser\":\"{0}\",", getCarid(dt.Rows[0]["MemberId"].ToString(), dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"touser\":\"{0}\",", dt.Rows[0]["openid"].ToString());
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("入住信息", AdminHotelid));
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri(AdminHotelid) + "/Reservation/OrderWith.aspx?AdminHotelid=" + AdminHotelid + "&Id=" + order);

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您好，您已经成功办理入住，祝您入住愉快\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //订单编号
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //酒店名称
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", GethotelName(dt.Rows[0]["HotelId"].ToString()));
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //房间名称
                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["RoomType"]);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //入住时间
                sb.Append("\"keyword4\":{");
                sb.AppendFormat("\"value\":\"{0}\",", Convert.ToDateTime(dt.Rows[0]["BeginTime"]).ToString("yyyy年MM月dd日"));
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //房间数量
                sb.Append("\"keyword5\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["id"]);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                sb.Append("\"remark\":{");
                sb.Append("\"value\":\"入住房号: " + dt.Rows[0]["RoomNum"].ToString() + "\\n"
                    + "订单金额：" + dt.Rows[0]["TomePrice"].ToString() //+ "\\n"
                    //+ "订单金额："+ dt.Rows[0]["MemberId"].ToString() + "\\n"
                    //+ "会员卡号："+ dt.Rows[0]["MemberId"].ToString() + "\\n"
                    //+ "同行人："+dt.Rows[0]["MemberId"].ToString() 
                    + "\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");



                sb.Append("}");
                sb.Append("}");

            }
            return sb.ToString();
        }
        #endregion




        #region 	提醒退房信息
        /// <summary>
        /// 订单付款成功通知
        /// </summary>
        /// <returns></returns>
        public static string getrzxxu(string order)
        {
            string sql = string.Format(@"select id,MemberId,RoomType,OrderNum,zip,TomePrice,PayType,AdminHotelid,Mobile,name,HotelId,BeginTime,EndTime,openid from Reservation where OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {

                sb.Append("{");
                //sb.AppendFormat("\"touser\":\"{0}\",", getCarid(dt.Rows[0]["MemberId"].ToString(), dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"touser\":\"{0}\",", dt.Rows[0]["openid"].ToString());
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("提醒退房信息", dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri(dt.Rows[0]["AdminHotelid"].ToString(), Convert.ToInt32(dt.Rows[0]["HotelId"].ToString())) + "/Reservation/OrderWith.aspx?AdminHotelid=" + dt.Rows[0]["AdminHotelid"].ToString() + "&Id=" + order);

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"尊敬的客户，您的退房时间快到了，请您提前收拾好自己的行李物品，以防遗漏\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //入住酒店
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");



                //房间号
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", GethotelName(dt.Rows[0]["HotelId"].ToString()));
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //到时时间
                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["BeginTime"]);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //
                sb.Append("\"remark\":{");
                sb.AppendFormat("\"value\":\"前台电话:{0}\",", dt.Rows[0]["Mobile"]);
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");




                sb.Append("}");
                sb.Append("}");

            }
            return sb.ToString();
        }
        #endregion

        #region 	退房账单
        /// <summary>
        /// 推送退房通知
        /// </summary>
        /// <returns></returns>
        public static string getrzyxx(string order)
        {
            string sql = string.Format(@"select id,MemberId,RoomType,OrderNum,zip,TomePrice,PayType,AdminHotelid,Mobile,name,HotelId,BeginTime,EndTime,openid,RoomNum RoomNumber from Reservation where OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {
                string AdminHotelid = dt.Rows[0]["AdminHotelid"].ToString();
                sb.Append("{");
                //sb.AppendFormat("\"touser\":\"{0}\",", getCarid(dt.Rows[0]["MemberId"].ToString(), dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"touser\":\"{0}\",", dt.Rows[0]["openid"].ToString());
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("退房账单", AdminHotelid));
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri(AdminHotelid) + "/Reservation/OrderWith.aspx?AdminHotelid=" + AdminHotelid + "&Id=" + order);

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您好，您本期的账单如下\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //酒店名称
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", GethotelName(dt.Rows[0]["HotelId"].ToString()));
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //房间号码
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["RoomNumber"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                sb.Append("\"remark\":{");
                sb.Append("\"value\":\"房间名称: " + dt.Rows[0]["RoomType"].ToString() + "\\n"
                    + "入离时间：" + Convert.ToDateTime(dt.Rows[0]["BeginTime"]).ToString("yyyy年MM月dd日") + "-" + Convert.ToDateTime(dt.Rows[0]["EndTime"]).ToString("yyyy年MM月dd日") + "\\n"
                    + "账单金额：" + dt.Rows[0]["TomePrice"].ToString() + "元\\n"
                    + "欢迎您下次入住\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");

                sb.Append("}");
                sb.Append("}");

            }
            return sb.ToString();
        }
        #endregion

        #region 	退房获得奖金
        /// <summary>
        /// 退房获得奖金
        /// </summary>
        /// <returns></returns>
        public static string getrzyxxs(string order)
        {
            string sql = string.Format(@"SELECT  (CASE WHEN ar.Name IS NULL THEN a.Name ELSE ar.Name END) AS Name ,
                b.name HotelName ,
                ISNULL(c.User_Name, '') User_Name ,
                ISNULL(c.openid, '') User_Openid ,
                TomePrice ,
                ServicingMoney ,
                EstimateMoney ,
                a.AddTime,
                a.id,a.MemberId,a.RoomType,a.OrderNum,a.zip,a.PayType,a.AdminHotelid,a.Mobile,a.HotelId,a.BeginTime,a.EndTime,a.openid,ar.RoomNumber
        FROM    dbo.Reservation a
                LEFT JOIN dbo.Hotel b ON a.HotelId = b.ID
                LEFT JOIN dbo.Base_UserInfo c ON a.StaffId = c.User_ID
                LEFT JOIN AllocatingRoom AS ar ON ar.ReservationID = a.ID where a.OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            string menuInfo = "";

            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {
                string EstimateMoney = dt.Rows[0]["EstimateMoney"].ToString();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> csInfo = new Dictionary<string, object>();

                csInfo.Add("touser", dt.Rows[0]["User_Openid"].ToString());//微信id
                csInfo.Add("template_id", GetTemplateid("退房账单", dt.Rows[0]["AdminHotelid"].ToString()));//推送模板id
                string url = "";

                url = "http://" + WxPayConfig.redirect_uri(dt.Rows[0]["AdminHotelid"].ToString(), Convert.ToInt32(dt.Rows[0]["HotelId"].ToString())) + "/Melt/Sale_bonus.aspx";
                csInfo.Add("url", url);//点击跳转地址



                #region ****** 参数信息 ******
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("first", new Dictionary<string, object>
                    {
                        { "value", "客人["+ReplaceWithSpecialChar(dt.Rows[0]["Name"].ToString())+"]预订了["+GethotelName(dt.Rows[0]["HotelId"].ToString())+"]的["+dt.Rows[0]["RoomType"].ToString()+"]，并成功办理了退房，您已获得" + EstimateMoney + "元售房奖金"},
                        { "color", "#000" }
                    });
                //
                data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value",GethotelName(dt.Rows[0]["HotelId"].ToString())},
                        { "color", "#000" }
                    });
                //
                data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value", dt.Rows[0]["RoomNumber"].ToString()},
                        { "color", "#000" }
                    });
                //


                string rv = "";


                rv += "订单编号：" + dt.Rows[0]["OrderNum"].ToString() + "";
                //rv += "支付时间: " + Convert.ToDateTime(dt.Rows[0]["AddTime"]).ToString("yyyy-MM-dd HH:mm:ss") + "\n";
                //rv += "支付金额: " + dt.Rows[0]["TomePrice"].ToString() + "元\n";
                //rv += "客房名称: " + dt.Rows[0]["RoomType"].ToString() + "";
                data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", rv},
                        { "color", "#000" }
                    });
                #endregion
                csInfo.Add("data", data);
                if (EstimateMoney != "")
                    menuInfo = serializer.Serialize(csInfo);



            }
            return menuInfo;
        }
        #endregion

        #region 	会员卡快捷支付收款通知
        /// <summary>
        /// 会员卡快捷支付收款通知
        /// </summary>
        /// <returns></returns>
        public static string gethykkjzfsktz(string order)
        {
            string sql = string.Format(@"SELECT f.Name,f.Monery,h.PersonOpenid,f.AdminHotelid,f.AddTime FROM dbo.Finance f LEFT JOIN dbo.Hotel h ON f.hotelid=h.ID WHERE  Number='{0}' ", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            string menuInfo = "";

            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {
                string EstimateMoney = dt.Rows[0]["Monery"].ToString();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> csInfo = new Dictionary<string, object>();

                csInfo.Add("touser", dt.Rows[0]["PersonOpenid"].ToString());//微信id
                csInfo.Add("template_id", GetTemplateid("收款通知", dt.Rows[0]["AdminHotelid"].ToString()));//推送模板id
                string url = "";

                url = "http://" + WxPayConfig.redirect_uri(dt.Rows[0]["AdminHotelid"].ToString()) + "/MemberSystem/login.aspx?AdminHotelid=" + dt.Rows[0]["AdminHotelid"].ToString();
                csInfo.Add("url", url);//点击跳转地址



                #region ****** 参数信息 ******
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("first", new Dictionary<string, object>
                    {
                        { "value", "客人["+ReplaceWithSpecialChar(dt.Rows[0]["Name"].ToString())+"]消费"+EstimateMoney+"元"},
                        { "color", "#000" }
                    });
                //
                data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value",Convert.ToDateTime(dt.Rows[0]["AddTime"].ToString()).ToString("yyyy年MM月dd日 HH:mm")},
                        { "color", "#000" }
                    });
                //
                data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value", EstimateMoney},
                        { "color", "#000" }
                    });
                //


                string rv = "";


                //rv += "订单编号：" + order + "";
                //rv += "支付时间: " + Convert.ToDateTime(dt.Rows[0]["AddTime"]).ToString("yyyy-MM-dd HH:mm:ss") + "\n";
                //rv += "支付金额: " + dt.Rows[0]["TomePrice"].ToString() + "元\n";
                //rv += "客房名称: " + dt.Rows[0]["RoomType"].ToString() + "";
                data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", rv},
                        { "color", "#000" }
                    });
                #endregion
                csInfo.Add("data", data);
                if (EstimateMoney != "")
                    menuInfo = serializer.Serialize(csInfo);



            }
            return menuInfo;
        }
        #endregion


        /// <summary>
        /// 获取微信ID
        /// </summary>
        /// <param name="hotelid"></param>
        /// <returns></returns>
        public static string getCarid(string id, string AdminHotelid)
        {
            string sql = string.Format(@"select carid from hy_hyzlxxb where   lsh=@lsh and AdminHotelid=@AdminHotelid");
            SqlParam[] parmAdd2 = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", AdminHotelid),
                                     new SqlParam("@lsh",id)};
            DataTable ds = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(new StringBuilder(sql), parmAdd2);
            if (ds != null && ds.Rows.Count > 0)
            {
                return ds.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }


        private static string fanshi(object o)
        {
            if (o.ToString() == "1")
            {
                return "到店付款";
            }
            else if (o.ToString() == "2")
            {
                return "积分兑换";
            }
            else if (o.ToString() == "3")
            {
                return "会员卡支付";
            }
            else if (o.ToString() == "4")
            {
                return "微信支付";
            }
            else if (o.ToString() == "5")
            {
                return "卡券抵扣";
            }
            else
            {
                return "微信支付";
            }
        }

        public static string GetTemplateid(string name, string AdminHotelid)
        {
            return ApplicationHelper.GetHotelTemplateId(name, AdminHotelid);
        }


        /////模板消息通知----------------------------------------------------
        //// ---------------------------------------------------

        #region 订单付款成功通知
        /// <summary>
        /// 订单付款成功通知
        /// </summary>
        /// <returns></returns>
        public static string getNewFKCG(string order)
        {
            string sql = string.Format(@"select r.id,MemberId,RoomType,OrderNum,zip,TomePrice,PayType,r.AdminHotelid,Mobile,r.name,HotelId,BeginTime,EndTime,h.Address,r.openid from Reservation r,Hotel h WHERE r.HotelId=h.ID AND r.OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", dt.Rows[0]["openid"].ToString());
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("订单支付成功", dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri(dt.Rows[0]["AdminHotelid"].ToString(), Convert.ToInt32(dt.Rows[0]["HotelId"].ToString())) + "/Reservation/OrderWith.aspx?AdminHotelid=" + dt.Rows[0]["AdminHotelid"].ToString() + "&Id=" + order);

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您的订单已正式提交并支付成功，点击查看详情\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //订单号
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //订单金额
                sb.Append("\"keyword2\":{");
                if (dt.Rows[0]["PayType"].ToString() == "2")
                {
                    sb.AppendFormat("\"value\":\"积分抵扣{0}分\",", dt.Rows[0]["Zip"]);
                }
                else if (dt.Rows[0]["PayType"].ToString() == "5")
                {
                    sb.AppendFormat("\"value\":\"卡券抵扣{0}元\",", dt.Rows[0]["Zip"]);
                }
                else if (dt.Rows[0]["PayType"].ToString() == "6")
                {
                    sb.AppendFormat("\"value\":\"预售券抵扣{0}元\",", dt.Rows[0]["Zip"]);
                }
                else
                {
                    sb.AppendFormat("\"value\":\"{0}元\",", dt.Rows[0]["TomePrice"]);
                }
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //酒店名称
                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", GethotelName(dt.Rows[0]["HotelId"].ToString()));
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //酒店房型
                sb.Append("\"keyword4\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["RoomType"]);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //抵离时间
                string sTime = Convert.ToDateTime(dt.Rows[0]["BeginTime"]).ToString("yyyy年MM月dd日") + "-" + Convert.ToDateTime(dt.Rows[0]["EndTime"]).ToString("yyyy年MM月dd日");
                sb.Append("\"keyword5\":{");
                sb.AppendFormat("\"value\":\"{0}\",", sTime);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                sb.Append("\"remark\":{");
                sb.AppendFormat("\"value\":\"酒店地址：" + dt.Rows[0]["Address"].ToString() + "\\n感谢您的预订\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");

                sb.Append("}");
                sb.Append("}");


            }
            return sb.ToString();
        }
        #endregion


        #region 零钱入账通知
        /// <summary>
        /// 零钱入账通知
        /// </summary>
        /// <returns></returns>
        public static string get_transfers(string order, string sjhm)
        {

            string sql = string.Format(@"select f.money,f.AdminHotelid,r.ID,r.OrderNum,r.RoomType,DATEDIFF(DAY,r.BeginTime,r.EndTime)                        AS OrderDAY,r.TomePrice FROM dbo.Distribution_Finance f INNER JOIN dbo.Reservation r ON 
            r.OrderNum = f.OrderNum WHERE r.OrderNum='{0}'  ", order);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            StringBuilder sb = new StringBuilder();
            if (ds != null && ds.Rows.Count > 0)
            {
                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", sjhm);
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("零钱入账通知", ds.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"url\":\"{0}\",", "");

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您好，您推广的客户成功通过酒店微网预订客房，您已获得" + ds.Rows[0]["money"] + "元奖金\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}元\",", ds.Rows[0]["money"]);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm"));
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", "推广奖金");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                sb.Append("\"keyword4\":{");
                sb.AppendFormat("\"value\":\"{0}\",", "客户预订" + ds.Rows[0]["RoomType"] + " " + ds.Rows[0]["OrderDAY"] + "晚 " + Convert.ToInt32(ds.Rows[0]["TomePrice"]) + "元");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                sb.Append("\"remark\":{");
                sb.Append("}");

                sb.Append("}");
                sb.Append("}");

            }
            return sb.ToString();
        }
        #endregion



        #region 门店服务订单付款成功通知
        /// <summary>
        /// 订单付款成功通知
        /// </summary>
        /// <returns></returns>
        public static string getfwddts(string order)
        {
            string sql = string.Format(@"select s.id,MemberId,StoresName,ServiceName,OrderNum,zip,PayPrice,PayType,OpenId,s.PayTime,s.AdminHotelid,Mobile,s.name,Hotelid,h.Address from ServiceOrder s,Hotel h WHERE s.Hotelid=h.ID and s.OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("{");
                //sb.AppendFormat("\"touser\":\"{0}\",", getCarid(dt.Rows[0]["MemberId"].ToString(), dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"touser\":\"{0}\",", dt.Rows[0]["OpenId"].ToString());
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("订单支付成功", dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri(dt.Rows[0]["AdminHotelid"].ToString(), Convert.ToInt32(dt.Rows[0]["HotelId"].ToString())) + "/Stores/orderdetails.aspx?AdminHotelid=" + dt.Rows[0]["AdminHotelid"].ToString() + "&Hotelid=" + dt.Rows[0]["Hotelid"].ToString() + "&Id=" + order);

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您的订单已正式提交并支付成功，点击查看详情\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //订单号
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //时间
                string sTime = Convert.ToDateTime(dt.Rows[0]["PayTime"]).ToString("yyyy年MM月dd日");
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", sTime);
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //订单金额
                sb.Append("\"keyword3\":{");
                if (dt.Rows[0]["PayType"].ToString() == "2")
                {
                    sb.AppendFormat("\"value\":\"积分抵扣{0}分\",", dt.Rows[0]["zip"]);
                }
                else if (dt.Rows[0]["PayType"].ToString() == "5")
                {
                    sb.AppendFormat("\"value\":\"卡券抵扣{0}元\",", dt.Rows[0]["zip"]);
                }
                else
                {
                    sb.AppendFormat("\"value\":\"{0}元\",", dt.Rows[0]["PayPrice"]);
                }
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                sb.Append("\"remark\":{");
                sb.AppendFormat("\"value\":\"门店地址：" + dt.Rows[0]["Address"].ToString() + "\\n感谢您的预订\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");

                sb.Append("}");
                sb.Append("}");


            }
            return sb.ToString();
        }
        #endregion


        #region 门店服务取消订单通知
        /// <summary>
        /// 订单付款成功通知
        /// </summary>
        /// <returns></returns>
        public static string getfwqxddts(string order)
        {
            string sql = string.Format(@"select s.id,MemberId,StoresName,ServiceName,OrderNum,zip,PayPrice,PayType,OpenId,s.CancelTime,s.AdminHotelid,Mobile,s.name,Hotelid,h.Address,h.phone from ServiceOrder s,Hotel h WHERE s.Hotelid=h.ID and s.OrderNum='{0}'", order);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("{");
                sb.AppendFormat("\"touser\":\"{0}\",", dt.Rows[0]["OpenId"].ToString());
                sb.AppendFormat("\"template_id\":\"{0}\",", GetTemplateid("服务订单取消", dt.Rows[0]["AdminHotelid"].ToString()));
                sb.AppendFormat("\"url\":\"{0}\",", "http://" + WxPayConfig.redirect_uri(dt.Rows[0]["AdminHotelid"].ToString(), Convert.ToInt32(dt.Rows[0]["HotelId"].ToString())) + "/Stores/orderdetails.aspx?AdminHotelid=" + dt.Rows[0]["AdminHotelid"].ToString() + "&Hotelid=" + dt.Rows[0]["Hotelid"].ToString() + "&Id=" + order);

                sb.Append("\"data\":{");
                sb.Append("\"first\":{");
                sb.AppendFormat("\"value\":\"您的订单已取消，点击查看详情\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                //订单号
                sb.Append("\"keyword1\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["OrderNum"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //预约项目
                sb.Append("\"keyword2\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["ServiceName"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");


                //联系电话
                sb.Append("\"keyword3\":{");
                sb.AppendFormat("\"value\":\"{0}\",", dt.Rows[0]["phone"].ToString());
                sb.Append("\"color\":\"#000\"");
                sb.Append("},");

                string sTime = Convert.ToDateTime(dt.Rows[0]["CancelTime"]).ToString("yyyy年MM月dd日");
                sb.Append("\"remark\":{");
                sb.AppendFormat("\"value\":\"取消时间: " + sTime + "\\n如有疑问请联系客服。\",");
                sb.Append("\"color\":\"#000\"");
                sb.Append("}");

                sb.Append("}");
                sb.Append("}");


            }
            return sb.ToString();
        }
        #endregion

        public static string GethotelName(string id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT name FROM Hotel WHERE ID={0}", id);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }



        /// <summary>
        /// 获取二维码链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string get_openid(string AdminHotelid, int Hotelid)
        {

            string openid = "";
            string postUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));  //二维码链接
            string returnJason = GetJson(postUrl);

            //获取返回信息
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(returnJason);
            object value;
            if (json.TryGetValue("openid", out value))
            {
                openid = value.ToString();
            }
            return openid;
        }

        /// <summary>
        /// 获取二维码链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string Sen_Hotelimg(string strid, string AdminHotelid, string id)
        {
            string show_qrcode_url = "";
            string postUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));  //二维码链接
            string menuInfo = gethotel_img(strid);//酒店ID

            if (menuInfo != "")
            {
                string sqljosn = PostWebRequest(postUrl, menuInfo);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(sqljosn);
                object value;
                if (json.TryGetValue("ticket", out value))
                {
                    show_qrcode_url = value.ToString();
                }
            }

            return show_qrcode_url;
        }

        /// <summary>
        /// 生成酒店/门店永久二维码
        /// </summary>
        /// <returns></returns>
        public static string gethotel_img(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"action_name\":\"{0}\",", "QR_LIMIT_STR_SCENE");//	二维码类型，QR_SCENE为临时的整型参数值，QR_STR_SCENE为临时的字符串参数值，QR_LIMIT_SCENE为永久的整型参数值，QR_LIMIT_STR_SCENE为永久的字符串参数值
            sb.Append("\"action_info\":{");
            sb.Append("\"scene\":{");
            sb.AppendFormat("\"scene_str\":\"{0}\",", "@" + id); //字符串类型，长度限制为1到64  
            sb.Append("}");
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }


        /// <summary>
        /// 生成二维码链接
        /// </summary>
        /// <param name="user_id">员工ID</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string Sen_img(string user_id, string id)
        {
            string AdminHotelid = "";
            string sql = string.Format(@"select id,AdminHotelid from Base_UserInfo where User_ID='{0}'", id);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            StringBuilder sb = new StringBuilder();
            if (ds != null && ds.Rows.Count > 0)
            {
                AdminHotelid = ds.Rows[0]["AdminHotelid"].ToString();
            }
            string show_qrcode_url = "";
            string postUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));  //酒店公众号信息
            string menuInfo = getyongjiu_img(user_id);//手机号码

            if (menuInfo != "")
            {
                string sqljosn = PostWebRequest(postUrl, menuInfo);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(sqljosn);
                object value;
                if (json.TryGetValue("ticket", out value))
                {
                    show_qrcode_url = value.ToString();
                }
            }

            return show_qrcode_url;
        }

        /// <summary>
        /// 生成二维码链接
        /// </summary>
        /// <param name="user_id">分销员ID</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string Sen_imgs(string user_id, string id)
        {

            string AdminHotelid = "";

            string sql = string.Format(@"select id,AdminHotelid from Distributor_UserInfo where User_ID='{0}'", id);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            StringBuilder sb = new StringBuilder();
            if (ds != null && ds.Rows.Count > 0)
            {
                AdminHotelid = ds.Rows[0]["AdminHotelid"].ToString();
            }

            string show_qrcode_url = "";
            string postUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));  //酒店公众号信息
            string menuInfo = getyongjiu_img(user_id);//手机号码

            if (menuInfo != "")
            {
                string sqljosn = PostWebRequest(postUrl, menuInfo);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(sqljosn);
                object value;
                if (json.TryGetValue("ticket", out value))
                {
                    show_qrcode_url = value.ToString();
                }
            }

            return show_qrcode_url;
        }

        /// <summary>
        /// 生成永久二维码
        /// </summary>
        /// <returns></returns>
        public static string getyongjiu_img(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"action_name\":\"{0}\",", "QR_LIMIT_STR_SCENE");//	二维码类型，QR_SCENE为临时的整型参数值，QR_STR_SCENE为临时的字符串参数值，QR_LIMIT_SCENE为永久的整型参数值，QR_LIMIT_STR_SCENE为永久的字符串参数值
            sb.Append("\"action_info\":{");
            sb.Append("\"scene\":{");
            sb.AppendFormat("\"scene_str\":\"{0}\",", "@" + id); //字符串类型，长度限制为1到64  
            sb.Append("}");
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }





        /// <summary>
        /// 餐桌生成二维码链接
        /// </summary>
        /// <param name="user_id">分销员ID</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string Sen_foodimg(string TableCode, string AdminHotelid)
        {
            string show_qrcode_url = "";
            string postUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));  //酒店公众号信息
            string menuInfo = getfood_img(TableCode);//餐桌码
            if (menuInfo != "")
            {
                string sqljosn = PostWebRequest(postUrl, menuInfo);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(sqljosn);
                object value;
                if (json.TryGetValue("ticket", out value))
                {
                    show_qrcode_url = value.ToString();
                }
            }

            return show_qrcode_url;
        }


        /// <summary>
        /// 餐桌生成永久二维码
        /// </summary>
        /// <returns></returns>
        public static string getfood_img(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"action_name\":\"{0}\",", "QR_LIMIT_STR_SCENE");//	二维码类型，QR_SCENE为临时的整型参数值，QR_STR_SCENE为临时的字符串参数值，QR_LIMIT_SCENE为永久的整型参数值，QR_LIMIT_STR_SCENE为永久的字符串参数值
            sb.Append("\"action_info\":{");
            sb.Append("\"scene\":{");
            sb.AppendFormat("\"scene_str\":\"{0}\",", "@" + id); //字符串类型，长度限制为1到64  
            sb.Append("}");
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }





        /// <summary>
        /// 活动生成二维码链接
        /// </summary>
        /// <param name="user_id">分销员ID</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string Sen_Activityimg(string Aid, string AdminHotelid)
        {
            string show_qrcode_url = "";
            string postUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            postUrl = string.Format(postUrl, GetAccessToken(AdminHotelid));  //酒店公众号信息
            string menuInfo = getactivity_img(Aid);//活动码
            if (menuInfo != "")
            {
                string sqljosn = PostWebRequest(postUrl, menuInfo);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(sqljosn);
                object value;
                if (json.TryGetValue("ticket", out value))
                {
                    show_qrcode_url = value.ToString();
                }
            }

            return show_qrcode_url;
        }


        /// <summary>
        /// 活动生成永久二维码
        /// </summary>
        /// <returns></returns>
        public static string getactivity_img(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"action_name\":\"{0}\",", "QR_LIMIT_STR_SCENE");//	二维码类型，QR_SCENE为临时的整型参数值，QR_STR_SCENE为临时的字符串参数值，QR_LIMIT_SCENE为永久的整型参数值，QR_LIMIT_STR_SCENE为永久的字符串参数值
            sb.Append("\"action_info\":{");
            sb.Append("\"scene\":{");
            sb.AppendFormat("\"scene_str\":\"{0}\",", "@" + id); //字符串类型，长度限制为1到64  
            sb.Append("}");
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }






        //获取房号
        public static string getRoomNumber(string id)
        {
            string RoomNumber = "";
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT RoomNumber FROM AllocatingRoom WHERE ReservationID={0} ", id);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                RoomNumber = dt.Rows[0]["RoomNumber"].ToString();
            }
            return RoomNumber;
        }

        //获取星期几
        public static string GetWeek()
        {
            string WeekVal = "";
            string Week = DateTime.Today.DayOfWeek.ToString();
            switch (Week)
            {
                case "Monday":
                    WeekVal = "1";
                    break;
                case "Tuesday":
                    WeekVal = "2";
                    break;
                case "Wednesday":
                    WeekVal = "3";
                    break;
                case "Thursday":
                    WeekVal = "4";
                    break;
                case "Friday":
                    WeekVal = "5";
                    break;
                case "Saturday":
                    WeekVal = "6";
                    break;
                case "Sunday":
                    WeekVal = "7";
                    break;
            }
            return WeekVal;
        }

        //获取时间
        public static string GetPeriodTime()
        {
            string PeriodTimeVal = "";
            string newTime = DateTime.Now.ToString();//当前时间
            string ShortDate = DateTime.Now.ToString("yyyy-MM-dd");
            string sql = string.Format(@"SELECT * FROM dbo.NoticeTime");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[] nDate = dt.Rows[i]["NoticeTime"].ToString().Split('-');
                    if (Convert.ToDateTime(newTime) > Convert.ToDateTime(ShortDate + " " + nDate[0].ToString()) && Convert.ToDateTime(newTime) < Convert.ToDateTime(ShortDate + " " + nDate[1].ToString()))
                    {
                        PeriodTimeVal = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            return PeriodTimeVal;
        }

        /// <summary>
        /// 将传入的字符串中间部分字符替换成特殊字符
        /// </summary>
        /// <param name="value">需要替换的字符串</param>
        /// <param name="startLen">前保留长度</param>
        /// <param name="endLen">尾保留长度</param>
        /// <param name="replaceChar">特殊字符</param>
        /// <returns>被特殊字符替换的字符串</returns>
        private static string ReplaceWithSpecialChar(string value, int startLen = 1, int endLen = 0, char specialChar = '※')
        {
            try
            {
                int lenth = value.Length - startLen - endLen;

                string replaceStr = value.Substring(startLen, lenth);

                string specialStr = "";

                for (int i = 0; i < replaceStr.Length; i++)
                {
                    specialStr += specialChar;
                }
                if (replaceStr != "")
                {
                    value = value.Replace(replaceStr, specialStr);
                }
            }
            catch (Exception)
            {

            }

            return value;
        }




        #region ** 房态提醒 **

        /// <summary>
        /// 房态满房提醒
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        internal static void NoticePush_RoomState(string HotelId, string RoomId)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> csInfo = new Dictionary<string, object>();
            csInfo.Add("HotelId", HotelId);//
            csInfo.Add("RoomId", RoomId);
            string menuInfo = serializer.Serialize(csInfo);
            string postUrl = "http://39.108.182.4:6203/api/NoticePush/RoomState";
            TemplateMessage.PostWebRequest(postUrl, menuInfo);
        }
        #endregion
    }
}