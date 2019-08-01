using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RM.Busines;
using System.Data;
using System.Collections;
using RM.Common.DotPqGrid;
using RM.Common.DotNetCode;
using System.Web.SessionState;
using RM.Common.DotNetBean;
using LitJson;
using RM.Web.App_Code;
namespace RM.Web.SysSetBase.Busin
{
    /// <summary>
    /// Book 的摘要说明
    /// </summary>
    public class Book : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Menu = context.Request["Menu"];                      //提交动作
            switch (Menu)
            {
                case "GetInfoList":  //酒店列表
                    GetInfoList(context);
                    break;
                case "SetSort":  //酒店排序
                    SetSort(context);
                    break;
                case "GetBookTypeList":   //类型列表
                    GetBookTypeList(context);
                    break;
                case "GetSetSort":   //类型排序
                    GetSetSort(context);
                    break;
                case "GetBookOrderList":   //类型排序
                    GetBookOrderList(context);
                    break;
                case "GetHotelInfo":  //酒店门店信息
                    GetHotelInfo(context);
                    break;

                case "GetBusinType":  //酒店门店信息
                    GetBusinType(context);
                    break;

                case "GetBusinImgList":  //营业点轮换
                    GetBusinImgList(context);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        ///获取营业点轮换图
        /// </summary>
        /// <param name="context"></param>
        private void GetBusinImgList(HttpContext context)
        {
            string BusinId = context.Request["BusinId"];
            string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();
            JsonData jsondata = new JsonData();
            string sql = string.Format(@"select ID, ImgFile from BusinPhoto where PID='{0}' and [type]='{1}' and AdminHotelid='{2}' order by ID asc", BusinId, (int)DefaultFilePath.SystemType.BusinPhoto, AdminHotelid);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            if (ds != null && ds.Rows.Count > 0)
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    JsonData json1 = new JsonData();
                    json1["ID"] = ds.Rows[i]["ID"].ToString();
                    json1["IMGFILE"] = ds.Rows[i]["ImgFile"].ToString();
                    jsondata.Add(json1);
                }
            }
            string json = "";
            json = jsondata.ToJson();
            context.Response.Write(json);
        }


        /// <summary>
        /// 获取客房信息列表
        /// </summary>
        /// <param name="context"></param>
        private void GetBusinType(HttpContext context)
        {
            string adminhotelid = context.Request["Adminhotelid"];
            string hotelid = context.Request["hotelid"];
            JsonData jsondata = new JsonData();

            StringBuilder str = new StringBuilder();
            str.Append(string.Format("  SELECT ID,BusinessName FROM Bookings  WHERE AdminHotelid=@AdminHotelid "));
            List<SqlParam> ilistStr = new List<SqlParam>();
            ilistStr.Add(new SqlParam("@AdminHotelid", adminhotelid));
            if (hotelid != "-1")
            {
                str.Append(" And Hotelid = @Hotelid");
                ilistStr.Add(new SqlParam("@Hotelid", hotelid));
            }
            str.Append("  ORDER BY ID DESC");
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(str, ilistStr.ToArray());
            if (ds != null && ds.Rows.Count > 0)
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    JsonData json1 = new JsonData();
                    json1["ID"] = ds.Rows[i]["ID"].ToString();
                    json1["TYPENAME"] = ds.Rows[i]["BusinessName"].ToString();

                    jsondata.Add(json1);
                }
            }
            string json = "";
            json = jsondata.ToJson();
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
            string hotelid = context.Request.QueryString["hotelid"];
            string type = context.Request.QueryString["type"];
            string content = context.Request.QueryString["content"];
            try
            {
                string pqGrid_PageIndex = context.Request.QueryString["pqGrid_PageIndex"];
                PageIndex = Convert.ToInt32(pqGrid_PageIndex);
                string pqGrid_PageSize = context.Request.QueryString["pqGrid_PageSize"];
                PageSize = Convert.ToInt32(pqGrid_PageSize);
                
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
                pqGrid_OrderField = "Sort";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sb = new StringBuilder();//查询条件

            sb.Append(" 1 = 1  and DeleteMark=1 ");

            sb.Append("  and  AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");

            if (hotelid != null && hotelid != "")
            {
                sb.Append("  and  HotelId='" + hotelid + "' ");
            }
            if (type != null && type != "-1")
            {
                sb.Append("  and  ID='" + type + "' ");
            }

            if (content != "" && content != null)
            {
                sb.AppendFormat(" and (BusinessName like '{0}' or TypeName like '{0}' or Telephone like '{0}' or BusinessTime like '{0}' or BusinessAddress like '{0}' )", "%" + content + "%");
            }


            DataTable dt = DataFactory.SqlDataBase().DbPager("V_Bookings", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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
        private void SetSort(HttpContext context)
        {
            if (RequestSession.GetSessionUser() == null)
            {
                context.Response.Write("");
                return;
            }

            string type = context.Request.QueryString["type"];
            string strValue = context.Request.QueryString["sv"];
            string[] str = strValue.Split('|');
            int id = int.Parse(str[0]);
            int sort = int.Parse(str[1]);
            int version = int.Parse(str[2]);
            StringBuilder sb = new StringBuilder();
            if (type == "up")
            {
                //向上动
                sb.Append("select top 1 ID,Sort from Bookings where Sort>@Sort  and AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid + "'    order by Sort asc");
            }
            else if (type == "down")
            {
                //向下动
                sb.Append("select top 1 ID,Sort from Bookings where Sort<@Sort and AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid + "'   order by Sort desc");
            }
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@sort", sort),
                                     new SqlParam("@version", version)};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parmAdd);
            if (dt != null && dt.Rows.Count > 0)
            {
                sb = new StringBuilder();
                sb.Append("update [Bookings] set Sort=@Sort1 where id=@id1 ; update [Bookings] set Sort=@Sort2 where id=@id2");
                parmAdd = new SqlParam[] { 
                                     new SqlParam("@sort1", dt.Rows[0]["Sort"].ToString()),
                                     new SqlParam("@id1", id),
                                     new SqlParam("@sort2", sort),
                                     new SqlParam("@id2", dt.Rows[0]["ID"].ToString())};

                int ojb = DataFactory.SqlDataBase().ExecuteBySql(sb, parmAdd);
                if (ojb > 0)
                {
                    context.Response.Write("1");
                }
                else
                {
                    context.Response.Write("0");
                }
            }
            else
            {
                context.Response.Write("0");
            }
        }




        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="context"></param>
        private void GetBookTypeList(HttpContext context)
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
         
            string hotelid = context.Request.QueryString["hotelid"];
            string content = context.Request.QueryString["content"];
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
            sb.Append(" 1 = 1   and DeleteMark=1 ");

            sb.Append("  and  AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");

            if (hotelid != null && hotelid != "")
            {
                sb.Append("  and  hotelid='" + hotelid + "' ");
            }

            if (content != "" && content != null)
            {
                sb.AppendFormat(" and (TypeName like '{0}' or Instructions like '{0}'  )", "%" + content + "%");
            }

            DataTable dt = DataFactory.SqlDataBase().DbPager("V_BookType", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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
        private void GetSetSort(HttpContext context)
        {
            string type = context.Request.QueryString["type"];
            string strValue = context.Request.QueryString["sv"];
            string[] str = strValue.Split('|');
            int id = int.Parse(str[0]);
            int sort = int.Parse(str[1]);
            int version = int.Parse(str[2]);
            StringBuilder sb = new StringBuilder();
            if (type == "up")
            {
                //向上动

                sb.Append("select top 1 ID,Sort from BookType where Sort>@Sort and Version=@Version  and AdminHotelid=@AdminHotelid order by Sort asc");

            }
            else if (type == "down")
            {
                //向下动

                sb.Append("select top 1 ID,Sort from BookType where Sort<@Sort and Version=@Version and AdminHotelid=@AdminHotelid  order by Sort desc");

            }
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@sort", sort),
                                     new SqlParam("@version", version),
                                     new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())   };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parmAdd);
            if (dt != null && dt.Rows.Count > 0)
            {
                sb = new StringBuilder();
                sb.Append("update [BookType] set Sort=@Sort1 where id=@id1 ; update [BookType] set Sort=@Sort2 where id=@id2");
                parmAdd = new SqlParam[] { 
                                     new SqlParam("@sort1", dt.Rows[0]["Sort"].ToString()),
                                     new SqlParam("@id1", id),
                                     new SqlParam("@sort2", sort),
                                     new SqlParam("@id2", dt.Rows[0]["ID"].ToString())};

                int ojb = DataFactory.SqlDataBase().ExecuteBySql(sb, parmAdd);
                if (ojb > 0)
                {
                    context.Response.Write("1");
                }
                else
                {
                    context.Response.Write("0");
                }
            }
            else
            {
                context.Response.Write("0");
            }
        }



        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="context"></param>
        private void GetBookOrderList(HttpContext context)
        {
            if (RequestSession.GetSessionUser() == null)
            {
                context.Response.Write("");
                return;
            }

            int totalRecords = 0;
            int PageIndex = 1;
            int PageSize = 10;

            string hotelid = context.Request.QueryString["hotelid"];
            string type = context.Request.QueryString["type"];
            string start = context.Request.QueryString["start"];
            string end = context.Request.QueryString["end"];
            string content = context.Request.QueryString["content"];
            try
            {
                string pqGrid_PageIndex = context.Request.QueryString["pqGrid_PageIndex"];
                PageIndex = Convert.ToInt32(pqGrid_PageIndex);
                string pqGrid_PageSize = context.Request.QueryString["pqGrid_PageSize"];
                PageSize = Convert.ToInt32(pqGrid_PageSize);

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
                pqGrid_OrderField = "Sort";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 0 : 1;

            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" 1 = 1  and DeleteMark=1 ");

            sb.Append("  and  AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "' ");

            if (hotelid != null && hotelid != "")
            {
                sb.Append("  and  HotelId='" + hotelid + "' ");
            }
            if ( type != null && type != "-1")
            {
                sb.Append("  and  BusinessId='" + type + "' ");
            }
            if (start != null && start != "" && end != null && end != "")
            {
                string sql = end + " 23:59:59";
                sb.AppendFormat(" and OrderTime>='{0} 00:00:00' and OrderTime<='{1}'", start, sql);
            }
            else
            {
                if (start != null && start != "")
                {
                    sb.AppendFormat(" and OrderTime>='{0} 00:00:00'", start);
                }
                else if (end != null && end != "")
                {
                    sb.AppendFormat("and OrderTime<='{0} 23:59:59'", end);
                }
            }

            if (content != "" && content != null)
            {
                sb.AppendFormat(" and (OrderNumber like '{0}' or BusinessName like '{0}' or Contact like '{0}' or ContactPhone like '{0}' or Address like '{0}' )", "%" + content + "%");
            }

            DataTable dt = DataFactory.SqlDataBase().DbPager("V_BookOrder", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);

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
        /// 获取酒店门店信息
        /// </summary>
        /// <param name="context"></param>
        private void GetHotelInfo(HttpContext context)
        {
            string adminhotelid = context.Request["adminhotelid"];
            JsonData jsondata = new JsonData();
            string sql = string.Format(@"select AdminHotelid,Name from Hotel_Admin where adminhotelid=@adminhotelid and DeleteMark=1 ");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid)};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (ds != null && ds.Rows.Count > 0)
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    JsonData json1 = new JsonData();
                    json1["ID"] = ds.Rows[i]["AdminHotelid"].ToString();
                    json1["NAME"] = ds.Rows[i]["Name"].ToString();
                    sql = string.Format(@"select id,name from Hotel where AdminHotelid=@AdminHotelid  and DeleteMark=1 order by sort desc");
                    SqlParam[] pmadd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", ds.Rows[i]["AdminHotelid"])};
                    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), pmadd);
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        json1["hotel"] = new JsonData();
                        for (int s = 0; s < dt.Rows.Count; s++)
                        {
                            JsonData json2 = new JsonData();
                            json2["id"] = dt.Rows[s]["id"].ToString();
                            json2["name"] = dt.Rows[s]["name"].ToString();
                            json1["hotel"].Add(json2);
                        }
                    }
                    jsondata.Add(json1);
                }
            }
            string json = "";
            json = jsondata.ToJson();
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