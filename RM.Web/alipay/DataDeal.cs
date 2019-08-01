using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using System.Collections;

namespace RM.Web.alipay
{
    public static class DataDeal
    {
        public static bool yxfcz(string[] types, string out_trade_no, string trade_no, string total_amount, string buyer_pay_amount, string body, string subject, string buyer_id)
        {
            string adminHotelId = types[1];
            string hotelId = types[2];
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT 1 FROM Hotel_Account WHERE TypeNum = 8 AND AdminHotelId = @AdminHotelId AND HotelId = @HotelId AND OrderNumber = @OrderNumber AND FlowNumber = @FlowNumber ");
            List<SqlParam> ls = new List<SqlParam>();
            ls.Add(new SqlParam("@AdminHotelId", adminHotelId));
            ls.Add(new SqlParam("@HotelId", hotelId));
            ls.Add(new SqlParam("@OrderNumber", out_trade_no));
            ls.Add(new SqlParam("@FlowNumber", trade_no));
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, ls.ToArray());
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            Hashtable ht = new Hashtable();
            ht["AdminHotelId"] = adminHotelId;
            ht["HotelId"] = hotelId;
            ht["RechargeMethod"] = "支付宝充值";
            ht["TypeNum"] = "8";//营销账户充值类型值
            ht["TypeExplain"] = "营销账户充值";//营销账户充值
            ht["Bank"] = "支付宝";
            ht["AccountName"] = "";
            ht["BankNumber"] = buyer_id;
            ht["OrderNumber"] = out_trade_no;//订单号
            ht["FlowNumber"] = trade_no;//流水号
            ht["Receipts"] = buyer_pay_amount;
            ht["CreateTime"] = DateTime.Now;
            ht["Remarks"] = body;
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Hotel_Account", "ID", "", ht);
            return IsOk;
        }


    }
}