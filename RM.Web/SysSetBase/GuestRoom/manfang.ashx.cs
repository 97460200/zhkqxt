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
    /// manfang1 的摘要说明
    /// </summary>
    public class manfang1 : IHttpHandler, IReadOnlySessionState
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
                default:
                    break;
            }
        }

        public void getinfo(HttpContext context)
        {
            string hotelid = context.Request["hotelid"].Trim();

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from Full_house where hotelid='{0}' and AdminHotelid='{1}' order by ID desc", hotelid, RequestSession.GetSessionUser().AdminHotelid.ToString());
            
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            string dtName = "Full_house";
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