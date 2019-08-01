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
using RM.Common.DotNetCode;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using LitJson;

namespace RM.Web.RMBase.SysUser
{
    /// <summary>
    /// UserInfo 的摘要说明
    /// </summary>
    public class UserInfo : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            string Action = context.Request["action"];                      //提交动作
            string user_ID = context.Request["user_ID"];                    //用户主键
            Hashtable ht = new Hashtable();
            int Return = -1;
            switch (Action)
            {
                case "accredit":                                            //用户信息启用
                    ht["DeleteMark"] = 1;
                    Return = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", user_ID, ht);
                    context.Response.Write(Return.ToString());
                    break;
                case "lock":                                                //锁定用户信息
                    ht["DeleteMark"] = 2;
                    Return = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", user_ID, ht);
                    context.Response.Write(Return.ToString());
                    break;

                case "Dimission": //离职
                    ht["DeleteMark"] = 0;
                    ht["LeaveDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                    Return = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", user_ID, ht);
                    string name = context.Request["user_name"];
                    CommonMethod.Base_Log("离职", "Base_UserInfo", user_ID, "办理离职", "员工[" + name + "]离职");//操作日志
                    context.Response.Write(Return > 0);
                    break;
                case "UntieById": //解绑
                    ht["openid"] = "";
                    ht["WX_Nickname"] = "";
                    Return = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", user_ID, ht);
                    string jb_name = context.Request["user_name"];
                    CommonMethod.Base_Log("解绑", "Base_UserInfo", user_ID, "解绑微信", "员工[" + jb_name + "]微信解绑");
                    context.Response.Write(Return > 0);
                    break;
                case "Entry":                                                //重新入职
                    ht["DeleteMark"] = 1;
                    string data_val = context.Request["data_val"]; //删除以前下线关系
                    string gx_msg = "关联回以前下线关系";
                    if (data_val == "0")
                    {
                        gx_msg = "永久删除以前下线关系";
                        Hashtable ht_del = new Hashtable();
                        ht_del["UserId"] = user_ID;
                        DataFactory.SqlDataBase().ExecuteByProc("P_UserEntryDelSource", ht_del);
                    }
                    Return = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", user_ID, ht);
                    string rz_name = context.Request["user_name"];
                    CommonMethod.Base_Log("入职", "Base_UserInfo", user_ID, "重新入职", "员工[" + rz_name + "]重新入职," + gx_msg);
                    context.Response.Write(Return > 0);
                    break;
                case "GetInfoList":
                    GetInfoList(context);
                    break;
                case "GetUserRoleList":
                    GetUserRoleList(context);
                    break;
                case "GetRoleList":
                    GetRoleList(context);
                    break;
                case "GetRoleGroup":
                    GetRoleGroup(context);
                    break;
                case "DefaultRole"://默认角色
                    DefaultRole(context);
                    break;
                case "GetOrgSelect"://获取部门信息
                    GetOrgSelect(context);
                    break;
                case "GetParentIds"://获取上级
                    GetParentIds(context);
                    break;
                case "CheckOrg"://验证是否有部门
                    CheckOrg(context);
                    break;
                default:
                    break;
            }
        }

        #region ** 部门信息 **

        //获取部门信息
        private void GetOrgSelect(HttpContext context)
        {
            string AdminHotelId = context.Request["AdminHotelId"];//集团id
            string HotelId = context.Request["HotelId"];//酒店id
            string orgid = context.Request["orgid"];//
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            DataTable dtOrg = user_idao.GetOrganizeList(AdminHotelId, HotelId, orgid);
            string json = "";
            if (dtOrg != null && dtOrg.Rows.Count > 0)
            {
                JsonData jsondata = new JsonData();
                jsondata["list"] = new JsonData();
                for (int i = 0; i < dtOrg.Rows.Count; i++)
                {
                    JsonData jd = new JsonData();
                    jd["Organization_ID"] = dtOrg.Rows[i]["Organization_ID"].ToString();
                    jd["Organization_Name"] = dtOrg.Rows[i]["Organization_Name"].ToString();
                    jsondata["list"].Add(jd);
                }
                json = jsondata.ToJson();
            }
            context.Response.Write(json);
        }

        //获取上级信息
        private void GetParentIds(HttpContext context)
        {
            string orgid = context.Request["orgid"];//
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            DataTable dtOrg = user_idao.GetOrganizeInfo(orgid);
            string pids = "";
            if (dtOrg != null && dtOrg.Rows.Count > 0)
            {
                pids = dtOrg.Rows[0]["ParentIds"].ToString();
            }
            context.Response.Write(pids);
        }

        //验证是否有部门
        private void CheckOrg(HttpContext context)
        {
            string AdminHotelId = context.Request["AdminHotelId"];//集团id
            string HotelId = context.Request["HotelId"];//酒店id
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            DataTable dtOrg = user_idao.GetOrganizeList(AdminHotelId, HotelId, "0");
            string pids = "0";
            if (dtOrg != null && dtOrg.Rows.Count > 0)
            {
                pids = "1";
            }
            context.Response.Write(pids);
        }
        #endregion

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
                        if (nv[0] == "USER_NAME")
                        {
                            sb.Append(" and (USER_NAME like '%" + nv[1] + "%' or User_Account like  '%" + nv[1] + "%' or theme like  '%" + nv[1] + "%'");
                        }
                        else if (nv[0] == "Hdhoteladmin")
                        {
                            sb.Append(" and AdminHotelid ='" + nv[1] + "'");
                        }
                        else
                        {
                            sb.Append(" and " + nv[0] + " = '" + nv[1] + "' ");
                        }
                    }
                }
            }
            if (RequestSession.GetSessionUser().UserAccount.ToString().ToLower() != "sewa")
            {
                sb.Append("and User_Account!='sewa'");
            }

            DataTable dt = DataFactory.SqlDataBase().DbPager("V_Base_UserInfo", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);
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

        private void GetUserRoleList(HttpContext context)
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
                        if (nv[0] == "SearchText")
                        {
                            sb.Append(" and (User_Account like '%" + nv[1] + "%' or User_Name like '%" + nv[1] + "%' or WX_Nickname like  '%" + nv[1] + "%') ");
                        }
                        else if (nv[0] == "orgid")
                        {
                            sb.Append(" and orgids like  '%" + nv[1] + "%' ");
                        }
                        else
                        {
                            sb.Append(" and " + nv[0] + " = '" + nv[1] + "' ");
                        }
                    }
                }
            }
            if (RequestSession.GetSessionUser().UserAccount.ToString().ToLower() != "sewa")
            {
                sb.Append("and User_Account!='sewa'");
            }

            DataTable dt = DataFactory.SqlDataBase().DbPager("V_Base_UserInfoRoles", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);
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
                            cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss"));
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

        private void GetRoleList(HttpContext context)
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
                pqGrid_OrderField = "SortCode";
            }
            int OrderType = pqGrid_OrderType == "asc" ? 1 : 0;

            StringBuilder sb = new StringBuilder();//查询条件
            sb.Append(" DeleteMark = 1 ");
            if (Search != null && Search.Length > 0)
            {
                for (int i = 0; i < Search.Length; i++)
                {
                    string[] nv = Search[i].Split('@');
                    if (nv.Length == 2)
                    {
                        if (nv[0] == "SearchText")
                        {
                            sb.Append(" and (Roles_Name like '%" + nv[1] + "%') ");
                        }
                        else
                        {
                            sb.Append(" and " + nv[0] + " = '" + nv[1] + "' ");
                        }
                    }
                }
            }

            DataTable dt = DataFactory.SqlDataBase().DbPager("Base_Roles", pqGrid_Sort, sb.ToString(), pqGrid_OrderField, OrderType, PageSize, PageIndex, out totalRecords);
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
                            cs.Add(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss"));
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

        private void GetRoleGroup(HttpContext context)
        {
            if (RequestSession.GetSessionUser() == null)
            {
                context.Response.Write("");
                return;
            }
            string roleid = context.Request.QueryString["roleid"];

            StringBuilder sb = new StringBuilder("SELECT * FROM Base_RolesMenuGroup WHERE Roles_ID = @Roles_ID");
            SqlParam[] parm = new SqlParam[] { new SqlParam("@Roles_ID", roleid) };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm);

            ArrayList data = new ArrayList();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    data.Add(dt.Rows[i]["MenuGroup_ID"].ToString());
                }
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            context.Response.Write(json);
        }

        private void DefaultRole(HttpContext context)
        {
            string adminHotelid = context.Request["AdminHotelid"];
            if (string.IsNullOrEmpty(adminHotelid))
            {
                return;
            }
            string hotelId = context.Request["HotelId"];
            if (string.IsNullOrEmpty(hotelId) || hotelId == "-1")
            {
                return;
            }

            HttpContext rq = HttpContext.Current;
            object obj = rq.Session["DefaultRole" + hotelId];
            if (obj != null)
            {
                return;
            }
            StringBuilder sb = new StringBuilder("SELECT Roles_ID FROM Base_Roles WHERE Hotel_Id = @Hotel_Id");
            SqlParam[] parm = new SqlParam[] { new SqlParam("@Hotel_Id", hotelId) };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm);
            if (dt != null && dt.Rows.Count > 0)
            {
                rq.Session["DefaultRole" + hotelId] = dt.Rows.Count;
                return;
            }


            sb = new StringBuilder(@"
            INSERT  INTO dbo.Base_Roles
                    ( Roles_ID ,
                      AdminHotelid ,
                      Hotel_Id ,
                      ParentId ,
                      Roles_Name ,
                      Role_Restriction ,
                      Roles_Remark ,
                      AllowEdit ,
                      AllowDelete ,
                      SortCode ,
                      DeleteMark ,
                      CreateDate ,
                      CreateUserId ,
                      CreateUserName ,
                      ModifyDate ,
                      ModifyUserId ,
                      ModifyUserName
                    )
                    SELECT  LOWER(NEWID()) ,
                            @AdminHotelid ,
                            @Hotel_Id ,
                            ParentId ,
                            Roles_Name ,
                            Role_Restriction ,
                            Roles_Remark ,
                            AllowEdit ,
                            AllowDelete ,
                            SortCode ,
                            DeleteMark ,
                            GETDATE() ,
                            CreateUserId ,
                            CreateUserName ,
                            GETDATE() ,
                            '' ,
                            ''
                    FROM    dbo.Base_Roles
                    WHERE   Hotel_Id = 16
            ");
            parm = new SqlParam[] { 
                new SqlParam("@AdminHotelid", adminHotelid),
                new SqlParam("@Hotel_Id", hotelId) };
            int sl = DataFactory.SqlDataBase().ExecuteBySql(sb, parm);
            if (sl > 0)
            {
                sb = new StringBuilder("SELECT Roles_ID,(SELECT Roles_ID FROM Base_Roles WHERE Hotel_Id = 16 AND Roles_Name = br.Roles_Name) OldID FROM Base_Roles br WHERE Hotel_Id = @Hotel_Id");
                parm = new SqlParam[] { new SqlParam("@Hotel_Id", hotelId) };

                dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Roles_ID = dt.Rows[i]["Roles_ID"].ToString();
                        string OldID = dt.Rows[i]["OldID"].ToString();
                        sb = new StringBuilder(@"
                        INSERT  INTO dbo.Base_RolesMenuGroup
                                ( RoleRight_ID ,
                                  Roles_ID ,
                                  MenuGroup_ID ,
                                  CreateDate ,
                                  CreateUserId ,
                                  CreateUserName
                                )
                                SELECT  LOWER(NEWID()) ,
                                        @NewRoles_ID ,
                                        MenuGroup_ID ,
                                        GETDATE() ,
                                        CreateUserId ,
                                        CreateUserName
                                FROM    dbo.Base_RolesMenuGroup
                                WHERE   Roles_ID = @OldID
                                    ");
                        parm = new SqlParam[] { 
                            new SqlParam("@NewRoles_ID", Roles_ID),
                            new SqlParam("@OldID", OldID) };
                        sl = DataFactory.SqlDataBase().ExecuteBySql(sb, parm);
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