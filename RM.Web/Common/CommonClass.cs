using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.Data;
using RM.Busines;
using RM.Common.DotNetCode;
using System.Text;
using LitJson;

/// <summary>
/// 公共类
/// </summary>
public class CommonClass
{
    public CommonClass()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    #region ******** 获取功能是否开启 ********

    /// <summary>
    /// 获取功能是否开启  Wx_function表,1为开启可用
    /// </summary>
    /// <param name="adminHotelId">集团id</param>
    /// <param name="columnName">列</param>
    /// <returns></returns>
    public static bool CheckFunctionIsOpen(string adminHotelId, string columnName)
    {
        return ApplicationHelper.CheckFunctionIsOpen(adminHotelId, columnName);
    }
    #endregion

    #region ******** 验证会员是否为员工 ********

    /// <summary>
    /// 验证会员是否为员工
    /// </summary>
    /// <param name="openid"></param>
    /// <returns></returns>
    public static bool CheckMemberIsUser(string adminHotelId, string openid)
    {
        bool bl = false;
        HttpContext rq = HttpContext.Current;
        string key_name = "MemberIsUser" + adminHotelId;
        if (rq.Session[key_name] == null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT  User_ID
            FROM    dbo.Base_UserInfo
            WHERE   User_Account IN (
                    SELECT  Phone
                    FROM    dbo.MemberInfo
                    WHERE   Openid = @Openid
                            AND AdminHotelId = @AdminHotelId )
                    AND DeleteMark = 1
                    AND AdminHotelid = @AdminHotelId
            ");
            SqlParam[] param = new SqlParam[] {
                new SqlParam("@AdminHotelId",adminHotelId),
                new SqlParam("@Openid",openid)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null)
            {
                bl = dt.Rows.Count > 0;
            }
            rq.Session[key_name] = bl;
        }
        else
        {
            bl = (bool)rq.Session[key_name];
        }
        return bl;
    }
    #endregion

    #region ******** 添加扫码记录 ********

