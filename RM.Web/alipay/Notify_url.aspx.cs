using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aop.Api.Util;
using System.Collections.Specialized;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using System.Collections;

namespace RM.Web.alipay
{
    public partial class Notify_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /* 实际验证过程建议商户添加以下校验。
            1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
            2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
            3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
            4、验证app_id是否为该商户本身。
            */
            Dictionary<string, string> sArray = GetRequestPost();
            if (sArray.Count != 0)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(sArray);
                //Log.Info("支付宝异步返回内容", json);
                bool flag = AlipaySignature.RSACheckV1(sArray, config.alipay_public_key, config.charset, config.sign_type, false);
                //Log.Info("支付宝异步flag", flag.ToString());
                if (flag)
                {
                    //交易状态
                    //判断该笔订单是否在商户网站中已经做过处理
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //请务必判断请求时的total_amount与通知时获取的total_fee为一致的
                    //如果有做过处理，不执行商户的业务程序

                    //注意：
                    //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                    string trade_status = Request.Form["trade_status"];//交易状态
                    if (trade_status != "TRADE_SUCCESS")
                    {
                        Response.Write("fail");
                        Response.End();
                    }

                    string passback_params = sArray["passback_params"];//公共回传参数
                    string[] types = passback_params.Split(',');
                   // Log.Info("支付宝异步passback_params", passback_params + "------" + types.Length);
                    if (types.Length != 3)
                    {
                        Response.Write("fail");
                        Response.End();
                    }
                    string out_trade_no = sArray["out_trade_no"];//订单号OrderNumber
                    string trade_no = sArray["trade_no"];//支付宝交易号
                    string total_amount = sArray["total_amount"];//订单金额
                    string buyer_pay_amount = sArray["buyer_pay_amount"];//用户在交易中支付的金额，单位为元，精确到小数点后2位
                    string body = sArray["body"];//商品描述
                    string subject = sArray["subject"];//订单标题
                    string buyer_id = sArray["buyer_id"];//用户支付宝号
                    bool IsOk = false;
                    switch (types[0])
                    {
                        case "yxfcz":
                           // Log.Info("支付宝异步out_trade_no", out_trade_no + "------" + total_amount + "---" + buyer_id);
                            IsOk = DataDeal.yxfcz(types, out_trade_no, trade_no, total_amount, buyer_pay_amount, body, subject, buyer_id);
                            break;
                        default:
                            break;
                    }

                    //Log.Info("支付宝异步IsOk", IsOk + "----" + trade_no);
                    if (IsOk)
                    {
                        Response.Write("success");
                        Response.End();
                    }
                    else
                    {
                        Response.Write("fail");
                        Response.End();
                    }
                }
                else
                {
                    Response.Write("fail");
                }
            }
        }

        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //coll = Request.Form;
            coll = Request.Form;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            return sArray;

        }
    }
}