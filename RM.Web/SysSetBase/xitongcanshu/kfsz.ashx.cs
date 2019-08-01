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



namespace RM.Web.SysSetBase.xitongcanshu
{
    /// <summary>
    /// kfsz1 的摘要说明
    /// </summary>
    public class kfsz1 : IHttpHandler, IReadOnlySessionState
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
                case "update":
                    update(context);
                    break;
                case "GetNetwork":
                    GetNetwork(context);
                    break;
                case "GetRoom":
                    GetRoom(context);
                    break;
                case "GetCenter":
                    GetCenter(context);
                    break;
                case "GetBook":
                    GetBook(context);
                    break;
                case "GetSubmit":
                    GetSubmit(context);
                    break;
                default:
                    break;
            }

        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="context"></param>
        private void GetSubmit(HttpContext context)
        {
            string a = "0";
            string AdminHotelid = context.Request["AdminHotelid"];
            string HotelId = context.Request["HotelId"]; //分店ID
            string IsNetwork = context.Request["IsNetwork"];
            string NetworkImg = context.Request["NetworkImg"];
            string NetworkType = context.Request["NetworkType"];
            string NetworkDay = context.Request["NetworkDay"];
            string NetworkName = context.Request["NetworkName"];
            string NetworkInfo = context.Request["NetworkInfo"];
            string IsRoom = context.Request["IsRoom"];
            string RoomImg = context.Request["RoomImg"];
            string RoomType = context.Request["RoomType"];
            string RoomDay = context.Request["RoomDay"];
            string RoomName = context.Request["RoomName"];
            string RoomInfo = context.Request["RoomInfo"];
            string IsCenter = context.Request["IsCenter"];
            string CenterImg = context.Request["CenterImg"];
            string CenterType = context.Request["CenterType"];
            string CenterDay = context.Request["CenterDay"];
            string CenterName = context.Request["CenterName"];
            string CenterInfo = context.Request["CenterInfo"];
            string IsBook = context.Request["IsBook"];
            string BookImg = context.Request["BookImg"];
            string BookType = context.Request["BookType"];
            string BookDay = context.Request["BookDay"];
            string BookName = context.Request["BookName"];
            string BookInfo = context.Request["BookInfo"];
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ID FROM TipsInfo where  AdminHotelid=@AdminHotelid ");
            SqlParam[] param = new SqlParam[] {
                                        new SqlParam("@AdminHotelid", AdminHotelid)};
            DataTable dstrs = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dstrs != null && dstrs.Rows.Count > 0)
            {

                Hashtable hs = new Hashtable();
                hs["IsNetwork"] = IsNetwork;
                hs["NetworkImg"] = NetworkImg;
                hs["NetworkType"] = NetworkType;
                hs["NetworkDay"] = NetworkDay;
                hs["NetworkName"] = NetworkName;
                hs["NetworkInfo"] = NetworkInfo;
                hs["IsRoom"] = IsRoom;
                hs["RoomImg"] = RoomImg;
                hs["RoomType"] = RoomType;
                hs["RoomDay"] = RoomDay;
                hs["RoomName"] = RoomName;
                hs["RoomInfo"] = RoomInfo;
                hs["IsCenter"] = IsCenter;
                hs["CenterImg"] = CenterImg;
                hs["CenterType"] = CenterType;
                hs["CenterDay"] = CenterDay;
                hs["CenterName"] = CenterName;
                hs["CenterInfo"] = CenterInfo;
                hs["IsBook"] = IsBook;
                hs["BookImg"] = BookImg;
                hs["BookType"] = BookType;
                hs["BookDay"] = BookDay;
                hs["BookName"] = BookName;
                hs["BookInfo"] = BookInfo;
                int b = DataFactory.SqlDataBase().UpdateByHashtable("TipsInfo", "ID", dstrs.Rows[0]["ID"].ToString(), hs);
                if (b > 0)
                {
                    a = "1";
                }
            }
            else
            {
                Hashtable ht = new Hashtable();
                ht["IsNetwork"] = IsNetwork;
                ht["NetworkImg"] = NetworkImg;
                ht["NetworkType"] = NetworkType;
                ht["NetworkDay"] = NetworkDay;
                ht["NetworkName"] = NetworkName;
                ht["NetworkInfo"] = NetworkInfo;
                ht["IsRoom"] = IsRoom;
                ht["RoomImg"] = RoomImg;
                ht["RoomType"] = RoomType;
                ht["RoomDay"] = RoomDay;
                ht["RoomName"] = RoomName;
                ht["RoomInfo"] = RoomInfo;
                ht["IsCenter"] = IsCenter;
                ht["CenterImg"] = CenterImg;
                ht["CenterType"] = CenterType;
                ht["CenterDay"] = CenterDay;
                ht["CenterName"] = CenterName;
                ht["CenterInfo"] = CenterInfo;
                ht["IsBook"] = IsBook;
                ht["BookImg"] = BookImg;
                ht["BookType"] = BookType;
                ht["BookDay"] = BookDay;
                ht["BookName"] = BookName;
                ht["BookInfo"] = BookInfo;
                ht["AdminHotelid"] = AdminHotelid;
                ht["HotelId"] = HotelId;
                int b = DataFactory.SqlDataBase().InsertByHashtable("TipsInfo", ht);
                if (b > 0)
                {
                    a = "1";
                }
            }
            context.Response.Write(a);
        }



        /// <summary>
        /// 进入订单页面是否弹出提示
        /// </summary>
        /// <param name="context"></param>
        private void GetBook(HttpContext context)
        {
            string a = "0";
            string AdminHotelid = context.Request["AdminHotelid"];
            string HotelId = context.Request["HotelId"]; //分店ID
            string IsBook = context.Request["IsBook"];
            string BookImg = context.Request["BookImg"];
            string BookType = context.Request["BookType"];
            string BookDay = context.Request["BookDay"];
            string BookName = context.Request["BookName"];
            string BookInfo = context.Request["BookInfo"];
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ID FROM TipsInfo where  AdminHotelid=@AdminHotelid ");
            SqlParam[] param = new SqlParam[] {
                                        new SqlParam("@AdminHotelid", AdminHotelid)};
            DataTable dstrs = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dstrs != null && dstrs.Rows.Count > 0)
            {

                Hashtable hs = new Hashtable();
                hs["IsBook"] = IsBook;
                hs["BookImg"] = BookImg;
                hs["BookType"] = BookType;
                hs["BookDay"] = BookDay;
                hs["BookName"] = BookName;
                hs["BookInfo"] = BookInfo;
                int b = DataFactory.SqlDataBase().UpdateByHashtable("TipsInfo", "ID", dstrs.Rows[0]["ID"].ToString(), hs);
                if (b > 0)
                {
                    a = "1";
                }
            }
            else
            {
                Hashtable ht = new Hashtable();
                ht["IsBook"] = IsBook;
                ht["BookImg"] = BookImg;
                ht["BookType"] = BookType;
                ht["BookDay"] = BookDay;
                ht["BookName"] = BookName;
                ht["BookInfo"] = BookInfo;
                ht["AdminHotelid"] = AdminHotelid;
                ht["HotelId"] = HotelId;
                int b = DataFactory.SqlDataBase().InsertByHashtable("TipsInfo", ht);
                if (b > 0)
                {
                    a = "1";
                }
            }
            context.Response.Write(a);
        }



        /// <summary>
        /// 进入会员中心是否弹出提示
        /// </summary>
        /// <param name="context"></param>
        private void GetCenter(HttpContext context)
        {
            string a = "0";
            string AdminHotelid = context.Request["AdminHotelid"];
            string HotelId = context.Request["HotelId"]; //分店ID
            string IsCenter = context.Request["IsCenter"];
            string CenterImg = context.Request["CenterImg"];
            string CenterType = context.Request["CenterType"];
            string CenterDay = context.Request["CenterDay"];
            string CenterName = context.Request["CenterName"];
            string CenterInfo = context.Request["CenterInfo"];
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ID FROM TipsInfo where  AdminHotelid=@AdminHotelid ");
            SqlParam[] param = new SqlParam[] {
                                        new SqlParam("@AdminHotelid", AdminHotelid)};
            DataTable dstrs = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dstrs != null && dstrs.Rows.Count > 0)
            {

                Hashtable hs = new Hashtable();
                hs["IsCenter"] = IsCenter;
                hs["CenterImg"] = CenterImg;
                hs["CenterType"] = CenterType;
                hs["CenterDay"] = CenterDay;
                hs["CenterName"] = CenterName;
                hs["CenterInfo"] = CenterInfo;
                int b = DataFactory.SqlDataBase().UpdateByHashtable("TipsInfo", "ID", dstrs.Rows[0]["ID"].ToString(), hs);
                if (b > 0)
                {
                    a = "1";
                }
            }
            else
            {
                Hashtable ht = new Hashtable();
                ht["IsCenter"] = IsCenter;
                ht["CenterImg"] = CenterImg;
                ht["CenterType"] = CenterType;
                ht["CenterDay"] = CenterDay;
                ht["CenterName"] = CenterName;
                ht["CenterInfo"] = CenterInfo;
                ht["AdminHotelid"] = AdminHotelid;
                ht["HotelId"] = HotelId;
                int b = DataFactory.SqlDataBase().InsertByHashtable("TipsInfo", ht);
                if (b > 0)
                {
                    a = "1";
                }
            }
            context.Response.Write(a);
        }

        /// <summary>
        /// 客房预订成功是否弹出提示
        /// </summary>
        /// <param name="context"></param>
        private void GetRoom(HttpContext context)
        {
            string a = "0";
            string AdminHotelid = context.Request["AdminHotelid"];
            string HotelId = context.Request["HotelId"]; //分店ID
            string IsRoom = context.Request["IsRoom"];
            string RoomImg = context.Request["RoomImg"];
            string RoomType = context.Request["RoomType"];
            string RoomDay = context.Request["RoomDay"];
            string RoomName = context.Request["RoomName"];
            string RoomInfo = context.Request["RoomInfo"];
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ID FROM TipsInfo where  AdminHotelid=@AdminHotelid ");
            SqlParam[] param = new SqlParam[] {
                                        new SqlParam("@AdminHotelid", AdminHotelid)};
            DataTable dstrs = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dstrs != null && dstrs.Rows.Count > 0)
            {

                Hashtable hs = new Hashtable();
                hs["IsRoom"] = IsRoom;
                hs["RoomImg"] = RoomImg;
                hs["RoomType"] = RoomType;
                hs["RoomDay"] = RoomDay;
                hs["RoomName"] = RoomName;
                hs["RoomInfo"] = RoomInfo;
                int b = DataFactory.SqlDataBase().UpdateByHashtable("TipsInfo", "ID", dstrs.Rows[0]["ID"].ToString(), hs);
                if (b > 0)
                {
                    a = "1";
                }
            }
            else
            {
                Hashtable ht = new Hashtable();
                ht["IsRoom"] = IsRoom;
                ht["RoomImg"] = RoomImg;
                ht["RoomType"] = RoomType;
                ht["RoomDay"] = RoomDay;
                ht["RoomName"] = RoomName;
                ht["RoomInfo"] = RoomInfo;
                ht["AdminHotelid"] = AdminHotelid;
                ht["HotelId"] = HotelId;
                int b = DataFactory.SqlDataBase().InsertByHashtable("TipsInfo", ht);
                if (b > 0)
                {
                    a = "1";
                }
            }
            context.Response.Write(a);
        }

        /// <summary>
        /// 首次进入微网是否弹出提示
        /// </summary>
        /// <param name="context"></param>
        private void GetNetwork(HttpContext context)
        {
            string a = "0";
            string AdminHotelid = context.Request["AdminHotelid"];
            string HotelId = context.Request["HotelId"]; //分店ID
            string IsNetwork = context.Request["IsNetwork"];
            string NetworkImg = context.Request["NetworkImg"];
            string NetworkType = context.Request["NetworkType"];
            string NetworkDay = context.Request["NetworkDay"];
            string NetworkName = context.Request["NetworkName"];
            string NetworkInfo = context.Request["NetworkInfo"];
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ID FROM TipsInfo where  AdminHotelid=@AdminHotelid ");
            SqlParam[] param = new SqlParam[] {
                                        new SqlParam("@AdminHotelid", AdminHotelid)};
            DataTable dstrs = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dstrs != null && dstrs.Rows.Count > 0)
            {

                Hashtable hs = new Hashtable();
                hs["IsNetwork"] = IsNetwork;
                hs["NetworkImg"] = NetworkImg;
                hs["NetworkType"] = NetworkType;
                hs["NetworkDay"] = NetworkDay;
                hs["NetworkName"] = NetworkName;
                hs["NetworkInfo"] = NetworkInfo;
                int b = DataFactory.SqlDataBase().UpdateByHashtable("TipsInfo", "ID", dstrs.Rows[0]["ID"].ToString(), hs);
                if (b > 0)
                {
                    a = "1";
                }
            }
            else
            {
                Hashtable ht = new Hashtable();
                ht["IsNetwork"] = IsNetwork;
                ht["NetworkImg"] = NetworkImg;
                ht["NetworkType"] = NetworkType;
                ht["NetworkDay"] = NetworkDay;
                ht["NetworkName"] = NetworkName;
                ht["NetworkInfo"] = NetworkInfo;
                ht["AdminHotelid"] = AdminHotelid;
                ht["HotelId"] = HotelId;
                int b = DataFactory.SqlDataBase().InsertByHashtable("TipsInfo", ht);
                if (b > 0)
                {
                    a = "1";
                }
            }
            context.Response.Write(a);
        }

        public void getinfo(HttpContext context)
        {


            string sql = string.Format(@"
            SELECT  m.ID ,
                    m.num ,
                    h.is_open ,
                    h.is_price ,
                    h.is_VipPrice ,
                    h.is_ShowType ,
                    ( SELECT TOP 1
                                ISRULENAME
                      FROM      dbo.Wx_function
                      WHERE     AdminHotelid = @AdminHotelid
                    ) ISRULENAME
            FROM    moday m ,
                    hotel h
            WHERE   m.AdminHotelid = h.AdminHotelid
                    AND m.AdminHotelid = @AdminHotelid
            ");

            SqlParam[] parmAdd = new SqlParam[] { 
               new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);

            string dtName = "Moday";
            string json = JsonHelper.DataTableToJson(dt, dtName);
            context.Response.Write(json);
        }

        public void update(HttpContext context)
        {
            string num = context.Request["num"].Trim();
            string iszs = context.Request["iszs"].Trim();
            string showtypejg = context.Request["showtypejg"].Trim();
            string is_vip = context.Request["is_vip"].Trim();
            string is_ShowType = context.Request["is_ShowType"].Trim();
            string is_CheckOutRemind = context.Request["is_CheckOutRemind"].Trim();
            string is_CheckOutTime = context.Request["is_CheckOutTime"].Trim();
            string isRuleName = context.Request["isRuleName"].Trim();
            string BusinessHour = context.Request["BusinessHour"].Trim();
            string BusinessMinute = context.Request["BusinessMinute"].Trim();
            string CashPledgeMoney = context.Request["CashPledgeMoney"].Trim();
            string adminHotelId = RequestSession.GetSessionUser().AdminHotelid.ToString();
            Hashtable hs = new Hashtable();
            hs["num"] = num;
            //hs["iszs"] = iszs;
            //hs["showtypejg"] = showtypejg;
            bool i = DataFactory.SqlDataBase().Submit_AddOrEdit("moday", "AdminHotelid", adminHotelId, hs);

            Hashtable hss = new Hashtable();
            hss["is_open"] = iszs;
            hss["is_price"] = showtypejg;
            hss["is_VipPrice"] = is_vip;
            hss["is_ShowType"] = is_ShowType;
            i = DataFactory.SqlDataBase().Submit_AddOrEdit("hotel", "AdminHotelid", adminHotelId, hss);

            Hashtable ht = new Hashtable();
            ht["CheckOutRemind"] = is_CheckOutRemind;
            ht["CheckOutTime"] = is_CheckOutTime;
            ht["isRuleName"] = isRuleName;
            i = DataFactory.SqlDataBase().Submit_AddOrEdit("Wx_function", "AdminHotelid", adminHotelId, ht);


            Hashtable htParameter = new Hashtable();
            htParameter["BusinessHour"] = BusinessHour;
            htParameter["BusinessMinute"] = BusinessMinute;
            htParameter["CashPledgeMoney"] = CashPledgeMoney;
            i = DataFactory.SqlDataBase().Submit_AddOrEdit("Hotel_AdminParameter", "AdminHotelId", adminHotelId, htParameter);

            ApplicationHelper.ClearFunction(adminHotelId);//清理 指定酒店功能参数
            if (i)
            {
                context.Response.Write("ok");
            }
            else
            {
                context.Response.Write("error");
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