    /// <summary>
    /// 添加员工扫码记录
    /// </summary>
    /// <param name="adminHotelId"></param>
    /// <param name="hotelId"></param>
    /// <param name="openid"></param>
    /// <param name="UserId"></param>
    /// <param name="sourceType"></param>
    /// <param name="sourceExplain"></param>
    public static void AddUserSource(string adminHotelId, string hotelId, string openid, string UserId, int sourceType, string sourceExplain)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT TOP 1 ID FROM MemberSource WHERE AddTime > @AddTime AND openid=@openid AND TGMember = @TGMember");
        SqlParam[] param = new SqlParam[]{
                    new SqlParam("@AddTime",DateTime.Now.AddHours(-6)),
                    new SqlParam("@openid",openid),
                    new SqlParam("@TGMember",UserId)
                };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);//  避免刷新重复添加
        if (dt != null && dt.Rows.Count > 0)
        {
            return;
        }

        Hashtable Temporaryht = new Hashtable();
        Temporaryht["name"] = "";
        Temporaryht["Phone"] = "";
        Temporaryht["AdminHotelid"] = adminHotelId;
        Temporaryht["HotelId"] = hotelId;
        Temporaryht["openid"] = openid;
        Temporaryht["HotelName"] = "";
        Temporaryht["TGMember"] = UserId;
        Temporaryht["MemberId"] = "";
        Temporaryht["TGType"] = sourceType;
        Temporaryht["Source"] = sourceExplain;
        Temporaryht["fjPhone"] = "";
        Temporaryht["AddTime"] = DateTime.Now;
        int w = DataFactory.SqlDataBase().InsertByHashtable("MemberSource", Temporaryht);
    }

    /// <summary>
    /// 添加会员扫码记录
    /// </summary>
    /// <param name="adminHotelId"></param>
    /// <param name="hotelId"></param>
    /// <param name="openid"></param>
    /// <param name="memberId"></param>
    /// <param name="sourceType"></param>
    /// <param name="sourceExplain"></param>
    public static void AddMemberSource(string adminHotelId, string hotelId, string openid, string memberId, int sourceType, string sourceExplain)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT TOP 1 ID FROM MemberSource WHERE AddTime > @AddTime AND openid=@openid AND MemberId = @MemberId");
        SqlParam[] param = new SqlParam[]{
                    new SqlParam("@AddTime",DateTime.Now.AddHours(-6)),
                    new SqlParam("@openid",openid),
                    new SqlParam("@MemberId",memberId)
                };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);// 避免刷新重复添加
        if (dt != null && dt.Rows.Count > 0)
        {
            return;
        }

        Hashtable Temporaryht = new Hashtable();
        Temporaryht["name"] = "";
        Temporaryht["Phone"] = "";
        Temporaryht["AdminHotelid"] = adminHotelId;
        Temporaryht["HotelId"] = hotelId;
        Temporaryht["openid"] = openid;
        Temporaryht["HotelName"] = "";
        Temporaryht["TGMember"] = "";
        Temporaryht["MemberId"] = memberId;
        Temporaryht["TGType"] = sourceType;
        Temporaryht["Source"] = sourceExplain;
        Temporaryht["fjPhone"] = "";
        Temporaryht["AddTime"] = DateTime.Now;
        int w = DataFactory.SqlDataBase().InsertByHashtable("MemberSource", Temporaryht);
    }
    #endregion

    #region **营销中心日志**
    public static void UserBrowseLog(string AdminHotelid, string hotelid, string UserId, string BrowseType, string Url)
    {
        Hashtable ht = new Hashtable();
        ht["AdminHotelId"] = AdminHotelid;
        ht["HotelId"] = hotelid;
        ht["UserId"] = UserId;
        ht["BrowseType"] = BrowseType;
        ht["Url"] = Url;
        DataFactory.SqlDataBase().InsertByHashtable("UserBrowseLog", ht);
    }
    #endregion

    #region **会员操作日志**
    public static void MemberOperationLog(string AdminHotelid, string hotelid, string openId, string operationType, string remarks, string urls)
    {
        Hashtable ht = new Hashtable();
        ht["AdminHotelId"] = AdminHotelid;
        ht["HotelId"] = hotelid;
        ht["OpenId"] = openId;
        ht["OperationType"] = operationType;
        ht["Remarks"] = remarks;
        ht["Url"] = urls;
        DataFactory.SqlDataBase().InsertByHashtable("MemberOperationLog", ht);
    }
    #endregion

    #region 保存房价明细
    public static void SaveReservationDetail(string OrderNum, int Number, int RoomId, int RuleId, string VipCode, string StartDate, string EndTime)
    {
        Hashtable htRoomPrice = new Hashtable();
        htRoomPrice["RoomId"] = RoomId;
        htRoomPrice["RuleId"] = RuleId;
        htRoomPrice["VipCode"] = VipCode;
        htRoomPrice["StartDate"] = StartDate;
        htRoomPrice["EndTime"] = EndTime;
        DataTable dtRoomPrice = DataFactory.SqlDataBase().GetDataTableProc("P_GetRoomPrice", htRoomPrice);

        if (dtRoomPrice != null && dtRoomPrice.Rows.Count > 0)
        {
            for (int i = 0; i < dtRoomPrice.Rows.Count - 1; i++)
            {
                int day_jg = CommonHelper.GetInt(dtRoomPrice.Rows[i]["Price"]); //价格
                int day_jf = CommonHelper.GetInt(dtRoomPrice.Rows[i]["Integral"]); //积分
                string DateRange = dtRoomPrice.Rows[i]["DateRange"].ToString();//日期
                string DateWeek = getWeekDay(DateRange);//星期
                Hashtable ht = new Hashtable();
                ht["OrderNum"] = OrderNum;
                ht["RoomId"] = RoomId;
                ht["RuleId"] = RuleId;
                ht["DateRange"] = DateRange;
                ht["DateWeek"] = DateWeek;
                ht["Price"] = day_jg;
                ht["Integral"] = day_jf;
                ht["Number"] = Number;
                ht["PriceTotal"] = day_jg * Number;
                ht["NumberTotal"] = day_jf * Number;
                DataFactory.SqlDataBase().InsertByHashtable("ReservationPriceDetail", ht);
            }
        }
    }

    public static string getWeekDay(string date)
    {
        DateTime dt = Convert.ToDateTime(date);
        string week = string.Empty;
        switch (dt.DayOfWeek)
        {
            case DayOfWeek.Monday:
                week = "周一";
                break;
            case DayOfWeek.Tuesday:
                week = "周二";
                break;
            case DayOfWeek.Wednesday:
                week = "周三";
                break;
            case DayOfWeek.Thursday:
                week = "周四";
                break;
            case DayOfWeek.Friday:
                week = "周五";
                break;
            case DayOfWeek.Saturday:
                week = "周六";
                break;
            case DayOfWeek.Sunday:
                week = "周日";
                break;
        }
        return week;
    }
    #endregion

    #region 保存财务记录
    /// <summary>
    /// 保存财务记录
    /// </summary>
    /// <param name="Number">编号</param>
    /// <param name="Name">姓名</param>
    /// <param name="Phone">手机号码</param>
    /// <param name="Monery">金额</param>
    /// <param name="Types">1微信支付,2会员卡支付,3积分兑换,4会员卡充值,5会员卡升级,6预订卡券抵扣,7到店付款,8订单退款,9会员卡充值赠送,10 注册购买会员卡</param>
    /// <param name="Detail">说明</param>
    /// <param name="MemberId">国光会员id</param>
    /// <param name="wxddh">微信单号</param>
    /// <param name="AdminHotelid">集团酒店id</param>
    /// <param name="hotelid">酒店id</param>
    /// <param name="refund_id">退款id</param>
    /// <param name="OpenId">微信id</param>
    /// <returns></returns>
    public static int InsertFinance(string Number, string Name, string Phone, double Monery, int Types, string Detail, string MemberId, string wxddh, string AdminHotelid, int hotelid, string refund_id, string OpenId)
    {
        Hashtable ht = new Hashtable();
        ht["Number"] = Number;
        ht["Name"] = Name;
        ht["Phone"] = Phone;
        ht["Monery"] = Monery;
        ht["Type"] = Types;
        ht["Detail"] = Detail;
        ht["MemberId"] = MemberId;
        ht["wxddh"] = wxddh;
        ht["AdminHotelid"] = AdminHotelid;
        ht["hotelid"] = hotelid;
        ht["refund_id"] = refund_id;
        ht["OpenId"] = OpenId;
        int bl = DataFactory.SqlDataBase().InsertByHashtable("Finance", ht);
        return bl;
    }

    #endregion

    #region ******** 成订单编号 ********
    /// <summary>
    /// 生成客房订单编号
    /// </summary>
    /// <returns></returns>
    public static string CreateOrderNumber(string hotelid, string phone)
    {
        Random Rdm = new Random();
        int iRdm = Rdm.Next(10, 99);
        string code = DateTime.Now.ToString("yyMMdd") + iRdm.ToString();
        code += phone.Substring(phone.Length - 4);
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        SELECT  SUM(sl) sl
        FROM    ( SELECT    COUNT(1) sl
                  FROM      Reservation
                  WHERE     HotelId = @HotelId
                            AND AddTime > @AddTime
                  UNION ALL
                  SELECT    COUNT(1) sl
                  FROM      dbo.WeChat_Reservation
                  WHERE     HotelId = @HotelId
                            AND AddTime > @AddTime
                ) tb
        ");
        SqlParam[] param = new SqlParam[] {
            new SqlParam("@HotelId",hotelid),
            new SqlParam("@AddTime",DateTime.Now.ToString("yyyy-MM-dd"))
        };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (dt != null)
        {
            int sl = Convert.ToInt32(dt.Rows[0]["sl"].ToString()) + 1;
            code += (sl < 10) ? "0" + sl.ToString() : sl.ToString();
        }
        return code;
    }
    /// <summary>
    /// 生成预售券订单编号
    /// </summary>
    /// <returns></returns>
    public static string TicketOrderNumber(string hotelid, string phone)
    {
        Random Rdm = new Random();
        int iRdm = Rdm.Next(10, 99);
        string code = DateTime.Now.ToString("yyMMdd") + iRdm.ToString();
        code += phone.Substring(phone.Length - 4);
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        SELECT  SUM(sl) sl
        FROM    ( SELECT    COUNT(1) sl
                  FROM      TP_Order
                  WHERE     HotelId = @HotelId
                            AND AddTime > @AddTime
                  UNION ALL
                  SELECT    COUNT(1) sl
                  FROM      dbo.TP_OrderTemp
                  WHERE     HotelId = @HotelId
                            AND AddTime > @AddTime
                ) tb
        ");
        SqlParam[] param = new SqlParam[] {
            new SqlParam("@HotelId",hotelid),
            new SqlParam("@AddTime",DateTime.Now.ToString("yyyy-MM-dd"))
        };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (dt != null)
        {
            int sl = Convert.ToInt32(dt.Rows[0]["sl"].ToString()) + 1;
            code += (sl < 10) ? "0" + sl.ToString() : sl.ToString();
        }
        return code;
    }


    /// <summary>
    /// 生成押金订单编号
    /// </summary>
    /// <returns></returns>
    public static string CashPledgeOrderNumber(string hotelid)
    {
        Random Rdm = new Random();
        int iRdm = Rdm.Next(10, 99);
        string code = DateTime.Now.ToString("yyMMddHHmm") + iRdm.ToString();
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        SELECT  SUM(sl) sl
        FROM    ( SELECT    COUNT(1) sl
                  FROM      CashPledge_Order
                  WHERE     HotelId = @HotelId
                            AND PayTime > @AddTime
                  UNION ALL
                  SELECT    COUNT(1) sl
                  FROM      dbo.CashPledge_TempOrder
                  WHERE     HotelId = @HotelId
                            AND PayTime > @AddTime
                ) tb
        ");
        SqlParam[] param = new SqlParam[] {
            new SqlParam("@HotelId",hotelid),
            new SqlParam("@AddTime",DateTime.Now.ToString("yyyy-MM-dd"))
        };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (dt != null)
        {
            int sl = Convert.ToInt32(dt.Rows[0]["sl"].ToString()) + 1;
            code += (sl < 10) ? "0" + sl.ToString() : sl.ToString();
        }
        return code;
    }
    #endregion

    #region ******** 默认统计查询时间 ********
    /// <summary>
    /// 获取酒店默认统计时间
    /// </summary>
    /// <param name="adminHotelId"></param>
    /// <param name="startDay"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    public static void GetQueryTime(string adminHotelId, ref string startDay, ref string startTime, ref string endTime)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("SELECT * FROM AccountQueryTime WHERE AdminHotelId = @AdminHotelId");
        IList<SqlParam> lParam = new List<SqlParam>();
        lParam.Add(new SqlParam("@AdminHotelId", adminHotelId));
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, lParam.ToArray());
        if (dt != null && dt.Rows.Count > 0)
        {
            startDay = dt.Rows[0]["StartDay"].ToString();
            startTime = dt.Rows[0]["StartTime"].ToString();
            endTime = dt.Rows[0]["EndTime"].ToString();
        }
    }
    #endregion

    #region 获取 星期几为周末

    /// <summary>
    /// 获取集团/酒店设置的周末
    /// </summary>
    /// <param name="adminHotelId"></param>
    /// <param name="hotelId"></param>
    /// <returns></returns>
    public static string GetWeek(string adminHotelId, string hotelId)
    {
        string week = ",4,5,";
        StringBuilder sql = new StringBuilder();
        if (!string.IsNullOrEmpty(adminHotelId))
        {
            sql.AppendFormat("SELECT Weekend FROM Weekend WHERE AdminHotelid=@AdminHotelid");
        }
        else if (!string.IsNullOrEmpty(hotelId))
        {
            sql.AppendFormat(@"
            SELECT  Weekend
            FROM    dbo.Weekend
            WHERE   AdminHotelid IN ( SELECT    AdminHotelid
                                      FROM      dbo.Hotel
                                      WHERE     ID = @hotelId )
            ");

        }
        SqlParam[] param = new SqlParam[] { 
            new SqlParam("@AdminHotelid",adminHotelId ),
            new SqlParam("@HotelID",hotelId )
        };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                week += "," + dt.Rows[i]["Weekend"].ToString();
            }
        }

        return week;
    }

    /// <summary>
    /// 获取星期几，返回整形
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static int GetWeek(DateTime dt)
    {
        int w = 0;
        string weekstr = dt.DayOfWeek.ToString();
        switch (weekstr)
        {
            case "Monday": w = 0; break;
            case "Tuesday": w = 1; break;
            case "Wednesday": w = 2; break;
            case "Thursday": w = 3; break;
            case "Friday": w = 4; break;
            case "Saturday": w = 5; break;
            case "Sunday": w = 6; break;
        }
        return w;
    }
    #endregion

    #region ** 更新预定存储过程 **

    public static void UpdateSysData(string adminHotelId, string hotelId, string dataType)
    {
        string dataVal = "";
        string key_name = "UpdateSysData" + adminHotelId + hotelId + dataType;
        HttpContext hc = HttpContext.Current;

        if (hc.Application[key_name] != null)
        {
            dataVal = hc.Application[key_name].ToString();
            return;
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM UpdateSysData WHERE AdminHotelId=@AdminHotelId AND HotelId=@HotelId AND DataType=@DataType ");
            SqlParam[] parm = new SqlParam[] { 
                new SqlParam("@AdminHotelId", adminHotelId),
                new SqlParam("@HotelId", hotelId),
                new SqlParam("@DataType", dataType)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm);
            if (dt != null && dt.Rows.Count > 0)
            {
                hc.Application[key_name] = "1";
                return;
            }
        }

        switch (dataType)
        {
            case "proc_wxydcl":
                set_proc_wxydcl(adminHotelId, hotelId);
                SetDayPrice(1, adminHotelId, hotelId);
                break;
            case "proc_wxxzcl":
                set_proc_wxxzcl(adminHotelId, hotelId);
                SetDayPrice(2, adminHotelId, hotelId);
                break;
            default:
                break;
        }
        Hashtable ht = new Hashtable();
        ht["AdminHotelId"] = adminHotelId;
        ht["HotelId"] = hotelId;
        ht["DataType"] = dataType;
        DataFactory.SqlDataBase().InsertByHashtable("UpdateSysData", ht);
    }

    //生成每日房价表
    private static void SetDayPrice(int type, string adminHotelId, string hotelId)
    {
        if (type == 1)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select Top 1 * From XX_yddhrqfj");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sb);
            if (dt == null || dt.Rows.Count == 0)
            {
                #region  ** **
                StringBuilder sb_update = new StringBuilder();
                sb_update.Append(@"
                If Exists ( Select  0
                            From    sysobjects
                            Where   name = 'XX_yddhrqfj'
                                    And xtype = 'U' )
                  Drop Table XX_yddhrqfj              
                ");
                DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteBySql(sb_update);

                sb_update = new StringBuilder();
                sb_update.Append(@"
                Create Table XX_yddhrqfj
                  (
                    [yddh] [Numeric](10, 0) Not Null ,
                    [rq] [DateTime] Not Null ,
                    [fj] [Money] Null ,
                    [bz] [Varchar](200) Null ,
                    Primary Key ( yddh, rq )
                  )
                On
                  [PRIMARY]
                ");
                DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteBySql(sb_update);

                sb_update = new StringBuilder();
                sb_update.Append(@"
                If Exists ( Select  0
                            From    sysobjects
                            Where   name = 'XX_yddhrqfj_insert'
                                    And xtype = 'TR' )
                  Drop Trigger XX_yddhrqfj_insert
                ");
                DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteBySql(sb_update);

                sb_update = new StringBuilder();
                sb_update.Append(@"
                Create Trigger XX_yddhrqfj_insert On XX_yddhrqfj
                  For Insert
                As
                  Declare @rq DateTime ,
                    @yddh Numeric(10, 0) ,
                    @fj Money
                  Select  @rq = rq ,
                          @yddh = yddh ,
                          @fj = fj
                  From    INSERTed
  
                  Update  XX_SKYDDB
                  Set     fj = @fj
                  Where   yddh = @yddh
                          And ddrq = @rq
                ");
                DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteBySql(sb_update);

                #endregion
            }
        }
        else if (type == 2)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select Top 1 * From XX_FJZHRQFJ");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sb);
            if (dt == null || dt.Rows.Count == 0)
            {
                #region  ** **
                StringBuilder sb_update = new StringBuilder();
                sb_update.Append(@"
                If Exists ( Select  0
                            From    sysobjects
                            Where   name = 'XX_FJZHRQFJ'
                                    And xtype = 'U' )
                  Drop Table XX_FJZHRQFJ            
                ");
                DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteBySql(sb_update);

                sb_update = new StringBuilder();
                sb_update.Append(@"
                Create Table XX_FJZHRQFJ
                  (
                    fjzh Numeric(10, 0) Not Null ,
                    rq DateTime Not Null ,
                    fj Money Null ,
                    bz Varchar(200) Null ,
                    Primary Key ( fjzh, rq )
                  )
                ");
                DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteBySql(sb_update);

                #endregion
            }
        }
    }

    /// <summary>
    /// 更新预定存储过程
    /// </summary>
    /// <param name="adminHotelId"></param>
    /// <param name="hotelId"></param>
    private static void set_proc_wxydcl(string adminHotelId, string hotelId)
    {
        StringBuilder sb_del = new StringBuilder();
        sb_del.Append("Drop Procedure proc_wxydcl");
        DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteBySql(sb_del);

        StringBuilder sql = new StringBuilder();
        sql.Append(@"
        CREATE Proc proc_wxydcl
            @ddrq DateTime ,
            @ldrq DateTime ,
            @krname Varchar(30) ,
            @ydly Smallint ,
            @tbyq Varchar(30) ,
            @kfcode Varchar(10) ,
            @ordernum Varchar(200) ,
            @dh Varchar(200) ,
            @pay Smallint ,
            @paytype Smallint ,
            @ydsl Smallint ,
            @zfj Numeric(10, 2) ,
            @vipkh Varchar(50) ,
            @wxid Varchar(100) ,
            @dfbz Varchar(200) ,
            @yddh Numeric(10, 0) Output
        As
            Begin Tran TRA_wxydcl                                        
            Declare @xttoday DateTime ,
                @fkcode Char(4) ,
                @fkdesc Varchar(50) ,
                @sjrq DateTime ,
                @krxb Char(1) ,
                @gjcode Varchar(5) ,
                @sales Varchar(30) ,
                @txbh Numeric(10, 0) ,
                @pyname Varchar(100)                                             
            Declare @bz Varchar(200) ,
                @bc Char(1) ,
                @scmcode Varchar(30) ,
                @krlxcode Varchar(30) ,
                @dw Varchar(100) ,
                @zdts Smallint                                        
            Declare @hydwbh Numeric(10, 0) ,
                @vipbj Char(1) ,
                @fj Numeric(10, 2) ,
                @maxxh Numeric(10, 0) ,
                @qryg Varchar(20)                                        
            
            
        --验证订单是否已经生成            
            If Exists ( Select  yddh
                        From    XX_SKYDDB
                        Where   wxddbh = @ordernum )
                Begin            
                            
                    Select  @yddh=yddh
                    From    xx_skyddb
                    Where   wxddbh = @ordernum            
                                  
                    Commit Tran                                             
                    Return                     
                End            
                    
            Select  @bz='订单编号:'+@ordernum                                        
            Select  @krname=IsNull(@krname, '')                                        
            Select  @vipbj='F'                               
            If @pay = 1
                Select  @vipbj='T'                                         
            Select  @zdts=DateDiff(Day, @ddrq, @ldrq)       
            If @zdts <= 0
                Select  @zdts=1                                       
            Select  @pyname=@krname                                        
            If @ydsl = 0
                Select  @ydsl=1                                        
            Select  @fj=@zfj/@ydsl                                        
          
            Select  @fj=@fj/@zdts                                    
                                        
            Select  @bz=Case @paytype
                          When 1 Then '前台支付'
                          When 2 Then '积分兑换'
                          When 3 Then '会员卡支付'
                          When 4 Then '公众号支付'
                          When 5 Then '优惠券抵扣'
                          When 6 Then '套票抵扣'
                        End           
            Select  @fkcode=Case @paytype
                              When 1 Then ''
                              When 2 Then 'JF'
                              When 3 Then 'MC'
                              When 4 Then 'GZH'
                              When 5 Then 'YHQ'
                              When 6 Then 'TP'
                            End         
  
            Select  @fkdesc=Case @paytype
                              When 1 Then ''
                              When 2 Then '积分兑换'
                              When 3 Then '会员卡支付(公众号扣款)'
                              When 4 Then '公众号支付'
                              When 5 Then '优惠券抵扣'
                              When 6 Then '套票抵扣'
                            End           
    
            If Len(@dfbz) <> 0
                Select  @bz=@bz+','+@dfbz                            
            If @ydly = 1
                Begin                                        
                    Select  @scmcode='WEB'                                        
                    Select  @krlxcode='WEB'                                        
                    Select  @dw='官网预订'                                        
                End                                        
            Else
                Begin                          
                    Select  @scmcode='WX'                                        
                    Select  @krlxcode='WX'                                        
                    Select  @dw='微信公众号预订'                                        
                End                                        
            Select  @krxb='M'                                        
            Select  @gjcode='CN'                                        
            Select  @sales='WX'                                        
                                     
            Select  @xttoday=DateAdd(Day, DateDiff(Day, GetDate(), xtdate), GetDate()), @sjrq=GetDate()
            From    CS_QTXTZTB                                            
            Select  @bc=Min(bc)
            From    CS_BCXXB
            Where   bj = 'F'       
                                   
            Select  @txbh=Max(txbh)
            From    XX_SKYDDB                                        
            Select  @txbh=IsNull(@txbh, 0)                                        
            Select  @txbh=@txbh+1                                        
                 
            Insert  Into XX_SKYDDB ( yddh, lsbh, ydrname, kycode, krlxcode, ydfscode, gjcode, qzcode, qzhm, rjxx, zscode, zjxycode, language, krname, krenname, krsename, pyname, krxb, zjcode, krzjhm, kfcode, kfyq, fh, dfs, krrs, xrrs, ddrq, ddxx, zdts, ldrq, ldxx, yjje, dw, dz, dh, bp, fax, dch, email, hydwbh, hth, condition, vipbj, tbdabj, fkcode, xykh, tsxqbj, lybj, scmcode, fj, yszzh, ydzt, usedflag, qrrq, qrr, czrq, czygh, czbc, xgrq, xgygh, bz, shbj, txbh, wxid, wxddbh )
            Values  ( 1000, Null, @krname, Null, @krlxcode, Null, @gjcode, Null, Null, Null, Null, Null, Null, @krname, '', @vipkh, @pyname, @krxb, Null, Null, @kfcode, @tbyq, Null, @ydsl, Null, Null, @ddrq, Null, @zdts, @ldrq, Null, Null, @dw, Null, @dh, Null, Null, Null, Null, @hydwbh, @qryg, Null, @vipbj, 'F', Null, Null, 'F', 'F', @scmcode, @fj, Null, 'DR', 'F', Null, @sales, @xttoday, '999999', @bc, Null, Null, @bz, Null, @txbh, @wxid, @ordernum )                                          
            Select  @yddh=Max(yddh)
            From    xx_skyddb
            Where   krname = @krname And DateDiff(Day, ddrq, @ddrq) = 0 And txbh = @txbh And krlxcode = 'WX' And bz = @bz                                        
            Select  @yddh=IsNull(@yddh, 0)                                        
            If @yddh > 0 And @pay = 1
                Begin                         
                    Update  xx_skyddb
                    Set     ydzt='GT'
                    Where   yddh = @yddh                                  
                                  
                    Select  @maxxh=Max(xh)
                    From    SY_KRXFMXB
                    Where   zh = @yddh And zhlb = 'Y'                                         
                    Select  @maxxh=IsNull(@maxxh, 0)                                            
                    Select  @maxxh=@maxxh+1                                           
                                        
                    Insert  SY_KRXFMXB ( zh, zhlb, zt, fjzh, xh, rq, sjrq, bc, xfzh, xfzhlb, fkcode, fymcode, desc0, jdflag, je, zsje, xyxe, jzflag, zzflag, czflag, ysflag, czygh, fh, otaddh, xfdh )
                    Values  ( @yddh, 'Y', 'A', 0, @maxxh, @xttoday, @sjrq, @bc, @yddh, 'Y', @fkcode, '##', @fkdesc, '1', @zfj, @zfj, @zfj, 'F', 'F', 'F', 'Y', '999999', '', @ordernum, Substring(@krname, 1, 20) )                                       
                End                                    
                                        
            Commit Tran                                             
            Return
        ");
        DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteBySql(sql);
    }

    /// <summary>
    /// 更新续住存储过程
    /// </summary>
    /// <param name="adminHotelId"></param>
    /// <param name="hotelId"></param>
    private static void set_proc_wxxzcl(string adminHotelId, string hotelId)
    {
        StringBuilder sb_del = new StringBuilder();
        sb_del.Append("Drop Procedure proc_wxxzcl");
        DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteBySql(sb_del);

        StringBuilder sql = new StringBuilder();
        sql.Append(@"
        Create Proc proc_wxxzcl
            @fh Varchar(6) ,
            @fjzh Numeric(10, 0) ,
            @ordernum Varchar(200) ,
            @zdts Smallint ,
            @zfj Money ,
            @pay Smallint ,
            @paytype Smallint ,
            @clzt Char(1) Output
        As
            Begin Tran TRA_wxxzcl                    
            Declare @xttoday DateTime ,
                @sjrq DateTime       
            Declare @bc Char(1) ,
                @maxxh Int ,
                @bz Varchar(200) ,
                @fkcode Varchar(4) ,
                @fkdesc Varchar(50)              
            Select  @xttoday=DateAdd(Day, DateDiff(Day, GetDate(), xtdate), GetDate()), @sjrq=GetDate()
            From    CS_QTXTZTB                        
            Select  @bc=Min(bc)
            From    CS_BCXXB
            Where   bj = 'F'                        
            Select  @clzt='F'    
            Update  XX_KFDJXXB
            Set     ldrq=DateAdd(Day, @zdts, ldrq), zdts=zdts+@zdts
            Where   fjzh = @fjzh    
    
            Update  XX_KRZLXXB
            Set     ldrq=DateAdd(Day, @zdts, ldrq), zdts=zdts+@zdts
            Where   fjzh = @fjzh And rzcode = 'I'    
            Select  @bz=Case @paytype
                          When 1 Then '前台支付'
                          When 2 Then '积分兑换'
                          When 3 Then '会员卡支付'
                          When 4 Then '公众号支付'
                          When 5 Then '优惠券抵扣'
                          When 6 Then '套票抵扣'
                        End           
            Select  @fkcode=Case @paytype
                              When 1 Then ''
                              When 2 Then 'JF'
                              When 3 Then 'MC'
                              When 4 Then 'GZH'
                              When 5 Then 'YHQ'
                              When 6 Then 'TP'
                            End         
  
            Select  @fkdesc=Case @paytype
                              When 1 Then ''
                              When 2 Then '积分兑换'
                              When 3 Then '会员卡支付(公众号扣款)'
                              When 4 Then '公众号支付'
                              When 5 Then '优惠券抵扣'
                              When 6 Then '套票抵扣'
                            End           

            If @pay = 1
                Begin   
            
                    Select  @maxxh=Max(xh)
                    From    SY_KRXFMXB
                    Where   zh = @fjzh And zhlb = 'F'                     
                    Select  @maxxh=IsNull(@maxxh, 0)                        
                    Select  @maxxh=@maxxh+1                       
                    
                    Insert  SY_KRXFMXB ( zh, zhlb, zt, fjzh, xh, rq, sjrq, bc, xfzh, xfzhlb, fkcode, fymcode, desc0, jdflag, je, zsje, xyxe, jzflag, zzflag, czflag, ysflag, czygh, fh, otaddh, bz )
                    Values  ( @fjzh, 'F', 'A', 0, @maxxh, @xttoday, @sjrq, @bc, @fjzh, 'F', @fkcode, '##', @fkdesc, '1', @zfj, @zfj, @zfj, 'F', 'F', 'F', 'N', '999999', @fh, @ordernum, '续住' )      
                End              
            Select  @clzt='T'    
            
            Commit Tran                         
            Return 
        ");
        DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteBySql(sql);
    }

    #endregion

    #region ** 生成每日房价 **

    /// <summary>
    /// 生成预定每日房价 - 国光
    /// </summary>
    /// <param name="adminHotelId"></param>
    /// <param name="hotelId"></param>
    /// <param name="orderNumber"></param>
    /// <param name="payType"></param>
    /// <param name="yddh"></param>
    /// <param name="yhje"></param>
    public static void SetDayPrice(string adminHotelId, string hotelId, string orderNumber, string payType, string yddh, string yhje)
    {
        double yhMoney = CommonHelper.GetDouble(yhje);
        StringBuilder sb = new StringBuilder();
        sb.Append("Select * From ReservationPriceDetail Where OrderNum = @OrderNum ");
        SqlParam[] parm = new SqlParam[] {
                new SqlParam("@OrderNum", orderNumber)
        };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime rq = CommonHelper.GetDateTime(dt.Rows[i]["DateRange"].ToString());
                double fj = CommonHelper.GetDouble(dt.Rows[i]["Price"].ToString());
                string bz = "";
                switch (payType)
                {
                    case "1":
                        if (i == 0 && yhMoney > 0)
                        {
                            fj = fj - yhMoney;
                            bz = "优惠券抵扣" + yhMoney;
                        }
                        break;
                    case "2":
                        fj = 0;
                        bz = "积分兑换";
                        break;
                    case "3":
                        if (i == 0 && yhMoney > 0)
                        {
                            fj = fj - yhMoney;
                        }
                        bz = "会员卡支付";
                        break;
                    case "4":
                        if (i == 0 && yhMoney > 0)
                        {
                            fj = fj - yhMoney;
                            bz = "优惠券抵扣" + yhMoney;
                        }
                        break;
                    case "5":
                        fj = 0;
                        bz = "免费入住券";
                        break;
                    case "6":
                        fj = 0;
                        bz = "预售券支付";
                        break;
                    default:
                        break;
                }
                if (fj < 0)
                {
                    fj = 0;
                }
                Hashtable ht_mrfj = new Hashtable();
                ht_mrfj["yddh"] = yddh;//预订单号
                ht_mrfj["rq"] = rq;//日期
                ht_mrfj["fj"] = fj;//房价
                ht_mrfj["bz"] = bz;//备注
                DataFactory.SqlDataBase(adminHotelId, hotelId).InsertByHashtable("XX_yddhrqfj", ht_mrfj);
            }
        }
    }

    /// <summary>
    /// 生成续住每日房价 - 国光
    /// </summary>
    /// <param name="adminHotelId"></param>
    /// <param name="hotelId"></param>
    /// <param name="orderNumber"></param>
    /// <param name="payType"></param>
    /// <param name="yddh"></param>
    /// <param name="yhje"></param>
    public static void XzDayPrice(string adminHotelId, string hotelId, string orderNumber, string payType, string fjzh, string yhje)
    {
        double yhMoney = CommonHelper.GetDouble(yhje);
        StringBuilder sb = new StringBuilder();
        sb.Append("Select * From ReservationPriceDetail Where OrderNum = @OrderNum ");
        SqlParam[] parm = new SqlParam[] {
                new SqlParam("@OrderNum", orderNumber)
        };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime rq = CommonHelper.GetDateTime(dt.Rows[i]["DateRange"].ToString());
                double fj = CommonHelper.GetDouble(dt.Rows[i]["Price"].ToString());
                string bz = "";
                switch (payType)
                {
                    case "1":
                        if (yhMoney > 0)
                        {
                            fj = fj - yhMoney;
                            bz = "优惠券抵扣" + yhMoney;
                        }
                        break;
                    case "2":
                        fj = 0;
                        bz = "积分兑换";
                        break;
                    case "3":
                        if (yhMoney > 0)
                        {
                            fj = fj - yhMoney;
                        }
                        bz = "会员卡支付";
                        break;
                    case "4":
                        if (yhMoney > 0)
                        {
                            fj = fj - yhMoney;
                            bz = "优惠券抵扣" + yhMoney;
                        }
                        break;
                    case "5":
                        fj = 0;
                        bz = "免费入住券";
                        break;
                    case "6":
                        fj = 0;
                        bz = "预售券支付";
                        break;
                    default:
                        break;
                }
                if (fj < 0)
                {
                    fj = 0;
                }
                Hashtable ht_mrfj = new Hashtable();
                ht_mrfj["fjzh"] = fjzh;//房间账号
                ht_mrfj["rq"] = rq;//日期
                ht_mrfj["fj"] = fj;//房价
                ht_mrfj["bz"] = bz;//备注
                DataFactory.SqlDataBase(adminHotelId, hotelId).InsertByHashtable("XX_FJZHRQFJ", ht_mrfj);
            }
        }
    }

    #endregion

    #region DataTable - JsonData
    /// <summary>
    /// DataTable - JsonData
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static JsonData DataTableToJsonData(DataTable dt)
    {
        JsonData jdData = new JsonData();//DataTable值     
        foreach (DataRow dr in dt.Rows)
        {
            JsonData jdList = new JsonData();
            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (dr[dc] != null && dr[dc] != DBNull.Value && dr[dc].ToString() != "")
                    jdList[dc.ColumnName] = dr[dc].ToString();//    
                else
                    jdList[dc.ColumnName] = "";//    
            }
            jdData.Add(jdList);
        }
        return jdData;
    }
    #endregion
}
