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

namespace RM.Web.SysSetBase.memInfo
{
    /// <summary>
    /// mem 的摘要说明
    /// </summary>
    public class mem : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Menu = context.Request["Menu"];                      //提交动作
            switch (Menu)
            {
                case "GetInfoList":
                    GetInfoList(context);//会员卡管理
                    break;
                case "GetInfoLists":
                    GetInfoLists(context);//会员卡记录
                    break;
                case "GetInfoListss":
                    GetInfoListss(context);//会员订单
                    break;
                case "GetIntegraList":
                    GetIntegraList(context);//积分记录
                    break;
                case "MemberSourceList":
                    MemberSourceList(context);//扫码记录
                    break;
                case "OperationLogList":
                    OperationLogList(context);//操作记录
                    break;
                case "GetInfoListsR":
                    GetInfoListsR(context);//充值记录
                    break;

                case "GetCardMoneyList":
                    GetCardMoneyList(context);//充值记录
                    break;

                case "UpdatePrintTime":
                    UpdatePrintTime(context);//更新打印小票时间
                    break;
                default:
                    break;
            }
        }



        private void GetCardMoneyList(HttpContext context)
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
            string type = context.Request.QueryString["type"];
            string pqGrid_OrderField = context.Request.QueryString["pqGrid_OrderField"];//排序字段名
            string pqGrid_OrderType = context.Request.QueryString["pqGrid_OrderType"];//排序方式 asc desc
            string pqGrid_Sort = context.Request.QueryString["pqGrid_Sort"];//需要查询的字段
            string pqGrid = context.Request.QueryString["_"];
            if (pqGrid_OrderField == null || pqGrid_OrderField == "")//排序字段不能为空
            {
                pqGrid_OrderField = "AddTime";
            }
            int OrderType = pqGrid_OrderType == "DESC" ? 0 : 1;



            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append("  AdminHotelid = '" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");


            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {

                        if (nv[0] == "start")
                        {
                            sb.Append(String.Format(" and CONVERT(varchar(100), AddTime, 23)>='{0}' ", nv[1]));
                        }
                        else if (nv[0] == "end")
                        {
                            sb.Append(String.Format(" and CONVERT(varchar(100), AddTime, 23)<='{0} 23:59:59' ", nv[1]));
                        }
                        else if (nv[0] == "type")
                        {
                            //sb.Append(String.Format(" and type={0} ", nv[1]));
                        }
                        else if (nv[0] == "text")
                        {
                            sb.Append(" and (Name like '%" + nv[1] + "%' or Number like '%" + nv[1] + "%' or CardNum like '%" + nv[1] + "%' )");
                        }

                    }
                }
            }

            //DataTable dt = DataFactory.SqlDataBase().GetPageLists("SELECT * FROM (SELECT Number,Phone AS CardNum,Name,Phone AS sjhm,Monery,Detail AS sm,'' as bz,wxddh,AddTime,Id,AdminHotelid  FROM dbo.Finance WHERE (Type='4' or Type='41')) AS a WHERE 1=1   " + sb + " ", pqGrid_Sort, pqGrid_OrderField, pqGrid_OrderType, PageIndex, PageSize, ref totalRecords);

            DataTable dt = DataFactory.SqlDataBase().DbPager("V_CardMoneyList", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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
                            if (dt.Rows[i][j] != null && dt.Rows[i][j].ToString() != "")
                            {
                                cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm"));
                            }
                            else
                            {
                                cs.Add("");
                            }
                        }
                        //else if (dt.Columns[j].DataType.Name == "decimal")
                        //{
                        //    if (dt.Rows[i][j] != null && dt.Rows[i][j].ToString() != "")
                        //    {
                        //        cs.Add(Convert.ToInt32(dt.Rows[i][j]));
                        //    }
                        //    else
                        //    {
                        //        cs.Add("");
                        //    }
                        //}
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



        #region ** 更新打印小票时间 **
        private void UpdatePrintTime(HttpContext context)
        {
            string RId = context.Request["RId"];
            string PrintTime = context.Request["PrintTime"]; 
            Hashtable hs = new Hashtable();
            hs["PrintTime"] = PrintTime.ToString();//打印时间;
            DataFactory.SqlDataBase().UpdateByHashtable("ReceiptInfo", "ID", RId.ToString(), hs);
            context.Response.Write("1");
        }
        #endregion

        #region ** 扫码记录 **

        /// <summary>
        /// 扫码记录
        /// </summary>
        /// <param name="context"></param>
        private void MemberSourceList(HttpContext context)
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
            string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();

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
                            sb.AppendFormat(" AND User_Name like '%{0}%'", nv[1]);
                        }
                        if (nv[0] == "openid")
                        {
                            sb.AppendFormat(" AND openid = '{0}'", nv[1]);
                        }
                    }
                }
            }
            DataTable dt = DataFactory.SqlDataBase().DbPager("V_MemberSource", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);
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



        #endregion


        #region ** 操作记录 **

        /// <summary>
        /// 操作记录
        /// </summary>
        /// <param name="context"></param>
        private void OperationLogList(HttpContext context)
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
            string type = context.Request.QueryString["type"];
            string pqGrid_OrderField = context.Request.QueryString["pqGrid_OrderField"];//排序字段名
            string pqGrid_OrderType = context.Request.QueryString["pqGrid_OrderType"];//排序方式 asc desc
            string pqGrid_Sort = context.Request.QueryString["pqGrid_Sort"];//需要查询的字段
            string pqGrid = context.Request.QueryString["_"];
            if (pqGrid_OrderField == null || pqGrid_OrderField == "")//排序字段不能为空
            {
                pqGrid_OrderField = "OperationDate";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;
            string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();

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
                            sb.AppendFormat(" AND Remarks like '%{0}%'", nv[1]);
                        }
                        if (nv[0] == "openid")
                        {
                            sb.AppendFormat(" AND OpenId = '{0}'", nv[1]);
                        }
                    }
                }
            }
            DataTable dt = DataFactory.SqlDataBase().DbPager("MemberOperationLog", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);
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



        #endregion


        #region ** 积分记录 **

        /// <summary>
        /// 积分记录
        /// </summary>
        /// <param name="context"></param>
        private void GetIntegraList(HttpContext context)
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
            string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();

            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(@"
            SELECT  b.lsh ,
                    b.kh ,
                    b.xm ,
                    b.sjhm ,
                    a.zmsm ,
                    a.czrq ,
                    a.jf ,
                    a.bz
            FROM    HY_HYXFJLB a
                    LEFT JOIN hy_hyzlxxb b ON a.lsh = b.lsh
            WHERE   jf <> 0
            ");

            IList<SqlParam> ilParams = new List<SqlParam>();

            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {
                        if (nv[0] == "text")
                        {
                            sb.AppendFormat(" AND a.bz like @bz", nv[1]);
                            ilParams.Add(new SqlParam("@bz", "%" + nv[1] + "%"));
                        }
                        if (nv[0] == "lsh")
                        {
                            sb.AppendFormat(" AND b.lsh = @lsh", nv[1]);
                            ilParams.Add(new SqlParam("@lsh", nv[1]));
                        }
                    }
                }
            }

            SqlParam[] param = ilParams.ToArray();
            //ORDER BY czrq DESC

            //会员信息
            DataTable dt = DataFactory.SqlDataBase(AdminHotelid).GetPageList(sb.ToString(), param, pqGrid_OrderField, pqGrid_OrderType, PageIndex, PageSize, ref totalRecords);

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



        #endregion

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="context"></param>
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
                Search = context.Request.QueryString["Search"].Split('|');//name≌value|name≌value
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
                pqGrid_OrderField = "addtime";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sql = new StringBuilder();//统计会员卡信息
            sql.AppendFormat("SELECT ISNULL(f.MemberId,0) AS MemberId,(SELECT ISNULL(SUM(Monery),0) FROM Finance WHERE Type=4 AND AdminHotelid='{0}' and 1=1 and 2=2  AND MemberId=f.MemberId) AS ljcz,(SELECT ISNULL(SUM(Monery),0) FROM Finance WHERE Type=9 AND AdminHotelid='{0}' and 1=1 and 2=2 AND MemberId=f.MemberId) AS czzs,(SELECT ISNULL(SUM(Monery),0) FROM Finance WHERE Type=2 AND AdminHotelid='{0}' and 1=1 and 2=2 AND MemberId=f.MemberId) AS ljxf FROM Finance f WHERE f.AdminHotelid='{0}' and 1=1 and 2=2 GROUP BY f.MemberId", RequestSession.GetSessionUser().AdminHotelid.ToString());
            StringBuilder sb = new StringBuilder();//查询条件
            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {
                        if (nv[0] == "start")
                        {
                            sb.AppendFormat(" and CONVERT(varchar(100), hy.AddTime, 23)>='{0}' ", nv[1]);
                        }
                        else if (nv[0] == "end")
                        {
                            sb.AppendFormat(" and CONVERT(varchar(100), hy.AddTime, 23)<='{0}' ", nv[1]);
                        }
                        else if (nv[0] == "text")
                        {
                            sb.Append(" and (hy.xm like '%" + nv[1] + "%' or hy.kh like '%" + nv[1] + "%' or hy.sjhm like '%" + nv[1] + "%' ) ");
                        }
                    }
                }
            }
            sb.AppendFormat(" and hy.AdminHotelid='{0}' ", RequestSession.GetSessionUser().AdminHotelid.ToString());
            //会员信息
            DataTable dt = DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).GetPageLists("SELECT hy.lsh,hy.kh,hy.xm,hy.sjhm,mb.hylxname,CONVERT(varchar(100), hy.addtime, 23) AS addtime,ISNULL(hy.hykye,0) AS hykye FROM hy_hyzlxxb hy,hy_hylxbmb mb WHERE hy.hylx=mb.hylxcode  and hy.carid is not null  " + sb, pqGrid_Sort, pqGrid_OrderField, pqGrid_OrderType, PageIndex, PageSize, ref totalRecords);

            //会员卡信息
            DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            var reslut = (from b in dt.AsEnumerable()
                          join a in dts.AsEnumerable()
                          on new { temp = b.Field<object>("lsh").ToString() } equals new { temp = a.Field<object>("MemberId").ToString() }
                          into temp
                          from t in temp.DefaultIfEmpty()
                          select
                          new
                          {
                              kh = b.Field<object>("kh"),
                              xm = b.Field<object>("xm"),
                              sjhm = b.Field<object>("sjhm"),
                              hylxname = b.Field<object>("hylxname"),
                              addtime = b.Field<object>("addtime"),
                              ljcz = t == null ? 0 : t["ljcz"],
                              czzs = t == null ? 0 : t["czzs"],
                              ljxf = t == null ? 0 : t["ljxf"],
                              hykye = b.Field<object>("hykye"),
                              lsh = b.Field<object>("lsh")
                          }).ToList();

            ArrayList data = new ArrayList();

            foreach (var obj in reslut)
            {
                ArrayList list = new ArrayList();

                list.Add(obj.kh);
                list.Add(obj.xm);
                list.Add(obj.sjhm);
                list.Add(obj.hylxname);
                list.Add(obj.addtime);
                list.Add(obj.ljcz);
                list.Add(obj.czzs);
                list.Add(obj.ljxf);
                list.Add(obj.hykye);
                list.Add(obj.lsh);
                data.Add(list);
            }


            PqGridHelper pq = new PqGridHelper();
            pq.totalRecords = totalRecords;
            pq.curPage = PageIndex;
            pq.data = data;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(pq);
            context.Response.Write(json);
        }



        private void GetInfoLists(HttpContext context)
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
            string type = context.Request.QueryString["type"];
            string pqGrid_OrderField = context.Request.QueryString["pqGrid_OrderField"];//排序字段名
            string pqGrid_OrderType = context.Request.QueryString["pqGrid_OrderType"];//排序方式 asc desc
            string pqGrid_Sort = context.Request.QueryString["pqGrid_Sort"];//需要查询的字段
            string pqGrid = context.Request.QueryString["_"];
            if (pqGrid_OrderField == null || pqGrid_OrderField == "")//排序字段不能为空
            {
                pqGrid_OrderField = "addtime";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;


            string lsh = context.Request["lsh"];
            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" and AdminHotelid = '" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");
            sb.Append(" and MemberId = '" + lsh + "' ");

            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {



                        if (nv[0] == "start")
                        {
                            sb.Append(String.Format(" and CONVERT(varchar(100), AddTime, 23)>='{0}' ", nv[1]));
                        }
                        else if (nv[0] == "end")
                        {
                            sb.Append(String.Format(" and CONVERT(varchar(100), AddTime, 23)<='{0} 23:59:59' ", nv[1]));
                        }
                        else if (nv[0] == "type")
                        {
                            sb.Append(String.Format(" and type={0} ", nv[1]));
                        }
                        else if (nv[0] == "text")
                        {
                            sb.Append(" and number like '%" + nv[1] + "%' ");
                        }

                    }
                }
            }

            DataTable dt = DataFactory.SqlDataBase().GetPageLists("SELECT id,Number,payname,AddTime,CASE Type WHEN 4 THEN Monery WHEN 8 THEN Monery WHEN 9 THEN Monery WHEN 44 THEN Monery ELSE NULL END AS czje,CASE Type WHEN 2 THEN Monery WHEN 41 THEN Monery WHEN 42 THEN Monery  WHEN 45 THEN Monery ELSE NULL END AS xfje,Detail FROM V_Finance WHERE Type IN (4,8,9,2,41,42,44,45)  " + sb, pqGrid_Sort, pqGrid_OrderField, pqGrid_OrderType, PageIndex, PageSize, ref totalRecords);



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



        private void GetInfoListss(HttpContext context)
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
            string lsh = context.Request["lsh"];
            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" 1 = 1 ");
            //if (Search != null && Search.Length > 0)
            //{
            //    for (int i = 0; i < Search.Length; i++)
            //    {
            //        string[] nv = Search[i].Split('@');
            //        if (nv.Length == 2)
            //        {
            //            var cs = "@obj" + i;
            //            if (nv[0] == "ordernum")
            //                sb.Append(" and (OrderNum like '%" + nv[1] + "%' or hotelname like '%" + nv[1] + "%' or Name like '%" + nv[1] + "%' or Mobile like '%" + nv[1] + "%'  )");
            //            if (nv[0] == "branch")
            //                sb.Append(" and hotelname ='" + nv[1] + "'");
            //            if (nv[0] == "roomType")
            //                sb.Append(" and RoomType ='" + nv[1] + "'");
            //            if (nv[0] == "Pay")
            //                sb.Append(" and Pay ='" + nv[1] + "'");
            //        }
            //    }
            //}

            if (RequestSession.GetSessionUser() == null)
            {
                context.Response.Write("");
                return;
            }

            sb.Append(" and AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");
            sb.Append(" and MemberId = '" + lsh + "' ");

            string sqls = string.Format("select hotelid from Base_UserInfo where AdminHotelid='{0}'", RequestSession.GetSessionUser().AdminHotelid.ToString());
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls));
            if (ds != null && ds.Rows.Count > 0)
            {
                if (ds.Rows[0]["hotelid"] != null && ds.Rows[0]["hotelid"].ToString() != "" && ds.Rows[0]["hotelid"].ToString() != "0")
                {
                    sb.AppendFormat("and hotelid='{0}'", ds.Rows[0]["hotelid"]);
                }
            }


            DataTable dt = DataFactory.SqlDataBase().DbPager("V_Reservations", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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

        private void GetInfoListsR(HttpContext context)
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
            string type = context.Request.QueryString["type"];
            string pqGrid_OrderField = context.Request.QueryString["pqGrid_OrderField"];//排序字段名
            string pqGrid_OrderType = context.Request.QueryString["pqGrid_OrderType"];//排序方式 asc desc
            string pqGrid_Sort = context.Request.QueryString["pqGrid_Sort"];//需要查询的字段
            string pqGrid = context.Request.QueryString["_"];
            if (pqGrid_OrderField == null || pqGrid_OrderField == "")//排序字段不能为空
            {
                pqGrid_OrderField = "AddTime";
            }
            int OrderType = pqGrid_OrderType == "DESC" ? 0 : 1;



            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append("  AdminHotelid = '" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");


            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {



                        if (nv[0] == "start")
                        {
                            sb.Append(String.Format(" and CONVERT(varchar(100), AddTime, 23)>='{0}' ", nv[1]));
                        }
                        else if (nv[0] == "end")
                        {
                            sb.Append(String.Format(" and CONVERT(varchar(100), AddTime, 23)<='{0} 23:59:59' ", nv[1]));
                        }
                        else if (nv[0] == "type")
                        {
                            //sb.Append(String.Format(" and type={0} ", nv[1]));
                        }
                        else if (nv[0] == "text")
                        {
                            sb.Append(" and (Name like '%" + nv[1] + "%' or Number like '%" + nv[1] + "%' or CardNum like '%" + nv[1] + "%' )");
                        }

                    }
                }
            }

            //DataTable dt = DataFactory.SqlDataBase().GetPageLists("SELECT * FROM (SELECT Number,Phone AS CardNum,Name,Phone AS sjhm,Monery,Detail AS sm,'' as bz,wxddh,AddTime,Id,AdminHotelid  FROM dbo.Finance WHERE (Type='4' or Type='41')) AS a WHERE 1=1   " + sb + " ", pqGrid_Sort, pqGrid_OrderField, pqGrid_OrderType, PageIndex, PageSize, ref totalRecords);

            DataTable dt = DataFactory.SqlDataBase().DbPager("V_RechargeList", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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
                            if (dt.Rows[i][j] != null && dt.Rows[i][j].ToString() != "")
                            {
                                cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm"));
                            }
                            else
                            {
                                cs.Add("");
                            }
                        }
                        //else if (dt.Columns[j].DataType.Name == "decimal")
                        //{
                        //    if (dt.Rows[i][j] != null && dt.Rows[i][j].ToString() != "")
                        //    {
                        //        cs.Add(Convert.ToInt32(dt.Rows[i][j]));
                        //    }
                        //    else
                        //    {
                        //        cs.Add("");
                        //    }
                        //}
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