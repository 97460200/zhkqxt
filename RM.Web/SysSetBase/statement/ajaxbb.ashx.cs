using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using RM.Common.DotNetCode;
using System.Text;
using RM.Busines;
using System.Data;
using LitJson;
using System.Collections;
using RM.Common.DotNetBean;
using RM.Web.WX_SET;
using RM.Common.DotNetJson;
using Newtonsoft.Json;
using RM.Common.DotNetData;

namespace RM.Web.SysSetBase.statement
{
    /// <summary>
    /// ajaxbb 的摘要说明
    /// </summary>
    public class ajaxbb : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Menu = context.Request["Menu"];                      //提交动作
            switch (Menu)
            {
                case "Getkftj":  //客房统计报表
                    Getkftj(context);
                    break;
                case "Getkfmx":  //客房明细报表
                    Getkfmx(context);
                    break;
                case "Getkqtj":  //卡券统计报表
                    Getkqtj(context);
                    break;
                case "Getkqmx":  //卡券明细报表
                    Getkqmx(context);
                    break;
                case "Gethytj": //会员
                    Gethytj(context);
                    break;
                case "Gethztj":
                    Gethztj(context);
                    break;
                case "Getzftj":
                    Getzftj(context);
                    break;
                case "Getzfjytj":
                    Getzfjytj(context);
                    break;
                case "Getfxtj":
                    Getfxtj(context);
                    break;
                case "Getygtj":
                    Getygtj(context);
                    break;
                case "R_T_SaleMoney":
                    R_T_SaleMoney(context);
                    break;
                case "R_T_SaleNight":
                    R_T_SaleNight(context);
                    break;
                case "R_T_HourCount":
                    R_T_HourCount(context);
                    break;
                case "R_T_WeekCount":
                    R_T_WeekCount(context);
                    break;
                case "GetCouponList":
                    GetCouponList(context);
                    break;
                case "person_sex":
                    person_sex(context);
                    break;
                case "pay_name":
                    pay_name(context);
                    break;
                case "R_T_OTTCount":
                    R_T_OTTCount(context);
                    break;
                case "MemberReport":
                    MemberReport(context);
                    break;
                case "IntegralReport":
                    IntegralReport(context);
                    break;
                case "IntegralReports":
                    IntegralReports(context);
                    break;
                    
                    
                    
                default:
                    break;
            }
        }

        private void Getkftj(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string hotelid = context.Request["hotelid"];
            string kfid = context.Request["kfid"];
            string zflx = context.Request["zflx"];
            string rqlx = context.Request["rqlx"];
            string kssj = context.Request["kssj"];
            string jssj = context.Request["jssj"];
            string gjc = context.Request["gjc"];

            JsonData jsondata = new JsonData();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT h.name as hname,g.Name AS gname, ru.Rule_Name,
                        ISNULL(SUM(r.TomePrice),0) AS TomePrice , ISNULL(round(AVG(r.RoomTotal),1),0) AS RoomTotal,
                        ISNULL(SUM(DATEDIFF( Day, r.BeginTime,r.EndTime)*r.Number),0) AS fws,
                        ISNULL((SUM(DATEDIFF( Day, r.BeginTime,r.EndTime)*r.Number)-COUNT(DISTINCT r.openid)),0) AS htfws,
                        ISNULL((CAST((SUM(DATEDIFF( Day, r.BeginTime,r.EndTime)*r.Number)-COUNT(DISTINCT r.openid))*1.0/(CASE SUM(DATEDIFF( Day,r.BeginTime,r.EndTime)*r.Number) WHEN 0 THEN 1 ELSE SUM(DATEDIFF( Day,r.BeginTime,r.EndTime)*r.Number) END)*100 as decimal)),0) AS htl,h.ID AS hid,g.ID AS gid 
                        FROM Hotel h LEFT JOIN Guestroom g ON g.HotelID=h.ID 
                        LEFT JOIN Room_Rule ru ON ru.Room_ID=g.ID
                        LEFT JOIN	 Reservation r ON ru.id=r.RuleId
                        WHERE h.AdminHotelid=@AdminHotelid ");
            
            List<SqlParam> ilistStr = new List<SqlParam>();
            ilistStr.Add(new SqlParam("@AdminHotelid", AdminHotelid));
            
           
            if (hotelid!=null&&hotelid != "" && hotelid != "0")
            {
                sb.Append(" AND h.ID = @hotelid ");
                ilistStr.Add(new SqlParam("@hotelid", hotelid.Trim()));
            }
            if (kfid!=null&&kfid != "" && kfid != "0")
            {
                sb.Append(" AND g.ID = @kfid ");
                ilistStr.Add(new SqlParam("@kfid", kfid.Trim()));
            }
            if (zflx!=null&&zflx != "" && zflx != "0")
            {
                sb.Append(" AND r.PayType = @zflx ");
                ilistStr.Add(new SqlParam("@zflx", zflx.Trim()));
            }
            if (gjc!=null&&gjc != "" && gjc != "0")
            {
                sb.AppendFormat(" AND (h.name LIKE '%{0}%' OR g.Name LIKE '%{0}%' OR ru.Rule_Name LIKE '%{0}%') ", gjc.Trim());
            }
            if (rqlx == "1") 
            {
                if (kssj != "") 
                {
                    sb.Append(" AND r.AddTime>= @kssj ");
                    ilistStr.Add(new SqlParam("@kssj", kssj.Trim()));
                }
                if (jssj != "")
                {
                    sb.Append(" AND r.AddTime<= @jssj ");
                    ilistStr.Add(new SqlParam("@jssj", jssj.Trim()+" 23:59"));
                }
            }
            else if (rqlx == "2") 
            {

            }
            else if (rqlx == "3")
            {

            }
            else if (rqlx == "4")
            {

            }
            else if (rqlx == "5")
            {

            }
            StringBuilder html = new StringBuilder();
            SqlParam[] param = ilistStr.ToArray();
            sb.Append(@" GROUP BY r.Adult,ru.ID,ru.Rule_Name,h.name,g.Name,h.sort,g.Sort,h.ID,g.ID ORDER BY h.sort,g.Sort");
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (ds != null && ds.Rows.Count > 0)
            {
                int xh = 0;
                
                
                string hid = string.Empty;
                string gid = string.Empty;
                int TomePrice = 0;
                int RoomTotal = 0;
                int fws = 0;
                int htfws = 0;
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    //<tr> <td>1</td><td>前厅部</td><td>里静</td><td>1000</td><td>1000.00</td><td>10%</td><td>1000</td><td>10% </td><td>10%</td><td>10%</td>
                    TomePrice += Convert.ToInt32(ds.Rows[i]["TomePrice"]);
                    RoomTotal += Convert.ToInt32(ds.Rows[i]["RoomTotal"]);
                    fws += Convert.ToInt32(ds.Rows[i]["fws"]);
                    htfws += Convert.ToInt32(ds.Rows[i]["htfws"]);
                    StringBuilder tr = new StringBuilder();
                    tr.AppendFormat("<tr>");
                    DataView dv = new DataView(ds);
                    dv.RowFilter = " hid = '" + ds.Rows[i]["hid"].ToString() + "' ";
                    if (hid != ds.Rows[i]["hid"].ToString())
                    {
                        xh++;
                        tr.AppendFormat("<td rowspan='" + dv.Count + "'>{0}</td>", xh);
                        tr.AppendFormat("<td rowspan='" + dv.Count + "'>" + ds.Rows[i]["hname"] + " </td>");//分店名称
                    }
                    DataView dvs = new DataView(ds);
                    dvs.RowFilter = " gid = '" + ds.Rows[i]["gid"].ToString() + "' ";
                    if (gid != ds.Rows[i]["gid"].ToString())
                    {
                        tr.AppendFormat("<td rowspan='" + dvs.Count + "'>" + ds.Rows[i]["gname"] + " </td>");//房型名称
                    }

                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["Rule_Name"]);
                    tr.AppendFormat("<td>{0}</td>", Convert.ToInt32(ds.Rows[i]["fws"]));
                    tr.AppendFormat("<td>{0}</td>", Convert.ToInt32(ds.Rows[i]["TomePrice"]));
                    tr.AppendFormat("<td>{0}</td>", Convert.ToInt32(ds.Rows[i]["RoomTotal"]));
                    
                    tr.AppendFormat("<td>{0}</td>", Convert.ToInt32(ds.Rows[i]["htfws"]));
                    tr.AppendFormat("<td>{0}%</td>", Convert.ToInt32(ds.Rows[i]["htl"]));
                    //tr.AppendFormat("<td>{0}%</td>", 0);
                    tr.AppendFormat("</tr>");
                    hid = ds.Rows[i]["hid"].ToString();
                    gid = ds.Rows[i]["gid"].ToString();
                    html.Append(tr);
                    //JsonData jsongz = new JsonData();
                    //jsongz["Rule_Name"] = ds.Rows[i]["Rule_Name"].ToString();
                    //jsongz["TomePrice"] = Convert.ToInt32(ds.Rows[i]["TomePrice"].ToString());
                    //jsongz["RoomTotal"] = Convert.ToInt32(ds.Rows[i]["RoomTotal"].ToString());
                    //jsongz["fws"] = Convert.ToInt32(ds.Rows[i]["fws"].ToString());
                    //jsongz["htfws"] = Convert.ToInt32(ds.Rows[i]["htfws"].ToString());
                    //jsongz["htl"] = Convert.ToInt32(ds.Rows[i]["htl"].ToString());
                    //jsongroom["glist"].Add(jsongz);

                    //if (i == 0 || ds.Rows[i]["gid"].ToString() != ds.Rows[i - 1]["gid"].ToString())
                    //{
                    //    jsongroom["gname"] = ds.Rows[i]["gname"].ToString();

                    //    if (i != 0) 
                    //    {
                    //        jsonhotel["hlist"].Add(jsongroom);
                    //        jsongroom = new JsonData();
                    //        jsongroom["glist"] = new JsonData();
                    //    }
                    //}

                    //if (i == 0 || ds.Rows[i]["hid"].ToString() != ds.Rows[i - 1]["hid"].ToString())
                    //{
                    //    xh++;
                    //    jsonhotel = new JsonData();
                    //    jsonhotel["Xh"] = xh;
                    //    jsonhotel["Name"] = ds.Rows[i]["hname"].ToString();

                    //    if (i != 0) 
                    //    { 
                    //        jsondata.Add(jsonhotel);
                    //        jsonhotel = new JsonData();
                    //        jsonhotel["hlist"] = new JsonData();
                    //    }
                    //}
                }

                //<tr><td></td><td style='font-weight:bold;'>合计</td><td></td><td></td><td class='color01'>1000</td><td class='color01'>10000.00</td><td class='color01'>10000.00  </td><td class='color01'>1000</td><td class='color01'>1000</td><td></td></tr>
                html.AppendFormat("<tr><td></td><td style='font-weight:bold;'>合计</td><td></td><td></td><td class='color01'>{0}</td><td class='color01'>{1}</td><td class='color01'>{2}  </td><td class='color01'>{3}</td><td class='color01'>{4}%</td></tr>", fws, TomePrice, RoomTotal, htfws, Convert.ToInt32(htfws * 1.0 / fws * 100));

                //string json = "";
                //json = jsondata.ToJson();
                
            }
            context.Response.Write(html);

        }


        private void Getkfmx(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string hotelid = context.Request["hotelid"];
            string kfid = context.Request["kfid"];
            string zflx = context.Request["zflx"];
            string rqlx = context.Request["rqlx"];
            string kssj = context.Request["kssj"];
            string jssj = context.Request["jssj"];
            string gjc = context.Request["gjc"];

            JsonData jsondata = new JsonData();
            StringBuilder sb = new StringBuilder();
            sb.Append(@" SELECT hotelname,RoomType,rule_name,AddTime,OrderNum,State,MemberId,Name,Mobile,ISNULL(DATEDIFF( Day, BeginTime,EndTime)*Number,0) AS fws,TomePrice,payname,Mobile as sj FROM dbo.V_Reservations WHERE AdminHotelid=@AdminHotelid ");

            List<SqlParam> ilistStr = new List<SqlParam>();
            ilistStr.Add(new SqlParam("@AdminHotelid", AdminHotelid));


            if (hotelid != null && hotelid != "" && hotelid != "0")
            {
                sb.Append(" AND HotelId = @hotelid ");
                ilistStr.Add(new SqlParam("@hotelid", hotelid.Trim()));
            }
            if (kfid != null && kfid != "" && kfid != "0")
            {
                sb.Append(" AND roomid = @kfid ");
                ilistStr.Add(new SqlParam("@kfid", kfid.Trim()));
            }
            if (zflx != null && zflx != "" && zflx != "0")
            {
                sb.Append(" AND Pay_Type = @zflx ");
                ilistStr.Add(new SqlParam("@zflx", zflx.Trim()));
            }
            if (gjc != null && gjc != "" && gjc != "0")
            {
                sb.AppendFormat(" AND (name LIKE '%{0}%' OR RoomType LIKE '%{0}%' OR rule_name LIKE '%{0}%') ", gjc.Trim());

            }

            if (rqlx == "1")
            {
                if (kssj != "")
                {
                    sb.Append(" AND AddTime>= @kssj ");
                    ilistStr.Add(new SqlParam("@kssj", kssj.Trim()));
                }
                if (jssj != "")
                {
                    sb.Append(" AND AddTime<= @jssj ");
                    ilistStr.Add(new SqlParam("@jssj", jssj.Trim() + " 23:59"));
                }
            }
            else if (rqlx == "2")
            {

            }
            else if (rqlx == "3")
            {

            }
            else if (rqlx == "4")
            {

            }
            else if (rqlx == "5")
            {

            }
            StringBuilder html = new StringBuilder();
            SqlParam[] param = ilistStr.ToArray();
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (ds != null && ds.Rows.Count > 0)
            {
                int xh = 0;
                
                
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    html.AppendFormat("<tr>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(i+1+"");
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][0].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][1].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][2].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat( Convert.ToDateTime( ds.Rows[i][3]).ToString("yyyy-MM-dd HH:mm"));
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][4].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][5].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][6].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][7].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][8].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][9].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][10].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][11].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("<td>");
                    html.AppendFormat(ds.Rows[i][12].ToString());
                    html.AppendFormat("</td>");
                    html.AppendFormat("</tr>");
                    
                }
                
               
            }

            context.Response.Write(html);
        }
        
        //卡券统计
        private void Getkqtj(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string hotelid = context.Request["hotelid"];
            string Couponid = context.Request["CouponList"];
            
            string rqlx = context.Request["rqlx"];
            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];
            //StartDate = StartDate == "" ? "2018-01-01" : StartDate;
            //EndDate = EndDate == "" ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate;
            string gjc = context.Request["gjc"];

            JsonData jsondata = new JsonData();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT c.CouponName,p.TypeName,Total,c.Par,c.addtime,c.BiginTime,c.EndinTime,COUNT(1) AS lqs,
