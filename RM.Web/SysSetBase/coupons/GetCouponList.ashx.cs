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
    /// GetCouponList 的摘要说明
    /// </summary>
    /// <summary>
    /// GetCouponList 的摘要说明
    /// </summary>
    public class GetCouponList : IHttpHandler, IRequiresSessionState
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
                case "GetCouponList":
                    GetCouponLists(context);
                    break;
                case "GetCoupon":
                    GetCouponListss(context);
                    break;
                case "GetPreferentialType":
                    GetPreferentialType(context);
                    break;
                case "GetMattersCoupon":
                    GetMattersCoupon(context);
                    break;
                case "GetCouponType":
                    GetCouponType(context);
                    break;
                case "SetCouponTypeSort":
                    SetCouponTypeSort(context);
                    break;
                case "DeleteSetProKind":
                    SetCouponTypeSort(context);
                    break;
                case "GiftCoupons":
                    GiftCoupons(context);
                    break;
                case "GiftCouponss":
                    GiftCouponss(context);
                    break;
                default:
                    break;
            }
        }


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
                pqGrid_OrderField = "EffectiveDateTime";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" AdminHotelid = '" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");
            sb.Append(" and AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");
            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('≌');
                    if (nv.Length == 2)
                    {
                        var cs = "@obj" + i;
                        if (nv[0] == "number")
                        {
                            sb.Append(" and (ordernum like '%" + nv[1] + "%' or couponname like '%" + nv[1] + "%')");
                        }
                        else
                        {
                            sb.Append(" and (" + nv[0] + " = '" + nv[1] + "' or isReceive ='" + nv[1] + "') ");
                        }
                    }
                }
            }

            DataTable dt = DataFactory.SqlDataBase().DbPager("v_ClientCoupon", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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



        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="context"></param>
        private void GetCouponLists(HttpContext context)
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
                pqGrid_OrderField = "Id";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" AdminHotelid = '" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");
            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('≌');
                    if (nv.Length == 2)
                    {
                        var cs = "@obj" + i;
                        if (nv[0] == "number")
                        {
                            sb.Append(" and (TypeName like '%" + nv[1] + "%')");
                        }
                        else
                        {
                            sb.Append(" and (" + nv[0] + " = '" + nv[1] + "') ");
                        }
                    }
                }
            }


            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = DataFactory.SqlDataBase().GetPageList("select * from V_CouponList where " + sb, IList_param.ToArray(), pqGrid_OrderField, "desc", PageIndex, PageSize, ref totalRecords);

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


        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="context"></param>
        private void GetCouponListss(HttpContext context)
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
                pqGrid_OrderField = "Id";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" AdminHotelid = '" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");
            sb.Append(" and AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");
            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('≌');
                    if (nv.Length == 2)
                    {
                        var cs = "@obj" + i;
                        if (nv[0] == "number")
                        {
                            sb.Append(" and (Couponname like '%" + nv[1] + "%')");
                        }
                        else
                        {
                            sb.Append(" and (" + nv[0] + " = '" + nv[1] + "') ");
                        }
                    }
                }
            }


            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = DataFactory.SqlDataBase().GetPageList("SELECT * FROM V_coupon where " + sb, IList_param.ToArray(), pqGrid_OrderField, "desc", PageIndex, PageSize, ref totalRecords);

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
                                cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                cs.Add("");
                            }
                        }
                        else if (dt.Columns[j].ColumnName=="ISHY")
                        {
                            if (dt.Rows[i][j] != null && dt.Rows[i][j].ToString() != "")
                            {
                                cs.Add(gethy(dt.Rows[i][j].ToString()));
                            }
                            else
                            {
                                cs.Add("无");
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


        private string gethy(string ishy) 
        {
            string strgethy = "";
            ishy = ishy.TrimEnd(',');
            ishy = ishy.Replace(",", "','");
            ishy = "'" + ishy + "'";
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * FROM dbo.hy_hylxbmb WHERE hylxcode IN({0}) ",ishy);
            DataTable dt = DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0) 
            {
                for (int i = 0; i < dt.Rows.Count; i++) 
                {
                    strgethy += dt.Rows[i]["hylxname"].ToString()+",";
                }
            }
            strgethy = strgethy.TrimEnd(',');
            return strgethy;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="context"></param>
        private void GetPreferentialType(HttpContext context)
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
                pqGrid_OrderField = "Id";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" 1 = 1 ");
            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('≌');
                    if (nv.Length == 2)
                    {
                        var cs = "@obj" + i;
                        if (nv[0] == "number")
                        {
                            sb.Append(" and (TypeName like '%" + nv[1] + "%')");
                        }
                        else
                        {
                            sb.Append(" and (" + nv[0] + " = '" + nv[1] + "') ");
                        }
                    }
                }
            }


            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = DataFactory.SqlDataBase().GetPageList("SELECT id,TypeName FROM PreferentialType where " + sb, IList_param.ToArray(), pqGrid_OrderField, "desc", PageIndex, PageSize, ref totalRecords);

            ArrayList data = new ArrayList();
            if (dt != null && dt.Rows.Count > 0)
            {


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ArrayList cs = new ArrayList();
                    for (int j = 1; j < dt.Columns.Count; j++)
                    {
                        cs.Add(dt.Rows[i][j].ToString());
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
        /// 优惠券信息设置
        /// </summary>
        /// <param name="context"></param>
        private void GetMattersCoupon(HttpContext context)
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
                pqGrid_OrderField = "Id";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" AdminHotelid = '" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");
            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('≌');
                    if (nv.Length == 2)
                    {
                        var cs = "@obj" + i;
                        if (nv[0] == "number")
                        {
                            sb.Append(" and (Couponname like '%" + nv[1] + "%')");
                        }
                        else
                        {
                            sb.Append(" and (" + nv[0] + " = '" + nv[1] + "') ");
                        }
                    }
                }
            }

            IList<SqlParam> IList_param = new List<SqlParam>();

            DataTable dt = DataFactory.SqlDataBase().GetPageList("SELECT * from V_zsyhq where " + sb, IList_param.ToArray(), pqGrid_OrderField, "desc", PageIndex, PageSize, ref totalRecords);

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
                                cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd"));
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


        /// <summary>
        /// 获取卡券类型
        /// </summary>
        /// <param name="context"></param>
        private void GetCouponType(HttpContext context)
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
                pqGrid_OrderField = "Sort";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" 1 = 1  and DeleteMark =1 ");

            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {
                        var cs = "@obj" + i;
                        if (nv[0] == "Name")
                        {
                            sb.Append(" and Name like '%" + nv[1] + "%' ");
                        }
                        else
                        {
                            sb.Append(" and " + nv[0] + " = '" + nv[1] + "' ");
                        }
                    }
                }
            }
            sb.Append(" and AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");
            DataTable dt = DataFactory.SqlDataBase().DbPager("CouponType", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);
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


        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="context"></param>
        private void SetCouponTypeSort(HttpContext context)
        {
            string type = context.Request.QueryString["type"];
            string ProKindId = context.Request.QueryString["ProKindId"];
            string AdminHotelid = context.Request.QueryString["AdminHotelid"];
            string HotelId = context.Request.QueryString["HotelId"];
            Hashtable ht = new Hashtable();
            ht["Type"] = type;
            ht["ProKindId"] = ProKindId;
            ht["AdminHotelid"] = AdminHotelid;

            DataTable dt = DataFactory.SqlDataBase().GetDataTableProc("P_SetCouponTypeSort", ht);
            if (dt != null && dt.Rows.Count > 0)
            {
                context.Response.Write("1");
            }
            else
            {
                context.Response.Write("0");
            }
        }

        /// <summary>
        /// 是否可以删除当前卡券类型栏目
        /// </summary>
        /// <param name="context"></param>
        private void DeleteSetProKind(HttpContext context)
        {
            string ProKindId = context.Request.QueryString["ProKindId"];
            StringBuilder sb = new StringBuilder();
            sb.Append(@" SELECT ID  FROM    dbo.Coupon  WHERE   TypeID= @ProKindId ");
            SqlParam[] param = new SqlParam[]{
                new SqlParam("@ProKindId", ProKindId)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                context.Response.Write("0");
            }
            else
            {
                context.Response.Write("1");
            }
        }


        private void GiftCoupons(HttpContext context) 
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string Id = context.Request["id"];
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT TOP 1 * FROM GiftCoupons WHERE AdminHotelid='{0}' ORDER BY ID DESC ",AdminHotelid);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            if (dt != null && dt.Rows.Count > 0) 
            {
                if (dt.Rows[0]["zssj"].ToString() != "") 
                {
                    if (Convert.ToDateTime(dt.Rows[0]["zssj"]).ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd")) 
                    {
                        return;
                    }
                }

                StringBuilder sqlupdate = new StringBuilder();
                sqlupdate.AppendFormat("UPDATE dbo.GiftCoupons SET IsGift=1 WHERE AdminHotelid='{0}' and id={1} ", AdminHotelid, dt.Rows[0]["id"]);
                DataFactory.SqlDataBase().ExecuteBySql(sqlupdate);

                if (dt.Rows[0]["dxlx"].ToString() == "0")
                {
                    StringBuilder hysql = new StringBuilder();
                    hysql.AppendFormat("SELECT lsh,kh FROM dbo.hy_hyzlxxb WHERE CarId IS NOT NULL  ");
                    if (dt.Rows[0]["dxlx"].ToString() != "0") 
                    {
                        hysql.AppendFormat(" and ',{0},' LIKE '%,'+hylx+',%' ", dt.Rows[0]["hydj"]);
                    }
                    DataTable hydt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(hysql);

                    if (hydt != null && hydt.Rows.Count > 0) 
                    {
                        hydt = ScreenXFCS(hydt, dt.Rows[0]["xfcs"].ToString(), AdminHotelid);
                        hydt = ScreenXFJG(hydt, dt.Rows[0]["xfjg"].ToString(), AdminHotelid, dt.Rows[0]["StartData"].ToString(), dt.Rows[0]["EndData"].ToString());
                    }
                    if (hydt != null && hydt.Rows.Count > 0) 
                    {
                        for (int i = 0; i < hydt.Rows.Count;i++ ) 
                        {
                            Gift(hydt.Rows[i]["kh"].ToString(), Convert.ToInt32(Id), AdminHotelid);
                        }
                    }
                }
                else 
                {
                    Gift(dt.Rows[0]["sjhm"].ToString(), Convert.ToInt32(Id), AdminHotelid);
                }
            }

        }

        public DataTable ScreenXFCS(DataTable dt, string xfcs, string AdminHotelid) 
        {
            if (xfcs == "0")
            {
                return dt;
            }
            DataTable hy = new DataTable();
            hy.Columns.Add("lsh", typeof(System.String));
            hy.Columns.Add("kh", typeof(System.String));
            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("SELECT COUNT(1) cs FROM dbo.Reservation WHERE MemberId='{0}' AND AdminHotelid='{1}'", dt.Rows[i]["lsh"],AdminHotelid);
                DataTable cs = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
                if (cs != null && cs.Rows.Count > 0) 
                {
                    int hyxfcs= Convert.ToInt32(cs.Rows[0][0]);
                    if (xfcs == "1")
                    {
                        if (hyxfcs >= 1 && hyxfcs <= 3)
                        {
                            DataRow row = hy.NewRow();
                            row["lsh"] = dt.Rows[i]["lsh"].ToString();
                            row["kh"] = dt.Rows[i]["kh"].ToString();
                            hy.Rows.Add(row);
                        }
                    }
                    else if (xfcs == "2") 
                    {
                        if (hyxfcs >= 4 && hyxfcs <= 6)
                        {
                            DataRow row = hy.NewRow();
                            row["lsh"] = dt.Rows[i]["lsh"].ToString();
                            row["kh"] = dt.Rows[i]["kh"].ToString();
                            hy.Rows.Add(row);
                        }
                    }
                    else if (xfcs == "3")
                    {
                        if (hyxfcs >= 7 && hyxfcs <= 10)
                        {
                            DataRow row = hy.NewRow();
                            row["lsh"] = dt.Rows[i]["lsh"].ToString();
                            row["kh"] = dt.Rows[i]["kh"].ToString();
                            hy.Rows.Add(row);
                        }
                    }
                    else if (xfcs == "4")
                    {
                        if (hyxfcs >10)
                        {
                            DataRow row = hy.NewRow();
                            row["lsh"] = dt.Rows[i]["lsh"].ToString();
                            row["kh"] = dt.Rows[i]["kh"].ToString();
                            hy.Rows.Add(row);
                        }
                    }

                }
            }

            return hy;
        }

        public DataTable ScreenXFJG(DataTable dt, string xfjg, string AdminHotelid, string starttime=null, string endtime=null)
        {
            if (xfjg == "0")
            {
                return dt;
            }
            DataTable hy = new DataTable();
            hy.Columns.Add("lsh", typeof(System.String));
            hy.Columns.Add("kh", typeof(System.String));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("SELECT COUNT(1) cs FROM dbo.Reservation WHERE MemberId='{0}' AND AdminHotelid='{1}'", dt.Rows[i]["lsh"], AdminHotelid);
                DateTime start=new DateTime();
                DateTime end=new DateTime();
                if (xfjg == "1")
                {
                    start = DateTime.Now.AddMonths(-1);
                    end = DateTime.Now;
                }
                else if (xfjg == "2") 
                {
                    start = DateTime.Now.AddMonths(-2);
                    end = DateTime.Now;
                }
                else if (xfjg == "3")
                {
                    start = DateTime.Now.AddMonths(-3);
                    end = DateTime.Now;
                }
                else if (xfjg == "4")
                {
                    start = DateTime.Now.AddMonths(-4);
                    end = DateTime.Now;
                }
                else if (xfjg == "5")
                {
                    start = Convert.ToDateTime(starttime);
                    end = Convert.ToDateTime(endtime);
                }
                sql.AppendFormat("AND AddTime>='{0}' AND AddTime<='{1}' ", start, end);
                DataTable cs = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
                if (cs != null && cs.Rows.Count > 0)
                {
                    int hyxfcs = Convert.ToInt32(cs.Rows[0][0]);
                    if (hyxfcs == 0)
                    {
                        DataRow row = hy.NewRow();
                        row["lsh"] = dt.Rows[i]["lsh"].ToString();
                        row["kh"] = dt.Rows[i]["kh"].ToString();
                        hy.Rows.Add(row);
                    }
                }
            }
            return hy;
        }

        private void GiftCouponss(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string Id = context.Request["id"];
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT TOP 1 * FROM GiftCoupons WHERE AdminHotelid='{0}' ORDER BY ID DESC ", AdminHotelid);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder sqlupdate = new StringBuilder();
                sqlupdate.AppendFormat("UPDATE dbo.GiftCoupons SET IsGift=1 WHERE AdminHotelid='{0}' ", AdminHotelid);
                DataFactory.SqlDataBase().ExecuteBySql(sqlupdate);

                if (dt.Rows[0]["dxlx"].ToString() == "0")
                {
                    StringBuilder hysql = new StringBuilder();
                    hysql.AppendFormat("SELECT lsh,kh FROM dbo.hy_hyzlxxb WHERE CarId IS NOT NULL  ");
                    if (dt.Rows[0]["dxlx"].ToString() != "0")
                    {
                        hysql.AppendFormat(" and ',{0},' LIKE '%,'+hylx+',%' ", dt.Rows[0]["hydj"]);
                    }
                    DataTable hydt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(hysql);

                    if (hydt != null && hydt.Rows.Count > 0)
                    {
                        hydt = ScreenXFCS(hydt, dt.Rows[0]["xfcs"].ToString(), AdminHotelid);
                        hydt = ScreenXFJG(hydt, dt.Rows[0]["xfjg"].ToString(), AdminHotelid, dt.Rows[0]["StartData"].ToString(), dt.Rows[0]["EndData"].ToString());
                    }
                    if (hydt != null && hydt.Rows.Count > 0)
                    {
                        for (int i = 0; i < hydt.Rows.Count; i++)
                        {
                            Gifts(hydt.Rows[i]["kh"].ToString(), Convert.ToInt32(Id), AdminHotelid);
                        }
                    }

                }
                else
                {
                    Gifts(dt.Rows[0]["sjhm"].ToString(), Convert.ToInt32(Id), AdminHotelid);
                }
            }

        }

        public void Gift(string kh,int id,string AdminHotelid)
        {
            
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from hy_hyzlxxb  where CarId IS NOT NULL  ");
                
                
                string khs = kh;
                khs = "'" + khs.Replace("，", ",").TrimEnd(',').Replace(",", "','") + "'";

                sql.AppendFormat(" and kh in ({0}) or sjhm in ({0}) ", khs);


                DataTable dt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);

                if (dt.Rows.Count > 0)
                {
                    string sqlcp = string.Format(@"select * from V_XXZSYHQ where CID=@CID and AdminHotelid=@AdminHotelid ");

                    SqlParam[] parmAdd3 = new SqlParam[] { 
                                     new SqlParam("@CID", id),
                                     new SqlParam("@AdminHotelid", AdminHotelid)};
                    DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //赠送优惠券

                        if (dts != null && dts.Rows.Count > 0)
                        {
                            CommonMethod.AddCardCoupons(dt.Rows[i]["lsh"].ToString(), AdminHotelid, "指定赠送", dts, "10");
                        }

                    }
                }



        }

        public void Gifts(string kh, int id, string AdminHotelid)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from hy_hyzlxxb  where CarId IS NOT NULL  ");


            string khs = kh;
            khs = "'" + khs.Replace("，", ",").TrimEnd(',').Replace(",", "','") + "'";

            sql.AppendFormat(" and kh in ({0}) or sjhm in ({0}) ", khs);


            DataTable dt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);

            if (dt.Rows.Count > 0)
            {
                string sqlcp = string.Format(@"select * from V_XXZSYHQs where CID=@CID and AdminHotelid=@AdminHotelid ");

                SqlParam[] parmAdd3 = new SqlParam[] { 
                                     new SqlParam("@CID", id),
                                     new SqlParam("@AdminHotelid", AdminHotelid)};
                DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //赠送优惠券

                    if (dts != null && dts.Rows.Count > 0)
                    {
                        CommonMethod.AddCardCoupons(dt.Rows[i]["lsh"].ToString(), AdminHotelid, "指定赠送", dts, "19");
                    }

                }
            }



        }

        public void Gift(DataTable GiftCoupons, int id, string AdminHotelid)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from hy_hyzlxxb  where CarId IS NOT NULL  ");


            string khs = "";
            khs = "'" + khs.Replace("，", ",").TrimEnd(',').Replace(",", "','") + "'";

            sql.AppendFormat(" and kh in ({0}) or sjhm in ({0}) ", khs);


            DataTable dt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);

            if (dt.Rows.Count > 0)
            {
                string sqlcp = string.Format(@"select * from V_XXZSYHQ where CID=@CID and AdminHotelid=@AdminHotelid ");

                SqlParam[] parmAdd3 = new SqlParam[] { 
                                     new SqlParam("@CID", id),
                                     new SqlParam("@AdminHotelid", AdminHotelid)};
                DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //赠送优惠券

                    if (dts != null && dts.Rows.Count > 0)
                    {
                        CommonMethod.AddCardCoupons(dt.Rows[i]["lsh"].ToString(), AdminHotelid, "指定赠送", dts, "10");
                    }

                }
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