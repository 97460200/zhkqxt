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
using RM.Common.DotNetJson;
using Newtonsoft.Json;
using RM.Common.DotNetData;

namespace RM.Web.SysSetBase.Ajax
{
    /// <summary>
    /// SysAjax 的摘要说明
    /// </summary>
    public class SysAjax : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Menu = context.Request["Menu"];                      //提交动作
            switch (Menu)
            {
                case "GetHotelList":  //获取酒店、客房信息列表
                    GetHotelList(context);
                    break;
                case "GetKFJGList":  //获取客房价格
                    GetKFJGList(context);
                    break;
                case "GetMemberType":  //获取客房参数
                    GetMemberType(context);
                    break;
                case "Save": //保存客房价格
                    Save(context);
                    break;
                case "Saverili": //保存前台预订多月
                    Saverili(context);
                    break;
                case "rililist": //显示日历价格
                    rililist(context);
                    break;
                case "Gethotel": //获取酒店列表
                    GetHotel(context);
                    break;
                case "GetroomList": //获取客房列表
                    GetroomList(context);
                    break;
                case "save_disFJ": //保存内部分销客房佣金
                    save_disFJ(context);
                    break;
                case "save_setfx": //保存功能设置
                    save_setfx(context);
                    break;
                case "Show_setfx": //展示分销功能设置
                    Show_setfx(context);
                    break;
                case "savehouseRule": //保存房价规则
                    savehouseRule(context);
                    break;
                case "houseRuleList": //房型房态列表
                    houseRuleList(context);
                    break;

                case "SetRulePrice": //设置房型规则的价格
                    SetRulePrice(context);
                    break;

                case "DefaultRoomPrice":  //默认房型价格设置
                    DefaultRoomPrice(context);
                    break;


                case "SetRoomState": //批量设置房态
                    SetRoomState(context);
                    break;

                case "GetMonthPriceTable": //加载 月 展示表格
                    GetMonthPriceTable(context);
                    break;
                case "GetRule": //售房规则
                    GetRule(context);
                    break;
                case "RoomState": //设置房间状态
                    RoomState(context);
                    break;
                case "GetWeekPriceTable": //加载 周 展示表格
                    GetWeekPriceTable(context);
                    break;
                case "WeekDate": //上下周
                    WeekDate(context);
                    break;
                ///////////////////////编辑///////////////////////
                case "GetEditRule": //售房规则
                    GetEditRule(context);
                    break;
                case "GetEditPrice": //获取编辑数据
                    GetEditPrice(context);
                    break;
                case "SaveRoomPrice": //保存
                    SaveRoomPrice(context);
                    break;
                ///////////////////////编辑end///////////////////////
                case "getRoom": //首页销售间夜
                    getRoom(context);
                    break;
                case "getUsersBonus": //首页业绩评估
                    getUsersBonus(context);
                    break;
                case "getdata": //首页数据统计
                    getdata(context);
                    break;
                default:
                    break;
            }
        }

        #region 客房价格设置



        /// <summary>
        /// 获取酒店、客房信息列表
        /// </summary>
        /// <param name="context"></param>
        private void GetHotelList(HttpContext context)
        {
            string adminhotelid = context.Request["adminhotelid"];
            JsonData jsondata = new JsonData();
            string sql = string.Format(@"select id,name from hotel where adminhotelid=@adminhotelid and DeleteMark=1 order by sort desc");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid)};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (ds != null && ds.Rows.Count > 0)
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    JsonData json1 = new JsonData();
                    json1["ID"] = ds.Rows[i]["id"].ToString();
                    json1["NAME"] = ds.Rows[i]["name"].ToString();
                    sql = string.Format(@"select id,name from Guestroom where hotelid=@hotelid order by sort desc");
                    SqlParam[] pmadd = new SqlParam[] { 
                                     new SqlParam("@hotelid", ds.Rows[i]["id"])};
                    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), pmadd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        json1["kf"] = new JsonData();
                        for (int s = 0; s < dt.Rows.Count; s++)
                        {
                            JsonData json2 = new JsonData();
                            json2["id"] = dt.Rows[s]["id"].ToString();
                            json2["name"] = dt.Rows[s]["name"].ToString();
                            json1["kf"].Add(json2);
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
        /// 获取客房价格列表
        /// </summary>
        /// <param name="context"></param>
        private void GetKFJGList(HttpContext context)
        {
            string gsid = context.Request["gsid"];
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT  'jg' + ltrim(rtrim(hydj)) + CAST([type] AS VARCHAR) + ','
                    + CAST(jg AS VARCHAR) jg ,
                    'jf' + ltrim(rtrim(hydj)) + CAST([type] AS VARCHAR) + ','
                    + CAST(jf AS VARCHAR) jf ,
                    'fjs' + ltrim(rtrim(hydj)) + CAST([type] AS VARCHAR) + ','
                    + CAST(fjs AS VARCHAR) fjs ,
                    zaocan ,
                    jf_zaocan
            FROM    GSMoeny
            WHERE   gsid = @gsid");
            SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@gsid", gsid)};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            context.Response.Write(ToJson(ds));
        }

        /// <summary>
        /// 获取客房价格列表
        /// </summary>
        /// <param name="context"></param>
        private void GetMemberType(HttpContext context)
        {
            string adminhotelid = context.Request["adminhotelid"]; //酒店参数ID

            string TbHtml = "";
            string zcHtml = "";
            zcHtml = @" <select class='zaocan'>
                                    <option value='无早'>无早</option >
                                    <option value='单早'>单早</option>
                                    <option value='双早'>双早</option>
                          </select>";
            #region ***** 加载表格 *****
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT replace(hylxcode, ' ', '') as jb,hylxname as LevelName FROM hy_hylxbmb where AdminHotelid='{0}' order by sort desc",adminhotelid);
            DataTable dt = DataFactory.SqlDataBase(adminhotelid).GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                sql = new StringBuilder();
                sql.Append("SELECT name,unit,code,Abbreviation FROM MemberType");
                DataTable dtType = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
                int textWidth = 400 / dt.Rows.Count;
                if (dtType != null && dtType.Rows.Count > 0)
                {
                    //头部标题
                    TbHtml += "<table>";
                    TbHtml += " <thead>";

                    TbHtml += "<tr>";
                    TbHtml += "<th class=\"fw_hy\" width='120'>会员等级";
                    TbHtml += "</th>";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        TbHtml += "<th class=\"\">";
                        TbHtml += dt.Rows[j]["LevelName"].ToString();
                        TbHtml += "</th>";
                    }
                    TbHtml += "</tr>";
                    TbHtml += "</thead>";
                    TbHtml += "<tbody>";
                    //类型列
                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        TbHtml += "<tr>";
                        TbHtml += "<td class=\"line\">";
                        TbHtml += dtType.Rows[i]["Name"].ToString();
                        TbHtml += "</td>";
                        string lx_code = dtType.Rows[i]["Code"].ToString();//类型 编号
                        string lx_sx = dtType.Rows[i]["Abbreviation"].ToString();//类型 缩写
                        for (int j = 0; j < dt.Rows.Count; j++)//循环等级
                        {
                            string jb = dt.Rows[j]["jb"].ToString();//会员级别
                            TbHtml += "<td>";
                            TbHtml += string.Format("<input type=\"text\" maxlength=\"5\" hydj='{0}' MemberType='{1}' id=\"{2}{0}{1}\" name=\"{2}{0}{1}\" autocomplete='off' /> ", jb, lx_code, lx_sx);
                            TbHtml += "<i class='dw'>" + dtType.Rows[i]["Unit"].ToString() + "</i>";
                            if (lx_sx != "fjs")
                            {
                                TbHtml += zcHtml;
                            }
                            TbHtml += "</td>";
                        }
                        TbHtml += "</tr>";
                    }
                    TbHtml += "</tbody>";
                    TbHtml += "</table>";
                }
            }
            #endregion
            context.Response.Write(TbHtml);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="context"></param>
        private void Save(HttpContext context)
        {
            string hid = context.Request["hid"];
            string gsid = context.Request["gsid"];
            string vals = context.Request["vals"];

            string adminhotelid = context.Request["adminhotelid"]; //酒店参数ID

            if (vals != "")
            {
                string sqls = string.Format(@"delete from GSMoeny where  gsid='{0}'", gsid);
                DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(sqls));
                string[] sArray = vals.Split('|');
                for (int i = 0; i < sArray.Length; i++)
                {
                    string[] s = sArray[i].ToString().Split(',');
                    if (s.Length < 5)
                    {
                        continue;
                    }
                    string sql = string.Format(@"insert into GSMoeny(hydj,type,jg,jf,fjs,hotelID,GSID,AdminHotelid)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                        s[0].Trim(), s[1], s[2], s[3], s[4], hid, gsid, adminhotelid);
                    DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(sql));
                }

                CommonMethod.Base_Log("修改", "GSMoeny", hid, "房价管理", "修改默认客房价格");//操作日志
                context.Response.Write("编辑成功！");
            }
            else
            {
                context.Response.Write("编辑失败！");
            }

        }

        /// <summary>
        /// 保存预订多月
        /// </summary>
        /// <param name="context"></param>
        private void Saverili(HttpContext context)
        {
            string adminhotelid = context.Request["adminhotelid"]; //酒店参数ID
            string rili = context.Request["rili"];

            string sql = string.Format(@"update moday set rili=@rili where AdminHotelid=@AdminHotelid");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid)};
            DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(sql), parmAdd);
            context.Response.Write("");
        }

        /// <summary>
        /// 显示日历
        /// </summary>
        /// <param name="context"></param>
        private void rililist(HttpContext context)
        {
            string adminhotelid = context.Request["adminhotelid"]; //酒店参数ID
            string year = context.Request["year"]; //年
            string month = context.Request["Month"]; //月
            string hydj = context.Request["hydj"]; //会员等级
            string gsid = context.Request["gsid"]; //客房ID
            string hotelid = context.Request["hotelid"]; //酒店ID

            string dayhtml = "";

            DateTime dt = DateTime.Parse(year + "-" + month + "-01");       //本月第一天
            StringBuilder sb = new StringBuilder();

            int day = dt.AddMonths(1).AddDays(-1).Day;      //本月总天数

            DateTime dts = DateTime.Parse(year + "-" + month + "-" + day);       //本月最后一天

            int week = WXJSAPI.getWeek(dt);
            sb.Append("<table><tr>");
            string s = "";
            string time;
            string tdclas = "";
            string kfmf = "";//客房满房
            //输出上月中的天数
            if (week != 7)
            {
                DateTime upyear = dt.AddDays(-1);       //上个月最后一天
                int upMonth = upyear.Day;           //上月总天数

                for (int i = 0; i < week; i++)
                {
                    int d = upMonth - week + 1;
                    time = DateTime.Parse(upyear.Year + "-" + upyear.Month + "-" + d).ToString();
                    sb.Append("<td><b class='date'>" + d + "</b></td>");
                    upMonth++;
                }
            }

            #region ***** 加载表格 *****
            StringBuilder sql = new StringBuilder();

            sql.Append("select jg,fjs,jf,zaocan,jf_zaocan from GSMoeny where GSID=@GSID and hydj=@hydj");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@GSID",gsid),
                new SqlParam("@hydj", hydj) };
            DataTable dsss = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);

            string sqlrl = string.Format(@"select id from Holiday where StartTime>='{0}' and EndTime>='{1}' and AdminHotelid='{2}' ", dt, dts, adminhotelid);
            DataTable dsrq = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlrl));

            int jr = 0;
            if (dsrq != null && dsrq.Rows.Count > 0)
            {
                jr = 1;
            }

            string sqlzdy = string.Format(@"select id from HousePrice where CurentDate>='{0}' and CurentDate<='{1}' and rid='{2}' and hydj='{3}'", dt, dts, gsid, hydj);


            DataTable dszdy = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlzdy));
            int zdy = 0;
            string zdyclass = ""; //自定义价格
            if (dszdy != null && dszdy.Rows.Count > 0)
            {
                zdy = 1;
            }

            string sql123 = string.Format(@"select id from Full_house where StartTime>='{0}' or EndTime>'{1}' and Gsid='{2}'", dt.ToString("yyyy-MM-dd"), dts.ToString("yyyy-MM-dd"), gsid);
            DataTable dsmf = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql123));
            int mf = 0;
            if (dsmf != null && dsmf.Rows.Count > 0)
            {
                mf = 1;
            }

            Hashtable info = new Hashtable();
            info["jg_breakfast"] = ""; //价格早餐
            info["jf_breakfast"] = ""; //积分早餐
            #region ***** 输出本月中的天数 *****
            for (int j = 1; j <= day; j++)
            {
                if ((j + week - 1) % 7 == 0)
                {
                    sb.Append("</tr><tr>");
                }
                else
                {
                    tdclas = "";
                }

                time = DateTime.Parse(year + "-" + month + "-" + j).ToString("yyyy-MM-dd");
                //下拉框
                //根据客服ID查询价格

                s = "";
                int s1 = 2;
                string pricename = "";
                info["jg_breakfast"] = ""; //价格早餐
                info["jf_breakfast"] = ""; //积分早餐
                #region ***** 自定义价格 *****
                if (zdy == 1)
                {
                    string sqlzdys = string.Format(@"select PriceName,Number,Price,Integral,jg_breakfast,jf_breakfast from HousePrice where CurentDate='{0}' and RId='{1}' and hydj='{2}' ", time, gsid, hydj);
                    DataTable hpin = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlzdys));
                    if (hpin != null && hpin.Rows.Count > 0)
                    {
                        s1 = Convert.ToInt32(hpin.Rows[0]["PriceName"]);
                        info["fjs"] = hpin.Rows[0]["Number"];
                        info["jg"] = hpin.Rows[0]["Price"];
                        info["jf"] = hpin.Rows[0]["Integral"];


                        if (hpin.Rows[0]["jg_breakfast"].ToString() == "单早")
                        {
                            info["jg_breakfast"] = "<span class='dz'>单早</span>";
                        }
                        else if (hpin.Rows[0]["jg_breakfast"].ToString() == "双早")
                        {
                            info["jg_breakfast"] = "<span class='sz'>双早</span>";
                        }
                        else
                        {
                            info["jg_breakfast"] = "";
                        }

                        if (hpin.Rows[0]["jf_breakfast"].ToString() == "单早")
                        {
                            info["jf_breakfast"] = "<span class='dz'>单早</span>";
                        }
                        else if (hpin.Rows[0]["jf_breakfast"].ToString() == "双早")
                        {
                            info["jf_breakfast"] = "<span class='sz'>双早</span>";
                        }
                        else
                        {
                            info["jf_breakfast"] = "";
                        }

                        zdyclass = "star";
                    }
                    else
                    {
                        zdyclass = "";
                        try
                        {
                            if (WXJSAPI.getzhoumo(time, adminhotelid) == 1)
                            {
                                s1 = 3;
                                info["fjs"] = Convert.ToInt32(dsss.Rows[2]["fjs"]);
                                info["jg"] = Convert.ToInt32(dsss.Rows[2]["jg"]);
                                info["jf"] = Convert.ToInt32(dsss.Rows[2]["jf"]);

                                if (dsss.Rows[2]["zaocan"].ToString() == "单早")
                                {
                                    info["jg_breakfast"] = "<span class='dz'>单早</span>";
                                }
                                else if (dsss.Rows[2]["zaocan"].ToString() == "双早")
                                {
                                    info["jg_breakfast"] = "<span class='sz'>双早</span>";
                                }

                                if (dsss.Rows[2]["jf_zaocan"].ToString() == "单早")
                                {
                                    info["jf_breakfast"] = "<span class='dz'>单早</span>";
                                }
                                else if (dsss.Rows[2]["jf_zaocan"].ToString() == "双早")
                                {
                                    info["jf_breakfast"] = "<span class='sz'>双早</span>";
                                }


                            }
                            else
                            {
                                info["fjs"] = Convert.ToInt32(dsss.Rows[1]["fjs"]);
                                info["jg"] = Convert.ToInt32(dsss.Rows[1]["jg"]);
                                info["jf"] = Convert.ToInt32(dsss.Rows[1]["jf"]);

                                if (dsss.Rows[1]["zaocan"].ToString() == "单早")
                                {
                                    info["jg_breakfast"] = "<span class='dz'>单早</span>";
                                }
                                else if (dsss.Rows[1]["zaocan"].ToString() == "双早")
                                {
                                    info["jg_breakfast"] = "<span class='sz'>双早</span>";
                                }

                                if (dsss.Rows[1]["jf_zaocan"].ToString() == "单早")
                                {
                                    info["jf_breakfast"] = "<span class='dz'>单早</span>";
                                }
                                else if (dsss.Rows[1]["jf_zaocan"].ToString() == "双早")
                                {
                                    info["jf_breakfast"] = "<span class='sz'>双早</span>";
                                }

                                s1 = 2;
                            }

                            if (jr == 1)
                            {
                                string sqls = string.Format(@"select id from Holiday where StartTime<='{0}' and EndTime>'{0}'", time);
                                DataTable dss = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls));
                                if (dss.Rows.Count > 0)
                                {
                                    s1 = 4;
                                    info["fjs"] = Convert.ToInt32(dsss.Rows[3]["fjs"]);
                                    info["jg"] = Convert.ToInt32(dsss.Rows[3]["jg"]);
                                    info["jf"] = Convert.ToInt32(dsss.Rows[3]["jf"]);

                                    if (dsss.Rows[3]["zaocan"].ToString() == "单早")
                                    {
                                        info["jg_breakfast"] = "<span class='dz'>单早</span>";
                                    }
                                    else if (dsss.Rows[3]["zaocan"].ToString() == "双早")
                                    {
                                        info["jg_breakfast"] = "<span class='sz'>双早</span>";
                                    }

                                    if (dsss.Rows[3]["jf_zaocan"].ToString() == "单早")
                                    {
                                        info["jf_breakfast"] = "<span class='dz'>单早</span>";
                                    }
                                    else if (dsss.Rows[3]["jf_zaocan"].ToString() == "双早")
                                    {
                                        info["jf_breakfast"] = "<span class='sz'>双早</span>";
                                    }

                                }
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    try
                    {

                        if (WXJSAPI.getzhoumo(time, adminhotelid) == 1)
                        {
                            s1 = 3;
                            info["fjs"] = Convert.ToInt32(dsss.Rows[2]["fjs"]);
                            info["jg"] = Convert.ToInt32(dsss.Rows[2]["jg"]);
                            info["jf"] = Convert.ToInt32(dsss.Rows[2]["jf"]);

                            if (dsss.Rows[2]["zaocan"].ToString() == "单早")
                            {
                                info["jg_breakfast"] = "<span class='dz'>单早</span>";
                            }
                            else if (dsss.Rows[2]["zaocan"].ToString() == "双早")
                            {
                                info["jg_breakfast"] = "<span class='sz'>双早</span>";
                            }

                            if (dsss.Rows[2]["jf_zaocan"].ToString() == "单早")
                            {
                                info["jf_breakfast"] = "<span class='dz'>单早</span>";
                            }
                            else if (dsss.Rows[2]["jf_zaocan"].ToString() == "双早")
                            {
                                info["jf_breakfast"] = "<span class='sz'>双早</span>";
                            }

                        }
                        else
                        {
                            info["fjs"] = Convert.ToInt32(dsss.Rows[1]["fjs"]);
                            info["jg"] = Convert.ToInt32(dsss.Rows[1]["jg"]);
                            info["jf"] = Convert.ToInt32(dsss.Rows[1]["jf"]);

                            if (dsss.Rows[1]["zaocan"].ToString() == "单早")
                            {
                                info["jg_breakfast"] = "<span class='dz'>单早</span>";
                            }
                            else if (dsss.Rows[1]["zaocan"].ToString() == "双早")
                            {
                                info["jg_breakfast"] = "<span class='sz'>双早</span>";
                            }

                            if (dsss.Rows[1]["jf_zaocan"].ToString() == "单早")
                            {
                                info["jf_breakfast"] = "<span class='dz'>单早</span>";
                            }
                            else if (dsss.Rows[1]["jf_zaocan"].ToString() == "双早")
                            {
                                info["jf_breakfast"] = "<span class='sz'>双早</span>";
                            }

                            s1 = 2;
                        }

                        if (jr == 1)
                        {
                            string sqls = string.Format(@"select id from Holiday where StartTime<='{0}' and EndTime>'{0}'", time);
                            DataTable dss = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqls));
                            if (dss != null && dss.Rows.Count > 0)
                            {
                                s1 = 4;
                                info["fjs"] = Convert.ToInt32(dsss.Rows[3]["fjs"]);
                                info["jg"] = Convert.ToInt32(dsss.Rows[3]["jg"]);
                                info["jf"] = Convert.ToInt32(dsss.Rows[3]["jf"]);

                                if (dsss.Rows[3]["zaocan"].ToString() == "单早")
                                {
                                    info["jg_breakfast"] = "<span class='dz'>单早</span>";
                                }
                                else if (dsss.Rows[3]["zaocan"].ToString() == "双早")
                                {
                                    info["jg_breakfast"] = "<span class='sz'>双早</span>";
                                }

                                if (dsss.Rows[3]["jf_zaocan"].ToString() == "单早")
                                {
                                    info["jf_breakfast"] = "<span class='dz'>单早</span>";
                                }
                                else if (dsss.Rows[3]["jf_zaocan"].ToString() == "双早")
                                {
                                    info["jf_breakfast"] = "<span class='sz'>双早</span>";
                                }

                            }
                        }
                    }
                    catch { }
                }
                #endregion

                int i = 0;
                if (mf == 1)
                {
                    ///满房设置
                    string sql12 = string.Format(@"select id from Full_house where StartTime<='{0}' and EndTime>'{0}' and Gsid='{1}'", time, gsid);  //已经设置满房
                    DataTable ds21 = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql12));
                    if (ds21 != null && ds21.Rows.Count > 0)
                    {
                        i = 1000000;
                        kfmf = "kgClose";
                    }
                    else
                    {
                        kfmf = "";
                    }
                }

                #region ***** 输出价格 *****
                //客服设置价格
                if (info != null)
                {
                    if (s1 == 2)
                    {
                        pricename = "平日价";
                    }
                    else if (s1 == 3)
                    {
                        pricename = "周末价";
                    }
                    else if (s1 == 4)
                    {
                        pricename = "节日价";
                    }

                    if (Convert.ToInt32(info["fjs"]) > 0 && Convert.ToInt32(info["fjs"]) > i)
                    {
                        s += string.Format(@"<div class='currInfo'>
                                <span><small class='{4}'>{0}</small><small>{1}元</small>{5}</span>
                                <span><small>需积分</small><small>{2}分</small>{6}</span>
                                <span><small>可订房</small><small>{3}间</small></span></div>
                            <i class='icon-kg1 {7}'></i>", pricename, info["jg"], info["jf"], info["fjs"], zdyclass, info["jg_breakfast"], info["jf_breakfast"], kfmf);
                    }
                    else
                    {
                        s += string.Format(@"<div class='currInfo'>
                                <span><small class='{3}'>{0}</small><small>{1}元</small>{4}</span>
                                <span><small>需积分</small><small>{2}分</small>{5}</span>
                                <span><small>可订房</small><small>满房</small></span></div>
                             <i class='icon-kg1 {6}'></i>", pricename, info["jg"], info["jf"], zdyclass, info["jg_breakfast"], info["jf_breakfast"], kfmf);
                    }
                }

                string a = "";
                if (i == 1000000)
                {
                    a = "<a val='0' onclick='UpDateFull_house(this)' class='close' ></a>";
                }
                else
                {
                    a = "<a val='1' onclick='UpDateFull_house(this)' ></a>";
                }
                if (DateTime.Parse(time) > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    sb.AppendFormat(@"<td class='calendar' gsid='{2}' day='{3}' ><b class='date'>{0}</b>{1}</td>", j, s, gsid, time);
                }
                else if (DateTime.Parse(time) == DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    sb.AppendFormat(@"<td class='calendar today' gsid='{2}' day='{3}'><b class='date'>{0}</b>{1}</td>", j, s, gsid, time);
                }
                else
                {
                    sb.AppendFormat(@"<td class='calendar guo' gsid='{2}' day='{3}'><b class='date'>{0}</b>{1}</td>", j, s, gsid, time);
                }

                #endregion
            }
            sb.Append("</table>");
            #endregion
            #endregion

            dayhtml = sb.ToString();

            context.Response.Write(dayhtml);
        }

        #endregion

        #region 分销功能设置

        /// <summary>
        /// 获取酒店信息列表
        /// </summary>
        /// <param name="context"></param>
        private void GetHotel(HttpContext context)
        {
            string adminhotelid = context.Request["adminhotelid"];
            JsonData jsondata = new JsonData();
            string sql = string.Format(@"select id,name from hotel where adminhotelid=@adminhotelid and DeleteMark=1 order by sort desc");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid)};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (ds != null && ds.Rows.Count > 0)
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    JsonData json1 = new JsonData();
                    json1["ID"] = ds.Rows[i]["id"].ToString();
                    json1["NAME"] = ds.Rows[i]["name"].ToString();
                    jsondata.Add(json1);
                }
            }
            string json = "";
            json = jsondata.ToJson();
            context.Response.Write(json);
        }

        /// <summary>
        /// 获取客房信息列表
        /// </summary>
        /// <param name="context"></param>
        private void GetroomList(HttpContext context)
        {
            string adminhotelid = context.Request["adminhotelid"];
            string hotelid = context.Request["hotelid"];
            JsonData jsondata = new JsonData();
            string sql = string.Format(@"SELECT id,Name,repeatedly_internal,first_internal FROM dbo.Guestroom WHERE HotelID=@hotelid AND AdminHotelid=@AdminHotelid order by sort desc");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid),
                                     new SqlParam("@hotelid", hotelid)};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (ds != null && ds.Rows.Count > 0)
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    JsonData json1 = new JsonData();
                    json1["ID"] = ds.Rows[i]["id"].ToString();
                    json1["NAME"] = ds.Rows[i]["Name"].ToString();
                    json1["first_internal"] = ds.Rows[i]["first_internal"].ToString(); //首次内部分销
                    json1["repeatedly_internal"] = ds.Rows[i]["repeatedly_internal"].ToString();//多次内部分销
                    jsondata.Add(json1);
                }
            }
            string json = "";
            json = jsondata.ToJson();
            context.Response.Write(json);
        }

        /// <summary>
        /// 保存内部分销客房佣金
        /// </summary>
        /// <param name="context"></param>
        private void save_disFJ(HttpContext context)
        {
            try
            {
                string adminhotelid = context.Request["adminhotelid"];
                string hotelid = context.Request["hotelid"];
                string hdvals = context.Request["hdvals"];


                string sql = string.Format(@"SELECT id,Name,repeatedly_internal,first_internal FROM dbo.Guestroom WHERE HotelID=@hotelid AND AdminHotelid=@AdminHotelid order by sort desc");
                SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid),
                                     new SqlParam("@hotelid", hotelid)};
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
                if (ds != null && ds.Rows.Count > 0)
                {
                    Hashtable ht = new Hashtable();
                    string[] hd = hdvals.Split('|');

                    for (int i = 0; i < hd.Length - 1; i++)
                    {
                        string[] a = hd[i].ToString().Split(',');

                        string sqlup = string.Format(@"update Guestroom set repeatedly_internal=@repeatedly_internal , first_internal=@first_internal where HotelID=@hotelid AND AdminHotelid=@AdminHotelid and id=@id");
                        SqlParam[] parmAddup = new SqlParam[] { 
                                    new SqlParam("@first_internal", a[0]),
                                    new SqlParam("@repeatedly_internal", a[1]),                                
                                    new SqlParam("@AdminHotelid", adminhotelid),
                                    new SqlParam("@hotelid", hotelid),
                                    new SqlParam("@id", ds.Rows[i]["id"])};

                        int s = DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(sqlup), parmAddup);
                    }
                }
                context.Response.Write("1");
                return;
            }
            catch
            {
                context.Response.Write("0");
            }
        }

        /// <summary>
        /// 保存分销功能设置
        /// </summary>
        /// <param name="context"></param>
        private void save_setfx(HttpContext context)
        {
            int fhz = 0;
            try
            {
                string adminhotelid = context.Request["adminhotelid"];
                string isopen = context.Request["isopen"];
                string isid = context.Request["isid"];
                if (isid == "0")
                {
                    Hashtable ht = new Hashtable();
                    ht["adminhotelid"] = adminhotelid;
                    ht["isopen"] = isopen;


                    fhz = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("Distribution", ht);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"update Distribution set isopen=@isopen ");
                    sb.AppendFormat(@" where id=@id and adminhotelid=@adminhotelid");

                    SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@adminHotelid", adminhotelid),
                                     new SqlParam("@id", isid),
                                     new SqlParam("@isopen", isopen)};
                    fhz = DataFactory.SqlDataBase().ExecuteBySql(sb, parmAdd);

                }



                context.Response.Write(fhz);
                return;
            }
            catch
            {
                context.Response.Write(fhz);
            }
        }

        /// <summary>
        /// 展示分销功能设置
        /// </summary>
        /// <param name="context"></param>
        private void Show_setfx(HttpContext context)
        {
            string adminhotelid = context.Request["adminhotelid"];
            string sql = string.Format(@"select ID	,isopen from Distribution where AdminHotelid=@AdminHotelid");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid)};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            if (ds != null && ds.Rows.Count > 0)
            {
                context.Response.Write(ToJson(ds));
            }
            else
            {
                context.Response.Write("");
            }
        }

        #endregion

        #region 设置房态房价规则

        /// <summary>
        /// 保存房价规则
        /// </summary>
        /// <param name="context"></param>
        private void savehouseRule(HttpContext c)
        {


            try
            {
                string RuleName = c.Request["RuleName"];
                string RuleType = c.Request["RuleType"]; //  规则类型提前多少天预订
                string RuleSales = c.Request["RuleSales"]; // 销售规则
                string breakfast_type = c.Request["breakfast_type"]; // 早餐类型
                string breakfast = c.Request["breakfast"];// 早餐份数
                string paytype = c.Request["paytype"]; // 付款方式
                string roomlistid = c.Request["roomlistid"]; //规则房型
                string bz = c.Request["bz"];// 备注
                string hotelid = c.Request["hotelid"];//  酒店ID

                Hashtable hs = new Hashtable();
                hs["RuleName"] = RuleName;
                hs["RuleType"] = RuleType;
                hs["RuleSales"] = RuleSales;
                hs["breakfast_type"] = breakfast_type;
                hs["breakfast"] = breakfast;
                hs["bz"] = bz;
                paytype = paytype.Substring(0, paytype.Length - 1);//减去最后一个“,”;
                hs["paytype"] = paytype;
                hs["hotelid"] = hotelid;

                int i = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("Rules", hs);
                if (i > 0)
                {
                    string[] kfid = roomlistid.Split(',');
                    for (int s = 0; s < kfid.Length - 1; s++)
                    {
                        Hashtable ht = new Hashtable();
                        ht["rule_ID"] = i;
                        ht["room_ID"] = kfid[s];
                        DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("Room_Rule", ht);
                    }
                }

                c.Response.Write(i);
                return;
            }
            catch
            {
                c.Response.Write("");
            }
        }

        /// <summary>
        /// 房型房态列表
        /// </summary>
        /// <param name="c"></param>
        private void houseRuleList(HttpContext c)
        {
            try
            {
                string hotelid = c.Request["hotelid"];
                string txtSearch = c.Request["txtSearch"];//搜索条件客房名称
                string kfid = c.Request["kfid"];//搜索条件客房ID

                JsonData jsondata = new JsonData();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT id,Name FROM Guestroom WHERE HotelID=@hotelid AND id IN (SELECT Room_ID FROM Room_Rule GROUP BY Room_ID)  ORDER BY Sort DESC");
                List<SqlParam> ilistStr = new List<SqlParam>();
                ilistStr.Add(new SqlParam("@hotelid", hotelid));
                if (kfid.Trim() != "" && kfid.Trim() != "0")
                {
                    sb.Append(" AND id = @id ");
                    ilistStr.Add(new SqlParam("@id", kfid.Trim()));
                }
                SqlParam[] param = ilistStr.ToArray();
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (ds != null && ds.Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        JsonData json1 = new JsonData();
                        json1["Id"] = ds.Rows[i]["id"].ToString();
                        json1["Name"] = ds.Rows[i]["name"].ToString();

                        sb = new StringBuilder();
                        sb.Append(@"SELECT * FROM Room_Rule WHERE room_ID = @room_ID");
                        ilistStr = new List<SqlParam>();
                        ilistStr.Add(new SqlParam("@room_ID", ds.Rows[i]["id"].ToString()));
                        if (txtSearch.Trim() != "")
                        {
                            sb.Append(" AND Rule_Name LIKE @Rule_Name ");
                            ilistStr.Add(new SqlParam("@Rule_Name", "%" + txtSearch.Trim() + "%"));//Like的写法              
                        }
                        param = ilistStr.ToArray();
                        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            json1["list"] = new JsonData();
                            for (int s = 0; s < dt.Rows.Count; s++)
                            {
                                JsonData jdInfo = new JsonData();
                                jdInfo["ID"] = dt.Rows[s]["ID"].ToString();
                                jdInfo["Room_ID"] = dt.Rows[s]["Room_ID"].ToString();
                                jdInfo["Rule_Name"] = dt.Rows[s]["Rule_Name"].ToString();
                                #region 销售规则
                                string Sales_Type = dt.Rows[s]["Sales_Type"].ToString();
                                string Sales_Val = dt.Rows[s]["Sales_Val"].ToString();
                                jdInfo["Sales_Val"] = Sales_Val;
                                jdInfo["Sales_Type"] = Sales_Type;
                                string Sales = "";
                                switch (Sales_Type)
                                {
                                    case "0":
                                        Sales = "无";
                                        break;
                                    case "1":
                                        Sales = "提前" + Sales_Val + "天预订";
                                        break;
                                    case "2":
                                        Sales = "连住" + Sales_Val + "晚以上";
                                        break;
                                    default:
                                        break;
                                }
                                jdInfo["Sales"] = Sales;
                                #endregion

                                #region 早餐
                                string Breakfast_Type = dt.Rows[s]["Breakfast_Type"].ToString();
                                string Breakfast_Val = dt.Rows[s]["Breakfast_Val"].ToString();
                                jdInfo["Breakfast_Type"] = Breakfast_Type;
                                jdInfo["Breakfast_Val"] = Breakfast_Val;
                                string Breakfast = "";
                                switch (Breakfast_Type)
                                {
                                    case "0":
                                        Breakfast = "无早";
                                        break;
                                    case "1":
                                        Breakfast = "单早";
                                        break;
                                    case "2":
                                        Breakfast = "双早";
                                        break;
                                    case "3":
                                        Breakfast = Breakfast_Val + "份";
                                        break;
                                    default:
                                        break;
                                }
                                jdInfo["Breakfast"] = Breakfast;
                                #endregion

                                #region 付款方式
                                string Pay_Type = dt.Rows[s]["Pay_Type"].ToString();
                                jdInfo["Pay_Type"] = Pay_Type;

                                string[] pays = Pay_Type.Split(',');
                                string Pay = "";
                                for (int k = 0; k < pays.Count(); k++)
                                {
                                    switch (pays[k])
                                    {
                                        case "1":
                                            Pay += "到店支付、";
                                            break;
                                        case "2":
                                            Pay += "积分兑换、";
                                            break;
                                        case "3":
                                            Pay += "会员卡支付、";
                                            break;
                                        case "4":
                                            Pay += "微信支付、";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                if (Pay.Length > 1)
                                {
                                    Pay = Pay.Substring(0, Pay.Length - 1);
                                }
                                jdInfo["Pay"] = Pay;
                                #endregion
                                jdInfo["Vip_Type"] = dt.Rows[s]["Vip_Type"].ToString();
                                jdInfo["Vip_Val"] = dt.Rows[s]["Vip_Val"].ToString();
                                jdInfo["Remarks"] = dt.Rows[s]["Remarks"].ToString();

                                #region 立减规则
                                string DiscountType = dt.Rows[s]["DiscountType"].ToString();
                                string Discount = dt.Rows[s]["Discount"].ToString();
                                string jyDiscount = dt.Rows[s]["jyDiscount"].ToString();

                                string Discounts = "";
                                switch (DiscountType)
                                {
                                    case "0":
                                        Discounts = "每订单立减" + Discount + "元";
                                        break;
                                    case "1":
                                        Discounts = "每间夜立减" + jyDiscount + "元";
                                        break;
                                    default:
                                        break;
                                }
                                jdInfo["Discount"] = Discounts;
                                #endregion

                                json1["list"].Add(jdInfo);
                            }
                        }
                        jsondata.Add(json1);
                    }
                }
                string json = "";
                json = jsondata.ToJson();
                c.Response.Write(json);
            }
            catch
            {
                c.Response.Write("");
            }
        }
        #endregion

        #region 设置房型规则的价格

        /// <summary>
        /// 设置房型规则的价格
        /// </summary>
        /// <param name="context"></param>
        private void SetRulePrice(HttpContext c)
        {
            try
            {
                string hotelid = c.Request["Hotelid"];//  酒店ID
                string jsonDate = c.Request["JsonDate"];
                DateTime StartDate = Convert.ToDateTime(c.Request["StartDate"]);
                DateTime EndDate = Convert.ToDateTime(c.Request["EndDate"]);
                int days = Math.Abs(((TimeSpan)(EndDate - StartDate)).Days);
                List<Hashtable> jsonlist = JsonConvert.DeserializeObject<List<Hashtable>>(jsonDate);
                if (jsonlist != null)
                {
                    for (int i = 0; i < jsonlist.Count; i++)
                    {
                        Hashtable ht = jsonlist[i];
                        for (int j = 0; j <= days; j++)
                        {
                            DateTime DateRange = StartDate.AddDays(j);
                            ht["DateRange"] = DateRange.ToString("yyyy-MM-dd");
                            ht["HotelId"] = hotelid;

                            StringBuilder sql = new StringBuilder();
                            sql.Append("DELETE Room_Price WHERE RoomId= @RoomId AND RuleId = @RuleId AND DateRange = @DateRange AND vip_code = @vip_code");
                            SqlParam[] param = new SqlParam[] {
                                     new SqlParam("@RoomId", ht["RoomId"]),
                                     new SqlParam("@RuleId", ht["RuleId"]),
                                     new SqlParam("@DateRange", ht["DateRange"]),
                                     new SqlParam("@vip_code", ht["Vip_Code"])};


                            string dayOfWeek = DateRange.DayOfWeek.ToString();
                            Hashtable htInsert = new Hashtable();
                            htInsert["HotelId"] = hotelid;
                            htInsert["RoomId"] = ht["RoomId"];
                            htInsert["RuleId"] = ht["RuleId"];
                            htInsert["DateRange"] = DateRange.ToString("yyyy-MM-dd");
                            htInsert["vip_code"] = ht["Vip_Code"];
                            htInsert["Number"] = 0;
                            htInsert["Room_State"] = 1;
                            htInsert["Room_Label"] = "";
                            if (dayOfWeek == "Friday" || dayOfWeek == "Saturday")//星期五 星期六为周末价
                            {
                                htInsert["Price"] = ht["zmj"];
                                htInsert["Integral"] = ht["zmj_jf"];
                            }
                            else
                            {
                                htInsert["Price"] = ht["prj"];
                                htInsert["Integral"] = ht["prj_jf"];
                            }
                            DataFactory.SqlDataBase().GetObjectValue(sql, param);//删除 之前设置
                            DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("Room_Price", htInsert);

                        }
                    }
                }
                c.Response.Write("1");
                return;
            }
            catch
            {
                c.Response.Write("");
            }
        }

        #endregion

        #region  默认房型价格设置

        /// <summary>
        /// 默认房型价格设置
        /// </summary>
        /// <param name="context"></param>
        private void DefaultRoomPrice(HttpContext context)
        {
            string adminhotelid = context.Request["adminhotelid"]; //酒店参数ID

            string TbHtml = "";
            #region ***** 加载表格 *****
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT replace(hylxcode, ' ', '') as jb,hylxname as LevelName FROM hy_hylxbmb where  AdminHotelid='{0}' order by sort desc",adminhotelid);
            DataTable dt = DataFactory.SqlDataBase(adminhotelid).GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                sql = new StringBuilder();
                sql.Append("SELECT name,unit,code,Abbreviation FROM MemberType WHERE isdelete = 1 ");
                string jfState = HotelTreeHelper.GetJFState();
                if (jfState != "1")
                {
                    sql.Append(" AND Abbreviation != 'jf'");
                }
                DataTable dtType = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
                int textWidth = 400 / dt.Rows.Count;
                if (dtType != null && dtType.Rows.Count > 0)
                {
                    //头部标题
                    TbHtml += "<table>";
                    TbHtml += " <thead>";

                    TbHtml += "<tr>";
                    TbHtml += "<th class=\"fw_hy\" width='120'>会员等级";
                    TbHtml += "</th>";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        TbHtml += "<th class=\"\">";
                        TbHtml += dt.Rows[j]["LevelName"].ToString();
                        TbHtml += "</th>";
                    }
                    TbHtml += "</tr>";
                    TbHtml += "</thead>";
                    TbHtml += "<tbody>";
                    //类型列
                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        TbHtml += "<tr>";
                        TbHtml += "<td>";
                        TbHtml += dtType.Rows[i]["Name"].ToString();
                        TbHtml += "</td>";
                        string lx_code = dtType.Rows[i]["Code"].ToString();//类型 编号
                        string lx_sx = dtType.Rows[i]["Abbreviation"].ToString();//类型 缩写
                        for (int j = 0; j < dt.Rows.Count; j++)//循环等级
                        {
                            string jb = dt.Rows[j]["jb"].ToString();//会员级别
                            TbHtml += "<td>";
                            TbHtml += string.Format("<input type=\"text\" maxlength=\"5\" hydj='{0}' MemberType='{1}' id=\"{2}{0}{1}\" name=\"{2}{0}{1}\" autocomplete='off'/> ", jb, lx_code, lx_sx);
                            TbHtml += "<i class='dw'>" + dtType.Rows[i]["Unit"].ToString() + "</i>";
                            TbHtml += "</td>";
                        }
                        TbHtml += "</tr>";
                    }
                    TbHtml += "</tbody>";
                    TbHtml += "</table>";
                }
            }
            #endregion
            context.Response.Write(TbHtml);
        }


        #endregion

        #region 批量设置房态

        /// <summary>
        /// 批量设置房态
        /// </summary>
        /// <param name="context"></param>
        private void SetRoomState(HttpContext c)
        {
            try
            {
                string hotelid = c.Request["Hotelid"];//  酒店ID
                string jsonDate = c.Request["JsonDate"];
                DateTime StartDate = Convert.ToDateTime(c.Request["StartDate"]);
                DateTime EndDate = Convert.ToDateTime(c.Request["EndDate"]);
                int rs = 1;//  房态
                rs = Convert.ToInt32(c.Request["Room_State"]);
                int en = 0;
                en = Convert.ToInt32(c.Request["EffectiveNumber"]);//可订房数
                string ed = c.Request["EffectiveDate"];//  有效日期
                int days = Math.Abs(((TimeSpan)(EndDate - StartDate)).Days);
                List<Hashtable> jsonlist = JsonConvert.DeserializeObject<List<Hashtable>>(jsonDate);
                if (jsonlist != null)
                {
                    DataTable dtVipCode = null;
                    for (int i = 0; i < jsonlist.Count; i++)
                    {
                        Hashtable ht = jsonlist[i];
                        for (int j = 0; j <= days; j++)
                        {
                            DateTime DateRange = StartDate.AddDays(j);
                            string dw = DateRange.DayOfWeek.ToString();
                            if (ed.IndexOf(dw) >= 0)
                            {

                                StringBuilder sql = new StringBuilder();
                                sql.Append("SELECT Id FROM Room_Price WHERE RoomId= @RoomId AND RuleId = @RuleId AND DateRange = @DateRange");
                                SqlParam[] param = new SqlParam[] {
                                     new SqlParam("@RoomId", ht["RoomId"]),
                                     new SqlParam("@RuleId", ht["RuleId"]),
                                     new SqlParam("@DateRange", DateRange.ToString("yyyy-MM-dd"))};

                                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);//
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    string id = dt.Rows[0][0].ToString();
                                    sql = new StringBuilder();
                                    sql.Append("UPDATE Room_Price SET Room_State = @Room_State WHERE RoomId= @RoomId AND RuleId = @RuleId AND DateRange = @DateRange");
                                    param = new SqlParam[] {
                                     new SqlParam("@Room_State",rs),
                                     new SqlParam("@RoomId", ht["RoomId"]),
                                     new SqlParam("@RuleId", ht["RuleId"]),
                                     new SqlParam("@DateRange", DateRange.ToString("yyyy-MM-dd"))};
                                    DataFactory.SqlDataBase().GetObjectValue(sql, param);//修改
                                }
                                else
                                {
                                    Hashtable htInsert = new Hashtable();
                                    htInsert["HotelId"] = hotelid;
                                    htInsert["RoomId"] = ht["RoomId"];
                                    htInsert["RuleId"] = ht["RuleId"];
                                    htInsert["DateRange"] = DateRange.ToString("yyyy-MM-dd");
                                    htInsert["Price"] = "0";
                                    htInsert["Integral"] = "0";
                                    htInsert["Number"] = "0";
                                    htInsert["Room_State"] = rs;
                                    if (dtVipCode == null)
                                    {
                                        StringBuilder sqlVip = new StringBuilder();
                                        sqlVip.AppendFormat("SELECT replace(hylxcode, ' ', '') as code,hylxname as LevelName FROM hy_hylxbmb where AdminHotelid='{0}' order by sort desc",RequestSession.GetSessionUser().AdminHotelid.ToString());
                                        dtVipCode = DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).GetDataTableBySQL(sqlVip);
                                    }
                                    if (dtVipCode != null && dtVipCode.Rows.Count > 0)
                                    {
                                        for (int v = 0; v < dtVipCode.Rows.Count; v++)
                                        {
                                            htInsert["vip_code"] = dtVipCode.Rows[v]["code"].ToString();
                                            DataFactory.SqlDataBase().InsertByHashtable("Room_Price", htInsert);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                c.Response.Write("1");
                return;
            }
            catch
            {
                c.Response.Write("");
            }
        }

        /// <summary>
        /// 设置房态
        /// </summary>
        /// <param name="context"></param>
        private void RoomState(HttpContext c)
        {
            try
            {
                string hotelid = c.Request["HotelId"];//  酒店ID
                string roomId = c.Request["RoomId"];
                string ruleId = c.Request["RuleId"];
                string vipCode = c.Request["VipCode"];
                int roomState = Convert.ToInt32(c.Request["RoomState"]);
                DateTime DateRange = Convert.ToDateTime(c.Request["DateRange"]);

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT Id FROM Room_Price WHERE RoomId= @RoomId AND RuleId = @RuleId AND DateRange = @DateRange AND vip_code = @vip_code");
                SqlParam[] param = new SqlParam[] {
                                     new SqlParam("@RoomId", roomId),
                                     new SqlParam("@RuleId", ruleId),
                                     new SqlParam("@vip_code", vipCode),
                                     new SqlParam("@DateRange", DateRange.ToString("yyyy-MM-dd"))};

                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);//
                if (dt != null && dt.Rows.Count > 0)
                {
                    string id = dt.Rows[0][0].ToString();
                    sql = new StringBuilder();
                    sql.Append("UPDATE Room_Price SET Room_State = @Room_State WHERE Id=@Id");
                    param = new SqlParam[] {
                                     new SqlParam("@Room_State",roomState),
                                     new SqlParam("@Id", id)};
                    DataFactory.SqlDataBase().GetObjectValue(sql, param);//修改
                }
                else
                {
                    Hashtable ht = new Hashtable();
                    ht["HotelId"] = hotelid;
                    ht["RoomId"] = roomId;
                    ht["RuleId"] = ruleId;
                    ht["DateRange"] = DateRange.ToString("yyyy-MM-dd");
                    ht["vip_code"] = vipCode;
                    ht["Price"] = "0";
                    ht["Integral"] = "0";
                    ht["Number"] = "0";
                    ht["Room_State"] = roomState;
                    DataFactory.SqlDataBase().InsertByHashtable("Room_Price", ht);
                }

                c.Response.Write("1");
                return;
            }
            catch
            {
                c.Response.Write("");
            }
        }

        #endregion

        #region 获取售房规则

        /// <summary>
        /// 获取售房规则
        /// </summary>
        /// <param name="context"></param>
        private void GetRule(HttpContext c)
        {
            try
            {
                string roomId = c.Request["RoomId"];//  房型ID
                string displayType = c.Request["DisplayType"];
                DataTable dt = null;//
                if (!string.IsNullOrEmpty(roomId))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("SELECT id,Rule_Name FROM Room_Rule WHERE Room_ID = @Room_ID");
                    SqlParam[] param = new SqlParam[] {
                                     new SqlParam("@Room_ID", roomId)};
                    dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);//
                }
                string ruleHtml = "";
                if (displayType == "3")
                {
                    ruleHtml += "<b dataid='0' class='active'>全部</b>";
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ruleHtml += string.Format("<b dataid='{0}' {1}>{2}</b>", dt.Rows[i]["id"].ToString(), (ruleHtml == "" && i == 0) ? "class='active'" : "", dt.Rows[i]["Rule_Name"].ToString());
                    }
                }
                c.Response.Write(ruleHtml);
                return;
            }
            catch
            {
                c.Response.Write("");
            }
        }

        #endregion

        #region ***按月设置***


        /// <summary>
        /// 加载 月 展示表格
        /// </summary>
        /// <param name="context"></param>
        private void GetMonthPriceTable(HttpContext context)
        {
            try
            {
                string hotelId = context.Request["HotelId"]; //酒店ID
                string roomId = context.Request["RoomId"]; //客房ID
                string ruleId = context.Request["RuleId"]; //客房ID
                string vipCode = context.Request["VipCode"]; //会员级别

                string year = context.Request["year"]; //年
                string month = context.Request["Month"]; //月


                string dayhtml = "";

                DateTime startDate = DateTime.Parse(year + "-" + month + "-01");       //本月第一天
                StringBuilder sb = new StringBuilder();

                int day = startDate.AddMonths(1).AddDays(-1).Day;      //本月总天数

                DateTime endTime = DateTime.Parse(year + "-" + month + "-" + day);       //本月最后一天

                int week = WXJSAPI.getWeek(startDate);
                sb.Append("<table><tr>");
                string time;
                //输出上月中的天数
                if (week != 7)
                {
                    DateTime upyear = startDate.AddDays(-1);       //上个月最后一天
                    int upMonth = upyear.Day;           //上月总天数
                    for (int i = 0; i < week; i++)
                    {
                        int d = upMonth - week + 1;
                        time = DateTime.Parse(upyear.Year + "-" + upyear.Month + "-" + d).ToString();
                        sb.Append("<td><b class='date'>" + d + "</b></td>");
                        upMonth++;
                    }
                }

                #region ***** 加载表格 *****
                Hashtable ht = new Hashtable();
                ht["RoomId"] = roomId;
                ht["RuleId"] = ruleId;
                ht["VipCode"] = vipCode;
                ht["StartDate"] = startDate.ToString("yyyy-MM-dd");
                ht["EndTime"] = endTime.ToString("yyyy-MM-dd");
                DataTable RoomPrice = DataFactory.SqlDataBase().GetDataTableProc("P_GetRoomPrice", ht);


                #region ***** 输出本月中的天数 *****
                string content = "";
                for (int j = 1; j <= day; j++)
                {
                    if ((j + week - 1) % 7 == 0)
                    {
                        sb.Append("</tr><tr>");
                    }
                    DateTime riqi = DateTime.Parse(year + "-" + month + "-" + j);
                    time = riqi.ToString("yyyy-MM-dd");
                    string dayOfWeek = riqi.DayOfWeek.ToString();
                    //下拉框
                    //根据客服ID查询价格

                    #region ***** 输出价格 *****
                    content = @"
                <div class='currInfo'>
                    <span class='fj'><small>房价</small><small>￥{0}</small></span>
                    <span class='jf'><small>积分</small><small>{1}</small></span>
                    <span class='kd'><small>可订</small><small>{3}/{2}</small></span>
                </div>";
                    string jg = "";
                    string jf = "";
                    string kdf = "";
                    string ydf = "0";
                    string kgClose = "";
                    if (RoomPrice != null)
                    {
                        DataTable newDT = DataTableHelper.GetNewDataTable(RoomPrice, "DateRange='" + time + "'");
                        if (newDT != null)
                        {
                            jg = newDT.Rows[0]["Price"].ToString();
                            jf = newDT.Rows[0]["Integral"].ToString();
                            kdf = newDT.Rows[0]["Number"].ToString();
                            ydf = newDT.Rows[0]["Order_Number"].ToString();
                            if (newDT.Rows[0]["Room_State"].ToString() != "1")
                            {
                                kgClose = " kgClose";
                            }
                        }
                    }
                    content = string.Format(content, jg, jf, kdf, ydf);

                    if (DateTime.Parse(time) > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        sb.AppendFormat(@"<td class='calendar' day='{0}' ><b class='date'>{1}</b>{2}<i day='{0}' class='icon-kg1 {3}'></i></td>", time, j, content, kgClose);
                    }
                    else if (DateTime.Parse(time) == DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        sb.AppendFormat(@"<td class='calendar today' day='{0}'><b class='date'>{1}</b>{2}<i day='{0}' class='icon-kg1 {3}'></i></td>", time, j, content, kgClose);
                    }
                    else
                    {
                        sb.AppendFormat(@"<td class='calendar guo' day='{0}'><b class='date'>{1}</b>{2}<i day='{0}' class='icon-kg1 {3}'></i></td>", time, j, content, kgClose);
                    }

                    #endregion
                }
                sb.Append("</table>");
                #endregion
                #endregion

                dayhtml = sb.ToString();

                context.Response.Write(dayhtml);

            }
            catch (Exception)
            {

                context.Response.Write("");
            }
        }


        #endregion

        #region ***按周设置***
        /// <summary> 
        /// 计算某日起始日期（礼拜一的日期） 
        /// </summary> 
        /// <param name="somedate">该周中任意一天</param> 
        /// <returns>返回礼拜一日期，后面的具体时、分、秒和传入值相等</returns> 
        public static DateTime getmondaydate(DateTime somedate)
        {
            int i = somedate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，sunday排在最前，此时sunday-monday=-1，必须+7=6。 
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return somedate.Subtract(ts);
        }

        /// <summary>
        /// 加载 周 展示表格
        /// </summary>
        /// <param name="context"></param>
        private void GetWeekPriceTable(HttpContext context)
        {
            try
            {
                string hotelId = context.Request["HotelId"]; //酒店ID
                string roomId = context.Request["RoomId"]; //客房ID
                string ruleId = context.Request["RuleId"]; //规则ID
                string vipCode = context.Request["VipCode"]; //会员级别

                DateTime dateRange = Convert.ToDateTime(context.Request["DateRange"]); //日期

                DateTime startDate = getmondaydate(dateRange);       // 指定日期的星期一
                DateTime endTime = startDate.AddDays(7);       //星期日
                #region 获取数据
                StringBuilder sqlRoom = new StringBuilder();
                sqlRoom.Append(@"
                SELECT  ID ,
                        Name
                FROM    Guestroom
                WHERE   HotelID = @HotelID
                        AND ID IN ( SELECT  Room_ID
                                    FROM    Room_Rule
                                    WHERE   HotelID = @HotelID )");
                List<SqlParam> lspRoom = new List<SqlParam>();
                lspRoom.Add(new SqlParam("@HotelID", hotelId));
                if (!string.IsNullOrEmpty(roomId))
                {
                    sqlRoom.Append(" AND ID = @ID ");
                    lspRoom.Add(new SqlParam("@ID", roomId));
                }
                SqlParam[] spRoom = lspRoom.ToArray();

                DataTable dtRoom = DataFactory.SqlDataBase().GetDataTableBySQL(sqlRoom, spRoom);//房型

                StringBuilder sqlRule = new StringBuilder();
                sqlRule.Append("SELECT ID,Rule_Name,Room_ID FROM Room_Rule WHERE HotelID = @HotelID ");
                List<SqlParam> lspRule = new List<SqlParam>();
                lspRule.Add(new SqlParam("@HotelID", hotelId));

                if (!string.IsNullOrEmpty(roomId))
                {
                    sqlRule.Append(" AND Room_ID = @Room_ID ");
                    lspRule.Add(new SqlParam("@Room_ID", roomId));
                }
                if (!string.IsNullOrEmpty(ruleId) && ruleId != "0")
                {
                    sqlRule.Append(" AND ID = @ID ");
                    lspRule.Add(new SqlParam("@ID", ruleId));
                }
                SqlParam[] spRule = lspRule.ToArray();
                DataTable dtRule = DataFactory.SqlDataBase().GetDataTableBySQL(sqlRule, spRule);//规则


                StringBuilder sqlDefaultPrice = new StringBuilder();
                sqlDefaultPrice.Append("SELECT * FROM GSMoeny WHERE HotelID = @HotelID ");

                List<SqlParam> lspDefaultPrice = new List<SqlParam>();
                lspDefaultPrice.Add(new SqlParam("@HotelID", hotelId));

                if (!string.IsNullOrEmpty(roomId))
                {
                    sqlDefaultPrice.Append(" AND GSID = @GSID ");
                    lspDefaultPrice.Add(new SqlParam("@GSID", roomId));
                }
                if (!string.IsNullOrEmpty(vipCode))
                {
                    sqlDefaultPrice.Append(" AND Hydj = @Hydj ");
                    lspDefaultPrice.Add(new SqlParam("@Hydj", vipCode));
                }

                SqlParam[] spDefaultPrice = lspDefaultPrice.ToArray();
                DataTable dtDefaultPrice = DataFactory.SqlDataBase().GetDataTableBySQL(sqlDefaultPrice, spDefaultPrice);//默认价格


                StringBuilder sqlWeekPrice = new StringBuilder();
                sqlWeekPrice.Append(@"
                SELECT  *
                FROM    dbo.Room_Price
                WHERE   HotelId = @HotelId
                        AND DateRange >= @startDate
                        AND DateRange <= @endTime");
                SqlParam[] spWeekPrice = new SqlParam[] { 
                                     new SqlParam("@HotelID",hotelId),
                                     new SqlParam("@startDate",startDate.ToString("yyyy-MM-dd")),
                                     new SqlParam("@endTime",endTime.ToString("yyyy-MM-dd"))};
                DataTable dtWeekPrice = DataFactory.SqlDataBase().GetDataTableBySQL(sqlWeekPrice, spWeekPrice);//周内价格
                #endregion

                #region 加载表格
                StringBuilder sb = new StringBuilder();
                int week = WXJSAPI.getWeek(startDate);
                sb.Append("<table>");
                string content = @"
                <div class='currInfo'>
                    <span class='fj'><small>房价</small><small>￥{0}</small></span>
                    <span class='jf'><small>积分</small><small>{1}</small></span>
                    <span class='kd'><small>可订</small><small>{2}</small></span>
                </div>
                <i class='icon-kg1 {3}'></i>";
                for (int iRoom = 0; iRoom < dtRoom.Rows.Count; iRoom++)
                {
                    string iRoom_ID = dtRoom.Rows[iRoom]["Id"].ToString();
                    DataTable newRule = DataTableHelper.GetNewDataTable(dtRule, "Room_ID='" + iRoom_ID + "'");
                    if (newRule != null)
                    {
                        for (int iRule = 0; iRule < newRule.Rows.Count; iRule++)
                        {
                            string iRuleId = newRule.Rows[iRule]["Id"].ToString();
                            sb.AppendFormat("<tr roomid='{0}' ruleid='{1}'>", iRoom_ID, iRuleId);
                            if (iRule == 0)
                            {
                                sb.AppendFormat("<td rowspan='{0}'>{1}</td>", newRule.Rows.Count, dtRoom.Rows[iRoom]["Name"]);
                            }

                            sb.AppendFormat("<td>{0}</td>", newRule.Rows[iRule]["Rule_Name"]);
                            for (int iWeek = 0; iWeek < 7; iWeek++)
                            {
                                DateTime iDateRange = startDate.AddDays(iWeek);
                                string jg = "";
                                string jf = "";
                                string kdf = "";
                                string kgClose = "";
                                string dayOfWeek = iDateRange.DayOfWeek.ToString();

                                #region ******** 默认价格 ********
                                var price_type = "2";
                                if (dayOfWeek == "Friday" || dayOfWeek == "Saturday")//星期五 星期六为周末价
                                {
                                    price_type = "3";
                                }
                                DataTable newDefaultPrice = DataTableHelper.GetNewDataTable(dtDefaultPrice, "GSID='" + iRoom_ID + "' AND Hydj = '" + vipCode + "' AND Type = '" + price_type + "'  ");
                                if (newDefaultPrice != null && newDefaultPrice.Rows.Count > 0)
                                {
                                    jg = newDefaultPrice.Rows[0]["jg"].ToString();
                                    jf = newDefaultPrice.Rows[0]["jf"].ToString();
                                    kdf = newDefaultPrice.Rows[0]["fjs"].ToString();
                                }
                                #endregion

                                #region ******** 自定义价格 ********
                                DataTable newWeekPrice = DataTableHelper.GetNewDataTable(dtWeekPrice, "RoomId='" + iRoom_ID + "' AND RuleId = '" + iRuleId + "' AND vip_code = '" + vipCode + "' AND DateRange = '" + iDateRange.ToString("yyyy-MM-dd") + "'");
                                if (newWeekPrice != null && newWeekPrice.Rows.Count > 0)
                                {
                                    object oPrice = newWeekPrice.Rows[0]["Price"];
                                    if (oPrice != null && Convert.ToDouble(oPrice) > 0)
                                    {
                                        jg = oPrice.ToString();
                                    }
                                    object oIntegral = newWeekPrice.Rows[0]["Integral"];
                                    if (oIntegral != null && Convert.ToDouble(oIntegral) > 0)
                                    {
                                        jf = oIntegral.ToString();
                                    }
                                    object oNumber = newWeekPrice.Rows[0]["Number"];
                                    if (oNumber != null && Convert.ToDouble(oNumber) > 0)
                                    {
                                        kdf = oNumber.ToString();
                                    }
                                    if (newWeekPrice.Rows[0]["Room_State"].ToString() != "1")
                                    {
                                        kgClose = " kgClose";
                                    }
                                }
                                #endregion

                                string dtHtml = content;
                                dtHtml = string.Format(dtHtml, jg, jf, kdf, kgClose);


                                string td_class = "calendar";
                                if (iDateRange.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                                {
                                    td_class = "calendar today";
                                }
                                else if (iDateRange < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                                {
                                    td_class = "calendar guo";
                                }

                                sb.AppendFormat("<td RoomId='{0}' RuleId='{1}' class='{2}' day='{3}'>{4}</td>", iRoom_ID, iRuleId, td_class, iDateRange.ToString("yyyy-MM-dd"), dtHtml);
                            }
                            sb.Append("</tr>");
                        }
                    }
                }
                #endregion

                context.Response.Write(sb.ToString());

            }
            catch (Exception)
            {

                context.Response.Write("");
            }
        }


        /// <summary>
        /// 上下周
        /// </summary>
        /// <param name="context"></param>
        private void WeekDate(HttpContext context)
        {
            try
            {
                string type = context.Request["Type"]; //
                DateTime dateRange = Convert.ToDateTime(context.Request["DateRange"]); //日期
                DateTime we;
                if (type == "LastWeek")
                {
                    we = dateRange.AddDays(-7);
                }
                else
                {
                    we = dateRange.AddDays(7);
                }
                context.Response.Write(we.ToString("yyyy-MM-dd"));
            }
            catch (Exception)
            {
                context.Response.Write(DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        #endregion


        #region  点击表格编辑
        /// <summary>
        /// 获取售房规则
        /// </summary>
        /// <param name="context"></param>
        private void GetEditRule(HttpContext c)
        {
            try
            {
                string hotelId = c.Request["HotelId"];//  酒店ID
                string roomId = c.Request["RoomId"];//  房型ID
                string ruleId = c.Request["RuleId"];
                DataTable dt = null;//
                if (!string.IsNullOrEmpty(roomId))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("SELECT id,Rule_Name FROM Room_Rule WHERE Room_ID = @Room_ID");
                    SqlParam[] param = new SqlParam[] {
                                     new SqlParam("@Room_ID", roomId)};
                    dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);//
                }
                string ruleHtml = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string rid = dt.Rows[i]["id"].ToString();
                        string tc = "";
                        if (rid == ruleId)
                        {
                            tc = " class='checked'";
                        }
                        ruleHtml += string.Format("<label dataid='{0}' {1}>{2}</label>", rid, tc, dt.Rows[i]["Rule_Name"].ToString());
                    }
                }
                c.Response.Write(ruleHtml);
                return;
            }
            catch
            {
                c.Response.Write("");
            }
        }
        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="context"></param>
        private void GetEditPrice(HttpContext c)
        {
            JsonData jd = new JsonData();
            jd["code"] = "0";
            try
            {
                string hotelId = c.Request["HotelId"];//  酒店ID
                string roomId = c.Request["RoomId"];//  房型ID
                string ruleId = c.Request["RuleId"];
                string vipCode = c.Request["VipCode"];
                string dateRange = c.Request["DateRange"];
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM Room_Price WHERE RoomId = @RoomId AND RuleId = @RuleId AND Vip_Code = @Vip_Code AND DateRange = @DateRange");
                SqlParam[] param = new SqlParam[] {
                                     new SqlParam("@RoomId", roomId),
                                     new SqlParam("@RuleId", ruleId),
                                     new SqlParam("@DateRange", dateRange),
                                     new SqlParam("@Vip_Code", vipCode)};
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);//
                if (dt != null && dt.Rows.Count > 0)
                {
                    jd["code"] = "1";
                    jd["Room_State"] = dt.Rows[0]["Room_State"].ToString();
                    jd["Number"] = dt.Rows[0]["Number"].ToString();
                    jd["Room_Label"] = dt.Rows[0]["Room_Label"].ToString();
                    jd["Price"] = dt.Rows[0]["Price"].ToString();
                    jd["Integral"] = dt.Rows[0]["Integral"].ToString();
                }
                c.Response.Write(jd.ToJson());
                return;
            }
            catch
            {
                c.Response.Write(jd.ToJson());
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="c"></param>
        private void SaveRoomPrice(HttpContext c)
        {
            try
            {
                string hotelId = c.Request["HotelId"];//  酒店ID
                string roomId = c.Request["RoomId"];//  房型ID
                string ruleId = c.Request["RuleId"];
                string vipCode = c.Request["VipCode"];
                string room_State = c.Request["RoomState"];
                DateTime startTime = Convert.ToDateTime(c.Request["StartTime"]);
                DateTime endTime = Convert.ToDateTime(c.Request["EndTime"]);
                string weeks = c.Request["Weeks"];
                string number = c.Request["Number"];
                string labels = c.Request["Labels"];
                string price = c.Request["Price"];
                string integral = c.Request["Integral"];

                int days = Math.Abs(((TimeSpan)(endTime - startTime)).Days);
                for (int j = 0; j <= days; j++)
                {
                    DateTime DateRange = startTime.AddDays(j);
                    string dw = DateRange.DayOfWeek.ToString();
                    if (weeks.IndexOf(dw) >= 0)
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("SELECT Id FROM Room_Price WHERE RoomId= @RoomId AND RuleId = @RuleId AND Vip_Code = @Vip_Code AND DateRange = @DateRange");
                        SqlParam[] param = new SqlParam[] {
                                     new SqlParam("@RoomId", roomId),
                                     new SqlParam("@RuleId", ruleId),
                                     new SqlParam("@Vip_Code",vipCode),
                                     new SqlParam("@DateRange", DateRange.ToString("yyyy-MM-dd"))};

                        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);//
                        string pid = "";
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            pid = dt.Rows[0]["id"].ToString();
                        }
                        Hashtable ht = new Hashtable();
                        ht["HotelId"] = hotelId;
                        ht["RoomId"] = roomId;
                        ht["RuleId"] = ruleId;
                        ht["DateRange"] = DateRange.ToString("yyyy-MM-dd");
                        ht["vip_code"] = vipCode;
                        ht["Price"] = price;
                        ht["Integral"] = integral;
                        ht["Number"] = number;
                        ht["Room_State"] = room_State;
                        ht["Room_Label"] = labels;

                        DataFactory.SqlDataBase().Submit_AddOrEdit("Room_Price", "Id", pid, ht);
                    }
                }
                c.Response.Write("1");
                return;
            }
            catch
            {
                c.Response.Write("");
            }
        }
        #endregion

        #region  首页销售间夜

        /// <summary>
        /// 销售间夜
        /// </summary>
        private void getRoom(HttpContext c)
        {
            string HotelId = c.Request["HotelId"];//  酒店ID
            string StartTime = c.Request["StartTime"];//  酒店ID
            string EndTime = c.Request["EndTime"];//  酒店ID

            Hashtable ht = new Hashtable();
            ht["HotelId"] = HotelId;
            ht["AdminHotelId"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
            ht["StartDate"] = StartTime;
            ht["EndDate"] = EndTime + " 23:59";
            DataTable dt = DataFactory.SqlDataBase().GetDataTableProc("P_ZDY_Index_Room", ht);
            c.Response.Write(ToJson(dt));
        }

        #endregion

        #region  首页业绩评估

        /// <summary>
        /// 业绩评估
        /// </summary>
        private void getUsersBonus(HttpContext c)
        {
            string HotelId = c.Request["HotelId"];//  酒店ID
            string StartTime = c.Request["StartTime"];//  酒店ID
            string EndTime = c.Request["EndTime"];//  酒店ID

            Hashtable ht = new Hashtable();
            ht["HotelId"] = HotelId;
            ht["AdminHotelId"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
            ht["StartDate"] = StartTime;
            ht["EndDate"] = EndTime + " 23:59";

            DataTable dt = DataFactory.SqlDataBase().GetDataTableProc("P_ZDY_Index_UsersBonus", ht);

            c.Response.Write(ToJson(dt));

        }

        #endregion


        #region  首页数据统计

        /// <summary>
        /// 数据统计
        /// </summary>
        private void getdata(HttpContext c)
        {
            string StartTime = c.Request["StartTime"];//  酒店ID
            string EndTime = c.Request["EndTime"];//  酒店ID
            Hashtable ht = new Hashtable();
            ht["AdminHotelId"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
            ht["StartDate"] = StartTime;
            ht["EndDate"] = EndTime + " 23:59";
            DataTable dt = DataFactory.SqlDataBase().GetDataTableProc("P_ZDY_Index", ht);
            c.Response.Write(ToJson(dt));
        }

        #endregion
        

        protected string ToJson(DataTable dt)
        {
            StringBuilder JsonString = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonString.Append("[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }
                JsonString.Append("]");

                return JsonString.ToString();
            }
            else
            {
                return null;
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