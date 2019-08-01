using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using PMS;
using System.Text;
using LitJson;
using System.Collections;

namespace WxCallback.PMS
{
    /// <summary>
    /// api 的摘要说明
    /// </summary>
    public class com_api : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string Action = context.Request["action"];               //提交动作
            if (string.IsNullOrEmpty(Action))
            {
                return;
            }
            switch (Action)
            {
                case "Submit"://
                    Submit(context);
                    break;
                default:
                    break;
            }
        }

        private void Submit(HttpContext context)
        {
            try
            {
                string[] path = context.Request["path"].Split(new char[1] { '/' });
                string className = path[2].Trim();
                string methodName = path[3].Trim();
                Type t = Type.GetType("PMS.api." + className);
                object instance = Activator.CreateInstance(t);
                MethodInfo methodInfo = t.GetMethod(methodName);
                if (instance == null || methodInfo == null)
                    throw new PMSException("api不存在");
                System.IO.StreamReader stream = new System.IO.StreamReader(context.Request.InputStream, Encoding.UTF8);
                string postData = stream.ReadToEnd();
                stream.Close();
                JsonData jd = new JsonData();//返回JsonData
                string[] key_val = postData.Split('&');
                for (int i = 0; i < key_val.Length; i++)
                {
                    if (key_val[i].Trim() != "")
                    {
                        string[] keys = key_val[i].Split('=');
                        jd[keys[0]] = keys[1];
                    }
                }
                postData = jd.ToJson();
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


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}