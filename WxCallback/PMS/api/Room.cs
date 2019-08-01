using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using RM.Busines;
using RM.Common.DotNetCode;
using System.Web.Script.Serialization;
using RM.Common.DotNetEncrypt;
using LitJson;
using System.Collections;
using RM.Busines.IDAO;
using RM.Busines.DAL;

namespace PMS.api
{
    public class Room
    {
        private void CheckData(JsonData jd_data, string key, string msg)
        {
            try
            {

                if (jd_data[key] == null)
                {
                    throw new PMSException(msg + "[" + key + "]必传");
                }
            }
            catch (Exception)
            {
                throw new PMSException(msg + "[" + key + "]必传");
            }
        }

        #region  ** 获取所有房价码 **

        public void GetRoomPriceCode(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT plan0 RoomPriceCode,desc0 RoomPriceName FROM CS_FJBMB");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["RoomPriceCode"] = dt.Rows[i]["RoomPriceCode"].ToString();
                    jdList["RoomPriceName"] = dt.Rows[i]["RoomPriceName"].ToString();
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到房价码!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }
        #endregion

        #region  ** 1.	获取酒店房间类型接口 **

        public void GetRoomType(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            CheckData(jd_data, "RoomPriceCode", "房价码");//必传判断
            string RoomPriceCode = jd_data["RoomPriceCode"].ToString();//房价码
            if (string.IsNullOrEmpty(RoomPriceCode))
                throw new PMSException("房价码不能为空");

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT  b.desc0 RoomName,
                    a.kfcode RoomCode,
                    fj1 RoomPrice
            FROM    CS_KFFJB a
                    LEFT JOIN CS_KFLXCSB b ON b.kfcode = a.kfcode
            WHERE   plan0 = @RoomPriceCode
            ");

            SqlParam[] param = new SqlParam[] { 
                    new SqlParam("@RoomPriceCode",RoomPriceCode)
            };
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, param);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["RoomName"] = dt.Rows[i]["RoomName"].ToString();
                    jdList["RoomCode"] = dt.Rows[i]["RoomCode"].ToString();
                    jdList["RoomPrice"] = dt.Rows[i]["RoomPrice"].ToString();
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到该类型!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 2.	获取指定房型下的可用房 **

        public void GetRoomAvailable(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];


            CheckData(jd_data, "CheckInTime", "入住时间");//必传判断
            string CheckInTime = jd_data["CheckInTime"].ToString();//入住时间
            if (string.IsNullOrEmpty(CheckInTime))
                throw new PMSException("入住时间不能为空");

            CheckData(jd_data, "LeaveTIme", "预离时间");//必传判断
            string LeaveTIme = jd_data["LeaveTIme"].ToString();//预离时间
            if (string.IsNullOrEmpty(LeaveTIme))
                throw new PMSException("预离时间不能为空");

            CheckData(jd_data, "RoomCode", "房间类型");//必传判断
            string RoomCode = jd_data["RoomCode"].ToString();//房间类型
            if (string.IsNullOrEmpty(RoomCode))
                throw new PMSException("房间类型不能为空");

            CheckData(jd_data, "RoomState", "房间状态");//必传判断
            string RoomState = jd_data["RoomState"].ToString();//房间状态
            if (string.IsNullOrEmpty(RoomState))
                throw new PMSException("房间状态不能为空");


            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }


            Hashtable ht = new Hashtable();
            ht["strq"] = CheckInTime;
            ht["enrq"] = LeaveTIme;
            ht["kfcode"] = RoomCode;
            ht["ftcode"] = RoomState;
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableProc("[PROC_kyf_fh]", ht);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["RoomNumber"] = dt.Rows[i]["fh"].ToString();//房号
                    jdList["RoomCode"] = dt.Rows[i]["kfcode"].ToString();//房间类型
                    jdList["FoolNumber"] = dt.Rows[i]["lcbm"].ToString();//楼层号
                    jdList["RoomDescribe"] = dt.Rows[i]["fjms"].ToString();//房间描述
                    jdList["DoorLockRoomNum"] = dt.Rows[i]["icfh"].ToString();//门锁房号
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到可用房!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 3.	房费计算 **

        public void GetRoomCharge(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "CheckInTime", "入住时间");//必传判断
            string CheckInTime = jd_data["CheckInTime"].ToString();//房价码
            if (string.IsNullOrEmpty(CheckInTime))
                throw new PMSException("入住时间不能为空");


            CheckData(jd_data, "LeaveTIme", "预离时间");//必传判断
            string LeaveTIme = jd_data["LeaveTIme"].ToString();//客人姓名
            if (string.IsNullOrEmpty(LeaveTIme))
                throw new PMSException("预离时间不能为空");



            CheckData(jd_data, "RoomPriceCode", "房价码");//必传判断
            string RoomPriceCode = jd_data["RoomPriceCode"].ToString();//房价码
            if (string.IsNullOrEmpty(RoomPriceCode))
                throw new PMSException("房价码不能为空");


