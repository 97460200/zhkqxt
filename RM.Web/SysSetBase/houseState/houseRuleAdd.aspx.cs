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
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.houseState
{
    public partial class houseRuleAdd : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["hotelid"]))
                {
                    hdHotelId.Value = Request.QueryString["hotelid"];
                    bind();
                }
                BindVip();
                if (!string.IsNullOrEmpty(Request.QueryString["key"]))
                {
                    hdId.Value = Request.QueryString["key"];
                    edid();
                }
                else
                {
                    lbEdit.Visible = false;
                }
            }
        }

        private void bind()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT id,Name FROM dbo.Guestroom where hotelid=@hotelid order by sort desc");
            SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@hotelid",hdHotelId.Value)};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            rptRoom.DataSource = dt;
            rptRoom.DataBind();


        }
        private void BindVip()
        {
            rptVip.DataSource = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            rptVip.DataBind();
        }

        private void edid()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Room_Rule", "ID", hdId.Value);
            ControlBindHelper.SetWebControls(this.Page, ht);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Name FROM Guestroom where id=@id");
            SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@id",ht["ROOM_ID"])};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                lbEdit.InnerHtml = dt.Rows[0][0].ToString();
            }
        }


        #region 保存事件
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            string guid = CommonHelper.GetGuid;
            Hashtable ht = new Hashtable();
            ht["Rule_Name"] = Rule_Name.Value;
            ht["Sales_Val"] = Sales_Val.Value;
            ht["Sales_Type"] = Sales_Type.Value;
            ht["Breakfast_Type"] = Breakfast_Type.Value;
            ht["Breakfast_Val"] = Breakfast_Val.Value;
            ht["Pay_Type"] = Pay_Type.Value;
            ht["Vip_Type"] = Vip_Type.Value;
            ht["Vip_Val"] = Vip_Val.Value;
            ht["Remarks"] = Remarks.Value;
            if (!string.IsNullOrEmpty(Discount.Value))
            {
                ht["Discount"] = Discount.Value;
            }
            if (!string.IsNullOrEmpty(DiscountType.Value))
            {
                ht["DiscountType"] = DiscountType.Value;
            }
            if (!string.IsNullOrEmpty(jyDiscount.Value))
            {
                ht["jyDiscount"] = jyDiscount.Value;
            }

            bool IsOk = false;
            if (!string.IsNullOrEmpty(hdId.Value))
            {
                ht["ModifyDate"] = DateTime.Now;
                ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
                ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
                IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Room_Rule", "ID", hdId.Value, ht);
            }
            else
            {
                ht["Room_ID"] = Room_ID.Value;
                ht["HotelId"] = hdHotelId.Value;

                ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
                ht["CreateDate"] = DateTime.Now;
                string[] rommIds = hdRoomIds.Value.Split(',');
                for (int i = 0; i < rommIds.Count(); i++)
                {
                    ht["Room_ID"] = rommIds[i];
                    IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Room_Rule", "ID", "", ht);
                }
            }

            if (IsOk)
            {
                ShowMsgHelper.AlertReloadClose("操作成功！", "ListGrid()");
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }
        }
        #endregion
    }
}