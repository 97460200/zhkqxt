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
    public class Demo
    {

        #region  ** 获取所有房价码 **

        public void GetRoomPriceCode(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }

            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);       
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

    }
}