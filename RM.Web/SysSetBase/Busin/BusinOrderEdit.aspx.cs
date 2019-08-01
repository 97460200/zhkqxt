using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetCode;
using SQL;
using System.Data;
using RM.Common.DotNetUI;
using RM.Busines;
using RM.Common.DotNetBean;
using RM.Web.App_Code;
using System.Collections;
namespace RM.Web.SysSetBase.Busin
{
    public partial class BusinOrderEdit : System.Web.UI.Page
    {
        public string _State = string.Empty;
        public string _StateContent = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                bind();
            }
        }

        void bind()
        {

            if (Request.QueryString["ID"] != null)
            {

                string sql = string.Format(@"select * from V_BookOrder where ID='{0}'", Request["ID"]);
                DataTable ds1 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds1.Rows.Count > 0)
                {
                    if (ds1.Rows[0]["OrderNumber"] != null && ds1.Rows[0]["OrderNumber"].ToString() != "")
                    {
                        lblOrderNumber.Text = ds1.Rows[0]["OrderNumber"].ToString();
                    }

                    if (ds1.Rows[0]["BusinessName"] != null && ds1.Rows[0]["BusinessName"].ToString() != "")
                    {
                        lblBusinessName.Text = ds1.Rows[0]["BusinessName"].ToString();
                    }
                    if (ds1.Rows[0]["ReservedTime"] != null && ds1.Rows[0]["ReservedTime"].ToString() != "")
                    {
                        lblReservedTime.Text = ds1.Rows[0]["ReservedTime"].ToString();
                    }

                    if (ds1.Rows[0]["Contact"] != null && ds1.Rows[0]["Contact"].ToString() != "")
                    {
                        lblContact.Text = ds1.Rows[0]["Contact"].ToString();
                    }
                    if (ds1.Rows[0]["ContactPhone"] != null && ds1.Rows[0]["ContactPhone"].ToString() != "")
                    {
                        lblContactPhone.Text = ds1.Rows[0]["ContactPhone"].ToString();
                    }

                    if (ds1.Rows[0]["Number"] != null && ds1.Rows[0]["Number"].ToString() != "")
                    {
                        lblNumber.Text = ds1.Rows[0]["Number"].ToString();
                    }

                    if (ds1.Rows[0]["Requirements"] != null && ds1.Rows[0]["Requirements"].ToString() != "")
                    {
                        lblRequirements.Text = ds1.Rows[0]["Requirements"].ToString();
                    }
                    if (ds1.Rows[0]["Address"] != null && ds1.Rows[0]["Address"].ToString() != "")
                    {
                        lbAddress.Text = ds1.Rows[0]["Address"].ToString();
                    }


                    // 状态（1、已确认2、未确认3、已取消）
                    StringBuilder sb = new StringBuilder();
                    if (Convert.ToInt32(ds1.Rows[0]["State"]) == 1)
                    {
                        _State = hfState.Value = "1";
                        sb.Append(string.Format("<span class='ddzt blue'>已确认</span>"));

                    }
                    else if (Convert.ToInt32(ds1.Rows[0]["State"]) == 2)
                    {

                        _State = hfState.Value = "2";
                        sb.Append(string.Format("<span class='ddzt red'>未确认</span>"));
                    }
                    else if (Convert.ToInt32(ds1.Rows[0]["State"]) == 3)
                    {
                        _State = hfState.Value = "3";
                        sb.Append(string.Format("<span class='ddzt gray'>已取消</span>"));
                    }
                    _StateContent = sb.ToString();

                    if (ds1.Rows[0]["Processing"] != null && ds1.Rows[0]["Processing"].ToString() != "")
                    {
                        lblProcessing.Value = ds1.Rows[0]["Processing"].ToString();
                    }
                }

            }

        }
        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumit_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                Hashtable ht = new Hashtable();
                ht["State"] = hfState.Value;
                ht["Processing"] = lblProcessing.Value;
                bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("BookOrder", "ID", Request.QueryString["ID"].ToString(), ht);
                if (IsOk)
                {
                    ShowMsgHelper.AlertReloadClose("提交成功！", "ListGrid()");
                }
                else
                {
                    ShowMsgHelper.Alert_Error("提交失败！");

                }
            }


        }
    }
}