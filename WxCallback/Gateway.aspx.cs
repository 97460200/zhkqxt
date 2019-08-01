using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Alipay;
using Aop.Api.Util;
using System.Xml;
using System.IO;

namespace WxCallback
{
    public partial class Gateway : System.Web.UI.Page
    {

        /// <summary>
        /// 记录日志到文件
        /// </summary>
        /// <param name="log"></param>
        public void log(string log)
        {
            string logfile = HttpRuntime.AppDomainAppPath.ToString() + "log/dotnet_log.txt";
            //FileStream fs = new FileStream(logfile, FileMode.Create);
            //StreamWriter sw = new StreamWriter(fs);
            StreamWriter sw = File.AppendText(logfile);
            sw.WriteLine(log);
            sw.Close();
            //fs.Close();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            log("-------------------------------");
            log(GetUrlParam(getRequstParam()));
            log("===============================");
            //验证网关
            if ("alipay.service.check".Equals(getRequestString("service")))
            {
                //Response.Output.WriteLine(GetUrlParam());

                verifygw();
            }
            else if ("alipay.mobile.public.message.notify".Equals(getRequestString("service")))
            {
                string eventType = getXmlNode(getRequestString("biz_content"), "EventType");
                string alipayUserId = getXmlNode(getRequestString("biz_content"), "FromAlipayUserId");
                string UserInfo = getXmlNode(getRequestString("biz_content"), "UserInfo");
                string ActionParam = getXmlNode(getRequestString("biz_content"), "ActionParam");
                string AgreementId = getXmlNode(getRequestString("biz_content"), "AgreementId");
                string AccountNo = getXmlNode(getRequestString("biz_content"), "AccountNo");
                string AppId = getXmlNode(getRequestString("biz_content"), "AppId");
                string CreateTime = getXmlNode(getRequestString("biz_content"), "CreateTime");
                string MsgType = getXmlNode(getRequestString("biz_content"), "MsgType");
                if ("follow".Equals(eventType))
                {


                    //用户新关注后，可以给用户发送一条欢迎消息，或者引导消息
                    //如：
                    string biz_content = "{\"msgType\":\"text\",\"text\":{\"content\":\"你好，欢迎来到服务窗\"},\"toUserId\":\"" + alipayUserId + "\"}";
                    Response.Output.WriteLine(MessageSendBiz.CustomSend(biz_content));
                }
                else if ("unfollow".Equals(eventType))
                {

                }
                else if ("click".Equals(eventType))
                {

                }
                else if ("enter".Equals(eventType))
                {

                }
                if ("text".Equals(MsgType))
                {
                    string biz_content = "{\"msgType\":\"text\",\"text\":{\"content\":\"你好，这是对话消息\"},\"toUserId\":\"" + alipayUserId + "\"}";
                    Response.Output.WriteLine(MessageSendBiz.CustomSend(biz_content));
                }
            }
        }

        //public void do

        /// <summary>
        /// 转换支付宝的请求为字典数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> getAlipayRequstParams()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("service", getRequestString("service"));
            dict.Add("sign_type", getRequestString("sign_type"));
            dict.Add("charset", getRequestString("charset"));
            dict.Add("biz_content", getRequestString("biz_content"));
            dict.Add("sign", getRequestString("sign"));
            return dict;
        }

        /// <summary>
        /// 验签支付宝请求
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool verifySignAlipayRequest(Dictionary<string, string> param)
        {
            bool result = AlipaySignature.RSACheckV2(param, Config.alipay_public_key, Config.charset, Config.signtype, false);
            return result;
        }

        /// <summary>
        /// 支付宝验证商户网关
        /// </summary>
        public void verifygw()
        {
            //  Request.Params;
            Dictionary<string, string> dict = getAlipayRequstParams();
            //string biz_content = AlipaySignature.CheckSignAndDecrypt(dict, Config.alipay_public_key, Config.merchant_private_key, true, false);
            string biz_content = dict["biz_content"];
            if (!verifySignAlipayRequest(dict))
            {
                verifygwResponse(false, Config.merchant_public_key);
            }
            if ("verifygw".Equals(getXmlNode(biz_content, "EventType")))
            {

                verifygwResponse(true, Config.merchant_public_key);

            }
        }

