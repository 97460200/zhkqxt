using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.App_Code;
using System.Text;
using RM.Common.DotNetBean;
using System.Data;
using RM.Busines;
using RM.Common.DotNetCode;

namespace RM.Web.RMBase.SysUser
{
    public partial class StoreRole_List : PageBase
    {
        public String hotelTreeHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Hdhoteladmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                bool blHotelTree = false;//是否有多分店权限 多店显示酒店树
                string HotelId = "";//如果只有一家店 默认的酒店ID
                if (RequestSession.GetSessionUser().Hotelid.ToString() == "0")
                {
                    hotelTreeHtml = HotelTreeHelper.HotelTree(Hdhoteladmin.Value, 2, out blHotelTree, out HotelId);
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
}