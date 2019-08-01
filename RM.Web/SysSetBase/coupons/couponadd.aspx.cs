using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SQL;

using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Busines;
using System.Text;
using System.Collections;
using RM.Common.DotNetBean;
using RM.Web.RMBase;
using RM.Common.DotNetCode;
using System.Net;
using RM.Web.Lib;

namespace RM.Web.SysSetBase.coupons
{
    public partial class couponadd : System.Web.UI.Page
    {

        public string _Effective = null;
        public string _sfzs = null;
        public string _sfqy = null;
        public string _hytype = null;
        public string _zdzs = null;
        public string _sumhotel = null;
        public string _Membergrade = string.Empty;
        public string _jjr = string.Empty;
        public string _yxMembergrade = string.Empty;
        public string _Is_Day_ok = null;
        public string _Open_Hotel = string.Empty;
        public string _ConsumptionType = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                adminhotelid.Value = RequestSession.GetSessionUser().AdminHotelid.ToString();
                StringBuilder sqlss = new StringBuilder();
                sqlss.Append("select * from PreferentialType where IsElectronics=0");
                DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(sqlss);
                if (dts.Rows.Count > 0)
                {
                    DDLmode.Items.Insert(0, new ListItem("请选择", "0"));
                    for (int i = 0; i < dts.Rows.Count; i++)
                    {
                        DDLmode.Items.Insert(i + 1, new ListItem(dts.Rows[i]["TypeName"].ToString(), dts.Rows[i]["id"].ToString()));
                    }
                }
                Com.SetChildren(0, 0, 0, DDLmode, 0);

                StringBuilder sqlsss = new StringBuilder();
                sqlsss.Append("select * from CouponType  ");
                DataTable dtss = DataFactory.SqlDataBase().GetDataTableBySQL(sqlsss);
                if (dtss.Rows.Count > 0)
                {
                    ddltype.Items.Insert(0, new ListItem("请选择", "0"));
                    for (int i = 0; i < dtss.Rows.Count; i++)
                    {
                        ddltype.Items.Insert(i + 1, new ListItem(dtss.Rows[i]["name"].ToString(), dtss.Rows[i]["id"].ToString()));
                    }
                }
                Com.SetChildren(0, 0, 0, ddltype, 0);

