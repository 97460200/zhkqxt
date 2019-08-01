using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.Lib;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using System.Text;
using System.Collections;
using RM.Web.business;

namespace RM.Web.WX_SET
{
    public partial class QueryOrder : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            //WxPayData notifyData = QueryOrderData("WX19042559975812", "1009333", 105);
            //CheckData(notifyData);
        }

        private WxPayData QueryOrderData(string out_trade_no, string AdminHotelid, int Hotelid)
        {
            WxPayData req = new WxPayData();
            req.SetValue("out_trade_no", out_trade_no);
            WxPayData res = WxPayApi.OrderQuery(req, AdminHotelid, Hotelid);
            return res;
        }

        private void CheckData(WxPayData notifyData)
        {
            WxPayData res = new WxPayData();
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error("订单号不存在", res.ToXml());
                return;
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            Log.Info("支付订单信息", notifyData.GetValue("attach").ToString());
            //填写自己的逻辑判断  a[0]订单号， a[1]酒店id ， a[2]会员卡支付
            string[] a = notifyData.GetValue("attach").ToString().Split(',');
            string AdminHotelid = "";
            int Hotelid = 0;
            AdminHotelid = a[1];
            if (a[2] == "客房预订" || a[2] == "客房续住" || a.Length > 3)
            {
                Hotelid = CommonHelper.GetInt(a[3]);
            }

            res.SetValue("return_code", "SUCCESS");
            res.SetValue("return_msg", "OK");

            if (a[2] == "客房预订")
            {
                wxzf(a, transaction_id, CommonHelper.GetInt(notifyData.GetValue("total_fee").ToString()) / 100);
            }
        }

        /// <summary>
        /// 微信订单支付
        /// </summary>
        /// <param name="a"></param>
        /// 
        /// <param name="transaction_id"></param>
        /// <param name="money"></param>
        private void wxzf(string[] a, string transaction_id, int money)
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
                        DataTable dsswx = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls1wx));
                        if (dsswx != null && dsswx.Rows.Count > 0)
                        {
                            if (money != CommonHelper.GetInt(dsswx.Rows[0]["TomePrice"]))//判断支付金额是否一致
                            {
                                Log.Info("支付金额错误：", "应付" + dsswx.Rows[0]["TomePrice"] + ",实付" + money + "。");
                                return;
                            }

                            StringBuilder sqlwx = new StringBuilder();
                            sqlwx.AppendFormat(@"
                            INSERT INTO Reservation
                            (OrderNum,RoomType,Number,Days,BeginTime,EndTime,Adult,RoomTotal,ServiceTotal,
                            TomePrice,Name,Sex,Mobile,Zip,Arrived,Other,Remark,PayType,State,
                            MemberId,Comm,Environment,Source,Pay,KeepTime,Quality,HotelId,Coupon,Distributor,
                            AdminHotelid,isdelete,rpid,XX_SKYDDB,openid,PayOpenid,RoomId,RuleId,StaffId,StaffRemarks,VipCode,VipName,FirstLive,ContinueState,ContinueOrderNum)
                             SELECT
                             OrderNum,RoomType,Number,Days,BeginTime,EndTime,Adult,RoomTotal,ServiceTotal,
                             TomePrice,Name,Sex,Mobile,Zip,Arrived,Other,Remark,PayType,State,
                             MemberId,Comm,Environment,Source,Pay,KeepTime,Quality,HotelId,Coupon,Distributor,
                             AdminHotelid,isdelete,rpid,XX_SKYDDB,openid,PayOpenid,RoomId,RuleId,StaffId,StaffRemarks,VipCode,VipName,FirstLive,ContinueState,ContinueOrderNum
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
                        if (money != CommonHelper.GetInt(dt_OrderInfo.Rows[0]["TomePrice"]))//判断支付金额是否一致
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
                                ht["zfj"] = CommonHelper.GetInt(dt_OrderInfo.Rows[0]["TomePrice"]);
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

                                Hashtable info = new Hashtable();
                                info["XX_SKYDDB"] = yddh;//国光订单编号
                                DataFactory.SqlDataBase().UpdateByHashtable("Reservation", "ordernum", OrderNum, info);//更新订单
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

        //修改卡券状态为已使用,多个用,隔开 
        private void UpdateCoupon(string Coupon)
        {
            try
            {
                for (int i = 0; i < Coupon.Split(',').Length; i++)
                {
                    Hashtable hs = new Hashtable();
                    hs["isReceive"] = true;
                    DataFactory.SqlDataBase().UpdateByHashtable("clientcoupon", "id", Coupon.Split(',')[i], hs);
                }
            }
            catch (Exception ex)
            {
                Log.Error("处理卡券错误", ex.Message);
            }

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

                        //lhb.Style.Add("display", "block");

                    }
                    else
                    {
                        //lhb.Style.Add("display", "none");
                    }


                }
            }
            catch { }

        }

    }
}