            CheckData(jd_data, "RoomCode", "房型编码");//必传判断
            string RoomCode = jd_data["RoomCode"].ToString();//客人姓名
            if (string.IsNullOrEmpty(RoomCode))
                throw new PMSException("房型编码不能为空");


            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }
            Hashtable ht = new Hashtable();
            ht["qsrq"] = CheckInTime;
            ht["zzrq"] = LeaveTIme;
            ht["kfcode"] = RoomCode;
            ht["fjb"] = RoomPriceCode;
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableProc("[proc_rqfj]", ht);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["CheckTime"] = dt.Rows[i]["Rq"].ToString();
                    jdList["RoomPrice"] = dt.Rows[i]["Fj"].ToString();
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到该类型!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 4.	获取入住客人信息 **

        public void GetRoomGuest(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "CertificatesNumber", "证件号码");//必传判断
            string CertificatesNumber = jd_data["CertificatesNumber"].ToString();//证件号码
            if (string.IsNullOrEmpty(CertificatesNumber))
                throw new PMSException("证件号码不能为空");

            CheckData(jd_data, "GuestName", "客人姓名");//必传判断
            string GuestName = jd_data["GuestName"].ToString();//客人姓名
            if (string.IsNullOrEmpty(GuestName))
                throw new PMSException("客人姓名不能为空");


            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            Hashtable ht = new Hashtable();
            ht["zjhm"] = CertificatesNumber;
            ht["krname"] = GuestName;
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableProc("[proc_krxx_cx]", ht);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["RoomAccount"] = dt.Rows[i]["fjzh"].ToString();//房间账号
                    jdList["GuestName"] = dt.Rows[i]["Name1"].ToString();//客人姓名
                    jdList["CertificatesNumber"] = dt.Rows[i]["zjhm"].ToString();//证件号码
                    jdList["RoomNumber"] = dt.Rows[i]["fh"].ToString();//房号
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到客人信息!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 5.	锁房 **
        public void GetLockRoom(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "RoomNumber", "房号");//必传判断
            string RoomNumber = jd_data["RoomNumber"].ToString();//房号
            if (string.IsNullOrEmpty(RoomNumber))
                throw new PMSException("房号不能为空");

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }


            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["fh"] = RoomNumber;
            ht["OUT_ret"] = null;
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("PROC_lockfh", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                if (rs["OUT_ret"] != null && rs["OUT_ret"].ToString() != "")
                {
                    JsonData jdList = new JsonData();
                    jdList["ReturnMessage"] = rs["OUT_ret"].ToString();//返回值
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到锁房信息!";
            }

            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 6.	解锁房间 **
        public void GetUnLockRoom(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "RoomNumber", "房号");//必传判断
            string RoomNumber = jd_data["RoomNumber"].ToString();//房号
            if (string.IsNullOrEmpty(RoomNumber))
                throw new PMSException("房号不能为空");

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }


            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["fh"] = RoomNumber;
            ht["OUT_ret"] = null;
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("PROC_unlockfh", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                if (rs["OUT_ret"] != null && rs["OUT_ret"].ToString() != "")
                {
                    JsonData jdList = new JsonData();
                    jdList["ReturnMessage"] = rs["OUT_ret"].ToString();//返回值
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到解锁房信息!";
            }

            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 7.	入住传入住人信息 **
        public void GetRoomCheckInInfo(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];


            string BookOrderNum = "";//预订单号
            if (jd_data["BookOrderNum"] != null)
            {
                BookOrderNum = jd_data["BookOrderNum"].ToString();//预订单号
            }


            string RoomAccount = "";//房间账号
            if (jd_data["RoomAccount"] != null)
            {
                RoomAccount = jd_data["RoomAccount"].ToString();//房间账号
            }



            CheckData(jd_data, "RoomNumber", "房号");//必传判断
            string RoomNumber = jd_data["RoomNumber"].ToString();//手机号码
            if (string.IsNullOrEmpty(RoomNumber))
                throw new PMSException("房号不能为空");



            CheckData(jd_data, "GuestName", "客人姓名");//必传判断
            string GuestName = jd_data["GuestName"].ToString();//客人姓名
            if (string.IsNullOrEmpty(GuestName))
                throw new PMSException("客人姓名不能为空");




            CheckData(jd_data, "Sex", "性别");//必传判断
            string Sex = jd_data["Sex"].ToString();//性别
            if (string.IsNullOrEmpty(Sex))
                throw new PMSException("性别不能为空");


            CheckData(jd_data, "CertificatesType", "证件类型");//必传判断
            string CertificatesType = jd_data["CertificatesType"].ToString();//证件类型
            if (string.IsNullOrEmpty(CertificatesType))
                throw new PMSException("证件类型不能为空");


            CheckData(jd_data, "CertificatesNumber", "证件号码");//必传判断
            string CertificatesNumber = jd_data["CertificatesNumber"].ToString();//证件号码
            if (string.IsNullOrEmpty(CertificatesNumber))
                throw new PMSException("证件号码不能为空");


            CheckData(jd_data, "Phone", "手机号码");//必传判断
            string Phone = jd_data["Phone"].ToString();//手机号码
            if (string.IsNullOrEmpty(Phone))
                throw new PMSException("手机号码不能为空");


            string Address = "";//地址
            if (jd_data["Address"] != null)
            {
                Address = jd_data["Address"].ToString();//地址
            }


            string MemberCardNumber = "";//会员卡号
            if (jd_data["MemberCardNumber"] != null)
            {
                MemberCardNumber = jd_data["MemberCardNumber"].ToString();//会员卡号
            }


            string RoomCode = "";//房价代码
            if (jd_data["RoomCode"] != null)
            {
                RoomCode = jd_data["RoomCode"].ToString();//房价代码
            }

            string RoomPrice = "";//房价
            if (jd_data["RoomPrice"] != null)
            {
                RoomPrice = jd_data["RoomPrice"].ToString();//房价
            }

            string GuestType = "";//客人类型
            if (jd_data["GuestType"] != null)
            {
                GuestType = jd_data["GuestType"].ToString();//客人类型
            }


            string BreakfastNumber = "";//早餐数量
            if (jd_data["BreakfastNumber"] != null)
            {
                BreakfastNumber = jd_data["BreakfastNumber"].ToString();//早餐数量
            }


            string NationalCode = "";//民族代码
            if (jd_data["NationalCode"] != null)
            {
                NationalCode = jd_data["NationalCode"].ToString();//民族代码
            }


            string StayDays = "";//住店天数
            if (jd_data["StayDays"] != null)
            {
                StayDays = jd_data["StayDays"].ToString();//住店天数
            }


            string ResidenceType = "";//住店天数
            if (jd_data["ResidenceType"] != null)
            {
                ResidenceType = jd_data["ResidenceType"].ToString();//住店类型
            }


            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            if (BookOrderNum == "")
            {
                ht["yddh"] = null;//预订单号
            }
            else
            {
                ht["yddh"] = BookOrderNum;//预订单号
            }


            if (RoomAccount == "")
            {
                ht["fjzh_in"] = null;//预订单号
            }
            else
            {
                ht["fjzh_in"] = RoomAccount;//房间账号
            }


            ht["fh"] = RoomNumber;//房号
            ht["krname"] = GuestName;//客人名字
            ht["xb"] = Sex;//性别
            ht["zjcode"] = CertificatesType;//证件类型
            ht["zjhm"] = CertificatesNumber;//证件号码
            ht["sjhm"] = Phone;//手机号码
            ht["dz"] = Address;//地址
            ht["vipkh"] = MemberCardNumber;//会员卡号
            ht["scmcode"] = RoomCode;//房价代码
            ht["fj"] = RoomPrice;//房价
            ht["krlx"] = GuestType;//客人类型
            ht["zcsl"] = BreakfastNumber;//早餐数量
            ht["mzcode"] = NationalCode;//民族代码
            ht["zdts"] = StayDays;//住店天数
            ht["zdlx"] = ResidenceType;//住店类型
            ht["OUT_fjzh_out"] = null;
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("PROC_Checkin", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值

                JsonData jdList = new JsonData();
                if (rs["OUT_fjzh_out"] != null && rs["OUT_fjzh_out"].ToString() != "")
                {
                    jdList["ReturnMessage"] = rs["OUT_fjzh_out"].ToString();//返回值
                }
                jdData.Add(jdList);
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到相关信息 !";
            }

            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 8.	入账 **
        //第一步:查询账务代码
        public void GetAccountCode(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "AccountCode", "账务代码");//必传判断
            string AccountCode = jd_data["AccountCode"].ToString();//账务代码
            if (string.IsNullOrEmpty(AccountCode))
                throw new PMSException("账务代码不能为空");

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            Hashtable ht = new Hashtable();
            ht["code"] = AccountCode;
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableProc("[proc_Account]", ht);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["AccountCode"] = dt.Rows[i]["code"].ToString();//账务代码
                    jdList["AccountName"] = dt.Rows[i]["Desc0"].ToString();//账务名称
                    jdList["AccountType"] = dt.Rows[i]["flag"].ToString();//账务类型(1付款方式 0费用代码)
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

        //第二步：录入账务
        public void GetAddAccount(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "RoomNumber", "房号");//必传判断
            string RoomNumber = jd_data["RoomNumber"].ToString();//房号
            if (string.IsNullOrEmpty(RoomNumber))
                throw new PMSException("房号不能为空");

            CheckData(jd_data, "AccountType", "账务类型");//必传判断
            string AccountType = jd_data["AccountType"].ToString();//账务类型(1付款0费用)
            if (string.IsNullOrEmpty(AccountType))
                throw new PMSException("账务类型不能为空");

            CheckData(jd_data, "AccountCode", "账务类型");//必传判断
            string AccountCode = jd_data["AccountCode"].ToString();//账务类型(1付款0费用)
            if (string.IsNullOrEmpty(AccountCode))
                throw new PMSException("账务代码不能为空");

            CheckData(jd_data, "AccountPrice", "金额");//必传判断
            string AccountPrice = jd_data["AccountPrice"].ToString();//金额
            if (string.IsNullOrEmpty(AccountPrice))
                throw new PMSException("金额不能为空");

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }


            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["fh"] = RoomNumber;//房号
            ht["flag"] = AccountType;//账务类型(1付款0费用)
            ht["code"] = AccountCode;//账务代码
            ht["je"] = AccountPrice;//金额
            ht["OUT_ret"] = null;
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("[PROC_Account_insert]", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                JsonData jdList = new JsonData();
                if (rs["OUT_ret"] != null && rs["OUT_ret"].ToString() != "")
                {
                    jdList["ReturnMessage"] = rs["OUT_ret"].ToString();//返回值
                    jdList["ReturnMessageVal"] = "0";//返回值
                }
                else
                {
                    jdList["ReturnMessage"] = "执行成功";
                    jdList["ReturnMessageVal"] = "1";//返回值
                }

                jdData.Add(jdList);
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

        #region  ** 10.	查询指定房间当前状态 **

        public void GetAppointRoomState(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "RoomNumber", "房号");//必传判断
            string RoomNumber = jd_data["RoomNumber"].ToString();//房号
            if (string.IsNullOrEmpty(RoomNumber))
                throw new PMSException("房号不能为空");


            //string RoomNumber = "";//房号
            //if (jd_data["RoomNumber"] != null)
            //{
            //    RoomNumber = jd_data["RoomNumber"].ToString();//房号
            //}

            CheckData(jd_data, "RoomCode", "客房类型代码");//必传判断
            string RoomCode = jd_data["RoomCode"].ToString();//客房类型代码
            if (string.IsNullOrEmpty(RoomCode))
                throw new PMSException("客房类型代码不能为空");


            //string RoomCode = "";//客房类型代码
            //if (jd_data["RoomCode"] != null)
            //{
            //    RoomCode = jd_data["RoomCode"].ToString();//客房类型代码
            //}

            CheckData(jd_data, "RoomState", "房间状态代码");//必传判断
            string RoomState = jd_data["RoomState"].ToString();//房间状态代码
            if (string.IsNullOrEmpty(RoomState))
                throw new PMSException("房间状态不能为空");

            //string RoomState = "";//房间状态代码
            //if (jd_data["RoomState"] != null)
            //{
            //    RoomState = jd_data["RoomState"].ToString();//房间状态代码
            //}


            CheckData(jd_data, "RoomStyle", "房间风格代码");//必传判断
            string RoomStyle = jd_data["RoomStyle"].ToString();//房间风格代码
            if (string.IsNullOrEmpty(RoomStyle))
                throw new PMSException("房间风格代码不能为空");


            //string RoomStyle = "";//房间风格代码
            //if (jd_data["RoomStyle"] != null)
            //{
            //    RoomStyle = jd_data["RoomStyle"].ToString();//房间风格代码
            //}

            CheckData(jd_data, "GuestType", "客人类型代码");//必传判断
            string GuestType = jd_data["GuestType"].ToString();//客人类型代码
            if (string.IsNullOrEmpty(GuestType))
                throw new PMSException("客人类型代码不能为空");

            //string GuestType = "";//客人类型代码
            //if (jd_data["GuestType"] != null)
            //{
            //    GuestType = jd_data["GuestType"].ToString();//客人类型代码
            //}

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            Hashtable ht = new Hashtable();
            ht["fh"] = RoomNumber;//房号
            ht["kfcode"] = RoomCode;//客房类型代码
            ht["ftcode"] = RoomState;//房间状态代码
            ht["tycode"] = RoomStyle;//房间风格代码
            ht["krlx"] = GuestType;//客人类型代码
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableProc("[proc_ssft_jdb]", ht);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["SerialNumber"] = dt.Rows[i]["Xh"].ToString();//序号
                    jdList["RoomNumber"] = dt.Rows[i]["fh"].ToString();//房号
                    jdList["RoomState"] = dt.Rows[i]["Ftcode"].ToString();//房间状态代码
                    jdList["GuestType"] = dt.Rows[i]["Krlx"].ToString();//客人类型代码
                    jdList["GuestName"] = dt.Rows[i]["Krname"].ToString();//客人姓名(在住、预订)
                    jdList["RoomCode"] = dt.Rows[i]["Kfcode"].ToString();//房间类型代码
                    jdList["FoolRoomNumber"] = dt.Rows[i]["Icfh"].ToString();//门锁发卡房号
                    jdList["ArrivalTime"] = dt.Rows[i]["ddrq"].ToString();//抵店日期
                    jdList["LeaveTime"] = dt.Rows[i]["ldrq"].ToString();//离店日期(在住、预订)
                    jdList["CertificatesNumber"] = dt.Rows[i]["zjhm"].ToString();//证件号码
                    jdList["RoomAccount"] = dt.Rows[i]["fjzh"].ToString();//房间账号(即PMS订单号)
                    jdList["MemberCardNumber"] = dt.Rows[i]["vipkh"].ToString();//会员卡号
                    jdList["Remarks"] = dt.Rows[i]["bz"].ToString();//备注
                    jdList["Phone"] = dt.Rows[i]["sjhm"].ToString();//手机号码(即PMS订单号)
                    jdList["InvalidInfo"] = dt.Rows[i]["Ftcode1"].ToString();//无效信息(预留)
                    jdList["TotalPrice"] = dt.Rows[i]["xfze"].ToString();//消费总额
                    jdList["CreditIimit"] = dt.Rows[i]["Xyxe"].ToString();//信用限额(押金金额)
                    jdList["RoomPrice"] = dt.Rows[i]["Fj"].ToString();//房价
                    jdList["Sex"] = dt.Rows[i]["krxb"].ToString();//客人性别
                    jdList["HouseInfo"] = dt.Rows[i]["fgxx"].ToString();//房管信息
                    jdList["RoomTypeName"] = dt.Rows[i]["flmc"].ToString();//房间类型名称
                    jdList["FoolCode"] = dt.Rows[i]["lcbm"].ToString();//楼层编码
                    jdList["SameInfo"] = dt.Rows[i]["txbh"].ToString();//同行编号
                    jdList["RoomStateName"] = dt.Rows[i]["fjzt"].ToString();//房间状态名称
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                if (dt != null && dt.Rows.Count == 0)
                {
                    jd["code"] = 1;
                    jd["message"] = "空房";
                }
                else
                {
                    jd["code"] = 0;
                    jd["message"] = "未查询到相关信息!";
                }

            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 11.	续住前判断指定时间内当前在住房是否可续住 **

        public void GetCheckContinued(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "LeaveTime", "新离店日期");//必传判断
            string LeaveTime = jd_data["LeaveTime"].ToString();//新离店日期
            if (string.IsNullOrEmpty(LeaveTime))
                throw new PMSException("新离店日期不能为空");

            CheckData(jd_data, "OrderNumber", "订单号");//必传判断
            string OrderNumber = jd_data["OrderNumber"].ToString();//订单号
            if (string.IsNullOrEmpty(OrderNumber))
                throw new PMSException("订单号不能为空");



            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }


            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["rq"] = LeaveTime;//新离店日期
            ht["chk"] = OrderNumber;//订单号
            ht["OUT_ret"] = null;
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("[proc_extension]", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                if (rs["OUT_ret"] != null && rs["OUT_ret"].ToString() != "")
                {
                    JsonData jdList = new JsonData();
                    jdList["ReturnMessage"] = rs["OUT_ret"].ToString();
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

        #region  ** 12.	预订订单查询 **

        public void GetOrderQuery(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            //CheckData(jd_data, "ArrivalTime", "抵店日期");//必传判断
            //string ArrivalTime = jd_data["ArrivalTime"].ToString();//抵店日期
            //if (string.IsNullOrEmpty(ArrivalTime))
            //    throw new PMSException("抵店日期不能为空");

            //CheckData(jd_data, "GuestName", "客人姓名");//必传判断
            //string GuestName = jd_data["GuestName"].ToString();//客人姓名
            //if (string.IsNullOrEmpty(GuestName))
            //    throw new PMSException("客人姓名不能为空");



            string ArrivalTime = "";//客人姓名
            if (jd_data["ArrivalTime"] != null)
            {
                ArrivalTime = jd_data["ArrivalTime"].ToString();//客人姓名
            }

            string GuestName = "";//客人姓名
            if (jd_data["GuestName"] != null)
            {
                GuestName = jd_data["GuestName"].ToString();//客人姓名
            }

            string Phone = "";//手机号
            if (jd_data["Phone"] != null)
            {
                Phone = jd_data["Phone"].ToString();//手机号
            }


            string BookOrderNum = "";//预订单号
            if (jd_data["BookOrderNum"] != null)
            {
                BookOrderNum = jd_data["BookOrderNum"].ToString();//预订单号
            }

            string OtaOrderNum = "";//OTA订单号
            if (jd_data["OtaOrderNum"] != null)
            {
                OtaOrderNum = jd_data["OtaOrderNum"].ToString();//OTA订单号
            }


            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            Hashtable ht = new Hashtable();
            ht["ddrq"] = ArrivalTime;//抵店日期
            ht["xm"] = GuestName;//客人姓名
            ht["sjhm"] = Phone;//预订时登记的手机号码
            if (BookOrderNum == "")
            {
                ht["yddh"] = null;//预订单号
            }
            else
            {
                ht["yddh"] = BookOrderNum;//预订单号
            }

            ht["otaddh"] = OtaOrderNum;//OTA订单号
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableProc("[proc_book_select]", ht);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["OrderNumber"] = dt.Rows[i]["yddh"].ToString();//订单号
                    jdList["BooktName"] = dt.Rows[i]["Ydrname"].ToString();//预订人姓名
                    jdList["LiveName"] = dt.Rows[i]["Krname"].ToString();//住店人姓名
                    jdList["RoomCode"] = dt.Rows[i]["Kfcode"].ToString();//房间类型代码
                    jdList["BookNum"] = dt.Rows[i]["Dfs"].ToString();//订房数量
                    jdList["ArrivalTime"] = dt.Rows[i]["Ddrq"].ToString();//抵店日期
                    jdList["LiveDay"] = dt.Rows[i]["Zdts"].ToString();//住店天数
                    jdList["LeaveTime"] = dt.Rows[i]["Ldrq"].ToString();//离店日期
                    jdList["Deposit"] = dt.Rows[i]["Yjje"].ToString();//已交押金金额
                    jdList["DiscountCode"] = dt.Rows[i]["Scmcode"].ToString();//折扣码
                    jdList["RoomPrice"] = dt.Rows[i]["Fj"].ToString();//房价
                    jdList["RoomNumber"] = dt.Rows[i]["Fh"].ToString();//房号 为空表示未分房
                    jdList["TogetherNumber"] = dt.Rows[i]["txbh"].ToString();//同行编号
                    jdList["Remarks"] = dt.Rows[i]["bz"].ToString();//备注
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

        #region  ** 13.预订单分房 **
        public void GetDistributionRoom(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];


            CheckData(jd_data, "OrderNumber", "订单号");//必传判断
            string OrderNumber = jd_data["OrderNumber"].ToString();//订单号
            if (string.IsNullOrEmpty(OrderNumber))
                throw new PMSException("订单号不能为空");

            CheckData(jd_data, "RoomNumber", "房号");//必传判断
            string RoomNumber = jd_data["RoomNumber"].ToString();//房号
            if (string.IsNullOrEmpty(RoomNumber))
                throw new PMSException("房号不能为空");



            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }


            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["yddh"] = OrderNumber;//预定单号
            ht["fh"] = RoomNumber;//房号
            ht["OUT_ret"] = null;
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("[proc_book_room]", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                JsonData jdList = new JsonData();
                if (rs["OUT_ret"] != null && rs["OUT_ret"].ToString() != "")
                {
                    jdList["ReturnMessage"] = rs["OUT_ret"].ToString();//返回值
                }
                jdData.Add(jdList);
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

        #region  ** 14.	判断是否是在住房 **

        public void GetCheckRoom(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "OrderNumber", "订单号");//必传判断
            string OrderNumber = jd_data["OrderNumber"].ToString();//订单号
            if (string.IsNullOrEmpty(OrderNumber))
                throw new PMSException("订单号不能为空");



            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }


            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["zh"] = OrderNumber;//订单号
            ht["OUT_ret"] = null;
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("[proc_krxx_zhcx]", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                if (rs["OUT_ret"] != null && rs["OUT_ret"].ToString() != "")
                {
                    JsonData jdList = new JsonData();
                    jdList["ReturnMessage"] = rs["OUT_ret"].ToString();
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

        #region  ** 15.续住 **
        public void GetContinueRoom(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];


            CheckData(jd_data, "OrderNumber", "订单号");//必传判断
            string OrderNumber = jd_data["OrderNumber"].ToString();//订单号
            if (string.IsNullOrEmpty(OrderNumber))
                throw new PMSException("订单号不能为空");

            CheckData(jd_data, "LeaveTIme", "预离时间");//必传判断
            string LeaveTIme = jd_data["LeaveTIme"].ToString();//预离时间
            if (string.IsNullOrEmpty(LeaveTIme))
                throw new PMSException("预离时间不能为空");


            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }


            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["fjzh"] = OrderNumber;//预定单号
            ht["ldrq"] = LeaveTIme;//预离时间
            ht["OUT_ret"] = null;
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("[PROC_exten]", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                JsonData jdList = new JsonData();
                if (rs["OUT_ret"] != null && rs["OUT_ret"].ToString() != "")
                {
                    jdList["ReturnMessage"] = rs["OUT_ret"].ToString();//返回值
                }
                jdData.Add(jdList);
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

        #region  ** 18.	账单查询 **

        public void GetBillQuery(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "OrderNumber", "订单号");//必传判断
            string OrderNumber = jd_data["OrderNumber"].ToString();//订单号
            if (string.IsNullOrEmpty(OrderNumber))
                throw new PMSException("订单号不能为空");


            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            Hashtable ht = new Hashtable();
            ht["fjzh"] = OrderNumber;//订单号
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableProc("[proc_bill]", ht);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["AccountNumber"] = dt.Rows[i]["zh"].ToString();//账号
                    jdList["AccountType"] = dt.Rows[i]["Zhlb"].ToString();//账号类别
                    jdList["AccountBook"] = dt.Rows[i]["ZT"].ToString();//账套
                    jdList["SerialNumber"] = dt.Rows[i]["Xh"].ToString();//序号
                    jdList["RecordTime"] = dt.Rows[i]["Rq"].ToString();//录入系统系统时间
                    jdList["ActualTime"] = dt.Rows[i]["Sjrq"].ToString();//实际录入系统时间
                    jdList["CostCode"] = dt.Rows[i]["fymcode"].ToString();//费用代码
                    jdList["PaymentCode"] = dt.Rows[i]["Fkcode"].ToString();//付款方式代码
                    jdList["AccountName"] = dt.Rows[i]["Desc0"].ToString();//账目名称
                    jdList["AccountitemType"] = dt.Rows[i]["Flag"].ToString();//账目类别
                    jdList["OperatorNumber"] = dt.Rows[i]["Czygh"].ToString();//操作员工号
                    jdList["Shifts"] = dt.Rows[i]["bc"].ToString();//操作班次
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到客人信息!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 19. 查询余额 **

        public void GetCheckBalance(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "OrderNumber", "订单号");//必传判断
            string OrderNumber = jd_data["OrderNumber"].ToString();//订单号
            if (string.IsNullOrEmpty(OrderNumber))
                throw new PMSException("订单号不能为空");

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }


            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["chk"] = OrderNumber;//订单号
            ht["OUT_ye"] = null;//返回值 剩余余额(不包含预授权金额)
            ht["OUT_xyye"] = null;//返回值 剩余信用限额(包含预授权金额)
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("[proc_balance]", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                JsonData jdList = new JsonData();
                if (rs["OUT_ye"] != null && rs["OUT_ye"].ToString() != "")
                {
                    jdList["SurplusBalance"] = rs["OUT_ye"].ToString();//剩余余额(不包含预授权金额)
                }
                if (rs["OUT_xyye"] != null && rs["OUT_xyye"].ToString() != "")
                {
                    jdList["CreditBalance"] = rs["OUT_xyye"].ToString();// 剩余信用限额(包含预授权金额)
                }
                jdData.Add(jdList);
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

        #region  ** 20.退房结账 **

        public void GetCheckout(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "OrderNumber", "订单号");//必传判断
            string OrderNumber = jd_data["OrderNumber"].ToString();//订单号
            if (string.IsNullOrEmpty(OrderNumber))
                throw new PMSException("订单号不能为空");

            CheckData(jd_data, "RoomNunber", "房号");//必传判断
            string RoomNunber = jd_data["RoomNunber"].ToString();//房号
            if (string.IsNullOrEmpty(RoomNunber))
                throw new PMSException("房号不能为空");

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["fjzh"] = OrderNumber;//订单号
            ht["fh"] = RoomNunber;//房号
            ht["OUT_ret"] = null;
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("[PROC_JZTF_fjzh]", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                if (rs["OUT_ret"] != null && rs["OUT_ret"].ToString() != "")
                {
                    JsonData jdList = new JsonData();
                    jdList["ReturnMessage"] = rs["OUT_ret"].ToString();
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

        #region  ** 21. 退房不结账（挂S账） **

        public void GetNoCheckout(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            CheckData(jd_data, "OrderNumber", "订单号");//必传判断
            string OrderNumber = jd_data["OrderNumber"].ToString();//订单号
            if (string.IsNullOrEmpty(OrderNumber))
                throw new PMSException("订单号不能为空");

            CheckData(jd_data, "RoomNunber", "房号");//必传判断
            string RoomNunber = jd_data["RoomNunber"].ToString();//房号
            if (string.IsNullOrEmpty(RoomNunber))
                throw new PMSException("房号不能为空");

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["fjzh"] = OrderNumber;//订单号
            ht["fh"] = RoomNunber;//房号
            ht["OUT_ret"] = null;
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("[PROC_KFTF_fjzh]", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                if (rs["OUT_ret"] != null && rs["OUT_ret"].ToString() != "")
                {
                    JsonData jdList = new JsonData();
                    jdList["ReturnMessage"] = rs["OUT_ret"].ToString();
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

        #region  ** 22. 获取客户端系统时间 **

        public void GetSystemTime(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];


            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            Hashtable rs = new Hashtable();
            Hashtable ht = new Hashtable();
            ht["OUT_rq"] = null;
            JsonData jd = new JsonData();//返回JsonData
            DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteByProcReturn("[proc_dqxtsj]", ht, ref rs);
            if (rs != null && rs.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                if (rs["OUT_rq"] != null && rs["OUT_rq"].ToString() != "")
                {
                    JsonData jdList = new JsonData();
                    jdList["ReturnMessage"] = Convert.ToDateTime(rs["OUT_rq"]).ToString("yyyy-MM-dd HH:mm:ss");
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




        #region  ** 30. 房态表 **

        public void GetRoomState(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM CS_FTDMB");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["RoomState"] = dt.Rows[i]["ftcode"].ToString();//房间状态
                    jdList["RoomName"] = dt.Rows[i]["desc0"].ToString();//中文名称
                    jdList["EnRoomName"] = dt.Rows[i]["endesc0"].ToString();//英文名称
                    jdList["Sybj"] = dt.Rows[i]["sybj"].ToString();
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到房价码!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }
        #endregion

        #region  ** 31. 客房房号参数表 **

        public void GetRoomNumParameter(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("  select * from CS_KFFHCSB");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["RoomNumber"] = dt.Rows[i]["fh"].ToString();//房号
                    jdList["RoomCode"] = dt.Rows[i]["kfcode"].ToString();//房间类型
                    jdList["FoolNumber"] = dt.Rows[i]["lcbm"].ToString();//楼层号
                    jdList["RoomDescribe"] = dt.Rows[i]["fjms"].ToString();//房间描述
                    jdList["DoorLockRoomNum"] = dt.Rows[i]["icfh"].ToString();//门锁房号
                    jdList["RoomState"] = dt.Rows[i]["ftcode"].ToString();//房间状态
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData;
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

        #region  ** 32. 客房类型参数表 同接口1 **

        public void GetRoomTypes(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("  select * from CS_KFLXCSB");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["RoomCode"] = dt.Rows[i]["kfcode"].ToString();//房间类型
                    jdList["RoomName"] = dt.Rows[i]["desc0"].ToString();//房间名称
                    jdList["RoomIntegral"] = dt.Rows[i]["kfjf"].ToString();//房间兑换积分
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData;
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

        #region  ** 33. 付款方式表 **

        public void GetPaymentMethod(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM CS_FKFSB ");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["PayCode"] = dt.Rows[i]["fkcode"].ToString();//付款代码
                    jdList["PayCodeName"] = dt.Rows[i]["desc0"].ToString();//付款代码名称
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData;
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

        #region  ** 34. 获取在住、预抵客人信息表 同接口4 **

        public void GetRoomLive(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string Content = CommonUse.ParamVal(jd_data, "Content", "查询内容", false, false);
            int pageIndex = CommonHelper.GetInt(CommonUse.ParamVal(jd_data, "PageIndex", "当前页", true, true));
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);

            string sign_new = Md5Helper.MD5(HotelCode + pageIndex, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT  fh ,
                    krname ,
                    fj ,
                    ddrq ,
                    ldrq
            FROM    view_fjzhkrname
            WHERE   rzcode = @rzcode       
            ");

            JsonData jdData = new JsonData();//DataTable值
            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@rzcode", "I"));
            if (Content != "")
            {
                sql.Append("  AND ( krname LIKE @Content OR fh LIKE @Content ) ");
                lParam.Add(new SqlParam("@Content", "%" + Content + "%"));
            }
            sql.Append(" ORDER BY ldrq ASC ");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                int PageSize = 20;
                int doCount = dt.Rows.Count;
                int pi_ps = (pageIndex - 1) * PageSize;
                int pi_dc = pi_ps + PageSize;
                if (pi_dc > doCount)
                {
                    pi_dc = doCount;
                }
                int more = doCount - pi_dc;//更多剩余数               
                more = more > 0 ? more : 0;
                for (int i = pi_ps; i < pi_dc; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["GuestNum"] = doCount;//数量
                    jdList["GuestName"] = dt.Rows[i]["krname"].ToString();//客人姓名
                    jdList["RoomNumber"] = dt.Rows[i]["fh"].ToString();//房号
                    jdList["RoomPrice"] = Convert.ToDouble(dt.Rows[i]["fj"]).ToString("0.00");//房价
                    jdList["ArrivalTime"] = dt.Rows[i]["ddrq"].ToString();//抵店日期
                    jdList["LeaveTime"] = dt.Rows[i]["ldrq"].ToString();//离店日期(在住、预订)
                    jdList["AlTime"] = Convert.ToDateTime(dt.Rows[i]["ddrq"]).ToString("MM/dd") + "-" + Convert.ToDateTime(dt.Rows[i]["ldrq"]).ToString("MM/dd");
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["count"] = doCount;//总数
                jd["more"] = more;//更多剩余数
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

        //预抵客人
        public void GetRoomArrivals(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string Content = CommonUse.ParamVal(jd_data, "Content", "查询内容", false, false);
            int pageIndex = CommonHelper.GetInt(CommonUse.ParamVal(jd_data, "PageIndex", "当前页", true, true));
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);

            string sign_new = Md5Helper.MD5(HotelCode + pageIndex, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT  krname + '-' + ISNULL(dw, '') krname ,
                    fj ,
                    ddrq ,
                    ldrq
            FROM    XX_SKYDDB
            WHERE   ydzt IN ( 'DR', 'GT' )
            ");

            JsonData jdData = new JsonData();//DataTable值
            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@rzcode", "I"));
            if (Content != "")
            {
                sql.Append("  AND krname LIKE @Content ");
                lParam.Add(new SqlParam("@Content", "%" + Content + "%"));
            }
            sql.Append(" ORDER BY ddrq ASC ");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                int PageSize = 20;
                int doCount = dt.Rows.Count;
                int pi_ps = (pageIndex - 1) * PageSize;
                int pi_dc = pi_ps + PageSize;
                if (pi_dc > doCount)
                {
                    pi_dc = doCount;
                }
                int more = doCount - pi_dc;//更多剩余数               
                more = more > 0 ? more : 0;
                for (int i = pi_ps; i < pi_dc; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["GuestNum"] = doCount;//数量
                    jdList["GuestName"] = dt.Rows[i]["krname"].ToString().Trim('-');//客人姓名
                    jdList["RoomPrice"] = Convert.ToDouble(dt.Rows[i]["fj"]).ToString("0.00");//房价
                    jdList["ArrivalTime"] = Convert.ToDateTime(dt.Rows[i]["ddrq"]).ToString("yyyy-MM-dd");//抵店日期
                    jdList["LeaveTime"] = Convert.ToDateTime(dt.Rows[i]["ldrq"]).ToString("yyyy-MM-dd");//离店日期(在住、预订)
                    jdList["AlTime"] = Convert.ToDateTime(dt.Rows[i]["ddrq"]).ToString("MM/dd") + "-" + Convert.ToDateTime(dt.Rows[i]["ldrq"]).ToString("MM/dd");
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["count"] = doCount;//总数
                jd["more"] = more;//更多剩余数
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

        #region  ** 35. 房态视图表 **

        private void CheckView(string adminHotelId, string hotelId)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
             Select 0
             From   sysobjects
             Where  name = 'view_fhfjzt'
                    And xtype = 'V' 
            ");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sb);
            if (dt == null || dt.Rows.Count == 0)
            {
                #region  ** 创建视图 **
                StringBuilder sb_update = new StringBuilder();
                sb_update.Append(@"
                CREATE  view [dbo].[view_fhfjzt](fh,ftcode,zt,xm,fgxx,lcbm,krlx,icfh)  
                as  
                SELECT   CS_SETFTB.fh,                                         
                         'O' + substring(CS_KFFHCSB.ftcode,2,1),                                      
                         '预离',                                      
                         view_fjzhkrname.krname,                                                                          
                ltrim(rtrim(isnull(CS_KFFHCSB.fgxx,''))),  
                cs_kffhcsb_lcbm = CS_KFFHCSB.lcbm,  
                     view_fjzhkrname.krlx,
                     cs_kffhcsb.FH                                          
                    FROM CS_KFFHCSB,                                         
                         CS_SETFTB,                                      
                         view_fjzhkrname                                        
                   WHERE ( CS_KFFHCSB.fh = CS_SETFTB.fh ) and                                         
                         ( view_fjzhkrname.fh = CS_KFFHCSB.fh) and                                      
                         ( CS_SETFTB.ftbms = '标准房态表' )  and                                       
                         (view_fjzhkrname.rzcode = 'I') and                                                        
                          datediff(day,view_fjzhkrname.ldrq,(select xtdate from CS_QTXTZTB)) = 0                                       
                      
                /*预离房*/                                      
                union                                      
                SELECT           
                         CS_SETFTB.fh,                                         
                         'O' + substring(CS_KFFHCSB.ftcode,2,1),                                      
                         '在住' ,                                      
                         view_fjzhkrname.krname,  
                ltrim(rtrim(isnull(CS_KFFHCSB.fgxx,''))), CS_KFFHCSB.lcbm,  
                     view_fjzhkrname.krlx,
                     cs_kffhcsb.FH                              
                    FROM CS_KFFHCSB,                       
                         CS_SETFTB,                                      
                         view_fjzhkrname                                        
                   WHERE ( CS_KFFHCSB.fh = CS_SETFTB.fh ) and                                         
                         ( view_fjzhkrname.fh = CS_KFFHCSB.fh) and                                      
                         ( CS_SETFTB.ftbms = '标准房态表')  and                     
                          datediff(day,view_fjzhkrname.ldrq,(select xtdate from CS_QTXTZTB)) <> 0                                      
                /*正常在住房*/                                      
                union                                      
                select                        
                       CS_SETFTB.fh ,                                      
                       'B' + substring(CS_KFFHCSB.ftcode,2,1),                                      
                       '预抵',                              
                        XX_SKYDDB.krname,                                      
                      LTRIM(RTRIM(ISNULL(CS_KFFHCSB.fgxx,''))),CS_KFFHCSB.lcbm,XX_SKYDDB.krlxcode,
                     cs_kffhcsb.FH                                         
                  FROM CS_KFFHCSB,CS_SETFTB,XX_SKYDDB                                      
                 WHERE (CS_KFFHCSB.fh = CS_SETFTB.fh ) AND                               
                         (ISNULL(XX_SKYDDB.fh,'') = CS_KFFHCSB.fh) AND                                      
                         ( CS_SETFTB.ftbms =  '标准房态表' )  AND                                                                    
                         (ISNULL(XX_SKYDDB.fh,'') NOT IN(SELECT fh FROM XX_KFDJXXB WHERE rzcode = 'I')) AND                                      
                        (ISNULL(XX_SKYDDB.fh,'') NOT IN(SELECT fh FROM XX_KFDJXXB WHERE rzcode = 'C' AND (DATEDIFF(DAY,ddrq,(SELECT xtdate FROM CS_QTXTZTB)) = 0))) AND                                      
                         (XX_SKYDDB.ydzt IN( 'DR','GT','WT')) AND                                       
                         (DATEDIFF(DAY,XX_SKYDDB.ddrq,(SELECT xtdate FROM CS_QTXTZTB)) = 0)                                      
                /*预抵房*/                                      
                                      
                UNION                                      
                SELECT                                       
                         CS_SETFTB.fh ,                                        
                         'B' + SUBSTRING(CS_KFFHCSB.ftcode,2,1),                                      
                         '预抵',                                      
                         view_fjzhkrname.krname,                                      
                         LTRIM(RTRIM(ISNULL(CS_KFFHCSB.fgxx,''))),CS_KFFHCSB.lcbm,view_fjzhkrname.krlx,
                     cs_kffhcsb.FH                                              
                    FROM CS_KFFHCSB,                                         
                         CS_SETFTB,                                      
                         view_fjzhkrname                                        
                   WHERE ( CS_KFFHCSB.fh = CS_SETFTB.fh ) AND                                         
                         ( view_fjzhkrname.fh = CS_KFFHCSB.fh) AND                                      
                         ( CS_SETFTB.ftbms =  '标准房态表' )  AND                                                
                          (view_fjzhkrname.rzcode = 'C') AND                                      
                          (DATEDIFF(DAY,view_fjzhkrname.ddrq,(SELECT xtdate FROM CS_QTXTZTB)) = 0) AND                                      
                          (view_fjzhkrname.fh NOT IN(SELECT fh FROM XX_KFDJXXB WHERE rzcode = 'I'))                                      
                /*团队预分房*/                         
                UNION                                      
                    SELECT                                      
                         ISNULL(CS_SETFTB.fh,''),                                         
                         CS_KFFHCSB.ftcode,                                      
                         '空房',                                      
                         ISNULL(CS_KFFHCSB.des,CS_KFLXCSB.desc0) ,                                      
                         LTRIM(RTRIM(ISNULL(CS_KFFHCSB.fgxx,''))),CS_KFFHCSB.lcbm,NULL,
                     cs_kffhcsb.FH                                         
                    FROM CS_KFFHCSB,                                         
                         CS_SETFTB,                                      
                         CS_KFLXCSB                                        
                   WHERE ( CS_KFFHCSB.fh = CS_SETFTB.fh ) AND                                         
                         ( CS_SETFTB.ftbms = '标准房态表' )  AND                                       
                         ( CS_KFLXCSB.kfcode = CS_KFFHCSB.kfcode) AND                                                                   
                         (CS_KFFHCSB.fh NOT IN(SELECT fh FROM XX_KFDJXXB WHERE rzcode = 'I')) AND                                      
                   (CS_KFFHCSB.fh NOT IN(SELECT fh FROM XX_KFDJXXB WHERE rzcode = 'C'          
                AND (DATEDIFF(DAY,ddrq,(SELECT xtdate FROM CS_QTXTZTB)) = 0))) AND                                      
                         (CS_KFFHCSB.fh NOT IN(SELECT ISNULL(XX_SKYDDB.fh,'') FROM XX_SKYDDB WHERE ydzt IN('DR','GT') AND                                       
                         (DATEDIFF(DAY,XX_SKYDDB.ddrq,(SELECT xtdate FROM CS_QTXTZTB)) = 0) )) AND                                      
                   (CS_KFFHCSB.ftcode NOT LIKE 'L%')                                
                              
                UNION                                      
                   SELECT                                         
                         ISNULL(CS_SETFTB.fh,''),                                         
                         'L' + SUBSTRING(CS_KFFHCSB.ftcode,2,1),                                      
                         '锁房',                                      
                         CS_KFFHCSB.des,LTRIM(RTRIM(ISNULL(CS_KFFHCSB.fgxx,''))),CS_KFFHCSB.lcbm,NULL ,
                     cs_kffhcsb.FH            
                    FROM CS_KFFHCSB,                                         
                         CS_SETFTB                                        
                   WHERE ( CS_KFFHCSB.fh = CS_SETFTB.fh ) AND                                         
                         ( CS_SETFTB.ftbms = '标准房态表' )  AND                                                              
                         (CS_KFFHCSB.fh NOT IN(SELECT fh FROM XX_KFDJXXB                        
                WHERE DATEDIFF(DAY,ddrq,(SELECT xtdate FROM CS_QTXTZTB)) >= 0  AND rzcode IN('C','I'))) AND                               
                         (CS_KFFHCSB.ftcode LIKE 'L%')   
          
                ");
                DataFactory.SqlDataBase(adminHotelId, hotelId).ExecuteBySql(sb_update);
                #endregion
            }
        }

        public void GetRoomStateView(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");


            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];

            string RoomState = "";//房间状态
            if (jd_data["RoomState"] != null)
            {
                RoomState = jd_data["RoomState"].ToString();//房间状态
            }

            string GuestType = "";//客人类型
            if (jd_data["GuestType"] != null)
            {
                GuestType = jd_data["GuestType"].ToString();//房间状态
            }

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }
            CheckView(adminHotelId, hotelId);

            StringBuilder sql = new StringBuilder();
            sql.Append("select  * from view_fhfjzt where 1=1 ");
            if (RoomState != "")
            {
                if (RoomState == "CMC")
                {
                    sql.Append(" and zt='预抵' and ftcode='BC' ");
                }
                else if (RoomState == "CMD")
                {
                    sql.Append(" and zt='预抵' and ftcode='BD' ");
                }
                else if (RoomState == "GO")
                {
                    sql.Append(" and zt='预离' ");
                }
                else if (RoomState == "LR")
                {
                    sql.Append(" and zt='锁房' ");
                }
                else
                {
                    sql.Append(" and ftcode='" + RoomState + "' ");
                }
            }

            if (GuestType != "")
            {
                sql.Append(" and krlx='" + GuestType + "' ");
            }

            sql.Append(" order by fh asc ");

            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql);

            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                string GuestNum = dt.Rows.Count.ToString();
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();

                    string StateStyle = "";
                    string ftcode = dt.Rows[i]["ftcode"].ToString().Trim();
                    string zt = dt.Rows[i]["zt"].ToString().Trim();
                    if (ftcode == "OC")//在住房
                    {

                        if (RoomState != "")
                        {
                            if (RoomState == "GO")
                            {
                                StateStyle = "yuli";
                            }
                            else
                            {
                                StateStyle = "zaizhu";
                            }
                        }
                        else
                        {
                            if (zt == "预离")
                            {
                                StateStyle = "yuli";
                            }
                            else
                            {
                                StateStyle = "zaizhu";
                            }
                        }
                    }
                    else if (ftcode == "OD")//在住脏房
                    {

                        if (RoomState != "")
                        {
                            if (RoomState == "GO")
                            {
                                StateStyle = "yuli";
                            }
                            else
                            {
                                StateStyle = "zhuzang";
                            }
                        }
                        else
                        {
                            if (zt == "预离")
                            {
                                StateStyle = "yuli";
                            }
                            else
                            {
                                StateStyle = "zhuzang";
                            }
                        }
                    }
                    else if (ftcode == "GO" && zt == "预离")//预离房
                    {
                        StateStyle = "yuli";
                    }

                    else if (ftcode == "CMC")//预抵净房
                    {
                        StateStyle = "yudijing";
                    }
                    else if (ftcode == "BC" && zt == "预抵")//预抵净房
                    {
                        StateStyle = "yudijing";
                    }

                    else if (ftcode == "CMD")//预抵脏房
                    {
                        StateStyle = "yudizang";
                    }
                    else if (ftcode == "BD" && zt == "预抵")//预抵脏房
                    {
                        StateStyle = "yudizang";
                    }

                    else if (ftcode == "OOO")//维修房
                    {
                        StateStyle = "weixiu";
                    }
                    else if (ftcode == "OOS")//维护房
                    {
                        StateStyle = "weihu";
                    }
                    else if (ftcode == "LR" && zt == "锁房")//锁房
                    {
                        StateStyle = "suo";
                    }
                    else if (ftcode == "LC" && zt == "锁房")//锁房
                    {
                        StateStyle = "suo";
                    }
                    else if (ftcode == "VD")//空脏房
                    {
                        StateStyle = "kongzang";
                    }
                    else if (ftcode == "VC")//空洁房
                    {
                        StateStyle = "kongjie";
                    }

                    jdList["RoomNumber"] = dt.Rows[i]["fh"].ToString();//房间号
                    jdList["StateStyle"] = StateStyle;//房间样式
                    jdList["RoomState"] = dt.Rows[i]["ftcode"].ToString().Trim(); //房间状态代码
                    jdList["RoomStateName"] = dt.Rows[i]["zt"].ToString().Trim();//房间状态
                    jdList["RoomName"] = dt.Rows[i]["xm"].ToString().Trim();//房间名称
                    jdList["RoomInfo"] = dt.Rows[i]["fgxx"].ToString().Trim();//房管信息
                    jdList["FoolNumber"] = dt.Rows[i]["lcbm"].ToString().Trim();//楼层编码
                    jdList["GuestCode"] = dt.Rows[i]["krlx"].ToString().Trim();//客人类型编码
                    if (dt.Rows[i]["krlx"] != null && dt.Rows[i]["krlx"].ToString().Trim() != "")
                    {
                        string GuestStyle = dt.Rows[i]["krlx"].ToString().Trim();
                        if (GuestStyle == "BTF")//白天房
                        {
                            GuestStyle = "btf";
                        }
                        else if (GuestStyle == "BK")//预订客人
                        {
                            GuestStyle = "ydk";
                        }
                        else if (GuestStyle == "HU")//自用房
                        {
                            GuestStyle = "zyf";
                        }
                        else if (GuestStyle == "CO")//协议公司客
                        {
                            GuestStyle = "xyk";
                        }
                        else if (GuestStyle == "OTA" || GuestStyle == "TG")//OTA客人
                        {
                            GuestStyle = "otakr";
                        }
                        else if (GuestStyle == "LXS" || GuestStyle == "LX")//旅行客
                        {
                            GuestStyle = "lxs";
                        }
                        else if (GuestStyle == "FR")//免费房
                        {
                            GuestStyle = "mff";
                        }
                        else if (GuestStyle == "JF")//积分换房
                        {
                            GuestStyle = "jfhf";
                        }
                        else if (GuestStyle == "LS")//长住客
                        {
                            GuestStyle = "czk";
                        }
                        else if (GuestStyle == "NET")//网络公司
                        {
                            GuestStyle = "wlgs";
                        }
                        else if (GuestStyle == "VIP")//VIP客人
                        {
                            GuestStyle = "vipkr";
                        }
                        else if (GuestStyle == "VVI")//金卡会员
                        {
                            GuestStyle = "jkhy";
                        }
                        else if (GuestStyle == "WI")//自来的散客
                        {
                            GuestStyle = "zldxk";
                        }
                        else if (GuestStyle == "WX")//公众号订单
                        {
                            GuestStyle = "wwkr";
                        }
                        else if (GuestStyle == "WYF")//午夜房
                        {
                            GuestStyle = "wyf";
                        }
                        else if (GuestStyle == "ZDF")//钟点房
                        {
                            GuestStyle = "zdf";
                        }
                        else if (GuestStyle == "ZDH")//招待房
                        {
                            GuestStyle = "zdh";
                        }

                        jdList["GuestStyle"] = GuestStyle; 

                    }
                    else
                    {
                        jdList["GuestType"] = "";
                    }
                    //CO协议公司客,OTA网络（会员）,LXS旅行社,WX公众号订单
                    //xyk, otakr, lxs, wwkr
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

        #region  ** 36. 统计各种房态数量 **

        public void GetRoomStateNum(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            CheckData(jd_data, "HotelCode", "酒店编号");//必传判断
            //获取Get方式的URL 参数
            string HotelCode = jd_data["HotelCode"].ToString();//酒店编号

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"SELECT ftcode,COUNT(1) AS ftcount FROM view_fhfjzt WHERE 1=1 GROUP BY ftcode
                                         union all
                                         SELECT 'CMC', COUNT(1) FROM view_fhfjzt WHERE  zt='预抵' and ftcode='BC'
                                         union all
                                         SELECT 'CMD', COUNT(1) FROM view_fhfjzt WHERE  zt='预抵' and ftcode='BD'
                                         union all
                                         SELECT 'GO', COUNT(1) FROM view_fhfjzt WHERE  zt='预离' 
                                         union all
                                         SELECT 'LR', COUNT(1) FROM view_fhfjzt WHERE  zt='锁房' 
                                         union all
                                         SELECT 'ALL', COUNT(1) FROM view_fhfjzt WHERE 1=1
            ");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql);
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                string GuestNum = dt.Rows.Count.ToString();
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["RoomState"] = dt.Rows[i]["ftcode"].ToString().Trim();//房间状态
                    jdList["RoomNumber"] = dt.Rows[i]["ftcount"].ToString().Trim(); //房态数量
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


        #region  ** 37. 数据统计 **

        public void DataStatistics(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);

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
            IList<SqlParam> lParam = new List<SqlParam>();
            sql.Append("select * from [XX_DQZTXXB] where 1=1 ");
            if (DataType > 1)
            {
                sql.Append(" AND jdid = @jdid ");
                lParam.Add(new SqlParam("@jdid", DataType));
            }
            sql.Append(" order by xh ");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                string GuestNum = dt.Rows.Count.ToString();
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["DataType"] = dt.Rows[i]["sjms"].ToString().Trim();//数据描述
                    jdList["Number"] = dt.Rows[i]["sl"].ToString().Trim(); //数量
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


        #region  ** 38. 客人类型 **


        public void GetGuestType(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];

            if (!DataFactory.CheckSqlIsOpen(adminHotelId, CommonHelper.GetInt(hotelId)))//数据库是否能连接
            {
                throw new PMSException("数据库连接失败,请验证网络是否正常");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [CS_KRLXFXB] WHERE krlxcode IN (select  krlx from view_fhfjzt WHERE krlx IS NOT NULL GROUP BY krlx)");
            DataTable dt = DataFactory.SqlDataBase(adminHotelId, hotelId).GetDataTableBySQL(sql);
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                string GuestNum = dt.Rows.Count.ToString();
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["GuestCode"] = dt.Rows[i]["krlxcode"].ToString().Trim();//客人类型编码
                    jdList["GuestTypeName"] = dt.Rows[i]["desc0"].ToString().Trim(); //客人类型名称
                    string GuestStyle = dt.Rows[i]["krlxcode"].ToString().Trim();
                    if (GuestStyle == "BTF")//白天房
                    {
                        GuestStyle = "btf";
                    }
                    else if (GuestStyle == "BK")//预订客人
                    {
                        GuestStyle = "ydk";
                    }
                    else if (GuestStyle == "HU")//自用房
                    {
                        GuestStyle = "zyf";
                    }
                    else if (GuestStyle == "CO")//协议公司客
                    {
                        GuestStyle = "xyk";
                    }
                    else if (GuestStyle == "OTA" || GuestStyle == "TG")//OTA客人
                    {
                        GuestStyle = "otakr";
                    }
                    else if (GuestStyle == "LXS" || GuestStyle == "LX")//旅行客
                    {
                        GuestStyle = "lxs";
                    }
                    else if (GuestStyle == "FR")//免费房
                    {
                        GuestStyle = "mff";
                    }
                    else if (GuestStyle == "JF")//积分换房
                    {
                        GuestStyle = "jfhf";
                    }
                    else if (GuestStyle == "LS")//长住客
                    {
                        GuestStyle = "czk";
                    }
                    else if (GuestStyle == "NET")//网络公司
                    {
                        GuestStyle = "wlgs";
                    }
                    else if (GuestStyle == "VIP")//VIP客人
                    {
                        GuestStyle = "vipkr";
                    }
                    else if (GuestStyle == "VVI")//金卡会员
                    {
                        GuestStyle = "jkhy";
                    }
                    else if (GuestStyle == "WI")//自来的散客
                    {
                        GuestStyle = "zldxk";
                    }
                    else if (GuestStyle == "WX")//公众号订单
                    {
                        GuestStyle = "wwkr";
                    }
                    else if (GuestStyle == "WYF")//午夜房
                    {
                        GuestStyle = "wyf";
                    }
                    else if (GuestStyle == "ZDF")//钟点房
                    {
                        GuestStyle = "zdf";
                    }
                    else if (GuestStyle == "ZDH")//招待房
                    {
                        GuestStyle = "zdh";
                    }
                    jdList["GuestStyle"] = GuestStyle; 
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


        #region  ** 39.	支付 **

        public void Pay(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string PayType = CommonUse.ParamVal(jd_data, "PayType", "支付类型", true, true);//1微信 2支付宝
            string PayMoney = CommonUse.ParamVal(jd_data, "PayMoney", "支付金额", true, true);
            string OrderNumber = CommonUse.ParamVal(jd_data, "OrderNumber", "订单编号", true, true);

            //参数判断
            if (string.IsNullOrEmpty(HotelCode))
                throw new PMSException("酒店编号不能为空");

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];

            Hashtable ht = new Hashtable();         

            JsonData jd = new JsonData();//返回JsonData
           // DataFactory.SqlDataBase().ExecuteByProcReturn("[SelfHelpPay]", ht);
            if (0==0)
            {

                jd["code"] = 1;
                jd["data"] = "";
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

        public void PayResult(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string OrderNumber = CommonUse.ParamVal(jd_data, "OrderNumber", "订单编号", true, true);
            string AccountCode = CommonUse.ParamVal(jd_data, "AccountCode", "账务代码", true, true);
            
            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length != 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }

            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];


            string Result = "0";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT PayState FROM SelfHelpPay WHERE OrderNumber = @OrderNumber");
            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@OrderNumber", OrderNumber));
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, lParam.ToArray());
            if (dt != null && dt.Rows.Count > 0)
            {
                Result = dt.Rows[0]["PayState"].ToString();
            }

            Hashtable ht = new Hashtable();

            JsonData jd = new JsonData();//返回JsonData
            // DataFactory.SqlDataBase().ExecuteByProcReturn("[SelfHelpPay]", ht);
            if (0 == 0)
            {
                //入账
                jd["code"] = 1;
                jd["data"] = "";
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