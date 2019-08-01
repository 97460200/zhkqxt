using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RM.Web.Lib;

namespace RM.Web.WX_SET
{
    public class WXRed_envelope
    {
        /// <summary>
        /// 企业发红包
        /// </summary>
        /// <param name="send_name">商户名称</param>
        /// <param name="openid">用户OPENID</param>
        /// <param name="total_amount">金额</param>
        /// <param name="AdminHotelid">酒店全局ID</param>
        /// <returns></returns>
        public static string GetCachred(string send_name, string openid, int total_amount, string AdminHotelid, int Hotelid = 0)
        {
            WxPayData data = new WxPayData();
            data.SetValue("send_name", send_name);//商户名称
            data.SetValue("re_openid", openid);//用户openid
            data.SetValue("total_amount", total_amount);//付款金额
            data.SetValue("total_num", 1);//红包发放总人数
            data.SetValue("wishing", "恭喜您获得红包一个！");//红包祝福语
            data.SetValue("client_ip", HttpContext.Current.Request.UserHostAddress);//Ip地址
            data.SetValue("act_name", "现金红包");//活动名称
            data.SetValue("remark", "关注公众号获得更多优惠信息！");//备注
            if (total_amount >= 200)
            {
                data.SetValue("scene_id", "PRODUCT_1");//场景id
            }
            WxPayData result = Refund(data, AdminHotelid, Hotelid);//红包接口，接收返回数据

            Log.Info("Refund", "Refund process complete, result : " + result.ToXml());
            return result.ToXml();
        }


        private static WxPayData Refund(WxPayData inputObj, string AdminHotelid, int Hotelid = 0, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
            inputObj.SetValue("mch_billno", WxPayApi.GenerateOutTradeNo(AdminHotelid, Hotelid));//商户订单号
            inputObj.SetValue("wxappid", WxPayConfig.APPID(AdminHotelid, Hotelid));//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID(AdminHotelid, Hotelid));//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign(AdminHotelid, Hotelid));//签名

            string xml = inputObj.ToXml();
            var start = DateTime.Now;

            Log.Debug("WxPayApi", "Refund request : " + xml);
            string response = HttpService.Post(xml, url, true, timeOut, AdminHotelid, Hotelid);//调用HTTP通信接口提交数据到API
            Log.Debug("WxPayApi", "Refund response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            return result;
        }

        /// <summary>
        /// 企业付款到零钱包
        /// </summary>
        /// <param name="openid">用户OPENID</param>
        /// <param name="amount">金额</param>
        /// <param name="AdminHotelid">酒店全局ID</param>
        /// <returns></returns>
        public static string transfers_user(string openid, int amount, string AdminHotelid, int Hotelid = 0)
        {
            WxPayData data = new WxPayData();
            data.SetValue("openid", openid);//用户openid
            data.SetValue("check_name", "NO_CHECK");//校验用户姓名选项
            data.SetValue("amount", amount);//企业付款金额，单位为分
            data.SetValue("desc", "奖金提现！");//红包祝福语
            data.SetValue("spbill_create_ip", HttpContext.Current.Request.UserHostAddress);//Ip地址

            string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
            WxPayData result = Refunds(data, AdminHotelid, url, Hotelid);//红包接口，接收返回数据

            Log.Info("奖金提现", result.ToXml());
            return result.ToXml();
        }

        /// <summary>
        /// 企业付款到零钱包
        /// </summary>
        /// <param name="inputObj"></param>
        /// <param name="AdminHotelid"></param>
        /// <param name="url"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        private static WxPayData Refunds(WxPayData inputObj, string AdminHotelid, string url, int Hotelid = 0, int timeOut = 6)
        {
            inputObj.SetValue("mch_appid", WxPayConfig.APPID(AdminHotelid, Hotelid));//商户账号appid         
            inputObj.SetValue("mchid", WxPayConfig.MCHID(AdminHotelid, Hotelid));//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            inputObj.SetValue("partner_trade_no", WxPayApi.GenerateOutTradeNo(AdminHotelid, Hotelid));//商户订单号
            inputObj.SetValue("sign", inputObj.MakeSignOwn(AdminHotelid, Hotelid));//签名

            string xml = inputObj.ToXml();
            var start = DateTime.Now;

            string response = HttpService.PostOwn(xml, url, true, timeOut, AdminHotelid, Hotelid);//调用HTTP通信接口提交数据到API
            Log.Debug("企业付款到零钱包response:", response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            return result;
        }

    }
}