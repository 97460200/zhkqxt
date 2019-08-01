﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;
using RM.Common.DotNetEncrypt;
using RM.Busines.IDAO;
using System.Collections;
using RM.Common.DotNetBean;

namespace RM.Busines.DAL
{
    /// <summary>
    /// 用户设置
    /// </summary>
    public class RM_UserInfo_Dal : RM_UserInfo_IDAO
    {
        #region 部门管理
        /// <summary>
        /// 加载部门所有员工
        /// </summary>
        /// <returns></returns>
        public DataTable Load_StaffOrganizeList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Organization_ID,Organization_Name,ParentId,'0' AS isUser FROM Base_Organization UNION ALL
                            SELECT U.User_ID AS Organization_ID ,U.User_Code+'|'+U.User_Name AS User_Name,S.Organization_ID,'1' 
                            AS isUser FROM Base_UserInfo U RIGHT JOIN Base_StaffOrganize S ON U.User_ID = S.User_ID");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }
        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetOrganizeList(string AdminHotelId, string HotelId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM Base_Organization WHERE DeleteMark = 1  AND AdminHotelId = @AdminHotelId AND HotelId = @HotelId ORDER BY SortCode ASC");
            SqlParam[] sp = new SqlParam[] { 
                new SqlParam("@AdminHotelId", AdminHotelId),
                new SqlParam("@HotelId", HotelId)
            };
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, sp);
        }


