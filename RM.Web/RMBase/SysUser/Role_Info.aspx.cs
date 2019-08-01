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
    public partial class Role_Info : PageBase
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
                hdRoleId.Value = Request.QueryString["roleId"];

                HotelAdmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                HotelId.Value = Request.QueryString["HotelId"];
                InitData();
                if (!string.IsNullOrEmpty(hdRoleId.Value))//编辑
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
            #region ****** 菜单 权限 ******
            string sql = "SELECT * FROM Base_MenuGroup ORDER BY SortCode ASC";
            menugroup_dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT  CASE WHEN type = '0' THEN '单体酒店' ELSE '连锁酒店' END HotelType,
                    CASE WHEN Pattern = '1' THEN '预存' WHEN Pattern = '2' THEN '月结' WHEN Pattern = '3' THEN '租用' ELSE '销售' END Pattern
            FROM    dbo.Hotel_Admin
            WHERE   AdminHotelid = @AdminHotelid");
            SqlParam[] param = new SqlParam[] {
                new SqlParam("@AdminHotelid", HotelAdmin.Value)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            string pid = "0";
            if (dt != null && dt.Rows.Count > 0)
            {
                sb = new StringBuilder();
                sb.Append(@"
                SELECT  Group_ID
                FROM    dbo.Base_MenuGroup
                WHERE   ParentId IN ( SELECT    Group_ID
                                      FROM      dbo.Base_MenuGroup
                                      WHERE     Group_Name = @Pattern )
                        AND Group_Name = @HotelType");
                param = new SqlParam[] {
                    new SqlParam("@HotelType", dt.Rows[0]["HotelType"].ToString()),
                    new SqlParam("@Pattern", dt.Rows[0]["Pattern"].ToString())
                };
                dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    pid = dt.Rows[0]["Group_ID"].ToString();
                }
            }

            DataTable mg_data = menugroup_dt.Clone();//拷贝框架
            DataRow[] dr = menugroup_dt.Select("ParentId = '" + pid + "'");
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
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_Roles", "Roles_ID", hdRoleId.Value);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
            }
        }

        private DataTable dtMenuGroup = null;
        public string GetMenuGroup(string group_ID)
        {
            if (dtMenuGroup == null)
            {
                StringBuilder sb = new StringBuilder("SELECT * FROM Base_RolesMenuGroup WHERE Roles_ID = @Roles_ID");
                SqlParam[] parm = new SqlParam[] { new SqlParam("@Roles_ID", hdRoleId.Value) };
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
                DataRow[] drMenuGroup = dtMenuGroup.Select("MenuGroup_ID = '" + mg_data.Rows[i]["Group_ID"].ToString() + "'");
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
            if (Roles_Name.Value != "")
            {
                ht["Roles_Name"] = Roles_Name.Value;
            }
            ht["Roles_Remark"] = Roles_Remark.Value;

            ht["ParentId"] = "0";
            ht["AllowEdit"] = "1";
            ht["SortCode"] = SortCode.Value;
            ht["Hotel_Id"] = HotelId.Value;
            ht["AdminHotelid"] = HotelAdmin.Value;

            if (!string.IsNullOrEmpty(hdRoleId.Value))
            {
                guid = hdRoleId.Value;
                ht["ModifyDate"] = DateTime.Now;
                ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
                ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
            }
            else
            {
                ht["Roles_ID"] = guid;
                ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
                ht["CreateDate"] = DateTime.Now;
            }
            ht["ModifyDate"] = DateTime.Now;
            ht["DeleteMark"] = 1;

            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_Roles", "Roles_ID", hdRoleId.Value, ht);
            if (IsOk)
            {
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

        public bool add_ItemForm(string[] item_value, string role_id)
        {
            try
            {
                int index = 0;
                StringBuilder[] sqls = new StringBuilder[(item_value.Length + 2) * 2];
                object[] objs = new object[(item_value.Length + 2) * 2];

                StringBuilder groupDelete_Right = new StringBuilder();
                groupDelete_Right.Append("Delete From Base_RolesMenuGroup Where Roles_ID = @Roles_ID");
                SqlParam[] group_Right = new SqlParam[] { new SqlParam("@Roles_ID", role_id) };
                sqls[0] = groupDelete_Right;
                objs[0] = group_Right;

                index = 1;

                foreach (var item in item_value)
                {
                    if (item.Length > 0)
                    {
                        //菜单组
                        StringBuilder sb_group = new StringBuilder();
                        sb_group.Append("Insert into Base_RolesMenuGroup(");
                        sb_group.Append("RoleRight_ID,Roles_ID,MenuGroup_ID,CreateUserId,CreateUserName");
                        sb_group.Append(")Values(");
                        sb_group.Append("@RoleRight_ID,@Roles_ID,@MenuGroup_ID,@CreateUserId,@CreateUserName)");

                        SqlParam[] parm_group = new SqlParam[] { 
                                     new SqlParam("@RoleRight_ID",CommonHelper.GetGuid),
                                     new SqlParam("@Roles_ID",role_id),
                                     new SqlParam("@MenuGroup_ID",  item),
                                     new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
                                     new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)};
                        sqls[index] = sb_group;
                        objs[index] = parm_group;
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