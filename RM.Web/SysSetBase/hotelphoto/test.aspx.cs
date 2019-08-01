using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;

using System.Data;

using System.IO;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System.Text;
using RM.Common.DotNetBean;
namespace RM.Web.SysSetBase.hotelphoto
{
    public partial class test : System.Web.UI.Page
    {
        public String hotelTreeHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Hdhoteladmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                //bool blHotelTree = false;//是否有多分店权限 多店显示酒店树
                //string HotelId = "";//如果只有一家店 默认的酒店ID

                //if (RequestSession.GetSessionUser().Hotelid.ToString() == "0")
                //{
                //    hotelTreeHtml = HotelTreeHelper.HotelTree(Hdhoteladmin.Value, out blHotelTree, out HotelId);
                //}
                //else
                //{
                //    blHotelTree = false;
                //    HotelId = RequestSession.GetSessionUser().Hotelid.ToString();
                //}
                //HotelTree.Visible = blHotelTree;
                //htHotelTree.Value = blHotelTree.ToString();
                //hdHotelId.Value = HotelId;

            }
        }


        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumit_Click(object sender, EventArgs e)
        {

          
        }

    }
}