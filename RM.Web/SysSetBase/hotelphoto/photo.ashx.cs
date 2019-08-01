using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using RM.Common.DotNetCode;
using System.Text;
using RM.Busines;
using System.Data;
using LitJson;
using System.Collections;
using RM.Common.DotNetBean;
using RM.Web.WX_SET;
using RM.Web.App_Code;

namespace RM.Web.SysSetBase.hotelphoto
{
    /// <summary>
    /// photo 的摘要说明
    /// </summary>
    public class photo : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Menu = context.Request["Menu"];                      //提交动作
            switch (Menu)
            {

                case "GetHotelPicList":  //获取酒店图片
                    GetHotelPicList(context);
                    break;
                case "GetRoomPicList":  //获取房型图片
                    GetRoomPicList(context);
                    break;
                case "GetBusinessPicList":  //获取营业点图片
                    GetBusinessPicList(context);
                    break;
                case "GetMallPicList":  //获取微商城图片
                    GetMallPicList(context);
                    break;
                case "AddPhoto":  //获取营业点图片
                    AddPhoto(context);
                    break;
                default:
                    break;
            }
        }



        /// <summary>
        ///获取酒店图片
        /// </summary>
        /// <param name="context"></param>
        private void GetHotelPicList(HttpContext context)
        {
            string HotelId = context.Request["HotelId"];
            string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();
            JsonData jsondata = new JsonData();
            string sql = string.Format(@"select ID, ImgFile from Photo where PID='{0}' and [type]='{1}' and AdminHotelid='{2}' order by ID asc", HotelId, (int)DefaultFilePath.SystemType.HotelType, AdminHotelid);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            if (ds != null && ds.Rows.Count > 0)
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    JsonData json1 = new JsonData();
                    json1["ID"] = ds.Rows[i]["ID"].ToString();
                    json1["IMGFILE"] = ds.Rows[i]["ImgFile"].ToString();
                    jsondata.Add(json1);
                }
            }
            string json = "";
            json = jsondata.ToJson();
            context.Response.Write(json);
        }

        /// <summary>
        ///获取酒店微商城图片
        /// </summary>
        /// <param name="context"></param>
        private void GetMallPicList(HttpContext context)
        {
            string HotelId = context.Request["HotelId"];
            string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();
            JsonData jsondata = new JsonData();
            string sql = string.Format(@"select ID, ImgFile from Photo where PID='{0}' and [type]='{1}' and AdminHotelid='{2}' order by ID asc", HotelId, (int)DefaultFilePath.SystemType.Mall, AdminHotelid);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            if (ds != null && ds.Rows.Count > 0)
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    JsonData json1 = new JsonData();
                    json1["ID"] = ds.Rows[i]["ID"].ToString();
                    json1["IMGFILE"] = ds.Rows[i]["ImgFile"].ToString();
                    jsondata.Add(json1);
                }
            }
            string json = "";
            json = jsondata.ToJson();
            context.Response.Write(json);
        }

        /// <summary>
        ///获取房型图片
        /// </summary>
        /// <param name="context"></param>
        private void GetRoomPicList(HttpContext context)
        {
            string HotelId = context.Request["HotelId"];
            JsonData jsondata = new JsonData();
            string sql = string.Format(@"select ID,Name,AdminHotelid from Guestroom where HotelID=@HotelId  order by sort desc");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@HotelId", HotelId)};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (ds != null && ds.Rows.Count > 0)
            {

     
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    JsonData json1 = new JsonData();
                    json1["ID"] = ds.Rows[i]["ID"].ToString();
                    json1["NAME"] = ds.Rows[i]["Name"].ToString();
                    sql = string.Format(@"select ID, ImgFile from Photo where PID='{0}' and [type]='{1}' and AdminHotelid='{2}' order by ID asc", ds.Rows[i]["id"], (int)DefaultFilePath.SystemType.MinBiaoType, ds.Rows[i]["AdminHotelid"]);
                    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        json1["pic"] = new JsonData();
                        for (int s = 0; s < dt.Rows.Count; s++)
                        {
                            JsonData json2 = new JsonData();
                            json2["ID"] = dt.Rows[s]["ID"].ToString();
                            json2["IMGFILE"] = dt.Rows[s]["ImgFile"].ToString();
                            json1["pic"].Add(json2);
                        }
                    }
                    jsondata.Add(json1);
                }
            }
            string json = "";
            json = jsondata.ToJson();
            context.Response.Write(json);
        }


        /// <summary>
        ///获取营业点图片
        /// </summary>
        /// <param name="context"></param>
        private void GetBusinessPicList(HttpContext context)
        {
            string HotelId = context.Request["HotelId"];
            JsonData jsondata = new JsonData();
            string sql = string.Format(@"select ID,TypeName,BusinessName,AdminHotelid from Bookings where HotelId=@HotelId  order by sort desc");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@HotelId", HotelId)};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (ds != null && ds.Rows.Count > 0)
            {


                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    JsonData json1 = new JsonData();
                    json1["ID"] = ds.Rows[i]["ID"].ToString();
                    json1["TYPENAME"] = ds.Rows[i]["TypeName"].ToString();
                    json1["BUSINESSNAME"] = ds.Rows[i]["BusinessName"].ToString();
                    sql = string.Format(@"select ID, ImgFile from Photo where PID='{0}' and [type]='{1}' and AdminHotelid='{2}' order by ID asc", ds.Rows[i]["id"], (int)DefaultFilePath.SystemType.Business, ds.Rows[i]["AdminHotelid"]);
                    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        json1["pic"] = new JsonData();
                        for (int s = 0; s < dt.Rows.Count; s++)
                        {
                            JsonData json2 = new JsonData();
                            json2["ID"] = dt.Rows[s]["ID"].ToString();
                            json2["IMGFILE"] = dt.Rows[s]["ImgFile"].ToString();
                            json1["pic"].Add(json2);
                        }
                    }
                    jsondata.Add(json1);
                }
            }
            string json = "";
            json = jsondata.ToJson();
            context.Response.Write(json);
        }

        public void AddPhoto(HttpContext context)
        {
            string hfImage = context.Request["hfImage"].Trim();
            string hdHotelId = context.Request["hdHotelId"].Trim();

            //先全部分割，读取每个分割值的第一位和第二位，然后查询分割值第一位和第二位符合条件语句数据，进行删除，再循环分割第三位值，进行插入表数据

            //添加多张图片
            string[] arr = hfImage.ToString().Split('|');
            int a = 0;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                string[] arrs = arr[i].Split('!');//将PID，TYPE!ImgFile分割
                string[] arres = arrs[0].Split(',');//PID，TYPE
                string[] ImgFile = arrs[1].Split(',');//ImgFile
                string PID = arres[0].ToString();//PID
                string TYPE = arres[1].ToString();//TYPE

                //清空关联的图片重新添加
                if (PID != "" && TYPE != "")
                {
                    string sqldel = string.Format(@"delete photo  where type='{0}' and Pid='{1}' and AdminHotelid='{2}'", TYPE, PID, RequestSession.GetSessionUser().AdminHotelid);
                    DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(sqldel));
                }

                Hashtable hss = new Hashtable();
                for (int y = 0; y < ImgFile.Length - 1; y++)
                {
                    hss["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
                    hss["ImgFile"] = ImgFile[y];
                    hss["Hotelid"] = hdHotelId.ToString();
                    hss["PID"] = PID.ToString();
                    hss["Type"] = TYPE.ToString();
                    a = DataFactory.SqlDataBase().InsertByHashtable("Photo", hss);
                }

            }
            if (a > 0)
            {
                context.Response.Write("ok");
            }
            else
            { 
                context.Response.Write("error");
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}