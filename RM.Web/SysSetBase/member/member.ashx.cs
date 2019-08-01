using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using RM.Busines;
using System.Web.SessionState;
using RM.Common.DotNetBean;
using System.Text;
using System.Data;
using RM.Common.DotNetJson;
using RM.Common.DotNetCode;

namespace RM.Web.SysSetBase.member
{
    /// <summary>
    /// member1 的摘要说明
    /// </summary>
    public class member1 : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string Action = context.Request["action"].Trim();               //提交动作

            switch (Action)
            {
                case "gethtml":
                    gethtml(context); 
                    break;
                case "gethtmls":
                    gethtmls(context);
                    break;
                case "gethtmlss":
                    gethtmlss(context);
                    break;
                case "update":
                    update(context);
                    break;
                case "updates":
                    updates(context);
                    break;
                case "getinfo":
                    getinfo(context);
                    break;
                case "getinfos":
                    getinfos(context);
                    break;
                default:
                    break;
            }

        }

        public void gethtml(HttpContext context) 
        {
            string TbHtml = "";
            #region ***** 加载表格 *****
            DataTable dt = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                
                    //头部标题
                    
                    TbHtml += "<tr>";
                    TbHtml += "<th width='40'>启用</th><th width='130'>项目</th>";
                    
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        TbHtml += "<th class='hylxname' jb='" + dt.Rows[j]["code"].ToString() + "'>";
                        TbHtml += dt.Rows[j]["LevelName"].ToString();
                        TbHtml += "</th>";
                    }
                    TbHtml += "</tr>";

                    //积分倍数

                    TbHtml+="<tr><td><i class='icon-radio6'></i></td><td>积分倍数</td>";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        TbHtml += "<td>";
                        TbHtml += "<input type='text' class='jfbs' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>倍</i>";
                        TbHtml += "</td>";
                    }
                    TbHtml += "</tr>";

                    //入住免押金
                    
                    TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>入住免押金</td>";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        TbHtml += "<td>";
                        TbHtml += "<div class='radio rzmyj'  jb='" + dt.Rows[j]["code"].ToString() + "'><input type='radio' name='w" + dt.Rows[j]["code"].ToString() + "' id='w" + dt.Rows[j]["code"].ToString() + "1' value='1' checked />";//checked
                        TbHtml += "<label for='w" + dt.Rows[j]["code"].ToString() + "1'>是</label>";
                        TbHtml += "<input type='radio' name='w" + dt.Rows[j]["code"].ToString() + "' id='w" + dt.Rows[j]["code"].ToString() + "2' value='0' /><label for='w" + dt.Rows[j]["code"].ToString() + "2'>否</label>";
                        TbHtml += "</div>";
                        TbHtml += "</td>";
                    }
                    TbHtml += "</tr>";

                    //电话预订保留时间

                    TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>电话预订保留时间</td>";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        TbHtml += "<td>";
                        TbHtml += "<select class='dhyjblsj' jb='" + dt.Rows[j]["code"].ToString() + "'>";
                        TbHtml += "<option value='17:00'>17:00</option>";
                        TbHtml += "<option value='18:00'>18:00</option>";
                        TbHtml += "<option value='19:00'>19:00</option>";
                        TbHtml += "<option value='20:00'>20:00</option>";
                        TbHtml += "<option value='21:00'>21:00</option>";
                        TbHtml += "<option value='22:00'>22:00</option>";
                        TbHtml += "<option value='23:00'>23:00</option>";
                        TbHtml += "<option value='00:00'>00:00</option>";
                        TbHtml += "</select>";
                        TbHtml += "</td>";
                    }
                    TbHtml += "</tr>";

                    //微网预订保留时间

                    TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>微网预订保留时间</td>";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        TbHtml += "<td>";
                        TbHtml += "<select class='wwyjblsj' jb='" + dt.Rows[j]["code"].ToString() + "'>";
                        TbHtml += "<option value='17:00'>17:00</option>";
                        TbHtml += "<option value='18:00'>18:00</option>";
                        TbHtml += "<option value='19:00'>19:00</option>";
                        TbHtml += "<option value='20:00'>20:00</option>";
                        TbHtml += "<option value='21:00'>21:00</option>";
                        TbHtml += "<option value='22:00'>22:00</option>";
                        TbHtml += "<option value='23:00'>23:00</option>";
                        TbHtml += "<option value='00:00'>00:00</option>";
                        TbHtml += "</select>";
                        TbHtml += "</td>";
                    }
                    TbHtml += "</tr>";

                    //支付预订保留时间

                    TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>支付预订保留时间</td>";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        TbHtml += "<td>";
                        TbHtml += "<select class='zfyjblsj' jb='" + dt.Rows[j]["code"].ToString() + "'>";
                        TbHtml += "<option value='17:00'>17:00</option>";
                        TbHtml += "<option value='18:00'>18:00</option>";
                        TbHtml += "<option value='19:00'>19:00</option>";
                        TbHtml += "<option value='20:00'>20:00</option>";
                        TbHtml += "<option value='21:00'>21:00</option>";
                        TbHtml += "<option value='22:00'>22:00</option>";
                        TbHtml += "<option value='23:00'>23:00</option>";
                        TbHtml += "<option value='00:00'>00:00</option>";
                        TbHtml += "</select>";
                        TbHtml += "</td>";
                    }
                    TbHtml += "</tr>";

                    //延时退房时间

                    TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>延时退房时间</td>";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        TbHtml += "<td>";
                        TbHtml += "<select class='ystfsj' jb='" + dt.Rows[j]["code"].ToString() + "'>";
                        TbHtml += "<option value='17:00'>17:00</option>";
                        TbHtml += "<option value='18:00'>18:00</option>";
                        TbHtml += "<option value='19:00'>19:00</option>";
                        TbHtml += "<option value='20:00'>20:00</option>";
                        TbHtml += "<option value='21:00'>21:00</option>";
                        TbHtml += "<option value='22:00'>22:00</option>";
                        TbHtml += "<option value='23:00'>23:00</option>";
                        TbHtml += "<option value='00:00'>00:00</option>";
                        TbHtml += "</select>";
                        TbHtml += "</td>";
                    }
                    TbHtml += "</tr>";

                
            }
            #endregion


            context.Response.Write(TbHtml);
        }


        public void gethtmls(HttpContext context)
        {
            string TbHtml = "";
            #region ***** 加载表格 *****
            DataTable dt = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {

                //头部标题

                TbHtml += "<tr>";
                TbHtml += "<th width='40'>启用</th><th width='130'>方式</th>";

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    
                        TbHtml += "<th class='hylxnames' jb='" + dt.Rows[j]["code"].ToString() + "'>";
                        TbHtml += dt.Rows[j]["LevelName"].ToString();
                        TbHtml += "</th>";
                    
                }
                TbHtml += "</tr>";

                //累计积分升级

                TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>累计积分升级</td>";
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    
                        TbHtml += "<td>";
                        if (j == 0)
                        {
                            TbHtml += "<input type='text' disabled='disabled' value='0' class='jfsjgy' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>分</i>";
                        }
                        else 
                        {
                            TbHtml += "<input type='text' class='jfsjgy' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>分</i>";
                        }
                        
                        TbHtml += "</td>";

                }
                TbHtml += "</tr>";



                //累计消费升级高于

                TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>累计消费升级</td>";
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    
                        TbHtml += "<td>";
                        if (j == 0)
                        {
                            TbHtml += "<input type='text' disabled='disabled' value='0' class='xfsjgy' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>元</i>";
                        }
                        else 
                        {
                            TbHtml += "<input type='text'  class='xfsjgy' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>元</i>";
                        }
                        TbHtml += "</td>";
                    
                }
                TbHtml += "</tr>";
                //单次消费升级高于

                TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>单次消费升级</td>";
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    
                        TbHtml += "<td>";
                        if (j == 0)
                        {
                            TbHtml += "<input type='text' disabled='disabled' value='0' class='dcxfsjgy' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>元</i>";
                        }
                        else 
                        {
                            TbHtml += "<input type='text' class='dcxfsjgy' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>元</i>";
                        }
                        
                        TbHtml += "</td>";
                    
                }
                TbHtml += "</tr>";

                //充值升级高于

                TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>累计充值升级</td>";
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    
                        TbHtml += "<td>";
                        if (j == 0)
                        {
                            TbHtml += "<input type='text' disabled='disabled' value='0' class='czsjgy' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>元</i>";
                        }
                        else 
                        {
                            TbHtml += "<input type='text' class='czsjgy' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>元</i>";
                        }
                        
                        TbHtml += "</td>";
                    
                }
                TbHtml += "</tr>";

                //单次充值升级高于

                TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>单次充值升级</td>";
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    
                    TbHtml += "<td>";
                    if (j == 0)
                    {
                        TbHtml += "<input type='text' disabled='disabled' value='0' class='dcczsjgy' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>元</i>";
                    }
                    else 
                    {
                        TbHtml += "<input type='text' class='dcczsjgy' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>元</i>";
                    }
                    
                    TbHtml += "</td>";
                   
                }
                TbHtml += "</tr>";
                
                //购买升级高于

                TbHtml += "<tr><td><i class='icon-radio6'></i></td><td>购买升级</td>";
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    
                        TbHtml += "<td>";
                        TbHtml += "<input type='text' class='gmsjgy' jb='" + dt.Rows[j]["code"].ToString() + "' /><i class='dw'>元</i>";
                        
                        TbHtml += "</td>";
                    
                }
