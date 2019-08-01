using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQL; 
using System.Data;
using RM.Busines;
using System.Text;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using System.Collections;

namespace RM.Web.RMBase.SysSetBase.sales
{
    public partial class MemberDetails : System.Web.UI.Page
    {
        public string ae;
        public string kh;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    ae = Request["id"];

                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT User_ID,User_Account,User_Name,WX_Nickname,Roles_Name,CreateDate FROM V_Base_UserInfoRole   WHERE  DeleteMark = 1  and AdminHotelid = '1'  and hotelid = '18'");
                     
                    DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
                    if (ds != null && ds.Rows.Count > 0)
                    {
                        hdUser_ID.Value = ds.Rows[0]["User_ID"].ToString();
                        lblPhone.Text = ds.Rows[0]["User_Account"].ToString();
                        lblWXName.Text = ds.Rows[0]["WX_Nickname"].ToString();
                        lblRolse.Text = ds.Rows[0]["Roles_Name"].ToString();
                        lblAddTime.Text =Convert.ToDateTime( ds.Rows[0]["CreateDate"].ToString()).ToString("yyyy-MM-dd hh:mm"); 
                         
                        //获取统计数据  
                        string memberid = "";// --获取带来客户
                        StringBuilder sqlKeHu = new StringBuilder();
                        sqlKeHu.AppendFormat(@"SELECT * FROM dbo.hy_hyzlxxb WHERE sjhm='" + ds.Rows[0]["User_Account"].ToString() + "'");
                        DataTable dtKeHu = DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).GetDataTableBySQL(sqlKeHu);
                        if (dtKeHu != null && dtKeHu.Rows.Count > 0)
                        {
                            memberid = dtKeHu.Rows[0]["lsh"].ToString();
                            GetTongJiData(memberid, ds.Rows[0]["User_Account"].ToString());
                        }
                     
                    }
                }

            }
        }


        private void GetTongJiData(string memberid, string sjhm)
        { 
            // --获取客房销售、奖金
            StringBuilder sqlJiangJin = new StringBuilder();
            sqlJiangJin.AppendFormat(@"SELECT ISNULL(SUM(CAST(Sales_Amount AS MONEY)),0) AS KeFangMoney,ISNULL(SUM(CAST(money AS MONEY)),0) AS JiangJin  FROM Distribution_Finance WHERE memberid='" + memberid + "'");
            DataTable dtJiangJin = DataFactory.SqlDataBase().GetDataTableBySQL(sqlJiangJin);
           lblKeFang.Text="￥"+Convert.ToDecimal(dtJiangJin.Rows[0]["KeFangMoney"].ToString());//客房销售
           lblJiangJin.Text = "￥"  +Convert.ToDecimal(dtJiangJin.Rows[0]["JiangJin"].ToString());//奖金

            // --获取提现奖金、剩余奖金
            StringBuilder sqlTiXian = new StringBuilder();
            sqlTiXian.AppendFormat(@"SELECT ISNULL(SUM(CAST(money AS MONEY)),0) AS TiXianMoney FROM Sales_withdraw WHERE sjhm='" + sjhm + "'");
            DataTable dtTiXian = DataFactory.SqlDataBase().GetDataTableBySQL(sqlTiXian);
            lblTiXian.Text = "￥" +Convert.ToDecimal(dtTiXian.Rows[0]["TiXianMoney"].ToString());//提现奖金
            lblShengYu.Text = "￥" + (Convert.ToDecimal(dtJiangJin.Rows[0]["JiangJin"].ToString()) - Convert.ToDecimal(dtTiXian.Rows[0]["TiXianMoney"].ToString())).ToString();//剩余奖金

            // --获取带来客户
            StringBuilder sqlKeHu = new StringBuilder();
            sqlKeHu.AppendFormat(@"SELECT COUNT(*) AS DaiLaikehu FROM dbo.hy_hyzlxxb WHERE par_uid='" + memberid + "'");
            DataTable dtKeHu = DataFactory.SqlDataBase(RequestSession.GetSessionUser().AdminHotelid.ToString()).GetDataTableBySQL(sqlKeHu);
           lblDaiLai.Text=dtKeHu.Rows[0]["DaiLaikehu"].ToString()+"个";//带来客户

     
        }

    }
}