        /// <summary>
        /// 按key获取get和post请求
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getRequestString(string key)
        {
            string value = null;
            if (Request.Form.Get(key) != null && Request.Form.Get(key).ToString() != "")
            {
                value = Request.Form.Get(key).ToString();
            }
            else if (Request.QueryString[key] != null && Request.QueryString[key].ToString() != "")
            {
                value = Request.QueryString[key].ToString();
            }

            return value;
        }

        /// <summary>/// 遍历Url中的参数列表/// </summary>
        /// <returns>如:(?userName=keleyi&userType=1)</returns>
        public string GetUrlParam(Dictionary<string, string> param)
        {
            string urlParam = "";
            if (param != null)
            {
                //urlParam = "?";

                foreach (string key in param.Keys)
                {
                    urlParam += key + "=" + param[key] + "&";
                }
                urlParam = urlParam.Substring(0, urlParam.LastIndexOf('&'));
            }
            return urlParam;
        }

        /// <summary>
        /// 获取xml中的事件类型
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public string getXmlNode(string xml, string node)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNodeList EventType = xmlDoc.GetElementsByTagName(node);
            string type = null;
            if (EventType.Count > 0)
            {
                type = xmlDoc.SelectSingleNode("//" + node).InnerText;
            }
            //Response.Output.WriteLine("EventType:" + EventType);
            return type;
        }

        /// <summary>
        /// 验证网关，签名内容并返回给支付宝xml
        /// </summary>
        /// <param name="_success"></param>
        /// <param name="merchantPubKey"></param>
        /// <returns></returns>
        public string verifygwResponse(bool _success, string merchantPubKey)
        {
            Response.ContentType = "text/xml";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.Clear();

            XmlDocument xmlDoc = new XmlDocument(); //创建实例
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmldecl);


            XmlElement xmlElem = xmlDoc.CreateElement("alipay"); //新建元素

            xmlDoc.AppendChild(xmlElem); //添加元素


            XmlNode alipay = xmlDoc.SelectSingleNode("alipay");
            XmlElement response = xmlDoc.CreateElement("response");
            XmlElement success = xmlDoc.CreateElement("success");
            if (_success)
            {
                success.InnerText = "true";//设置文本节点 
                response.AppendChild(success);//添加到<Node>节点中 
            }
            else
            {
                success.InnerText = "false";//设置文本节点 
                response.AppendChild(success);//添加到<Node>节点中 
                XmlElement err = xmlDoc.CreateElement("error_code");
                err.InnerText = "VERIFY_FAILED";
                response.AppendChild(err);
            }

            XmlElement biz_content = xmlDoc.CreateElement("biz_content");
            biz_content.InnerText = merchantPubKey;
            response.AppendChild(biz_content);

            alipay.AppendChild(response);

            string _sign = AlipaySignature.RSASign(response.InnerXml, Config.merchant_private_key, Config.charset, Config.signtype);

            XmlElement sign = xmlDoc.CreateElement("sign");
            sign.InnerText = _sign;
            alipay.AppendChild(sign);
            XmlElement sign_type = xmlDoc.CreateElement("sign_type");
            sign_type.InnerText = Config.signtype;
            alipay.AppendChild(sign_type);

            Response.Output.Write(xmlDoc.InnerXml);
            Response.End();

            return null;
        }

        /// <summary>
        /// 获取所有请求参数，转换为字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> getRequstParam()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (Request.QueryString != null)
            {
                foreach (string key in Request.QueryString.AllKeys)
                {
                    //log(key + " -:- " + Request.QueryString[key]);
                    // dict.Add(key, Request.QueryString[key]);
                }
            }

            if (Request.Form != null)
            {
                for (int i = 0; i < Request.Params.Count; i++)
                {
                    //  log(Request.Params.Keys[i].ToString() + " -:- " + Request.Params[i].ToString());
                    dict.Add(Request.Params.Keys[i].ToString(), Request.Params[i].ToString());
                }
            }



            return dict;
        }

        public void verifygw_success_response()
        {
            Response.ContentType = "text/xml";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.Clear();
            string resp = AlipaySignature.encryptAndSign("<success>true</success><biz_content>" + Config.merchant_public_key + "</biz_content>", Config.alipay_public_key, Config.merchant_private_key, "utf-8", false, true);
            Response.Output.WriteLine(resp);
            Response.End();
        }

    }
}