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
using System.Collections;
using RM.Common.DotNetUI;
using RM.Common.DotNetCode;
using RM.Web.business;
using System.Drawing;
using RM.Common.DotNetFile;
using System.IO;

namespace RM.Web.SysSetBase.pay
{
    public partial class pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdAdminHotelId.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("SELECT ID,name,pay,Qtpay,Hypay,jfpay,JFZhoumo,JFJieri,yhqzhoumo,yhqjieri  FROM  dbo.Hotel WHERE  DeleteMark=1  and  AdminHotelid='{0}' order by sort desc", RequestSession.GetSessionUser().AdminHotelid);
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
                this.DDLHotelList.DataSource = dt;
                this.DDLHotelList.DataValueField = "ID";
                this.DDLHotelList.DataTextField = "name";
                this.DDLHotelList.DataBind();

                if (CommonClass.CheckFunctionIsOpen(hdAdminHotelId.Value, "Wx_Deposit"))//押金功能 是否开启
                {
                    dDeposit.Style.Add("display", "");
                }
                Bind();
            }
        }

        private void Bind()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM dbo.Hotel_AdminParameter WHERE AdminHotelId = @AdminHotelId");
            SqlParam[] param = new SqlParam[] {
                new SqlParam("@AdminHotelId",hdAdminHotelId.Value)
            };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdPledgeMoneyEnable.Value = dt.Rows[0]["PledgeMoneyEnable"].ToString();//是否启用默认押金
                hdPledgeMoneyRoom.Value = dt.Rows[0]["PledgeMoneyRoom"].ToString();//是否启用房型押金

                CashPledgeMoney.Value = dt.Rows[0]["CashPledgeMoney"].ToString();//押金金额
                hdCashMoneyReturnType.Value = dt.Rows[0]["CashMoneyReturnType"].ToString();//押金退回方式
                hdCashMoneyEdit.Value = dt.Rows[0]["CashMoneyEdit"].ToString();//客人是否可修改押金

                if (dt.Rows[0]["PledgeMoneyCode"] != null && dt.Rows[0]["PledgeMoneyCode"].ToString() != "")
                {
                    imgPledgeMoneyCode.Src = dt.Rows[0]["PledgeMoneyCode"].ToString();
                }
                else
                {
                    string img_name = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + TemplateMessage.Sen_Hotelimg("PledgeMoney@" + hdAdminHotelId.Value, hdAdminHotelId.Value, ""); //生成永久二维码      
                    imgPledgeMoneyCode.Src = img_name;
                    Hashtable ht = new Hashtable();
                    ht["PledgeMoneyCode"] = img_name;
                    DataFactory.SqlDataBase().UpdateByHashtable("Hotel_AdminParameter", "AdminHotelId", hdAdminHotelId.Value, ht);
                }

                if (dt.Rows[0]["CardMoneyCode"] != null && dt.Rows[0]["CardMoneyCode"].ToString() != "")
                {
                    kjzf.Src = dt.Rows[0]["CardMoneyCode"].ToString();
                }
                else
                {
                    string img_name = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + TemplateMessage.Sen_Hotelimg("CardMoney@" + hdAdminHotelId.Value, hdAdminHotelId.Value, ""); //生成永久二维码      
                    kjzf.Src = img_name;
                    Hashtable ht = new Hashtable();
                    ht["CardMoneyCode"] = img_name;
                    DataFactory.SqlDataBase().UpdateByHashtable("Hotel_AdminParameter", "AdminHotelId", hdAdminHotelId.Value, ht);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Hashtable htParameter = new Hashtable();
            htParameter["CashPledgeMoney"] = CashPledgeMoney.Value;
            htParameter["CashMoneyReturnType"] = hdCashMoneyReturnType.Value;
            htParameter["CashMoneyEdit"] = hdCashMoneyEdit.Value;

            htParameter["PledgeMoneyEnable"] = hdPledgeMoneyEnable.Value;
            htParameter["PledgeMoneyRoom"] = hdPledgeMoneyRoom.Value;
            DataFactory.SqlDataBase().Submit_AddOrEdit("Hotel_AdminParameter", "AdminHotelId", hdAdminHotelId.Value, htParameter);
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            string url = Request.Url.ToString();
            url = url.Substring(0, url.LastIndexOf("/"));
            // <param name="Url">网页地址</param>
            //<param name="BrowserWidth">页面宽度</param>
            // <param name="BrowserHeight">页面高度</param>
            // <param name="ThumbnailWidth">图像宽度</param>
            //<param name="ThumbnailHeight">图像高度</param>
            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(url + "/PledgeMoneyCode.aspx?AdminHotelid=" + hdAdminHotelId.Value, 540, 540, 540, 540); //宽高根据要获取快照的网页决定
            string PhotoName = "押金二维码.jpg";
            string filename = "~/Upload/QRcode/" + PhotoName;
            string fpath = HttpContext.Current.Server.MapPath(filename);
            m_Bitmap.Save(fpath, System.Drawing.Imaging.ImageFormat.Jpeg); //图片格式可以自由控制
            DownloadFile(Server.UrlEncode(PhotoName), Server.MapPath(filename));
        }


        protected void btnDownloads_Click(object sender, EventArgs e)
        {
            string url = Request.Url.ToString();
            url = url.Substring(0, url.LastIndexOf("/"));
            // <param name="Url">网页地址</param>
            //<param name="BrowserWidth">页面宽度</param>
            // <param name="BrowserHeight">页面高度</param>
            // <param name="ThumbnailWidth">图像宽度</param>
            //<param name="ThumbnailHeight">图像高度</param>
            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(url + "/CardMoneyCode.aspx?AdminHotelid=" + hdAdminHotelId.Value, 540, 540, 540, 540); //宽高根据要获取快照的网页决定
            string PhotoName = "快捷支付二维码.jpg";
            string filename = "~/Upload/QRcode/" + PhotoName;
            string fpath = HttpContext.Current.Server.MapPath(filename);
            m_Bitmap.Save(fpath, System.Drawing.Imaging.ImageFormat.Jpeg); //图片格式可以自由控制
            DownloadFile(Server.UrlEncode(PhotoName), Server.MapPath(filename));
        }

        private void DownloadFile(string fileName, string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                File.Delete(filePath);//删除已下载文件
                return;
            }
            catch
            {

            }
        }

    }
}