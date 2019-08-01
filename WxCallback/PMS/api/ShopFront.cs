using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;
using PMS;
using RM.Common.DotNetEncrypt;
using RM.Busines;
using RM.Common.DotNetCode;
using System.Text;
using System.Data;

namespace PMS.api
{
    /// <summary>
    /// 门店
    /// </summary>
    public class ShopFront
    {

        #region  ** 1. 实情 **

        public void Truth(HttpContext context, string jsonDate)
        {

            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }

            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);
            string sign_new = Md5Helper.MD5(HotelCode, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }
            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            string date = CommonUse.ParamVal(jd_data, "Date", "时间", true, true);
            string type = CommonUse.ParamVal(jd_data, "Type", "时间类型", true, true);
            string sd = CommonUse.ParamVal(jd_data, "StartDate", "开始日期", false, false);
            string ed = CommonUse.ParamVal(jd_data, "EndDate", "结束日期", false, false);
            int DataType = CommonHelper.GetInt(CommonUse.ParamVal(jd_data, "DataType", "数据类型", false, false));

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];
            if (DataType > 1)//数据类型不为0表示为数据查询数据库读取
            {
                adminHotelId = "1010142";
                hotelId = "0";
            }
            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            int SalesMoney = 0;
            int SalesAccumulate = 0;
            int SaleNight = 0;
            int RentalRate = 0;
            int AveragePrice = 0;
            int RevPAR = 0;

            int StayIn = 0;
            int GetRoom = 0;
            int RetreatRoom = 0;
            int Arrivals = 0;
            int Repair = 0;
            int EstimateMoney = 0;

            DateTime StartDate = Convert.ToDateTime("2010-01-01");
            DateTime EndDate = DateTime.Now.AddDays(1);

            DateTime sdAccumulate = Convert.ToDateTime("2010-01-01");
            DateTime edAccumulate = DateTime.Now.AddDays(1);

            switch (type)
            {
                case "1":
                    StartDate = Convert.ToDateTime(date);
                    EndDate = StartDate.AddDays(1);

                    sdAccumulate = Convert.ToDateTime(StartDate.ToString("yyyy-MM-01"));
                    edAccumulate = Convert.ToDateTime(StartDate.AddMonths(1).ToString("yyyy-MM-01"));
                    break;
                case "2":
                    StartDate = Convert.ToDateTime(date + "01日");
                    EndDate = StartDate.AddMonths(1);
                    sdAccumulate = Convert.ToDateTime(StartDate.ToString("yyyy-01-01"));
                    edAccumulate = Convert.ToDateTime(StartDate.AddYears(1).ToString("yyyy-01-01"));
                    break;
                case "3":
                    StartDate = Convert.ToDateTime(date + "01月01日");
                    EndDate = StartDate.AddYears(1);
                    break;
                case "4":
                    try
                    {
                        sdAccumulate = StartDate = Convert.ToDateTime(sd);
                        edAccumulate = EndDate = Convert.ToDateTime(ed).AddDays(1);
                    }
                    catch
                    {
                        throw new PMSException("自定义时间格式错误！");
                    }
                    break;
                default:
                    break;
            }
            #region ** 获取数据值 **
            try
            {
                DataTable dt = GetSales(DataType, adminHotelId, hotelId, StartDate, EndDate);
                DataTable dtAcc = GetSales(DataType, adminHotelId, hotelId, sdAccumulate, edAccumulate);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int days = CommonHelper.GetInt(dt.Rows[0]["dayNum"].ToString());
                    SalesMoney = CommonHelper.GetInt(dt.Rows[0]["yysr"].ToString());
                    SalesAccumulate = CommonHelper.GetInt(dtAcc.Rows[0]["yysr"].ToString());
                    SaleNight = CommonHelper.GetInt(dt.Rows[0]["fws"].ToString());
                    RentalRate = CommonHelper.GetInt(dt.Rows[0]["czl"].ToString()) / days;
                    AveragePrice = CommonHelper.GetInt(dt.Rows[0]["pjfj"].ToString()) / days;
                    RevPAR = CommonHelper.GetInt(dt.Rows[0]["repar"].ToString()) / days;
                }

                DataTable dt_day = GetDayNumber(DataType, adminHotelId, hotelId, StartDate, EndDate);
                if (dt_day != null && dt_day.Rows.Count > 0)
                {
                    StayIn = CommonHelper.GetInt(dt_day.Rows[0]["dqzzs"].ToString());
                    GetRoom = CommonHelper.GetInt(dt_day.Rows[0]["jrkfs"].ToString());
                    RetreatRoom = CommonHelper.GetInt(dt_day.Rows[0]["jrtfs"].ToString());
                    Arrivals = CommonHelper.GetInt(dt_day.Rows[0]["jryd"].ToString());
                    Repair = CommonHelper.GetInt(dt_day.Rows[0]["jrwx"].ToString());
                    EstimateMoney = CommonHelper.GetInt(dt_day.Rows[0]["yjjrsr"].ToString());
                }
            }
            catch
            {

            }
            #endregion

            JsonData jd = new JsonData();//返回JsonData
            JsonData jdData = new JsonData();//DataTable值

            jdData["SalesMoney"] = SalesMoney;     //营收
            jdData["SalesAccumulate"] = SalesAccumulate;//累计
            jdData["SaleNight"] = SaleNight;      //房晚数
            jdData["RentalRate"] = RentalRate;     //出租率
            jdData["AveragePrice"] = AveragePrice;   //平均房价
            jdData["RevPAR"] = RevPAR;         //RevPAR

            jdData["StayIn"] = StayIn;         //在住数
            jdData["GetRoom"] = GetRoom;        //开房
            jdData["RetreatRoom"] = RetreatRoom;    //退房
            jdData["Arrivals"] = Arrivals;       //预抵
            jdData["Repair"] = Repair;         //维修房
            jdData["EstimateMoney"] = EstimateMoney;  //预计房租收入

            jd["code"] = 1;
            jd["data"] = jdData;

            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        private DataTable GetSales(int DataType, string adminHotelId, string hotelId, DateTime StartDate, DateTime EndDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT  SUM(yysr) yysr ,
                    SUM(fws) fws ,
                    SUM(czl) czl ,
                    SUM(pjfj) pjfj ,
                    SUM(repar) repar ,
                    COUNT(1) dayNum 
            FROM    TJ_RQYYFX
            WHERE   rq >= @StartDate
                    AND rq < @EndDate
            ");

            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@StartDate", StartDate.ToString("yyyy-MM-dd")));
            lParam.Add(new SqlParam("@EndDate", EndDate.ToString("yyyy-MM-dd")));
            if (DataType > 1)
            {
                sql.Append(" AND jdid = @jdid ");
                lParam.Add(new SqlParam("@jdid", DataType));
            }
            return DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());
        }

        private DataTable GetDayNumber(int DataType, string adminHotelId, string hotelId, DateTime StartDate, DateTime EndDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT  *
            FROM    TJ_DQZDXX
            ");
            IList<SqlParam> lParam = new List<SqlParam>();
            if (DataType > 1)
            {
                sql.Append(" WHERE jdid = @jdid ");
                lParam.Add(new SqlParam("@jdid", DataType));
            }
            return DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql);
        }

        #endregion

        #region  ** 1. 实情 营收曲线图 **

        public void RevenueMonth(HttpContext context, string jsonDate)
        {

            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(context.Server.UrlDecode(jsonDate));
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);
            string sign_new = Md5Helper.MD5(HotelCode, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }
            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            string date = CommonUse.ParamVal(jd_data, "Date", "时间", true, true);
            string type = CommonUse.ParamVal(jd_data, "Type", "时间类型", true, true);

            int DataType = CommonHelper.GetInt(CommonUse.ParamVal(jd_data, "DataType", "数据类型", false, false));

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];
            if (DataType > 1)//数据类型不为0表示为数据查询数据库读取
            {
                adminHotelId = "1010142";
                hotelId = "0";
            }
            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }
            DateTime StartDate = Convert.ToDateTime("2010-01-01");
            DateTime EndDate = DateTime.Now.AddDays(1);
            string json = "";
            switch (type)
            {
                case "1":
                    StartDate = Convert.ToDateTime(date + "01日");
                    EndDate = StartDate.AddMonths(1);
                    json = MonthData(adminHotelId, hotelId, StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), DataType);
                    break;
                case "2":
                    StartDate = Convert.ToDateTime(date + "01月01日");
                    EndDate = StartDate.AddYears(1);
                    json = YearData(adminHotelId, hotelId, StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), DataType);
                    break;
                case "3":
                    json = AllData(adminHotelId, hotelId, DataType);
                    break;
                default:
                    break;
            }
            if (json == "")
            {
                JsonData jd = new JsonData();//返回JsonData
                jd["code"] = 0;
                jd["message"] = "没有找到您要找的数据！";
                json = jd.ToJson();
            }
            // 统一输出
            PMSResponse.WirterString(json);
        }

        private string MonthData(string adminHotelId, string hotelId, string StartDate, string EndDate, int DataType)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT  rq,yysr 
            FROM    TJ_RQYYFX
            WHERE   rq >= @StartDate
                    AND rq < @EndDate            
            ");
            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@StartDate", StartDate));
            lParam.Add(new SqlParam("@EndDate", EndDate));
            if (DataType > 1)
            {
                sql.Append(" AND jdid = @jdid ");
                lParam.Add(new SqlParam("@jdid", DataType));
            }
            sql.Append(" ORDER BY rq ASC ");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            JsonData jdData = new JsonData();//DataTable值
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdList = new JsonData();
                JsonData rqs = new JsonData();
                JsonData vals = new JsonData();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string rq = Convert.ToDateTime(dt.Rows[i]["rq"]).ToString("dd");
                    double val = CommonHelper.GetDouble(dt.Rows[i]["yysr"].ToString());
                    rqs.Add(rq);
                    vals.Add(val);
                }
                jdList["Date"] = rqs;
                jdList["Value"] = vals;
                jdData.Add(jdList);
            }
            jd["code"] = 1;
            jd["data"] = jdData.IsArray ? jdData : false;
            return jd.ToJson();
        }

        private string YearData(string adminHotelId, string hotelId, string StartDate, string EndDate, int DataType)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            Select  IsNull(Sum(Case Month(rq) When '1' Then yysr Else 0 End), 0) As '01',
					IsNull(Sum(Case Month(rq) When '2' Then yysr Else 0 End), 0) As '02',
					IsNull(Sum(Case Month(rq) When '3' Then yysr Else 0 End), 0) As '03',
					IsNull(Sum(Case Month(rq) When '4' Then yysr Else 0 End), 0) As '04',
					IsNull(Sum(Case Month(rq) When '5' Then yysr Else 0 End), 0) As '05',
					IsNull(Sum(Case Month(rq) When '6' Then yysr Else 0 End), 0) As '06',
					IsNull(Sum(Case Month(rq) When '7' Then yysr Else 0 End), 0) As '07',
					IsNull(Sum(Case Month(rq) When '8' Then yysr Else 0 End), 0) As '08',
					IsNull(Sum(Case Month(rq) When '9' Then yysr Else 0 End), 0) As '09',
					IsNull(Sum(Case Month(rq) When '10' Then yysr Else 0 End), 0) As '10',
					IsNull(Sum(Case Month(rq) When '11' Then yysr Else 0 End), 0) As '11',
					IsNull(Sum(Case Month(rq) When '12' Then yysr Else 0 End), 0) As '12'
            From    TJ_RQYYFX
            WHERE   rq >= @StartDate
                    AND rq < @EndDate            
            ");
            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@StartDate", StartDate));
            lParam.Add(new SqlParam("@EndDate", EndDate));
            if (DataType > 1)
            {
                sql.Append(" AND jdid = @jdid ");
                lParam.Add(new SqlParam("@jdid", DataType));
            }
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            JsonData jdData = new JsonData();//DataTable值
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdList = new JsonData();
                JsonData rqs = new JsonData();
                JsonData vals = new JsonData();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string rq = dt.Columns[i].ToString();
                    double val = CommonHelper.GetDouble(dt.Rows[0][rq].ToString());
                    rqs.Add(rq);
                    vals.Add(val);
                }
                jdList["Date"] = rqs;
                jdList["Value"] = vals;
                jdData.Add(jdList);
            }
            jd["code"] = 1;
            jd["data"] = jdData.IsArray ? jdData : false;
            return jd.ToJson();
        }

        private string AllData(string adminHotelId, string hotelId, int DataType)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select Top 1 rq From dbo.TJ_RQYYFX ");
            IList<SqlParam> lParam = new List<SqlParam>();
            if (DataType > 1)
            {
                sql.Append(" Where jdid = @jdid ");
                lParam.Add(new SqlParam("@jdid", DataType));
            }
            sql.Append(" Order By rq ");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());
            string ksrq = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                ksrq = dt.Rows[0]["rq"].ToString();
            }
            int StartYear = CommonHelper.GetDateTime(ksrq).Year;
            int EndYear = DateTime.Now.Year;
            sql = new StringBuilder();
            sql.Append("Select ");
            for (int i = StartYear; i <= EndYear; i++)
            {
                sql.AppendFormat(" IsNull(Sum(Case Year(rq) When '{0}' Then yysr Else 0 End), 0) As '{0}' ", i);
                if (i < EndYear)
                {
                    sql.Append(" , ");
                }
            }
            sql.Append(" FROM TJ_RQYYFX ");
            dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());

            JsonData jd = new JsonData();//返回JsonData
            JsonData jdData = new JsonData();//DataTable值
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdList = new JsonData();
                JsonData rqs = new JsonData();
                JsonData vals = new JsonData();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string rq = dt.Columns[i].ToString();
                    double val = CommonHelper.GetDouble(dt.Rows[0][rq].ToString());
                    rqs.Add(rq);
                    vals.Add(val);
                }
                jdList["Date"] = rqs;
                jdList["Value"] = vals;
                jdData.Add(jdList);
            }
            jd["code"] = 1;
            jd["data"] = jdData.IsArray ? jdData : false;
            return jd.ToJson();
        }

        #endregion

        #region  ** 2. 监管 **

        public void Supervise(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);
            string sign_new = Md5Helper.MD5(HotelCode, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }
            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            int pageIndex = CommonHelper.GetInt(CommonUse.ParamVal(jd_data, "PageIndex", "当前页", true, true));
            int DataType = CommonHelper.GetInt(CommonUse.ParamVal(jd_data, "DataType", "数据类型", false, false));
            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];
            if (DataType > 1)//数据类型不为0表示为数据查询数据库读取
            {
                adminHotelId = "1010142";
                hotelId = "0";
            }
            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            int PageSize = 20;
            int doCount = 0;
            int more = 0;
            JsonData jd = new JsonData();//返回JsonData
            JsonData jdData = new JsonData();//DataTable值
            #region ** 获取数据值 **
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"
                SELECT  rq ,
                        jglb ,
                        xh ,
                        jglr
                FROM    XX_JGXJCX
                ");
                IList<SqlParam> lParam = new List<SqlParam>();
                if (DataType > 1)
                {
                    sql.Append(" WHERE jdid = @jdid ");
                    lParam.Add(new SqlParam("@jdid", DataType));
                }
                sql.Append(" ORDER BY rq DESC ");
                DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    doCount = dt.Rows.Count;
                    int pi_ps = (pageIndex - 1) * PageSize;
                    int pi_dc = pi_ps + PageSize;
                    if (pi_dc > doCount)
                    {
                        pi_dc = doCount;
                    }
                    more = doCount - pi_dc;//更多剩余数
                    more = more > 0 ? more : 0;
                    if (pi_dc > doCount)
                    {
                        pi_dc = doCount;
                    }
                    for (int i = pi_ps; i < pi_dc; i++)
                    {
                        JsonData jdList = new JsonData();//
                        jdList["Time"] = dt.Rows[i]["rq"].ToString();//时间
                        jdList["Type"] = dt.Rows[i]["jglb"].ToString(); ;//类型
                        jdList["Operator"] = "00" + dt.Rows[i]["xh"].ToString();//操作人
                        jdList["Content"] = dt.Rows[i]["jglr"].ToString();//操作内容
                        jdData.Add(jdList);
                    }
                }
            }
            catch
            {

            }
            #endregion

            jd["code"] = 1;
            jd["count"] = doCount;//数量
            jd["more"] = more;//更多剩余数
            jd["data"] = jdData.IsArray ? jdData : false;

            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion
    }
}