using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Busines;
using System.Collections;
using System.Data;
using RM.Common.DotPqGrid;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;
using System.Web.SessionState;

namespace RM.Web.SysSetBase.coupons
{
    /// <summary>
    /// selectkh 的摘要说明
    /// </summary>
    public class selectkh : IHttpHandler, IReadOnlySessionState
    {


        public void ProcessRequest(HttpContext context)
        {
            string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();

            context.Response.ContentType = "text/plain";
            string kh = context.Request["kh"];
            kh = "'" + kh.Replace("，", ",").TrimEnd(',').Replace(",", "','") + "'";
            string sql = string.Format(@"select lsh,kh,xm,sjhm from hy_hyzlxxb where AdminHotelid={0} and kh in ({1}) or sjhm in ({1}) ", AdminHotelid, kh);
            DataTable dt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(new StringBuilder(sql));
            if (dt.Rows.Count > 0)
            {
                context.Response.Write(ToJson(dt));
            }
            else
            {
                context.Response.Write("1");
            }
        }

        protected string ToJson(DataTable dt)
        {
            StringBuilder JsonString = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonString.Append("[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }
                JsonString.Append("]");

                return JsonString.ToString();
            }
            else
            {
                return null;
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