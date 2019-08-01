using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;
using RM.Common.DotNetUI;
using System.Text;
using System.Data;
using RM.Busines;
using System.Collections;
using System.IO;
using RM.Web.App_Code;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.memInfo
{
    public partial class MemberAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();
                txtCreateTime.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            
                StringBuilder str = new StringBuilder();
                str.Append(string.Format(" SELECT * FROM hy_hylxbmb  WHERE AdminHotelid=@AdminHotelid "));
                List<SqlParam> ilistStr = new List<SqlParam>();
                ilistStr.Add(new SqlParam("@AdminHotelid", AdminHotelid));
                str.Append("  ORDER BY Sort DESC");
                DataTable dtstr = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(str, ilistStr.ToArray());
                if (dtstr != null && dtstr.Rows.Count > 0)
                {
                    this.ddlMemberLevel.DataSource = dtstr;
                    this.ddlMemberLevel.DataTextField = "hylxname";
                    this.ddlMemberLevel.DataValueField = "hylxcode";
                    this.ddlMemberLevel.DataBind();
                }
            }
        }


        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();
            Hashtable hy = new Hashtable();
            hy["xm"] = txtName.Value;
            hy["xb"] = "M";
            hy["kh"] = txtPhone.Value;
            hy["hylx"] = ddlMemberLevel.SelectedValue;
            hy["sjhm"] = txtPhone.Value;
            hy["hyklx"] = "T";
            hy["Carid"] = "";
            hy["headimgurl"] = "";
            hy["AdminHotelid"] = AdminHotelid;
            hy["scmcode"] = "WX";
            hy["krlxcode"] = "WX";
            hy["skkh"] = txtPhone.Value;
            hy["czyh"] = "999999";
            hy["zzbj"] = "F";
            hy["gj"] = "CN";
            hy["bmcode"] = "99";
            hy["bz"] = txtNote.Value;
            hy["addtime"] = txtCreateTime.Value;
            int ss = DataFactory.SqlDataBase(AdminHotelid).InsertByHashtable("hy_hyzlxxb", hy);
            if (ss > 0)
            {
                ShowMsgHelper.AlertClose("添加成功！");
            }
            else 
            {
                ShowMsgHelper.AlertClose("添加失败！");
            }
        }
    }
}