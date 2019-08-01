using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetCode;
using RM.Busines;
using System.Data;
using Common;
using System.Web.Script.Serialization;
using WxCallback.Api;

namespace WxCallback
{
    public partial class UserTag : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
                SELECT  AdminHotelid ,
                        Name
                FROM    dbo.Hotel_Admin
                WHERE   Name IN ( SELECT    AdminHotelName
                                  FROM      dbo.V_Wx_PlatformUser )
                ORDER BY AdminHotelid
                ");
                //SqlParam[] parm = new SqlParam[] { 
                //    new SqlParam("@HotelId", hotelId),
                //    new SqlParam("@AdminHotelid", adminHotelId)
                //};
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
                string userHtml = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string AdminHotelId = dt.Rows[i]["AdminHotelid"].ToString();
                    string Name = dt.Rows[i]["Name"].ToString();
                    int users = get_users(AdminHotelId);
                    int tag_user = tags_user(AdminHotelId);
                    string sh = "<div onclick=\"set_tag('" + AdminHotelId + "')\">";
                    sh += "<span class='AdminHotelId'>" + AdminHotelId + "</span><span class='Name'>" + Name + "</span><span class='users'>" + users + "</span><span class='tag_user'>" + tag_user + "</span>";
                    sh += "</div>";
                    userHtml += sh;
                }
                userIsTag.InnerHtml = userHtml;
            }
        }

        private int tags_user(string AdminHotelId)
        {
            int tag_user = 0;
            string accessToken = TemplateMessage.GetAccessToken(AdminHotelId);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/get?access_token={0}", accessToken.Trim());
            string menuInfo = "{\"openid_list\":[\"\"],\"tagid\":2}";
            string js_val = TemplateMessage.PostWebRequest(url, menuInfo);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            Tags ts = serializer.Deserialize<Tags>(js_val);
            if (ts != null && ts.tags != null)
            {
                if (ts.tags.Count() > 0)
                {
                    foreach (TagInfo item in ts.tags)
                    {
                        if (item.id == "2")
                        {
                            tag_user = item.count;
                        }
                    }
                }
            }
            return tag_user;
        }

        private int get_users(string AdminHotelId)
        {
            int users = 0;
            string accessToken = TemplateMessage.GetAccessToken(AdminHotelId);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}", accessToken.Trim());
            string menuInfo = "{\"openid_list\":[\"\"],\"tagid\":2}";
            string jsval = TemplateMessage.PostWebRequest(url, menuInfo); ;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            UserInfo userInfo = serializer.Deserialize<UserInfo>(jsval);
            if (userInfo != null && userInfo.count > 0)
            {
                users = userInfo.count;
            }
            return users;
        }
    }

    public class Tags
    {
        public TagInfo[] tags { get; set; }
    }

    public class TagInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public int count { get; set; }
    }
}