                if (Request["ID"] != null)
                {

                    string sql = string.Format(@"select * from coupon where id='{0}'", Request["id"]);
                    DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
                    if (ds != null && ds.Rows.Count > 0)
                    {

                        B_sj.Value = ds.Rows[0]["BiginTime"].ToString().Split(' ')[0];
                        E_sj.Value = ds.Rows[0]["EndinTime"].ToString().Split(' ')[0];
                        txtName.Value = ds.Rows[0]["CouponName"].ToString();
                        txtPar.Value = ds.Rows[0]["Par"].ToString();
                        txtUsedMin.Value = ds.Rows[0]["UsedMin"].ToString();
                        txtInstructions.Value = ds.Rows[0]["ProductRange"].ToString();
                        _sumhotel = sumhotel.Value = ds.Rows[0]["ServiceRange"].ToString();
                        DDLmode.SelectedValue = ds.Rows[0]["Mode"].ToString();
                        ddltype.SelectedValue = ds.Rows[0]["TypeID"].ToString();
                        //指定赠送
                        if (ds.Rows[0]["MembergradeType"] != null && ds.Rows[0]["MembergradeType"].ToString() != "")
                        {
                            _zdzs = hfzdzs.Value = ds.Rows[0]["MembergradeType"].ToString();
                        }


                        if (ds.Rows[0]["FirstMoney"] != null && ds.Rows[0]["FirstMoney"].ToString() != "")
                        {
                            txtFirstMoney.Value = ds.Rows[0]["FirstMoney"].ToString();
                        }
                        if (ds.Rows[0]["SecondMoney"] != null && ds.Rows[0]["SecondMoney"].ToString() != "")
                        {
                            txtcz.Value = ds.Rows[0]["SecondMoney"].ToString();
                        }

                        if (ds.Rows[0]["IsEnable"] != null && ds.Rows[0]["IsEnable"].ToString() != "")
                        {

                            hfsfqy.Value = _sfqy = ds.Rows[0]["IsEnable"].ToString();
                        }
                        if (ds.Rows[0]["Is_Day_ok"] != null && ds.Rows[0]["Is_Day_ok"].ToString() != "")
                        {

                            hfIs_Day_ok.Value = _Is_Day_ok = ds.Rows[0]["Is_Day_ok"].ToString();
                        }

                        if (ds.Rows[0]["hytype"] != null && ds.Rows[0]["hytype"].ToString() != "")
                        {

                            hhytype.Value = _hytype = ds.Rows[0]["hytype"].ToString();
                        }
                        if (ds.Rows[0]["Isgiving"] != null && ds.Rows[0]["Isgiving"].ToString() != "")
                        {
                            hfsfzs.Value = _sfzs = ds.Rows[0]["Isgiving"].ToString();
                        }

                        if (ds.Rows[0]["ConsumptionType"] != null && ds.Rows[0]["ConsumptionType"].ToString() != "")
                        {

                            HConsumptionType.Value = _ConsumptionType = ds.Rows[0]["ConsumptionType"].ToString();
                        }

                        txtNum.Value = ds.Rows[0]["count"].ToString();
                        txtTotal.Value = ds.Rows[0]["Total"].ToString();
                        if (ds.Rows[0]["Mode"].ToString() == "2")//消费赠送
                        {
                            srzs.Style.Add("display", "none");
                            zdzs.Style.Add("display", "none");
                            hydj.Style.Add("display", "none");
                            hykh.Style.Add("display", "none");
                            hyxx.Style.Add("display", "none");
                            xfje.Style.Add("display", "");
                            zssm.Style.Add("display", "none");
                            sfzs.Style.Add("display", "none");

                        }
                        else if (ds.Rows[0]["Mode"].ToString() == "10")//指定赠送
                        {
                            srzs.Style.Add("display", "none");
                            zdzs.Style.Add("display", "");
                            hydj.Style.Add("display", "none");
                            hykh.Style.Add("display", "none");
                            hyxx.Style.Add("display", "none");
                            xfje.Style.Add("display", "none");
                            zssm.Style.Add("display", "none");
                            sfzs.Style.Add("display", "none");

                            if (ds.Rows[0]["MembergradeType"].ToString() == "1")
                            {
                                srzs.Style.Add("display", "none");
                                zdzs.Style.Add("display", "");
                                hydj.Style.Add("display", "none");
                                hykh.Style.Add("display", "none");
                                hyxx.Style.Add("display", "none");
                                xfje.Style.Add("display", "none");
                                zssm.Style.Add("display", "");
                                sfzs.Style.Add("display", "");


                            }
                            else if (ds.Rows[0]["MembergradeType"].ToString() == "2")
                            {
                                srzs.Style.Add("display", "none");
                                zdzs.Style.Add("display", "");
                                hydj.Style.Add("display", "");
                                hykh.Style.Add("display", "none");
                                hyxx.Style.Add("display", "none");
                                xfje.Style.Add("display", "none");
                                zssm.Style.Add("display", "");
                                sfzs.Style.Add("display", "");

                            }
                            else if (ds.Rows[0]["MembergradeType"].ToString() == "3")
                            {
                                srzs.Style.Add("display", "none");
                                zdzs.Style.Add("display", "");
                                hydj.Style.Add("display", "none");
                                hykh.Style.Add("display", "");
                                hyxx.Style.Add("display", "");
                                xfje.Style.Add("display", "none");
                                zssm.Style.Add("display", "");
                                sfzs.Style.Add("display", "");

                            }

                        }
                        else if (ds.Rows[0]["Mode"].ToString() == "15") //生日赠送
                        {
                            hdSendType.Value = ds.Rows[0]["BirthdaySendType"].ToString();
                            srzs.Style.Add("display", "");
                            zdzs.Style.Add("display", "none");
                            hydj.Style.Add("display", "none");
                            hykh.Style.Add("display", "none");
                            hyxx.Style.Add("display", "none");
                            xfje.Style.Add("display", "none");
                            zssm.Style.Add("display", "none");
                            sfzs.Style.Add("display", "none");
                        }
                        else//其他类型不可见
                        {
                            srzs.Style.Add("display", "none");
                            zdzs.Style.Add("display", "none");
                            hydj.Style.Add("display", "none");
                            hykh.Style.Add("display", "none");
                            hyxx.Style.Add("display", "none");
                            xfje.Style.Add("display", "none");
                            zssm.Style.Add("display", "none");
                            sfzs.Style.Add("display", "none");
                        }



                        //有效期类型
                        if (ds.Rows[0]["EffectiveType"] != null && ds.Rows[0]["EffectiveType"].ToString() != "")
                        {

                            hfEffectiveType.Value = _Effective = ds.Rows[0]["EffectiveType"].ToString();

                        }

                        //有效天数
                        if (ds.Rows[0]["EffectiveDay"] != null && ds.Rows[0]["EffectiveDay"].ToString() != "")
                        {
                            txtDay.Value = ds.Rows[0]["EffectiveDay"].ToString();
                        }


                        hfdID.Value = Request["id"];
                        IsMembergrade(ds.Rows[0]["Membergrade"].ToString());
                        IsyxMembergrade(ds.Rows[0]["ishy"].ToString());
                        Isjjr(ds.Rows[0]["jjr"].ToString());
                        IsOpen_Hotel(ds.Rows[0]["Open_Hotel"].ToString());
                        //hjjr.Value = ds.Rows[0]["jjr"].ToString();
                        hyxrq.Value = ds.Rows[0]["yxrq"].ToString();
                    }
                }
                else
                {
                    IsMembergrade("0");
                    IsyxMembergrade("0");
                    Isjjr("0");
                    IsOpen_Hotel("0");
                }


            }
        }




        public string loadaddMembergrade(string MembergradeId)
        {
            string[] Membergrade = MembergradeId.Split(',');
            string a = "";
            DataTable dtjb = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            if (dtjb != null && dtjb.Rows.Count > 0)
            {
                for (int k = 0; k < Membergrade.Length; k++)
                {
                    for (int i = 0; i < dtjb.Rows.Count; i++)
                    {
                        if (Membergrade[k].ToString() == dtjb.Rows[i]["code"].ToString())
                        {
                            a += dtjb.Rows[i]["code"].ToString() + ",";
                        }
                    }
                }
            }
            return a;
        }




        public void IsMembergrade(string MembergradeId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtjb = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            if (dtjb != null && dtjb.Rows.Count > 0)
            {
                for (int i = 0; i < dtjb.Rows.Count; i++)
                {
                    sb.Append(string.Format("<label class='{0}' val='{2}'>{1}</label>", loadMembergrade(dtjb.Rows[i]["code"].ToString(), MembergradeId.ToString()), dtjb.Rows[i]["LevelName"].ToString(), dtjb.Rows[i]["code"].ToString()));
                }
            }
            _Membergrade = sb.ToString();
        }

        public void IsyxMembergrade(string MembergradeId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtjb = CommonMethod.GetVip(RequestSession.GetSessionUser().AdminHotelid.ToString());
            if (dtjb != null && dtjb.Rows.Count > 0)
            {
                for (int i = 0; i < dtjb.Rows.Count; i++)
                {
                    sb.Append(string.Format("<label class='{0}' val='{2}'>{1}</label>", loadMembergrade(dtjb.Rows[i]["code"].ToString(), MembergradeId.ToString()), dtjb.Rows[i]["LevelName"].ToString(), dtjb.Rows[i]["code"].ToString()));
                }
            }
            _yxMembergrade = sb.ToString();
        }


        public void IsOpen_Hotel(string Open_Hotel)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT ID,name FROM dbo.Hotel WHERE type=1 AND DeleteMark=1 AND AdminHotelid='{0}'", RequestSession.GetSessionUser().AdminHotelid.ToString());
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append(string.Format("<label class='{0}' val='{2}'>{1}</label>", loadOpen_Hotel(dt.Rows[i]["ID"].ToString(), Open_Hotel.ToString()), dt.Rows[i]["name"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
            }
            _Open_Hotel = sb.ToString();
        }

        public void Isjjr(string jjr)
        {
            StringBuilder html = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT year,COUNT(year) count FROM Festival WHERE AdminHotelid='{0}' GROUP BY year ", RequestSession.GetSessionUser().AdminHotelid.ToString());
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.AppendFormat("<li class='clearfix'>");
                    html.AppendFormat("<span'>{0}年</span>", dt.Rows[i][0].ToString());
                    html.AppendFormat("<p class='checkbox cb2'  style='display: block;'>");
                    if (CommonHelper.GetInt(dt.Rows[i][1]) > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("SELECT * FROM  dbo.Festival WHERE year={0} and AdminHotelid='{1}' ORDER BY year ASC", dt.Rows[i][0].ToString(), RequestSession.GetSessionUser().AdminHotelid.ToString());
                        DataTable dtjr = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
                        if (dtjr != null && dtjr.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtjr.Rows.Count; j++)
                            {
                                html.AppendFormat("<label val='{0}' class='{2}' >{1}</label>", dtjr.Rows[j]["id"].ToString(), dtjr.Rows[j]["name"].ToString(), loadjjr(dtjr.Rows[j]["id"].ToString(), jjr));
                            }
                        }

                    }
                    html.AppendFormat("</p></li>");
                }
            }
            _jjr = html.ToString();
        }

        string loadjjr(string id, string jjr)
        {
            if (jjr == "0")
            {
                return "checked";
            }
            string[] jjrs = jjr.Split(',');

            for (int j = 0; j < jjrs.Length; j++)
            {
                if (id.ToString() == jjrs[j].ToString())
                {
                    return "checked";
                }
                else
                {
                    //return "";
                }
            }
            return "";
        }

        string loadMembergrade(string wxdj, string MembergradeId)
        {
            if (MembergradeId == "0")
            {
                return "checked";
            }
            string[] Membergrade = MembergradeId.Split(',');

            for (int j = 0; j < Membergrade.Length; j++)
            {
                if (wxdj.ToString() == Membergrade[j].ToString())
                {
                    return "checked";
                }
                else
                {
                    //return "";
                }
            }
            return "";
        }


        string loadOpen_Hotel(string ID, string Open_Hotel)
        {
            if (Open_Hotel == "0" || Open_Hotel == "")
            {
                return "checked";
            }
            string[] Hotel = Open_Hotel.Split(',');

            for (int j = 0; j < Hotel.Length; j++)
            {
                if (ID.ToString() == Hotel[j].ToString())
                {
                    return "checked";
                }
                else
                {
                    //return "";
                }
            }
            return "";
        }

        public void Gift(int id)
        {
            if (DDLmode.Items[DDLmode.SelectedIndex].Text == "指定赠送")
            {
                try
                {
                    string URI = "http://www.zidinn.com/SysSetBase/coupons/GetCouponList.ashx";
                    //string URI = "http://localhost:520/SysSetBase/coupons/GetCouponList.ashx";
                    string myParameters = "id=" + id + "&Menu=GiftCoupons&AdminHotelid=" + RequestSession.GetSessionUser().AdminHotelid.ToString();
                    //实例化
                    WebClient client = new WebClient();
                    //  client.UseDefaultCredentials = true;
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    client.Encoding = Encoding.UTF8;
                    client.UploadString(URI, myParameters);
                }
                catch (Exception ex)
                {
                    Log.Error("指定赠送错误", ex.Message);
                }
            }


        }


        /// <summary>
        /// 添加，修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumit_Click(object sender, EventArgs e)
        {
            string BiginTime, EndinTime = null;
            if (hfEffectiveType.Value == "1")
            {
                BiginTime = DateTime.Now.ToString("yyyy-MM-dd");
                EndinTime = DateTime.Now.AddYears(100).ToString("yyyy-MM-dd");
            }
            else if (hfEffectiveType.Value == "2")
            {
                BiginTime = DateTime.Now.ToString("yyyy-MM-dd");
                EndinTime = DateTime.Now.AddDays(Convert.ToDouble(txtDay.Value)).ToString("yyyy-MM-dd");
            }
            else
            {
                BiginTime = B_sj.Value;
                EndinTime = E_sj.Value;
            }

            Hashtable hs = new Hashtable();
            hs["CouponName"] = txtName.Value;
            hs["Par"] = Convert.ToInt32(txtPar.Value);
            hs["UsedMin"] = Convert.ToInt32(txtUsedMin.Value);
            hs["BiginTime"] = BiginTime;
            hs["EndinTime"] = EndinTime;
            hs["ServiceRange"] = sumhotel.Value;
            hs["ProductRange"] = txtInstructions.Value;
            hs["Mode"] = Convert.ToInt32(DDLmode.SelectedValue);

            hs["Membergrade"] = loadaddMembergrade(labelhtml.Value.ToString());
            hs["FirstMoney"] = txtFirstMoney.Value;
            hs["SecondMoney"] = txtcz.Value;
            hs["IsEnable"] = hfsfqy.Value;
            hs["Isgiving"] = hfsfzs.Value;
            hs["hytype"] = hhytype.Value;
            hs["EffectiveType"] = hfEffectiveType.Value;
            hs["EffectiveDay"] = txtDay.Value;
            hs["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
            hs["yxrq"] = hyxrq.Value;
            hs["jjr"] = hjjr.Value;
            hs["count"] = txtNum.Value;
            hs["ishy"] = hishy.Value;
            hs["Is_Day_ok"] = hfIs_Day_ok.Value;
            hs["Total"] = txtTotal.Value;
            hs["Open_Hotel"] = HOpen_Hotel.Value;
            hs["ConsumptionType"] = HConsumptionType.Value;

            hs["TypeID"] = ddltype.SelectedValue;

            hs["BirthdaySendType"] = hdSendType.Value;
            

            if (hfdID.Value == "0")
            {
                //添加
                int x = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("Coupon", hs);
                if (x <= 0)
                {
                    ShowMsgHelper.Alert_Error("添加失败！");
                }
                else
                {
                    if (hfsfzs.Value == "1") { Gift(x); }//赠送优惠券
                    CommonMethod.Base_Log("添加", "Coupon", "", "优惠券管理", "添加[" + txtName.Value + "]");//操作日志
                    ShowMsgHelper.AlertReloadClose("添加成功！", "ListGrid()");
                }
            }
            else
            {
                //修改

                if (DataFactory.SqlDataBase().UpdateByHashtable("Coupon", "ID", hfdID.Value, hs) <= 0)
                {
                    ShowMsgHelper.Alert_Error("修改失败！");
                }
                else
                {
                    if (hfsfzs.Value == "1") { Gift(Convert.ToInt32(hfdID.Value)); }//赠送优惠券

                    CommonMethod.Base_Log("修改", "Coupon", hfdID.Value, "优惠券管理", "修改[" + txtName.Value + "]");//操作日志

                    ShowMsgHelper.AlertReloadClose("修改成功！", "ListGrid()");
                }
            }
        }
    }
}