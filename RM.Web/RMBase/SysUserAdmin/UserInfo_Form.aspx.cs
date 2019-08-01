using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetUI;
using System.Text;
using System.Data;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;
using RM.Common.DotNetData;
using RM.Web.App_Code;
using RM.Common.DotNetEncrypt;

namespace RM.Web.RMBase.SysUserAdmin
{
    public partial class UserInfo_Form : APageBase
    {
        public StringBuilder strUserRightHtml = new StringBuilder();//用户权限
        RM_System_IDAO systemidao = new RM_System_Dal();
        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        string _key;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            CreateUserName.Value = Request.Cookies["dladmin_COOKIE"]["User_Name"].ToString();
            CreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InitUserRight();
            if (!IsPostBack)
            {

                if (!string.IsNullOrEmpty(_key))
                {
                    InitData();
                }

               
                DataTable dt = new DataTable();
                string sqls = string.Format(@"select id,name from AdminHotelid where 1=1  order by AddTime desc");
                dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls));
                DropDownList1.DataTextField = "name";
                DropDownList1.DataValueField = "ID";
                DropDownList1.DataSource = dt;
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, "所有酒店");
            }
        }




        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("ABase_UserInfo", "User_ID", _key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
                User_Pwd.Value = "*************";              
                if (ht["hotelid".ToUpper()] != null && ht["hotelid".ToUpper()].ToString() != "" && ht["hotelid".ToUpper()].ToString() != "0")
                {
                    DropDownList1.SelectedValue = ht["hotelid".ToUpper()].ToString();
                }
            }
        }

        #region 用户权限
        /// <summary>
        /// 菜单树列表
        /// </summary>
        public void InitUserRight()
        {
            DataTable dtList = systemidao.GetMenuBindA();   
            DataTable dtUserRight = user_idao.InitUserRightA(_key);
            if (DataTableHelper.IsExistRows(dtList))
            {
                DataTable dtButoon = DataTableHelper.GetNewDataTable(dtList, "Menu_Type = '3'");
                DataTable dtMenu = DataTableHelper.GetNewDataTable(dtList, "Menu_Type < '3'");
                DataView dv = new DataView(dtMenu);
                dv.RowFilter = " ParentId = '0'";
                int eRowIndex = 0;
                foreach (DataRowView drv in dv)
                {
                    string trID = "node-" + eRowIndex.ToString();
                    strUserRightHtml.Append("<tr id='" + trID + "'>");
                    strUserRightHtml.Append("<td style='width: 200px;padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"] + "</span></td>");
                    strUserRightHtml.Append("<td style=\"width: 23px; text-align: left;\"><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked("Menu_Id", drv["Menu_Id"].ToString(), dtUserRight) + "  value=\"" + drv["Menu_Id"] + "|用户权限" + "\" name=\"checkbox\" /></td>");
                    strUserRightHtml.Append("<td>" + GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtUserRight) + "</td>");
                    strUserRightHtml.Append("</tr>");
                    //创建子节点
                    strUserRightHtml.Append(GetTreeNodeUserRight(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtUserRight));
                    eRowIndex++;
                }
            }
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTreeNodeUserRight(string parentID, DataTable dtMenu, string parentTRID, DataTable dtButoon, DataTable dtUserRight)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            DataView dv = new DataView(dtMenu);
            dv.RowFilter = "ParentId = '" + parentID + "'";
            int i = 1;
            foreach (DataRowView drv in dv)
            {
                string trID = parentTRID + "-" + i.ToString();
                sb_TreeNode.Append("<tr id='" + trID + "' class='child-of-" + parentTRID + "'>");
                sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"].ToString() + "</span></td>");
                sb_TreeNode.Append("<td style=\"width: 23px; text-align: left;\"><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" " + GetChecked("Menu_Id", drv["Menu_Id"].ToString(), dtUserRight) + " style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"" + drv["Menu_Id"] + "|用户权限" + "\" name=\"checkbox\" /></td>");
                sb_TreeNode.Append("<td>" + GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtUserRight) + "</td>");
                sb_TreeNode.Append("</tr>");
                //创建子节点
                sb_TreeNode.Append(GetTreeNodeUserRight(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtUserRight));
                i++;
            }
            return sb_TreeNode.ToString();
        }
        /// <summary>
        /// 获取导航菜单所属按钮
        /// </summary>
        /// <param name="Menu_Type">类型</param>
        /// <returns></returns>
        public string GetButton(string Menu_Id, DataTable dt, string parentTRID, DataTable dtUserRight)
        {
            StringBuilder ButtonHtml = new StringBuilder(); ;
            DataTable dt_Button = DataTableHelper.GetNewDataTable(dt, "ParentId = '" + Menu_Id + "'");
            if (DataTableHelper.IsExistRows(dt_Button))
            {
                int i = 1;
                foreach (DataRow drv in dt_Button.Rows)
                {
                    string trID = parentTRID + "--" + i.ToString();
                    ButtonHtml.Append("<lable><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" " + GetChecked("Menu_Id", drv["Menu_Id"].ToString(), dtUserRight) + " style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"" + drv["Menu_Id"] + "|用户权限" + "\" name=\"checkbox\" />");
                    ButtonHtml.Append("" + drv["Menu_Name"].ToString() + "</lable>&nbsp;&nbsp;&nbsp;&nbsp;");
                    i++;
                }
                return ButtonHtml.ToString();
            }
            return ButtonHtml.ToString();
        }
        #endregion

        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="pkName">对象主键</param>
        /// <param name="Obj_id">对象主键值</param>
        /// <param name="dt">数据源</param>
        /// <returns></returns>
        public string GetChecked(string pkName, string Obj_Val, DataTable dt)
        {
            StringBuilder strSql = new StringBuilder();
            dt = DataTableHelper.GetNewDataTable(dt, "" + pkName + " = '" + Obj_Val + "'");
            if (DataTableHelper.IsExistRows(dt))
                return "checked=\"checked\"";
            else
                return "";
        }

        #region 保存事件
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            string guid = CommonHelper.GetGuid;
            Hashtable ht = new Hashtable();
            ht["User_Name"] = User_Name.Value;
            ht["User_Account"] = User_Account.Value;
            if (User_Pwd.Value != "*************")
            {
                ht["User_Pwd"] = Md5Helper.MD5(User_Pwd.Value, 32);
            }
            ht["User_Sex"] = User_Sex.Value;
            ht["Email"] = Email.Value;
            ht["Title"] = Title.Value;
            ht["Theme"] = Theme.Value;
            ht["User_Remark"] = User_Remark.Value;
            if (DropDownList1.SelectedValue != "所有酒店" && DropDownList1.SelectedValue != "0")
            {
                ht["hotelid"] = DropDownList1.SelectedValue;
            }
            else
            {
                ht["hotelid"] = "";
            }
          
            if (!string.IsNullOrEmpty(_key))
            {
                guid = _key;
                ht["ModifyDate"] = DateTime.Now;
                ht["ModifyUserId"] = Request.Cookies["dladmin_COOKIE"]["User_ID"].ToString();
                ht["ModifyUserName"] = Request.Cookies["dladmin_COOKIE"]["User_Name"].ToString();
            }
            else
            {
                StringBuilder sbPR = new StringBuilder();
                sbPR.Append("SELECT User_Account FROM dbo.ABase_UserInfo WHERE User_Account= @User_Account and DeleteMark=1");
                SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@User_Account", User_Account.Value)};
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sbPR, parmAdd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ShowMsgHelper.Alert_Error("操作失败！用户名已存在");
                    return;
                }
                ht["User_ID"] = guid;
                ht["CreateUserId"] = Request.Cookies["dladmin_COOKIE"]["User_ID"].ToString();
                ht["CreateUserName"] = Request.Cookies["dladmin_COOKIE"]["User_Name"].ToString();
            }

            ht["CreateDate"] = DateTime.Now;
            ht["ModifyDate"] = DateTime.Now;
            ht["DeleteMark"] = 1;

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("ABase_UserInfo", "User_ID", _key, ht);
            if (IsOk)
            {
                IsOk = this.add_ItemForm(checkbox_value.Value.Split(','), guid);
            }
            if (IsOk)
            {
                ShowMsgHelper.ParmAlertMsgS("操作成功！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }
        }
        /// <summary>
        /// 批量新增用户权限
        /// </summary>
        /// <param name="item_value">对象主键</param>
        /// <param name="user_id">用户主键</param>
        /// <returns></returns>
        public bool add_ItemForm(string[] item_value, string user_id)
        {
            try
            {
                int index = 0;
                string key;
                string type;
                StringBuilder[] sqls = new StringBuilder[item_value.Length + 4];
                object[] objs = new object[item_value.Length + 4];

                StringBuilder sbDelete_Right = new StringBuilder();
                sbDelete_Right.Append("Delete From ABase_UserRight Where User_ID =@User_ID");
                SqlParam[] parm_Right = new SqlParam[] { new SqlParam("@User_ID", user_id) };
                sqls[0] = sbDelete_Right;
                objs[0] = parm_Right;
                index = 1;

                foreach (var item in item_value)
                {
                    if (item.Length > 0)
                    {
                        string[] str_item = item.Split('|');
                        key = str_item[0];
                        type = str_item[1];
                        if (type == "用户权限")
                        {
                            StringBuilder sbadd = new StringBuilder();
                            sbadd.Append("Insert into ABase_UserRight(");
                            sbadd.Append("UserRight_ID,User_ID,Menu_Id,CreateUserId,CreateUserName");
                            sbadd.Append(")Values(");
                            sbadd.Append("@UserRight_ID,@User_ID,@Menu_Id,@CreateUserId,@CreateUserName)");
                            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@UserRight_ID", CommonHelper.GetGuid),
                                     new SqlParam("@User_ID",user_id),
                                     new SqlParam("@Menu_Id",  key),
                                     new SqlParam("@CreateUserId", Request.Cookies["dladmin_COOKIE"]["User_ID"].ToString()),
                                     new SqlParam("@CreateUserName", Request.Cookies["dladmin_COOKIE"]["User_Name"].ToString())};
                            sqls[index] = sbadd;
                            objs[index] = parmAdd;
                        }
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