using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;
using RM.Common.DotNetCode;
using RM.Common.DotNetEncrypt;
using RM.Busines;
using System.Text;
using System.Data;

namespace PMS.api
{
    public class Statistics
    {
        #region  ** 1.营业数据 **

        public void BusinessData(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string date = CommonUse.ParamVal(jd_data, "Date", "日期", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);
            string sign_new = Md5Helper.MD5(HotelCode + date, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            int DataType = CommonHelper.GetInt(CommonUse.ParamVal(jd_data, "DataType", "数据类型", false, false));
            #endregion


            DateTime startDate = CommonHelper.GetDateTime(date);
            DateTime endDate = startDate.AddDays(1);
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
                throw new PMSException("数据库连接失败！");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT  rq ,
                    lb ,
                    xmcode ,
                    xmname ,
                    jrfs ,
                    bylj ,
                    sytq ,
                    bnlj ,
                    sntq
            FROM    tj_tjzhtjb
            WHERE   rq >= @startDate
                    AND rq < @endDate
            ");

            JsonData jdData = new JsonData();//DataTable值
            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@startDate", startDate));
            lParam.Add(new SqlParam("@endDate", endDate));
            if (DataType > 1)
            {
                sql.Append(" AND jdid = @jdid ");
                lParam.Add(new SqlParam("@jdid", DataType));
            }
            sql.Append(" ORDER BY lb ASC ");

            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["ItemName"] = dt.Rows[i]["xmname"].ToString();//项目名称
                    jdList["ThisDay"] = dt.Rows[i]["jrfs"].ToString();//日累计
                    jdList["ThisMmonth"] = dt.Rows[i]["bylj"].ToString();//本月累计
                    jdList["LastMonth"] = Convert.ToDouble(dt.Rows[i]["sytq"]).ToString("0.00");//上月累计
                    jdList["ThisYear"] = dt.Rows[i]["bnlj"].ToString();//本年累计
                    jdList["LastYearDate"] = dt.Rows[i]["sntq"].ToString();//上年同期
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到相关信息!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 2.营收日报表 **

        public void DailyReport(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string date = CommonUse.ParamVal(jd_data, "Date", "日期", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);

            string sign_new = Md5Helper.MD5(HotelCode + date, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            int DataType = CommonHelper.GetInt(CommonUse.ParamVal(jd_data, "DataType", "数据类型", false, false));
            #endregion


            DateTime startDate = CommonHelper.GetDateTime(date);
            DateTime endDate = startDate.AddDays(1);
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
                throw new PMSException("数据库连接失败！");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT  rq ,
                    xmlb ,
                    xmbh ,
                    xmname ,
                    bs ,
                    xfje ,
                    jsje ,
                    ye
            FROM    view_yysrrbb
            WHERE   rq >= @startDate
                    AND rq < @endDate
            ");

            JsonData jdData = new JsonData();//DataTable值
            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@startDate", startDate));
            lParam.Add(new SqlParam("@endDate", endDate));
            if (DataType > 1)
            {
                sql.Append(" AND jdid = @jdid ");
                lParam.Add(new SqlParam("@jdid", DataType));
            }
            sql.Append(" ORDER BY xmlb ASC ");

            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["ItemName"] = dt.Rows[i]["xmname"].ToString();//项目名称
                    jdList["TransactionNumber"] = Convert.ToDouble(dt.Rows[i]["bs"]);//笔数
                    jdList["ConsumeMoney"] = Convert.ToDouble(dt.Rows[i]["xfje"]);//消费金额
                    jdList["SettleMoney"] = Convert.ToDouble(dt.Rows[i]["jsje"]);//结算金额
                    jdList["SurplusMoney"] = Convert.ToDouble(dt.Rows[i]["ye"]);//余额
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到相关信息!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 3.营收结算表 **

        public void SettleReport(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string date = CommonUse.ParamVal(jd_data, "Date", "日期", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);

            string sign_new = Md5Helper.MD5(HotelCode + date, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            int DataType = CommonHelper.GetInt(CommonUse.ParamVal(jd_data, "DataType", "数据类型", false, false));
            #endregion


            DateTime startDate = CommonHelper.GetDateTime(date);
            DateTime endDate = startDate.AddDays(1);
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
                throw new PMSException("数据库连接失败！");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT  jdid ,
                    xmdesc ,
                    je
            FROM    [view_yyjssrb]
            WHERE   rq >= @startDate
                    AND rq < @endDate
            ");

            JsonData jdData = new JsonData();//DataTable值
            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@startDate", startDate));
            lParam.Add(new SqlParam("@endDate", endDate));
            if (DataType > 1)
            {
                sql.Append(" AND jdid = @jdid ");
                lParam.Add(new SqlParam("@jdid", DataType));
            }
            //sql.Append(" ORDER BY xmlb ASC ");

            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["ItemName"] = dt.Rows[i]["xmdesc"].ToString();//项目名称
                    jdList["SurplusMoney"] = Convert.ToDouble(dt.Rows[i]["je"]);//余额
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到相关信息!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion


        #region  ** 35.房态视图表 **

        public void GetRoomStateView(HttpContext context, string jsonDate)
        {

            #region  ** 解码酒店编号 **

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

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from XX_SSFTB where 1=1 ");

            IList<SqlParam> lParam = new List<SqlParam>();
            if (DataType > 1)
            {
                sql.Append(" AND jdid = @jdid ");
                lParam.Add(new SqlParam("@jdid", DataType));
            }
            sql.Append(" order by fh asc ");

            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                string GuestNum = dt.Rows.Count.ToString();
                JsonData jdData = new JsonData();//DataTable值
                JsonData jdTotalData = new JsonData();//DataTable值
                int ZS = 0;
                int OC = 0;
                int VC = 0;
                int VD = 0;
                int BC = 0;
                int OOO = 0;
                int LR = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ZS++;
                    JsonData jdList = new JsonData();
                    string StateStyle = "";
                    string ftcode = dt.Rows[i]["fjzt"].ToString().Trim();
                    if (ftcode == "OC" || ftcode == "OD")//在住房
                    {
                        OC++;
                        StateStyle = "zaizhu";
                    }
                    //else if (ftcode == "OD")//在住脏房
                    //{
                    //    ftname = "在住脏房";
                    //    StateStyle = "zhuzang";
                    //}
                    else if (ftcode == "VD")//空脏房
                    {
                        VD++;
                        StateStyle = "kongzang";
                    }
                    else if (ftcode == "VC")//空洁房
                    {
                        VC++;
                        StateStyle = "kongjie";
                    }
                    else if (ftcode == "BC" || ftcode == "BD" || ftcode == "CMD" || ftcode == "CMC")//预抵净房
                    {
                        BC++;
                        StateStyle = "yudijing";
                    }
                    //else if (ftcode == "CMD")//预抵脏房
                    //{
                    //    StateStyle = "yudizang";
                    //}
                    //else if (ftcode == "BD")//预抵脏房
                    //{
                    //    StateStyle = "yudizang";
                    //}
                    //else if (ftcode == "CMC")//预抵净房
                    //{
                    //    StateStyle = "yudijing";
                    //}
                    else if (ftcode == "OOO" || ftcode == "OOS")//维修房
                    {
                        OOO++;
                        StateStyle = "weixiu";
                    }
                    //else if (ftcode == "OOS")//维护房
                    //{
                    //    StateStyle = "weihu";
                    //}
                    else if (ftcode == "GO")//预离房
                    {
                        StateStyle = "yuli";
                    }
                    else if (ftcode == "LR" || ftcode == "LC")//锁房
                    {
                        LR++;
                        StateStyle = "suo";
                    }
                    //else if (ftcode == "LC")//锁房
                    //{
                    //    StateStyle = "suo";
                    //}

                    jdList["RoomNumber"] = dt.Rows[i]["fh"].ToString();//房间号
                    jdList["StateStyle"] = StateStyle;//房间样式
                    jdList["RoomState"] = ftcode; //房间状态代码
                    jdData.Add(jdList);
                }
                JsonData jdhj = new JsonData();
                jdhj["RoomState"] = "ZS";//房间状态
                jdhj["RoomNumber"] = ZS; //房态数量
                jdTotalData.Add(jdhj);
                //
                jdhj = new JsonData();
                jdhj["RoomState"] = "OC";//房间状态
                jdhj["RoomNumber"] = OC; //房态数量
                jdTotalData.Add(jdhj);
                //
                jdhj = new JsonData();
                jdhj["RoomState"] = "VC";//房间状态
                jdhj["RoomNumber"] = VC; //房态数量
                jdTotalData.Add(jdhj);
                //
                jdhj = new JsonData();
                jdhj["RoomState"] = "VD";//房间状态
                jdhj["RoomNumber"] = VD; //房态数量
                jdTotalData.Add(jdhj);
                //
                jdhj = new JsonData();
                jdhj["RoomState"] = "BC";//房间状态
                jdhj["RoomNumber"] = BC; //房态数量
                jdTotalData.Add(jdhj);
                //
                jdhj = new JsonData();
                jdhj["RoomState"] = "OOO";//房间状态
                jdhj["RoomNumber"] = OOO; //房态数量
                jdTotalData.Add(jdhj);
                //
                jdhj = new JsonData();
                jdhj["RoomState"] = "LR";//房间状态
                jdhj["RoomNumber"] = LR; //房态数量
                jdTotalData.Add(jdhj);

                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
                jd["TotalData"] = jdTotalData.IsArray ? jdTotalData : false;
                int or = Convert.ToInt32((Convert.ToDouble(OC) / Convert.ToDouble(ZS)) * 1000);
                jd["OccupancyRate"] = or / 10;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到相关信息!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

    }
}
