using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Web.Script.Serialization;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using System.Xml;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Alipay;
using System.Collections;
using WxCallback.Api;
using RM.Common.DotNetFile;
using RM.Common.DotNetUI;
using System.IO;
using RM.Common.DotNetEncrypt;

namespace WxCallback
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string adminHotelId = "1010086";
            string hotelId = "152";
            string HotelCode = adminHotelId + "," + hotelId;
            string keyValue = DESEncrypt.Encrypt(HotelCode);
            txtHotelCode.Value = keyValue;
            
            string postUrl = "http://wx.zidinn.com/PMS/Room/GetRoomGuest";
            string val = TemplateMessage.PostWebRequest(postUrl, "{\"HotelCode\":\"" + txtHotelCode.Value + "\"}");
                        
        }
    }
}
