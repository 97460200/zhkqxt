using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;

namespace RM.Web.SysSetBase.houseState
{
    public partial class setState : System.Web.UI.Page
    {
        public String hotelTreeHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtEndTime.Value = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
                bind();
            }
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
    }
}