using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// MessageSend 的摘要说明
/// </summary>

namespace Alipay
{
    public class MessageSendBiz
    {
        public MessageSendBiz()
        {
        }


        //消息异步单发接口
        public static string CustomSend(string biz_content)
        {
            AlipayOpenPublicMessageCustomSendRequest pushRequst = new AlipayOpenPublicMessageCustomSendRequest();
            pushRequst.BizContent = biz_content;

            //Response.Output.WriteLine(biz_content);
            // Response.End();

            IAopClient client = new DefaultAopClient(Config.serverUrl, Config.appId, Config.merchant_private_key, "json", "1.0", Config.signtype, Config.alipay_public_key, Config.charset, false);
            AlipayOpenPublicMessageCustomSendResponse pushResponse = client.Execute(pushRequst);
            return pushResponse.Body;
        }


    }
}