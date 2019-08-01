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

namespace RM.Web.SysSetBase.points
{
    /// <summary>
    /// points1 的摘要说明
    /// </summary>
    public class points1 : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Action = context.Request["action"].Trim();               //提交动作

            switch (Action)
            {
                case "update":
                    update(context);
                    break;
                case "getinfo":
                    getinfo(context);
                    break;
                case "getinfos":
                    getinfos(context);
                    break;
                case "updates":
                    updates(context);
                    break;
                case "gethtmls":
                    gethtmls(context);
                    break;
                case "getDayNumber":
                    getDayNumber(context);
                    break;
                case "setDayNumber":
                    setDayNumber(context);
                    break;
                default:
                    break;
            }

        }

        public void getDayNumber(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT jfDayNumber FROM Hotel_Admin WHERE AdminHotelid = @AdminHotelid ");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())
            };
            string val = "1";
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                val = dt.Rows[0]["jfDayNumber"].ToString();
            }
            context.Response.Write(val);
        }

        public void setDayNumber(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string DayNumber = context.Request["DayNumber"];
            Hashtable ht = new Hashtable();
            ht["jfDayNumber"] = DayNumber;
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Hotel_Admin", "AdminHotelid", AdminHotelid, ht);
            if (IsOk)
            {
                context.Response.Write("ok");
            }
            else
            {
                context.Response.Write("error");
            }
        }

        public void update(HttpContext context)
        {
            string jfzhi = context.Request["jfzhi"].Trim();
            string isEnble = context.Request["isEnble"].Trim();
            string jfzhi1 = context.Request["jfzhi1"].Trim();
            string isEnble1 = context.Request["isEnble1"].Trim();
            string jfzhi2 = context.Request["jfzhi2"].Trim();
            string isEnble2 = context.Request["isEnble2"].Trim();
            string isjf = context.Request["isjf"].Trim();
            Hashtable hs = new Hashtable();
            hs["jfzhi"] = jfzhi;
            hs["isEnble"] = isEnble;
            hs["jfzhi1"] = jfzhi1;
            hs["isEnble1"] = isEnble1;
            hs["jfzhi2"] = jfzhi2;
            hs["isEnble2"] = isEnble2;
            hs["isjf"] = isjf;
            hs["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
            string AdminHotelids = "";
            string sql = string.Format(@"select id from jfmatter where AdminHotelid=@AdminHotelid");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (dt != null && dt.Rows.Count > 0)
            {
                AdminHotelids = RequestSession.GetSessionUser().AdminHotelid.ToString();
            }
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("jfmatter", "AdminHotelid", AdminHotelids, hs);
            if (IsOk)
            {
                context.Response.Write("ok");
            }
            else
            {
                context.Response.Write("error");
            }
        }

        public void getinfo(HttpContext context)
        {


            string sql = string.Format(@"select jfzhi,jfzhi1,jfzhi2,isEnble,isEnble1,isEnble2,isjf from jfmatter where AdminHotelid=@AdminHotelid");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);

            string dtName = "jfmatter";
            string json = JsonHelper.DataTableToJson(dt, dtName);
            context.Response.Write(json);

        }

        public void getinfos(HttpContext context)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * FROM jfmatter WHERE AdminHotelid=@AdminHotelid ");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, parmAdd);

            string dtName = "jfmatter";
            string json = JsonHelper.DataTableToJson(dt, dtName);
            context.Response.Write(json);
        }

        public void updates(HttpContext context)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("delete jfmatter WHERE AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid + "' ");
            DataFactory.SqlDataBase().ExecuteBySql(sql);
            string value = context.Request["values"].Trim();
            string isEnble = context.Request["isEnble"].Trim();
            string isEnble1 = context.Request["isEnble1"].Trim();
            string isEnble2 = context.Request["isEnble2"].Trim();
            string isEnble3 = context.Request["isEnble3"].Trim();

            string isjf = context.Request["isjf"].Trim();
            int x = 0;
            string[] values = value.Split('|');
            for (int i = 0; i < values.Length; i++)
            {
                Hashtable ht = new Hashtable();
                ht["jb"] = values[i].Split(',')[0];
                ht["jfzhi"] = values[0].Split(',')[1];
                ht["jfzhi1"] = values[i].Split(',')[2];
                ht["jfzhi2"] = values[i].Split(',')[3];
                ht["jfzhi3"] = values[i].Split(',')[4];

                ht["isEnble"] = isEnble;
                ht["isEnble1"] = isEnble1;
                ht["isEnble2"] = isEnble2;
                ht["isEnble3"] = isEnble3;

                ht["isjf"] = isjf;
                ht["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();

                x += DataFactory.SqlDataBase().InsertByHashtable("jfmatter", ht);

            }

            context.Response.Write(x);
        }

        public void gethtmls(HttpContext context)
        {
            string TbHtml = "";
            #region ***** 加载表格 *****
            DataTable dt = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("SELECT TOP 1 TypeName FROM dbo.Set_Association WHERE AdminHotelid=@AdminHotelid ");
                SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())};
                DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(sql, parmAdd);
                bool dlb = false;
                if (dts != null && dts.Rows.Count > 0)
                {
                    if (dts.Rows[0][0].ToString() == "独立版")
                    {
                        dlb = true;
                    }

                }

                //头部标题

                TbHtml += "<tr>";
                TbHtml += "<th width='40'>启用</th><th width='130'>方式</th>";

                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    TbHtml += "<th class='hylxnames' jb='" + dt.Rows[j]["code"].ToString() + "'>";
                    TbHtml += dt.Rows[j]["LevelName"].ToString();
                    TbHtml += "</th>";

                }
                TbHtml += "</tr>";

                //微网注册

                TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>微网注册<em style='margin-left:35px;'>注册会员可获得</em></td>";
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    TbHtml += "<td>";
                    if (j == 0)
                    {
                        TbHtml += "<input type='text' value='' class='jfzhi' jb='" + dt.Rows[j]["code"].ToString() + "' /><small>分</small>";
                    }
                    else
                    {
                        TbHtml += "<input type='text' value='0' class='jfzhi' style='display:none;' jb='" + dt.Rows[j]["code"].ToString() + "' />";
                    }

                    TbHtml += "</td>";

                }
                TbHtml += "</tr>";



                //客户点评

                TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>客户点评<em style='margin-left:35px;'>每次点评可获得</em></td>";
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    TbHtml += "<td>";

                    TbHtml += "<input type='text'  class='jfzhi2' jb='" + dt.Rows[j]["code"].ToString() + "' /><small>分</small>";

                    TbHtml += "</td>";

                }
                TbHtml += "</tr>";
                //消费积分

                TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>消费积分<em style='margin-left:35px;'>每消费1元可获得</em></td>";
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    TbHtml += "<td>";

                    TbHtml += "<input type='text' " + (dlb ? "" : "disabled") + " class='jfzhi1' jb='" + dt.Rows[j]["code"].ToString() + "' /><small>分</small>";

                    TbHtml += "</td>";

                }

                TbHtml += (dlb ? "" : "<td style='text-align:left;'>以PMS系统的积分为准</td>");
                TbHtml += "</tr>";

                //签到积分

                TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>签到积分<em style='margin-left:35px;'>每签到1次可获得</em></td>";
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    TbHtml += "<td>";

                    TbHtml += "<input type='text' class='jfzhi3' jb='" + dt.Rows[j]["code"].ToString() + "' /><small>分</small>";

                    TbHtml += "</td>";

                }

                TbHtml += "</tr>";

            }
            #endregion


            context.Response.Write(TbHtml);
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