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

namespace RM.Web.SysSetBase.GuestRoom
{
    /// <summary>
    /// jri1 的摘要说明
    /// </summary>
    public class jri1 : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string Action = context.Request["action"].Trim();               //提交动作

            switch (Action)
            {
                case "addjr"://添加
                    add(context);
                    break;
                case "updatejr"://修改
                    update(context);
                    break;
                case "getinfo":
                    getinfo(context);
                    break;
                default:
                    break;
            }
        }

        public void add(HttpContext context) 
        {
            string Name = context.Request["Name"].Trim();
            string BeginTime = context.Request["BeginTime"].Trim();
            string EndTime = context.Request["EndTime"].Trim();
            

            Hashtable hs = new Hashtable();
            hs["FestivalName"] = Name;
            hs["StartTime"] = DateTime.Parse(BeginTime);
            hs["EndTime"] = DateTime.Parse(EndTime);

            DateTime dtbig = DateTime.Parse(BeginTime);
            DateTime dtend = DateTime.Parse(EndTime);

            TimeSpan ts = dtend - dtbig;
            hs["Number"] = int.Parse(ts.Days.ToString());

            hs["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();

            int a = DataFactory.SqlDataBase().InsertByHashtable("Holiday", hs);
            if (a > 0)
            {
                //添加成功
                context.Response.Write("ok");
            }
            else { context.Response.Write("error"); }

            

        }


        public void update(HttpContext context)
        {
            string Name = context.Request["Name"].Trim();
            string BeginTime = context.Request["BeginTime"].Trim();
            string EndTime = context.Request["EndTime"].Trim();
            string ID = context.Request["ID"].Trim();

            Hashtable hs = new Hashtable();
            hs["FestivalName"] = Name;
            hs["StartTime"] = DateTime.Parse(BeginTime);
            hs["EndTime"] = DateTime.Parse(EndTime);

            DateTime dtbig = DateTime.Parse(BeginTime);
            DateTime dtend = DateTime.Parse(EndTime);

            TimeSpan ts = dtend - dtbig;
            hs["Number"] = int.Parse(ts.Days.ToString());

            hs["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();

            int x = DataFactory.SqlDataBase().UpdateByHashtable("Holiday", "ID", ID, hs);
            if (x > 0)
            {
                //修改成功
                context.Response.Write("ok");
            }
            else { context.Response.Write("error"); }

            

        }



        public void getinfo(HttpContext context)
        {
            string ID = context.Request["ID"].Trim();

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * FROM Holiday WHERE ID=" + ID + " ");

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            string dtName = "Holiday";
            string json = JsonHelper.DataTableToJson(dt, dtName);
            context.Response.Write(json);

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