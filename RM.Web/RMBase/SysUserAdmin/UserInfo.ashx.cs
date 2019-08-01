using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using RM.Busines;
using System.Collections;
using System.Text;
using System.Data;
using RM.Common.DotPqGrid;
using RM.Common.DotNetBean;

namespace RM.Web.RMBase.SysUserAdmin
{
    /// <summary>
    /// UserInfo 的摘要说明
    /// </summary>
    public class UserInfo : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string Action = context.Request["action"];                      //提交动作
            string user_ID = context.Request["user_ID"];                    //用户主键
            Hashtable ht = new Hashtable();
            int Return = -1;
            switch (Action)
            {
                case "accredit":                                            //用户信息启用
                    ht["DeleteMark"] = 1;
                    Return = DataFactory.SqlDataBase().UpdateByHashtable("ABase_UserInfo", "User_ID", user_ID, ht);
                    context.Response.Write(Return.ToString());
                    break;
                case "lock":                                                //锁定用户信息
                    ht["DeleteMark"] = 2;
                    Return = DataFactory.SqlDataBase().UpdateByHashtable("ABase_UserInfo", "User_ID", user_ID, ht);
                    context.Response.Write(Return.ToString());
                    break;
                case "GetInfoList":
                    GetInfoList(context);
                    break;
                default:
                    break;
            }
        }

        private void GetInfoList(HttpContext context)
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
                Search = context.Request.QueryString["Search"].Split('|');//name@value|name@value
            }
            catch
            {
            }
            string type = context.Request.QueryString["type"];
            string pqGrid_OrderField = context.Request.QueryString["pqGrid_OrderField"];//排序字段名
            string pqGrid_OrderType = context.Request.QueryString["pqGrid_OrderType"];//排序方式 asc desc
            string pqGrid_Sort = context.Request.QueryString["pqGrid_Sort"];//需要查询的字段
            string pqGrid = context.Request.QueryString["_"];
            if (pqGrid_OrderField == null || pqGrid_OrderField == "")//排序字段不能为空
            {
                pqGrid_OrderField = "CreateDate";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" 1 = 1 ");
            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {
                        if (nv[0] == "USER_NAME")
                        {
                            sb.Append(" and (USER_NAME like '%" + nv[1] + "%' or User_Account like  '%" + nv[1] + "%' or theme like  '%" + nv[1] + "%'");
                        }
                        else
                        {
                            sb.Append(" and " + nv[0] + " = '" + nv[1] + "' ");
                        }
                    }
                }
            }

            if (context.Request.Cookies["dladmin_COOKIE"]["User_Account"].ToLower() != "sewa")
            {
                sb.Append("and User_Account!='sewa'");
            }

            DataTable dt = DataFactory.SqlDataBase().DbPager("V_ABase_UserInfo", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);
            ArrayList data = new ArrayList();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ArrayList cs = new ArrayList();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].DataType.Name == "DateTime")
                        {
                            cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd"));
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