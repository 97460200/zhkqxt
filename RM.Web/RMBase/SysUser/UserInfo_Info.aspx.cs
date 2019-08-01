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

namespace RM.Web.RMBase.SysUser
{
    public partial class UserInfo_Info : PageBase
    {
        DataTable menugroup_dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Request.QueryString["HotelId"]))
                {
                    ShowMsgHelper.OpenClose("操作失败！请先选择酒店...");
                    return;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["useredit"]))//首页 用户自己编辑
                {
                    User_Account.Disabled = true;
                    Credentials_Type.Disabled = true;
                    ddlUser_Role.Enabled = true;
                    ddlUser_Role.Attributes.Add("disabled", "disabled");
                    User_Pwd.Disabled = true;
                }
                hdUserId.Value = Request.QueryString["UserId"];
                hRestore.Value = Request.QueryString["Restore"];
                AdminHotelid.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                HotelId.Value = Request.QueryString["HotelId"];
                InitData();
                if (!string.IsNullOrEmpty(hdUserId.Value))//编辑
                {
                    EditData();
                }
            }
        }

        /// <summary>
        /// 初始值绑定
        /// </summary>
        private void InitData()
        {
            #region ** 部门下拉框 **

            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            DataTable dtOrg = user_idao.GetOrganizeList(AdminHotelid.Value, HotelId.Value, "0");
            if (dtOrg != null && dtOrg.Rows.Count > 0)
            {
                dOrg.Visible = true;
                ddlOrg.DataSource = dtOrg;
                ddlOrg.DataTextField = "Organization_Name";
                ddlOrg.DataValueField = "Organization_ID";
                ddlOrg.DataBind();
                ddlOrg.Items.Insert(0, new ListItem("==请选择==", ""));
            }
            else
            {
                dOrg.Visible = false;
            }
            #endregion

            CommonMethod.BindCredentials(Credentials_Type);
            #region  ****** 角色下拉框 ******

            DataTable dt = new DataTable();
            string sql_role = "SELECT Roles_ID,Roles_Name FROM Base_Roles WHERE Hotel_Id = @Hotel_Id";
            SqlParam[] param = new SqlParam[] { new SqlParam("Hotel_Id", HotelId.Value) };

            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql_role), param);
            ddlUser_Role.DataTextField = "Roles_Name";
            ddlUser_Role.DataValueField = "Roles_ID";
            ddlUser_Role.DataSource = dt;
            ddlUser_Role.DataBind();


            #endregion
            return;
            //#region ****** 菜单 权限 ******
            //string sql = "SELECT * FROM Base_MenuGroup ORDER BY SortCode";
            //menugroup_dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            //DataTable mg_data = menugroup_dt.Clone();//拷贝框架
            //DataRow[] dr = menugroup_dt.Select("ParentId = '0'");
            //for (int i = 0; i < dr.Count(); i++)
            //{
            //    mg_data.ImportRow((DataRow)dr[i]);
            //}
            //rptMenuGroup.DataSource = mg_data;
            //rptMenuGroup.DataBind();
            //#endregion
        }
        /// <summary>
        /// 编辑绑定
        /// </summary>
        private void EditData()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_UserInfo", "User_ID", hdUserId.Value);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
                User_Pwd.Value = "*************";
                string rs = ht["RoomState".ToUpper()].ToString();
                if (rs == "1")
                {
                    lbRoomState.Attributes.Add("class", "checked");
                }
                else
                {
                    lbRoomState.Attributes.Add("class", "");
                }
                if (ht["Organization_ID".ToUpper()] != null && ht["Organization_ID".ToUpper()].ToString() != "")
                {
                    hdOrgId.Value = ht["Organization_ID".ToUpper()].ToString();
                }

                string hd = ht["HotelData".ToUpper()].ToString();
                if (hd == "1")
                {
                    lbHotelData.Attributes.Add("class", "checked");
                }
                else
                {
                    lbHotelData.Attributes.Add("class", "");
                }

                string ps = ht["PublicShow".ToUpper()].ToString();
                if (ps == "1")
                {
                    lbPublicShow.Attributes.Add("class", "checked");
                }
                else
                {
                    lbPublicShow.Attributes.Add("class", "");
                }
                
                string rm = ht["ReserveMoney".ToUpper()].ToString();
                if (rm == "1")
                {
                    lbReserveMoney.Attributes.Add("class", "checked");
                }
                else
                {
                    lbReserveMoney.Attributes.Add("class", "");
                }

                Hashtable htRole = DataFactory.SqlDataBase().GetHashtableById("Base_UserRole", "User_ID", hdUserId.Value);
                if (htRole.Count > 0 && htRole != null)
                {
                    ddlUser_Role.SelectedValue = htRole["Roles_ID".ToUpper()].ToString();
                }
            }
        }

        private DataTable dtMenuGroup = null;
        public string GetMenuGroup(string group_ID)
        {
            if (dtMenuGroup == null)
            {
                StringBuilder sb = new StringBuilder("SELECT * FROM Base_UserMenuGroup WHERE User_Id = @User_Id");
                SqlParam[] parm = new SqlParam[] { new SqlParam("@User_Id", hdUserId.Value) };
                dtMenuGroup = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm);
            }

            string mg_html = "<div class='checkbox'>";
            DataTable mg_data = menugroup_dt.Clone();//拷贝框架
            DataRow[] dr = menugroup_dt.Select("ParentId = '" + group_ID + "'");
            for (int i = 0; i < dr.Count(); i++)
            {
                mg_data.ImportRow((DataRow)dr[i]);
            }
            for (int i = 0; i < mg_data.Rows.Count; i++)
            {
                string ck = "";
                DataRow[] drMenuGroup = dtMenuGroup.Select("MenuGroup_Id = '" + mg_data.Rows[i]["Group_ID"].ToString() + "'");
                if (drMenuGroup.Count() > 0)
                {
                    ck = " class='checked' ";
                }
                mg_html += string.Format("<label dataid='{0}' {2}>{1}</label>",
                    mg_data.Rows[i]["Group_ID"].ToString(), mg_data.Rows[i]["Group_Name"].ToString(), ck);
            }
            mg_html += "</div>";
            return mg_html;
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
            Hashtable ht_Notice = new Hashtable();
            ht["IsAdmin"] = 2;
            ht["User_Name"] = User_Name.Value;
            ht["User_Account"] = User_Account.Value;//登录账号
            ht["User_Phone"] = User_Account.Value;//手机号码
            if (User_Pwd.Value != "*************")
            {
                if (User_Pwd.Value == "")
                {
                    User_Pwd.Value = "123456";
                }
                ht["User_Pwd"] = Md5Helper.MD5(User_Pwd.Value, 32);
            }
            ht["User_Sex"] = 1;
            ht["Email"] = "";// 电子邮箱
            ht["Title"] = "";
            ht["Theme"] = "";//
            ht["User_Remark"] = "";//备注
            ht["ygh"] = ygh.Value;//国光工号
            ht["hotelid"] = HotelId.Value;
            ht["HotelListId"] = HotelId.Value;
            ht["AdminHotelid"] = AdminHotelid.Value;
            ht["Credentials_Type"] = Credentials_Type.Value;
            ht["Credentials_Number"] = Credentials_Number.Value;
            ht["RoomState"] = hdRoomState.Value;
            ht["HotelData"] = hdHotelData.Value;
            ht["Organization_ID"] = hdOrgId.Value;
            ht["PublicShow"] = hdPublicShow.Value;
            ht["ReserveMoney"] = hdReserveMoney.Value;

            if (!string.IsNullOrEmpty(hdUserId.Value) && string.IsNullOrEmpty(hRestore.Value))
            {
                guid = hdUserId.Value;
                ht["ModifyDate"] = DateTime.Now;
                ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
                ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
                StringBuilder sbPR = new StringBuilder();
                sbPR.Append("SELECT User_Account FROM Base_UserInfo WHERE User_Account= @User_Account and User_ID <> @User_ID and DeleteMark = 1");
                SqlParam[] parmAdd = new SqlParam[] {
                    new SqlParam("@User_Account", User_Account.Value),
                    new SqlParam("@User_ID",hdUserId.Value)
                };
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sbPR, parmAdd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ShowMsgHelper.Alert_Error("操作失败！该员工手机号已存在");
                    return;
                }
            }
            else
            {
                StringBuilder sbPR = new StringBuilder();
                sbPR.Append("SELECT User_Account FROM Base_UserInfo WHERE User_Account= @User_Account and (DeleteMark = 1 OR hotelid = @hotelid) and IsAdmin = 2");
                SqlParam[] parmAdd = new SqlParam[] {
                                     new SqlParam("@User_Account", User_Account.Value),
                                     new SqlParam("@hotelid",HotelId.Value)};
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sbPR, parmAdd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ShowMsgHelper.Alert_Error("操作失败！该员工手机号已存在");
                    return;
                }
                ht["User_ID"] = guid;
                ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
                ht["CreateDate"] = DateTime.Now;
            }

            ht["ModifyDate"] = DateTime.Now;
            ht["DeleteMark"] = 1;

            bool IsOk;
            if (!string.IsNullOrEmpty(hRestore.Value))
            {
                ht["User_Pwd"] = Md5Helper.MD5("123456", 32);
                IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "User_ID", "", ht);
            }
            else
            {
                IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "User_ID", hdUserId.Value, ht);
            }
            if (IsOk)
            {
                #region  ** 角色 **
                if (ddlUser_Role.SelectedValue != "-1")
                {
                    DataFactory.SqlDataBase().DeleteData("Base_UserRole", "User_ID", guid);//删除角色
                    Hashtable htRole = new Hashtable();
                    htRole["UserRole_ID"] = CommonHelper.GetGuid;
                    htRole["User_ID"] = guid;
                    htRole["Roles_ID"] = ddlUser_Role.SelectedValue;
                    DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserRole", "User_ID", "", htRole);//重新添加角色
                }
                #endregion
                //  IsOk = this.add_ItemForm(checkbox_value.Value.Split(','), guid);
            }
            if (IsOk)
            {

                ht_Notice["HotelData"] = hdHotelData.Value;
                DataFactory.SqlDataBase().Submit_AddOrEdit("Notice", "UserId", hdUserId.Value, ht_Notice);
                ShowMsgHelper.AlertReloadClose("操作成功！", "ListGrid(false)");
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
                StringBuilder[] sqls = new StringBuilder[(item_value.Length + 2)];
                object[] objs = new object[(item_value.Length + 2)];

                StringBuilder groupDelete_Right = new StringBuilder();
                groupDelete_Right.Append("Delete From Base_UserMenuGroup Where User_ID = @User_ID");
                SqlParam[] group_Right = new SqlParam[] { new SqlParam("@User_ID", user_id) };
                sqls[0] = groupDelete_Right;
                objs[0] = group_Right;

                StringBuilder sbDelete_Right = new StringBuilder();
                sbDelete_Right.Append("Delete From Base_UserRight Where User_ID =@User_ID");
                SqlParam[] parm_Right = new SqlParam[] { new SqlParam("@User_ID", user_id) };
                sqls[1] = sbDelete_Right;
                objs[1] = parm_Right;



                index = 2;

                foreach (var item in item_value)
                {
                    if (item.Length > 0)
                    {
                        //菜单组
                        StringBuilder sb_group = new StringBuilder();
                        sb_group.Append("Insert into Base_UserMenuGroup(");
                        sb_group.Append("User_ID,MenuGroup_ID,CreateUserId,CreateUserName");
                        sb_group.Append(")Values(");
                        sb_group.Append("@User_ID,@MenuGroup_ID,@CreateUserId,@CreateUserName)");

                        SqlParam[] parm_group = new SqlParam[] { 
                                     new SqlParam("@User_ID",user_id),
                                     new SqlParam("@MenuGroup_ID",  item),
                                     new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
                                     new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)};
                        sqls[index] = sb_group;
                        objs[index] = parm_group;
                        index++;
                        ////用户权限
                        //StringBuilder sbadd = new StringBuilder();
                        //sbadd.Append("Insert into Base_UserRight(");
                        //sbadd.Append("UserRight_ID,User_ID,Menu_Id,CreateUserId,CreateUserName");
                        //sbadd.Append(") SELECT LOWER(NEWID()),'" + user_id + "',Menu_Id,'" + RequestSession.GetSessionUser().UserId + "','" + RequestSession.GetSessionUser().UserName + "' FROM Base_MenuGroupRight WHERE MenuGroup_ID = @MenuGroup_ID ");

                        //SqlParam[] parmAdd = new SqlParam[] { 
                        //             new SqlParam("@MenuGroup_ID",  item)};
                        //sqls[index] = sbadd;
                        //objs[index] = parmAdd;
                        //index++;
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