using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Busines;
using System.Collections;
using RM.Common.DotNetBean;

namespace RM.Web.Ajax
{
    /// <summary>
    /// Common_Ajax 的摘要说明
    /// </summary>
    public class Common_Ajax : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 公共一般处理程序
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            string Action = context.Request["action"];              //提交动作
            string module = context.Request["module"];              //业务模块
            string tableName = context.Request["tableName"];        //数据库表
            string pkName = context.Request["pkName"];              //字段主键
            string pkVal = context.Request["pkVal"];                //字段值
            int Return = -1;
            RM_System_IDAO systemidao = new RM_System_Dal();
            switch (Action)
            {
                case "DeleteP":                  //删除多条记录 1,2,3,4,5,6
                    Return = DataFactory.SqlDataBase().BatchDeleteData(tableName.Trim(), pkName.Trim(), pkVal.Split(','));
                    if (Return > 0)
                    {
                        CommonMethod.Base_Log("删除", tableName.Trim(), pkName.Trim() + ":" + pkVal, module.Trim(), "删除id:" + pkVal);//操作记录
                    }
                    context.Response.Write(Return.ToString());
                    break;
                case "AdminDelete":                  //删除多条记录 1,2,3,4,5,6
                    Return = DataFactory.SqlDataBase().BatchDeleteData(tableName.Trim(), pkName.Trim(), pkVal.Split(','));
                    if (Return > 0)
                    {
                        //CommonMethod.Base_Log("删除", tableName.Trim(), pkName.Trim() + ":" + pkVal, module.Trim(), "删除id:" + pkVal);//操作记录
                    }
                    context.Response.Write(Return.ToString());
                    break;
                case "ISDelete":                  //逻辑删除数据

                    string[] s = pkVal.Split(',');

                    for (int i = 0; i < s.Length; i++)
                    {
                        Hashtable ht = new Hashtable();
                        ht["DeleteMark"] = 0;
                        Return = DataFactory.SqlDataBase().UpdateByHashtable(tableName.Trim(), pkName.Trim(), s[i], ht);
                    }
                    if (Return > 0)
                    {
                        CommonMethod.Base_Log("删除", tableName.Trim(), pkName.Trim() + ":" + pkVal, module.Trim(), "删除id:" + pkVal);//操作记录
                    }

                    context.Response.Write(Return.ToString());
                    break;

                case "GetMenus":
                    string ru = "0";
                    if (RequestSession.GetSessionUser() == null)
                    {
                        context.Response.Write(0);
                    }
                    else
                    {
                        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
                        ru = user_idao.InitUserRight(RequestSession.GetSessionUser().UserId.ToString(), pkName);//获取用户对应菜单的权限
                    }
                    context.Response.Write(ru);
                    break;
                default:
                    break;
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