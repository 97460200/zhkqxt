using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.App_Code;
using RM.Common.DotNetBean;
using System.Text;
using System.Data;
using RM.Busines;

namespace RM.Web.SysSetBase.houseState
{
    public partial class setPrice : PageBase
    {
        public String hotelTreeHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtEndTime.Value = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                bind();
                BindVip();
            }
            hdJFState.Value = HotelTreeHelper.GetJFState();
        }

        private void bind()
        {
            Hdhoteladmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
            bool blHotelTree = false;//是否有多分店权限 多店显示酒店树
            string HotelId = "";//如果只有一家店 默认的酒店ID

            if (RequestSession.GetSessionUser().Hotelid.ToString() == "0")
            {
                hotelTreeHtml = HotelTreeHelper.HotelTree(Hdhoteladmin.Value, 1, out blHotelTree, out HotelId);
            }
            else
            {
                blHotelTree = false;
                HotelId = RequestSession.GetSessionUser().Hotelid.ToString();
            }
            HotelTree.Visible = blHotelTree;
            htHotelTree.Value = blHotelTree.ToString();
            hdHotelId.Value = HotelId;
        }

        private DataTable dtVip = null;
        private void BindVip()
        {
            dtVip = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            rptVip.DataSource = dtVip;
            rptVip.DataBind();
        }
        public string GetFooterTr()
        {
            string ftr = "";
            if (dtVip != null)
            {
                for (int i = 0; i < dtVip.Rows.Count; i++)
                {
                    ftr += "<th>平日价<em class='jf'>/积分</em></th><th>周末价<em class='jf'>/积分</em></th>";
                }
            }
            return ftr;
        }
    }
}