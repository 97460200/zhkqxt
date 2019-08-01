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
using System.Collections;

namespace RM.Web.SysSetBase.sales
{
    public partial class bonusrecord : System.Web.UI.Page
    { 
        public string ae;
        public string kh;
        public String hotelTreeHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["User_ID"] != null)
                {

                    hdHotelId.Value = Request["HotelId"].ToString();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT User_ID,User_Account,User_Name,WX_Nickname,Roles_Name,CreateDate FROM V_Base_UserInfoRole   WHERE  AdminHotelid = '" + RequestSession.GetSessionUser().AdminHotelid.ToString() + "'  and hotelid = '" + hdHotelId.Value + "' and  User_ID='" + Request["User_ID"].ToString() + "' ");

                    DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
                    if (ds != null && ds.Rows.Count > 0)
                    {
                       
                        hdUser_ID.Value = ds.Rows[0]["User_ID"].ToString(); 
                        lblPhone.Text = ds.Rows[0]["User_Account"].ToString();
                        lblName.Text = ds.Rows[0]["User_Name"].ToString();
                        lblWXName.Text = ds.Rows[0]["WX_Nickname"].ToString();
                        lblRolse.Text = ds.Rows[0]["Roles_Name"].ToString();
                        lblAddTime.Text = Convert.ToDateTime(ds.Rows[0]["CreateDate"].ToString()).ToString("yyyy-MM-dd hh:mm");
                      
                        Hashtable hs = new Hashtable();
                        hs["UserId"] = ds.Rows[0]["User_ID"].ToString();
                        hs["Phone"] = ds.Rows[0]["User_Account"].ToString();
                        DataTable ds_Sales = DataFactory.SqlDataBase().GetDataTableProc("P_Sales_record", hs);
                        if (ds_Sales != null && ds_Sales.Rows.Count > 0)
                        {
                            //销量金额  @Sales_Total
                          lblSales_Total.Text= ds_Sales.Rows[0]["Sales_Total"].ToString();
                            //累计奖金 @Bonus_Total
                          lblBonus_Total.Text = ds_Sales.Rows[0]["Bonus_Total"].ToString();
                            //提现奖金 @WithdrawCash
                          lblWithdrawCash.Text = ds_Sales.Rows[0]["WithdrawCash"].ToString();
                            //剩余奖金 @ExtractableMoney
                          lblExtractableMoney.Text = ds_Sales.Rows[0]["ExtractableMoney"].ToString();
                            //带来客户 @Register_Total
                          lblRegister_Total.Text = ds_Sales.Rows[0]["Register_Total"].ToString();
                            //入住客户 @CheckIn_Total
                          lblCheckIn_Total.Text = ds_Sales.Rows[0]["CheckIn_Total"].ToString();
                        }


                    }
                }


                //树形菜单
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
}