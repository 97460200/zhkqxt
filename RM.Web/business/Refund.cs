using System;
using System.Collections.Generic;
using System.Web;
using RM.Web.Lib;

namespace RM.Web.business
{
    public class Refund
    {
        /***
        * 申请退款完整业务流程逻辑
        * @param transaction_id 微信订单号（优先使用）
        * @param out_trade_no 商户订单号
        * @param total_fee 订单总金额
        * @param refund_fee 退款金额
        * @return 退款结果（xml格式）
        */
        public static string Run(string transaction_id, string out_trade_no, string total_fee, string refund_fee, string AdminHotelid, int hotelid)
        {
            WxPayData data = new WxPayData();
            if (!string.IsNullOrEmpty(transaction_id))//微信订单号存在的条件下，则已微信订单号为准
            {
                data.SetValue("transaction_id", transaction_id);
            }
            else//微信订单号不存在，才根据商户订单号去退款
            {
                data.SetValue("out_trade_no", out_trade_no);
            }
            data.SetValue("total_fee", int.Parse(total_fee));//订单总金额
            data.SetValue("refund_fee", int.Parse(refund_fee));//退款金额
            data.SetValue("out_refund_no", WxPayApi.GenerateOutTradeNo(AdminHotelid, hotelid));//随机生成商户退款单号
            data.SetValue("op_user_id", WxPayConfig.MCHID(AdminHotelid, hotelid));//操作员，默认为商户号
            WxPayData result = WxPayApi.Refund(data, AdminHotelid, hotelid);//提交退款申请给API，接收返回数据
            //Log.Info("Refund", "Refund process complete, result : " + result.ToXml());
            return result.ToXml();
        }
        /// <summary>
        /// 下载资金账单
        /// </summary>
        /// <param name="AdminHotelid"></param>
        /// <param name="hotelid"></param>
        /// <param name="bill_date"></param>
        /// <returns></returns>
        public static string DownloadFundFlow(string AdminHotelid, int hotelid, string bill_date)
        {
            WxPayData data = new WxPayData();
            data.SetValue("bill_date", bill_date);//资金账单日期
            WxPayData result = WxPayApi.DownloadFundFlow(data, AdminHotelid, hotelid);//提交退款申请给API，接收返回数据
            return result.ToXml();
        }
        /// <summary>
        /// 下载对账单
        /// </summary>
        /// <param name="AdminHotelid"></param>
        /// <param name="hotelid"></param>
        /// <param name="bill_date"></param>
        /// <returns></returns>
        public static string DownloadBill(string AdminHotelid, int hotelid, string bill_date)
        {
            WxPayData data = new WxPayData();
            data.SetValue("bill_date", bill_date);//资金账单日期
            data.SetValue("bill_type", "ALL");//账单类型
            WxPayData result = WxPayApi.DownloadBill(data, AdminHotelid, hotelid);//提交退款申请给API，接收返回数据
            return result.ToXml();
        }
    }
}