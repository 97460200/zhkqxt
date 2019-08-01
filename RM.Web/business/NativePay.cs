using System;
using System.Collections.Generic;
using System.Web;
using SQL;
using System.Data;
using RM.Web.Lib;

namespace RM.Web.business
{

    public class NativePay
    {
        #region ** 参数 **

        /// <summary>
        /// 商品金额，用于统一下单
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 附加信息 原路返回
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string order { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string detail { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public string productId { get; set; }
        #endregion

        /**
        * 生成扫描支付模式一URL
        * @param productId 商品ID
        * @return 模式一URL
        */
        public string GetPrePayUrl(string productId, string AdminHotelid, int Hotelid = 0)
        {
            Log.Info(this.GetType().ToString(), "Native pay mode 1 url is producing...");

            WxPayData data = new WxPayData();
            data.SetValue("appid", WxPayConfig.APPID(AdminHotelid, Hotelid));//公众帐号id
            data.SetValue("mch_id", WxPayConfig.MCHID(AdminHotelid, Hotelid));//商户号
            data.SetValue("time_stamp", WxPayApi.GenerateTimeStamp());//时间戳
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("sign", data.MakeSign(AdminHotelid, Hotelid));//签名
            string str = ToUrlParams(data.GetValues());//转换为URL串
            string url = "weixin://wxpay/bizpayurl?" + str;

            Log.Info(this.GetType().ToString(), "Get native pay mode 1 url : " + url);
            return url;
        }

        /**
        * 生成直接支付url，支付url有效期为2小时,模式二
        * @param productId 商品ID
        * @return 模式二URL
        */
        public string GetPayUrl(string adminHotelId, int hotelId)
        {
            //查询订单信息

            WxPayData data = new WxPayData();

            data.SetValue("body", body);//商品描述
            data.SetValue("attach", attach);//附加数据
            data.SetValue("detail", detail);//商品详情
            data.SetValue("out_trade_no", order);//随机字符串
            data.SetValue("total_fee", total_fee);//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddHours(2).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", productId);//商品ID

            WxPayData result = WxPayApi.UnifiedOrder(data, adminHotelId, hotelId);//调用统一下单接口
            if (!result.IsSet("code_url"))
            {
                if (result.IsSet("return_msg"))
                {
                    string msg = result.GetValue("return_msg").ToString();
                    throw new WxPayException(msg);
                }
                throw new WxPayException("返回二维码链接失败!");
            }
            string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
            return url;
        }

        /**
        * 参数数组转换为url格式
        * @param map 参数名与参数值的映射表
        * @return URL字符串
        */
        private string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }
    }
}