//                TbHtml += "<td>";
//                TbHtml += @"<div class='radio' id='gnlx'>
//                                        <label   class='checked'>原价购买</label>
//                                        <label  >差价购买</label>
//                                    </div>";
//                TbHtml += "</td>";
                TbHtml += "</tr>";


            }
            #endregion


            context.Response.Write(TbHtml);
        }

        public void gethtmlss(HttpContext context)
        {
            string TbHtml = "";
            #region ***** 加载表格 *****
            DataTable dt = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {

                //头部标题

               //<div class="sjxs clearfix" style=" display:"><span>微会员购买升级形式/说明</span><input type="text" name="name" value="" /></div>

                for (int i = 0; i < dt.Rows.Count; i++) 
                {
                    TbHtml += "<div class='sjxs clearfix' style=' display:'><span>" + dt.Rows[i]["LevelName"].ToString() + "购买升级形式/说明</span><input type='text' jb='" + dt.Rows[i]["code"].ToString() + "' class='sm' name='name'  /></div>";
                }


            }
            #endregion


            context.Response.Write(TbHtml);
        }


        public void update(HttpContext context) 
        {
            //&values=5,1,0,17:00,17:00,17:00,17:00|B,2,0,18:00,18:00,18:00,18:00|A,3,1,20:00,20:00,20:00,20:00&jfbs=1&rzmyj=1&dhyjblsj=1&wwyjblsj=0&zfyjblsj=0&ystfsj=0

            StringBuilder sql = new StringBuilder();
            sql.Append("delete Set_Privilege WHERE AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid + "' ");
            DataFactory.SqlDataBase().ExecuteBySql(sql);
            string value = context.Request["values"].Trim();
            string jfbs = context.Request["jfbs"].Trim();
            string rzmyj = context.Request["rzmyj"].Trim();
            string dhyjblsj = context.Request["dhyjblsj"].Trim();
            string wwyjblsj = context.Request["wwyjblsj"].Trim();
            string zfyjblsj = context.Request["zfyjblsj"].Trim();
            string ystfsj = context.Request["ystfsj"].Trim();
            
            string[] values = value.Split('|');
            int x = 0; 
            for (int i = 0; i < values.Length; i++) 
            {
                Hashtable ht = new Hashtable();
                ht["jb"] = values[i].Split(',')[0];
                ht["jfbs"] = values[i].Split(',')[1];
                ht["rzmyj"] = values[i].Split(',')[2];
                ht["dhyjblsj"] = values[i].Split(',')[3];
                ht["wwyjblsj"] = values[i].Split(',')[4];
                ht["zfyjblsj"] = values[i].Split(',')[5];
                ht["ystfsj"] = values[i].Split(',')[6];
                ht["isjfbs"] = jfbs;
                ht["isrzmyj"] = rzmyj;
                ht["isdhyjblsj"] = dhyjblsj;
                ht["iswwyjblsj"] = wwyjblsj;
                ht["iszfyjblsj"] = zfyjblsj;
                ht["isystfsj"] = ystfsj;
                
                ht["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();


                //string AdminHotelids = "";
                //string sql = string.Format(@"select id from jfmatter where AdminHotelid=@AdminHotelid");
                //SqlParam[] parmAdd = new SqlParam[] { 
                //                     new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())};
                //DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    AdminHotelids = RequestSession.GetSessionUser().AdminHotelid.ToString();
                //}

                //bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("jfmatter", "AdminHotelid", AdminHotelids, ht);

                x = DataFactory.SqlDataBase().InsertByHashtable("Set_Privilege", ht);

                

            }

            context.Response.Write(x);
        }




        public void updates(HttpContext context)
        {
            //&values=5,1,0,17:00,17:00,17:00,17:00|B,2,0,18:00,18:00,18:00,18:00|A,3,1,20:00,20:00,20:00,20:00&jfbs=1&rzmyj=1&dhyjblsj=1&wwyjblsj=0&zfyjblsj=0&ystfsj=0
            StringBuilder sql = new StringBuilder();
            sql.Append("delete Set_Upgrade WHERE AdminHotelid='" + RequestSession.GetSessionUser().AdminHotelid + "' ");
            DataFactory.SqlDataBase().ExecuteBySql(sql);
            string value = context.Request["values"].Trim();
            string jfsjgy = context.Request["jfsjgy"].Trim();
            //string jfsjdy = context.Request["jfsjdy"].Trim();
            string xfsjgy = context.Request["xfsjgy"].Trim();
            string czsjgy = context.Request["czsjgy"].Trim();
            string dcxfsjgy = context.Request["dcxfsjgy"].Trim();
            string dcczsjgy = context.Request["dcczsjgy"].Trim();
            string gmsjgy = context.Request["gmsjgy"].Trim();
            string gmlx = context.Request["gmlx"].Trim();
            int x = 0; 
            string[] values = value.Split('|');
            for (int i = 0; i < values.Length; i++)
            {
                Hashtable ht = new Hashtable();
                ht["jb"] = values[i].Split(',')[0];
                ht["jfsjgy"] = values[i].Split(',')[1];
                //ht["jfsjdy"] = values[i].Split(',')[2];
                ht["xfsjgy"] = values[i].Split(',')[2];
                ht["dcxfsjgy"] = values[i].Split(',')[3];
                ht["czsjgy"] = values[i].Split(',')[4];
                ht["dcczsjgy"] = values[i].Split(',')[5];
                ht["gmsjgy"] = values[i].Split(',')[6];
                ht["sm"] = values[i].Split(',')[7];
                

                ht["isjfsjgy"] = jfsjgy;
                //ht["isjfsjdy"] = jfsjdy;
                ht["isxfsjgy"] = xfsjgy;
                ht["isczsjgy"] = czsjgy;
                ht["isdcxfsjgy"] = dcxfsjgy;
                ht["isdcczsjgy"] = dcczsjgy;
                ht["isgmsjgy"] = gmsjgy;
                ht["gmlx"] = gmlx;
                ht["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();

                x += DataFactory.SqlDataBase().InsertByHashtable("Set_Upgrade", ht);

                string sqls = string.Format(@"Update hy_hylxbmb Set zk=@zk Where hylxcode=@hylxcode");
                SqlParam[] parmAdd2 = new SqlParam[] { 
                                    new SqlParam("@zk",ht["gmsjgy"]),
                                     new SqlParam("@hylxcode",ht["jb"]) };
                DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).ExecuteBySql(new StringBuilder(sqls), parmAdd2);

                if (i == 0) 
                {
                    int gmzc = 0;
                    if (gmsjgy == "1")
                    {
                        if (values[i].Split(',')[6].Trim() != "0")
                        {
                            gmzc = 1;
                        }
                    }
                    
                    StringBuilder sqlup = new StringBuilder();
                    sqlup.AppendFormat("UPDATE Hotel_Admin SET RegCharge={0} ,RegMoney={1} WHERE AdminHotelid='{2}' ", gmzc.ToString(), values[i].Split(',')[6], RequestSession.GetSessionUser().AdminHotelid.ToString());
                    DataFactory.SqlDataBase().ExecuteBySql(sqlup);
                }

            }

            context.Response.Write(x);
        }

        public void getinfo(HttpContext context)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * FROM Set_Privilege WHERE AdminHotelid=@AdminHotelid ");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, parmAdd);

            string dtName = "Set_Privilege";
            string json = JsonHelper.DataTableToJson(dt, dtName);
            context.Response.Write(json);
        }

        public void getinfos(HttpContext context) 
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * FROM Set_Upgrade WHERE AdminHotelid=@AdminHotelid ");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",RequestSession.GetSessionUser().AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, parmAdd);

            string dtName = "Set_Upgrade";
            string json = JsonHelper.DataTableToJson(dt, dtName);
            context.Response.Write(json);
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