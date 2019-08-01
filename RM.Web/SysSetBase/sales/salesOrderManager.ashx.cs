using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Common.DotNetCode;
using RM.Common.DotPqGrid;
using System.Collections;
using RM.Busines;
using System.Data;
using RM.Common.DotNetBean;
using System.Web.SessionState;

namespace RM.Web.SysSetBase.sales
{
    /// <summary>
    /// salesOrderManager 的摘要说明
    /// </summary>
    public class salesOrderManager : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Menu = context.Request["Menu"];                      //提交动作
            switch (Menu)
            {
                case "GetInfoList":
                    GetInfoList(context);
                    break;
                case "GetGjss":
                    break;
                case "GetOrderByMemberID":
                    GetOrderByMemberID(context);
                    break;
                case "GetTiXianByMemberID":
                    GetTiXianByMemberID(context);
                    break;
                case "GetDaiLaiKeHuByMemberID":
                    GetDaiLaiKeheByMemberID(context);
                    break;
                case "GetFinaceByMemberID":
                    GetFinaceByMemberID(context);
                    break;
                case "GetIntegralInfoMemberID":
                    GetIntegralInfoMemberID(context);
                    break;
                case "SetPopularizeState":
                    SetPopularizeState(context);
                    break;
                case "AllPopularizeState":
                    AllPopularizeState(context);
                    break;

                default:
                    break;
            }
        }
        private void SetPopularizeState(HttpContext context)
        {
            string userid = context.Request["userid"];
            string state = context.Request["state"];
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(state))
            {
                context.Response.Write(0);
                return;
            }
            Hashtable ht = new Hashtable();
            ht["PopularizeState"] = state;
            DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", userid, ht);
            context.Response.Write(1);
        }
        private void AllPopularizeState(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelId"];
            string Hotelid = context.Request["HotelId"];
            string state = context.Request["state"];
            if (string.IsNullOrEmpty(AdminHotelid) || string.IsNullOrEmpty(Hotelid) || string.IsNullOrEmpty(state))
            {
                context.Response.Write(0);
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Base_UserInfo Set PopularizeState = @PopularizeState Where AdminHotelid = @AdminHotelid And hotelid = @Hotelid");
            SqlParam[] param = new SqlParam[]{
                new SqlParam("@AdminHotelid",AdminHotelid),
                new SqlParam("@Hotelid",Hotelid),
                new SqlParam("@PopularizeState",state)
            };
            int val = DataFactory.SqlDataBase().ExecuteBySql(sb, param);
            if (val > 0)
            {
                context.Response.Write(1);
            }
        }


        /// <summary>
        /// 积分信息
        /// </summary>
        /// <param name="context"></param>
        private void GetIntegralInfoMemberID(HttpContext context)
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
                pqGrid_OrderField = "czrq";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sb = new StringBuilder();//查询条件
            sb.AppendFormat(" and AdminHotelid = '{0}' ", RequestSession.GetSessionUser().AdminHotelid.ToString());
            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {
                        var cs = "@obj" + i;
                        if (nv[0] == "memberID")
                        {
                            sb.Append(" and lsh=" + nv[1] + "");
                        }
                    }
                }
            }

            DataTable dt = DataFactory.SqlDataBase().GetPageLists("select jf.lsh,jf.kh,hy_hyzlxxb.xm,jf.czrq,jf.zmsm,jf.jf,sjhm from hy_hyxfjlb jf inner join hy_hyzlxxb on hy_hyzlxxb.lsh=jf.lsh where 1=1 and hy_hyzlxxb.carid is not null " + sb, pqGrid_Sort, pqGrid_OrderField, pqGrid_OrderType, PageIndex, PageSize, ref totalRecords);

            //  DataTable dt = DataFactory.SqlDataBase().DbPager("V_IntegralInfo", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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
                        else if (dt.Columns[j].DataType.Name == "Decimal")
                        {
                            cs.Add(Convert.ToInt32(dt.Rows[i][j]).ToString());
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
        /// <summary>
        /// 根据会员获取会员财务信息
        /// </summary>
        /// <param name="context"></param>
        private void GetFinaceByMemberID(HttpContext context)
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
                pqGrid_OrderField = "AddTime";
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
                        var cs = "@obj" + i;
                        if (nv[0] == "memberID")
                        {
                            sb.Append(" and MemberId=" + nv[1] + "");
                        }
                    }
                }
            }

            DataTable dt = DataFactory.SqlDataBase().DbPager("finance", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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
                            cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm"));
                        }
                        else if (dt.Columns[j].DataType.Name == "Decimal")
                        {
                            cs.Add(Convert.ToInt32(dt.Rows[i][j]).ToString());
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
        /// <summary>
        /// 根据会员获取奖金记录
        /// </summary>
        /// <param name="context"></param>
        private void GetOrderByMemberID(HttpContext context)
        {
            if (RequestSession.GetSessionUser() == null)
            {
                context.Response.Write("");
                return;
            }

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
            string hotelid = context.Request.QueryString["hotelid"];
            if (pqGrid_OrderField == null || pqGrid_OrderField == "")//排序字段不能为空
            {
                pqGrid_OrderField = "CreateDate";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" 1 = 1 ");
            sb.Append(" and AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid + "' ");



            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {
                        var cs = "@obj" + i;
                        if (nv[0] == "Search")
                        {
                            sb.Append(" and (Remarks like '%" + nv[1] + "%' or OrderNumber like '%" + nv[1] + "%'  )");
                        }
                        else if (nv[0] == "UserId")
                        {
                            sb.Append(" and UserId='" + nv[1] + "' ");
                        }
                        else if (nv[0] == "CreateDate")
                        {
                            sb.Append(" and CreateDate>='" + nv[1] + "' ");
                        }
                        else if (nv[0] == "CreateDate2")
                        {
                            sb.Append(" and CreateDate<='" + nv[1] + "' ");
                        }

                    }
                }
            }


            DataTable dt = DataFactory.SqlDataBase().DbPager("V_GeneralizeCommision", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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
                            cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm"));
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

        /// <summary>
        /// 根据会员获取带来客户信息
        /// </summary>
        /// <param name="context"></param>
        private void GetDaiLaiKeheByMemberID(HttpContext context)
        {
            if (RequestSession.GetSessionUser() == null)
            {
                context.Response.Write("");
                return;
            }

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
            string hotelid = context.Request.QueryString["hotelid"];
            if (pqGrid_OrderField == null || pqGrid_OrderField == "")//排序字段不能为空
            {
                pqGrid_OrderField = "addtime";
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
                        if (nv[0] == "text")
                        {
                            sb.Append(" and (sjhm like '%" + nv[1] + "%' or xm like '%" + nv[1] + "%'  )");
                        }
                        else
                        {
                            sb.Append(" and " + nv[0] + "='" + nv[1] + "' ");
                        }
                    }
                }
            }

            DataTable dt = DataFactory.SqlDataBase().DbPager("V_Sales_fans", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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
                            cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm"));
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

        //获取统计数据
        private ArrayList GetDaiLaiData(ArrayList cs)
        {

            return cs;
        }

        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="lsh">会员ID</param>
        /// <param name="adminhotelid">酒店全局id</param>
        /// <param name="type">类型1姓名、2手机号码、3头像</param>
        private string gethyxx(string lsh, string adminhotelid, string type)
        {
            string fhz = "";
            try
            {
                string sql = string.Format(@"select xm,sjhm,hylx from hy_hyzlxxb where carid is not null and lsh=@lsh");
                SqlParam[] parmAddfs = new SqlParam[] { 
                                     new SqlParam("@lsh",lsh)};
                DataTable dtfs = DataFactory.SqlDataBase(adminhotelid).GetDataTableBySQL(new StringBuilder(sql), parmAddfs);
                if (dtfs != null && dtfs.Rows.Count > 0)
                {
                    if (type == "1")
                    {
                        fhz = dtfs.Rows[0]["xm"].ToString();
                    }
                    else if (type == "2")
                    {
                        string a = dtfs.Rows[0]["sjhm"].ToString().Substring(0, 3);
                        string b = dtfs.Rows[0]["sjhm"].ToString().Substring(7, 4);
                        fhz = a + "****" + b;
                    }
                    else if (type == "3")
                    {
                        fhz = gethylx(adminhotelid, dtfs.Rows[0]["hylx"].ToString());
                    }
                }
            }
            catch
            {

            }
            return fhz;
        }

        private string gethylx(string adminhotelid, string hylxcode)
        {
            string fhz = "";
            try
            {

                string sql = string.Format(@"SELECT replace(hylxcode, ' ', '') as code,hylxname as LevelName FROM hy_hylxbmb where hylxcode=@hylxcode and AdminHotelid=@AdminHotelid ");
                SqlParam[] parmAddfs = new SqlParam[] { 
                                     new SqlParam("@hylxcode",hylxcode),
                new SqlParam("@AdminHotelid",adminhotelid)};
                DataTable dtfs = DataFactory.SqlDataBase(adminhotelid).GetDataTableBySQL(new StringBuilder(sql), parmAddfs);
                if (dtfs != null && dtfs.Rows.Count > 0)
                {
                    fhz = dtfs.Rows[0]["LevelName"].ToString();
                }

            }
            catch
            {

            }
            return fhz;
        }

        /// <summary>
        /// 根据会员获取会员提现信息
        /// </summary>
        /// <param name="context"></param>
        private void GetTiXianByMemberID(HttpContext context)
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
                pqGrid_OrderField = "ID";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;
            string Memberid = "";
            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" 1 = 1 ");


            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {
                        var cs = "@obj" + i;
                        if (nv[0] == "Search")
                        {
                            sb.Append(" and (bz like '%" + nv[1] + "%' or wxddh like '%" + nv[1] + "%'  )");
                        }
                        else if (nv[0] == "UserId")
                        {
                            sb.Append(" and UserId='" + nv[1] + "' ");
                        }
                        else if (nv[0] == "addtime")
                        {
                            sb.Append(" and addtime>='" + nv[1] + "' ");
                        }
                        else if (nv[0] == "addtime2")
                        {
                            sb.Append(" and addtime<='" + nv[1] + "' ");
                        }

                    }
                }
            }


            DataTable dt = DataFactory.SqlDataBase().DbPager("V_Sales_withdraw", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);


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
                            cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm"));
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




        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="context"></param>
        private void GetInfoList(HttpContext context)
        {
            if (RequestSession.GetSessionUser() == null)
            {
                context.Response.Write("");
                return;
            }

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
                        var cs = "@obj" + i;
                        if (nv[0] == "kh")
                        {
                            sb.Append(" and (User_Account like '%" + nv[1] + "%' or User_Name like '%" + nv[1] + "%' or Roles_Name like '%" + nv[1] + "%' )");
                        }
                        else if (nv[0] == "CreateDate")
                        {
                            sb.Append(" and CreateDate>='" + nv[1] + "' ");
                        }
                        else if (nv[0] == "CreateDate2")
                        {
                            sb.Append(" and CreateDate<='" + nv[1] + " 23:59:59" + "' ");
                        }
                        else
                        {
                            sb.Append(" and " + nv[0] + " = '" + nv[1] + "' ");
                        }
                    }
                }
            }

            IList<SqlParam> IList_param = new List<SqlParam>();
            DataTable dt = DataFactory.SqlDataBase().DbPager("V_HotelMemberList", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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
                            cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm"));
                        }
                        else if (dt.Columns[j].DataType.Name == "Decimal")
                        {
                            cs.Add(dt.Rows[i][j]).ToString();
                        }
                        else
                        {
                            cs.Add(dt.Rows[i][j].ToString());
                        }
                    }
                    //cs = GetTongJiData(cs);//增加统计数据
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

        //获取统计数据
        private ArrayList GetTongJiData(ArrayList cs)
        {


            Hashtable hs = new Hashtable();
            hs["Phone"] = cs[1].ToString();
            hs["UserId"] = cs[0].ToString();
            DataTable ds = DataFactory.SqlDataBase().GetDataTableProc("P_Sales_record", hs);
            if (ds != null && ds.Rows.Count > 0)
            {
                //销量金额  @Sales_Total
                cs.Add(ds.Rows[0]["Sales_Total"].ToString());
                //累计奖金 @Bonus_Total
                cs.Add(ds.Rows[0]["Bonus_Total"].ToString());
                //提现奖金 @WithdrawCash
                cs.Add(ds.Rows[0]["WithdrawCash"].ToString());
                //剩余奖金 @ExtractableMoney
                cs.Add(ds.Rows[0]["ExtractableMoney"].ToString());
                //带来客户 @Register_Total
                cs.Add(ds.Rows[0]["Register_Total"].ToString());
            }


            //StringBuilder sb = new StringBuilder();
            //sb.Append(@"SELECT * from hy_hyzlxxb where sjhm=@sjhm");
            //SqlParam[] param = new SqlParam[] { 
            //               new SqlParam("@sjhm", sjhm)};
            //DataTable dt = DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).GetDataTableBySQL(sb, param); 
            //string memberid = dt.Rows[0]["lsh"].ToString();//会员Memberid，eg：1004


            //// --获取客房销售、奖金
            //StringBuilder sqlJiangJin = new StringBuilder();
            //sqlJiangJin.AppendFormat(@"SELECT ISNULL(SUM(CAST(Sales_Amount AS MONEY)),0) AS KeFangMoney,ISNULL(SUM(CAST(money AS MONEY)),0) AS JiangJin  FROM Distribution_Finance WHERE memberid='" + memberid + "'");
            //DataTable dtJiangJin = DataFactory.SqlDataBase().GetDataTableBySQL(sqlJiangJin);
            //cs.Add(dtJiangJin.Rows[0]["KeFangMoney"].ToString());//客房销售
            //cs.Add(dtJiangJin.Rows[0]["JiangJin"].ToString());//奖金

            //// --获取提现奖金、剩余奖金
            //StringBuilder sqlTiXian = new StringBuilder();
            //sqlTiXian.AppendFormat(@"SELECT ISNULL(SUM(CAST(money AS MONEY)),0) AS TiXianMoney FROM Sales_withdraw WHERE sjhm='" + sjhm + "'");
            //DataTable dtTiXian = DataFactory.SqlDataBase().GetDataTableBySQL(sqlTiXian);
            //cs.Add(dtTiXian.Rows[0]["TiXianMoney"].ToString());//提现奖金
            //cs.Add(Convert.ToDecimal(dtJiangJin.Rows[0]["JiangJin"].ToString()) - Convert.ToDecimal(dtTiXian.Rows[0]["TiXianMoney"].ToString()));//剩余奖金

            //// --获取带来客户
            //StringBuilder sqlKeHu = new StringBuilder();
            //sqlKeHu.AppendFormat(@"SELECT COUNT(*) AS DaiLaikehu FROM dbo.hy_hyzlxxb WHERE par_uid='" + memberid + "'");
            //DataTable dtKeHu = DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).GetDataTableBySQL(sqlKeHu);
            //cs.Add(dtKeHu.Rows[0]["DaiLaikehu"].ToString());//带来客户

            return cs;
        }





        //获取累计奖金
        private ArrayList GetLeiJiJiangJin(ArrayList cs, string memberid, DateTime Addtime)
        {

            // --获取客房销售、奖金
            StringBuilder sqlJiangJin = new StringBuilder();
            sqlJiangJin.AppendFormat(@"SELECT  ISNULL(SUM(CAST(money AS MONEY)),0) AS JiangJin  FROM Distribution_Finance WHERE memberid='" + memberid + "' and  addtime<='" + Addtime + "'");
            DataTable dtJiangJin = DataFactory.SqlDataBase().GetDataTableBySQL(sqlJiangJin);
            cs.Add(dtJiangJin.Rows[0]["JiangJin"].ToString());//奖金

            return cs;
        }

        //获取剩余奖金
        private ArrayList GetShengYuJiangJin(ArrayList cs, string memberid, string sjhm, DateTime Addtime)
        {

            // --获取剩余奖金
            StringBuilder sqlJiangJin = new StringBuilder();
            sqlJiangJin.AppendFormat(@"SELECT ISNULL(SUM(CAST(money AS MONEY)),0) AS JiangJin  FROM Distribution_Finance WHERE memberid='" + memberid + "' and  addtime<='" + Addtime + "'");
            DataTable dtJiangJin = DataFactory.SqlDataBase().GetDataTableBySQL(sqlJiangJin);

            StringBuilder sqlTiXian = new StringBuilder();
            sqlTiXian.AppendFormat(@"SELECT ISNULL(SUM(CAST(money AS MONEY)),0) AS TiXianMoney FROM Sales_withdraw WHERE sjhm='" + sjhm + "' and  addtime<='" + Addtime + "'");
            DataTable dtTiXian = DataFactory.SqlDataBase().GetDataTableBySQL(sqlTiXian);
            cs.Add(Convert.ToDecimal(dtJiangJin.Rows[0]["JiangJin"].ToString()) - Convert.ToDecimal(dtTiXian.Rows[0]["TiXianMoney"].ToString()));//剩余奖金 

            return cs;
        }

        private string GetOrderTotal(string fans_ID)
        {
            // --获取带来客户
            string GetGetOrderTotalNum = "";
            StringBuilder sqlKeHu = new StringBuilder();
            sqlKeHu.AppendFormat(@"SELECT  ISNULL(SUM(TomePrice),0)  as OrderTotalMoney  FROM dbo.Reservation WHERE MemberId ='" + fans_ID + "' AND Pay=1");
            DataTable dtKeHu = DataFactory.SqlDataBase().GetDataTableBySQL(sqlKeHu);
            GetGetOrderTotalNum = dtKeHu.Rows[0]["OrderTotalMoney"].ToString();

            return GetGetOrderTotalNum;
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