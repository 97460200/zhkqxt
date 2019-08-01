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
using RM.Common.DotPqGrid;

namespace RM.Web.SysSetBase.rechange
{
    /// <summary>
    /// rechange1 的摘要说明
    /// </summary>
    public class rechange1 : IHttpHandler, IReadOnlySessionState
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
            int totalRecords = 0;
            int PageIndex = 1;
            int PageSize = 10;
            string[] Search = null;
            try
            {
                string pqGrid_PageIndex = context.Request.QueryString["pqGrid_PageIndex"];
                PageIndex = Convert.ToInt32(pqGrid_PageIndex);
                string pqGrid_PageSize = context.Request.QueryString["pqGrid_PageSize"];
                PageSize = Convert.ToInt32(pqGrid_PageSize);
                Search = context.Request.QueryString["Search"].Split('|');//name≌value|name≌value
            }
            catch
            {
            }
            
            string pqGrid_OrderField = context.Request.QueryString["pqGrid_OrderField"];//排序字段名
            string pqGrid_OrderType = context.Request.QueryString["pqGrid_OrderType"];//排序方式 asc desc
            string pqGrid_Sort = context.Request.QueryString["pqGrid_Sort"];//需要查询的字段
            string pqGrid = context.Request.QueryString["_"];
            if (pqGrid_OrderField == null || pqGrid_OrderField == "")//排序字段不能为空
            {
                pqGrid_OrderField = "sort";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;
            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");
      
            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {
                        var cs = "@obj" + i;
                        if (nv[0] == "HotelName")
                        {
                            sb.Append(" and (HotelName like '%" + nv[1] + "%' or hylxname like '%" + nv[1] + "%'  or bz like '%" + nv[1] + "%'  )");
                     
                        }
                        else if (nv[0] == "HotelId")
                        {
                            if (nv[1].ToString() != "-1") 
                            {
                                sb.Append(" and HotelId = '" + nv[1] + "' ");
                            }
                        }
                        else
                        {
                            sb.Append(" and " + nv[0] + " = '" + nv[1] + "' ");
                        }
                    }
                }
            }

            DataTable dt = DataFactory.SqlDataBase().DbPager("v_CardRecharges", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);
            IList<SqlParam> IList_param = new List<SqlParam>();
            ArrayList data = new ArrayList();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ArrayList cs = new ArrayList();
                    for (int j = 1; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].DataType.Name == "DateTime")
                        {
                            if (dt.Rows[i][j] != null && dt.Rows[i][j].ToString() != "")
                            {
                                cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm"));
                            }
                            else
                            {
                                cs.Add("");
                            }
                        }
                        else
                        {
                            cs.Add(dt.Rows[i][j].ToString());
                        }

                    }
                    data.Add(cs);
                }
            }

            PqGridHelper pq = new PqGridHelper();
            pq.totalRecords = totalRecords;
            pq.curPage = PageIndex;
            pq.data = data;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(pq);
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