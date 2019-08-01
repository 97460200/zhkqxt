using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using RM.Common.DotNetEncrypt;
using System.Collections;
using RM.Common.DotNetCode;
using System.Data;
using RM.Busines;
using RM.Common.DotNetBean;

/// <summary>
/// 全局变量 Application 公共类
/// </summary>
public static class ApplicationHelper
{

    #region ** 获取 微信配置信息 **
    public static string GetWxPayConfig(string adminHotelId, string columnName, int hotelId = 0)
    {
        string dataVal = "";
        string key_name = "WxPayConfig_" + adminHotelId + hotelId;
        HttpContext rq = HttpContext.Current;

        DataTable dt = null;
        if (rq.Application[key_name] != null)
        {
            dt = (DataTable)rq.Application[key_name];
        }

        if (dt == null || dt.Rows.Count == 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT APPID,MCHID,KEYS,APPSECRET,NOTIFY_URL,SSLCERT_PATH,SSLCERT_PASSWORD,Original_ID FROM WeChatInfo where AdminHotelid=@AdminHotelid and HotelId=@HotelId ");
            SqlParam[] parm = new SqlParam[] { 
                new SqlParam("@HotelId", hotelId),
                new SqlParam("@AdminHotelid", adminHotelId)
            };
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm);
            rq.Application[key_name] = dt;
        }

        if (dt != null && dt.Rows.Count > 0)
        {
            object obj_val = dt.Rows[0][columnName];
            if (obj_val != null && obj_val.ToString() != "")
            {
                dataVal = dt.Rows[0][columnName].ToString().Trim();
            }
        }
        return dataVal;
    }

    /// <summary>
    /// 清理 全部酒店微信配置信息
    /// </summary>
    public static void ClearWxPayConfig()
    {
        HttpContext rq = HttpContext.Current;
        for (int i = 0; i < rq.Application.Keys.Count; i++)
        {
            string key_name = rq.Application.Keys[i];
            if (key_name.IndexOf("WxPayConfig_") >= 0)
            {
                rq.Application.Remove(key_name);
            }
        }
    }

    /// <summary>
    /// 清理 指定酒店微信配置信息
    /// </summary>
    /// <param name="adminHotelId">集团id</param>
    /// <param name="hotelId">酒店id</param>
    public static void ClearHotelTemplateId(string adminHotelId, int hotelId = 0)
    {
        HttpContext rq = HttpContext.Current;
        string key_name = "WxPayConfig_" + adminHotelId + hotelId;
        rq.Application.Remove(key_name);
    }

    #endregion

    #region ** 智订云商户平台模板id **
    /// <summary>
    /// 获取 智订云商户平台模板id
    /// </summary>
    /// <param name="name">模板名称</param>
    /// <returns></returns>
    public static string GetAdminTemplateId(string name)
    {
        string dataVal = "";
        string key_name = "AdminTemplateId";
        HttpContext rq = HttpContext.Current;
        DataTable dt = null;
        if (rq.Application[key_name] != null)
        {
            dt = (DataTable)rq.Application[key_name];
        }

        if (dt == null || dt.Rows.Count == 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT name,template_id FROM Admin_Template");
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
            rq.Application[key_name] = dt;
        }

        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["name"].ToString() == name)
                {
                    dataVal = dt.Rows[i]["template_id"].ToString().Trim();
                }
            }
        }
        return dataVal;
    }

    /// <summary>
    /// 清理 智订云商户平台模板id
    /// </summary>
    public static void ClearAdminTemplateId()
    {
        HttpContext rq = HttpContext.Current;
        string key_name = "AdminTemplateId";
        rq.Application.Remove(key_name);
    }

    #endregion

    #region ** 酒店模板id **
    /// <summary>
    /// 获取 酒店模板id
    /// </summary>
    /// <param name="name">模板名称</param>
    /// <param name="adminHotelId">集团Id</param>
    /// <returns></returns>
    public static string GetHotelTemplateId(string name, string adminHotelId)
    {
        string dataVal = "";
        string key_name = "TemplateId_" + adminHotelId;
        HttpContext rq = HttpContext.Current;

        DataTable dt = null;
        if (rq.Application[key_name] != null)
        {
            dt = (DataTable)rq.Application[key_name];
        }

        if (dt == null || dt.Rows.Count == 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT name,template_id FROM  Set_TemplateId WHERE AdminHotelid=@AdminHotelid");
            SqlParam[] parm = new SqlParam[] { 
                    new SqlParam("@AdminHotelid", adminHotelId)
                };
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm);
            rq.Application[key_name] = dt;
        }

        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["name"].ToString() == name)
                {
                    dataVal = dt.Rows[i]["template_id"].ToString().Trim();
                }
            }
        }
        return dataVal;
    }

    /// <summary>
    /// 清理 所有酒店模板id
    /// </summary>
    public static void ClearHotelTemplateId()
    {
        HttpContext rq = HttpContext.Current;
        for (int i = 0; i < rq.Application.Keys.Count; i++)
        {
            string key_name = rq.Application.Keys[i];
            if (key_name.IndexOf("TemplateId_") >= 0)
            {
                rq.Application.Remove(key_name);
            }
        }
    }

    /// <summary>
    /// 清理 指定酒店模板id
    /// </summary>
    /// <param name="adminHotelId">集团ID</param>
    public static void ClearHotelTemplateId(string adminHotelId)
    {
        HttpContext rq = HttpContext.Current;
        string key_name = "TemplateId_" + adminHotelId;
        rq.Application.Remove(key_name);
    }
    #endregion

    #region ** 微信推文信息 **

    public static DataTable GetHotelTweetsInfo(string originalId)
    {
        string key_name = "OriginalId_" + originalId;
        HttpContext rq = HttpContext.Current;

        DataTable dt = null;
        if (rq.Application[key_name] != null)
        {
            dt = (DataTable)rq.Application[key_name];
        }

        if (dt == null || dt.Rows.Count == 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT  w.AdminHotelid ,
                    h.Name ,
                    h.type ,
                    ISNULL(( SELECT TOP 1
                                    ImgFile
                             FROM   Photo
                             WHERE  [type] = '9'
                                    AND AdminHotelid = h.AdminHotelid
                             ORDER BY hotelid,ID ASC
                           ), '') images,
                           r.content,
                           r.photo,
                           r.bt,
                           r.url
            FROM    dbo.WeChatInfo w
                    INNER JOIN dbo.Hotel_Admin h ON w.AdminHotelid = h.AdminHotelid
                    LEFT JOIN Reply_attention r ON r.AdminHotelid = h.AdminHotelid 
            WHERE   w.Original_ID = @Original_ID
                    AND h.AdminHotelid <> '1'
            ");
            SqlParam[] param = new SqlParam[] { 
                new SqlParam("@Original_ID", originalId)
            };
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
            rq.Application[key_name] = dt;
        }
        return dt;
    }


    #endregion

    /// <summary>
    /// 清除全部 Application
    /// </summary>
    public static void ClearAllApplication()
    {
        HttpContext rq = HttpContext.Current;
        rq.Application.Clear();//清除全部
    }
}
