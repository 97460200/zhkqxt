using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;
using RM.Common.DotNetUI;


using RM.Web.App_Code;
using RM.Busines;
using System.Text;
using System.Data;
using System.Collections;
using RM.Common.DotNetBean;
namespace RM.Web.SysSetBase.Busin
{
    public partial class addcolu : System.Web.UI.Page
    {
        int sort;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }
        void bind()
        {
            if (Request["ID"] != null)
            {
                int Id = Convert.ToInt32(Request["ID"]);
                string sql = string.Format("select * from BookType where Id={0}", Id);
                DataTable ds1 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds1 != null && ds1.Rows.Count > 0)
                {
                    txtTypeName.Value = ds1.Rows[0]["TypeName"].ToString();
                    txtInstructions.Value = ds1.Rows[0]["Instructions"].ToString();
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
            Hashtable hss = new Hashtable();
            hss["TypeName"] = txtTypeName.Value;
            hss["Instructions"] = txtInstructions.Value;
            hss["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
            hss["Hotelid"] = Request["HotelId"];
            if (Request["ID"] != null)
            {
                //处理
                int a = DataFactory.SqlDataBase().UpdateByHashtable("BookType", "id", Request["ID"], hss);
                if (a > 0)
                {
                    CommonMethod.Base_Log("修改", "BookType", Request["ID"], "类型管理", "修改[" + txtTypeName.Value + "]");//操作日志
                    ShowMsgHelper.AlertMsg("编辑成功");
                }
                else
                {
                    ShowMsgHelper.Alert_Error("编辑失败！");
                }
            }
            else
            {

                string sql = string.Format("select sort from BookType order by Sort desc");
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds != null && ds.Rows.Count > 0)
                {
                    sort = Convert.ToInt32(ds.Rows[0]["Sort"].ToString()) + 1;
                }
                else
                {
                    sort = 1;
                }

                hss["Sort"] = sort;
                //添加
                int a = DataFactory.SqlDataBase().InsertByHashtable("BookType", hss);
                if (a > 0)
                {
                    CommonMethod.Base_Log("添加", "BookType", "", "类型管理", "添加[" + txtTypeName.Value + "]");//操作日志
                    ShowMsgHelper.AlertMsg("添加成功");
                }
                else
                {
                    ShowMsgHelper.Alert_Error("添加失败！");
                }
            }
        }
    }
}