        /// <summary>
        /// 部门列表
        /// </summary>
        /// <param name="pid">父级ID</param>
        /// <returns></returns>
        public DataTable GetOrganizeList(string AdminHotelId, string HotelId, string pid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM Base_Organization WHERE DeleteMark = 1 AND AdminHotelId = @AdminHotelId AND HotelId = @HotelId and ParentId=@ParentId ORDER BY SortCode ASC");
            SqlParam[] sp = new SqlParam[] { 
                new SqlParam("@AdminHotelId", AdminHotelId),
                new SqlParam("@HotelId", HotelId),
                new SqlParam("@ParentId", pid)
            };
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, sp);
        }

        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <param name="oid">id</param>
        /// <returns></returns>
        public DataTable GetOrganizeInfo(string oid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM Base_Organization WHERE Organization_ID=@Organization_ID");
            SqlParam[] sp = new SqlParam[] { 
                new SqlParam("@Organization_ID", oid)
            };
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, sp);
        }

        /// <summary>
        /// 用户 部门列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserOrganizeList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Organization_ID,Organization_Name,ParentId FROM Base_Organization WHERE DeleteMark = 1 AND (Organization_ID IN (SELECT Organization_ID FROM Base_StaffOrganize WHERE User_ID='" + RequestSession.GetSessionUser().UserId + "')  OR ParentId<>'0') ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }


        #endregion

        #region 用户管理
        /// <summary>
        /// 用户列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        //public DataTable GetUserInfoPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(@"SELECT U.User_ID,U.User_Code,U.User_Name,U.User_Account,U.User_Sex,U.Title,U.DeleteMark,U.User_Remark,U.CreateDate from Base_UserInfo U LEFT JOIN Base_StaffOrganize S ON U.User_ID = S.User_ID where U.DeleteMark !=0");
        //    strSql.Append(SqlWhere);
        //    strSql.Append("GROUP BY U.User_ID,U.User_Code,U.User_Name,U.User_Account,U.User_Sex,U.Title,U.DeleteMark,U.User_Remark,U.CreateDate");
        //    return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "CreateDate", "Desc", pageIndex, pageSize, ref count);
        //}

        /// <summary>
        /// 用户列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetUserInfoPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from Base_UserInfo where DeleteMark !=0 ");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "CreateDate", "Desc", pageIndex, pageSize, ref count);
        }

        /// <summary>
        /// 后台登陆验证
        /// </summary>
        /// <param name="name">账户</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public DataTable UserLogin(string name, string pwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"
            SELECT TOP 1
                    *
            FROM    V_Login
            WHERE   User_Account = @User_Account
                    AND User_Pwd = @User_Pwd
            ORDER BY IsDefault ASC 
            ");
            SqlParam[] para = {
                new SqlParam("@User_Account",name),
                new SqlParam("@User_Pwd",Md5Helper.MD5(pwd, 32))
            };
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        public DataTable UserIdLogin(string userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"
            SELECT TOP 1
                    *
            FROM    V_Login
            WHERE   User_ID = @User_ID
            ORDER BY IsDefault ASC
            ");
            SqlParam[] para = {
                new SqlParam("@User_ID",userid)
            };
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable PhoneLogin(string phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"
            SELECT TOP 1
                    *
            FROM    V_Login
            WHERE   User_Phone = @User_Phone
            ORDER BY IsDefault ASC
            ");
            SqlParam[] para = {
                new SqlParam("@User_Phone",phone)
            };
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        /// <summary>
        /// 所有用户信息
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <returns></returns>
        public DataTable GetUserInfoInfo(StringBuilder SqlWhere, IList<SqlParam> IList_param)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from Base_UserInfo where DeleteMark !=0");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, IList_param.ToArray());
        }
        /// <summary>
        /// 加载所属用户权限
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        public DataTable InitUserRight(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Menu_Id FROM Base_UserRight WHERE User_ID = @User_ID");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",User_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        /// <summary>
        /// 加载所属用户权限
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        public DataTable InitUserRightA(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Menu_Id FROM ABase_UserRight WHERE User_ID = @User_ID");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",User_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        /// <summary>
        /// 加载所属用户权限
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        public DataTable InitUserRightAgent(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Menu_Id FROM AgentBase_UserRight WHERE User_ID = @User_ID");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",User_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        /// <summary>
        /// 加载所属用户权限
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        /// <param name="Menu_Name">菜单名称</param>
        /// <returns></returns>
        public string InitUserRight(string User_ID, string Menu_Name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  LTRIM(Menu_Name) + ','
                            FROM    Base_SysMenu
                            WHERE   Menu_Id IN ( SELECT Menu_Id
                                                 FROM   Base_UserRight
                                                 WHERE  User_ID = @User_ID )
                                    AND ParentId = ( SELECT Menu_Id
                                                     FROM   Base_SysMenu
                                                     WHERE  Menu_Name = @Menu_Name
                                                   )
                            FOR     XML PATH('') ");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",User_ID),
                                         new SqlParam("@Menu_Name",Menu_Name)};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
            string mn = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                mn = dt.Rows[0][0].ToString();
            }
            return mn;
        }

        /// <summary>
        /// 加载所属用户权限
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        /// <param name="Menu_Name">菜单名称</param>
        /// <returns></returns>
        public string InitUserRightA(string User_ID, string Menu_Name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  LTRIM(Menu_Name) + ','
                            FROM    ABase_SysMenu
                            WHERE   Menu_Id IN ( SELECT Menu_Id
                                                 FROM   ABase_UserRight
                                                 WHERE  User_ID = @User_ID )
                                    AND ParentId = ( SELECT Menu_Id
                                                     FROM   ABase_SysMenu
                                                     WHERE  Menu_Name = @Menu_Name
                                                   )
                            FOR     XML PATH('') ");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",User_ID),
                                         new SqlParam("@Menu_Name",Menu_Name)};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
            string mn = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                mn = dt.Rows[0][0].ToString();
            }
            return mn;
        }


        /// <summary>
        /// 加载所属用户权限
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        /// <param name="Menu_Name">菜单名称</param>
        /// <returns></returns>
        public string InitUserRightAgent(string User_ID, string Menu_Name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  LTRIM(Menu_Name) + ','
                            FROM    AgentBase_SysMenu
                            WHERE   Menu_Id IN ( SELECT Menu_Id
                                                 FROM   AgentBase_UserRight
                                                 WHERE  User_ID = @User_ID )
                                    AND ParentId = ( SELECT Menu_Id
                                                     FROM   AgentBase_SysMenu
                                                     WHERE  Menu_Name = @Menu_Name
                                                   )
                            FOR     XML PATH('') ");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",User_ID),
                                         new SqlParam("@Menu_Name",Menu_Name)};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
            string mn = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                mn = dt.Rows[0][0].ToString();
            }
            return mn;
        }
        /// <summary>
        /// 加载所属用户组
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        public DataTable InitUserInfoUserGroup(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT UserGroup_ID FROM Base_UserInfoUserGroup WHERE User_ID = @User_ID");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",User_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        /// <summary>
        /// 加载所属角色
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        public DataTable InitUserRole(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Roles_ID FROM Base_UserRole WHERE User_ID = @User_ID");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",User_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        /// <summary>
        /// 加载所属部门
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        public DataTable InitStaffOrganize(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Organization_ID FROM Base_StaffOrganize WHERE User_ID = @User_ID");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",User_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        #endregion

        #region 系统日志
        /// <summary>
        /// 商户登录日志
        /// </summary>
        /// <param name="loginType">登录类型 1 用户名 2 手机</param>
        /// <param name="loginWay">登录方式 1 微网后台 2 智订云商户 3 移动PMS</param>
        /// <param name="SYS_USER_ACCOUNT">登录账户</param>
        /// <param name="SYS_LOGINLOG_STATUS">登录状态 0 失败 1 成功</param>
        /// <param name="OWNER_address">ip所在地</param>
        /// <param name="userid">userid</param>
        /// <param name="AdminHotelid">商户id</param>
        public void SysLoginLog(int loginType, int loginWay, string SYS_USER_ACCOUNT, string SYS_LOGINLOG_STATUS, string OWNER_address, string userid, string AdminHotelid)
        {
            Hashtable ht = new Hashtable();
            ht["LoginType"] = loginType;
            ht["LoginWay"] = loginWay;
            ht["User_Account"] = SYS_USER_ACCOUNT;
            ht["SYS_LOGINLOG_IP"] = RequestHelper.GetIP();
            ht["OWNER_address"] = OWNER_address;
            ht["SYS_LOGINLOG_STATUS"] = SYS_LOGINLOG_STATUS;
            ht["Base_UserInfo_ID"] = userid;
            ht["AdminHotelid"] = AdminHotelid;
            DataFactory.SqlDataBase().InsertByHashtable("Base_SysLoginlog", ht);
        }
        /// <summary>
        /// 登录日志列表
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetSysLoginLogPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from Base_SysLoginlog where 1=1");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "SYS_LOGINLOG_TIME", "Desc", pageIndex, pageSize, ref count);
        }
        /// <summary>
        /// 获取登陆状况
        /// </summary>
        /// <param name="count">本月登录总数</param>
        /// <returns></returns>
        public DataTable GetLogin_Info(ref int count)
        {
            //d1是本月的第一天，d2本月的最后一天，
            DateTime now = DateTime.Now;
            DateTime d1 = new DateTime(now.Year, now.Month, 1);
            DateTime d2 = d1.AddMonths(1).AddDays(-1);
            string UserAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSqlCount = new StringBuilder();
            strSql.Append("Select top 2 SYS_LOGINLOG_IP,Sys_LoginLog_Time from Base_SysLoginlog where User_Account = @User_Account");
            strSql.Append(" and Sys_LoginLog_Time >= @BeginBuilTime");
            strSql.Append(" and Sys_LoginLog_Time <= @endBuilTime ORDER BY Sys_LoginLog_Time DESC ");

            strSqlCount.Append("Select count(1) from Base_SysLoginlog where User_Account = @User_Account");
            strSqlCount.Append(" and Sys_LoginLog_Time >= @BeginBuilTime");
            strSqlCount.Append(" and Sys_LoginLog_Time <= @endBuilTime");
            SqlParam[] para = {
                                         new SqlParam("@User_Account",UserAccount),
                                         new SqlParam("@BeginBuilTime",d1),
                                         new SqlParam("@endBuilTime",d2)};
            count = Convert.ToInt32(DataFactory.SqlDataBase().GetObjectValue(strSqlCount, para));
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }



        #region  操作日志
        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="SYS_Operation_Type">操作类型:1增2删3改4查</param>
        /// <param name="OWNER_address">IP所在地址</param>
        /// <param name="OperationBusiness">操作业务</param>
        /// <param name="OperationExplain">操作说明</param>
        /// <param name="OperationRemarks">操作备注</param>
        public void SysOperationLog(string SYS_Operation_Type, string OWNER_address, string OperationBusiness, string OperationExplain, string OperationRemarks)
        {
            Hashtable ht = new Hashtable();
            ht["SYS_Operation_ID"] = CommonHelper.GetGuid;
            ht["SYS_Operation_IP"] = RequestHelper.GetIP();
            ht["SYS_Operation_TIME"] = DateTime.Now;
            ht["User_Account"] = RequestSession.GetSessionUser().UserId;
            ht["SYS_Operation_Type"] = SYS_Operation_Type;
            ht["OWNER_address"] = OWNER_address;
            ht["OperationBusiness"] = OperationBusiness;
            ht["OperationExplain"] = OperationExplain;
            ht["OperationRemarks"] = OperationRemarks;
            DataFactory.SqlDataBase().InsertByHashtable("Base_OperationLog", ht);
        }

        /// <summary>
        /// 操作日志列表
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public DataTable GetSysOperationLogPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from Base_OperationLog where 1=1");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "SYS_Operation_TIME", "Desc", pageIndex, pageSize, ref count);
        }

        #endregion

        #endregion

        #region 用户组管理
        /// <summary>
        /// 用户组列表
        /// </summary>
        /// <returns></returns>
        public DataTable InitUserGroupList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * from Base_UserGroup WHERE DeleteMark = 1 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }
        /// <summary>
        /// 节点位置下拉框绑定
        /// </summary>
        /// <returns></returns>
        public DataTable InitUserGroupParentId()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT UserGroup_ID,
                            UserGroup_Name+' - '+CASE ParentId WHEN '0' THEN '父节' ELSE  '子节' END AS UserGroup_Name
                            FROM Base_UserGroup WHERE DeleteMark = 1 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }
        /// <summary>
        /// 加载用户组成员
        /// </summary>
        /// <param name="UserGroup_ID">用户组主键</param>
        /// <returns></returns>
        public DataTable Load_UserInfoUserGroupList(string UserGroup_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT UserInfoUserGroup_ID,U.User_Name+'|'+U.User_Code AS User_Name,U.User_Account,U.User_Sex,U.Title,U.DeleteMark,U.User_Remark
                            FROM Base_UserInfo U RIGHT JOIN Base_UserInfoUserGroup G ON G.User_ID = U.User_ID 
                            WHERE G.UserGroup_ID = @UserGroup_ID");
            SqlParam[] para = {
                                         new SqlParam("@UserGroup_ID",UserGroup_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        /// <summary>
        /// 加载所属用户组权限
        /// </summary>
        /// <param name="UserGroup_ID">用户组主键</param>
        public DataTable InitUserGroupRight(string UserGroup_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Menu_Id FROM Base_UserGroupRight WHERE UserGroup_ID = @UserGroup_ID");
            SqlParam[] para = {
                                         new SqlParam("@UserGroup_ID",UserGroup_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        /// <summary>
        /// 新增用户组成员
        /// </summary>
        /// <param name="User_ID">员工主键</param>
        /// <param name="UserGroup_ID">用户组主键</param>
        /// <returns></returns>
        public bool AddUserGroupMenber(string[] User_ID, string UserGroup_ID)
        {
            try
            {
                StringBuilder[] sqls = new StringBuilder[User_ID.Length];
                object[] objs = new object[User_ID.Length];
                int index = 0;
                foreach (string item in User_ID)
                {
                    if (item.Length > 0)
                    {
                        StringBuilder sbadd = new StringBuilder();
                        sbadd.Append("Insert into Base_UserInfoUserGroup(");
                        sbadd.Append("UserInfoUserGroup_ID,User_ID,UserGroup_ID,CreateUserId,CreateUserName");
                        sbadd.Append(")Values(");
                        sbadd.Append("@UserInfoUserGroup_ID,@User_ID,@UserGroup_ID,@CreateUserId,@CreateUserName)");
                        SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@UserInfoUserGroup_ID", CommonHelper.GetGuid),
                                     new SqlParam("@User_ID", item),
                                     new SqlParam("@UserGroup_ID", UserGroup_ID),
                                     new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
                                     new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)};
                        sqls[index] = sbadd;
                        objs[index] = parmAdd;
                        index++;
                    }
                }
                return DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0 ? true : false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 用户组配权限
        /// </summary>
        /// <param name="pkVal">选择权限主键</param>
        /// <param name="UserGroup_ID">用户组主键</param>
        /// <returns></returns>
        public bool Add_UserGroupAllotAuthority(string[] pkVal, string UserGroup_ID)
        {
            try
            {
                StringBuilder[] sqls = new StringBuilder[pkVal.Length + 1];
                object[] objs = new object[pkVal.Length + 1];
                int index = 0;
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("Delete From Base_UserGroupRight Where UserGroup_ID =@UserGroup_ID");
                SqlParam[] parm = new SqlParam[] { new SqlParam("@UserGroup_ID", UserGroup_ID) };
                sqls[0] = sbDelete;
                objs[0] = parm;
                index = 1;
                foreach (string item in pkVal)
                {
                    if (item.Length > 0)
                    {
                        StringBuilder sbadd = new StringBuilder();
                        sbadd.Append("Insert into Base_UserGroupRight(");
                        sbadd.Append("UserGroupRight_ID,UserGroup_ID,Menu_Id,CreateUserId,CreateUserName");
                        sbadd.Append(")Values(");
                        sbadd.Append("@UserGroupRight_ID,@UserGroup_ID,@Menu_Id,@CreateUserId,@CreateUserName)");
                        SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@UserGroupRight_ID", CommonHelper.GetGuid),
                                     new SqlParam("@UserGroup_ID", UserGroup_ID),
                                     new SqlParam("@Menu_Id", item),
                                     new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
                                     new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)};
                        sqls[index] = sbadd;
                        objs[index] = parmAdd;
                        index++;
                    }
                }
                return DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0 ? true : false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
