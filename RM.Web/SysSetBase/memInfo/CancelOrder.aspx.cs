using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.business;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using RM.Web.Lib;
using RM.Common.DotNetUI;
using System.Collections;

namespace RM.Web.SysSetBase.memInfo
{
    public partial class CancelOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    hdId.Value = Request.QueryString["Id"];
                    Bind();
                }
            }
        }

        private void Bind()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            select * from Finance where id= @ID
            ");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@ID", hdId.Value)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                spOrderNum.InnerHtml = dt.Rows[0]["Number"].ToString();
                spTomePrice.InnerHtml= dt.Rows[0]["Monery"].ToString();


                hdPayType.Value = "4";
                

            }
        }

        protected void btnSumit_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
           select * from Finance where id= @ID
            ");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@ID", hdId.Value)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                string adminHotelid = dt.Rows[0]["AdminHotelid"].ToString();
                int hotelid = CommonHelper.GetInt(dt.Rows[0]["HotelId"].ToString());

                string orderNum = dt.Rows[0]["Number"].ToString();
                string name = dt.Rows[0]["Name"].ToString();
                string phone = dt.Rows[0]["Phone"].ToString();
                string memberId = dt.Rows[0]["MemberId"].ToString();
                string openid = dt.Rows[0]["openid"].ToString();


                int money = CommonHelper.GetInt(dt.Rows[0]["Monery"].ToString());//订单金额

                
                double cancel_money = CommonHelper.GetDouble(txtMoney.Text);//取消金额

                
                if (cancel_money > money)
                {
                    ShowMsgHelper.Alert_Error("操作失败,退回金额不能大于订单支付金额！");
                    return;
                }
                if (cancel_money > 0)//退回金额
                {
                    switch (hdPayType.Value)
                    {
                        case "1"://到店支付

                            break;
                        case "2"://积分兑换

                            break;
                        case "3"://会员卡支付

                            break;
                        case "4"://微信支付
                            #region ******** 微信原路退回 ********
                            try
                            {
                                string transaction_id = dt.Rows[0]["wxddh"].ToString();
                                if (transaction_id == "")
                                {
                                    ShowMsgHelper.Alert_Error("操作失败,未找到微信支付单号！");
                                    return;
                                }
                                string ssl_path = WxPayConfig.SSLCERT_PATH(adminHotelid, hotelid);
                                if (ssl_path == "")
                                {
                                    ShowMsgHelper.Alert_Error("操作失败,请联系开发人员配置API证书！");
                                    return;
                                }
                                //判断文件的存在
                                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~" + ssl_path)))
                                {
                                    //存在文件
                                }
                                else
                                {
                                    ShowMsgHelper.Alert_Error("操作失败,API证书不存在！");
                                    return;
                                }
                                string total_fee = (money * 100).ToString();
                                string refund_fee = (cancel_money * 100).ToString();

                                string result = Refund.Run(transaction_id, orderNum, total_fee, refund_fee, adminHotelid, hotelid);
                                WxPayData data = new WxPayData();
                                data.FromXml(result);//把XML转换成对象
                                string GetReturnCode = data.GetValue("return_code").ToString();
                                if (GetReturnCode == "SUCCESS")
                                {
                                    string GetResultCode = data.GetValue("result_code").ToString();
                                    if (GetResultCode == "SUCCESS")
                                    {
                                        string refund_id = data.GetValue("refund_id").ToString();
                                        Dictionary<string, object> datas = new Dictionary<string, object>();
                                        datas.Add("transaction_id", data.GetValue("transaction_id"));//微信订单号
                                        datas.Add("out_trade_no", data.GetValue("out_trade_no")); //商户订单号
                                        datas.Add("out_refund_no", data.GetValue("out_refund_no")); //商户退款单号
                                        datas.Add("refund_id", data.GetValue("refund_id")); //微信退款单号

                                        //财务记录
                                        int iVal = CommonClass.InsertFinance(orderNum, name, phone, cancel_money, 41, "充值退款", memberId, transaction_id, adminHotelid, hotelid, refund_id, openid);
                                        if (iVal > 0)
                                        {
                                            try
                                            {
                                                Hashtable hs = new Hashtable();
                                                hs["vipkh"] = dt.Rows[0]["Phone"];
                                                hs["bmcode"] = "99";
                                                hs["yddh"] = orderNum;
                                                hs["je"] = CommonHelper.GetDouble(cancel_money);
                                                //hs["fhcs"] = "T";
                                                hs["OUT_fhcs"] = null;
                                                Hashtable rs = new Hashtable();
                                                DataFactory.SqlDataBase(adminHotelid).ExecuteByProcReturn("PROC_WXCZKK", hs, ref rs);
                                            }
                                            catch (Exception ex) 
                                            {
                                                
                                            }
                                           CommonMethod.Base_Log("充值退款", "Reservation", "ID:" + hdId.Value, "退款金额:" + cancel_money + "，" + txtReason.Text, "订单编号:" + orderNum);
                                        }
                                    }
                                    else
                                    {
                                        //https://pay.weixin.qq.com/wiki/doc/api/micropay.php?chapter=9_4
                                        if (GetResultCode == "NOTENOUGH")//余额不足
                                        {
                                            ShowMsgHelper.ShowScript("cancelTips()");
                                            return;
                                        }
                                        ShowMsgHelper.Alert_Error("操作失败," + data.GetValue("err_code_des") + "！");
                                        return;
                                    }
                                }
                                else
                                {
                                    ShowMsgHelper.Alert_Error("签名验证失败！");
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowMsgHelper.Alert_Error("操作错误,签名验证失败");
                                return;
                            }
                            #endregion
                            break;
                        default:
                            ShowMsgHelper.Alert_Error("操作失败！");
                            return;
                            break;
                    }
                }

                Hashtable ht = new Hashtable();
                //ht["CancelNumber"] = cancel_night;
                ht["CancelTime"] = DateTime.Now;
                //string qxtxt = cancel_night + "间夜";

                CommonMethod.Base_Log("充值退款", "Finance", "ID:" + hdId.Value, "退款原因：" + txtReason.Text, "订单编号:" + orderNum);
                //int val = DataFactory.SqlDataBase().UpdateByHashtable("Reservation", "id", hdId.Value, ht);
                //if (val > 0)
                //{
                ShowMsgHelper.OpenClose("退款成功！");
                //}

                
            }
        }
    }
}