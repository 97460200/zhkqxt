using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.business;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using RM.Web.Lib;
using RM.Common.DotNetUI;
using System.Collections;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.IO;
using RM.Common.DotNetBean;

namespace RM.Web.SysSetBase.memInfo
{
    public partial class TicketPreview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["MemberId"]))
                {
                    string AdminHotelid = hdAdminHotelid.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                    string MemberId = hdMemberId.Value = Request.QueryString["MemberId"];
                    string RechargeId = Request.QueryString["RechargeId"];

                    Bind(AdminHotelid, MemberId);//加载用户信息

                    //判断当前用户是否存在打印表
                    string newTime = DateTime.Now.ToString(); //当前时间
                    spPrintTime.InnerHtml = Convert.ToDateTime(newTime).ToString("yyyy-mm-dd HH:mm");//打印时间

                    string Code_Url = "http://www.zidinn.com/Melt/Card_Pay.aspx?AdminHotelid=" + AdminHotelid + "&MemberId=" + MemberId;
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"
                    SELECT  *  FROM    ReceiptInfo 
                    WHERE   AdminHotelid =@AdminHotelid and MemberId=@MemberId
                    ");
                    SqlParam[] param = new SqlParam[] {   
                                                new SqlParam("@AdminHotelid", AdminHotelid),  
                                                new SqlParam("@MemberId", MemberId)};
                    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        hdId.Value = dt.Rows[0]["ID"].ToString();
                        string Img_path = dt.Rows[0]["CodeImg"].ToString();
                        if (dt.Rows[0]["Effective"].ToString() == "0")
                        {
                            //判断打印时间是否超过两小时
                            string PrintTime = Convert.ToDateTime(dt.Rows[0]["PrintTime"]).AddHours(2).ToString();//开始时间
                            if (Convert.ToDateTime(PrintTime) > Convert.ToDateTime(newTime))//有效时间
                            {
                                try
                                {
                                    //判断文件的存在
                                    if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("..") + "\\..\\SysSetBase\\memInfo\\PayImg\\" + Img_path))
                                    {
                                        erweima.Src = "~/SysSetBase/memInfo/PayImg/" + Img_path;
                                    }
                                }
                                catch { }
                            }
                            else //超过时间(重新生成码)
                            {
                                try
                                {
                                    //判断文件的存在
                                    if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("..") + "\\..\\SysSetBase\\memInfo\\PayImg\\" + Img_path))
                                    {
                                        FileInfo file;
                                        file = new FileInfo(Server.MapPath("..") + "\\..\\SysSetBase\\memInfo\\PayImg\\" + Img_path);
                                        file.Delete();
                                    }
                                }
                                catch { }

                                Bitmap bt = new Bitmap(500, 500);
                                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                                qrCodeEncoder.QRCodeScale = 4;
                                qrCodeEncoder.QRCodeVersion = 8;
                                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                                String data = Code_Url;
                                System.Drawing.Bitmap image = qrCodeEncoder.Encode(data, Encoding.UTF8);
                                System.IO.MemoryStream MStream = new System.IO.MemoryStream();
                                string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                                image.Save(Server.MapPath("~/SysSetBase/memInfo/PayImg/") + filename);
                                image.Save(MStream, System.Drawing.Imaging.ImageFormat.Gif);
                                erweima.Src = "~/SysSetBase/memInfo/PayImg/" + filename;

                                //更新图片信息
                                Hashtable hs = new Hashtable();
                                hs["PrintTime"] = newTime; //打印时间;
                                hs["CodeImg"] = filename;
                                DataFactory.SqlDataBase().UpdateByHashtable("ReceiptInfo", "ID", dt.Rows[0]["ID"].ToString(), hs);

                            }
                        }
                        else
                        {
                            try
                            {
                                //判断文件的存在
                                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("..") + "\\..\\SysSetBase\\memInfo\\PayImg\\" + Img_path))
                                {
                                    erweima.Src = "~/SysSetBase/memInfo/PayImg/" + Img_path;
                                }
                            }
                            catch { }
                        }
                    }
                    else
                    {

                        Hashtable hs = new Hashtable();
                        hs["MemberId"] = hdMemberId.Value;
                        hs["CardNumber"] = spCardNumber.InnerHtml;
                        hs["MemberLevel"] = spMemberLevel.InnerHtml;
                        hs["Name"] = spName.InnerHtml;
                        hs["Phone"] = spPhone.InnerHtml;
                        hs["PrintTime"] = newTime; //打印时间;
                        //hs["TopUpMoney"] = spTopUpMoney.InnerHtml;
                        //hs["Consumption"] = spConsumption.InnerHtml;
                        hs["Remaining"] = spRemaining.InnerHtml;
                        hs["RulesContent"] = "";
                        hs["Adminhotelid"] = hdAdminHotelid.Value;

                        Bitmap bt = new Bitmap(500, 500);
                        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                        qrCodeEncoder.QRCodeScale = 4;
                        qrCodeEncoder.QRCodeVersion = 8;
                        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                        String data = Code_Url;
                        System.Drawing.Bitmap image = qrCodeEncoder.Encode(data, Encoding.UTF8);
                        System.IO.MemoryStream MStream = new System.IO.MemoryStream();
                        string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                        image.Save(Server.MapPath("~/SysSetBase/memInfo/PayImg/") + filename);
                        image.Save(MStream, System.Drawing.Imaging.ImageFormat.Gif);
                        erweima.Src = "~/SysSetBase/memInfo/PayImg/" + filename;

                        hs["CodeImg"] = filename;
                        int Rid = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("ReceiptInfo", hs);
                        if (Rid > 0)
                        {
                            hdId.Value = Rid.ToString();
                        }
                    }
                }

            }
        }

        private void Bind(string AdminHotelid, string MemberId)
        {
       
            //查询会员卡余额
            StringBuilder sbhy = new StringBuilder();
            sbhy.Append(@"
            SELECT  hy.lsh ,
                    hy.kh ,
                    hy.xm ,
                    hy.sjhm ,
                    mb.hylxname ,
                    CONVERT(VARCHAR(100), hy.addtime, 23) AS addtime ,
                    ISNULL(hy.hykye, 0) AS hykye
            FROM    hy_hyzlxxb hy ,
                    hy_hylxbmb mb
            WHERE   hy.hylx = mb.hylxcode and hy.lsh=@MemberId
            ");
            SqlParam[] parmhy = new SqlParam[] {                  
                                     new SqlParam("@MemberId", MemberId)};
            DataTable ds = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sbhy, parmhy);
            if (ds != null && ds.Rows.Count > 0)
            {
                if (ds.Rows[0]["kh"] != null && ds.Rows[0]["kh"].ToString() != "")
                {
                    spCardNumber.InnerHtml = ds.Rows[0]["kh"].ToString();
                }

                if (ds.Rows[0]["hylxname"] != null && ds.Rows[0]["hylxname"].ToString() != "")
                {
                    spMemberLevel.InnerHtml = ds.Rows[0]["hylxname"].ToString();
                }

                if (ds.Rows[0]["xm"] != null && ds.Rows[0]["xm"].ToString() != "")
                {
                    spName.InnerHtml = ds.Rows[0]["xm"].ToString();
                }

                if (ds.Rows[0]["sjhm"] != null && ds.Rows[0]["sjhm"].ToString() != "")
                {
                    spPhone.InnerHtml = ds.Rows[0]["sjhm"].ToString();
                }
                spRemaining.InnerHtml = Convert.ToDouble(ds.Rows[0]["hykye"]).ToString();
            }

            //加载酒店信息
            StringBuilder sb_hotel = new StringBuilder();
            sb_hotel.Append(@"SELECT Name  FROM Hotel_Admin WHERE AdminHotelid=@AdminHotelid and DeleteMark=1 ");
            SqlParam[] parm_hotel = new SqlParam[] {                  
                                     new SqlParam("@AdminHotelid", AdminHotelid)};
            DataTable dt_hotel = DataFactory.SqlDataBase().GetDataTableBySQL(sb_hotel, parm_hotel);
            if (dt_hotel != null && dt_hotel.Rows.Count > 0)
            {
                if (dt_hotel.Rows[0]["Name"] != null && dt_hotel.Rows[0]["Name"].ToString() != "")
                {
                    spHotelName.InnerHtml = dt_hotel.Rows[0]["Name"].ToString();
                }
            }

            //加载规则信息
            StringBuilder sb_rule = new StringBuilder();
            sb_rule.Append(@"SELECT RulesContent  FROM ReceiptRules WHERE AdminHotelid=@AdminHotelid ");
            SqlParam[] parm_rule = new SqlParam[] {                  
                                     new SqlParam("@AdminHotelid", AdminHotelid)};
            DataTable dt_rule = DataFactory.SqlDataBase().GetDataTableBySQL(sb_rule, parm_rule);
            if (dt_rule != null && dt_rule.Rows.Count > 0)
            {
                if (dt_rule.Rows[0]["RulesContent"] != null && dt_rule.Rows[0]["RulesContent"].ToString() != "")
                {
                    RulesContent.InnerHtml = dt_rule.Rows[0]["RulesContent"].ToString();
                }
            }
        }


    }
}