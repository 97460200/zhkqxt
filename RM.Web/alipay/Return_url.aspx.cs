using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aop.Api.Util;
using System.Collections.Specialized;

namespace RM.Web.alipay
{
    public partial class Return_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /* 实际验证过程建议商户添加以下校验。
            1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
            2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
            3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
            4、验证app_id是否为该商户本身。
            */
            Dictionary<string, string> sArray = GetRequestGet();
            Dictionary<string, string> sArrayp = GetRequestPost();
            if (sArray.Count != 0)
            {

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(sArray);
                //Log.Info("支付宝同步返回内容", json);
                string jsonp = Newtonsoft.Json.JsonConvert.SerializeObject(sArrayp);
                //Log.Info("支付宝同步返回内容ppp", jsonp);

                bool flag = AlipaySignature.RSACheckV1(sArray, config.alipay_public_key, config.charset, config.sign_type, false);
                if (flag)
                {
                    Response.Write("同步验证通过");
                }
                else
                {
                    Response.Write("同步验证失败");
                    //total_amount=0.01&
                    //timestamp=2018-08-14+09%3a12%3a42&
                    //sign=d230qm+AkwuGMz9lfEn0pdcPgGxii7%2fGd%2f8gBMHjVvaS8fcAXI1Iv0ktH29yXq0OWYNQte69xV0fUjc1t1jsrIDp6J6uZ5X5YhVKf1hRQCR31M7i5PMAPf7yyDBIImjesaYSAp+udGPJImnpbxNziAOn53sxQHgv2EkZEYcBzQp6jRGinmZoPX7WYTpx+uzRpSlQxtFM080aB3S%2fHCHujbVq+GQbW%2fC6OAUFTe2IzVRHOLfOd0WSgojMqU9d6cq0Jci+oZN7RB5PLCTSY5sJosPKONqzpizqbIl81vicQnp7OM+1WFB3cMZdUr1BUXbJw2Wuf6IcJtI649klWbHUyQ%3d%3d&
                    //trade_no=2018081421001004740535762277&
                    //sign_type=RSA2&
                    //auth_app_id=2018080860963479&
                    //charset=UTF-8&
                    //seller_id=2088002326374601&
                    //method=alipay.trade.page.pay.return&
                    //app_id=2018080860963479&
                    //out_trade_no=YXF201808140911579066&
                    //version=1.0
                }
            }
        }

        public Dictionary<string, string> GetRequestGet()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //coll = Request.Form;
            coll = Request.QueryString;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }
            return sArray;

        }
        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            coll = Request.Form;
            //coll = Request.QueryString;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }
            return sArray;

        }
    }
}