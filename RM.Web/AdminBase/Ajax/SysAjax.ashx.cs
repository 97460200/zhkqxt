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


namespace RM.Web.AdminBase.Ajax
{
    /// <summary>
    /// SysAjax1 的摘要说明
    /// </summary>
    public class SysAjax1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Menu = context.Request["Menu"];                      //提交动作
            switch (Menu)
            {
                case "Show_Set_now": //展示提现功能设置
                    Show_Set_now(context);
                    break;
                case "Set_now": //保存提现功能设置
                    Set_now(context);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 展示提现功能设置
        /// </summary>
        /// <param name="context"></param>
        private void Show_Set_now(HttpContext context)
        {
            string adminhotelid = context.Request["adminhotelid"];
            string sql = string.Format(@"select ID	,is_reward,Minimum,Limit,Maxmum from Set_now ");

            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            if (ds != null && ds.Rows.Count > 0)
            {
                context.Response.Write(ToJson(ds));
            }
            else
            {
                context.Response.Write("");
            }
        }

        /// <summary>
        /// 保存提现功能设置
        /// </summary>
        /// <param name="context"></param>
        private void Set_now(HttpContext context)
        {
            int fhz = 0;
            try
            {
                string is_reward = context.Request["is_reward"];
                string Minimum = context.Request["Minimum"];
                string Limit = context.Request["Limit"];
                string Maxmum = context.Request["Maxmum"];

                string isid = context.Request["isid"];
                if (isid == "0")
                {
                    Hashtable ht = new Hashtable();
                    ht["is_reward"] = is_reward;
                    if (is_reward == "1")
                    {
                        ht["Minimum"] = Minimum;
                        ht["Limit"] = Limit;
                        ht["Maxmum"] = Maxmum;
                    }
                    fhz = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("Set_now", ht);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"update Set_now set is_reward=@is_reward ");

                    if (is_reward == "1")
                    {
                        sb.AppendFormat(@",Minimum='{0}',Limit='{1}',Maxmum='{2}' ", Minimum, Limit, Maxmum);
                    }

                    sb.AppendFormat(@" where id=@id ");

                    SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@is_reward", is_reward),
                                    new SqlParam("@id", isid)};
                    fhz = DataFactory.SqlDataBase().ExecuteBySql(sb, parmAdd);

                }
                context.Response.Write(fhz);
                return;
            }
            catch
            {
                context.Response.Write(fhz);
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