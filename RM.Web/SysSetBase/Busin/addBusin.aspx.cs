using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetCode;
using SQL;
using System.Data;
using RM.Common.DotNetUI;
using RM.Busines;
using RM.Common.DotNetBean;
using RM.Web.App_Code;
using System.Collections;
namespace RM.Web.SysSetBase.Busin
{
    public partial class addBusin : System.Web.UI.Page
    {

        private static string imagepath = "";
        int sort;
        public string _Reservation = string.Empty;
        public string _OrderMeal = string.Empty;
        public string _isopen = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Hdhoteladmin.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
            if (!IsPostBack)
            {
                bind(); BookType(); Restaurant();
            }
        }

        void BookType()
        {
            if (Request["HotelId"] != null)
            {
                hdHotelId.Value = Request["HotelId"].ToString();
                string sql = string.Format("SELECT ID,TypeName FROM BookType where Hotelid=" + Request["HotelId"].ToString() + " order by Sort desc ");
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ddlType.DataSource = dt;
                    this.ddlType.DataTextField = "TypeName";
                    this.ddlType.DataValueField = "ID";
                    this.ddlType.DataBind();
                    this.ddlType.Items.Insert(0, new ListItem("请选择", ""));
                }
            }
        }

        void Restaurant()
        {
            if (Request["HotelId"] != null)
            {
                hdHotelId.Value = Request["HotelId"].ToString();
                string sql = string.Format("SELECT ID,RestaurantName FROM Restaurant where HotelId=" + Request["HotelId"].ToString() + " order by AddTime desc ");
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ddlRestaurant.DataSource = dt;
                    this.ddlRestaurant.DataTextField = "RestaurantName";
                    this.ddlRestaurant.DataValueField = "ID";
                    this.ddlRestaurant.DataBind();
                    this.ddlRestaurant.Items.Insert(0, new ListItem("请选择", ""));
                }
            }
        }

        void bind()
        {

            if (Request.QueryString["ID"] != null)
            {

                string sql = string.Format(@"select * from Bookings where ID='{0}'", Request["ID"]);
                DataTable ds1 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds1.Rows.Count > 0)
                {
                   
                    hfBusinId.Value = ds1.Rows[0]["ID"].ToString();
                    if (ds1.Rows[0]["BusinessName"] != null && ds1.Rows[0]["BusinessName"].ToString() != "")
                    {
                        txtBusinessName.Value = ds1.Rows[0]["BusinessName"].ToString();
                        
                    }
                    ddlType.SelectedValue = ds1.Rows[0]["TypeId"].ToString();

                    if (ds1.Rows[0]["RestaurantId"] != null && ds1.Rows[0]["RestaurantId"].ToString() != "")
                    {
                        ddlRestaurant.SelectedValue = ds1.Rows[0]["RestaurantId"].ToString();
                    }

                    if (ds1.Rows[0]["Telephone"] != null && ds1.Rows[0]["Telephone"].ToString() != "")
                    {
                        txtTelephone.Value = ds1.Rows[0]["Telephone"].ToString();
                    }
                    if (ds1.Rows[0]["Extension"] != null && ds1.Rows[0]["Extension"].ToString() != "")
                    {
                        txtExtension.Value = ds1.Rows[0]["Extension"].ToString();
                    }
                    if (ds1.Rows[0]["BusinessTime"] != null && ds1.Rows[0]["BusinessTime"].ToString() != "")
                    {
                        txtBusinessTime.Value = ds1.Rows[0]["BusinessTime"].ToString();
                    }
                    if (ds1.Rows[0]["BusinessTime1"] != null && ds1.Rows[0]["BusinessTime1"].ToString() != "")
                    {
                        txtBusinessTime1.Value = ds1.Rows[0]["BusinessTime1"].ToString();
                    }
                    if (ds1.Rows[0]["BusinessTime2"] != null && ds1.Rows[0]["BusinessTime2"].ToString() != "")
                    {
                        txtBusinessTime2.Value = ds1.Rows[0]["BusinessTime2"].ToString();
                    } if (ds1.Rows[0]["BusinessTime3"] != null && ds1.Rows[0]["BusinessTime3"].ToString() != "")
                    {
                        txtBusinessTime3.Value = ds1.Rows[0]["BusinessTime3"].ToString();
                    }
                    if (ds1.Rows[0]["BusinessAddress"] != null && ds1.Rows[0]["BusinessAddress"].ToString() != "")
                    {
                        txtBusinessAddress.Value = ds1.Rows[0]["BusinessAddress"].ToString();
                    }
                    if (ds1.Rows[0]["Introduction"] != null && ds1.Rows[0]["Introduction"].ToString() != "")
                    {
                        txtIntroduction.Value = ds1.Rows[0]["Introduction"].ToString();
                        fckContent.Value = ds1.Rows[0]["Introduction"].ToString();
                    }
                    if (ds1.Rows[0]["Reservation"] != null && ds1.Rows[0]["Reservation"].ToString() != "")
                    {
                        _Reservation = hfReservation.Value = ds1.Rows[0]["Reservation"].ToString();
                    }
                    if (ds1.Rows[0]["OrderMeal"] != null && ds1.Rows[0]["OrderMeal"].ToString() != "")
                    {
                        _OrderMeal = hfOrderMeal.Value = ds1.Rows[0]["OrderMeal"].ToString();
                    }

                    if (ds1.Rows[0]["isopen"] != null && ds1.Rows[0]["isopen"].ToString() != "")
                    {
                        _isopen = hfisopen.Value = ds1.Rows[0]["isopen"].ToString();
                    }
                    if (ds1.Rows[0]["Recontent"] != null && ds1.Rows[0]["Recontent"].ToString() != "")
                    {
                        txtRecontent.Value = ds1.Rows[0]["Recontent"].ToString();
                    }
                    if (ds1.Rows[0]["Map"] != null && ds1.Rows[0]["Map"].ToString() != "") 
                    {
                        txtMap.Value = ds1.Rows[0]["Map"].ToString();
                    }

                    if (ds1.Rows[0]["ButtonName"] != null && ds1.Rows[0]["ButtonName"].ToString() != "") 
                    {
                        txtButtonName.Value = ds1.Rows[0]["ButtonName"].ToString();
                    }

                    if (ds1.Rows[0]["BookUrl"] != null && ds1.Rows[0]["BookUrl"].ToString() != "") 
                    {
                        txtBookUrl.Value = ds1.Rows[0]["BookUrl"].ToString();
                    }
                }

            }
        
        }
        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumit_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                Hashtable ht = new Hashtable();
                ht["HotelId"] = Request.QueryString["HotelId"];
                ht["HotelName"] = Request.QueryString["HotelName"];
                ht["BusinessName"] = txtBusinessName.Value;
                ht["TypeId"] = ddlType.SelectedValue;
                ht["TypeName"] = ddlType.SelectedItem.Text;
                if (ddlRestaurant.SelectedValue != null && ddlRestaurant.SelectedValue !="")
                {
                    ht["RestaurantId"] = ddlRestaurant.SelectedValue;
                    ht["RestaurantName"] = ddlRestaurant.SelectedItem.Text;
                }
                ht["Telephone"] = txtTelephone.Value;
                ht["Extension"] = txtExtension.Value;
                ht["BusinessTime"] = txtBusinessTime.Value;
                ht["BusinessTime1"] = txtBusinessTime1.Value;
                ht["BusinessTime2"] = txtBusinessTime2.Value;
                ht["BusinessTime3"] = txtBusinessTime3.Value;
                ht["BusinessAddress"] = txtBusinessAddress.Value;
                //ht["Introduction"] = txtIntroduction.Value;
                ht["Introduction"] = fckContent.Value;
                ht["Reservation"] = hfReservation.Value;
                ht["OrderMeal"] = hfOrderMeal.Value;
                ht["isopen"] = hfisopen.Value;
                ht["Recontent"] = txtRecontent.Value;
                ht["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
                ht["Map"] = txtMap.Value;
                ht["BookUrl"] = txtBookUrl.Value;
                ht["ButtonName"] = txtButtonName.Value;

                bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Bookings", "ID", Request["ID"], ht);
                if (IsOk)
                {
                    ShowMsgHelper.AlertReloadClose("处理成功！", "ListGrid()");


                    string[] arr = hfBusinImage.Value.ToString().Split(',');
                    int a = 0;
                    //清空关联的图片重新添加
                    string PID = Request.QueryString["ID"].ToString();
                    string sqldel = string.Format(@"delete BusinPhoto  where type='{0}' and Pid='{1}' and AdminHotelid='{2}'", (int)DefaultFilePath.SystemType.BusinPhoto, PID, RequestSession.GetSessionUser().AdminHotelid);
                    DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(sqldel));
                    for (int i = 0; i < arr.Length - 1; i++)
                    {
                        Hashtable hs = new Hashtable();
                        hs["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
                        hs["ImgFile"] = arr[i];
                        hs["Hotelid"] = hdHotelId.Value;
                        hs["PID"] = PID.ToString();
                        hs["BusinId"] = PID.ToString();
                        hs["Type"] = (int)DefaultFilePath.SystemType.BusinPhoto;
                        a = DataFactory.SqlDataBase().InsertByHashtable("BusinPhoto", hs);
                    }


                    //更新餐厅关联
                    if (ddlRestaurant.SelectedValue != null && ddlRestaurant.SelectedValue != "")
                    {
                        Hashtable hss = new Hashtable();
                        hss["BusinId"] = Request["ID"];
                        hss["BusinName"] = txtBusinessName.Value;
                        DataFactory.SqlDataBase().UpdateByHashtable("Restaurant", "ID", ddlRestaurant.SelectedValue, hss);
                    }
                }
                else
                {
                    ShowMsgHelper.AlertReloadClose("处理失败！", "ListGrid()");
            
                }
            }
            else
            {

                string sql = string.Format("select sort from Bookings order by Sort desc");
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                if (ds != null && ds.Rows.Count > 0)
                {
                    sort = Convert.ToInt32(ds.Rows[0]["sort"].ToString()) + 1;
                }
                else
                {
                    sort = 1;
                }

                Hashtable ht = GetHashtable();
                int i = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("Bookings", ht);
                if (i > 0)
                {

                    ShowMsgHelper.AlertReloadClose("添加成功！", "ListGrid()");

                    string[] arr = hfBusinImage.Value.ToString().Split(',');
                    int a = 0;
                    //清空关联的图片重新添加

                    for (int j = 0; j < arr.Length - 1; j++)
                    {
                        Hashtable hs = new Hashtable();
                        hs["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
                        hs["ImgFile"] = arr[j];
                        hs["Hotelid"] = hdHotelId.Value;
                        hs["PID"] = i.ToString();
                        hs["BusinId"] = i.ToString();
                        hs["Type"] = (int)DefaultFilePath.SystemType.BusinPhoto;
                        a = DataFactory.SqlDataBase().InsertByHashtable("BusinPhoto", hs);
                    }



                    //更新餐厅关联
                    if (ddlRestaurant.SelectedValue != null && ddlRestaurant.SelectedValue != "")
                    {
                        Hashtable hss = new Hashtable();
                        hss["BusinId"] = i;
                        hss["BusinName"] = txtBusinessName.Value;
                        DataFactory.SqlDataBase().UpdateByHashtable("Restaurant", "ID", ddlRestaurant.SelectedValue, hss);
                    }
                }
                else
                {
                    ShowMsgHelper.AlertReloadClose("添加失败！", "ListGrid()");
                }


            }

        }

        private Hashtable GetHashtable()
        {
            Hashtable ht = new Hashtable();
            ht["HotelId"] = Request.QueryString["HotelId"];
            ht["HotelName"] = Request.QueryString["HotelName"];
            ht["BusinessName"] = txtBusinessName.Value;
            ht["TypeId"] = ddlType.SelectedValue;
            ht["TypeName"] = ddlType.SelectedItem.Text;
            if (ddlRestaurant.SelectedValue != null && ddlRestaurant.SelectedValue != "")
            {
                ht["RestaurantId"] = ddlRestaurant.SelectedValue;
                ht["RestaurantName"] = ddlRestaurant.SelectedItem.Text;
            }
            ht["Telephone"] = txtTelephone.Value;
            ht["BusinessTime"] = txtBusinessTime.Value;
            ht["BusinessAddress"] = txtBusinessAddress.Value;
            ht["Introduction"] = fckContent.Value;
            ht["Reservation"] = hfReservation.Value;
            ht["OrderMeal"] = hfOrderMeal.Value;
            ht["isopen"] = hfisopen.Value;
            ht["Recontent"] = txtRecontent.Value;
            ht["Sort"] = sort;
            ht["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
            ht["Map"] = txtMap.Value;
            ht["BookUrl"] = txtBookUrl.Value;
            ht["ButtonName"] = txtButtonName.Value;
            return ht;
        }


    }
}