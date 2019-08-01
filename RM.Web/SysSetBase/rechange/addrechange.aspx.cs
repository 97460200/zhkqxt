using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;
using RM.Common.DotNetUI;
using System.Text;
using System.Data;
using RM.Busines;
using System.Collections;
using System.IO;
using RM.Web.App_Code;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.rechange
{
    public partial class addrechange : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //SessionUser user = new SessionUser();
                //user.AdminHotelid = "1";
                //RequestSession.AddSessionUser(user);


                string sql1 = string.Format(@"select REPLACE(hylxcode, ' ', '')  hylxcode,hylxname from hy_hylxbmb where AdminHotelid='{0}' ",RequestSession.GetSessionUser().AdminHotelid.ToString());
                hyjb.DataSource = DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).GetDataTableBySQL(new StringBuilder(sql1));
                hyjb.DataTextField = "hylxname";
                hyjb.DataValueField = "hylxcode";
                hyjb.DataBind();

                StringBuilder sb = new StringBuilder();
                sb.Append("select ID,CouponName from V_coupon where TypeName='单次充值赠送' AND isend='未过期' ");
                if (RequestSession.GetSessionUser().AdminHotelid != null)
                {
                    sb.Append(" and AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid + "' ");
                }
                
                yhq.DataSource = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
                yhq.DataTextField = "CouponName";
                yhq.DataValueField = "ID";
                yhq.DataBind();


                StringBuilder sb_hotel = new StringBuilder();
                sb_hotel.Append(string.Format(" SELECT ID,name FROM Hotel WHERE    AdminHotelid IN ( SELECT  AdminHotelid FROM Hotel_Admin WHERE AdminHotelid=@AdminHotelid ) "));
                List<SqlParam> listStr = new List<SqlParam>();
                listStr.Add(new SqlParam("@AdminHotelid", RequestSession.GetSessionUser().AdminHotelid));
                sb_hotel.Append("  ORDER BY ID DESC");
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_hotel, listStr.ToArray());
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ddlHotel.DataSource = dt;
                    this.ddlHotel.DataTextField = "name";
                    this.ddlHotel.DataValueField = "ID";
                    this.ddlHotel.DataBind();
                }



                if (Request["id"] != null)
                {
                    string sql = string.Format(@"select id,moneys,zsmoneys,REPLACE(hylxcode, ' ', '') hylxcode,zsjf,bz,iszsmoneys,iszsjf,ishylxcode,iscouponid,couponid,HotelId from CardRecharge where id=@id");
                    SqlParam[] parmAdd2 = new SqlParam[] { 
                                     new SqlParam("@id",Request["id"]) };
                    DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd2);
                    if (ds != null && ds.Rows.Count > 0)
                    {
                        czje.Text = ds.Rows[0]["moneys"].ToString();
                        zsje.Text = ds.Rows[0]["zsmoneys"].ToString();
                        zsjf.Text = ds.Rows[0]["zsjf"].ToString();
                        //yhq.SelectedValue = ds.Rows[0]["couponid"].ToString();
                        couponids.Value = ds.Rows[0]["couponid"].ToString() + ",";
                        hyjb.SelectedValue = ds.Rows[0]["hylxcode"].ToString();
                        txtInfo.Value = ds.Rows[0]["bz"].ToString();
                        iszsmoneys.Value = ds.Rows[0]["iszsmoneys"].ToString();
                        iszsjf.Value = ds.Rows[0]["iszsjf"].ToString();
                        ishylxcode.Value = ds.Rows[0]["ishylxcode"].ToString();
                        iscouponid.Value = ds.Rows[0]["iscouponid"].ToString();
                        if (ds.Rows[0]["HotelId"] != null && ds.Rows[0]["HotelId"].ToString() != "")
                        {
                            ddlHotel.SelectedValue = ds.Rows[0]["HotelId"].ToString();
                        }
         
                    }
                }
                else
                {
                    btnSubmit.Text = "添加";
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
            Hashtable hs = new Hashtable();
            hs["moneys"] = czje.Text;
            if (zsje.Text.Trim() == "")
            {
                hs["zsmoneys"] = 0;
            }
            else
            {
                hs["zsmoneys"] = zsje.Text;
            }

            if (zsjf.Text.Trim() == "")
            {
                hs["zsjf"] = 0;
            }
            else
            {
                hs["zsjf"] = zsjf.Text;
            }

            hs["couponid"] = couponids.Value.Trim(','); //yhq.SelectedValue;
            hs["HYLXNAME"] = hyjb.SelectedItem.Text;
            hs["iszsmoneys"] = iszsmoneys.Value;
            hs["iszsjf"] = iszsjf.Value;
            hs["ishylxcode"] = ishylxcode.Value;
            hs["iscouponid"] = iscouponid.Value;
            hs["hylxcode"] = hyjb.SelectedValue;
            hs["bz"] = txtInfo.Value;
            hs["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
            hs["HotelId"] = ddlHotel.SelectedValue;
            if (Request["id"] != null)
            {
                int i = DataFactory.SqlDataBase().UpdateByHashtable("CardRecharge", "ID", Request["id"], hs);
                if (i > 0)
                {
                    ShowMsgHelper.OpenClose("修改成功！");
                }
                else
                {
                    ShowMsgHelper.Alert_Error("修改失败！");
                }
            }
            else
            {
                int i = DataFactory.SqlDataBase().InsertByHashtable("CardRecharge", hs);
                if (i > 0)
                {
                    ShowMsgHelper.OpenClose("添加成功！");
                }
                else
                {
                    ShowMsgHelper.Alert_Error("添加失败！");
                }
            }

        }

    }
}