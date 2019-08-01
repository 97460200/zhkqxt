using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using SQL;
using System.Data.SqlClient;
using DBUtility;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.OleDb;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Services.Description;
using RM.Busines;
using RM.Common.DotNetCode;
using RM.Common.DotNetEncrypt;

namespace RM.Web.RMBase.SysUser
{
    public partial class daoru : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



        }



        #region 保存事件
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {

            if (FileUpload1.HasFile == false)//HasFile用来检查FileUpload是否有指定文件
            {
                Response.Write("<script>alert('请您选择Excel文件')</script> ");
                return;//当无文件时,返回
            }
            string FileNameLength = FileUpload1.FileName.ToString();
            if (FileNameLength.Length > 30)//HasFile用来检查FileUpload是否有指定文件
            {
                Response.Write("<script>alert('请您修改Excel文件名和工作表名称的长度')</script> ");
                return;//当无文件时,返回
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
            System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();//
            if (IsXls != ".xls" && IsXls != ".xlsx" && IsXls != ".csv")  //
            {
                Response.Write("<script>alert('只可以选择Excel文件')</script>");
                return;//当选择的不是Excel文件时,返回
            }

            DataTable dt = ExcelDataSource();

            //拼接字段
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StringBuilder sb_user = new StringBuilder();
                sb_user.Append("SELECT User_ID FROM Base_UserInfo WHERE User_Account= @User_Account and  hotelid = @hotelid ");
                SqlParam[] parm_user = new SqlParam[] {
                    new SqlParam("@User_Account", dt.Rows[i]["手机号码"].ToString()),
                    new SqlParam("@hotelid",97)
                };

                DataTable dt_user = DataFactory.SqlDataBase().GetDataTableBySQL(sb_user, parm_user);


                StringBuilder sb_org = new StringBuilder();
                sb_org.Append(@"
                 SELECT    Organization_ID
                                          FROM      dbo.Base_Organization
                                          WHERE     Organization_Name = @bumen

                ");
                SqlParam[] parm_org = new SqlParam[] {
                    new SqlParam("@bumen", dt.Rows[i]["部门"].ToString())
                };

                DataTable dt_org = DataFactory.SqlDataBase().GetDataTableBySQL(sb_org, parm_org);

                string org_id = "";
                if (dt_org != null && dt_org.Rows.Count > 0)
                {
                    org_id = dt_org.Rows[0]["Organization_ID"].ToString();
                }


                sb_org = new StringBuilder();
                sb_org.Append(@"
                SELECT  Organization_ID
                FROM    dbo.Base_Organization
                WHERE   Organization_Name = @zhiwei
                        AND ParentId IN ( SELECT    Organization_ID
                                          FROM      dbo.Base_Organization
                                          WHERE     Organization_ID = @org_id )

                ");
                parm_org = new SqlParam[] {
                    new SqlParam("@zhiwei", dt.Rows[i]["职位"].ToString()),
                    new SqlParam("@org_id", org_id)
                };

                dt_org = DataFactory.SqlDataBase().GetDataTableBySQL(sb_org, parm_org);

                if (dt_org != null && dt_org.Rows.Count > 0)
                {
                    org_id = dt_org.Rows[0]["Organization_ID"].ToString();
                }


                if (dt_user != null && dt_user.Rows.Count > 0)
                {
                    Hashtable ht = new Hashtable();
                    ht["Organization_ID"] = org_id;
                    string User_ID = dt_user.Rows[0]["User_ID"].ToString();
                    bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "User_ID", User_ID, ht);
                }
                else
                {
                    string guid = CommonHelper.GetGuid;
                    Hashtable ht = new Hashtable();
                    ht["IsAdmin"] = 2;
                    ht["User_Name"] = dt.Rows[i]["姓名"].ToString();
                    ht["User_Account"] = dt.Rows[i]["手机号码"].ToString();
                    ht["User_Pwd"] = Md5Helper.MD5("123456", 32);
                    if (dt.Rows[i]["性别"].ToString() == "男")
                    {
                        ht["User_Sex"] = 1;
                    }
                    else
                    {
                        ht["User_Sex"] = 2;
                    }

                    ht["Email"] = dt.Rows[i]["手机号码"].ToString();
                    ht["Title"] = "";
                    ht["Theme"] = "";//电子邮箱
                    ht["User_Remark"] = "导入";//备注
                    ht["hotelid"] = 97;
                    ht["HotelListId"] = 97;
                    ht["AdminHotelid"] = "1001587";

                    ht["User_ID"] = guid;
                    ht["CreateUserId"] = "2E0CA5B1252F1F6B1E0AC91BE7E7219E";
                    ht["CreateUserName"] = "宝晖商务酒店";
                    ht["CreateDate"] = DateTime.Now;
                    ht["DeleteMark"] = 1;
                    ht["Organization_ID"] = org_id;

                    //c7b1c600-52b3-4789-be30-89855d3c1411  --酒店员工
                    //bd9653be-3151-4b81-a54a-cac3714f79ef  --酒店财务
                    //cb4ee06b-0591-48b4-8d59-c51677dca8f2  --酒店经理
                    //f727621e-5775-4f74-a551-e73532b3901b  --酒店管理员

                    string zw = dt.Rows[i]["职位"].ToString();

                    string Roles_ID = "c7b1c600-52b3-4789-be30-89855d3c1411";
                    if (zw.IndexOf("管理员") > -1)
                    {
                        Roles_ID = "f727621e-5775-4f74-a551-e73532b3901b";
                    }
                    else if (zw.IndexOf("经理") > -1)
                    {
                        Roles_ID = "cb4ee06b-0591-48b4-8d59-c51677dca8f2";
                    }
                    else if (dt.Rows[i]["部门"].ToString() == "财务部")
                    {
                        Roles_ID = "bd9653be-3151-4b81-a54a-cac3714f79ef";
                    }

                    bool IsOk;
                    IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "User_ID", "", ht);
                    if (IsOk)
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
            string fileNamed = Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName); //获取文件名（不包括扩展名）
            string FileNames = FileUpload1.FileName.ToString();
            if (filepath != "")
            {
                //if (regex.Match(filepath).Success)
                //{ 
                DateTime dt = DateTime.Now;

                //根据时间创建目录名称   
                string fileName = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString();
                NewPath = NewPath + fileName + Path.DirectorySeparatorChar;
                NewPath = NewPath.Replace("//", "////");
                if (!Directory.Exists(NewPath))
                {
                    //创建目录结构   
                    Directory.CreateDirectory(NewPath);
                }
                //将文件保存到新建的目录结构下面   
                //FileUpload1.SaveAs(NewPath + "数据模板.xls");
                //if (File.Exists(filepath))
                //{
                if (strType == ".xlsx")
                {
                    FileUpload1.SaveAs(NewPath + FileNames);
                }
                else
                {
                    FileUpload1.SaveAs(NewPath + FileNames);
                }
                //}
                //}
            }
            string strConn = "";
            //if (File.Exists(filepath))
            //{
            if (strType == ".xlsx")
            {
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + NewPath + FileNames + ";Extended Properties='Excel 12.0;IMEX=1'";
            }
            else if (strType == ".xls")
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + NewPath + FileNames + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
            }
            //}
            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbDataAdapter oada = new OleDbDataAdapter(" SELECT * FROM [Sheet1$]", strConn);
            conn.Close();
            conn.Dispose();
            DataSet ds = new DataSet();
            oada.Fill(ds);
            DataTable dtt = ds.Tables[0];
            return dtt;
        }
        #endregion
    }
}