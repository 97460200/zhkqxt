using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;
using System.Collections;
using System.Data;
using RM.Busines;
using System.Text;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;

namespace RM.Web.SysSetBase.statement
{
    public partial class FinancialReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //txtStartDate.Value = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
                //txtEndDate.Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                txtDate.Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

                hdAdminHotelId.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                Bind();
                BindHotel();
                QueryData();
            }
        }

        private void BindHotel()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ID,name FROM dbo.Hotel WHERE DeleteMark=1 And id In(Select HotelId From Hotel_BillDown Where AdminHotelId = @AdminHotelId Group By HotelId) ");
            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@AdminHotelId", hdAdminHotelId.Value));
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, lParam.ToArray());
            ddlHotel.DataSource = dt;
            ddlHotel.DataTextField = "name";
            ddlHotel.DataValueField = "ID";
            ddlHotel.DataBind();
        }


        private void Bind()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT BusinessHour,BusinessMinute FROM Hotel_AdminParameter WHERE AdminHotelId = @AdminHotelId");
            SqlParam[] param = new SqlParam[] {
                new SqlParam("@AdminHotelId",hdAdminHotelId.Value)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                int bh = CommonHelper.GetInt(dt.Rows[0]["BusinessHour"].ToString());//酒店的营业时间（时）
                int bm = CommonHelper.GetInt(dt.Rows[0]["BusinessMinute"].ToString());//酒店的营业时间（分）
                string bt = (bh < 10) ? "0" + bh : bh + "";
                bt += ":";
                bt += (bm < 10) ? "0" + bm : bm + "";
                hdBusinessTime.Value = bt;
            }
            spBusinessTime.InnerHtml = "当日" + hdBusinessTime.Value + "-次日" + hdBusinessTime.Value;
        }

        protected void lbSumit_Click(object sender, EventArgs e)
        {
            QueryData();
        }

        private void QueryData()
        {
            spSysTime1.InnerHtml = spSysTime2.InnerHtml = spSysTime3.InnerHtml = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            string StartDate = "";
            string EndDate = "";
            bool bl = LoadTitle(out StartDate, out EndDate);
            if (!bl)
            {
                ShowMsgHelper.ExecuteScript("操作失败，日期格式错误！");
                return;
            }
            Hashtable ht = new Hashtable();
            ht["AdminHotelId"] = hdAdminHotelId.Value;
            ht["HotelId"] = ddlHotel.SelectedValue;
            ht["PaySource"] = hdPaySource.Value;
            ht["Transaction_Type"] = hdTransaction_Type.Value;
            ht["StartDate"] = StartDate;
            ht["EndDate"] = EndDate;
            ht["DateType"] = hdDateType.Value;
            ht["ReportType"] = hdReportType.Value;

            DataTable dt = DataFactory.SqlDataBase().GetDataTableProc("P_Hotel_Bill_Report", ht);
            string title = "";
            switch (hdReportType.Value)
            {
                case "1":
                    title = "财务对账汇总表";
                    rptData1.DataSource = dt;
                    rptData1.DataBind();
                    #region ** 导出 **
                    if (hdExport.Value == "1" && dt != null && dt.Rows.Count > 0)
                    {
                        dt.Columns["Pay_Type"].ColumnName = "资金渠道";
                        dt.Columns["Income_Money"].ColumnName = "收入";
                        dt.Columns["Income_Number"].ColumnName = "收入笔数";
                        dt.Columns["Refund_Money"].ColumnName = "支出";
                        dt.Columns["Refund_Number"].ColumnName = "支出笔数";
                        dt.Columns["Total_Money"].ColumnName = "收支金额";
                        dt.Columns["Service_Charge"].ColumnName = "手续费";
                        dt.Columns["DayNetAmount"].ColumnName = "期末净额";
                        dt.Columns.Remove("SerialNumber");
                        dt.Columns.Remove("PaySource");
                        dt.Columns.Remove("Transaction_Type");
                        ExcelHelper.ExportExcel(dt, title);
                    }
                    #endregion
                    break;
                case "2":
                    title = "财务对账明细表";
                    rptData2.DataSource = dt;
                    rptData2.DataBind();
                    #region ** 导出 **
                    if (hdExport.Value == "1" && dt != null && dt.Rows.Count > 0)
                    {
                        dt.Columns["PaySource"].ColumnName = "资金渠道";
                        dt.Columns["Transaction_Type"].ColumnName = "收款场景";
                        dt.Columns["Transaction_Time"].ColumnName = "交易时间";
                        dt.Columns["Order_Numbe"].ColumnName = "商户订单号";
                        dt.Columns["Transaction_State"].ColumnName = "交易状态";
                        dt.Columns["Total_Money"].ColumnName = "收款金额";
                        dt.Columns["Refund_Numbe"].ColumnName = "商户退款单号";
                        dt.Columns["Refund_Money"].ColumnName = "退款金额";
                        dt.Columns["Service_Charge"].ColumnName = "手续费";
                        ExcelHelper.ExportExcel(dt, title);
                    }
                    #endregion
                    break;
                case "3":
                    title = "财务对账日统计表";
                    rptData3.DataSource = dt;
                    rptData3.DataBind();
                    #region ** 导出 **
                    if (hdExport.Value == "1" && dt != null && dt.Rows.Count > 0)
                    {
                        dt.Columns["Bill_Date"].ColumnName = "账单日期";
                        dt.Columns["Income_Money"].ColumnName = "收入";
                        dt.Columns["Income_Number"].ColumnName = "收入笔数";
                        dt.Columns["Refund_Money"].ColumnName = "支出";
                        dt.Columns["Refund_Number"].ColumnName = "支出笔数";
                        dt.Columns["Total_Money"].ColumnName = "收支金额";
                        dt.Columns["Service_Charge"].ColumnName = "手续费";
                        dt.Columns["DayNetAmount"].ColumnName = "日终净额";
                        ExcelHelper.ExportExcel(dt, title);
                    }
                    #endregion
                    break;
                default:
                    break;
            }
            spTitle.InnerHtml = title;
        }

        private bool LoadTitle(out string StartDate, out string EndDate)
        {
            StartDate = "";
            EndDate = "";
            try
            {
                string dt = hdTime.Value;
                if (dt == "0")
                {
                    DateTime dtDay = Convert.ToDateTime(txtDate.Value + " " + hdBusinessTime.Value);
                    if (hdDateType.Value == "0")
                    {
                        StartDate = dtDay.ToString("yyyy-MM-dd");
                        EndDate = dtDay.AddDays(1).ToString("yyyy-MM-dd");
                    }
                    else if (hdDateType.Value == "1")
                    {
                        StartDate = dtDay.ToString("yyyy-MM-dd HH:mm");
                        EndDate = dtDay.AddDays(1).ToString("yyyy-MM-dd HH:mm");
                    }
                    spDate.InnerHtml = dtDay.ToString("yyyy-MM-dd");
                }
                else if (dt == "1")
                {
                    DateTime sd = Convert.ToDateTime(txtStartDate.Value);
                    DateTime ed = Convert.ToDateTime(txtEndDate.Value);
                    StartDate = sd.ToString("yyyy-MM-dd HH:mm");
                    EndDate = ed.ToString("yyyy-MM-dd HH:mm");
                    spDate.InnerHtml = StartDate + "至" + EndDate;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}