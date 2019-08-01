using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Busines.IDAO
{
    /// <summary>
    /// 用户设置
    /// </summary>
    public interface RM_UserInfo_IDAO
    {
        #region 用户管理
        /// <summary>
        /// 用户名登录
        /// </summary>
        /// <param name="name">账户</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        DataTable UserLogin(string name, string pwd);
        /// <summary>
        /// 手机号登录
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        DataTable PhoneLogin(string phone);

        DataTable UserIdLogin(string userid);
        /// <summary>
        /// 用户列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        DataTable GetUserInfoPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        /// <summary>
        /// 所有用户信息
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <returns></returns>
        DataTable GetUserInfoInfo(StringBuilder SqlWhere, IList<SqlParam> IList_param);
        /// <summary>
        /// 加载所属用户权限
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        DataTable InitUserRight(string User_ID);

        /// <summary>
        /// 加载所属用户权限
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        DataTable InitUserRightA(string User_ID);

        DataTable InitUserRightAgent(string User_ID);

        string InitUserRight(string User_ID, string Menu_Name);

        string InitUserRightA(string User_ID, string Menu_Name);

        string InitUserRightAgent(string User_ID, string Menu_Name);
        /// <summary>
        /// 加载所属用户组
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        DataTable InitUserInfoUserGroup(string User_ID);
        /// <summary>
        /// 加载所属角色
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        DataTable InitUserRole(string User_ID);
        /// <summary>
        /// 加载所属部门
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        DataTable InitStaffOrganize(string User_ID);
        #endregion

        #region 系统日志
        /// <summary>
        /// 商户登录日志
        /// </summary>
        /// <param name="loginType">登录类型 1 用户名 2 手机</param>
        /// <param name="loginWay">登录方式 1 微网后台 2 智订云商户 3 移动PMS  10后台切换 20商户切换 30 PMS切换</param>
        /// <param name="SYS_USER_ACCOUNT">登录账户</param>
        /// <param name="SYS_LOGINLOG_STATUS">登录状态 0 失败 1 成功 2账户被锁</param>
        /// <param name="OWNER_address">ip所在地</param>
        /// <param name="userid">userid</param>
        /// <param name="AdminHotelid">商户id</param>
        void SysLoginLog(int loginType, int loginWay, string SYS_USER_ACCOUNT, string SYS_LOGINLOG_STATUS, string OWNER_address, string userid, string AdminHotelid);
        /// <summary>
        /// 登录日志列表
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        DataTable GetSysLoginLogPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        /// <summary>
        /// 获取登陆状况
        /// </summary>
        /// <param name="count">本月登录总数</param>
        /// <returns></returns>
        DataTable GetLogin_Info(ref int count);



        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="SYS_Operation_Type">操作类型</param>
        /// <param name="OWNER_address">IP所在地址</param>
        /// <param name="OperationBusiness">操作业务</param>
        /// <param name="OperationExplain">操作说明</param>
        /// <param name="OperationRemarks">操作备注</param>
        void SysOperationLog(string SYS_Operation_Type, string OWNER_address, string OperationBusiness, string OperationExplain, string OperationRemarks);

        /// <summary>
        /// 操作日志列表
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        DataTable GetSysOperationLogPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);

        #endregion

        #region 部门管理
        /// <summary>
        /// 加载部门所有员工
        /// </summary>
        /// <returns></returns>
        DataTable Load_StaffOrganizeList();
        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        DataTable GetOrganizeList(string AdminHotelId, string HotelId);
        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <param name="oid">id</param>
        /// <returns></returns>
        DataTable GetOrganizeInfo(string oid);
        /// <summary>
        /// 部门列表
        /// </summary>
        /// <param name="pid">父级ID</param>
        /// <returns></returns>
        DataTable GetOrganizeList(string AdminHotelId, string HotelId, string pid);
        /// <summary>
        /// 用户 部门列表
        /// </summary>
        /// <returns></returns>
        DataTable GetUserOrganizeList();
        #endregion

        #region 用户组管理
        /// <summary>
        /// 用户组列表
        /// </summary>
        /// <returns></returns>
        DataTable InitUserGroupList();
        /// <summary>
        /// 节点位置下拉框绑定
        /// </summary>
        /// <returns></returns>
        DataTable InitUserGroupParentId();
        /// <summary>
        /// 加载用户组成员
        /// </summary>
        /// <param name="UserGroup_ID">用户组主键</param>
        /// <returns></returns>
        DataTable Load_UserInfoUserGroupList(string UserGroup_ID);
        /// <summary>
        /// 加载所属用户组权限
        /// </summary>
        /// <param name="UserGroup_ID">用户组主键</param>
        DataTable InitUserGroupRight(string UserGroup_ID);
        /// <summary>
        /// 新增用户组成员
        /// </summary>
        /// <param name="User_ID">员工主键</param>
        /// <param name="UserGroup_ID">用户组主键</param>
        /// <returns></returns>
        bool AddUserGroupMenber(string[] User_ID, string UserGroup_ID);
        /// <summary>
        /// 用户组配权限
        /// </summary>
        /// <param name="pkVal">选择权限主键</param>
        /// <param name="UserGroup_ID">用户组主键</param>
        /// <returns></returns>
        bool Add_UserGroupAllotAuthority(string[] pkVal, string UserGroup_ID);
        #endregion
    }
}
