using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Collections;
using System.Web.Security;
using Common;
using System.Xml;
using Tencent;

namespace Api
{
    //====================================================================================
    // url: ip/api/类名/方法名
    // 统一JSON格式 正确{"code":"0" },错误{"code":"非0","message":"错误消息"}
    //=====================================================================================
    /// <summary>
    /// 消息与事件 API 入口类,
    /// </summary>
    public class APIHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        #region 验证微信签名 保持默认即可
        const string Token = "sewapower";

        private void Valid(HttpContext context)
        {
            string echoStr = context.Request.QueryString["echoStr"].ToString();
            if (CheckSignature(context))
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    APIResponse.WirterString(echoStr);
                }
            }
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        private bool CheckSignature(HttpContext context)
        {
            string signature = context.Request.QueryString["signature"].ToString();
            string timestamp = context.Request.QueryString["timestamp"].ToString();
            string nonce = context.Request.QueryString["nonce"].ToString();
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
        #endregion

        string log_Folder = "API";
        string sAppID = "wx1939f9ee65384f14";
        string sToken = "sewapower";
        string sEncodingAESKey = "Sd8AFrmKTlF2u5jbQl8vQEYJX57aALEz1OivuIUgD3r";

        public void ProcessRequest(HttpContext context)
        {
            try
            {

                JobLogs.Writer(log_Folder, "------------------- 分割线 -------------------");
                ////写入日志
                string url = context.Request.Url.ToString();
                JobLogs.Writer(log_Folder, "消息与事件URL:" + url);
                if (context.Request.HttpMethod == "POST")
                {

                }
                else
                {
                    Valid(context);
                    return;
                }
                // wx.zidinn.com/api/$APPID$/callback
                string[] path = context.Request.Path.Split(new char[1] { '/' });
                if (path.Length < 3)
                {
                    return;
                }
                string timestamp = context.Request.QueryString["timestamp"];//时间戳
                string nonce = context.Request.QueryString["nonce"];//随机数
                string encrypt_type = context.Request.QueryString["encrypt_type"];//加密类型，为aes
                string msg_signature = context.Request.QueryString["msg_signature"];//消息体签名，用于验证消息体的正确性
                string appid = path[2].Trim();
                string className = "Callback";
                string methodName = "Analysis";
                //认证请求  
                if (!IsAuthentication(context, className, methodName))
                    throw new APIException("认证失败");

                WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);

                System.IO.StreamReader stream = new System.IO.StreamReader(context.Request.InputStream, Encoding.UTF8);
                string postData = stream.ReadToEnd();
                stream.Close();

                ////写入日志
                JobLogs.Writer(log_Folder, "appid:" + appid);
                JobLogs.Writer(log_Folder, "消息与事件postData:" + postData);

                string original_text = "";
                int ret = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, postData, ref original_text);
                if (original_text == "")
                {
                    JobLogs.Writer(log_Folder, "---------------------------------------消息与事件解密失败" + ret + "---------------------------------------");
                    context.Response.Write("error");
                    return;
                }
                JobLogs.Writer(log_Folder, "消息与事件解密结果:" + original_text);

                Callback cb = new Callback();
                string msg = cb.Analysis(context, original_text, appid);
                JobLogs.Writer(log_Folder, "返回结果:" + msg);
                string en_msg = "";//加密消息
                if (msg != "" && msg != "error")
                {
                    ret = wxcpt.EncryptMsg(msg, timestamp, nonce, ref en_msg);
                    if (en_msg == "")
                    {
                        JobLogs.Writer(log_Folder, "++++++++++++++++++++++++++++++++++++++消息与事件加密失败" + ret + "++++++++++++++++++++++++++++++++++++++");
                    }
                }
                JobLogs.Writer(log_Folder, "返回结果(加密):" + en_msg);

                context.Response.Write(en_msg);
                return;
            }
            catch (APIException ex)
            {
                int errCode = (int)ex.ReCode;
                string message = string.Empty;
                if (ex.ReCode == APICode.ERROR)
                {
                    message = ex.Message; //直接输出消息
                }
                else
                {
                    message = ex.ReCode.ToString();
                }
                Hashtable ht = new Hashtable();
                ht.Add("code", errCode);
                ht.Add("message", message);
                string json = LitJson.JsonMapper.ToJson(ht);
                APIResponse.WirterString(json);
            }
            catch (Exception ex)
            {
                APIException apiex = ex.InnerException as APIException;
                int errCode = 0;
                string message = string.Empty;

                if (apiex != null) //如果是自定义异常抛出
                {
                    errCode = (int)apiex.ReCode;
                    if (apiex.ReCode == APICode.ERROR)
                    {
                        message = apiex.Message; //直接输出消息
                    }
                    else
                    {
                        message = APIResponse.GetEnumDesc(apiex.ReCode);
                    }
                }
                else
                {
                    errCode = -1;
                    message = "系统异常,已记录日志!";
                    //系统异常写入日志
                    // Log4J.Error("webapi 系统异常", ex);
                }
                Hashtable ht = new Hashtable();
                ht.Add("code", errCode);
                ht.Add("message", message);
                string json = LitJson.JsonMapper.ToJson(ht);
                APIResponse.WirterString(json);
            }
        }

        private bool IsAuthentication(HttpContext context, string className, string methodName)
        {
            //这里需要做安全认证：IP白名单，ID + js

            return true;
        }


    }
}
