using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;
using System.Data;
using RM.Web.Lib;
using System.Collections;
using RM.Busines;
using System.Text;
using RM.Common.DotNetCode;
using RM.Web.WX_SET;
using LitJson;
using System.Net;

namespace RM.Web.business
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify : Notify
    {
        public ResultNotify(Page page)
            : base(page)
        {
        }

        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error("订单号不存在", res.ToXml());
                page.Response.Write(res.ToXml());
                return;
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            Log.Info("支付订单信息", notifyData.GetValue("attach").ToString());
            //填写自己的逻辑判断  a[0]集团id，a[1]酒店id，a[2]类型，a[3]订单编号...
            string[] a = notifyData.GetValue("attach").ToString().Split(',');
            string AdminHotelid = a[1];
            int Hotelid = 0;
            string payType = a[2];

            if (payType == "客房预订" || payType == "客房续住" || a.Length > 3)
            {
                Hotelid = CommonHelper.GetInt(a[3]);
            }
            if (payType == "移动PMS续费" || payType == "充值预留款" || payType == "自助入住机")
            {
                AdminHotelid = a[0];
                Hotelid = CommonHelper.GetInt(a[1]);
            }

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id, AdminHotelid, Hotelid))
            {
                //若订单查询失败，则立即返回结果给微信支付后台

                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Log.Error("订单查询失败", res.ToXml());
                page.Response.Write(res.ToXml());
                return;
            }
            //查询订单成功
            else
            {
                if (payType == "客房预订")
                {
                    wxzf(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }
                else if (payType == "客房续住")
                {
                    wxzfxz(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }
                else if (payType == "服务预订")
                {
                    wxfwzf(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }
                else if (payType == "点餐预订")
                {
                    wxdczf(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }
                else if (payType == "商品预订")
                {
                    wxspzf(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }
                else if (payType == "会员卡充值")
                {
                    hykcz(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }
                else if (payType == "会员卡升级")
                {
                    hyksj(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }
                else if (payType == "注册购买会员卡")
                {
                    string openid = notifyData.GetValue("openid").ToString();
                    hyzccz(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100, openid);
                }
                else if (payType == "预售券预订")
                {
                    TicketOrder(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }
                else if (payType == "押金订单")
                {
                    CashPledgeOrder(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }
                else if (payType == "移动PMS续费")
                {
                    PMS_RenewFee(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }
                else if (payType == "充值预留款")
                {
                    ReserveMoney(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }
                else if (payType == "自助入住机")
                {
                    SelfHelpPay(a, transaction_id, CommonHelper.GetDouble(notifyData.GetValue("total_fee").ToString()) / 100);
                }

                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                page.Response.Write(res.ToXml());
                return;
            }
        }


        #region ******** 自助入住机 ********
        /// <summary>
        /// 自助入住机
        /// </summary>
        /// <param name="a"></param>
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        public void SelfHelpPay(string[] a, string transaction_id, double money)
        {
            try
            {
                string OrderNum = a[3];
                SqlParam[] spOrderNum = new SqlParam[] { 
                    new SqlParam("@OrderNum",OrderNum)
                };
                StringBuilder sqlOrder = new StringBuilder();
                sqlOrder.Append("SELECT * FROM SelfHelpPay WHERE OrderNumber = @OrderNum ");
                DataTable orderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(sqlOrder, spOrderNum);
                if (orderInfo != null && orderInfo.Rows.Count > 0)
                {
                    Hashtable ht = new Hashtable();
                    ht["PayMoney"] = money;
                    ht["PayState"] = "1";
                    DataFactory.SqlDataBase().UpdateByHashtable("SelfHelpPay", "OrderNumber", OrderNum, ht);
                }
                else
                {
                    Log.Info("充值预留款", "未找到订单!");
                }
            }
            catch (Exception e)
            {
                Log.Info("充值预留款", e.Message);
            }
        }

        #endregion

        #region ******** 充值预留款 ********
        /// <summary>
        /// 充值预留款
        /// </summary>
        /// <param name="a"></param>
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        public void ReserveMoney(string[] a, string transaction_id, double money)
        {
            try
            {
                string OrderNum = a[3];

                SqlParam[] spOrderNum = new SqlParam[] { 
                    new SqlParam("@OrderNum",OrderNum)
                };
                StringBuilder sqlFinance = new StringBuilder();
                sqlFinance.Append("select id,number from Finance where number = @OrderNum ");
                DataTable dtFinance = DataFactory.SqlDataBase().GetDataTableBySQL(sqlFinance, spOrderNum);
                if (dtFinance != null && dtFinance.Rows.Count > 0)
                {
                    return;
                }
                else
                {
                    StringBuilder sqlOrder = new StringBuilder();
                    sqlOrder.Append("SELECT * FROM ReserveMoney WHERE OrderNumber = @OrderNum ");

                    DataTable orderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(sqlOrder, spOrderNum);

                    if (orderInfo != null && orderInfo.Rows.Count > 0)
                    {
                        string adminHotelid = orderInfo.Rows[0]["AdminHotelId"].ToString();
                        string hotelId = orderInfo.Rows[0]["HotelId"].ToString();
                        string name = "";
                        string phone = "";
                        string memberId = "";
                        string openid = orderInfo.Rows[0]["Openid"].ToString();
                        //财务记录
                        int iss = CommonClass.InsertFinance(OrderNum, name, phone, money, 120, "微信商户平台预留款", memberId, transaction_id, adminHotelid, CommonHelper.GetInt(hotelId), "", openid);
                        if (iss > 0)
                        {
                            #region 发送消息
                            //CommonMethod.Message_TaoPiao(OrderNum);//推送模板消息给 预订人
                            //CommonMethod.SendMessage_ZDY(OrderNum);//推送消息给智订云管理员及员工
                            #endregion
                        }
                    }
                    else
                    {
                        Log.Info("充值预留款", "未找到订单!");
                    }
                }
            }
            catch (Exception e)
            {
                Log.Info("充值预留款", e.Message);
            }
        }

        #endregion

        #region ******** 移动PMS续费 ********
        /// <summary>
        /// 移动PMS续费
        /// </summary>
        /// <param name="a"></param>
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        public void PMS_RenewFee(string[] a, string transaction_id, double money)
        {
            try
            {
                string OrderNum = a[3];

                SqlParam[] spOrderNum = new SqlParam[] { 
                    new SqlParam("@OrderNum",OrderNum)
                };
                StringBuilder sqlFinance = new StringBuilder();
                sqlFinance.Append("select id,number from Finance where number = @OrderNum ");
                DataTable dtFinance = DataFactory.SqlDataBase().GetDataTableBySQL(sqlFinance, spOrderNum);
                if (dtFinance != null && dtFinance.Rows.Count > 0)
                {
                    return;
                }
                else
                {
                    StringBuilder sqlOrder = new StringBuilder();
                    sqlOrder.Append("SELECT * FROM PMS_RenewFee WHERE OrderNumber = @OrderNum ");

                    DataTable orderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(sqlOrder, spOrderNum);

                    #region 微信支付订单处理
                    if (orderInfo == null || orderInfo.Rows.Count <= 0)
                    {
                        StringBuilder sbTemp = new StringBuilder();
                        sbTemp.Append("select PayMoney  from PMS_RenewFeeTemp where OrderNumber = @OrderNum");
                        DataTable dtTemp = DataFactory.SqlDataBase().GetDataTableBySQL(sbTemp, spOrderNum);
                        if (dtTemp != null && dtTemp.Rows.Count > 0)
                        {
                            if (money != CommonHelper.GetDouble(dtTemp.Rows[0]["PayMoney"]))//判断支付金额是否一致
                            {
                                Log.Info("支付金额错误：", "应付" + dtTemp.Rows[0]["PayMoney"] + ",实付" + money + "。");
                                return;
                            }
                        }

                        StringBuilder sqlInsertOrder = new StringBuilder();
                        sqlInsertOrder.AppendFormat(@"
                        Insert  Into PMS_RenewFee
                                ( AdminHotelId ,
                                  HotelId ,
                                  UserId ,
                                  FeeType ,
                                  Years ,
                                  StartDate ,
                                  EndDate ,
                                  OrderNumber ,
                                  PayWay ,
                                  PayMoney ,
                                  PayNumber ,
                                  Remarks ,
                                  Openid )
                                Select  AdminHotelId ,
                                        HotelId ,
                                        UserId ,
                                        FeeType ,
                                        Years ,
                                        StartDate ,
                                        EndDate ,
                                        OrderNumber ,
                                        PayWay ,
                                        PayMoney ,
                                        '{0}' ,
                                        Remarks ,
                                        Openid
                                From    PMS_RenewFeeTemp Where OrderNumber = @OrderNum ", transaction_id);
                        int i = DataFactory.SqlDataBase().ExecuteBySql(sqlInsertOrder, spOrderNum);
                        if (i > 0)
                        {
                            orderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(sqlOrder, spOrderNum);
                        }
                    }
                    #endregion
                    if (orderInfo != null && orderInfo.Rows.Count > 0)
                    {
                        string adminHotelid = orderInfo.Rows[0]["AdminHotelId"].ToString();
                        string hotelId = orderInfo.Rows[0]["HotelId"].ToString();
                        string name = "";
                        string phone = "";
                        string memberId = "";
                        string openid = orderInfo.Rows[0]["Openid"].ToString();
                        //财务记录
                        int iss = CommonClass.InsertFinance(OrderNum, name, phone, money, 110, "移动PMS续费", memberId, transaction_id, adminHotelid, CommonHelper.GetInt(hotelId), "", openid);
                        if (iss > 0)
                        {
                            #region 发送消息
                            //CommonMethod.Message_TaoPiao(OrderNum);//推送模板消息给 预订人
                            //CommonMethod.SendMessage_ZDY(OrderNum);//推送消息给智订云管理员及员工
                            #endregion
                        }
                    }
                    else
                    {
                        Log.Info("移动PMS续费(PMS_RenewFee)", "未找到订单!");
                    }
                }

            }
            catch (Exception e)
            {
                Log.Info("移动PMS续费(PMS_RenewFee)", e.Message);
            }
        }

        #endregion

        #region ******** 预售券预订 ********
        /// <summary>
        /// 预售券预订
        /// </summary>
        /// <param name="a"></param>
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        public void TicketOrder(string[] a, string transaction_id, double money)
        {
            try
            {
                string OrderNum = a[0];
                SqlParam[] spOrderNum = new SqlParam[] { 
                    new SqlParam("@OrderNum",OrderNum)
                };
                StringBuilder sqlFinance = new StringBuilder();
                sqlFinance.Append("select id,number from Finance where number = @OrderNum ");
                DataTable dtFinance = DataFactory.SqlDataBase().GetDataTableBySQL(sqlFinance, spOrderNum);
                if (dtFinance != null && dtFinance.Rows.Count > 0)
                {
                    return;
                }
                else
                {
                    StringBuilder sqlOrder = new StringBuilder();
                    sqlOrder.Append("SELECT  * FROM TP_Order WHERE OrderNum = @OrderNum ");

                    DataTable orderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(sqlOrder, spOrderNum);

                    #region 微信支付订单处理
                    if (orderInfo == null || orderInfo.Rows.Count <= 0)
                    {
                        StringBuilder sbTemp = new StringBuilder();
                        sbTemp.Append("select PayMoney  from TP_OrderTemp where OrderNum = @OrderNum");
                        DataTable dtTemp = DataFactory.SqlDataBase().GetDataTableBySQL(sbTemp, spOrderNum);
                        if (dtTemp != null && dtTemp.Rows.Count > 0)
                        {
                            if (money != CommonHelper.GetDouble(dtTemp.Rows[0]["PayMoney"]))//判断支付金额是否一致
                            {
                                Log.Info("支付金额错误：", "应付" + dtTemp.Rows[0]["PayMoney"] + ",实付" + money + "。");
                                return;
                            }
                        }

                        StringBuilder sqlInsertOrder = new StringBuilder();
                        sqlInsertOrder.Append(@"
                        INSERT  INTO TP_Order
                                ( AdminHotelId ,
                                    HotelId ,
                                    TicketId ,
                                    OrderNum ,
                                    Name ,
                                    Phone ,
                                    Number ,
                                    UnitPrice ,
                                    TotalPrice ,
                                    PayMoney ,
                                    PayType ,
                                    Pay ,
                                    Remarks ,
                                    StartDate ,
                                    EndDate ,
                                    State ,
                                    Openid ,
                                    PayOpenid ,
                                    MemberId ,
                                    VipCode ,
                                    StaffId ,
                                    StaffRemarks
                                )
                                SELECT  AdminHotelId ,
                                        HotelId ,
                                        TicketId ,
                                        OrderNum ,
                                        Name ,
                                        Phone ,
                                        Number ,
                                        UnitPrice ,
                                        TotalPrice ,
                                        PayMoney ,
                                        PayType ,
                                        Pay ,
                                        Remarks ,
                                        StartDate ,
                                        EndDate ,
                                        State ,
                                        Openid ,
                                        PayOpenid ,
                                        MemberId ,
                                        VipCode ,
                                        StaffId ,
                                        StaffRemarks
                                FROM    TP_OrderTemp
                                WHERE   OrderNum = @OrderNum ");
                        int i = DataFactory.SqlDataBase().ExecuteBySql(sqlInsertOrder, spOrderNum);
                        if (i > 0)
                        {
                            orderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(sqlOrder, spOrderNum);
                            StringBuilder sqldel = new StringBuilder();
                            sqldel.Append("DELETE TP_OrderTemp WHERE OrderNum = @OrderNum");
                            DataFactory.SqlDataBase().ExecuteBySql(sqldel, spOrderNum);
                        }
                    }
                    #endregion
                    if (orderInfo != null && orderInfo.Rows.Count > 0)
                    {
                        string adminHotelid = orderInfo.Rows[0]["AdminHotelid"].ToString();
                        string hotelId = orderInfo.Rows[0]["hotelid"].ToString();
                        string name = orderInfo.Rows[0]["name"].ToString();
                        string phone = orderInfo.Rows[0]["Phone"].ToString();
                        string memberId = orderInfo.Rows[0]["MemberId"].ToString();
                        string openid = orderInfo.Rows[0]["openid"].ToString();
                        //财务记录
                        int iss = CommonClass.InsertFinance(OrderNum, name, phone, money, 20, "预售券预订支付", memberId, transaction_id, adminHotelid, CommonHelper.GetInt(hotelId), "", openid);

                        if (iss > 0)
                        {
                            #region 发送消息
                            CommonMethod.Message_TaoPiao(OrderNum);//推送模板消息给 预订人
                            CommonMethod.TaoPiaoMeg_ZDY(OrderNum);//推送消息给智订云管理员及员工
                            #endregion

                            if (!CommonClass.CheckFunctionIsOpen(adminHotelid, "Wx_BonusGetMode"))//非自提
                            {
                                string postUrl = "http://zidinn.com/Members/Commision/api.ashx?Menu=automatic_withdraw&AdminHotelId=" + adminHotelid + "&OrderNum=" + OrderNum;
                                TemplateMessage.PostWebRequest(postUrl, "");
                            }
                        }
                    }
                    else
                    {
                        Log.Info("微信支付错误信息(wxzf)", "未找到订单!");
                    }
                }

            }
            catch (Exception e)
            {
                Log.Info("微信支付错误信息(wxzf)", e.Message);
            }
        }

        #endregion

        #region ******** 押金订单 ********
        /// <summary>
        /// 押金订单
        /// </summary>
        /// <param name="a"></param>
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        public void CashPledgeOrder(string[] a, string transaction_id, double money)
        {
            try
            {
                string OrderNum = a[0];
                SqlParam[] spOrderNum = new SqlParam[] { 
                    new SqlParam("@OrderNum",OrderNum)
                };
                StringBuilder sqlFinance = new StringBuilder();
                sqlFinance.Append("select id,number from Finance where number = @OrderNum ");
                DataTable dtFinance = DataFactory.SqlDataBase().GetDataTableBySQL(sqlFinance, spOrderNum);
                if (dtFinance != null && dtFinance.Rows.Count > 0)
                {
                    return;
                }
                else
                {
                    StringBuilder sqlOrder = new StringBuilder();
                    sqlOrder.Append("SELECT  * FROM CashPledge_Order WHERE PayNumber = @OrderNum ");

                    DataTable orderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(sqlOrder, spOrderNum);

                    #region 微信支付订单处理
                    if (orderInfo == null || orderInfo.Rows.Count <= 0)
                    {
                        StringBuilder sbTemp = new StringBuilder();
                        sbTemp.Append("select PayMoney  from CashPledge_TempOrder where PayNumber = @OrderNum");
                        DataTable dtTemp = DataFactory.SqlDataBase().GetDataTableBySQL(sbTemp, spOrderNum);
                        if (dtTemp != null && dtTemp.Rows.Count > 0)
                        {
                            if (money != CommonHelper.GetDouble(dtTemp.Rows[0]["PayMoney"]))//判断支付金额是否一致
                            {
                                Log.Info("支付金额错误：", "应付" + dtTemp.Rows[0]["PayMoney"] + ",实付" + money + "。");
                                return;
                            }
                        }

                        StringBuilder sqlInsertOrder = new StringBuilder();
                        sqlInsertOrder.Append(@"
                        INSERT  INTO CashPledge_Order
                                (
                                AdminHotelId ,
                                HotelId ,
                                State ,
                                PayType ,
                                PayNumber ,
                                PayMoney ,
                                ConsumeMoney ,
                                ConsumeRemarks ,
                                ReturnMoney ,
                                OrderNum ,
                                PayTime ,
                                SettleDate ,
                                Operator ,
                                Remarks ,
                                MemberId ,
                                Openid,
                                PayOpenid,
                                StaffId,
                                StaffRemarks
                                )
                                SELECT 
                                AdminHotelId ,
                                HotelId ,
                                1 ,
                                PayType ,
                                PayNumber ,
                                PayMoney ,
                                ConsumeMoney ,
                                ConsumeRemarks ,
                                ReturnMoney ,
                                OrderNum ,
                                PayTime ,
                                SettleDate ,
                                Operator ,
                                Remarks ,
                                MemberId ,
                                Openid ,
                                PayOpenid,
                                StaffId,
                                StaffRemarks
                                FROM    CashPledge_TempOrder
                                WHERE   PayNumber = @OrderNum ");
                        int i = DataFactory.SqlDataBase().ExecuteBySql(sqlInsertOrder, spOrderNum);
                        if (i > 0)
                        {
                            orderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(sqlOrder, spOrderNum);
                        }
                    }
                    #endregion
                    if (orderInfo != null && orderInfo.Rows.Count > 0)
                    {
                        string adminHotelid = orderInfo.Rows[0]["AdminHotelId"].ToString();
                        string hotelId = orderInfo.Rows[0]["HotelId"].ToString();
                        string name = "";
                        string phone = "";
                        string memberId = CommonHelper.GetInt(orderInfo.Rows[0]["MemberId"]).ToString();
                        string openid = orderInfo.Rows[0]["Openid"].ToString();
                        //财务记录
                        int iss = CommonClass.InsertFinance(OrderNum, name, phone, money, 40, "交押金", memberId, transaction_id, adminHotelid, CommonHelper.GetInt(hotelId), "", openid);

                        if (iss > 0)
                        {
                            #region 发送消息
                            //CommonMethod.Message_TaoPiao(OrderNum);//推送模板消息给 预订人
                            //CommonMethod.SendMessage_ZDY(OrderNum);//推送消息给智订云管理员及员工
                            #endregion

                        }
                    }
                    else
                    {
                        Log.Info("微信支付错误信息(wxzf)", "未找到订单!");
                    }
                }

            }
            catch (Exception e)
            {
                Log.Info("微信支付错误信息(wxzf)", e.Message);
            }
        }

        #endregion

        /// <summary>
        /// 注册购买会员卡
        /// </summary>
        /// <param name="a">传入参数值</param>
        /// <param name="transaction_id">微信订单号</param>
        /// <param name="money">充值金额</param>
        public void hyzccz(string[] a, string transaction_id, double money, string openid)
        {
            try
            {
                string Number = a[0];
                string AdminHotelid = a[1];

                int hotelid = CommonHelper.GetInt(a[3]);
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("SELECT Phone FROM dbo.MemberInfo WHERE Openid = '{0}'", openid);
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
                string phone = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    phone = dt.Rows[0]["Phone"].ToString();
                }
                CommonClass.InsertFinance(Number, "", phone, money, 10, "注册购买会员卡", "", transaction_id, AdminHotelid, hotelid, "", openid);
            }
            catch (Exception e)
            {
                Log.Info("注册购买会员卡错误信息", e.Message);
            }
        }

        private void InOrderQuery(string adminHotelId, int hotelId, string orderNumber, int type)
        {
            Hashtable ht_on = new Hashtable();
            if (type == 1)
            {
                ht_on["AdminHotelId"] = adminHotelId;
                ht_on["HotelId"] = hotelId;
                ht_on["OrderNumber"] = orderNumber;
                DataFactory.SqlDataBase().InsertByHashtable("OrderQuery", ht_on);
            }
            else if (type == 2)
            {
                ht_on["Finance"] = 1;
                DataFactory.SqlDataBase().Submit_AddOrEdit("OrderQuery", "OrderNumber", orderNumber, ht_on);
            }
            else if (type == 3)
            {
                ht_on["Finance"] = 1;
                ht_on["Success"] = 1;
                DataFactory.SqlDataBase().Submit_AddOrEdit("OrderQuery", "OrderNumber", orderNumber, ht_on);
            }
        }


        /// <summary>
        /// 会员卡充值
        /// </summary>
        /// <param name="a">传入参数值</param>
        /// <param name="transaction_id">微信订单号</param>
        /// <param name="money">充值金额</param>
        public void hykcz(string[] a, string transaction_id, double money)
        {
            try
            {
                string number = a[0];//订单号
                string adminHotelId = a[1];
                string typelx = a[2];
                int hotelId = CommonHelper.GetInt(a[3]);
                string gg_kh = a[4];//国光卡号
                string bmcode = a[5];
                InOrderQuery(adminHotelId, hotelId, number, 1);
                string sql = string.Format(@"select id,number from Finance where number='{0}'", number);
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds != null && ds.Rows.Count > 0)
                {
                    InOrderQuery(adminHotelId, hotelId, number, 2);
                }
                else
                {
                    string sqlhy = string.Format(@"select lsh,sjhm,xm,hykye,hylx,scmcode,kh,IsNull(hy_hylxbmb.jb, 0) jb,hy_hyzlxxb.Carid from hy_hyzlxxb left join hy_hylxbmb on  hy_hyzlxxb.hylx=hy_hylxbmb.hylxcode   where  kh='{0}' and hy_hyzlxxb.AdminHotelid='{1}' ", gg_kh, adminHotelId);
                    DataTable dshy = DataFactory.SqlDataBase(adminHotelId).GetDataTableBySQL(new StringBuilder(sqlhy));
                    if (dshy != null && dshy.Rows.Count > 0)
                    {
                        string name = dshy.Rows[0]["xm"].ToString();
                        string phone = dshy.Rows[0]["sjhm"].ToString();
                        string memberId = dshy.Rows[0]["lsh"].ToString();
                        string openid = dshy.Rows[0]["Carid"].ToString();
                        //财务记录
                        CommonClass.InsertFinance(number, name, phone, money, 4, "会员卡充值", memberId, transaction_id, adminHotelId, hotelId, "", openid);
                        try
                        {
                            //充值记录
                            Hashtable hshys = new Hashtable();
                            hshys["Name"] = name;
                            hshys["CardNum"] = phone;
                            hshys["Money"] = money;
                            hshys["bz"] = "";
                            hshys["sm"] = "会员卡充值";
                            hshys["InDate"] = DateTime.Now;
                            hshys["AdminHotelid"] = adminHotelId;
                            DataFactory.SqlDataBase().InsertByHashtable("CardMoney", hshys);
                        }
                        catch { }

                        int zsje = 0;//赠送金额
                        double hykye = 0; //会员卡余额
                        string hykdj = dshy.Rows[0]["hylx"].ToString();//会员卡类型
                        string scmcode = dshy.Rows[0]["scmcode"].ToString(); //会员卡折扣                              

                        string sqls = string.Format(@"SELECT  id , moneys , zsmoneys ,  hylxcode  ,zsjf,iszsmoneys,iszsjf,ishylxcode,couponid,iscouponid  FROM dbo.CardRecharge where moneys='{0}' and Adminhotelid='{1}' ", money, adminHotelId);
                        DataTable hyc = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls));
                        if (hyc != null && hyc.Rows.Count > 0)
                        {
                            if (hyc.Rows[0]["zsmoneys"].ToString() != "" && hyc.Rows[0]["iszsmoneys"].ToString() == "1")
                            {
                                zsje = CommonHelper.GetInt(hyc.Rows[0]["zsmoneys"]);
                                if (zsje > 0)
                                {
                                    //会员卡充值赠送 财务记录
                                    CommonClass.InsertFinance(number, name, phone, zsje, 9, "会员卡充值赠送", memberId, transaction_id, adminHotelId, hotelId, "", openid);
                                    try
                                    {//充值记录赠送
                                        Hashtable hshys = new Hashtable();
                                        hshys["Name"] = name;
                                        hshys["CardNum"] = phone;
                                        hshys["Money"] = zsje;
                                        hshys["bz"] = "";
                                        hshys["sm"] = "会员卡充值赠送";
                                        hshys["InDate"] = DateTime.Now;
                                        hshys["AdminHotelid"] = adminHotelId;
                                        DataFactory.SqlDataBase().InsertByHashtable("CardMoney", hshys);
                                    }
                                    catch { }
                                }
                            }
                            if (hyc.Rows[0]["zsjf"].ToString() != "" && hyc.Rows[0]["zsjf"].ToString() != "0" && hyc.Rows[0]["iszsjf"].ToString() == "1")
                            {
                                Hashtable hs = new Hashtable();
                                hs["lsh"] = dshy.Rows[0]["lsh"];
                                hs["kh"] = dshy.Rows[0]["kh"];
                                hs["hylb"] = dshy.Rows[0]["hylx"];
                                hs["bmcode"] = bmcode;
                                hs["zmsm"] = "充值赠送";
                                //hs["xfje"] = money;
                                hs["jf"] = hyc.Rows[0]["zsjf"].ToString();
                                hs["jzflag"] = "F";
                                hs["czrq"] = DateTime.Now;
                                hs["czygh"] = 999999;
                                hs["czbc"] = 1;
                                hs["bz"] = "积分来源:充值" + money + "赠送," + "订单号:" + number;
                                hs["Adminhotelid"] = a[1];
                                DataFactory.SqlDataBase(adminHotelId).InsertByHashtable("hy_hyxfjlb", hs);
                            }
                            if (hyc.Rows[0]["hylxcode"].ToString() != "0" && hyc.Rows[0]["ishylxcode"].ToString() == "1")
                            {
                                string mb = string.Format(@"select hylxcode,jb,mrscm  FROM dbo.hy_hylxbmb where hylxcode='{0}' and AdminHotelid='{1}' ", hyc.Rows[0]["hylxcode"].ToString(), adminHotelId);
                                DataTable dsmb = DataFactory.SqlDataBase(adminHotelId).GetDataTableBySQL(new StringBuilder(mb));
                                if (dsmb != null && dsmb.Rows.Count > 0)
                                {
                                    if (CommonHelper.GetInt(dshy.Rows[0]["jb"]) > CommonHelper.GetInt(dsmb.Rows[0]["jb"]))
                                    {
                                        //微网自定义等级 1是最高级别 如果本身的会员级别大于充值的那么就更新会员级别
                                        hykdj = dsmb.Rows[0]["hylxcode"].ToString();
                                        scmcode = dsmb.Rows[0]["mrscm"].ToString();
                                    }
                                }
                            }

                            //卡券
                            if (hyc.Rows[0]["couponid"].ToString() != "" && hyc.Rows[0]["iscouponid"].ToString() == "1")
                            {
                                CommonMethod.GiveCoupon(adminHotelId, dshy.Rows[0]["lsh"].ToString(), "单次充值赠送,累计充值赠送", Convert.ToInt32(money), hyc.Rows[0]["couponid"].ToString());
                            }
                        }

                        if (dshy.Rows[0]["hykye"] != null && dshy.Rows[0]["hykye"].ToString() != "")
                        {
                            hykye = zsje + money + CommonHelper.GetInt(dshy.Rows[0]["hykye"]);
                        }
                        else
                        {
                            hykye = zsje + money;
                        }

                        Hashtable hshy = new Hashtable();
                        hshy["hykye"] = hykye;
                        hshy["hylx"] = hykdj;
                        hshy["scmcode"] = scmcode;
                        DataFactory.SqlDataBase(adminHotelId).UpdateByHashtable("hy_hyzlxxb", "lsh", dshy.Rows[0]["lsh"].ToString(), hshy);

                        //国光对接
                        Hashtable hs2 = new Hashtable();
                        hs2["vipkh"] = a[4];
                        hs2["bmcode"] = bmcode;
                        hs2["je"] = money;
                        hs2["zsje"] = zsje;
                        hs2["fhcs"] = "T";
                        DataFactory.SqlDataBase(adminHotelId).GetDataTableProc("PROC_WXCZ", hs2);
                        InOrderQuery(adminHotelId, hotelId, number, 3);
                        //会员升级
                        CommonMethod.UpdateUpgrade(adminHotelId, dshy.Rows[0]["Carid"].ToString(), 0, Convert.ToInt32(money));

                        #region 发送消息
                        CommonMethod.Message_CZ(number);//推送模板消息给 预订人
                        CommonMethod.SendMessage_HYCZZDY(number);//推送消息给智订云管理员及员工
                        #endregion

                    }
                }
            }
            catch (Exception e)
            {
                Log.Info("会员卡充值错误信息", e.Message);
            }
        }

        /// <summary>
        /// 会员卡升级
        /// </summary>
        /// <param name="a">"HYK20180531173032199", "1004613", "会员卡升级", "15636876901", "", "99"</param>
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        public void hyksj(string[] a, string transaction_id, double money)
        {
            try
            {
                string number = a[0];
                string adminHotelId = a[1];
                string typelx = a[2];
                int hotelId = CommonHelper.GetInt(a[3]);
                string kh = a[4];
                string bmcode = a[5];

                InOrderQuery(adminHotelId, hotelId, number, 1);

                string sql = string.Format(@"select id,number from Finance where number='{0}'", a[0]);
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds != null && ds.Rows.Count > 0)
                {
                    InOrderQuery(adminHotelId, hotelId, number, 2);
                }
                else
                {
                    #region  ** 升级记录 **
                    //升级记录
                    Hashtable ht_vu = new Hashtable();
                    ht_vu["AdminHotelId"] = adminHotelId;
                    ht_vu["HotelId"] = hotelId;
                    ht_vu["OrderNumber"] = number;
                    ht_vu["CardNumber"] = kh;
                    ht_vu["PastCode"] = "";
                    ht_vu["NewCode"] = bmcode;
                    ht_vu["UpgradeMoney"] = money;
                    ht_vu["transaction_id"] = transaction_id;
                    int vu_val = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("VipUpgrade", ht_vu);
                    #endregion

                    string sqlhy = string.Format(@"select lsh,sjhm,xm,hykye,hylx,scmcode,bmcode,Carid from hy_hyzlxxb inner join hy_hylxbmb on  hy_hyzlxxb.hylx=hy_hylxbmb.hylxcode   where  kh='{0}' and hy_hyzlxxb.AdminHotelid='{1}' ", a[4], adminHotelId);
                    DataTable dshy = DataFactory.SqlDataBase(a[1]).GetDataTableBySQL(new StringBuilder(sqlhy));
                    if (dshy != null && dshy.Rows.Count > 0)
                    {
                        string hylx = dshy.Rows[0]["hylx"].ToString();
                        string name = dshy.Rows[0]["xm"].ToString();
                        string phone = dshy.Rows[0]["sjhm"].ToString();
                        string memberId = dshy.Rows[0]["lsh"].ToString();
                        string openid = dshy.Rows[0]["Carid"].ToString();
                        if (hotelId < 1)
                        {
                            StringBuilder hotelsql = new StringBuilder();
                            hotelsql.AppendFormat("SELECT ID,name FROM Hotel WHERE AdminHotelid='{0}' and bmcode='{1}' ", adminHotelId, dshy.Rows[0]["bmcode"].ToString());
                            DataTable hoteldt = DataFactory.SqlDataBase().GetDataTableBySQL(hotelsql);
                            if (hoteldt != null && hoteldt.Rows.Count > 0)
                            {
                                hotelId = CommonHelper.GetInt(hoteldt.Rows[0]["ID"].ToString());
                            }
                        }
                        CommonClass.InsertFinance(number, name, phone, money, 5, "会员卡升级", memberId, transaction_id, adminHotelId, hotelId, "", openid);

                        string sql1 = string.Format(@"update hy_hyzlxxb set hylx='{0}' where lsh='{1}' ", a[5], dshy.Rows[0]["lsh"]);
                        int x = DataFactory.SqlDataBase(adminHotelId).ExecuteBySql(new StringBuilder(sql1));
                        if (x > 0)
                        {
                            ht_vu = new Hashtable();
                            ht_vu["PastCode"] = hylx;
                            ht_vu["Openid"] = openid;
                            ht_vu["Success"] = 1;
                            DataFactory.SqlDataBase().UpdateByHashtable("VipUpgrade", "Id", vu_val.ToString(), ht_vu);
                            InOrderQuery(adminHotelId, hotelId, number, 3);
                            CommonMethod.GiveCoupon(adminHotelId, dshy.Rows[0]["lsh"].ToString(), "升级会员赠送");
                        }
                        else
                        {
                            Log.Info("会员卡升级失败", sql1);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Log.Info("会员卡升级错误信息", e.Message);
            }
        }

        /// <summary>
        /// 微信订单支付
        /// </summary>
        /// <param name="a"></param>
        /// 
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        public void wxzf(string[] a, string transaction_id, double money)
        {
            try
            {
                string OrderNum = a[0];
                string sql = string.Format(@"select id,number from Finance where number='{0}'", OrderNum);
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds != null && ds.Rows.Count > 0)
                {
                    return;
                }
                else
                {

                    SqlParam[] spOrderNum = new SqlParam[] { 
                        new SqlParam("@OrderNum",OrderNum)
                    };
                    #region 订单信息
                    StringBuilder sb_OrderInfo = new StringBuilder();
                    sb_OrderInfo.Append(@"
                    SELECT  PayType ,
                            TomePrice ,
                            MemberId ,
                            Mobile ,
                            AdminHotelid ,
                            hotelid ,
                            RoomId ,
                            BeginTime ,
                            EndTime ,
                            CAST(name AS VARCHAR(30)) name ,
                            CAST(Environment AS VARCHAR(30)) Environment ,
                            CAST(Quality AS CHAR(4)) Quality ,
                            OrderNum ,
                            pay ,
                            Adult ,
                            RoomType ,
                            Number ,
                            Days ,
                            StaffId ,
                            openid ,
                            Coupon ,
                            RuleId ,
                            Zip
                    FROM    Reservation
                    WHERE   OrderNum = @OrderNum
                    ");
                    DataTable dt_OrderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(sb_OrderInfo, spOrderNum);
                    #endregion
                    #region 微信支付订单处理
                    if (dt_OrderInfo == null || dt_OrderInfo.Rows.Count <= 0)
                    {
                        string sqls1wx = string.Format(@"select PayType,TomePrice,name,MemberId,Mobile,hotelid  from WeChat_Reservation where OrderNum='{0}'", OrderNum);
                        DataTable dt_temp = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls1wx));
                        if (dt_temp != null && dt_temp.Rows.Count > 0)
                        {
                            StringBuilder sqlwx = new StringBuilder();
                            sqlwx.AppendFormat(@"
                            INSERT INTO Reservation
                            (OrderNum,RoomType,Number,Days,BeginTime,EndTime,Adult,RoomTotal,ServiceTotal,
                            TomePrice,Name,Sex,Mobile,Zip,Arrived,Other,Remark,PayType,State,
                            MemberId,Comm,Environment,Source,Pay,KeepTime,Quality,HotelId,CouponID,Coupon,Distributor,
                            AdminHotelid,isdelete,rpid,XX_SKYDDB,openid,PayOpenid,RoomId,RuleId,StaffId,StaffRemarks,VipCode,VipName,FirstLive,ContinueState,ContinueOrderNum
                            ,DistributorId,Source_MemberId)
                             SELECT
                             OrderNum,RoomType,Number,Days,BeginTime,EndTime,Adult,RoomTotal,ServiceTotal,
                             TomePrice,Name,Sex,Mobile,Zip,Arrived,Other,Remark,PayType,State,
                             MemberId,Comm,Environment,Source,Pay,KeepTime,Quality,HotelId,CouponID,Coupon,Distributor,
                             AdminHotelid,isdelete,rpid,XX_SKYDDB,openid,PayOpenid,RoomId,RuleId,StaffId,StaffRemarks,VipCode,VipName,FirstLive,ContinueState,ContinueOrderNum
                             ,DistributorId,Source_MemberId
                             FROM WeChat_Reservation
                             WHERE OrderNum='{0}' ", a[0]);
                            int i = DataFactory.SqlDataBase().ExecuteBySql(sqlwx);
                            if (i > 0)
                            {
                                dt_OrderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(sb_OrderInfo, spOrderNum);
                            }
                        }
                    }
                    #endregion
                    if (dt_OrderInfo != null && dt_OrderInfo.Rows.Count > 0)
                    {
                        string adminHotelid = dt_OrderInfo.Rows[0]["AdminHotelid"].ToString();
                        string hotelId = dt_OrderInfo.Rows[0]["hotelid"].ToString();
                        if (money != CommonHelper.GetDouble(dt_OrderInfo.Rows[0]["TomePrice"]))//判断支付金额是否一致
                        {
                            Log.Info("支付金额错误：", "应付" + dt_OrderInfo.Rows[0]["TomePrice"] + ",实付" + money + "。");
                            StringBuilder sqldel = new StringBuilder();
                            sqldel.AppendFormat("UPDATE Reservation SET Remark = '{0}' WHERE OrderNum = '{1}'", "支付金额错误：" + "应付" + dt_OrderInfo.Rows[0]["TomePrice"] + ",实付" + money + "。", OrderNum);
                            DataFactory.SqlDataBase().ExecuteBySql(sqldel);
                            return;
                        }
                        //处理优惠券为已使用
                        if (dt_OrderInfo.Rows[0]["Coupon"] != null && dt_OrderInfo.Rows[0]["Coupon"].ToString() != "")
                        {
                            UpdateCoupon(dt_OrderInfo.Rows[0]["Coupon"].ToString());
                        }
                        string name = dt_OrderInfo.Rows[0]["name"].ToString();
                        string phone = dt_OrderInfo.Rows[0]["Mobile"].ToString();
                        string memberId = dt_OrderInfo.Rows[0]["MemberId"].ToString();
                        string openid = dt_OrderInfo.Rows[0]["openid"].ToString();
                        //财务记录
                        int iss = CommonClass.InsertFinance(OrderNum, name, phone, money, 1, "酒店客房预订支付", memberId, transaction_id, adminHotelid, CommonHelper.GetInt(hotelId), "", openid);

                        if (iss > 0)
                        {
                            #region 同步至国光系统
                            if (DataFactory.CheckSqlIsOpen(adminHotelid, CommonHelper.GetInt(hotelId)))//订单数据库是否能连接
                            {
                                string mem = string.Format(@"select lsh,kh,hylx,Carid from hy_hyzlxxb where lsh='{0}' and AdminHotelid='{1}'", memberId, adminHotelid);
                                DataTable dsm = DataFactory.SqlDataBase(adminHotelid).GetDataTableBySQL(new StringBuilder(mem));//会员信息
                                string hykh = "";//会员卡号
                                if (dsm != null && dsm.Rows.Count > 0)
                                {
                                    hykh = dsm.Rows[0]["kh"].ToString();
                                }
                                string kfcode = dt_OrderInfo.Rows[0]["Quality"].ToString();
                                if (kfcode.Length > 4)
                                {
                                    kfcode = kfcode.Substring(0, 4);
                                }

                                Hashtable ht = new Hashtable();
                                ht["ddrq"] = dt_OrderInfo.Rows[0]["BeginTime"];
                                ht["ldrq"] = dt_OrderInfo.Rows[0]["EndTime"];
                                ht["krname"] = dt_OrderInfo.Rows[0]["Name"];
                                ht["ydly"] = 2;
                                ht["tbyq"] = dt_OrderInfo.Rows[0]["Environment"];
                                ht["kfcode"] = kfcode;
                                ht["ordernum"] = dt_OrderInfo.Rows[0]["OrderNum"];
                                ht["dh"] = dt_OrderInfo.Rows[0]["Mobile"];
                                ht["pay"] = 1;
                                ht["paytype"] = 4;
                                ht["ydsl"] = CommonHelper.GetInt(dt_OrderInfo.Rows[0]["Number"]);
                                ht["zfj"] = CommonHelper.GetDouble(dt_OrderInfo.Rows[0]["TomePrice"]);
                                ht["wxid"] = dt_OrderInfo.Rows[0]["openid"];
                                ht["dfbz"] = dt_OrderInfo.Rows[0]["OrderNum"] + "," + GetRule(dt_OrderInfo.Rows[0]["RuleId"].ToString()) + "," + dt_OrderInfo.Rows[0]["Environment"];
                                if (dt_OrderInfo.Rows[0]["Coupon"] != null && dt_OrderInfo.Rows[0]["Coupon"].ToString() != "")
                                {
                                    ht["dfbz"] += ("," + "卡券" + dt_OrderInfo.Rows[0]["Zip"] + "元");
                                }
                                ht["vipkh"] = hykh;
                                ht["OUT_yddh"] = null;

                                Hashtable rs = new Hashtable();
                                DataFactory.SqlDataBase(adminHotelid, hotelId).ExecuteByProcReturn("proc_wxydcl", ht, ref rs);
                                string yddh = rs["OUT_yddh"].ToString();
                                if (yddh != "" && yddh != "0")
                                {
                                    Hashtable info = new Hashtable();
                                    info["XX_SKYDDB"] = yddh;//国光订单编号
                                    DataFactory.SqlDataBase().UpdateByHashtable("Reservation", "ordernum", OrderNum, info);//更新订单
                                    CommonClass.SetDayPrice(adminHotelid, hotelId, OrderNum, "4", yddh, dt_OrderInfo.Rows[0]["zip"].ToString());//生成每日房价 - 国光
                                }
                            }
                            #endregion

                            #region 发送消息
                            TemplateMessage.Sen(OrderNum, "4");//推送模板消息给 预订人
                            CommonMethod.SendMessage_ZDY(OrderNum);//推送消息给智订云管理员及员工
                            TemplateMessage.NoticePush_RoomState(hotelId, dt_OrderInfo.Rows[0]["RoomId"].ToString());

                            if (DataFactory.CheckSqlIsOpen(adminHotelid, 0))//会员数据库是否能连接
                            {
                                //会员升级
                                CommonMethod.UpdateUpgrade(a[1], openid, money, 0);
                                #region 获得优惠券奖励
                                CommonMethod.GiveCoupon(adminHotelid, memberId, "单次消费赠送,累计消费赠送", CommonHelper.GetInt(dt_OrderInfo.Rows[0]["TomePrice"]), "", OrderNum);
                                #endregion
                                //消费红包
                                AddReward(openid, adminHotelid);
                            }

                            //代理通知
                            //string url = "http://119.23.135.8:6204/api/AgentPush/Agent"; //URL
                            //JsonData jsonData = new JsonData();
                            //jsonData["OrderNum"] = OrderNum;
                            //string json = jsonData.ToJson();
                            //byte[] postData = Encoding.UTF8.GetBytes(json);
                            //WebClient client = new WebClient();
                            //client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                            //client.Headers.Add("ContentLength", postData.Length.ToString());
                            //byte[] data_ret_bytes = client.UploadData(url, "POST", postData);

                            //string test = Encoding.UTF8.GetString(data_ret_bytes);
                            //JsonData jdRes = JsonMapper.ToObject(test);
                            //Log.Info("代理推送通知:", test);

                            #endregion

                        }
                    }
                    else
                    {
                        Log.Info("微信支付错误信息(wxzf)", "未找到订单!");
                    }
                }

            }
            catch (Exception e)
            {
                Log.Info("微信支付错误信息(wxzf)", e.Message);
            }
        }

        /// <summary>
        /// 微信订单支付(续住)
        /// </summary>
        /// <param name="a"></param>
        /// 
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        public void wxzfxz(string[] a, string transaction_id, double money)
        {
            try
            {
                string OrderNum = a[0];
                string sql = string.Format(@"select id,number from Finance where number='{0}'", OrderNum);
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds != null && ds.Rows.Count > 0)
                {
                    return;
                }
                else
                {
                    string sqls12 = string.Format(@"
                    select 
                    PayType,TomePrice,MemberId,Mobile,AdminHotelid,hotelid,BeginTime,EndTime,name,Environment,Quality,OrderNum,FristOrderNum,pay,Adult,RoomType,Number,Days,StaffId,openid,Coupon,RoomId,RuleId,Zip,RoomNum,RoomAccount    
                    from Reservation
                    where OrderNum='{0}'", OrderNum);
                    DataTable dt_OrderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls12));
                    string FristOrderNum = "";
                    #region 微信支付订单处理
                    if (dt_OrderInfo == null || dt_OrderInfo.Rows.Count <= 0)
                    {
                        string sqls1wx = string.Format(@"select PayType,TomePrice,name,MemberId,Mobile,hotelid,FristOrderNum  from WeChat_Reservation where OrderNum='{0}'", OrderNum);
                        DataTable dt_temp = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls1wx));
                        if (dt_temp != null && dt_temp.Rows.Count > 0)
                        {
                            if (money != CommonHelper.GetDouble(dt_temp.Rows[0]["TomePrice"]))//判断支付金额是否一致
                            {
                                Log.Info("微信订单支付(续住)金额错误：", "应付" + dt_temp.Rows[0]["TomePrice"] + ",实付" + money + "。");
                                return;
                            }
                            StringBuilder sqlwx = new StringBuilder();
                            sqlwx.AppendFormat(@"
                            INSERT INTO Reservation
                            (OrderNum,RoomType,Number,Days,BeginTime,EndTime,Adult,RoomTotal,ServiceTotal,
                            TomePrice,Name,Sex,Mobile,Zip,Arrived,Other,Remark,PayType,State,AddTime,
                            MemberId,Comm,Environment,Source,Pay,KeepTime,Quality,HotelId,Coupon,Distributor,
                            AdminHotelid,isdelete,rpid,XX_SKYDDB,openid,PayOpenid,RoomId,RuleId,StaffId,StaffRemarks,VipCode,VipName,FristOrderNum,FirstLive,ContinueState,RoomNum,RoomAccount
,DistributorId,Source_MemberId)
                             SELECT
                             OrderNum,RoomType,Number,Days,BeginTime,EndTime,Adult,RoomTotal,ServiceTotal,
                             TomePrice,Name,Sex,Mobile,Zip,Arrived,Other,Remark,PayType,State,AddTime,
                             MemberId,Comm,Environment,Source,Pay,KeepTime,Quality,HotelId,Coupon,Distributor,
                             AdminHotelid,isdelete,rpid,XX_SKYDDB,openid,PayOpenid,RoomId,RuleId,StaffId,StaffRemarks,VipCode,VipName,FristOrderNum,FirstLive,ContinueState,RoomNum,RoomAccount
,DistributorId,Source_MemberId
                             FROM WeChat_Reservation
                             WHERE OrderNum='{0}' ", a[0]);
                            int i = DataFactory.SqlDataBase().ExecuteBySql(sqlwx);

                            if (i > 0)
                            {
                                dt_OrderInfo = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls12));
                            }
                        }
                    }
                    #endregion
                    if (dt_OrderInfo != null && dt_OrderInfo.Rows.Count > 0)
                    {
                        FristOrderNum = dt_OrderInfo.Rows[0]["FristOrderNum"].ToString();
                        //更新首单状态值
                        Hashtable hs_Second = new Hashtable();
                        hs_Second["FristOrderNum"] = FristOrderNum;// 续单的首单订单编号
                        hs_Second["ContinueState"] = 1;// 续单状态
                        DataFactory.SqlDataBase().UpdateByHashtable("Reservation", "OrderNum", FristOrderNum, hs_Second);

                        string adminHotelid = dt_OrderInfo.Rows[0]["AdminHotelid"].ToString();
                        string hotelId = dt_OrderInfo.Rows[0]["hotelid"].ToString();
                        if (money != CommonHelper.GetDouble(dt_OrderInfo.Rows[0]["TomePrice"]))//判断支付金额是否一致
                        {
                            Log.Info("微信订单支付(续住)金额错误：", "应付" + dt_OrderInfo.Rows[0]["TomePrice"] + ",实付" + money + "。");
                            StringBuilder sqldel = new StringBuilder();
                            sqldel.AppendFormat("UPDATE Reservation SET Remark = '{0}' WHERE OrderNum = '{1}'", "支付金额错误：" + "应付" + dt_OrderInfo.Rows[0]["TomePrice"] + ",实付" + money + "。", a[0]);
                            DataFactory.SqlDataBase().ExecuteBySql(sqldel);
                            return;
                        }
                        //处理优惠券为已使用
                        if (dt_OrderInfo.Rows[0]["Coupon"] != null && dt_OrderInfo.Rows[0]["Coupon"].ToString() != "")
                        {
                            UpdateCoupon(dt_OrderInfo.Rows[0]["Coupon"].ToString());
                        }

                        string name = dt_OrderInfo.Rows[0]["name"].ToString();
                        string phone = dt_OrderInfo.Rows[0]["Mobile"].ToString();
                        string memberId = dt_OrderInfo.Rows[0]["MemberId"].ToString();
                        string openid = dt_OrderInfo.Rows[0]["openid"].ToString();
                        //财务记录
                        int iss = CommonClass.InsertFinance(OrderNum, name, phone, money, 1, "酒店客房预订支付", memberId, transaction_id, adminHotelid, CommonHelper.GetInt(hotelId), "", openid);

                        if (iss > 0)
                        {
                            #region 同步至国光系统

                            string mem = string.Format(@"select lsh,kh,hylx,Carid from hy_hyzlxxb where lsh='{0}' and AdminHotelid='{1}'", dt_OrderInfo.Rows[0]["MemberId"], adminHotelid);
                            DataTable dsm = DataFactory.SqlDataBase(adminHotelid).GetDataTableBySQL(new StringBuilder(mem));//会员信息
                            string hykh = "";//会员卡号
                            if (dsm != null && dsm.Rows.Count > 0)
                            {
                                hykh = dsm.Rows[0]["kh"].ToString();
                            }

                            //续住提交订单国光续住存储过程
                            Hashtable ht = new Hashtable();
                            ht["fh"] = dt_OrderInfo.Rows[0]["RoomNum"].ToString();//房号
                            ht["fjzh"] = dt_OrderInfo.Rows[0]["RoomAccount"].ToString(); //房间账号
                            ht["ordernum"] = OrderNum;//订单编号
                            ht["zdts"] = CommonHelper.GetInt(dt_OrderInfo.Rows[0]["Days"].ToString());//住店天数
                            ht["zfj"] = CommonHelper.GetInt(dt_OrderInfo.Rows[0]["TomePrice"].ToString());//总房价
                            ht["pay"] = 1;
                            ht["paytype"] = 4;
                            ht["OUT_clzt"] = null;
                            Hashtable rs = new Hashtable();
                            DataFactory.SqlDataBase(adminHotelid, hotelId).ExecuteByProcReturn("proc_wxxzcl", ht, ref rs);
                            if (rs["OUT_clzt"] != null && rs["OUT_clzt"].ToString() == "T")//同步至国光订单成功
                            {
                                StringBuilder sb_ddid = new StringBuilder();
                                sb_ddid.AppendFormat("select XX_SKYDDB from Reservation where OrderNum='{0}'", FristOrderNum);
                                DataTable dt_ddid = DataFactory.SqlDataBase().GetDataTableBySQL(sb_ddid);
                                string XX_SKYDDB = "";
                                if (dt_ddid != null && dt_ddid.Rows.Count > 0)
                                {
                                    if (dt_ddid.Rows[0]["XX_SKYDDB"] != null && dt_ddid.Rows[0]["XX_SKYDDB"].ToString() != "")
                                    {
                                        XX_SKYDDB = dt_ddid.Rows[0]["XX_SKYDDB"].ToString();
                                    }
                                }
                                Hashtable info = new Hashtable();
                                info["State"] = 3;//入住状态
                                info["XX_SKYDDB"] = XX_SKYDDB;//国光订单id
                                DataFactory.SqlDataBase().UpdateByHashtable("Reservation", "ordernum", OrderNum, info);//更新订单

                                CommonClass.XzDayPrice(adminHotelid, hotelId, OrderNum, "4", ht["fjzh"].ToString(), dt_OrderInfo.Rows[0]["zip"].ToString());//续住生成每日房价 - 国光    

                                //插入国光会员卡续住视图在住房型信息
                                string sql_Live = string.Format(@"select * from ContinueLiveInfo where FristOrderNum='{0}'", dt_OrderInfo.Rows[0]["FristOrderNum"].ToString());
                                DataTable dt_Live = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql_Live));
                                if (dt_Live != null && dt_Live.Rows.Count > 0)
                                {
                                    Hashtable hs_Live = new Hashtable();
                                    hs_Live["LeaveTime"] = dt_OrderInfo.Rows[0]["EndTime"].ToString();// 结束日期
                                    hs_Live["LiveState"] = 1;// 续住标识
                                    hs_Live["RoomNum"] = dt_OrderInfo.Rows[0]["RoomNum"].ToString().Trim();
                                    hs_Live["RoomAccount"] = dt_OrderInfo.Rows[0]["RoomAccount"].ToString().Trim();
                                    DataFactory.SqlDataBase().UpdateByHashtable("ContinueLiveInfo", "ID", dt_Live.Rows[0]["ID"].ToString(), hs_Live);
                                }
                            }

                            #endregion

                            Log.Info("微信订单支付(续住)", OrderNum + "订单号");

                            #region 发送消息
                            TemplateMessage.Sen(OrderNum, "4");//推送模板消息给 预订人
                            CommonMethod.SendMessage_ZDY(OrderNum);//推送消息给智订云管理员及员工
                            #endregion

                            if (dsm != null && dsm.Rows.Count > 0)
                            {
                                #region 获得积分奖励

                                string sql_jf = string.Format(@"select isEnble1,jfzhi1 from jfmatter where AdminHotelid='{0}' and isEnble1=1 and isjf=1 ", adminHotelid);
                                DataTable dt_jf = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql_jf));
                                if (dt_jf != null && dt_jf.Rows.Count > 0)
                                {
                                    Hashtable hs = new Hashtable();
                                    hs["lsh"] = dsm.Rows[0]["lsh"];
                                    hs["kh"] = dsm.Rows[0]["kh"];
                                    hs["hylb"] = dsm.Rows[0]["hylx"];
                                    hs["bmcode"] = "99";
                                    hs["zmsm"] = "消费获得积分";
                                    hs["xfje"] = money;
                                    hs["jf"] = money * CommonHelper.GetInt(dt_jf.Rows[0]["jfzhi1"]);
                                    hs["jzflag"] = "F";
                                    hs["czrq"] = DateTime.Now;
                                    hs["bz"] = "订单号:" + a[0] + ",积分来源:" + dt_OrderInfo.Rows[0]["RoomType"].ToString() + ",备注:";
                                    hs["Adminhotelid"] = adminHotelid;
                                    DataFactory.SqlDataBase(adminHotelid).InsertByHashtable("hy_hyxfjlb", hs);
                                }

                                #endregion
                                #region 获得优惠券奖励
                                CommonMethod.GiveCoupon(adminHotelid, dsm.Rows[0]["lsh"].ToString(), "单次消费赠送,累计消费赠送", CommonHelper.GetInt(dt_OrderInfo.Rows[0]["TomePrice"]));
                                #endregion
                                //会员升级
                                CommonMethod.UpdateUpgrade(a[1], dsm.Rows[0]["CarId"].ToString(), money, 0);
                            }
                        }
                    }
                    else
                    {
                        Log.Info("微信订单支付(续住)错误信息", "未找到订单!");
                    }
                }


            }
            catch (Exception e)
            {
                Log.Info("微信订单支付(续住)错误信息", e.Message);
            }
        }

        /// <summary>
        /// 微信服务订单支付
        /// </summary>
        /// <param name="a"></param>
        /// 
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        public void wxfwzf(string[] a, string transaction_id, double money)
        {
            try
            {
                string OrderNum = a[0];
                string sqls12 = string.Format(@"
                    select 
                    PayType,PayPrice,Name,MemberId,Mobile,AdminHotelid,Hotelid,OpenId
                    from ServiceOrder
                    where OrderNum='{0}'", a[0]);
                DataTable dss1 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls12));

                if (dss1 != null && dss1.Rows.Count > 0)
                {
                    string adminHotelid = dss1.Rows[0]["AdminHotelid"].ToString();
                    string hotelId = dss1.Rows[0]["Hotelid"].ToString();
                    if (money != CommonHelper.GetDouble(dss1.Rows[0]["PayPrice"]))//判断支付金额是否一致
                    {
                        Log.Info("微信服务订单支付金额错误：", "应付" + dss1.Rows[0]["PayPrice"] + ",实付" + money + "。");
                        return;
                    }
                    string name = dss1.Rows[0]["Name"].ToString();
                    string phone = dss1.Rows[0]["Mobile"].ToString();
                    string memberId = dss1.Rows[0]["MemberId"].ToString();
                    string openid = dss1.Rows[0]["OpenId"].ToString();
                    //财务记录
                    int iss = CommonClass.InsertFinance(OrderNum, name, phone, money, 1, "门店服务预订支付", memberId, transaction_id, adminHotelid, CommonHelper.GetInt(hotelId), "", openid);

                    if (iss > 0)
                    {
                        if (dss1.Rows[0]["PayType"].ToString() == "4")//微信支付
                        {
                            Hashtable info = new Hashtable();
                            info["Pay"] = 1;//已支付
                            info["State"] = 1;//待使用
                            info["PayTime"] = DateTime.Now;
                            info["UnUseTime"] = DateTime.Now;
                            DataFactory.SqlDataBase().UpdateByHashtable("ServiceOrder", "ordernum", a[0], info);//更新订单

                            //更新订单
                            Hashtable hgo = new Hashtable();
                            hgo["fkbj"] = "T";//已支付
                            hgo["ydzt"] = "1";/*预订状态 1 预订 2 已确认 3 已完成  4 已取消*/
                            hgo["xfrq"] = DateTime.Now;
                            DataFactory.SqlDataBase(adminHotelid).UpdateByHashtable("XX_MDSPYDB", "yddh", a[0], hgo);//更新订单

                        }
                        Log.Info("微信服务订单支付", a[0].ToString() + "订单号");
                        #region 发送消息
                        //TemplateMessage.Sen(a[0].ToString(), "4");//推送模板消息给 预订人
                        TemplateMessage.Sens(a[0].ToString(), "1");
                        // CommonMethod.SendMessage_MDZDY(a[0].ToString());//推送消息给智订云管理员及员工
                        #endregion
                    }
                }
                else
                {
                    Log.Info("微信服务订单支付错误信息：", "未找到订单!");
                }



            }
            catch (Exception e)
            {
                Log.Info("微信服务订单支付错误信息：", e.Message);
            }
        }


        /// <summary>
        /// 微信商品订单支付
        /// </summary>
        /// <param name="a"></param>
        /// 
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        public void wxspzf(string[] a, string transaction_id, double money)
        {
            try
            {
                string OrderNum = a[0];
                string sql = string.Format(@"select id,number from Finance where number='{0}'", OrderNum);
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds != null && ds.Rows.Count > 0)
                {
                    return;
                }
                else
                {
                    string sqls12 = string.Format(@"
                    select 
                    PayType,PayPrice,Name,MemberId,Mobile,AdminHotelid,Hotelid,OpenId,CouponID
                    from ProductOrder
                    where OrderNum='{0}'", a[0]);
                    DataTable dss1 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls12));

                    #region 微信支付订单处理
                    if (dss1 == null || dss1.Rows.Count <= 0)
                    {
                        string sqls1wx = string.Format(@"select PayType,PayPrice,Name,MemberId,Mobile,Hotelid  from WeChat_ProductOrder where OrderNum='{0}'", OrderNum);
                        DataTable dsswx = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls1wx));
                        if (dsswx != null && dsswx.Rows.Count > 0)
                        {
                            if (money != CommonHelper.GetDouble(dsswx.Rows[0]["PayPrice"]))//判断支付金额是否一致
                            {
                                Log.Info("支付金额错误：", "应付" + dsswx.Rows[0]["PayPrice"] + ",实付" + money + "。");
                                return;
                            }

                            StringBuilder sqlwx = new StringBuilder();
                            sqlwx.AppendFormat(@"
                            INSERT INTO ProductOrder
                            (OrderNum,Number,ProductId,ProductName,ProkindId,ProkindName,Name,Mobile,Sex
                             ,MemberId,MemberLevel,zip,CouponID,Special,TotalPrice,PayPrice,OpenId,AdminHotelid,Hotelid
                             ,PayType,Pay,State,Comm,CancelReason,CancelContent,CancelTime,StaffId,StaffRemarks,ServicingMoney,EstimateMoney,IsScan,UserOrderNum,Address,SureTime,SendTime,DeliveryTime,SubmissionTime,PayTime,Remark 
                             ,VipCode,VipName,DeleteMark,Sort,AddTime)
                             SELECT
                             OrderNum,Number,ProductId,ProductName,ProkindId,ProkindName,Name,Mobile,Sex
                             ,MemberId,MemberLevel,zip,CouponID,Special,TotalPrice,PayPrice,OpenId,AdminHotelid,Hotelid
                             ,PayType,Pay,State,Comm,CancelReason,CancelContent,CancelTime,StaffId,StaffRemarks,ServicingMoney,EstimateMoney,IsScan,UserOrderNum,Address,SureTime,SendTime,DeliveryTime,SubmissionTime,PayTime,Remark
                             ,VipCode,VipName,DeleteMark,Sort,AddTime
                             FROM WeChat_ProductOrder
                             WHERE OrderNum='{0}' ", a[0]);
                            int i = DataFactory.SqlDataBase().ExecuteBySql(sqlwx);
                            if (i > 0)
                            {
                                dss1 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls12));
                                StringBuilder sqldel = new StringBuilder();
                                sqldel.AppendFormat("DELETE WeChat_ProductOrder WHERE OrderNum='{0}'", OrderNum);
                                DataFactory.SqlDataBase().ExecuteBySql(sqldel);
                            }
                        }
                    }
                    #endregion

                    if (dss1 != null && dss1.Rows.Count > 0)
                    {
                        string adminHotelid = dss1.Rows[0]["AdminHotelid"].ToString();
                        string hotelId = dss1.Rows[0]["Hotelid"].ToString();
                        if (money != CommonHelper.GetDouble(dss1.Rows[0]["PayPrice"]))//判断支付金额是否一致
                        {
                            Log.Info("微信商品订单支付金额错误：", "应付" + dss1.Rows[0]["PayPrice"] + ",实付" + money + "。");
                            return;
                        }

                        //处理优惠券为已使用
                        if (dss1.Rows[0]["CouponID"] != null && dss1.Rows[0]["CouponID"].ToString() != "")
                        {
                            Hashtable hs = new Hashtable();
                            hs["isReceive"] = true;
                            DataFactory.SqlDataBase().UpdateByHashtable("clientcoupon", "id", dss1.Rows[0]["CouponID"].ToString(), hs);
                        }
                        string name = dss1.Rows[0]["Name"].ToString();
                        string phone = dss1.Rows[0]["Mobile"].ToString();
                        string memberId = dss1.Rows[0]["MemberId"].ToString();
                        string openid = dss1.Rows[0]["OpenId"].ToString();
                        //财务记录
                        int iss = CommonClass.InsertFinance(OrderNum, name, phone, money, 30, "商城商品预订支付", memberId, transaction_id, adminHotelid, CommonHelper.GetInt(hotelId), "", openid);

                        if (iss > 0)
                        {
                            if (dss1.Rows[0]["PayType"].ToString() == "30")//微信支付
                            {
                                Hashtable info = new Hashtable();
                                info["Pay"] = 1;//已支付
                                info["State"] = 1;// 0待支付 1待处理（待确认）、2已确认、3已完成、4已取消
                                info["PayTime"] = DateTime.Now;
                                DataFactory.SqlDataBase().UpdateByHashtable("ProductOrder", "ordernum", a[0], info);//更新订单
                            }
                            Log.Info("微信商品订单支付", a[0].ToString() + "订单号");


                            //更新商品数量
                            CommonMethod.UPDATE_Product_Num(a[0].ToString());
                            //更新出库数量
                            CommonMethod.UPDATE_ProductOutBill_Num(a[0].ToString());
                            //发送消息
                            TemplateMessage.ProductSen(a[0].ToString(), "1");
                        }
                    }
                    else
                    {
                        Log.Info("微信商品订单支付错误信息：", "未找到订单!");
                    }

                }

            }
            catch (Exception e)
            {
                Log.Info("微信商品订单支付错误信息：", e.Message);
            }
        }


        /// <summary>
        /// 微信点餐订单支付
        /// </summary>
        /// <param name="a"></param>
        /// 
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        public void wxdczf(string[] a, string transaction_id, double money)
        {
            try
            {
                string OrderNum = a[0];
                string sql = string.Format(@"select id,number from Finance where number='{0}'", OrderNum);
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds != null && ds.Rows.Count > 0)
                {
                    return;
                }
                else
                {
                    string sqls12 = string.Format(@"
                    select 
                    ID,OrderNum,PayType,PayPrice,Name,MemberId,Mobile,AdminHotelid,Hotelid,OpenId,CouponID,zip,RestaurantId,RestaurantName,TableCode
                    from FoodOrder
                    where OrderNum='{0}'", a[0]);
                    DataTable dss1 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls12));

                    #region 微信支付订单处理
                    if (dss1 == null || dss1.Rows.Count <= 0)
                    {
                        string sqls1wx = string.Format(@"select PayType,PayPrice,Name,MemberId,Mobile,Hotelid  from WeChat_FoodOrder where OrderNum='{0}'", OrderNum);
                        DataTable dsswx = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls1wx));
                        if (dsswx != null && dsswx.Rows.Count > 0)
                        {
                            if (money != CommonHelper.GetDouble(dsswx.Rows[0]["PayPrice"]))//判断支付金额是否一致
                            {
                                Log.Info("支付金额错误：", "应付" + dsswx.Rows[0]["PayPrice"] + ",实付" + money + "。");
                                return;
                            }

                            StringBuilder sqlwx = new StringBuilder();
                            sqlwx.AppendFormat(@"
                            INSERT INTO FoodOrder
                            (OrderNum,TableCode,MealNumber,Number,TypeId,TypeName,spcode,FoodId,FoodName,Name,Mobile,Sex,MemberId,MemberLevel
                             ,zip,CouponID,Special,TotalPrice,PayPrice,OpenId,AdminHotelid,Hotelid,RestaurantId
                             ,RestaurantName,BusinId,BusinName,PayType,Pay,State,Address,SendTime,Tableware,SubmissionTime,PayTime
                             ,SureTime,UnUseTime,UseTime,ReLabel,Remark,Send,Comm ,CancelReason,CancelContent
                             ,CancelTime,StaffId,StaffRemarks,ServicingMoney,EstimateMoney,IsScan,UserOrderNum
                             ,VipCode,VipName,DeleteMark,Sort,AddTime)
                             SELECT
                             OrderNum,TableCode,MealNumber,Number,TypeId,TypeName,spcode,FoodId,FoodName,Name,Mobile,Sex,MemberId,MemberLevel
                             ,zip,CouponID,Special,TotalPrice,PayPrice,OpenId,AdminHotelid,Hotelid,RestaurantId
                             ,RestaurantName,BusinId,BusinName,PayType,Pay,State,Address,SendTime,Tableware,SubmissionTime,PayTime
                             ,SureTime,UnUseTime,UseTime,ReLabel,Remark,Send,Comm ,CancelReason,CancelContent
                             ,CancelTime,StaffId,StaffRemarks,ServicingMoney,EstimateMoney,IsScan,UserOrderNum
                             ,VipCode,VipName,DeleteMark,Sort,AddTime
                             FROM WeChat_FoodOrder
                             WHERE OrderNum='{0}' ", a[0]);
                            int i = DataFactory.SqlDataBase().ExecuteBySql(sqlwx);
                            if (i > 0)
                            {
                                dss1 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls12));
                                StringBuilder sqldel = new StringBuilder();
                                sqldel.AppendFormat("DELETE WeChat_FoodOrder WHERE OrderNum='{0}'", OrderNum);
                                DataFactory.SqlDataBase().ExecuteBySql(sqldel);
                            }
                        }
                    }
                    #endregion

                    if (dss1 != null && dss1.Rows.Count > 0)
                    {
                        string adminHotelid = dss1.Rows[0]["AdminHotelid"].ToString();
                        string hotelId = dss1.Rows[0]["Hotelid"].ToString();
                        if (money != CommonHelper.GetDouble(dss1.Rows[0]["PayPrice"]))//判断支付金额是否一致
                        {
                            Log.Info("微信点餐订单支付金额错误：", "应付" + dss1.Rows[0]["PayPrice"] + ",实付" + money + "。");
                            return;
                        }

                        //处理优惠券为已使用
                        int couponmoney = 0;
                        if (dss1.Rows[0]["CouponID"] != null && dss1.Rows[0]["CouponID"].ToString() != "")
                        {
                            if (dss1.Rows[0]["zip"] != null && dss1.Rows[0]["zip"].ToString() != "")
                            {
                                couponmoney = CommonHelper.GetInt(dss1.Rows[0]["zip"]);
                            }
                            Hashtable hs = new Hashtable();
                            hs["isReceive"] = true;
                            DataFactory.SqlDataBase().UpdateByHashtable("clientcoupon", "id", dss1.Rows[0]["CouponID"].ToString(), hs);
                        }
                        string name = dss1.Rows[0]["Name"].ToString();
                        string phone = dss1.Rows[0]["Mobile"].ToString();
                        string memberId = dss1.Rows[0]["MemberId"].ToString();
                        string openid = dss1.Rows[0]["OpenId"].ToString();
                        //财务记录
                        int iss = CommonClass.InsertFinance(OrderNum, name, phone, money, 11, "餐厅点餐预订支付", memberId, transaction_id, adminHotelid, CommonHelper.GetInt(hotelId), "", openid);

                        if (iss > 0)
                        {
                            if (dss1.Rows[0]["PayType"].ToString() == "11")//微信支付
                            {
                                //更新订单表、订单详情表
                                CommonMethod.UPDATE_Food_Order(dss1.Rows[0]["ID"].ToString(), dss1.Rows[0]["OrderNum"].ToString(), dss1.Rows[0]["TableCode"].ToString(), dss1.Rows[0]["OpenId"].ToString(), dss1.Rows[0]["RestaurantId"].ToString().ToString(), dss1.Rows[0]["RestaurantName"].ToString(), dss1.Rows[0]["AdminHotelid"].ToString(), dss1.Rows[0]["Hotelid"].ToString());
                                //国光存储过程返回值
                                string Food_clbj = "";
                                string Food_ret = "";
                                Hashtable rs = new Hashtable();
                                Hashtable ht = new Hashtable();
                                ht["cth"] = dss1.Rows[0]["TableCode"].ToString();
                                ht["zffs"] = 2;
                                ht["zfje"] = money;
                                ht["yhje"] = couponmoney;
                                ht["vipkh"] = null;
                                ht["wxfkdh"] = OrderNum;
                                ht["OUT_clbj"] = null;
                                ht["OUT_ret"] = null;
                                DataFactory.SqlDataBase(dss1.Rows[0]["AdminHotelid"].ToString(), dss1.Rows[0]["Hotelid"].ToString()).ExecuteByProcReturn("proc_ctjsfk", ht, ref rs);
                                if (rs != null && rs.Count > 0)
                                {
                                    if (rs["OUT_clbj"] != null && rs["OUT_clbj"].ToString() != "")
                                    {
                                        Food_clbj = rs["OUT_clbj"].ToString();
                                    }
                                    if (rs["OUT_ret"] != null && rs["OUT_ret"].ToString() != "")
                                    {
                                        Food_ret = rs["OUT_ret"].ToString();
                                    }
                                }
                                //智订云更新值
                                Hashtable info = new Hashtable();
                                info["Pay"] = 1;//已支付
                                info["State"] = 1;// 0待支付 1待处理（待确认）、2已确认、3已完成、4已取消
                                info["PayTime"] = DateTime.Now;
                                info["Food_Clbj"] = Food_clbj;
                                info["Food_Ret"] = Food_ret;
                                DataFactory.SqlDataBase().UpdateByHashtable("FoodOrder", "ordernum", a[0], info);//更新订单

                            }
                            Log.Info("微信点餐订单支付", a[0].ToString() + "订单号");

                            //更新点餐数量
                            CommonMethod.UPDATE_Food_Num(a[0].ToString());

                            #region 发送消息
                            //TemplateMessage.FoodSen(a[0].ToString(), "1");//推送模板消息给 预订人
                            TemplateMessage.FoodSen(a[0].ToString(), "1");
                            //CommonMethod.SendMessage_MDZDY(a[0].ToString());//推送消息给智订云管理员及员工
                            #endregion
                        }
                    }
                    else
                    {
                        Log.Info("微信点餐订单支付错误信息：", "未找到订单!");
                    }

                }

            }
            catch (Exception e)
            {
                Log.Info("微信服务订单支付错误信息：", e.Message);
            }
        }

        /// <summary>
        /// 订单传送到国光数据库0不传送1传送
        /// </summary>
        /// <returns></returns>
        public int dzcs(string AdminHotelid)
        {
            int fhz = 0;
            string sql = string.Format(@"select IsIntranet from Set_Association where AdminHotelid=@AdminHotelid");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", AdminHotelid)};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (ds != null && ds.Rows.Count > 0)
            {
                if (ds.Rows[0]["IsIntranet"].ToString() == "1")
                {
                    fhz = 1;
                }
            }
            return fhz;
        }

        private bool QueryOrder(string transaction_id, string AdminHotelid, int Hotelid)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req, AdminHotelid, Hotelid);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //获取酒店名称
        private string GetHotelName(string id, string AdminHotelid)
        {
            string hotelname = "";
            StringBuilder hotelsql = new StringBuilder();
            hotelsql.AppendFormat("SELECT ID,name FROM Hotel WHERE AdminHotelid='{0}' and id={1} ", AdminHotelid, id);
            DataTable hoteldt = DataFactory.SqlDataBase().GetDataTableBySQL(hotelsql);
            if (hoteldt != null && hoteldt.Rows.Count > 0)
            {
                hotelname = hoteldt.Rows[0]["name"].ToString();
            }
            return hotelname;
        }

        //获取规则名称
        private string GetRule(string ID)
        {
            string name = "";
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT Rule_Name FROM Room_Rule WHERE ID={0} ", ID);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                name = dt.Rows[0][0].ToString();
            }
            return name;
        }

        //消费红包
        private void AddReward(string openid, string AdminHotelid)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("SELECT lsh,kh,hylx,xm,sjhm FROM dbo.hy_hyzlxxb WHERE CarId='{0}' AND AdminHotelid='{1}'", openid, AdminHotelid);
                DataTable dt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    StringBuilder sqlisjf = new StringBuilder();
                    sqlisjf.AppendFormat(@"SELECT * FROM dbo.RewardInfo WHERE AdminHotelid='{0}' 
                                            AND IsEnabless=1
                                            AND TotalAllocationss>(SELECT ISNULL(SUM(RewardMoney),0)  FROM RewarAccount WHERE  DateDiff(dd,AddTime,getdate())=0 AND AdminHotelid='{0}' AND TypeID=4  ) 
AND Numberss>(SELECT ISNULL(COUNT(RewardMoney),0) FROM RewarAccount WHERE  DateDiff(dd,AddTime,getdate())=0 AND Openid='{1}'  AND TypeID=4 ) ", AdminHotelid, openid);
                    DataTable dtisjf = DataFactory.SqlDataBase().GetDataTableBySQL(sqlisjf);
                    if (dtisjf != null && dtisjf.Rows.Count > 0)
                    {
                        Random ra = new Random();

                        int num = ra.Next(CommonHelper.GetInt(dtisjf.Rows[0]["MinMoneyss"]), CommonHelper.GetInt(dtisjf.Rows[0]["MaxMoneyss"]) - 1);
                        int xs = ra.Next(0, 10);
                        double nums = num + xs / 10.0;


                        string zmsm = "消费赠送";
                        Hashtable ht = new Hashtable();
                        ht["Openid"] = openid;
                        ht["RewardMoney"] = nums;
                        ht["AdminHotelid"] = AdminHotelid;
                        ht["TypeName"] = zmsm;
                        ht["TypeID"] = 4;
                        ht["RewardType"] = 1;
                        ht["Url"] = "";
                        ht["Name"] = dt.Rows[0]["xm"];
                        ht["Phone"] = dt.Rows[0]["sjhm"];
                        int x = DataFactory.SqlDataBase().InsertByHashtable("RewarAccount", ht);


                    }
                    else
                    {

                    }


                }
            }
            catch { }

        }

        //修改卡券状态为已使用,多个用,隔开 
        private void UpdateCoupon(string Coupon)
        {
            try
            {
                string[] cids = Coupon.Split(',');
                for (int i = 0; i < cids.Length; i++)
                {
                    Hashtable hs = new Hashtable();
                    hs["isReceive"] = true;
                    DataFactory.SqlDataBase().UpdateByHashtable("clientcoupon", "id", cids[i], hs);
                }
            }
            catch (Exception ex)
            {
                Log.Error("处理卡券错误", ex.Message);
            }
        }
    }
}