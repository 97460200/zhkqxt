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
using RM.Common.DotPqGrid;
using LitJson;
using Newtonsoft.Json;

namespace RM.Web.SysSetBase.superAdmin
{
    /// <summary>
    /// MarketingConfigure 的摘要说明
    /// </summary>
    public class MarketingConfigure : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Action = context.Request["action"].Trim();               //提交动作
            switch (Action)
            {
                case "getinfo":
                    getinfo(context);
                    break;
                case "getfoodinfo":
                    getfoodinfo(context);
                    break;

                case "getproductinfo":
                    getproductinfo(context);
                    break;

                case "update":
                    update(context);
                    break;
                case "GetRoomList":
                    GetRoomList(context);
                    break;
                case "GetProductList":
                    GetProductList(context);
                    break;
                case "SaveMoney":
                    SaveMoney(context);
                    break;

                case "getpublicinfo":
                    getpublicinfo(context);
                    break;
                case "GetpublicRoomList":
                    GetpublicRoomList(context);
                    break;
                case "publicSaveMoney":
                    publicSaveMoney(context);
                    break;
                case "SaveProductMoney":
                    SaveProductMoney(context);
                    break;
                default:
                    break;
            }

        }


        private void GetProductList(HttpContext c)
        {
            try
            {
                string hotelid = c.Request["HotelId"];
                JsonData jsondata = new JsonData();
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
                SELECT  id ,
                        Name ,
                        CASE WHEN PromoteMoney < 0
                             THEN ( SELECT TOP 1
                                            MallProductMoney
                                    FROM    dbo.ProductMarketingConfigure
                                    WHERE   ProductMarketingConfigure.AdminHotelId = Product.AdminHotelid
                                  )
                             ELSE PromoteMoney
                        END PromoteMoney ,
                      
                        CASE WHEN PromoteProportion < 0
                             THEN ( SELECT TOP 1
                                            MallProductProportion
                                    FROM    dbo.ProductMarketingConfigure
                                    WHERE   ProductMarketingConfigure.AdminHotelId = Product.AdminHotelid
                                  )
                             ELSE PromoteProportion
                        END PromoteProportion
                       
                FROM    dbo.Product  WHERE DeleteMark=1 and  HotelID =@hotelid
                ORDER BY Sort DESC
                ");
                List<SqlParam> ilistStr = new List<SqlParam>();
                ilistStr.Add(new SqlParam("@hotelid", hotelid));
                SqlParam[] param = ilistStr.ToArray();
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (ds != null && ds.Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        string product_ID = ds.Rows[i]["id"].ToString();
                        JsonData json1 = new JsonData();
                        json1["Id"] = product_ID;
                        json1["Name"] = ds.Rows[i]["name"].ToString();
                        json1["PromoteMoney"] = ds.Rows[i]["PromoteMoney"].ToString();
                        json1["PromoteProportion"] = ds.Rows[i]["PromoteProportion"].ToString();
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
        /// <summary>
        /// 保存默认价格
        /// </summary>
        /// <param name="c"></param>
        private void SaveProductMoney(HttpContext c)
        {
            try
            {
                string JsonDate = c.Request["JsonDate"];
                List<Hashtable> jd = JsonConvert.DeserializeObject<List<Hashtable>>(JsonDate);
                if (jd != null && jd.Count > 0)
                {
                    for (int i = 0; i < jd.Count; i++)
                    {
                        Hashtable ht = new Hashtable();
                        string productId = jd[i]["ProductId"].ToString();
                        ht["PromoteMoney"] = jd[i]["PromoteMoney"].ToString();
                        ht["PromoteProportion"] = jd[i]["PromoteProportion"].ToString();
                        int uc = DataFactory.SqlDataBase().UpdateByHashtable("Product", "id", productId, ht);
                        if (uc < 0)
                        {
                            c.Response.Write("-1");
                            return;
                        }
                    }
                }
                c.Response.Write("1");
            }
            catch (Exception)
            {
                c.Response.Write("");
            }
        }

        private void GetRoomList(HttpContext c)
        {
            try
            {
                string hotelid = c.Request["HotelId"];
                JsonData jsondata = new JsonData();
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
                SELECT  id ,
                        Name ,
                        CASE WHEN SalesMoney < 0
                             THEN ( SELECT TOP 1
                                            CheckInMoney
                                    FROM    dbo.MarketingConfigure
                                    WHERE   MarketingConfigure.AdminHotelId = Guestroom.AdminHotelid
                                  )
                             ELSE SalesMoney
                        END SalesMoney ,
                        CASE WHEN SalesProportion < 0
                             THEN ( SELECT TOP 1
                                            CheckInProportion
                                    FROM    dbo.MarketingConfigure
                                    WHERE   MarketingConfigure.AdminHotelId = Guestroom.AdminHotelid
                                  )
                             ELSE SalesProportion
                        END SalesProportion,
                        CASE WHEN SalesTwoMoney < 0
                             THEN ( SELECT TOP 1
                                            CheckInTwoMoney
                                    FROM    dbo.MarketingConfigure
                                    WHERE   MarketingConfigure.AdminHotelId = Guestroom.AdminHotelid
                                  )
                             ELSE SalesTwoMoney
                        END SalesTwoMoney ,
                        CASE WHEN SalesTwoProportion < 0
                             THEN ( SELECT TOP 1
                                            CheckInTwoProportion
                                    FROM    dbo.MarketingConfigure
                                    WHERE   MarketingConfigure.AdminHotelId = Guestroom.AdminHotelid
                                  )
                             ELSE SalesTwoProportion
                        END SalesTwoProportion
                FROM    Guestroom
                WHERE   HotelID = @hotelid
                ORDER BY Sort DESC 
                ");
                List<SqlParam> ilistStr = new List<SqlParam>();
                ilistStr.Add(new SqlParam("@hotelid", hotelid));
                SqlParam[] param = ilistStr.ToArray();
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (ds != null && ds.Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        string room_ID = ds.Rows[i]["id"].ToString();
                        JsonData json1 = new JsonData();
                        json1["Id"] = room_ID;
                        json1["Name"] = ds.Rows[i]["name"].ToString();
                        json1["SalesMoney"] = ds.Rows[i]["SalesMoney"].ToString();
                        json1["SalesProportion"] = ds.Rows[i]["SalesProportion"].ToString();
                        json1["SalesTwoMoney"] = ds.Rows[i]["SalesTwoMoney"].ToString();
                        json1["SalesTwoProportion"] = ds.Rows[i]["SalesTwoProportion"].ToString();
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
        /// <summary>
        /// 保存默认价格
        /// </summary>
        /// <param name="c"></param>
        private void SaveMoney(HttpContext c)
        {
            try
            {
                string JsonDate = c.Request["JsonDate"];
                List<Hashtable> jd = JsonConvert.DeserializeObject<List<Hashtable>>(JsonDate);
                if (jd != null && jd.Count > 0)
                {
                    for (int i = 0; i < jd.Count; i++)
                    {
                        Hashtable ht = new Hashtable();
                        string roomId = jd[i]["RoomId"].ToString();
                        ht["SalesMoney"] = jd[i]["SalesMoney"].ToString();
                        ht["SalesProportion"] = jd[i]["SalesProportion"].ToString();
                        ht["SalesTwoMoney"] = jd[i]["SalesTwoMoney"].ToString();
                        ht["SalesTwoProportion"] = jd[i]["SalesTwoProportion"].ToString();
                        int uc = DataFactory.SqlDataBase().UpdateByHashtable("Guestroom", "id", roomId, ht);
                        if (uc < 0)
                        {
                            c.Response.Write("-1");
                            return;
                        }
                    }
                }
                c.Response.Write("1");
            }
            catch (Exception)
            {
                c.Response.Write("");
            }
        }


        public void getinfo(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string sql = string.Format(@"SELECT  *  FROM  MarketingConfigure WHERE AdminHotelid=@AdminHotelid  ");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            string dtName = "MarketingConfigure";
            string json = JsonHelper.DataTableToJson(dt, dtName);
            context.Response.Write(json);

        }

        public void getfoodinfo(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string sql = string.Format(@"SELECT  *  FROM  FoodMarketingConfigure WHERE AdminHotelid=@AdminHotelid  ");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            string dtName = "FoodMarketingConfigure";
            string json = JsonHelper.DataTableToJson(dt, dtName);
            context.Response.Write(json);

        }


        public void getpublicinfo(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string sql = string.Format(@"SELECT  *, ISNULL((SELECT TOP 1 User_ID FROM dbo.Base_UserInfo WHERE PublicWithdrawal=1 AND AdminHotelid=Public_MarketingConfigure.AdminHotelId),'0') AS PublicWithdrawal   FROM  Public_MarketingConfigure WHERE AdminHotelid=@AdminHotelid  ");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            string dtName = "Public_MarketingConfigure";
            string json = JsonHelper.DataTableToJson(dt, dtName);
            context.Response.Write(json);

        }


        public void getproductinfo(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string sql = string.Format(@"SELECT  *  FROM  ProductMarketingConfigure WHERE AdminHotelid=@AdminHotelid  ");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);
            string dtName = "ProductMarketingConfigure";
            string json = JsonHelper.DataTableToJson(dt, dtName);
            context.Response.Write(json);

        }
        


        public void update(HttpContext context)
        {
            string AdminHotelid = context.Request["AdminHotelid"];
            string Maintain = context.Request["Maintain"];
            string MaintainMoney = context.Request["MaintainMoney"];
            string MaintainProportion = context.Request["MaintainProportion"];
            string RegisterMoney = context.Request["RegisterMoney"];
            string CheckIn = context.Request["CheckIn"];
            string CheckInMoney = context.Request["CheckInMoney"];
            string CheckInProportion = context.Request["CheckInProportion"];
            string CheckInTwo = context.Request["CheckInTwo"];
            string CheckInTwoMoney = context.Request["CheckInTwoMoney"];
            string CheckInTwoProportion = context.Request["CheckInTwoProportion"];
            string Recharge = context.Request["Recharge"];
            string RechargeMoney = context.Request["RechargeMoney"];
            string RechargeProportion = context.Request["RechargeProportion"];
            string Upgrade = context.Request["Upgrade"];
            string UpgradeMoney = context.Request["UpgradeMoney"];
            string UpgradeProportion = context.Request["UpgradeProportion"];
            string MinPayMoney = context.Request["MinPayMoney"];
            string CheckTwoStandard = context.Request["CheckTwoStandard"];

            string sql = string.Format(@"SELECT ID FROM  MarketingConfigure WHERE AdminHotelid=@AdminHotelid  ");
            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",AdminHotelid.ToString())};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql), parmAdd);

            string id = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                id = dt.Rows[0]["ID"].ToString();
            }
            Hashtable hs = new Hashtable();
            hs["AdminHotelid"] = AdminHotelid;
            hs["Maintain"] = Maintain;
            hs["MaintainMoney"] = MaintainMoney;
            hs["MaintainProportion"] = MaintainProportion;
            hs["RegisterMoney"] = RegisterMoney;
            hs["CheckIn"] = CheckIn;
            hs["CheckInMoney"] = CheckInMoney;
            hs["CheckInProportion"] = CheckInProportion;

            hs["CheckTwoStandard"] = CheckTwoStandard;
            hs["CheckInTwo"] = CheckInTwo;
            hs["CheckInTwoMoney"] = CheckInTwoMoney;
            hs["CheckInTwoProportion"] = CheckInTwoProportion;
            hs["Recharge"] = Recharge;
            hs["RechargeMoney"] = RechargeMoney;
            hs["RechargeProportion"] = RechargeProportion;
            hs["Upgrade"] = Upgrade;
            hs["UpgradeMoney"] = UpgradeMoney;
            hs["UpgradeProportion"] = UpgradeProportion;
            hs["MinPayMoney"] = MinPayMoney;
            bool i = DataFactory.SqlDataBase().Submit_AddOrEdit("MarketingConfigure", "ID", id, hs);


            //设置点餐参数
            string StaffFood = context.Request["StaffFood"];
            string StaffFoodMoney = context.Request["StaffFoodMoney"];
            string StaffFoodProportion = context.Request["StaffFoodProportion"];
            string StaffMaintain = context.Request["StaffMaintain"];
            string StaffMaintainMoney = context.Request["StaffMaintainMoney"];
            string StaffMaintainProportion = context.Request["StaffMaintainProportion"];
            string TableFood = context.Request["TableFood"];
            string TableFoodMoney = context.Request["TableFoodMoney"];
            string TableFoodProportion = context.Request["TableFoodProportion"];
            string TableMaintain = context.Request["TableMaintain"];
            string TableWinning = context.Request["TableWinning"];
            string RoomFood = context.Request["RoomFood"];
            string RoomFoodMoney = context.Request["RoomFoodMoney"];
            string RoomFoodProportion = context.Request["RoomFoodProportion"];
            string RoomMaintain = context.Request["RoomMaintain"];
            string RoomWinning = context.Request["RoomWinning"];
            string GuestFood = context.Request["GuestFood"];
            string GuestFoodMoney = context.Request["GuestFoodMoney"];
            string GuestFoodProportion = context.Request["GuestFoodProportion"];
            string GuestMaintain = context.Request["GuestMaintain"];

            string sqlj = string.Format(@"SELECT ID FROM  FoodMarketingConfigure WHERE AdminHotelid=@AdminHotelid  ");
            SqlParam[] parmAddj = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",AdminHotelid.ToString())};
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlj), parmAddj);

            string dsj = "";
            if (ds != null && ds.Rows.Count > 0)
            {
                dsj = ds.Rows[0]["ID"].ToString();
            }
            Hashtable ht = new Hashtable();
            ht["AdminHotelid"] = AdminHotelid;
            ht["StaffFood"] = StaffFood;
            ht["StaffFoodMoney"] = StaffFoodMoney;
            ht["StaffFoodProportion"] = StaffFoodProportion;
            ht["StaffMaintain"] = StaffMaintain;
            ht["StaffMaintainMoney"] = StaffMaintainMoney;
            ht["StaffMaintainProportion"] = StaffMaintainProportion;
            ht["TableFood"] = TableFood;
            ht["TableFoodMoney"] = TableFoodMoney;
            ht["TableFoodProportion"] = TableFoodProportion;
            ht["TableMaintain"] = TableMaintain;
            ht["TableWinning"] = TableWinning;
            ht["RoomFood"] = RoomFood;
            ht["RoomFoodMoney"] = RoomFoodMoney;
            ht["RoomFoodProportion"] = RoomFoodProportion;
            ht["RoomMaintain"] = RoomMaintain;
            ht["RoomWinning"] = RoomWinning;
            ht["GuestFood"] = GuestFood;
            ht["GuestFoodMoney"] = GuestFoodMoney;
            ht["GuestFoodProportion"] = GuestFoodProportion;
            ht["GuestMaintain"] = GuestMaintain;
            bool j = DataFactory.SqlDataBase().Submit_AddOrEdit("FoodMarketingConfigure", "ID", dsj, ht);


            //公共奖金
            string PublicRegisterMoney = context.Request["PublicRegisterMoney"];
            string PublicCheckIn = context.Request["PublicCheckIn"];
            string PublicCheckInMoney = context.Request["PublicCheckInMoney"];
            string PublicCheckInProportion = context.Request["PublicCheckInProportion"];
            string PublicCheckInTwo = context.Request["PublicCheckInTwo"];
            string PublicCheckInTwoMoney = context.Request["PublicCheckInTwoMoney"];
            string PublicCheckInTwoProportion = context.Request["PublicCheckInTwoProportion"];
            string PublicRecharge = context.Request["PublicRecharge"];
            string PublicRechargeMoney = context.Request["PublicRechargeMoney"];
            string PublicRechargeProportion = context.Request["PublicRechargeProportion"];
            string PublicUpgrade = context.Request["PublicUpgrade"];
            string PublicUpgradeMoney = context.Request["PublicUpgradeMoney"];
            string PublicUpgradeProportion = context.Request["PublicUpgradeProportion"];
            string PublicStaffFood = context.Request["PublicStaffFood"];
            string PublicStaffFoodMoney = context.Request["PublicStaffFoodMoney"];
            string PublicStaffFoodProportion = context.Request["PublicStaffFoodProportion"];
            string PublicUser = context.Request["PublicUser"];


            string sqlss = string.Format(@"SELECT ID FROM  Public_MarketingConfigure WHERE AdminHotelid=@AdminHotelid  ");
            SqlParam[] parmAddss = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",AdminHotelid.ToString())};
            DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlss), parmAddss);

            string idss = "";
            if (dts != null && dts.Rows.Count > 0){
                idss = dts.Rows[0]["ID"].ToString();
            }
            Hashtable hss = new Hashtable();
            hss["AdminHotelid"] = AdminHotelid;
           
            hss["RegisterMoney"] = PublicRegisterMoney;
            hss["CheckIn"] = PublicCheckIn;
            hss["CheckInMoney"] = PublicCheckInMoney;
            hss["CheckInProportion"] = PublicCheckInProportion;
            hss["CheckInTwo"] = PublicCheckInTwo;
            hss["CheckInTwoMoney"] = PublicCheckInTwoMoney;
            hss["CheckInTwoProportion"] = PublicCheckInTwoProportion;
            hss["Recharge"] = PublicRecharge;
            hss["RechargeMoney"] = PublicRechargeMoney;
            hss["RechargeProportion"] = PublicRechargeProportion;
            hss["Upgrade"] = PublicUpgrade;
            hss["UpgradeMoney"] = PublicUpgradeMoney;
            hss["UpgradeProportion"] = PublicUpgradeProportion;
            hss["StaffFood"] = PublicStaffFood;
            hss["StaffFoodMoney"] = PublicStaffFoodMoney;
            hss["StaffFoodProportion"] = PublicStaffFoodProportion;
            bool z = DataFactory.SqlDataBase().Submit_AddOrEdit("Public_MarketingConfigure", "ID", idss, hss);
            bool y = SetPublic(PublicUser);


            //设置商城参数
            string MallProduct = context.Request["MallProduct"];
            string MallProductMoney = context.Request["MallProductMoney"];
            string MallProductProportion = context.Request["MallProductProportion"];
            string MallMaintain = context.Request["MallMaintain"];
            string MallMaintainMoney = context.Request["MallMaintainMoney"];
            string MallMaintainProportion = context.Request["MallMaintainProportion"];

            string sqlk = string.Format(@"SELECT ID FROM  ProductMarketingConfigure WHERE AdminHotelid=@AdminHotelid  ");
            SqlParam[] parmAddk = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",AdminHotelid.ToString())};
            DataTable dk = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlk), parmAddk);

            string dsk = "";
            if (dk != null && dk.Rows.Count > 0)
            {
                dsk = dk.Rows[0]["ID"].ToString();
                
            }

            Hashtable hk = new Hashtable();
            hk["AdminHotelid"] = AdminHotelid;
            hk["MallProduct"] = MallProduct;
            hk["MallProductMoney"] = MallProductMoney;
            hk["MallProductProportion"] = MallProductProportion;

            hk["MallMaintain"] = MallMaintain;
            hk["MallMaintainMoney"] = MallMaintainMoney;
            hk["MallMaintainProportion"] = MallMaintainProportion;

            bool k = DataFactory.SqlDataBase().Submit_AddOrEdit("ProductMarketingConfigure", "ID", dsk, hk);

            if (i == true && j == true && k == true && z == true && y == true)
            {
                context.Response.Write("ok");
            }
            else
            {
                context.Response.Write("error");
            }
        }

        private bool SetPublic(string PublicUser) 
        {
            bool biaoshi = false;
            if (PublicUser != "0" && PublicUser.Length > 4)
            {
                Hashtable ht = new Hashtable();
                ht["PublicWithdrawal"] = 1;

                int x = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", PublicUser, ht);
                if (x > 0)
                {
                    biaoshi = true;
                    StringBuilder sql = new StringBuilder();
                    sql.AppendFormat("UPDATE Base_UserInfo SET PublicWithdrawal=0 WHERE AdminHotelid=(SELECT AdminHotelid FROM Base_UserInfo WHERE User_ID='{0}') AND Base_UserInfo.User_ID<>'{0}' AND Base_UserInfo.IsAdmin=2 AND Base_UserInfo.DeleteMark=1 ", PublicUser);
                    DataFactory.SqlDataBase().ExecuteBySql(sql);
                }
            }
            else 
            {
                biaoshi = true;
            }

            return biaoshi;
        }


        private void GetpublicRoomList(HttpContext c)
        {
            try
            {
                string hotelid = c.Request["HotelId"];
                JsonData jsondata = new JsonData();
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
                SELECT  id ,
                        Name ,
                        CASE WHEN PublicSalesMoney < 0
                             THEN ( SELECT TOP 1
                                            CheckInMoney
                                    FROM    dbo.Public_MarketingConfigure
                                    WHERE   Public_MarketingConfigure.AdminHotelId = Guestroom.AdminHotelid
                                  )
                             ELSE PublicSalesMoney
                        END PublicSalesMoney ,
                        CASE WHEN PublicSalesProportion < 0
                             THEN ( SELECT TOP 1
                                            CheckInProportion
                                    FROM    dbo.Public_MarketingConfigure
                                    WHERE   Public_MarketingConfigure.AdminHotelId = Guestroom.AdminHotelid
                                  )
                             ELSE PublicSalesProportion
                        END PublicSalesProportion,
                        CASE WHEN PublicSalesTwoMoney < 0
                             THEN ( SELECT TOP 1
                                            CheckInTwoMoney
                                    FROM    dbo.Public_MarketingConfigure
                                    WHERE   Public_MarketingConfigure.AdminHotelId = Guestroom.AdminHotelid
                                  )
                             ELSE PublicSalesTwoMoney
                        END PublicSalesTwoMoney ,
                        CASE WHEN PublicSalesTwoProportion < 0
                             THEN ( SELECT TOP 1
                                            CheckInTwoProportion
                                    FROM    dbo.Public_MarketingConfigure
                                    WHERE   Public_MarketingConfigure.AdminHotelId = Guestroom.AdminHotelid
                                  )
                             ELSE PublicSalesTwoProportion
                        END PublicSalesTwoProportion
                FROM    Guestroom
                WHERE   HotelID = @hotelid
                ORDER BY Sort DESC 
                ");
                List<SqlParam> ilistStr = new List<SqlParam>();
                ilistStr.Add(new SqlParam("@hotelid", hotelid));
                SqlParam[] param = ilistStr.ToArray();
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (ds != null && ds.Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        string room_ID = ds.Rows[i]["id"].ToString();
                        JsonData json1 = new JsonData();
                        json1["Id"] = room_ID;
                        json1["Name"] = ds.Rows[i]["name"].ToString();
                        json1["SalesMoney"] = ds.Rows[i]["PublicSalesMoney"].ToString();
                        json1["SalesProportion"] = ds.Rows[i]["PublicSalesProportion"].ToString();
                        json1["SalesTwoMoney"] = ds.Rows[i]["PublicSalesTwoMoney"].ToString();
                        json1["SalesTwoProportion"] = ds.Rows[i]["PublicSalesTwoProportion"].ToString();
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
        /// <summary>
        /// 保存默认价格
        /// </summary>
        /// <param name="c"></param>
        private void publicSaveMoney(HttpContext c)
        {
            try
            {
                string JsonDate = c.Request["JsonDate"];
                List<Hashtable> jd = JsonConvert.DeserializeObject<List<Hashtable>>(JsonDate);
                if (jd != null && jd.Count > 0)
                {
                    for (int i = 0; i < jd.Count; i++)
                    {
                        Hashtable ht = new Hashtable();
                        string roomId = jd[i]["RoomId"].ToString();
                        ht["PublicSalesMoney"] = jd[i]["SalesMoney"].ToString();
                        ht["PublicSalesProportion"] = jd[i]["SalesProportion"].ToString();
                        ht["PublicSalesTwoMoney"] = jd[i]["SalesTwoMoney"].ToString();
                        ht["PublicSalesTwoProportion"] = jd[i]["SalesTwoProportion"].ToString();
                        int uc = DataFactory.SqlDataBase().UpdateByHashtable("Guestroom", "id", roomId, ht);
                        if (uc < 0)
                        {
                            c.Response.Write("-1");
                            return;
                        }
                    }
                }
                c.Response.Write("1");
            }
            catch (Exception)
            {
                c.Response.Write("");
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