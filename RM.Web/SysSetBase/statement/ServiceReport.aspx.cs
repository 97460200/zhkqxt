using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Busines;
using System.Data;
using System.Collections;
using RM.Common.DotNetUI;

namespace RM.Web.SysSetBase.statement
{
    public partial class ServiceReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindHotel();
                QueryData();
            }
        }
        private void BindHotel()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"
            SELECT  a.HotelId ,
                    b.name HotelName ,
                    SUM(fkje) fkje
            FROM    dbo.XX_WXFKJL a
                    LEFT JOIN dbo.Hotel b ON a.HotelId = b.ID
            GROUP BY a.HotelId ,
                    b.name
            ORDER BY fkje DESC
            ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
            ddlHotel.DataSource = dt;
            ddlHotel.DataTextField = "HotelName";
            ddlHotel.DataValueField = "HotelId";
            ddlHotel.DataBind();
            ddlHotel.Items.Insert(0, new ListItem("所有酒店", "0"));

            for (int i = 2018; i <= DateTime.Now.Year; i++)
            {
                ddlYear.Items.Add(i.ToString());
            }
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
        }
        protected void lbSumit_Click(object sender, EventArgs e)
        {
            QueryData();
        }

        private void QueryData()
        {
            spSysTime.InnerHtml = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            string StartDate = "";
            string EndDate = "";
            bool bl = LoadTitle(out StartDate, out EndDate);
            if (!bl)
            {
                return;
            }
            Hashtable ht = new Hashtable();
            ht["AdminHotelId"] = hdAdminHotelId.Value;
            ht["HotelId"] = ddlHotel.SelectedValue;
            ht["PaySource"] = hdPaySource.Value;
            ht["StartDate"] = StartDate;
            ht["EndDate"] = EndDate;
            ht["ReportType"] = hdReportType.Value;

            DataTable dt = DataFactory.SqlDataBase().GetDataTableProc("P_ServiceScanPay_Report", ht);
            string dataHtml = "";

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string ps = dt.Rows[i]["PaySource"].ToString();
                    string tr = string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td>", i + 1, dt.Rows[i]["HotelName"], ps == "1" ? "微信" : ps == "2" ? "支付宝" : "");
                    switch (hdReportType.Value)
                    {
                        case "0":
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["合计"], 0));
                            break;
                        case "1":
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["合计"], 0));
                            break;
                        case "2":
                            DateTime dtMonth = Convert.ToDateTime(txtStartMonth.Value + "-01");
                            spReportDate.InnerHtml = dtMonth.ToString("yyyy年MM月");
                            int days = DateTime.DaysInMonth(dtMonth.Year, dtMonth.Month);
                            for (int m = 1; m <= days; m++)
                            {
                                tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["day" + m], 0));
                            }
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["合计"], 0));
                            break;
                        case "3":
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["01月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["02月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["03月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["04月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["05月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["06月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["07月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["08月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["09月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["10月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["11月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["12月"], 0));
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["合计"], 0));
                            break;
                        case "4":

                            DateTime sd = Convert.ToDateTime(txtStartDate.Value);
                            DateTime ed = Convert.ToDateTime(txtEndDate.Value);
                            int day4 = (ed - sd).Days;
                            if (day4 > 31)
                            {
                                return;
                            }

                            for (int z = 0; z <= day4; z++)
                            {
                                int m = sd.Day + z;
                                tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["day" + m], 0));
                            }
                            tr += string.Format("<td>{0}</td>", ConvertHelper.ToDouble(dt.Rows[i]["合计"], 0));

                            //for (int z = 0; i <= day4; i++)
                            //{
                            //    dtTitle.Rows.Add(sd.AddDays(i).ToString("dd"));
                            //}

                            break;
                        default:
                            break;
                    }
                    tr += "</tr>";
                    dataHtml += tr;
                }
            }
            tbData.InnerHtml = dataHtml;
        }

        private bool LoadTitle(out string StartDate, out string EndDate)
        {
            StartDate = "";
            EndDate = "";

            DataTable dtTitle = new DataTable();
            dtTitle.Columns.Add("Name", typeof(String));
            string dt = hdReportType.Value;
            if (dt == "0")
            {
                dtTitle.Rows.Add("合计");
                spReportDate.InnerHtml = "全部";
            }
            else if (dt == "1")
            {
                DateTime dtDay = Convert.ToDateTime(txtDate.Value);
                spReportDate.InnerHtml = dtDay.ToString("yyyy年MM月dd日");

                dtTitle.Rows.Add(txtDate.Value);
                StartDate = txtDate.Value;
                EndDate = Convert.ToDateTime(txtDate.Value).AddDays(1).ToString("yyyy-MM-dd");
            }
            else if (dt == "2")
            {
                DateTime dtMonth = Convert.ToDateTime(txtStartMonth.Value + "-01");
                spReportDate.InnerHtml = dtMonth.ToString("yyyy年MM月");
                int days = DateTime.DaysInMonth(dtMonth.Year, dtMonth.Month);
                for (int i = 0; i < days; i++)
                {
                    dtTitle.Rows.Add(dtMonth.AddDays(i).ToString("dd"));
                }
                dtTitle.Rows.Add("合计");
                StartDate = dtMonth.ToString("yyyy-MM-dd");
                EndDate = dtMonth.AddMonths(1).ToString("yyyy-MM-dd");
            }
            else if (dt == "3")
            {
                DateTime dtYear = Convert.ToDateTime(ddlYear.SelectedValue + "-01-01");
                spReportDate.InnerHtml = ddlYear.SelectedValue + "年";
                dtTitle.Rows.Add("01月");
                dtTitle.Rows.Add("02月");
                dtTitle.Rows.Add("03月");
                dtTitle.Rows.Add("04月");
                dtTitle.Rows.Add("05月");
                dtTitle.Rows.Add("06月");
                dtTitle.Rows.Add("07月");
                dtTitle.Rows.Add("08月");
                dtTitle.Rows.Add("09月");
                dtTitle.Rows.Add("10月");
                dtTitle.Rows.Add("11月");
                dtTitle.Rows.Add("12月");
                dtTitle.Rows.Add("合计");
                StartDate = dtYear.ToString("yyyy-MM-dd");
                EndDate = dtYear.AddYears(1).ToString("yyyy-MM-dd");
            }
            else if (dt == "4")
            {
                DateTime sd = Convert.ToDateTime(txtStartDate.Value);
                DateTime ed = Convert.ToDateTime(txtEndDate.Value);
                int day4 = (ed - sd).Days;
                if (day4 > 31)
                {
                    ShowMsgHelper.ExecuteScript("操作失败！日期跨度不能超过一个月");
                    return false;
                }
                for (int i = 0; i <= day4; i++)
                {
                    dtTitle.Rows.Add(sd.AddDays(i).ToString("dd"));
                }
                dtTitle.Rows.Add("合计");
                StartDate = sd.ToString("yyyy-MM-dd");
                EndDate = ed.AddDays(1).ToString("yyyy-MM-dd");
            }
            rptTitle.DataSource = dtTitle;
            rptTitle.DataBind();
            return true;
        }
    }
}