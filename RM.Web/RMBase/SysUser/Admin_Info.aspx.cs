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
    public partial class Admin_Info : PageBase
    {
        DataTable menugroup_dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdUserId.Value = Request.QueryString["UserId"];

                HotelAdmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
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
            DataTable dtHotel = new DataTable();
            StringBuilder sbHotel = new StringBuilder();
            sbHotel.Append("select id,name from Hotel where AdminHotelid= @AdminHotelid order by sort asc");
            SqlParam[] paramHotel = new SqlParam[] { new SqlParam("AdminHotelid", HotelAdmin.Value) };
            dtHotel = DataFactory.SqlDataBase().GetDataTableBySQL(sbHotel, paramHotel);
            rptHotelList.DataSource = dtHotel;
            rptHotelList.DataBind();

            CommonMethod.BindCredentials(Credentials_Type);

            #region ****** 菜单 权限 ******
            string sql = "SELECT * FROM Base_MenuGroup";
            menugroup_dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            DataTable mg_data = menugroup_dt.Clone();//拷贝框架
            DataRow[] dr = menugroup_dt.Select("ParentId = '0'");
            for (int i = 0; i < dr.Count(); i++)
            {
                mg_data.ImportRow((DataRow)dr[i]);
            }
            rptMenuGroup.DataSource = mg_data;
            rptMenuGroup.DataBind();
            #endregion
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
                //if (drMenuGroup.Count() > 0)
                //{
                //    ck = " class='checked' ";
                //}
                ck = " class='checked' ";
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
            ht["IsAdmin"] = 1;
            ht["User_Name"] = User_Name.Value;
            ht["User_Account"] = User_Account.Value;
            if (User_Pwd.Value != "*************")
            {
                ht["User_Pwd"] = Md5Helper.MD5(User_Pwd.Value, 32);
            }
            ht["User_Sex"] = 1;
            ht["Email"] = "";//联系电话
            ht["Title"] = "";
            ht["Theme"] = "";//电子邮箱
            ht["User_Remark"] = "";//备注
            ht["hotelid"] = "0";
            ht["HotelListId"] = HotelListId.Value;
            ht["AdminHotelid"] = HotelAdmin.Value;
            ht["Credentials_Type"] = Credentials_Type.Value;
            ht["Credentials_Number"] = Credentials_Number.Value;

            if (!string.IsNullOrEmpty(hdUserId.Value))
            {
                guid = hdUserId.Value;
                ht["ModifyDate"] = DateTime.Now;
                ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
                ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
            }
            else
            {
                StringBuilder sbPR = new StringBuilder();
                sbPR.Append("SELECT User_Account FROM Base_UserInfo WHERE User_Account= @User_Account and AdminHotelid=@hotelid and DeleteMark=1");
                SqlParam[] parmAdd = new SqlParam[] {
                                     new SqlParam("@User_Account", User_Account.Value),
                                     new SqlParam("@hotelid", HotelId.Value)};
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sbPR, parmAdd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ShowMsgHelper.Alert_Error("操作失败！用户名已存在");
                    return;
                }
                ht["User_ID"] = guid;
                ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
                ht["CreateDate"] = DateTime.Now;
            }

            ht["ModifyDate"] = DateTime.Now;
            ht["DeleteMark"] = 1;

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "User_ID", hdUserId.Value, ht);
            if (IsOk)
            {
                //#region  ** 角色 **
                //DataFactory.SqlDataBase().DeleteData("Base_UserRole", "User_ID", guid);//删除角色
                //Hashtable htRole = new Hashtable();
                //htRole["UserRole_ID"] = CommonHelper.GetGuid;
                //htRole["User_ID"] = guid;
                //htRole["Roles_ID"] = "0";
                //DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserRole", "User_ID", "", htRole);//重新添加角色
                //#endregion
                IsOk = this.add_ItemForm(checkbox_value.Value.Split(','), guid);
            }
            if (IsOk)
            {
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



                //index = 1;
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
                        //sbadd.Append(") SELECT NEWID(),'" + user_id + "',Menu_Id,'" + RequestSession.GetSessionUser().UserId + "','" + RequestSession.GetSessionUser().UserName + "' FROM Base_MenuGroupRight WHERE MenuGroup_ID = @MenuGroup_ID ");

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