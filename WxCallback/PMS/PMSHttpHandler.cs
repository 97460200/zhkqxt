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

namespace PMS
{
    //====================================================================================
    // url: ip/PMS/类名/方法名
    // 统一JSON格式 正确{"code":"0" },错误{"code":"非0","message":"错误消息"}
    //=====================================================================================
    /// <summary>
    /// 消息与事件 PMS 入口类,
    /// </summary>
    public class PMSHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        string log_Folder = "PMS";

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string[] path = context.Request.Path.Split(new char[1] { '/' });
                string className = path[2].Trim();
                string methodName = path[3].Trim();

                System.IO.StreamReader stream = new System.IO.StreamReader(context.Request.InputStream, Encoding.UTF8);
                string postData = stream.ReadToEnd();
                stream.Close();

                ////写入日志
                JobLogs.Writer(log_Folder, "POST数据:" + postData);

                //认证请求  
                if (!IsAuthentication(context, className, methodName))
                    throw new PMSException("认证失败");

                Type t = Type.GetType("PMS.api." + className);
                object instance = Activator.CreateInstance(t);

                MethodInfo methodInfo = t.GetMethod(methodName);
                if (instance == null || methodInfo == null)
                    throw new PMSException("api不存在");

                methodInfo.Invoke(instance, new object[] { context, context.Server.UrlDecode(postData) });

            }
            catch (PMSException ex)
            {
                int errCode = (int)ex.ReCode;
                string message = string.Empty;
                if (ex.ReCode == PMSCode.ERROR)
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
                PMSResponse.WirterString(json);
            }
            catch (Exception ex)
            {
                PMSException ex_ie = ex.InnerException as PMSException;
                int errCode = 0;
                string message = string.Empty;

                if (ex_ie != null) //如果是自定义异常抛出
                {
                    errCode = (int)ex_ie.ReCode;
                    if (ex_ie.ReCode == PMSCode.ERROR)
                    {
                        message = ex_ie.Message; //直接输出消息
                    }
                    else
                    {
                        message = PMSResponse.GetEnumDesc(ex_ie.ReCode);
                    }
                }
                else
                {
                    errCode = -1;
                    message = "系统异常," + ex.Message;
                }
                Hashtable ht = new Hashtable();
                ht.Add("code", errCode);
                ht.Add("message", message);
                string json = LitJson.JsonMapper.ToJson(ht);
                PMSResponse.WirterString(json);
            }
        }


        private bool IsAuthentication(HttpContext context, string className, string methodName)
        {
            //这里需要做安全认证：IP白名单，ID + js

            return true;
        }


    }
}
