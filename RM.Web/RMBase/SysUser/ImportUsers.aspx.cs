using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;
using RM.Common.DotNetUI;
using RM.Busines;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using RM.Common.DotNetEncrypt;

namespace RM.Web.RMBase.SysUser
{
    public partial class ImportUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Request.QueryString["HotelId"]))
                {
                    ShowMsgHelper.OpenClose("操作失败！");
                    return;
                }
                hdHotelId.Value = Request.QueryString["HotelId"];
                hdAdminHotelId.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                hdUserId.Value = RequestSession.GetSessionUser().UserId.ToString();
            }
        }

        string jdyg_rid = "";//酒店员工
        string jdcw_rid = "";//酒店财务
        string jdjl_rid = "";//酒店经理
        string jdgly_rid = "";//酒店管理员
        private void getRoles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT  Roles_ID ,
                    Roles_Name
            FROM    Base_Roles
            WHERE   AdminHotelid = @AdminHotelId
                    AND Hotel_Id = @HotelId
                    AND DeleteMark = 1 AND Roles_Name='酒店员工'
            ");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@AdminHotelId",  hdAdminHotelId.Value),
                new SqlParam("@HotelId",  hdHotelId.Value)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                jdyg_rid = dt.Rows[0]["Roles_ID"].ToString();
            }
        }

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile == false)//HasFile用来检查FileUpload是否有指定文件
            {
                errorMsg.Visible = true;
                errorMsg.InnerHtml = "请您选择要导入的Excel文件！";
                return;
            }

            string IsXls = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名           
            if (IsXls != ".xls" && IsXls != ".xlsx")//
            {
                errorMsg.Visible = true;
                errorMsg.InnerHtml = "只可以选择Excel文件！";
                return;
            }

            DataTable dt = ExcelDataSource();
            if (dt == null || dt.Rows.Count < 1)
            {
                errorMsg.Visible = true;
                errorMsg.InnerHtml = "1.请检测Excel文件是否按照模板指定格式。2.确认工作表名称是否为Sheet1 ！";
                return;
            }

            if (!dt.Columns.Contains("姓名") || !dt.Columns.Contains("性别") || !dt.Columns.Contains("手机号码") || !dt.Columns.Contains("部门") || !dt.Columns.Contains("职位"))
            {
                errorMsg.Visible = true;
                errorMsg.InnerHtml = "请检测Excel文件是否按照模板指定格式！";
                return;
            }
            getRoles();//获取角色
            //导入
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string name = dt.Rows[i]["姓名"].ToString().Trim();
                string sex = dt.Rows[i]["性别"].ToString().Trim();
                string phone = dt.Rows[i]["手机号码"].ToString().Trim();
                if (name == "" || phone == "")
                {
                    continue;
                }
                string department = dt.Rows[i]["部门"].ToString().Trim();
                string position = dt.Rows[i]["职位"].ToString().Trim();
                int iSex = (sex == "女") ? 0 : 1;

                StringBuilder sb_user = new StringBuilder();
                sb_user.Append("SELECT User_ID,HotelId FROM Base_UserInfo WHERE User_Account= @User_Account AND DeleteMark = 1");
                SqlParam[] parm_user = new SqlParam[] {
                    new SqlParam("@User_Account",phone)
                };
                DataTable dt_user = DataFactory.SqlDataBase().GetDataTableBySQL(sb_user, parm_user);

                string org_id = "";
                if (department != "")
                {
                    #region 部门信息

                    StringBuilder sb_org = new StringBuilder();
                    sb_org.Append(@"SELECT Organization_ID FROM dbo.Base_Organization WHERE ParentId = '0' AND Organization_Name = @bumen");
                    SqlParam[] parm_org = new SqlParam[] {
                        new SqlParam("@bumen",department)
                    };

                    DataTable dt_org = DataFactory.SqlDataBase().GetDataTableBySQL(sb_org, parm_org);
                    if (dt_org != null && dt_org.Rows.Count > 0)
                    {
                        org_id = dt_org.Rows[0]["Organization_ID"].ToString();
                    }
                    else
                    {
                        Hashtable ht_org = new Hashtable();
                        string oid = CommonHelper.GetGuid;
                        ht_org["Organization_ID"] = oid;
                        ht_org["Organization_Name"] = department;
                        ht_org["ParentId"] = "0";
                        ht_org["DeleteMark"] = "1";
                        ht_org["AdminHotelId"] = hdAdminHotelId.Value;
                        ht_org["HotelId"] = hdHotelId.Value;

                        ht_org["CreateDate"] = DateTime.Now;
                        ht_org["CreateUserId"] = hdUserId.Value;
                        ht_org["CreateUserName"] = RequestSession.GetSessionUser().UserName.ToString();
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_Organization", "", "", ht_org);
                        if (IsOk)
                        {
                            org_id = oid;
                        }
                    }
                    #endregion

                    if (position != "")
                    {
                        #region 职位信息

                        sb_org = new StringBuilder();
                        sb_org.Append(@"
                        SELECT  Organization_ID
                        FROM    dbo.Base_Organization
                        WHERE   Organization_Name = @zhiwei
                                AND ParentId = @ParentId
                        ");
                        parm_org = new SqlParam[] {
                            new SqlParam("@zhiwei", position),
                            new SqlParam("@ParentId", org_id)
                        };
                        dt_org = DataFactory.SqlDataBase().GetDataTableBySQL(sb_org, parm_org);
                        if (dt_org != null && dt_org.Rows.Count > 0)
                        {
                            org_id = dt_org.Rows[0]["Organization_ID"].ToString();
                        }
                        else
                        {
                            Hashtable ht_org = new Hashtable();
                            string oid = CommonHelper.GetGuid;
                            ht_org["Organization_ID"] = oid;
                            ht_org["Organization_Name"] = position;
                            ht_org["ParentId"] = org_id;
                            ht_org["DeleteMark"] = "1";
                            ht_org["AdminHotelId"] = hdAdminHotelId.Value;
                            ht_org["HotelId"] = hdHotelId.Value;

                            ht_org["CreateDate"] = DateTime.Now;
                            ht_org["CreateUserId"] = hdUserId.Value;
                            ht_org["CreateUserName"] = RequestSession.GetSessionUser().UserName.ToString();
                            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_Organization", "", "", ht_org);
                            if (IsOk)
                            {
                                org_id = oid;
                            }
                        }
                        #endregion
                    }
                }

                if (dt_user != null && dt_user.Rows.Count > 0)
                {
                    if (dt_user.Rows[0]["HotelId"].ToString() == hdHotelId.Value)
                    {
                        Hashtable ht = new Hashtable();
                        ht["User_Name"] = name;
                        ht["User_Sex"] = iSex;
                        ht["Organization_ID"] = org_id;
                        string User_ID = dt_user.Rows[0]["User_ID"].ToString();
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "User_ID", User_ID, ht);
                    }
                }
                else
                {
                    string guid = CommonHelper.GetGuid;
                    Hashtable ht = new Hashtable();
                    ht["IsAdmin"] = 2;
                    ht["User_Name"] = name;
                    ht["User_Account"] = phone;
                    ht["User_Pwd"] = Md5Helper.MD5("123456", 32);
                    ht["User_Sex"] = iSex;
                    ht["Email"] = phone;
                    ht["Title"] = "";
                    ht["Theme"] = "";//电子邮箱
                    ht["User_Remark"] = "导入";//备注
                    ht["hotelid"] = hdHotelId.Value;
                    ht["HotelListId"] = hdHotelId.Value;
                    ht["AdminHotelid"] = hdAdminHotelId.Value;
                    ht["User_ID"] = guid;
                    ht["CreateUserId"] = hdUserId.Value;
                    ht["CreateUserName"] = RequestSession.GetSessionUser().UserName.ToString();
                    ht["CreateDate"] = DateTime.Now;
                    ht["DeleteMark"] = 1;
                    ht["Organization_ID"] = org_id;
                    bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "User_ID", "", ht);

                    string Roles_ID = jdyg_rid;
                    if (IsOk && Roles_ID != "")
                    {
                        #region  ** 角色 **
                        DataFactory.SqlDataBase().DeleteData("Base_UserRole", "User_ID", guid);//删除角色
                        Hashtable htRole = new Hashtable();
                        htRole["UserRole_ID"] = CommonHelper.GetGuid;
                        htRole["User_ID"] = guid;
                        htRole["Roles_ID"] = Roles_ID;
                        DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserRole", "User_ID", "", htRole);//重新添加角色
                        #endregion

                    }
                }

                ShowMsgHelper.OpenClose("操作成功！");
            }
        }

        public DataTable ExcelDataSource()//string filepath, string sheetname
        {
            //DataSet ds = new DataSet();
            Regex regex = new Regex(".+//.xls$");
            //客户端导入Excel的路径   
            string filepath = FileUpload1.PostedFile.FileName.ToString();
            // 找到服务器端的目录结构   
            string NewPath = Server.MapPath("~/upload/");
            string strType = System.IO.Path.GetExtension(FileUpload1.FileName);
            string filename = NewPath + "UsersFile" + strType;
            if (filepath != "")
            {
                //根据时间创建目录名称   
                if (!Directory.Exists(NewPath))
                {
                    //创建目录结构   
                    Directory.CreateDirectory(NewPath);
                }
                if (strType == ".xlsx")
                {
                    FileUpload1.SaveAs(filename);
                }
                else
                {
                    FileUpload1.SaveAs(filename);
                }
            }
            string strConn = "";
            if (strType == ".xlsx")
            {
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties='Excel 12.0;IMEX=1'";
            }
            else if (strType == ".xls")
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
            }
            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbDataAdapter oada = new OleDbDataAdapter(" SELECT * FROM [Sheet1$]", strConn);
            conn.Close();
            conn.Dispose();
            DataSet ds = new DataSet();
            oada.Fill(ds);
            DataTable dtt = ds.Tables[0];
            return dtt;
        }
    }
}