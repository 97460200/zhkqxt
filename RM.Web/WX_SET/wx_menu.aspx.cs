using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.business;
using System.Text;
using System.Net;
using System.IO;
using RM.Common.DotNetConfig;
using LitJson;

namespace RM.Web.WX_SET
{
    public partial class wx_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreateMenu_Click(object sender, EventArgs e)
        {
            string AdminHotelid = "";
            if (Request["AdminHotelid"] != null)
            {
                AdminHotelid = Request["AdminHotelid"];
            }
            string postUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken(AdminHotelid));
            string menuInfo = getMenuInfo(AdminHotelid);
            lblResult.Text = "结果：" + PostWebRequest(postUrl, menuInfo);           
        }

        private string PostWebRequest(string postUrl, string menuInfo)
        {
            string returnValue = string.Empty;
            try
            {
                byte[] byteData = Encoding.UTF8.GetBytes(menuInfo);
                Uri uri = new Uri(postUrl);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteData.Length;
                //定义Stream信息
                Stream stream = webReq.GetRequestStream();
                stream.Write(byteData, 0, byteData.Length);
                stream.Close();
                //获取返回信息
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                returnValue = streamReader.ReadToEnd();
                //关闭信息
                streamReader.Close();
                response.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.ToString();
            }
            return returnValue;
        }

        private string getMenuInfo(string id)
        {
            string url = ConfigHelper.GetAppSettings("Url");

            string str = "";
            if (id == "1")
            {
                //智订云商户平台
                str = "{";
                str += "\"button\":[{\"type\":\"view\",\"name\":\"我的奖金\",\"url\":\"" + url + "/Melt/Sale_bonus.aspx\"}],";
                str += "\"button\":[{\"type\":\"view\",\"name\":\"房态设置\",\"url\":\"" + url + "/Melt/SetRoomState.aspx\"}],";
                str += "\"button\":[{\"type\":\"view\",\"name\":\"推广码\",\"url\":\"" + url + "/Melt/Sale_code.aspx\"}]";
                str += "}";

                //str = "{";
                //str += "\"button\":[{\"type\":\"view\",\"name\":\"领取奖励\",\"url\":\"" + url + "/Melt/Sale_bonus.aspx\"}],";

                //str += "\"button\":[{\"name\":\"营销推广\",\"sub_button\":[";
                //str += "{\"type\":\"view\",\"name\":\"我的推广码\",\"url\":\"" + url + "/Melt/Sale_code.aspx\"},";
                //str += "{\"type\":\"view\",\"name\":\"营销说明\",\"url\":\"" + url + "/Melt/Sale_expl.aspx\"}";
                //str += "]}]";
                //str += "}";
            }

            else if (id != "" && !string.IsNullOrEmpty(Request.QueryString["mr"]))
            {
                //默认
                str = "{\"button\":[{\"type\":\"view\",\"name\":\"酒店预订\",\"url\":\"" + url + "/Reservation/HotelList.aspx?AdminHotelid=" + id + "\"}],\"button\":[{\"name\":\"会员尊享\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"会员中心\",\"url\":\"" + url + "/Members/MemberCantre.aspx?AdminHotelid=" + id + "\"},";
                str += "{\"type\":\"view\",\"name\":\"会员码\",\"url\":\"" + url + "/Vipcard/payment.aspx?AdminHotelid=" + id + "\"},";
                str += "{\"type\":\"view\",\"name\":\"我要付款\",\"url\":\"" + url + "/Marketing/pay/PayCode.aspx?AdminHotelid=" + id + "\"},";
                str += "{\"type\":\"view\",\"name\":\"充值有礼\",\"url\":\"" + url + "/Vipcard/MemCart.aspx?AdminHotelid=" + id + "\"}";
                str += "]}],";

                str += "\"button\":[{\"name\":\"更多服务\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"滴滴出行\",\"url\":\"http://common.diditaxi.com.cn/general/webEntry\"}";
                str += "]}]";
                str += "}";
            }
            else if (id == "1006203")
            {
                //湖北宾馆
                str = "{\"button\":[{\"type\":\"view\",\"name\":\"一键订房\",\"url\":\"" + url + "/Reservation/HotelDetails.aspx?AdminHotelid=1006203&hotelid=44\"}],\"button\":[{\"name\":\"会员尊享\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"会员中心\",\"url\":\"" + url + "/Members/MemberCantre.aspx?AdminHotelid=1006203\"},";
                str += "{\"type\":\"view\",\"name\":\"会员码\",\"url\":\"" + url + "/Vipcard/payment.aspx?AdminHotelid=1006203\"},";
                str += "{\"type\":\"view\",\"name\":\"我要付款\",\"url\":\"" + url + "/Marketing/pay/PayCode.aspx?AdminHotelid=1006203\"},";
                str += "{\"type\":\"view\",\"name\":\"充值有礼\",\"url\":\"" + url + "/Vipcard/MemCart.aspx?AdminHotelid=1006203\"}";
                str += "]}],";

                str += "\"button\":[{\"name\":\"更多服务\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"滴滴出行\",\"url\":\"http://common.diditaxi.com.cn/general/webEntry\"}";
                str += "]}]";
                str += "}";
            }
            else if (id == "1006690")
            {
                //铜陵天玑酒店
                str = "{\"button\":[{\"type\":\"view\",\"name\":\"一键订房\",\"url\":\"" + url + "/Reservation/HotelDetails.aspx?AdminHotelid=1006690&hotelid=49\"}],\"button\":[{\"name\":\"会员尊享\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"会员中心\",\"url\":\"" + url + "/Members/MemberCantre.aspx?AdminHotelid=1006690\"},";
                str += "{\"type\":\"view\",\"name\":\"会员码\",\"url\":\"" + url + "/Vipcard/payment.aspx?AdminHotelid=1006690\"},";
                str += "{\"type\":\"view\",\"name\":\"我要付款\",\"url\":\"" + url + "/Marketing/pay/PayCode.aspx?AdminHotelid=1006690\"},";
                str += "{\"type\":\"view\",\"name\":\"充值有礼\",\"url\":\"" + url + "/Vipcard/MemCart.aspx?AdminHotelid=1006690\"}";
                str += "]}],";

                str += "\"button\":[{\"name\":\"更多服务\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"滴滴出行\",\"url\":\"http://common.diditaxi.com.cn/general/webEntry\"}";
                str += "]}]";
                str += "}";
            }
            else if (id == "1001507")
            {
                //嘉禧酒店
                str = "{\"button\":[{\"name\":\"走进嘉禧\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"嘉禧首页\",\"url\":\"" + url + "/fwindex.aspx?AdminHotelid=" + id + "\"},";
                str += "{\"type\":\"view\",\"name\":\"酒店预订\",\"url\":\"" + url + "/Reservation/HotelList.aspx?AdminHotelid=" + id + "\"}";
                str += "]}],";

                str += "\"button\":[{\"name\":\"会员尊享\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"会员中心\",\"url\":\"" + url + "/Members/MemberCantre.aspx?AdminHotelid=" + id + "\"},";
                str += "{\"type\":\"view\",\"name\":\"会员码\",\"url\":\"" + url + "/Vipcard/payment.aspx?AdminHotelid=" + id + "\"},";
                str += "{\"type\":\"view\",\"name\":\"我要付款\",\"url\":\"" + url + "/Marketing/pay/PayCode.aspx?AdminHotelid=" + id + "\"}";
                str += "]}]";
                str += "}";
            }
            else if (id == "1004040")
            {
                //喜格酒店
                str = "{\"button\":[{\"type\":\"view\",\"name\":\"一键订房\",\"url\":\"" + url + "/Reservation/HotelDetails.aspx?AdminHotelid=1004040&hotelid=50\"}],\"button\":[{\"name\":\"会员尊享\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"会员中心\",\"url\":\"" + url + "/Members/MemberCantre.aspx?AdminHotelid=1004040\"},";
                str += "{\"type\":\"view\",\"name\":\"会员码\",\"url\":\"" + url + "/Vipcard/payment.aspx?AdminHotelid=1004040\"},";
                str += "{\"type\":\"view\",\"name\":\"我要付款\",\"url\":\"" + url + "/Marketing/pay/PayCode.aspx?AdminHotelid=1004040\"}";
                //str += "{\"type\":\"view\",\"name\":\"充值有礼\",\"url\":\"" + url + "/Vipcard/MemCart.aspx?AdminHotelid=1004040\"}";
                str += "]}],";

                str += "\"button\":[{\"name\":\"更多服务\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"滴滴出行\",\"url\":\"http://common.diditaxi.com.cn/general/webEntry\"}";
                str += "]}]";
                str += "}";
            }
            else if (id == "SEWA004160")
            {
                str = "{\"button\":[{\"name\":\"走进银濠\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"银濠首页\",\"url\":\"" + url + "/indes.aspx?AdminHotelid=" + id + "\"},";
                str += "{\"type\":\"view\",\"name\":\"客房预订\",\"url\":\"" + url + "/Reservation/HotelDetails.aspx?AdminHotelid=" + id + "\"},";
                str += "{\"type\":\"view\",\"name\":\"餐饮预订\",\"url\":\"" + url + "/PageText/wx_dytw.aspx?AdminHotelid=" + id + "&urlname=银濠酒楼\"},";
                str += "{\"type\":\"view\",\"name\":\"会议预订\",\"url\":\"" + url + "/PageText/wx_dytw.aspx?AdminHotelid=" + id + "&urlname=银濠会场\"}";
                str += "]}],";

                str += "\"button\":[{\"name\":\"会员尊享\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"会员中心\",\"url\":\"" + url + "/Members/MemberCantre.aspx?AdminHotelid=" + id + "\"},";
                str += "{\"type\":\"view\",\"name\":\"会员码\",\"url\":\"" + url + "/Marketing/pay/MemCode.aspx?AdminHotelid=" + id + "\"},";
                str += "{\"type\":\"view\",\"name\":\"我要付款\",\"url\":\"" + url + "/Marketing/pay/PayCode.aspx?AdminHotelid=" + id + "\"}";
                str += "]}]";
                str += "}";


            }
            else if (id == "1003920")
            {
                //皇朝酒店
                str = "{\"button\":[{\"type\":\"view\",\"name\":\"一键订房\",\"url\":\"" + url + "/Reservation/HotelList.aspx?AdminHotelid=1003920\"}],\"button\":[{\"name\":\"会员尊享\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"会员中心\",\"url\":\"" + url + "/Members/MemberCantre.aspx?AdminHotelid=1003920\"},";
                str += "{\"type\":\"view\",\"name\":\"会员码\",\"url\":\"" + url + "/Vipcard/payment.aspx?AdminHotelid=1003920\"},";
                str += "{\"type\":\"view\",\"name\":\"我要付款\",\"url\":\"" + url + "/Marketing/pay/PayCode.aspx?AdminHotelid=1003920\"},";
                str += "{\"type\":\"view\",\"name\":\"充值有礼\",\"url\":\"" + url + "/Vipcard/MemCart.aspx?AdminHotelid=1003920\"}";
                str += "]}],";

                str += "\"button\":[{\"name\":\"更多服务\",\"sub_button\":[";
                str += "{\"type\":\"view\",\"name\":\"滴滴出行\",\"url\":\"http://common.diditaxi.com.cn/general/webEntry\"}";
                str += "]}]";
                str += "}";
            }
            return str;

        }

        protected void btnDeleteMenu_Click(object sender, EventArgs e)
        {
            string AdminHotelid = "";
            if (Request["AdminHotelid"] != null)
            {
                AdminHotelid = Request["AdminHotelid"];
            }
            string postUrl = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";
            postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken(AdminHotelid));
            string menuInfo = getMenuInfo(AdminHotelid);
            lblResult.Text = "结果：" + PostWebRequest(postUrl, menuInfo);
        }
    }
}