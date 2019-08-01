using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using SQL;
using System.Collections;
using RM.Busines;
using System.Text;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;

namespace RM.Web.RMBase
{
    public static class Com
    {

        /// <summary>
        /// 获取下拉列表框的栏目类型
        /// </summary>
        /// <param name="boardId">父类的ID</param>
        /// <param name="count">递归调用次数</param>
        /// <param name="typeId">所属功能模块</param>
        /// <param name="ddlKindsId">下拉控件</param>
        /// <param name="maxcount">最多显示几级</param>
        /// <param name="version">版本</param>
        public static void SetChildren(int boardId, int count, int typeId, DropDownList ddlKindsId, int maxcount, string version, string adminhotelid)
        {

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat("SELECT * FROM Kind where PID='{0}' and TypeId='{1}' and Version='{2}' and adminhotelid='{3}' ORDER BY Sort DESC,ID ASC", boardId, typeId, version, adminhotelid);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            count++;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //判断是几级栏目
                if (boardId != 0 && count != 0)
                {
                    string front = string.Empty;
                    for (int j = 1; j < count; j++)
                    {
                        char nbsp = (char)0xA0;
                        string text = "|—";
                        front += text.PadLeft(4, nbsp);
                    }

                    ddlKindsId.Items.Add(new ListItem(front + dt.Rows[i]["Name"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
                else
                {
                    ddlKindsId.Items.Add(new ListItem(dt.Rows[i]["Name"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
                //再一次调用(递归调用)
                if (count != maxcount)
                    SetChildren(int.Parse(dt.Rows[i]["ID"].ToString()), count, typeId, ddlKindsId, maxcount, adminhotelid);
            }
        }
        /// <summary>
        /// 获取下拉列表框的栏目类型
        /// </summary>
        /// <param name="boardId">父类的ID</param>
        /// <param name="count">递归调用次数</param>
        /// <param name="typeId">所属功能模块</param>
        /// <param name="ddlKindsId">下拉控件</param>
        /// <param name="maxcount">最多显示几级</param>
        public static void SetChildren(int boardId, int count, int typeId, DropDownList ddlKindsId, int maxcount)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat("SELECT * FROM Kind where PID='{0}' and TypeId='{1}' ORDER BY Sort ASC,ID ASC", boardId, typeId);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            count++;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //判断是几级栏目
                if (boardId != 0 && count != 0)
                {
                    string front = string.Empty;
                    for (int j = 1; j < count; j++)
                    {
                        char nbsp = (char)0xA0;
                        string text = "|—";
                        front += text.PadLeft(4, nbsp);
                    }

                    ddlKindsId.Items.Add(new ListItem(front + dt.Rows[i]["Name"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
                else
                {
                    ddlKindsId.Items.Add(new ListItem(dt.Rows[i]["Name"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
                //再一次调用(递归调用)
                if (count != maxcount)
                    SetChildren(int.Parse(dt.Rows[i]["ID"].ToString()), count, typeId, ddlKindsId, maxcount);
            }
        }

        /// <summary>
        /// 获取下拉列表框的栏目类型
        /// </summary>
        /// <param name="boardId">父类的ID</param>
        /// <param name="count">递归调用次数</param>
        /// <param name="typeId">所属功能模块</param>
        /// <param name="ddlKindsId">下拉控件</param>
        /// <param name="maxcount">最多显示几级</param>
        public static void SetChildren(int boardId, int count, int typeId, DropDownList ddlKindsId, int maxcount, string adminhotelid)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat("SELECT * FROM Kind where PID='{0}' and TypeId='{1}'  and adminhotelid='{2}' ORDER BY Sort ASC,ID ASC", boardId, typeId, adminhotelid);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            count++;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //判断是几级栏目
                if (boardId != 0 && count != 0)
                {
                    string front = string.Empty;
                    for (int j = 1; j < count; j++)
                    {
                        char nbsp = (char)0xA0;
                        string text = "|—";
                        front += text.PadLeft(4, nbsp);
                    }

                    ddlKindsId.Items.Add(new ListItem(front + dt.Rows[i]["Name"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
                else
                {
                    ddlKindsId.Items.Add(new ListItem(dt.Rows[i]["Name"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
                //再一次调用(递归调用)
                if (count != maxcount)
                    SetChildren(int.Parse(dt.Rows[i]["ID"].ToString()), count, typeId, ddlKindsId, maxcount, adminhotelid);
            }
        }
        /// <summary>
        /// 获取下拉列表框的栏目类型
        /// </summary>
        /// <param name="boardId">父类的ID</param>
        /// <param name="count">递归调用次数</param>
        /// <param name="typeId">所属功能模块</param>
        /// <param name="ddlKindsId">下拉控件</param>
        /// <param name="maxcount">最多显示几级</param>
        public static void SetinfoKindsChildren(int boardId, int count, int typeId, DropDownList ddlKindsId, int maxcount, string adminhotelid)
        {

            StringBuilder sql = new StringBuilder();


            sql.AppendFormat("SELECT * FROM Kind where PID='{0}' and TypeId='{1}'  and adminhotelid='{2}' ORDER BY Sort DESC,ID ASC", boardId, typeId, adminhotelid);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            count++;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //判断是几级栏目
                if (boardId != 0 && count != 0)
                {
                    string front = string.Empty;
                    for (int j = 1; j < count; j++)
                    {
                        char nbsp = (char)0xA0;
                        string text = "|—";
                        front += text.PadLeft(4, nbsp);
                    }

                    ddlKindsId.Items.Add(new ListItem(front + dt.Rows[i]["Name"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
                else
                {
                    ddlKindsId.Items.Add(new ListItem(dt.Rows[i]["Name"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
                //再一次调用(递归调用)
                if (count != maxcount && dt.Rows[i]["ID"].ToString() != "52" && dt.Rows[i]["ID"].ToString() != "53")
                    SetChildren(int.Parse(dt.Rows[i]["ID"].ToString()), count, typeId, ddlKindsId, maxcount, adminhotelid);
            }
        }


        /// <summary>
        /// 获取列表中显示的栏目名称--递归调用
        /// </summary>
        /// <param name="KindsId">栏目ID</param>
        /// <returns>栏目名称</returns>
        public static string setKindsName(int KindId)
        {
            //KindInfo info = (new KindBll()).GetKindById(KindId);
            Hashtable info = DataFactory.SqlDataBase().GetHashtableById("Kind", "ID", KindId.ToString());
            string strKindsName = "";
            //将栏目ID转换为栏目名称
            if (info != null)
            {
                strKindsName = info["Name"].ToString();
                if (info["Pid"] == "0")
                {
                    strKindsName = info["Name"].ToString();
                }
                else
                {

                }
            }
            return strKindsName;
        }


        public static string title(string adminhotelid)
        {
            string title = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("select Title from BaseInfo where Version=1 and AdminHotelid=@AdminHotelid");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@AdminHotelid", adminhotelid)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                title = " - " + dt.Rows[0]["Title"].ToString();
            }
            return title;
        }
        /// <summary>
        /// 本地存储注册会员信息
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="openid"></param>
        /// <param name="adminHotelid"></param>
        public static void SetMemberInfo(string unionid, string phone, string openid, string openid_xcx, string adminHotelid, string hotelId, string NickName, string Sex, string HeadImgUrl, string SourceUrl)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT MemberId FROM MemberInfo WHERE AdminHotelId = @AdminHotelId AND Openid = @Openid");
                SqlParam[] param = new SqlParam[]{
                    new SqlParam("@AdminHotelId",adminHotelid),
                    new SqlParam("@Openid",openid)
                };
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);//会员
                if (dt != null && dt.Rows.Count > 0)
                {
                    string MemberId = dt.Rows[0]["MemberId"].ToString();
                    Hashtable ht = new Hashtable();
                    ht["Phone"] = phone;
                    ht["HotelId"] = hotelId;
                    ht["unionid"] = unionid;
                    ht["Openid_xcx"] = openid_xcx;
                    ht["NickName"] = NickName;
                    ht["Sex"] = Sex;
                    ht["HeadImgUrl"] = HeadImgUrl;
                    DataFactory.SqlDataBase().UpdateByHashtable("MemberInfo", "MemberId", MemberId, ht);
                }
                else
                {
                    Hashtable ht = new Hashtable();
                    ht["Phone"] = phone;
                    ht["Openid"] = openid;
                    ht["AdminHotelId"] = adminHotelid;
                    ht["HotelId"] = hotelId;
                    ht["unionid"] = unionid;
                    ht["Openid_xcx"] = openid_xcx;
                    ht["NickName"] = NickName;
                    ht["Sex"] = Sex;
                    ht["HeadImgUrl"] = HeadImgUrl;
                    ht["SourceUrl"] = SourceUrl;
                    DataFactory.SqlDataBase().InsertByHashtable("MemberInfo", ht);
                }
            }
            catch
            {
            }
        }

        public static void UpdateMemberInfo(string MemberId, string openid)
        {
            try
            {
                Hashtable ht = new Hashtable();
                ht["Openid"] = openid;
                DataFactory.SqlDataBase().UpdateByHashtable("MemberInfo", "MemberId", MemberId, ht);
            }
            catch
            {
            }
        }

        public static void UpdateMemberInfo(string AdminHotelid, string unionid, string openid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE MemberInfo SET unionid = @unionid WHERE AdminHotelId = @AdminHotelId AND Openid = @Openid AND unionid = '0'");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@AdminHotelId", AdminHotelid),
                new SqlParam("@unionid", unionid),
                new SqlParam("@Openid", openid)
            };
            DataFactory.SqlDataBase().ExecuteBySql(sb, param);
        }


        /// <summary>
        /// 验证是否有超级管理查看数据统计权限
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static bool MarketingDate(string openid)
        {
            bool bl = false;
            try
            {
                if (HttpContext.Current.Request.Cookies[openid] != null)
                {
                    bl = Convert.ToBoolean(HttpContext.Current.Request.Cookies[openid]["MarketingDate"]);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"
                        SELECT  b.openid
                        FROM    Set_WeChat a
                                LEFT JOIN dbo.Base_UserInfo b ON b.User_Account = a.Phone
                        WHERE   b.openid = @openid
                        GROUP BY b.openid
                    ");
                    SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@openid", openid)};
                    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        bl = true;
                    }
                    HttpCookie cookie = new HttpCookie(openid);
                    cookie.Expires = DateTime.Now.AddDays(1);
                    cookie.Values.Add("MarketingDate", bl.ToString());
                    HttpContext.Current.Response.AppendCookie(cookie);
                }
            }
            catch
            {
            }
            return bl;
        }

        /// <summary>
        /// 验证是否是代理
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static bool MarketingDate_Agent(string openid, out string AgentId, out bool IsWithdraw)
        {
            bool bl = true;
            AgentId = "";
            IsWithdraw = false;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
                    SELECT  User_ID ,
                            Admin_agent ,
                            ISNULL(zdy_openid, '') zdy_openid ,
                            IsSithdraw
                    FROM    Agent_UserInfo
                    WHERE   openid = @openid
                ");
                SqlParam[] param = new SqlParam[] { 
                    new SqlParam("@openid", openid)
                };
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    bl = true;
                    AgentId = dt.Rows[0]["User_ID"].ToString();
                    if (dt.Rows[0]["Admin_agent"].ToString() != "0")
                    {
                        AgentId = dt.Rows[0]["Admin_agent"].ToString();
                    }
                    if (dt.Rows[0]["IsSithdraw"].ToString() == "1")
                    {
                        IsWithdraw = true;
                    }
                }
            }
            catch
            {

            }
            return bl;
        }
    }
}