CAST(( CAST(COUNT(1) AS DECIMAL(18, 2))/ CAST((CASE Total WHEN 0 THEN 1 ELSE Total END) AS DECIMAL(18, 2)) ) AS DECIMAL(18,2))* 100 AS lql,
(SELECT COUNT(1) FROM ClientCoupon WHERE isReceive=1 AND CouponID=c.ID {0}) AS sys, 
CAST(( CAST((SELECT COUNT(1) FROM ClientCoupon WHERE isReceive=1 AND CouponID=c.ID {0}) AS DECIMAL(18, 2))/ CAST((CASE COUNT(1) WHEN 0 THEN 1 ELSE COUNT(1) END) AS DECIMAL(18, 2)) ) AS DECIMAL(18,2))* 100 AS syl
FROM dbo.Coupon c LEFT JOIN ClientCoupon cl ON cl.CouponID=c.ID LEFT JOIN dbo.PreferentialType p ON p.ID=c.Mode WHERE c.AdminHotelid=@AdminHotelid  and c.DeleteMark=1");

            List<SqlParam> ilistStr = new List<SqlParam>();
            ilistStr.Add(new SqlParam("@AdminHotelid", AdminHotelid));



            if (Couponid != null && Couponid != "" && Couponid != "0")
            {
                sb.Append(" AND c.ID = @Couponid ");
                ilistStr.Add(new SqlParam("@Couponid", Couponid.Trim()));
            }
            
            if (gjc != null && gjc != "" && gjc != "0")
            {
                sb.AppendFormat(" AND (c.CouponName LIKE '%{0}%' OR p.TypeName LIKE '%{0}%' ) ", gjc.Trim());

            }
            string addtime = "";
            if (StartDate != "") 
            {
                sb.AppendFormat(" AND cl.addtime>='{0}' ", StartDate.Trim());
                addtime += String.Format(" AND addtime>='{0}' ", StartDate.Trim());
            }
            if (EndDate != "")
            {
                sb.AppendFormat(" AND cl.addtime<='{0}' ", EndDate.Trim());
                addtime += String.Format(" AND addtime<='{0}' ", EndDate.Trim());
            }
            sb = new StringBuilder(String.Format(sb.ToString(), addtime));
            SqlParam[] param = ilistStr.ToArray();
            sb.Append(@" GROUP BY c.ID,c.CouponName,p.TypeName,Total,c.Par,c.addtime,c.BiginTime,c.EndinTime");
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (ds != null && ds.Rows.Count > 0)
            {
                int xh = 0;
                int Total = 0;
                int lqs = 0;
                int sys = 0;
                
                StringBuilder html = new StringBuilder();
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    Total += Convert.ToInt32(ds.Rows[i]["Total"]);
                    lqs += Convert.ToInt32(ds.Rows[i]["lqs"]);
                    sys += Convert.ToInt32(ds.Rows[i]["sys"]);
                    StringBuilder tr = new StringBuilder();
                    tr.AppendFormat("<tr>");
                    tr.AppendFormat("<td>{0}</td>", i+1);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["CouponName"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["TypeName"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["Par"]);
                    tr.AppendFormat("<td>{0}</td>", Convert.ToDateTime(ds.Rows[i]["addtime"]).ToString("yyyy-MM-dd"));
                    tr.AppendFormat("<td>{0}</td>", Convert.ToDateTime(ds.Rows[i]["BiginTime"]).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(ds.Rows[i]["EndinTime"]).ToString("yyyy-MM-dd"));
                    tr.AppendFormat("<td>{0}</td>", Convert.ToInt32(ds.Rows[i]["Total"]));
                    tr.AppendFormat("<td>{0}</td>", Convert.ToInt32(ds.Rows[i]["lqs"]));
                    tr.AppendFormat("<td>{0}%</td>", Convert.ToInt32(ds.Rows[i]["lql"]));
                    tr.AppendFormat("<td>{0}</td>", Convert.ToInt32(ds.Rows[i]["sys"]));
                    tr.AppendFormat("<td>{0}%</td>", Convert.ToInt32(ds.Rows[i]["syl"]));
                    tr.AppendFormat("</tr>");
                    html.Append(tr);
                    
                }

                html.AppendFormat("<tr><td></td><td style='font-weight:bold;'>合计</td><td></td><td></td><td></td><td></td><td class='color01'>{0}</td><td class='color01'>{1}</td><td class='color01'>{2}%  </td><td class='color01'>{3}</td><td class='color01'>{4}%</td></tr>", Total, lqs, Convert.ToInt32(lqs * 1.0 / Total * 100), sys, Convert.ToInt32(sys * 1.0 / lqs * 100));

                //string json = "";
                //json = jsondata.ToJson();
                context.Response.Write(html);
            }


        }

        //卡券明细
        private void Getkqmx(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string hotelid = context.Request["hotelid"];
            string Couponid = context.Request["CouponList"];

            string rqlx = context.Request["rqlx"];
            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];
            string gjc = context.Request["gjc"];

            JsonData jsondata = new JsonData();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT CouponName,type,Par,EffectiveDateTime,endinTime,xm,sjhm,hotelname,RoomType,raddtime,OrderNum,isend,isReceive FROM dbo.v_ClientCoupon WHERE AdminHotelid=@AdminHotelid  ");

            List<SqlParam> ilistStr = new List<SqlParam>();
            ilistStr.Add(new SqlParam("@AdminHotelid", AdminHotelid));



            if (Couponid != null && Couponid != "" && Couponid != "0")
            {
                sb.Append(" AND CouponID = @CouponID ");
                ilistStr.Add(new SqlParam("@CouponID", Couponid.Trim()));
            }

            if (gjc != null && gjc != "" && gjc != "0")
            {
                sb.AppendFormat(" AND (CouponName LIKE '%{0}%' OR type LIKE '%{0}%' OR xm LIKE '%{0}%' OR sjhm LIKE '%{0}%' OR RoomType LIKE '%{0}%') ", gjc.Trim());

            }

            if (StartDate != "")
            {
                sb.AppendFormat(" AND addtime>='{0}' ", StartDate.Trim());
            }
            if (EndDate != "")
            {
                sb.AppendFormat(" AND addtime<='{0}' ", EndDate.Trim());
            }
            SqlParam[] param = ilistStr.ToArray();
            
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (ds != null && ds.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    StringBuilder tr = new StringBuilder();
                    tr.AppendFormat("<tr>");
                    tr.AppendFormat("<td>{0}</td>", i + 1);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["CouponName"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["type"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["Par"]);
                    tr.AppendFormat("<td>{0}</td>", Convert.ToDateTime(ds.Rows[i]["EffectiveDateTime"]).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(ds.Rows[i]["endinTime"]).ToString("yyyy-MM-dd"));
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["xm"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["sjhm"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["hotelname"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["RoomType"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["isReceive"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["raddtime"].ToString()==""?"":Convert.ToDateTime(ds.Rows[i]["raddtime"]).ToString("yyyy-MM-dd"));
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["OrderNum"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["isend"]);
                    
                    tr.AppendFormat("</tr>");
                    html.Append(tr);

                }

                context.Response.Write(html);
            }


        }


        //会员统计
        private void Gethytj(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            
            

            
            string kssj = context.Request["kssj"];
            string jssj = context.Request["jssj"];
            string gjc = context.Request["gjc"];

            JsonData jsondata = new JsonData();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT m.hylxname,COUNT(x.lsh) AS rs, 
CAST(( CAST(COUNT(x.lsh) AS DECIMAL(18, 2))/ CAST((SELECT COUNT(lsh) FROM hy_hyzlxxb WHERE 1=1) AS DECIMAL(18, 2)) ) AS DECIMAL(18,2))* 100 AS zb,
ISNULL(SUM(x.czze),0)  AS czze,
ISNULL(AVG(x.czze),0) AS czpj,
ISNULL(SUM(x.xfze),0) AS xfze,
ISNULL(AVG(x.xfze),0) AS xfpj,
ISNULL(SUM(x.hykye),0) AS ye
FROM dbo.hy_hylxbmb m LEFT JOIN (
SELECT 
lsh,
hylx,
ISNULL((select SUM(je) from SY_KRXFMXB WHERE jdflag=1 AND zhlb='V' AND zh=lsh),0) AS czze,
ISNULL((select SUM(je) from SY_KRXFMXB WHERE jdflag=0 AND zhlb='V' AND zh=lsh),0) AS xfze,
ISNULL(hykye,0) AS hykye
FROM dbo.hy_hyzlxxb ) AS x
ON REPLACE(m.hylxcode,' ','')=REPLACE(x.hylx,' ','') WHERE 1=1 ");

            //List<SqlParam> ilistStr = new List<SqlParam>();
            //ilistStr.Add(new SqlParam("@AdminHotelid", AdminHotelid));


            
            //SqlParam[] param = ilistStr.ToArray();
            sb.Append(@" GROUP BY m.hylxname ");
            DataTable ds = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sb);
            if (ds != null && ds.Rows.Count > 0)
            {
                int rs = 0;
                int zb = 0;
                int czze = 0;
                int czpj = 0;
                int xfze = 0;
                int xfpj = 0;
                int ye = 0;


                StringBuilder html = new StringBuilder();
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    rs += Convert.ToInt32(ds.Rows[i]["rs"]);
                    czze += Convert.ToInt32(ds.Rows[i]["czze"]);
                    xfze += Convert.ToInt32(ds.Rows[i]["xfze"]);
                    ye += Convert.ToInt32(ds.Rows[i]["ye"]);
                    zb = 100;
                    czpj = (czpj + Convert.ToInt32(ds.Rows[i]["czpj"])) / 2;
                    xfpj = (xfpj + Convert.ToInt32(ds.Rows[i]["xfpj"])) / 2;
                    StringBuilder tr = new StringBuilder();
                    tr.AppendFormat("<tr>");
                    tr.AppendFormat("<td>{0}</td>", i + 1);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["hylxname"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["rs"]);
                    tr.AppendFormat("<td>{0}%</td>", ds.Rows[i]["zb"]);
                    tr.AppendFormat("<td>{0}</td>",ds.Rows[i]["czze"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["czpj"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["xfze"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["xfpj"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["ye"]);
                    
                    tr.AppendFormat("</tr>");
                    html.Append(tr);

                }

                html.AppendFormat("<tr><td></td><td style='font-weight:bold;'>合计</td><td class='color01'>{0}</td><td class='color01'>{1}</td><td class='color01'>{2}</td><td class='color01'>{3}</td><td class='color01'>{4}</td><td class='color01'>{5}</td><td class='color01'>{6}</td>",rs,zb+"%",czze,czpj,xfze,xfpj,ye);

                //string json = "";
                //json = jsondata.ToJson();
                context.Response.Write(html);
            }


        }


        //汇总EXEC P_Report_HZ_HZ 0,'1008337'
        private void Gethztj(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];

            string hotelid = context.Request["Hotelid"];

            if (hotelid == null || hotelid == "") 
            {
                hotelid = "0";
            }

            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];

           
            
            

            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();
            ht["HotelId"] = hotelid;
            ht["AdminHotelid"] = AdminHotelid;
            ht["StartDate"] = DateTime.Parse(StartDate == null ? DateTime.Now.ToString("yyyy-MM-01") : StartDate);
            ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate);
            
            DataTable ds = DataFactory.SqlDataBase().GetDataTableProc("P_Report_HZ_HZ", ht);
            
            if (ds != null && ds.Rows.Count > 0)
            {
               


                StringBuilder html = new StringBuilder();
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    
                    StringBuilder tr = new StringBuilder();
                    tr.AppendFormat("<tr>");
                    tr.AppendFormat("<td>{0}</td>", i + 1);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["bt"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["hys"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["xse"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["jys"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["czze"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["hykxf"]);
                    

                    tr.AppendFormat("</tr>");
                    html.Append(tr);

                }

               
                context.Response.Write(html);
            }


        }

        private void Getzftj(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];

            string hotelid = context.Request["Hotelid"];
            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];
            if (hotelid == null || hotelid == "")
            {
                hotelid = "0";
            }


            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();
            ht["HotelId"] = hotelid;
            ht["AdminHotelid"] = AdminHotelid;
            ht["StartDate"] = DateTime.Parse(StartDate == null ? DateTime.Now.ToString("yyyy-MM-01") : StartDate);
            ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableProc("P_Report_HZ_ZF", ht);

            if (ds != null && ds.Rows.Count > 0)
            {



                StringBuilder html = new StringBuilder();
                for (int i = 0; i < ds.Rows.Count; i++)
                {

                    StringBuilder tr = new StringBuilder();
                    tr.AppendFormat("<tr>");
                    tr.AppendFormat("<td>{0}</td>", i + 1);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["payname"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["num"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["TomePrice"]);
              


                    tr.AppendFormat("</tr>");
                    html.Append(tr);

                }


                context.Response.Write(html);
            }


        }

        private void Getzfjytj(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];

            string hotelid = context.Request["Hotelid"];
            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];
            if (hotelid == null || hotelid == "")
            {
                hotelid = "0";
            }


            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();
            ht["HotelId"] = hotelid;
            ht["AdminHotelid"] = AdminHotelid;
            ht["StartDate"] = DateTime.Parse(StartDate == null ? DateTime.Now.ToString("yyyy-MM-01") : StartDate);
            ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableProc("P_Report_HZ_ZFJY", ht);

            if (ds != null && ds.Rows.Count > 0)
            {



                StringBuilder html = new StringBuilder();
                for (int i = 0; i < ds.Rows.Count; i++)
                {

                    StringBuilder tr = new StringBuilder();
                    tr.AppendFormat("<tr>");
                    tr.AppendFormat("<td>{0}</td>", i + 1);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["payname"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["num"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["TomePrice"]);



                    tr.AppendFormat("</tr>");
                    html.Append(tr);

                }


                context.Response.Write(html);
            }


        }

        
        private void Getfxtj(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];

            string hotelid = context.Request["Hotelid"];
            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];
            if (hotelid == null || hotelid == "")
            {
                hotelid = "0";
            }


            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();
            ht["HotelId"] = hotelid;
            ht["AdminHotelid"] = AdminHotelid;
            ht["StartDate"] = DateTime.Parse(StartDate == null ? DateTime.Now.ToString("yyyy-MM-01") : StartDate);
            ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableProc("P_Report_HZ_FX", ht);

            if (ds != null && ds.Rows.Count > 0)
            {



                StringBuilder html = new StringBuilder();
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    StringBuilder tr = new StringBuilder();
                    if (i == ds.Rows.Count - 1)
                    {
                        
                        tr.AppendFormat("<tr>");
                        tr.AppendFormat("<td>{0}</td>", i + 1);
                        tr.AppendFormat("<td style='font-weight:bold;'>{0}</td>", ds.Rows[i]["gname"]);
                        tr.AppendFormat("<td></td>", ds.Rows[i]["Rule_Name"]);
                        tr.AppendFormat("<td class='color01'>{0}</td>", ds.Rows[i]["fws"]);
                        tr.AppendFormat("<td></td>", ds.Rows[i]["fwl"]);
                        tr.AppendFormat("<td class='color01'>{0}</td>", ds.Rows[i]["TomePrice"]);
                        tr.AppendFormat("<td></td>", ds.Rows[i]["Tl"]);
                        tr.AppendFormat("<td class='color01'>{0}</td>", ds.Rows[i]["RoomTotal"]);
                        tr.AppendFormat("</tr>");
                        html.Append(tr);

                        break;
                    }
                    
                    tr.AppendFormat("<tr>");
                    tr.AppendFormat("<td>{0}</td>", i + 1);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["gname"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["Rule_Name"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["fws"]);
                    tr.AppendFormat("<td>{0}%</td>", ds.Rows[i]["fwl"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["TomePrice"]);
                    tr.AppendFormat("<td>{0}%</td>", ds.Rows[i]["Tl"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["RoomTotal"]);
                    tr.AppendFormat("</tr>");
                    html.Append(tr);

                    

                }


                context.Response.Write(html);
            }


        }

        //[P_Report_HZ_YG]
        private void Getygtj(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];

            string hotelid = context.Request["Hotelid"];
            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];
            if (hotelid == null || hotelid == "")
            {
                hotelid = "0";
            }


            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();
            ht["HotelId"] = hotelid;
            ht["AdminHotelid"] = AdminHotelid;
            ht["StartDate"] = DateTime.Parse(StartDate == null ? DateTime.Now.ToString("yyyy-MM-01") : StartDate);
            ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableProc("P_Report_HZ_YG", ht);

            if (ds != null && ds.Rows.Count > 0)
            {



                StringBuilder html = new StringBuilder();
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    StringBuilder tr = new StringBuilder();
                    //if (i == ds.Rows.Count - 1)
                    //{

                    //    tr.AppendFormat("<tr>");
                    //    tr.AppendFormat("<td>{0}</td>", i + 1);
                    //    tr.AppendFormat("<td style='font-weight:bold;'>{0}</td>", ds.Rows[i]["gname"]);
                    //    tr.AppendFormat("<td></td>", ds.Rows[i]["Rule_Name"]);
                    //    tr.AppendFormat("<td class='color01'>{0}</td>", ds.Rows[i]["fws"]);
                    //    tr.AppendFormat("<td></td>", ds.Rows[i]["fwl"]);
                    //    tr.AppendFormat("<td class='color01'>{0}</td>", ds.Rows[i]["TomePrice"]);
                    //    tr.AppendFormat("<td></td>", ds.Rows[i]["Tl"]);
                    //    tr.AppendFormat("<td class='color01'>{0}</td>", ds.Rows[i]["RoomTotal"]);
                    //    tr.AppendFormat("</tr>");
                    //    html.Append(tr);

                    //    break;
                    //}

                    tr.AppendFormat("<tr>");
                    tr.AppendFormat("<td>{0}</td>", i + 1);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["User_Name"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["SaleNight"]);
                    tr.AppendFormat("<td>{0}</td>", ds.Rows[i]["SalesMoney"]);
                    tr.AppendFormat("<td>{0}%</td>", ds.Rows[i]["Tl"]);
                    
                    tr.AppendFormat("</tr>");
                    html.Append(tr);



                }


                context.Response.Write(html);
            }


        }

        //销售额折线图
        private void R_T_SaleMoney(HttpContext context)
        {
            JsonData json1 = new JsonData();
            string AdminHotelid = context.Request["AdminHotelid"];

            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];

            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();
            
            ht["AdminHotelid"] = AdminHotelid;
            ht["StartDate"] = (StartDate == null ? "2018-01-01" : StartDate);
            ht["EndDate"] = (EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate);

            DataTable dtgg = DataFactory.SqlDataBase().GetDataTableProc("R_T_SaleMoney", ht);

            if (dtgg != null && dtgg.Rows.Count > 0)
            {
                
                    for (int j = 0; dtgg.Rows.Count > j; j++)
                    {
                        if (Convert.ToDateTime(dtgg.Rows[j]["data_time"].ToString()) <= DateTime.Now)
                        {
                            JsonData json2 = new JsonData();
                            if (dtgg.Rows.Count <= 31)
                            {
                                json2["every_time"] = Convert.ToDateTime(dtgg.Rows[j]["data_time"]).ToString("dd");
                            }
                            else 
                            {
                                json2["every_time"] = Convert.ToDateTime(dtgg.Rows[j]["data_time"]).ToString("MM-dd");
                            }
                            json2["val"] = CommonHelper.GetInt(dtgg.Rows[j]["val"].ToString());
                            json1.Add(json2);
                        }
                    }
                
            }

            string json = "";
            json = json1.ToJson();
            context.Response.Write(json);


        }


        //间夜、会员折线图
        private void R_T_SaleNight(HttpContext context)
        {
            JsonData json1 = new JsonData();
            string AdminHotelid = context.Request["AdminHotelid"];

            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];

            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();

            ht["AdminHotelid"] = AdminHotelid;
            ht["StartDate"] = DateTime.Parse(StartDate == null ? "2018-01-01" : StartDate).ToString("yyyy-MM-01");
            ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate).AddMonths(1).ToString("yyyy-MM-01");

            DataTable dtgg = DataFactory.SqlDataBase().GetDataTableProc("R_T_SaleNight", ht);

            if (dtgg != null && dtgg.Rows.Count > 0)
            {

                for (int j = 0; dtgg.Rows.Count > j; j++)
                {
                    if (Convert.ToDateTime(dtgg.Rows[j]["data_time"].ToString()) <= DateTime.Now)
                    {
                        JsonData json2 = new JsonData();
                        
                        json2["every_time"] = Convert.ToDateTime(dtgg.Rows[j]["data_time"]).ToString("yyyy-MM");
                        json2["SaleNight"] = CommonHelper.GetInt(dtgg.Rows[j]["SaleNight"].ToString());
                        json2["ComeBackNight"] = CommonHelper.GetInt(dtgg.Rows[j]["ComeBackNight"].ToString());
                        json2["Members"] = CommonHelper.GetInt(dtgg.Rows[j]["Members"].ToString());
                        json1.Add(json2);
                    }
                }

            }

            string json = "";
            json = json1.ToJson();
            context.Response.Write(json);


        }

        //小时统计间夜
        private void R_T_HourCount(HttpContext context)
        {
            JsonData json1 = new JsonData();
            string AdminHotelid = context.Request["AdminHotelid"];

            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];

            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();

            ht["AdminHotelid"] = AdminHotelid;
            ht["StartDate"] = DateTime.Parse(StartDate == null ? "2018-01-01" : StartDate).ToString("yyyy-MM-01");
            ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate).AddMonths(1).ToString("yyyy-MM-01");

            DataTable dtgg = DataFactory.SqlDataBase().GetDataTableProc("R_T_HourCount", ht);

            if (dtgg != null && dtgg.Rows.Count > 0)
            {

                for (int j = 0; dtgg.Rows.Count > j; j++)
                {
                    
                        JsonData json2 = new JsonData();

                        json2["every_time"] = dtgg.Rows[j]["date_time"].ToString();
                        json2["count"] = CommonHelper.GetInt(dtgg.Rows[j]["count"].ToString());
                        
                        json1.Add(json2);
                    
                }

            }

            string json = "";
            json = json1.ToJson();
            context.Response.Write(json);


        }

        //星期统计间夜
        private void R_T_WeekCount(HttpContext context)
        {
            JsonData json1 = new JsonData();
            string AdminHotelid = context.Request["AdminHotelid"];

            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];

            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();

            ht["AdminHotelid"] = AdminHotelid;
            //ht["StartDate"] = DateTime.Parse(StartDate == null ? "2018-01-01" : StartDate).ToString("yyyy-MM-01");
            //ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate).AddMonths(1).ToString("yyyy-MM-01");
            ht["StartDate"] = DateTime.Parse(StartDate == null ? "2018-01-01" : StartDate);
            ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate);

            DataTable dtgg = DataFactory.SqlDataBase().GetDataTableProc("R_T_WeekCount", ht);

            if (dtgg != null && dtgg.Rows.Count > 0)
            {

                for (int j = 0; dtgg.Rows.Count > j; j++)
                {

                    JsonData json2 = new JsonData();

                    json2["every_time"] = getWeek(dtgg.Rows[j]["date_time"].ToString());
                    json2["count"] = CommonHelper.GetInt(dtgg.Rows[j]["count"].ToString());

                    json1.Add(json2);

                }

            }

            string json = "";
            json = json1.ToJson();
            context.Response.Write(json);


        }



            //person_sex
        private void person_sex(HttpContext context)
        {
            JsonData json1 = new JsonData();
            string AdminHotelid = context.Request["AdminHotelid"];

            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];

            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();

            ht["AdminHotelid"] = AdminHotelid;
            ht["StartDate"] = DateTime.Parse(StartDate == null ? "2018-01-01" : StartDate).ToString("yyyy-MM-01");
            ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate).AddMonths(1).ToString("yyyy-MM-01");

            StringBuilder sql=new StringBuilder();
            sql.AppendFormat(@"declare @sumPerson float
	set @sumPerson=cast((select count(*) from hy_hyzlxxb  where carid IS NOT NULL AND (xb='F' OR xb='M'))  as float)
	select CASE xb WHEN 'F' THEN '女' ELSE '男' END name ,
    cast( convert (decimal(18,2),100*cast(count(*) as float)/@sumPerson ) as varchar) y
     from hy_hyzlxxb where carid IS NOT NULL AND (xb='F' OR xb='M') ");

            sql.AppendFormat("and addtime>='{0}' ",ht["StartDate"]);
            sql.AppendFormat("and addtime<='{0}' ",ht["EndDate"]);

            sql.AppendFormat(" group by xb ");

            //DataTable dtgg = DataFactory.SqlDataBase().GetDataTableProc("proc_person_sex_select", ht);
            DataTable dtgg = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);
            if (dtgg != null && dtgg.Rows.Count > 0)
            {

                for (int j = 0; dtgg.Rows.Count > j; j++)
                {

                    JsonData json2 = new JsonData();

                    json2["name"] = dtgg.Rows[j]["name"].ToString();
                    json2["y"] = dtgg.Rows[j]["y"].ToString();

                    json1.Add(json2);

                }

            }

            string json = "";
            json = json1.ToJson();
            context.Response.Write(json);


        }



        //支付方式比例
        private void pay_name(HttpContext context)
        {
            JsonData json1 = new JsonData();
            string AdminHotelid = context.Request["AdminHotelid"];

            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];

            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();

            ht["AdminHotelid"] = AdminHotelid;
            ht["StartDate"] = DateTime.Parse(StartDate == null ? "2018-01-01" : StartDate).ToString("yyyy-MM-01");
            ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate).AddMonths(1).ToString("yyyy-MM-01");

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"declare @sumPerson float
	set @sumPerson=cast((select count(*) from V_Reservations  where AdminHotelid='{0}' and addtime>='{1}' and addtime<='{2}' )  as float)
	SELECT payname   name ,
    cast( convert (decimal(18,2),100*cast(count(*) as float)/@sumPerson ) as varchar) y
     from V_Reservations where AdminHotelid='{0}' and addtime>='{1}' and addtime<='{2}' 
    
      ", AdminHotelid, ht["StartDate"], ht["EndDate"]);

            sql.AppendFormat(" group by payname ");

            //DataTable dtgg = DataFactory.SqlDataBase().GetDataTableProc("proc_person_sex_select", ht);
            DataTable dtgg = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            if (dtgg != null && dtgg.Rows.Count > 0)
            {

                for (int j = 0; dtgg.Rows.Count > j; j++)
                {

                    JsonData json2 = new JsonData();

                    json2["name"] = dtgg.Rows[j]["name"].ToString();
                    json2["y"] = dtgg.Rows[j]["y"].ToString();

                    json1.Add(json2);

                }

            }

            string json = "";
            json = json1.ToJson();
            context.Response.Write(json);


        }

        public static string getWeek(string id)
        {
            string w = "";
            
            switch (id)
            {
                case "2": w = "周一"; break;
                case "3": w = "周二"; break;
                case "4": w = "周三"; break;
                case "5": w = "周四"; break;
                case "6": w = "周五"; break;
                case "7": w = "周六"; break;
                case "1": w = "周日"; break;
            }
            return w;
        }

        /// <summary>
        /// 获取卡券列表
        /// </summary>
        /// <param name="context"></param>
        private void GetCouponList(HttpContext context)
        {
            string adminhotelid = context.Request["adminhotelid"];
            JsonData jsondata = new JsonData();
            string sql = string.Format(@"SELECT * FROM dbo.Coupon WHERE AdminHotelid=@AdminHotelid AND DeleteMark =1");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid)};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (ds != null && ds.Rows.Count > 0)
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    JsonData json1 = new JsonData();
                    json1["ID"] = ds.Rows[i]["id"].ToString();
                    json1["COUPONNAME"] = ds.Rows[i]["COUPONNAME"].ToString();
                    
                    jsondata.Add(json1);
                }
            }
            string json = "";
            json = jsondata.ToJson();
            context.Response.Write(json);
        }


        private void R_T_OTTCount(HttpContext context)
        {
            JsonData json1 = new JsonData();
            string AdminHotelid = context.Request["AdminHotelid"];

            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];

            JsonData jsondata = new JsonData();

            Hashtable ht = new Hashtable();

            ht["AdminHotelid"] = AdminHotelid;
            //ht["StartDate"] = DateTime.Parse(StartDate == null ? "2018-01-01" : StartDate).ToString("yyyy-MM-01");
            //ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate).AddMonths(1).ToString("yyyy-MM-01");
            ht["StartDate"] = DateTime.Parse(StartDate == null ? "2018-01-01" : StartDate);
            ht["EndDate"] = DateTime.Parse(EndDate == null ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate);

            DataTable dtgg = DataFactory.SqlDataBase().GetDataTableProc("R_T_OTTCount", ht);

            if (dtgg != null && dtgg.Rows.Count > 0)
            {

                for (int j = 0; dtgg.Rows.Count > j; j++)
                {

                    JsonData json2 = new JsonData();


                    json2["one"] = CommonHelper.GetInt(dtgg.Rows[j]["one"].ToString());
                    json2["two"] = CommonHelper.GetInt(dtgg.Rows[j]["two"].ToString());
                    json2["three"] = CommonHelper.GetInt(dtgg.Rows[j]["three"].ToString());

                    json1.Add(json2);

                }

            }

            string json = "";
            json = json1.ToJson();
            context.Response.Write(json);


        }

        private void MemberReport(HttpContext context) 
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string gjc = context.Request["gjc"];
            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];
            StartDate = StartDate == "" ? "2018-01-01" : StartDate;
            EndDate = EndDate == "" ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate;
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"declare @sumPerson float
	set @sumPerson=cast((select count(*) from hy_hyzlxxb  where carid IS NOT NULL and addtime>='{1}' and addtime<='{2}' )  as float)
	select a.hylx  ,b.hylxname,count(*) num,
    cast( convert (decimal(18,2),100*cast(count(*) as float)/@sumPerson ) as varchar) y
     from hy_hyzlxxb a,hy_hylxbmb b WHERE a.hylx=b.hylxcode AND b.AdminHotelid='{0}' AND carid IS NOT NULL  ", AdminHotelid, StartDate, EndDate);

            sql.AppendFormat("and addtime>='{0}' ", StartDate);
            sql.AppendFormat("and addtime<='{0}' ", EndDate);

            sql.AppendFormat(" group by hylx,b.hylxname,b.sort ORDER BY b.sort DESC  ");

            DataTable dt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);
            int num = 0;
            
            if (dt != null && dt.Rows.Count > 0) 
            {
                StringBuilder html = new StringBuilder();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    num+=Convert.ToInt32(dt.Rows[i]["num"]);
                    StringBuilder tr = new StringBuilder();
                    tr.AppendFormat("<tr>");
                    tr.AppendFormat("<td>{0}</td>", i + 1);
                    tr.AppendFormat("<td>{0}</td>", dt.Rows[i]["hylxname"]);
                    tr.AppendFormat("<td>{0}</td>", dt.Rows[i]["num"]);
                    tr.AppendFormat("<td>{0}%</td>", dt.Rows[i]["y"]);
                    tr.AppendFormat("<td>{0}</td>", 0);
                    tr.AppendFormat("<td>{0}</td>", 0);
                    tr.AppendFormat("<td>{0}</td>", 0);
                    tr.AppendFormat("<td>{0}</td>", 0);
                    tr.AppendFormat("<td>{0}</td>", 0);
                    tr.AppendFormat("</tr>");
                    html.Append(tr);

                }
                html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>", "", "合计", num,"100%",0,0,0,0,0);
                context.Response.Write(html);
            }

        }


        private void IntegralReport(HttpContext context) 
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string gjc = context.Request["gjc"];
            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];
            StartDate = StartDate == "" ? "2018-01-01" : StartDate;
            EndDate = EndDate == "" ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate;
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"declare @sumPerson float
	set @sumPerson=cast((select SUM(jf) from hy_hyxfjlb  where AdminHotelid='{0}' and jf>0 and czrq>='{1}' and czrq<='{2}' )  as float)
	select zmsm,SUM(jf) num,
    cast( convert (decimal(18,2),100*cast(SUM(jf) as float)/@sumPerson ) as varchar) y
     from hy_hyxfjlb WHERE  AdminHotelid='{0}' AND jf>0    ", AdminHotelid, StartDate, EndDate);

            sql.AppendFormat("and czrq>='{0}' ", StartDate);
            sql.AppendFormat("and czrq<='{0}' ", EndDate);

            sql.AppendFormat(" group by zmsm  ");

            DataTable dt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);
            int num = 0;

            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"declare @sumPerson float
	set @sumPerson=cast((select SUM(jf) from hy_hyxfjlb  where AdminHotelid='{0}' and jf>0 and czrq>='{1}' and czrq<='{2}')  as float)
	select SUM(jf)*-1 num,
    cast( convert (decimal(18,2),100*cast(SUM(jf)*-1 as float)/@sumPerson ) as varchar) y,
    @sumPerson+SUM(jf) sy
     from hy_hyxfjlb WHERE  AdminHotelid='{0}' AND jf<0 and czrq>='{1}' and czrq<='{2}'", AdminHotelid, StartDate, EndDate);
                DataTable ds = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sb);
                StringBuilder html = new StringBuilder();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    num += Convert.ToInt32(dt.Rows[i]["num"]);
                    StringBuilder tr = new StringBuilder();
                    tr.AppendFormat("<tr>");
                    tr.AppendFormat("<td>{0}</td>", i + 1);
                    tr.AppendFormat("<td>{0}</td>", dt.Rows[i]["zmsm"]);
                    tr.AppendFormat("<td>{0}</td>", dt.Rows[i]["num"]);
                    tr.AppendFormat("<td>{0}%</td>", dt.Rows[i]["y"]);
                    if (i == 0)
                    {
                        if (ds != null && ds.Rows.Count > 0)
                        {
                            tr.AppendFormat("<td rowspan='{1}'>{0}</td>", ds.Rows[0]["num"], dt.Rows.Count + 1);
                            tr.AppendFormat("<td rowspan='{1}'>{0}%</td>", ds.Rows[0]["y"], dt.Rows.Count + 1);
                            tr.AppendFormat("<td rowspan='{1}'>{0}</td>", ds.Rows[0]["sy"], dt.Rows.Count + 1);
                        }
                        else 
                        {
                            tr.AppendFormat("<td rowspan='{1}'>{0}</td>", 0, dt.Rows.Count + 1);
                            tr.AppendFormat("<td rowspan='{1}'>{0}%</td>", 0, dt.Rows.Count + 1);
                            tr.AppendFormat("<td rowspan='{1}'>{0}</td>", 0, dt.Rows.Count + 1);
                        }
                    }
                    
                    
                    tr.AppendFormat("</tr>");
                    html.Append(tr);

                }
                html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", "", "合计", num, "100%");
                context.Response.Write(html);
            }
        }


        private void IntegralReports(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string gjc = context.Request["gjc"];
            string StartDate = context.Request["StartDate"];
            string EndDate = context.Request["EndDate"];
            StartDate = StartDate == "" ? "2018-01-01" : StartDate;
            EndDate = EndDate == "" ? DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") : EndDate;
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"SELECT jf.zmsm,hy.xm,hy.sjhm,jf.jf,jf.czrq,jf.bz FROM dbo.hy_hyxfjlb jf,dbo.hy_hyzlxxb hy WHERE hy.lsh=jf.lsh and jf.AdminHotelid='{0}' and jf.czrq>='{1}' and jf.czrq<='{2}'  ", AdminHotelid, StartDate, EndDate);

            if (gjc != null && gjc.ToString() != "") 
            {
                sql.AppendFormat(" and jf.zmsm like '%{0}%' ", gjc);
            }

           

            DataTable dt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);
            int num = 0;

            if (dt != null && dt.Rows.Count > 0)
            {
                
                StringBuilder html = new StringBuilder();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                   
                    StringBuilder tr = new StringBuilder();
                    tr.AppendFormat("<tr>");
                    tr.AppendFormat("<td>{0}</td>", i + 1);
                    tr.AppendFormat("<td>{0}</td>", dt.Rows[i]["zmsm"]);
                    tr.AppendFormat("<td>{0}</td>", dt.Rows[i]["xm"]);
                    tr.AppendFormat("<td>{0}</td>", dt.Rows[i]["sjhm"]);
                    tr.AppendFormat("<td>{0}</td>", dt.Rows[i]["jf"]);
                    tr.AppendFormat("<td>{0}</td>", DateTime.Parse(dt.Rows[i]["czrq"].ToString()).ToString("yyyy-MM-dd HH:ff"));
                    tr.AppendFormat("<td>{0}</td>", dt.Rows[i]["bz"]);

                    tr.AppendFormat("</tr>");
                    html.Append(tr);

                }
               //html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", "", "合计", num, "100%");
                context.Response.Write(html);
            }
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