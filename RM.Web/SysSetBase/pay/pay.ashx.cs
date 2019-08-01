using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using RM.Busines;
using System.Web.SessionState;
using RM.Common.DotNetBean;
using System.Text;
using System.Data;
using RM.Common.DotNetJson;
using RM.Common.DotNetCode;
using LitJson;
using Newtonsoft.Json;

namespace RM.Web.SysSetBase.pay
{
    /// <summary>
    /// pay1 的摘要说明
    /// </summary>
    public class pay1 : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Action = context.Request["action"].Trim();               //提交动作

            switch (Action)
            {
                case "getinfo":
                    getinfo(context);
                    break;
                case "update":
                    update(context);
                    break;
                case "GetRoomList":
                    GetRoomList(context);
                    break;
                case "SaveMoney":
                    SaveMoney(context);
                    break;
                default:
                    break;
            }
        }

        private void SaveMoney(HttpContext c)
        {
            try
            {
                string JsonDate = c.Request["JsonDate"];
                List<Hashtable> jd = JsonConvert.DeserializeObject<List<Hashtable>>(JsonDate);
                if (jd != null && jd.Count > 0)
                {
                    for (int i = 0; i < jd.Count; i++)
                    {
                        Hashtable ht = new Hashtable();
                        string roomId = jd[i]["RoomId"].ToString();
                        ht["CashPledgeMoney"] = jd[i]["CashPledgeMoney"].ToString();
                        int uc = DataFactory.SqlDataBase().UpdateByHashtable("Guestroom", "id", roomId, ht);
                        if (uc < 0)
                        {
                            c.Response.Write("-1");
                            return;
                        }
                    }
                }
                c.Response.Write("1");
            }
            catch (Exception)
            {
                c.Response.Write("");
            }
        }

        private void GetRoomList(HttpContext c)
        {
            try
            {
                string hotelid = c.Request["HotelId"];
                JsonData jsondata = new JsonData();
                StringBuilder sb = new StringBuilder();
                sb.Append(@"                SELECT  id ,
                        Name ,
                        ( SELECT TOP 1
                                    jg
                          FROM      dbo.GSMoeny
                          WHERE     GSID = Guestroom.ID
                                    AND type = 2
                        ) Price ,
                        CASE WHEN CashPledgeMoney <= 0
                             THEN ( SELECT TOP 1
                                            CashPledgeMoney
                                    FROM    dbo.Hotel_AdminParameter
                                    WHERE   AdminHotelId = Guestroom.AdminHotelid
                                  )
                             ELSE CashPledgeMoney
                        END CashPledgeMoney
                FROM    Guestroom
                WHERE   HotelID = @hotelid
                ORDER BY Sort DESC 
                ");
                List<SqlParam> ilistStr = new List<SqlParam>();
                ilistStr.Add(new SqlParam("@hotelid", hotelid));
                SqlParam[] param = ilistStr.ToArray();
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (ds != null && ds.Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        string room_ID = ds.Rows[i]["id"].ToString();
                        JsonData json1 = new JsonData();
                        json1["Id"] = room_ID;
                        json1["Name"] = ds.Rows[i]["name"].ToString();
                        json1["Price"] = ds.Rows[i]["Price"].ToString();
                        json1["CashPledgeMoney"] = ds.Rows[i]["CashPledgeMoney"].ToString();
                        jsondata.Add(json1);
                    }
                }
                string json = "";
                json = jsondata.ToJson();
                c.Response.Write(json);
            }
            catch
            {
                c.Response.Write("");
            }
        }

        public void getinfo(HttpContext context)
        {


            string sql = string.Format(@"SELECT ID,name,pay,Qtpay,Hypay,jfpay,JFZhoumo,JFJieri,yhqzhoumo,yhqjieri,mrzf,is_xz_Coupon,is_dj_Coupon  FROM  Hotel WHERE  DeleteMark=1  and AdminHotelid=@AdminHotelid order by sort desc ");

            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);

            string dtName = "Hotel";
            string json = JsonHelper.DataTableToJson(dt, dtName);
            context.Response.Write(json);

        }

        public void update(HttpContext context)
        {
            string Hotelid = context.Request["Hotelid"].Trim();
            string Pay = context.Request["Pay"].Trim();
            string Hypay = context.Request["Hypay"].Trim();
            string Jfpay = context.Request["Jfpay"].Trim();
            string Qtpay = context.Request["Qtpay"].Trim();
            string JFZhoumo = context.Request["JFZhoumo"].Trim();
            string JFJieri = context.Request["JFJieri"].Trim();
            string yhqzhoumo = context.Request["yhqzhoumo"].Trim();
            string yhqjieri = context.Request["yhqjieri"].Trim();
            string mrzf = context.Request["mrzf"].Trim();

            string is_dj_Coupon = context.Request["is_dj_Coupon"].Trim();
            string is_xz_Coupon = context.Request["is_xz_Coupon"].Trim();

            Hashtable ht = new Hashtable();
            ht["Pay"] = Pay;
            ht["Hypay"] = Hypay;
            ht["Jfpay"] = Jfpay;
            ht["Qtpay"] = Qtpay;
            ht["JFZhoumo"] = JFZhoumo;
            ht["JFJieri"] = JFJieri;
            ht["yhqzhoumo"] = yhqzhoumo;
            ht["yhqjieri"] = yhqjieri;
            ht["mrzf"] = mrzf;

            ht["is_dj_Coupon"] = is_dj_Coupon;
            ht["is_xz_Coupon"] = is_xz_Coupon;


            if (DataFactory.SqlDataBase().Submit_AddOrEdit("Hotel", "ID", Hotelid, ht))
            {
                try
                {
                    string bz = "";
                    bz += (Pay == "1" ? "微信支付," : "");
                    bz += (Hypay == "1" ? "会员卡支付," : "");
                    bz += (Jfpay == "1" ? "积分兑换," : "");
                    bz += (Qtpay == "1" ? "前台付款" : "");
                    bz = bz.Trim(',');
                    CommonMethod.Base_Log("修改支付方式", "Hotel", Hotelid, "修改支付方式", bz);//操作记录
                }
                catch { }
                context.Response.Write("ok");

            }
            else
                context.Response.Write("error");

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}