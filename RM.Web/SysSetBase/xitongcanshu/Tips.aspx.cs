using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL;
using System.Data;
using RM.Common.DotNetUI;
using RM.Busines;
using RM.Common.DotNetBean;
using RM.Web.App_Code;
using System.Collections;
using System.IO;
using System.Text;
using RM.Common.DotNetCode;
namespace RM.Web.SysSetBase.xitongcanshu
{
    public partial class Tips : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["AdminHotelid"] != null)
                {
                    hdAdminHotelId.Value = Request.QueryString["AdminHotelid"].ToString();
                }
                else
                {
                    hdAdminHotelId.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                }
                if (Request["HotelId"] != null)
                {
                    hdHotelId.Value = Request["HotelId"].ToString();
                }
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT *  FROM    dbo.TipsInfo  WHERE  DeleteMark=1 and  AdminHotelId= @AdminHotelId ");
                SqlParam[] param = new SqlParam[]{
                    new SqlParam("@AdminHotelId", hdAdminHotelId.Value)
                };
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (ds != null && ds.Rows.Count > 0)
                {
                    if (ds.Rows[0]["IsNetwork"] != null && ds.Rows[0]["IsNetwork"].ToString() != "")
                    {
                        hdIsNetwork.Value = ds.Rows[0]["IsNetwork"].ToString();
                    }

                    if (ds.Rows[0]["NetworkType"] != null && ds.Rows[0]["NetworkType"].ToString() != "")
                    {
                        hdNetworkType.Value = ds.Rows[0]["NetworkType"].ToString();
                    }

                    if (ds.Rows[0]["NetworkName"] != null && ds.Rows[0]["NetworkName"].ToString() != "")
                    {
                        lblNetworkName.Text = ds.Rows[0]["NetworkName"].ToString();
                    }

                    if (ds.Rows[0]["NetworkDay"] != null && ds.Rows[0]["NetworkDay"].ToString() != "")
                    {
                        hdNetworkDay.Value = ds.Rows[0]["NetworkDay"].ToString();
                    }
                    
                    if (ds.Rows[0]["NetworkImg"] != null && ds.Rows[0]["NetworkImg"].ToString() != "")
                    {
                        hdNetworkImg.Value = ds.Rows[0]["NetworkImg"].ToString();
                        iNetworkImg.Src = "/upload/TipsPhoto/" + ds.Rows[0]["NetworkImg"].ToString();
                    }

                    if (ds.Rows[0]["NetworkInfo"] != null && ds.Rows[0]["NetworkInfo"].ToString() != "")
                    {
                        pNetworkInfo.InnerHtml = ds.Rows[0]["NetworkInfo"].ToString();
                        txtNetworkInfo.Value = ds.Rows[0]["NetworkInfo"].ToString();
                    }


                    if (ds.Rows[0]["IsRoom"] != null && ds.Rows[0]["IsRoom"].ToString() != "")
                    {
                        hdIsRoom.Value = ds.Rows[0]["IsRoom"].ToString();
                    }
  

                    if (ds.Rows[0]["RoomType"] != null && ds.Rows[0]["RoomType"].ToString() != "")
                    {
                        hdRoomType.Value = ds.Rows[0]["RoomType"].ToString();
                    }

                    if (ds.Rows[0]["RoomName"] != null && ds.Rows[0]["RoomName"].ToString() != "")
                    {
                        lblRoomName.Text = ds.Rows[0]["RoomName"].ToString();
                    }

                    if (ds.Rows[0]["RoomDay"] != null && ds.Rows[0]["RoomDay"].ToString() != "")
                    {
                        hdRoomDay.Value = ds.Rows[0]["RoomDay"].ToString();
                    }


                    if (ds.Rows[0]["RoomImg"] != null && ds.Rows[0]["RoomImg"].ToString() != "")
                    {
                        hdRoomImg.Value = ds.Rows[0]["RoomImg"].ToString();
                        iRoomImg.Src = "/upload/TipsPhoto/" + ds.Rows[0]["RoomImg"].ToString();
                    }

                    if (ds.Rows[0]["RoomInfo"] != null && ds.Rows[0]["RoomInfo"].ToString() != "")
                    {
                        pRoomInfo.InnerHtml = ds.Rows[0]["RoomInfo"].ToString();
                        txtRoomInfo.Value = ds.Rows[0]["RoomInfo"].ToString();
                    }

                    if (ds.Rows[0]["IsCenter"] != null && ds.Rows[0]["IsCenter"].ToString() != "")
                    {
                        hdIsCenter.Value = ds.Rows[0]["IsCenter"].ToString();
                    }


                    if (ds.Rows[0]["CenterType"] != null && ds.Rows[0]["CenterType"].ToString() != "")
                    {
                        hdCenterType.Value = ds.Rows[0]["CenterType"].ToString();
                    }

                    if (ds.Rows[0]["CenterName"] != null && ds.Rows[0]["CenterName"].ToString() != "")
                    {
                        lblCenterName.Text = ds.Rows[0]["CenterName"].ToString();
                    }

                    if (ds.Rows[0]["CenterDay"] != null && ds.Rows[0]["CenterDay"].ToString() != "")
                    {
                        hdCenterDay.Value = ds.Rows[0]["CenterDay"].ToString();
                    }


                    if (ds.Rows[0]["CenterImg"] != null && ds.Rows[0]["CenterImg"].ToString() != "")
                    {
                        hdCenterImg.Value = ds.Rows[0]["CenterImg"].ToString();
                        iCenterImg.Src = "/upload/TipsPhoto/" + ds.Rows[0]["CenterImg"].ToString();
                    }

                    if (ds.Rows[0]["CenterInfo"] != null && ds.Rows[0]["CenterInfo"].ToString() != "")
                    {
                        pCenterInfo.InnerHtml = ds.Rows[0]["CenterInfo"].ToString();
                        txtCenterInfo.Value = ds.Rows[0]["CenterInfo"].ToString();
                    }


                    if (ds.Rows[0]["IsBook"] != null && ds.Rows[0]["IsBook"].ToString() != "")
                    {
                        hdIsBook.Value = ds.Rows[0]["IsBook"].ToString();
                    }


                    if (ds.Rows[0]["BookType"] != null && ds.Rows[0]["BookType"].ToString() != "")
                    {
                        hdBookType.Value = ds.Rows[0]["BookType"].ToString();
                    }

                    if (ds.Rows[0]["BookName"] != null && ds.Rows[0]["BookName"].ToString() != "")
                    {
                        lblBookName.Text = ds.Rows[0]["BookName"].ToString();
                    }

                    if (ds.Rows[0]["BookDay"] != null && ds.Rows[0]["BookDay"].ToString() != "")
                    {
                        hdBookDay.Value = ds.Rows[0]["BookDay"].ToString();
                    }


                    if (ds.Rows[0]["BookImg"] != null && ds.Rows[0]["BookImg"].ToString() != "")
                    {
                        hdBookImg.Value = ds.Rows[0]["BookImg"].ToString();
                        iBookImg.Src = "/upload/TipsPhoto/" + ds.Rows[0]["BookImg"].ToString();
                    }

                    if (ds.Rows[0]["BookInfo"] != null && ds.Rows[0]["BookInfo"].ToString() != "")
                    {
                        pBookInfo.InnerHtml = ds.Rows[0]["BookInfo"].ToString();
                        txtBookInfo.Value = ds.Rows[0]["BookInfo"].ToString();
                    }
               
                }
            }
        }
    }
}