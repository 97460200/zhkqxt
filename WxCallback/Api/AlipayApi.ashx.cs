using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections;
using RM.Busines;
using Aop.Api;
using Alipay;
using Aop.Api.Request;
using Aop.Api.Response;
using System.Net;
using System.IO;
using System.Text;
using RM.Common.DotNetFile;
using RM.Common.DotNetCode;
using System.Data;

namespace WxCallback.Api
{
    /// <summary>
    /// AlipayApi 的摘要说明
    /// </summary>
    public class AlipayApi : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"].ToString();
            switch (action)
            {
                case "downloadurl":
                    downloadurl(context);
                    break;
                default:
                    break;
            }
        }

        #region

        private void downloadurl(HttpContext context)
        {
            if (DateTime.Now.Hour < 10)
            {
                return;
            }
            DateTime bill_date = DateTime.Now.AddDays(-1);
            if (!string.IsNullOrEmpty(context.Request["bill_date"]))
            {
                bill_date = CommonHelper.GetDateTime(context.Request["bill_date"]); //指定日期
            }
            else
            {
                StringBuilder sb_ck = new StringBuilder();
                sb_ck.Append(@"
                    SELECT TOP 1 Bill_Date
                    FROM    dbo.Hotel_Bill_Total
                    WHERE Bill_Date = @Bill_Date AND PaySource = 2 
                    ");
                SqlParam[] param_ck = new SqlParam[] { 
                           new SqlParam("@Bill_Date", bill_date.ToString("yyyy-MM-dd"))
                    };
                DataTable dt_ck = DataFactory.SqlDataBase().GetDataTableBySQL(sb_ck, param_ck);
                if (dt_ck != null && dt_ck.Rows.Count > 0)
                {
                    return;
                }
            }

            string app_id = Config.appId;
            string serverUrl = Config.serverUrl;
            string merchant_private_key = Config.merchant_private_key;
            string format = "json";
            string version = Config.version;
            string signType = Config.signtype;
            string alipay_public_key = Config.alipay_public_key;
            string charset = Config.charset;
            IAopClient client = new DefaultAopClient(serverUrl, app_id, merchant_private_key, format, version, signType, alipay_public_key, charset, false);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT  *
            FROM    dbo.Alipay_PlatformUser
            WHERE   HotelId > 0
            ");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);

            AlipayDataDataserviceBillDownloadurlQueryRequest request = new AlipayDataDataserviceBillDownloadurlQueryRequest();
            request.BizContent = "{" +
            "\"bill_type\":\"trade\"," +
            "\"bill_date\":\"" + bill_date.ToString("yyyy-MM-dd") + "\"" +
            "  }";
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string AdminHotelid = dt.Rows[i]["AdminHotelid"].ToString(); //集团ID
                    int hotelid = CommonHelper.GetInt(dt.Rows[i]["hotelid"]); //酒店ID
                    string appAuthToken = dt.Rows[i]["AppAuthToken"].ToString(); //集团ID
                    BillDownloadurl(context, client, AdminHotelid, hotelid, bill_date, appAuthToken, request);
                }
            }
            context.Response.Write("success");
        }

        private void BillDownloadurl(HttpContext context, IAopClient client, string adminHotelId, int hotelId, DateTime bill_date, string appAuthToken, AlipayDataDataserviceBillDownloadurlQueryRequest request)
        {
            try
            {
                string session = "";
                AlipayDataDataserviceBillDownloadurlQueryResponse response = client.Execute(request, session, appAuthToken);
                if (response != null && !string.IsNullOrEmpty(response.BillDownloadUrl))
                {
                    string url = response.BillDownloadUrl;//得到下载路径

                    string filename = HttpContext.Current.Server.MapPath("/File/Zip/AlipayBill.zip");
                    //使用WebClient 下载文件
                    System.Net.WebClient myWebClient = new System.Net.WebClient();
                    byte[] data = myWebClient.DownloadData(url);
                    Stream stream = new MemoryStream(data);//byte[] 转换成 流
                    FileHelper.StreamToFile(stream, filename);//保存下载zip
                    string destinationFile = HttpContext.Current.Server.MapPath("/File/");//解压路径
                    GZipHelper.UnZip(filename, destinationFile);//解压
                    string[] files = FileHelper.GetDirectoryFileList(destinationFile);

                    for (int i = 0; i < files.Length; i++)
                    {
                        string fn = files[i];

                        context.Response.Write("\r\n");
                        context.Response.Write(fn);
                        string filePath = HttpContext.Current.Server.MapPath("/File/" + fn);//文件路径      
                        DataTable dt = GetdataFromCVS(filePath);
                        FileHelper.DeleteFile(filePath);
                        if (dt == null || dt.Rows.Count < 1)
                        {
                            continue;
                        }

                        context.Response.Write(".<br />");
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (fn.IndexOf("汇总") > 0)
                            {
                                Hashtable htStatistics = new Hashtable();
                                htStatistics["AdminHotelId"] = adminHotelId;
                                htStatistics["HotelId"] = hotelId;
                                htStatistics["Bill_Date"] = bill_date.ToString("yyyy-MM-dd");
                                htStatistics["PaySource"] = 2;

                                htStatistics["Transaction_Number"] = dt.Rows[k]["交易订单总笔数"].ToString(); //总交易单数
                                htStatistics["Transaction_Money"] = dt.Rows[k]["订单金额（元）"].ToString();  //总交易额
                                htStatistics["Refund_Money"] = dt.Rows[k]["退款订单总笔数"].ToString();       //总退款金额
                                htStatistics["Vouchers_Discounts"] = dt.Rows[k]["支付宝优惠（元）"].ToString(); //总代金券或立减优惠退款金额
                                htStatistics["Service_Charge"] = dt.Rows[k]["服务费（元）"].ToString();     //手续费总金额
                                DataFactory.SqlDataBase().InsertByHashtable("Hotel_Bill_Total", htStatistics);
                            }
                            else
                            {
                                Hashtable ht = new Hashtable();
                                ht["AdminHotelId"] = adminHotelId;
                                ht["HotelId"] = hotelId;
                                ht["Bill_Date"] = bill_date.ToString("yyyy-MM-dd");
                                ht["PaySource"] = 2;

                                ht["Transaction_Time"] = dt.Rows[k]["完成时间"].ToString();           //交易时间
                                ht["Appid"] = dt.Rows[k]["商品名称"].ToString();                      //公众账号ID
                                ht["Mch_Id"] = dt.Rows[k]["门店编号"].ToString();                     //商户号
                                ht["Sub_Mch_Id"] = dt.Rows[k]["门店名称"].ToString();                 //子商户号
                                ht["Device_Number"] = dt.Rows[k]["终端号"].ToString();              //设备号
                                ht["WX_Order_Numbe"] = dt.Rows[k]["支付宝交易号"].ToString();             //微信订单号
                                ht["Order_Numbe"] = dt.Rows[k]["商户订单号"].ToString();                //商户订单号
                                ht["Openid"] = dt.Rows[k]["对方账户"].ToString();                     //用户标识
                                ht["Transaction_Type"] = dt.Rows[k]["商品名称"].ToString() == "扫码支付" ? "MICROPAY" : "JSAPI";           //交易类型
                                if (dt.Rows[k]["业务类型"].ToString().IndexOf("退款") >= 0)
                                {
                                    ht["Transaction_State"] = "REFUND";          //交易状态
                                    ht["Total_Money"] = "0.00";               //总金额
                                    ht["Refund_Money"] = CommonHelper.GetDouble(dt.Rows[k]["订单金额（元）"].ToString()) * -1;              //退款金额
                                    ht["WX_Refund_Numbe"] = dt.Rows[k]["支付宝交易号"].ToString();           //微信退款单号
                                    ht["Refund_Numbe"] = dt.Rows[k]["商户订单号"].ToString();              //商户退款单号
                                    ht["Refund_Type"] = "ORIGINAL";//退款类型
                                    ht["Refund_State"] = "SUCCESS";//退款状态
                                    ht["Refund_Vouchers_Discounts"] = dt.Rows[k]["支付宝优惠（元）"].ToString(); //代金券或立减优惠退款金额
                                }
                                else
                                {
                                    ht["Transaction_State"] = "SUCCESS";          //交易状态
                                    ht["Total_Money"] = dt.Rows[k]["订单金额（元）"].ToString();               //总金额
                                    ht["Refund_Money"] = "0.00";              //退款金额
                                }
                                ht["Paying_Bank"] = "";               //付款银行
                                ht["Currency_Type"] = "CNY";             //货币种类
                                ht["Data_Package"] = "";              //商户数据包
                                ht["Vouchers_Discounts"] = dt.Rows[k]["支付宝优惠（元）"].ToString();        //代金券或立减优惠金额
                                ht["Commodity_Name"] = dt.Rows[k]["商品名称"].ToString();            //商品名称
                                ht["Service_Charge"] = CommonHelper.GetDouble(dt.Rows[k]["服务费（元）"].ToString()) * -1;            //手续费
                                ht["Fee_Rate"] = "0.60%";                  //费率
                                DataFactory.SqlDataBase().InsertByHashtable("Hotel_Bill", ht);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static DataTable GetdataFromCVS(string fileName)
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable();
                StreamReader sr = new StreamReader(fileName, Encoding.Default);
                string strTitle1 = sr.ReadLine();
                string strTitle2 = sr.ReadLine();
                string strTitle3 = sr.ReadLine();
                string strTitle4 = sr.ReadLine();
                string strTitle = sr.ReadLine();

                string[] strColumTitle = strTitle.Split(',');   //CVS 文件默认以逗号隔开

                for (int i = 0; i < strColumTitle.Length; i++)
                {
                    dt.Columns.Add(strColumTitle[i]);
                }
                while (!sr.EndOfStream)
                {
                    string strTest = sr.ReadLine();
                    string[] strTestAttribute = strTest.Split(',');
                    if (strColumTitle.Length == strTestAttribute.Length && strTestAttribute[0].IndexOf("合计") < 0)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < strColumTitle.Length; i++)
                        {
                            dr[strColumTitle[i]] = strTestAttribute[i];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                sr.Close();
            }
            catch (Exception)
            {

            }
            return dt;
        }


        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}