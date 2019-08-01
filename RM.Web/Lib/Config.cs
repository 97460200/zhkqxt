using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using RM.Busines;
using System.Text;
using System.Collections;
using System.Web.SessionState;
using RM.Common.DotNetCode;

namespace RM.Web.Lib
{
    /**
    * 	配置账号信息
    */
    public class WxPayConfig : IRequiresSessionState
    {
        //智订云服务商号
        public static string ZDY_Appid = "wxb1befead0f19ed91";
        public static string ZDY_Mch_Id = "1526733001";
        public static string ZDY_Sslcert_Path = "/Authorization/Zdy/apiclient_cert.p12";
        public static string ZDY_Key = "64A2B83F276F1AE2AAD671346688E62E";

        private static string GetDataVal(string AdminHotelid, string columnName, int Hotelid = 0)
        {
            return ApplicationHelper.GetWxPayConfig(AdminHotelid, columnName, Hotelid);
        }

        /// <summary>
        /// 是否为智订云下子商户
        /// </summary>
        /// <param name="AdminHotelid"></param>
        /// <param name="Hotelid"></param>
        /// <returns></returns>
        public static bool ServiceProvider(string AdminHotelid, int Hotelid = 0)
        {
            string sp = GetDataVal(AdminHotelid, "IsServiceProvider", Hotelid);
            if (sp == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public static string APPID(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "appid", Hotelid);
        }

        public static string MCHID(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "MCHID", Hotelid);
        }

        public static string AppId_xcx(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "AppId_xcx", Hotelid);
        }

        public static string MchId_xcx(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "MchId_xcx", Hotelid);
        }

        public static string APPSECRET(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "APPSECRET", Hotelid);
        }

        public static string redirect_uri(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "NOTIFY_URL", Hotelid);
        }
        public static string IsTicket(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "IsTicket", Hotelid);
        }
        public static string PrintNumber(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "PrintNumber", Hotelid);
        }

        public static string FullName(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "FullName", Hotelid);
        }

        public static string zfb_FullName(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "zfb_FullName", Hotelid);
        }

        public static string KEY(string AdminHotelid, int Hotelid)
        {
            if (ServiceProvider(AdminHotelid, Hotelid))
            {
                return ZDY_Key;
            }
            else
            {
                return GetDataVal(AdminHotelid, "KEYS", Hotelid);
            }
        }

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        //public const string SSLCERT_PATH = "cert/apiclient_cert.p12";
        //public const string SSLCERT_PASSWORD = "1233410002";
        public static string SSLCERT_PATH(string AdminHotelid, int Hotelid = 0)
        {
            if (ServiceProvider(AdminHotelid, Hotelid))
            {
                return ZDY_Sslcert_Path;
            }
            else
            {
                return GetDataVal(AdminHotelid, "SSLCERT_PATH", Hotelid);
            }
        }

        public static string SSLCERT_PASSWORD(string AdminHotelid, int Hotelid = 0)
        {
            if (ServiceProvider(AdminHotelid, Hotelid))
            {
                return ZDY_Mch_Id;
            }
            else
            {
                return GetDataVal(AdminHotelid, "SSLCERT_PASSWORD", Hotelid);
            }

        }

        //酒店各自的参数 用于下载资金账单 或 员工提现
        public static string key_own(string AdminHotelid, int Hotelid)
        {
            return GetDataVal(AdminHotelid, "KEYS", Hotelid);
        }

        public static string sslcert_path_own(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "SSLCERT_PATH", Hotelid);
        }

        public static string sslcert_password_own(string AdminHotelid, int Hotelid = 0)
        {
            return GetDataVal(AdminHotelid, "SSLCERT_PASSWORD", Hotelid);
        }

        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public static string NOTIFY_URL(string AdminHotelid, int Hotelid = 0)
        {
            string redirect_uri = GetDataVal(AdminHotelid, "NOTIFY_URL", Hotelid);
            return "http://" + redirect_uri + "/Reservation/ResultNotifyPage.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
        }

        // public static string NOTIFY_URL = "http://" + redirect_uri() + "/Reservation/ResultNotifyPage.aspx";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动
         * 
         * 
         * 
         * 配置也可在程序中自动获取
        */
        public const string IP = "8.8.8.8";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 3;
    }
}