using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;
using System.Data;
using RM.Busines;
using System.Text;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.memInfo
{
    public partial class memAdminInfo : System.Web.UI.Page
    {
        public string ae;
        public string kh;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["lsh"]))
                {
                    hdMemberId.Value = ae = Request.QueryString["lsh"];
                    string iUrl = "ReservationList.aspx?lsh=" + ae;
                    if (!string.IsNullOrEmpty(Request.QueryString["type"]))
                    {
                        switch (Request.QueryString["type"])
                        {
                            case "2":
                                iUrl = "memcard.aspx?lsh" + ae;
                                break;
                            case "3":
                                iUrl = "IntegralList.aspx?lsh=" + ae;
                                break;
                            default:
                                break;
                        }
                    }
                    iframe1.Attributes.Add("src", iUrl);

                    string sql = string.Format(@"select hy.xb,hy.kh,ISNULL(hy.ksjf,0) as ksjf,hy.lsh,ISNULL(hy.hykye,0) as hykye,hy.xm,hy.headimgurl,hy.sjhm ,hy.carid,hy.addtime,mb.hylxname 
            from hy_hyzlxxb hy LEFT JOIN dbo.hy_hylxbmb mb ON hy.hylx=mb.hylxcode where  lsh=@lsh");

                    SqlParam[] parmAdd2 = new SqlParam[] { 
                                        new SqlParam("@lsh", Request["lsh"])};
                    DataTable ds = DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).GetDataTableBySQL(new StringBuilder(sql), parmAdd2);
                    if (ds != null && ds.Rows.Count > 0)
                    {

                        //称谓
                        if (ds.Rows[0]["xb"].ToString() == "F")
                        {
                            lblSex.InnerText = "女";
                        }
                        else
                        {
                            lblSex.InnerText = "男";
                        }
                        if (ds.Rows[0]["xm"] != null && ds.Rows[0]["carid"].ToString() != "")
                        {
                            hdOpenId.Value = ds.Rows[0]["carid"].ToString();
                            aOperationLog.HRef = "OperationLog.aspx?openid=" + hdOpenId.Value;
                            aMemberSource.HRef = "MemberSource.aspx?openid=" + hdOpenId.Value;
                            aWithdraw.HRef = "../../RMBase/SysMember/Sales_withdrawList.aspx?Openid=" + hdOpenId.Value;
                        }
                        //会员来源
                        kh = khs.InnerText = ds.Rows[0]["kh"].ToString();
                        xm.InnerText = name.InnerText = ds.Rows[0]["xm"].ToString();
                        xm.InnerText += "[" + kh + "]" + "[" + ds.Rows[0]["hylxname"].ToString() + "]";
                        sjhm.InnerText = ds.Rows[0]["sjhm"].ToString();
                        addtime.InnerText = ds.Rows[0]["addtime"].ToString();
                        jf.InnerText = ds.Rows[0]["ksjf"].ToString().Split('.')[0] + "分";
                        hykye.InnerText = "￥" + ds.Rows[0]["hykye"].ToString().Split('.')[0];

                        StringBuilder sqls = new StringBuilder();
                        sqls.AppendFormat("SELECT ISNULL(SUM(TomePrice),0) FROM Reservation WHERE MemberId='{0}' AND Pay=1 AND AdminHotelid='{1}' ", Request["lsh"], RequestSession.GetSessionUser().AdminHotelid.ToString());
                        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sqls);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            ddze.InnerText = "￥" + dt.Rows[0][0].ToString().Split('.')[0];
                        }

                    }
                }

            }
        }
    }
}