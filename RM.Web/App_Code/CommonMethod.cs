using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Web;
using System.Data;
using RM.Busines;
using System.Xml;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RM.Common.DotNetCode;
using RM.Web.business;
using RM.Common.DotNetConfig;
using RM.Common.DotNetBean;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using RM.Common.DotNetUI;
using RM.Web.Lib;
using System.Threading;
using LitJson;
using RM.Common.DotNetData;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

/// <summary>
/// Common 的摘要说明
/// </summary>
public static class CommonMethod
{
    #region 截取字符串
    /// <summary>
    /// 截取字符串
    /// </summary>
    /// <param name="rawString">原始字符串</param>
    /// <param name="maxLength">最大长度</param>
    /// <returns>string</returns>
    public static string Substring(string rawString, int maxLength)
    {
        return Substring(rawString, maxLength, false);
    }

    /// <summary>
    /// 截取字符串（按字节）
    /// </summary>
    /// <param name="rawString">原始字符串</param>
    /// <param name="maxLength">最大长度</param>
    /// <returns>string</returns>
    public static string SubstringB(string rawString, int maxLength)
    {
        return SubstringB(rawString, maxLength, false);
    }

    /// <summary>
    /// 截取字符串
    /// </summary>
    /// <param name="rawString">原始字符串</param>
    /// <param name="maxLength">最大长度</param>
    /// <param name="isDoted">是否有省略号</param>
    /// <returns>string</returns>
    public static string Substring(string rawString, int maxLength, bool isDoted)
    {
        if (rawString == null || rawString == string.Empty || maxLength <= 0)
        {
            return string.Empty;
        }

        if (maxLength >= rawString.Length)
        {
            return rawString;
        }

        return isDoted ? rawString.Substring(0, maxLength - 3) + "..." : rawString.Substring(0, maxLength);
    }

    /// <summary>
    /// 截取字符串（按字节）
    /// </summary>
    /// <param name="rawString">原始字符串</param>
    /// <param name="maxLength">最大长度</param>
    /// <param name="isDoted">是否有省略号</param>
    /// <returns>string</returns>
    public static string SubstringB(string rawString, int maxLength, bool isDoted)
    {
        if (rawString == null || rawString == string.Empty || maxLength <= 0)
        {
            return string.Empty;
        }

        int rawLen = rawString.Length;
        for (int i = 0; i < maxLength && i < rawLen; i++)
        {
            if ((int)(rawString[i]) > 0xFF)
            {
                maxLength--;
            }
        }

        if (maxLength > rawLen)
        {
            return rawString;
        }

        if (isDoted)
        {
            int dotedLength = maxLength - 3;
            for (int i = maxLength - 1; i > maxLength - 3; i--)
            {
                if ((int)(rawString[i]) > 0xFF)
                {
                    dotedLength++;
                }
            }

            return rawString.Substring(0, dotedLength) + "...";
        }

        return rawString.Substring(0, maxLength);
    }
    #endregion

    #region 缩略图
    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="originalPath">原图路径</param>
    /// <param name="thumbPath">缩略图的保存路径</param>
    /// <param name="thumbWidth">缩略图的宽度</param>
    /// <param name="thumbHeight">缩略图的高度</param>
    /// <param name="isScale">是否按比例缩放，默认为False</param>
    public static void CreateThumbnail(string originalPath, string thumbPath, int thumbWidth, int thumbHeight, bool isScale)
    {
        //从文件创建Image对象
        System.Drawing.Image oImage = System.Drawing.Image.FromFile(originalPath);
        CreateThumbnail(oImage, thumbWidth, thumbHeight, isScale, thumbPath, (string)null);

        oImage.Dispose();
    }

    /// <summary>
    /// 上传图片生成缩略图
    /// </summary>
    /// <param name="imgStream">图像流</param>
    /// <param name="thumbWidth">缩略图的宽度</param>
    /// <param name="thumbHeight">缩略图的高度</param>
    /// <param name="isScale">是否按比例缩放，默认为False</param>
    /// <param name="thumbPath">缩略图的保存路径</param>
    public static void CreateThumbnail(System.IO.Stream originalStream, int thumbWidth, int thumbHeight, bool isScale, string thumbPath)
    {
        //以读取的数据流来创建Image对象
        System.Drawing.Image oImage = System.Drawing.Image.FromStream(originalStream);
        CreateThumbnail(originalStream, thumbWidth, thumbHeight, isScale, thumbPath, (string)null);

        oImage.Dispose();
    }

    /// <summary>
    /// 上传图片同时生成缩略图，并保存原图
    /// </summary>
    /// <param name="imgStream">图像流</param>
    /// <param name="thumbWidth">缩略图的宽度</param>
    /// <param name="thumbHeight">缩略图的高度</param>
    /// <param name="isScale">是否按比例缩放，默认为False</param>
    /// <param name="thumbPath">缩略图的保存路径</param>
    /// <param name="originalPath">原图的保存路径</param>
    public static void CreateThumbnail(System.IO.Stream originalStream, int thumbWidth, int thumbHeight, bool isScale, string thumbPath, string originalPath)
    {
        //以读取的数据流来创建Image对象
        System.Drawing.Image oImage = System.Drawing.Image.FromStream(originalStream);
        CreateThumbnail(oImage, thumbWidth, thumbHeight, isScale, thumbPath, originalPath);

        oImage.Dispose();
    }

    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="oImage">原图Image对象</param>
    /// <param name="thumbWidth">缩略图的宽度</param>
    /// <param name="thumbHeight">缩略图的高度</param>
    /// <param name="isScale">是否按比例缩放，默认为False</param>
    /// <param name="thumbPath">缩略图的保存路径</param>
    /// <param name="originalPath">原图的保存路径</param>
    public static void CreateThumbnail(System.Drawing.Image oImage, int thumbWidth, int thumbHeight, bool isScale, string thumbPath, string originalPath)
    {
        //原图的宽度
        int oWidth = oImage.Width;
        //原图的高度
        int oHeight = oImage.Height;

        //按比例计算出缩略图的宽度和高度 
        if (isScale)
        {
            //缩小的比例
            double scale = (double)Math.Min(thumbWidth, thumbHeight) / Math.Max(oWidth, oHeight);

            thumbHeight = (int)Math.Floor(oHeight * scale);
            thumbWidth = (int)Math.Floor(oWidth * scale);
        }

        //以缩略图的宽高创建位图对象
        System.Drawing.Bitmap tImage = new System.Drawing.Bitmap(thumbWidth, thumbHeight);
        //创建Graphics对象，用于缩略图的图像的绘制
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(tImage);
        //指定高质量插值法
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
        //指定高质量、低速度呈现
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //清除整个绘图面并以透明色填充
        g.Clear(System.Drawing.Color.Transparent);
        //缩略图绘制 
        g.DrawImage(oImage, new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight), new System.Drawing.Rectangle(0, 0, oWidth, oHeight), System.Drawing.GraphicsUnit.Pixel);

        try
        {
            //以JPG格式保存图片
            if (!string.IsNullOrEmpty(originalPath))
            {
                oImage.Save(originalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            tImage.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        catch (Exception ex)
        {

        }
        finally
        {
            g.Dispose();
            tImage.Dispose();
        }
    }

    /// <summary>
    /// 通过上传控件的PostedFile.ContentType获取图片的后缀名
    /// </summary>
    /// <param name="contentType">PostedFile.ContentType</param>
    /// <returns>图片的后缀名，非图片则返回string.Empty</returns>
    public static string GetPictureExtension(string contentType)
    {
        string ext = null;

        switch (contentType)
        {
            case "image/pjpeg":
                ext = ".jpg";
                break;
            case "image/gif":
                ext = ".gif";
                break;
            case "image/x-png":
                ext = ".png";
                break;
            case "image/bmp":
                ext = ".bmp";
                break;
            default:
                ext = string.Empty;
                break;
        }

        return ext;
    }
    #endregion

    #region 防止SQL注入式攻击
    /// <summary>
    /// 过滤参数
    /// </summary>
    /// <param name="strParam">字符串参数</param>
    /// <returns>string</returns>
    public static string FilterSQL(string strParam)
    {
        return strParam.Replace("'", "''");
    }
    #endregion

    #region 过滤HTML
    /// <summary>
    /// 过滤尖括号（以HTML代码过滤来显示）
    /// </summary>
    /// <param name="strParam">字符串参数</param>
    /// <returns>string</returns>
    public static string FilterTagWithHtml(string strParam)
    {
        return strParam.Replace("<", "<FONT><</FONT>");
    }

    /// <summary>
    /// 去除HTML标记
    /// </summary>
    /// <param name="strParam">字符串参数</param>
    /// <returns>string</returns>
    public static string NoHTML(string strParam)
    {
        //删除脚本 
        strParam = Regex.Replace(strParam, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

        //删除HTML 
        strParam = Regex.Replace(strParam, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"-->", "", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"<!--.*", "", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
        strParam = Regex.Replace(strParam, @"&#(\d+);", "", RegexOptions.IgnoreCase);
        strParam.Replace("<", "");
        strParam.Replace(">", "");
        strParam.Replace("\r\n", "");

        return System.Web.HttpContext.Current.Server.HtmlEncode(strParam);
    }

    /// <summary>
    /// HtmlEncode
    /// </summary>
    /// <param name="strParam">字符串参数</param>
    /// <returns>string</returns>
    public static string HtmlEncode(string strParam)
    {
        strParam = strParam.Replace("&", "&amp;")
                            .Replace("\"", "&quot;")
                            .Replace(" ", "&nbsp;")
                            .Replace("<", "&lt;")
                            .Replace(">", "&gt;")
                            .Replace("\n", "<br>");

        return strParam;
    }

    /// <summary>
    /// HtmlDecode
    /// </summary>
    /// <param name="strParam">字符串参数</param>
    /// <returns>string</returns>
    public static string HtmlDecode(string strParam)
    {
        strParam = strParam.Replace("<br>", "\n")
                            .Replace("&gt;", ">")
                            .Replace("&lt;", "<")
                            .Replace("&nbsp;", " ")
                            .Replace("&quot;", "\"");

        return strParam;
    }
    #endregion

    #region Alert
    /// <summary>
    /// 弹出消息框
    /// </summary>
    /// <param name="page">Page对象</param>
    /// <param name="message">消息</param>
    public static void Alert(System.Web.UI.Page page, string message)
    {
        page.ClientScript.RegisterClientScriptBlock(
            page.GetType(),
            "alert",
            string.Format(
                "alert(\"{0}\");",
                message.Replace("\"", "\\\"")
                ),
            true
            );
    }

    /// <summary>
    /// 弹出消息框，并以替换地址栏方式跳转页面，多用于避免刷新页面重复提交
    /// </summary>
    /// <param name="page">Page对象</param>
    /// <param name="message">消息</param>
    public static void AlertAndRedirect(System.Web.UI.Page page, string message)
    {
        AlertAndRedirect(page, message, page.Request.Url.PathAndQuery);
    }

    /// <summary>
    /// 弹出消息框，并以替换地址栏方式跳转页面，多用于避免刷新页面重复提交
    /// </summary>
    /// <param name="page">Page对象</param>
    /// <param name="redirect">跳转页面</param>
    /// <param name="message">消息</param>
    public static void AlertAndRedirect(System.Web.UI.Page page, string message, string redirect)
    {
        page.ClientScript.RegisterClientScriptBlock(
            page.GetType(),
            "alert",
            string.Format(
                "alert(\"{0}\"); location.replace(\"{1}\");",
                message.Replace("\"", "\\\""),
                redirect.Replace("\"", "\\\"")
                ),
            true
            );
    }
    #endregion

    #region Confirm
    /// <summary>
    /// 弹出确认框，根据选择执行脚本
    /// </summary>
    /// <param name="page">Page对象</param>
    /// <param name="message">消息</param>
    /// <param name="yesScript">选择 “是” 执行的脚本</param>
    /// <param name="noScript">选择 “否” 执行的脚本</param>
    public static void Confirm(System.Web.UI.Page page, string message, string yesScript, string noScript)
    {
        page.ClientScript.RegisterClientScriptBlock(
            page.GetType(),
            "confirm",
            string.Format(
                "if(confirm(\"{0}\")) {{ {1} }} else {{ {2} }}",
                message.Replace("\"", "\\\""),
                yesScript,
                noScript
                ),
            true
            );
    }

    /// <summary>
    /// 弹出确认框，根据选择跳转页面
    /// </summary>
    /// <param name="page">Page对象</param>
    /// <param name="message">消息</param>
    /// <param name="yesRedirect">选择 “是” 跳转的页面</param>
    /// <param name="noRedirect)">选择 “否” 跳转的页面</param>
    public static void ConfirmAndRedirect(System.Web.UI.Page page, string message, string yesRedirect, string noRedirect)
    {
        page.ClientScript.RegisterClientScriptBlock(
            page.GetType(),
            "confirm",
            string.Format(
                "if(confirm(\"{0}\")) {{ location.replace(\"{1}\"); }} else {{ location.replace(\"{2}\"); }}",
                message.Replace("\"", "\\\""),
                yesRedirect,
                noRedirect
                ),
            true
            );
    }
    #endregion

    #region Regex
    /// <summary>
    /// 是否数字
    /// </summary>
    private static Regex RegNumber = new Regex("^[0-9]+$");
    /// <summary>
    /// 是否数字 可带正负号
    /// </summary>
    private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
    /// <summary>
    /// 是否浮点数
    /// </summary>
    private static Regex RegDecimal = new Regex("^[0-9]+([.]?[0-9]+)?$");
    /// <summary>
    /// 是否浮点数 可带正负号
    /// </summary>
    private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+([.]?[0-9]+)?$");
    /// <summary>
    /// 是否邮件地址
    /// </summary>
    private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
    /// <summary>
    /// 是否含有中文字符
    /// </summary>
    private static Regex RegChinese = new Regex("[\u4e00-\u9fa5]");
    #endregion

    #region 字符串检查
    /// <summary>
    /// 是否数字
    /// </summary>
    /// <param name="strParam">字符串参数</param>
    /// <returns>bool</returns>
    public static bool IsNumber(string strParam)
    {
        return RegNumber.IsMatch(strParam);
    }

    /// <summary>
    /// 是否数字 可带正负号
    /// </summary>
    /// <param name="strParam">字符串参数</param>
    /// <returns>bool</returns>
    public static bool IsNumberSign(string strParam)
    {
        return RegNumberSign.IsMatch(strParam);
    }

    /// <summary>
    /// 是否浮点数
    /// </summary>
    /// <param name="strParam">输入字符串</param>
    /// <returns>bool</returns>
    public static bool IsDecimal(string strParam)
    {
        return RegDecimal.IsMatch(strParam);
    }

    /// <summary>
    /// 是否浮点数 可带正负号
    /// </summary>
    /// <param name="strParam">输入字符串</param>
    /// <returns>bool</returns>
    public static bool IsDecimalSign(string strParam)
    {
        return RegDecimalSign.IsMatch(strParam);
    }

    /// <summary>
    /// 是否邮件地址
    /// </summary>
    /// <param name="strParam">输入字符串</param>
    /// <returns>bool</returns>
    public static bool IsEmail(string strParam)
    {
        return RegEmail.IsMatch(strParam);
    }

    /// <summary>
    /// 是否含有中文字符
    /// </summary>
    /// <param name="strParam">输入字符串</param>
    /// <returns>bool</returns>
    public static bool ContainChinese(string strParam)
    {
        return RegChinese.IsMatch(strParam);
    }
    #endregion

    #region Json
    /// <summary>
    /// 将DataTable以JSON数据格式输出
    /// </summary>
    /// <param name="dt">DataTable</param>
    /// <param name="dtName">输出的表名</param>
    /// <returns>string</returns>
    public static string DataTableToJson(System.Data.DataTable dt)
    {
        return DataTableToJson(dt, string.Empty, new string[] { });
    }

    /// <summary>
    /// 将DataTable以JSON数据格式输出
    /// </summary>
    /// <param name="dt">DataTable</param>
    /// <param name="dtName">输出的表名</param>
    /// <returns>string</returns>
    public static string DataTableToJson(System.Data.DataTable dt, string dtName)
    {
        return DataTableToJson(dt, dtName, new string[] { });
    }

    /// <summary>
    /// 将DataTable以JSON数据格式输出
    /// </summary>
    /// <param name="dt">DataTable</param>
    /// <param name="dtName">输出的表名</param>
    /// <param name="dtColumns">输出的列集合</param>
    /// <returns>string</returns>
    public static string DataTableToJson(System.Data.DataTable dt, string dtName, string[] dtColumns)
    {
        //是否输出所有列
        bool isAll = dtColumns.Length == 0;

        //缓存行数及列数
        int rowCount = dt.Rows.Count;
        int columnCount = isAll ? dt.Columns.Count : dtColumns.Length;

        StringBuilder sb = new StringBuilder();
        if (!string.IsNullOrEmpty(dtName))
        {
            sb.AppendFormat("{{\"{0}\": ", dtName);
        }
        sb.Append("[");

        if (rowCount > 0)
        {
            for (int i = 0; i < rowCount; i++)
            {
                sb.Append("{");
                for (int j = 0; j < columnCount; j++)
                {
                    if (isAll)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        sb.AppendFormat("\"{0}\": \"{1}\"", dt.Columns[j].ColumnName, StringFormat(dt.Rows[i][j].ToString(), type));
                    }
                    else
                    {
                        Type type = dt.Rows[i][dtColumns[j]].GetType();
                        sb.AppendFormat("\"{0}\": \"{1}\"", dtColumns[j], StringFormat(dt.Rows[i][dtColumns[j]].ToString(), type));
                    }
                    if (j != columnCount - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append("}");

                if (i != rowCount - 1)
                {
                    sb.Append(", ");
                }
            }
        }

        sb.Append("]");
        if (!string.IsNullOrEmpty(dtName))
        {
            sb.Append("}");
        }

        return sb.ToString();
    }

    public static JsonData DataTableToJsonData(DataTable dt)
    {
        JsonData jd = new JsonData();
        if (DataTableHelper.IsExistRows(dt))
        {
            jd["List"] = new JsonData();
            foreach (DataRow dr in dt.Rows)
            {
                JsonData jdRows = new JsonData();
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    string cn = dc.ColumnName;
                    string val = "";
                    if (dr[dc] != null && dr[dc] != DBNull.Value && dr[dc].ToString() != "")
                        val = dr[dc].ToString();
                    jdRows[cn] = val;
                }
                jd["List"].Add(jdRows);
            }
        }
        return jd;
    }


    /// <summary>   
    /// 过滤特殊字符   
    /// </summary>   
    /// <param name="s">输出字符串</param>   
    /// <returns>string</returns>
    private static string String2Json(string s)
    {
        if (s == null)
        {
            return string.Empty;
        }

        StringBuilder sb = new StringBuilder();
        foreach (char c in s)
        {
            switch (c)
            {
                case '"':
                    sb.Append("\\\"");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '/':
                    sb.Append("\\/");
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        return sb.ToString();
    }

    /// <summary>   
    /// 格式化字符型、日期型、布尔型   
    /// </summary>   
    /// <param name="str">输出字符串</param>   
    /// <param name="type">类型</param>
    /// <returns>string</returns>
    private static string StringFormat(string str, Type type)
    {
        if (type == typeof(string))
        {
            str = String2Json(str);
        }
        else if (type == typeof(DateTime))
        {
            //
        }
        else if (type == typeof(bool))
        {
            str = str.ToLower();
        }

        return str;
    }
    #endregion

    /// <summary>
    /// 移去小数末尾的无效0
    /// </summary>
    /// <param name="num">小数</param>
    /// <returns>string</returns>
    public static string TrimDecimalZeroFraction(decimal num)
    {
        string val = num.ToString();
        if (val.IndexOf('.') != -1)
        {
            val = val.TrimEnd('0').TrimEnd('.');
        }

        return val;
    }

    /// <summary>
    /// 判断是否绝对路径
    /// </summary>
    /// <param name="path">要测试的路径</param>
    /// <returns>bool</returns>
    public static bool IsPathRooted(string path)
    {
        if (path.StartsWith(@"\\") || path.IndexOf(@":\") != -1 || path.IndexOf(@"://") != -1)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 替换URL参数，没有该参数则补上
    /// </summary>
    /// <param name="requesturl">URL</param>
    /// <param name="paramname">参数名称</param>
    /// <param name="paramvalue">参数值</param>
    /// <returns>string</returns>
    public static string ReplaceUrlParamValue(string requesturl, string paramname, string paramvalue)
    {
        if (requesturl.IndexOf(string.Concat(paramname, "=")) != -1)
        {
            Regex re = new Regex(string.Concat("(", paramname, "=)([^&]*)"), RegexOptions.IgnoreCase);
            return re.Replace(requesturl, string.Concat(paramname, "=", paramvalue));
        }
        else
        {
            int index = requesturl.LastIndexOf("&");
            if (index == requesturl.Length - 1)
            {
                return string.Concat(requesturl, paramname, "=", paramvalue);
            }
            else if (index != -1)
            {
                return string.Concat(requesturl, "&", paramname, "=", paramvalue);
            }

            index = requesturl.IndexOf("?");
            if (index == requesturl.Length - 1)
            {
                return string.Concat(requesturl, paramname, "=", paramvalue);
            }
            else if (index == -1)
            {
                return string.Concat(requesturl, "?", paramname, "=", paramvalue);
            }
            else
            {
                return string.Concat(requesturl, "&", paramname, "=", paramvalue);
            }
        }
    }


    #region 判断是否参与摇一摇活动
    /// <summary>
    /// 判断是否参与摇一摇活动
    /// </summary>
    public static void UPDATE_Activity(string AdminHotelid, string OpenId, string MenberId)
    {

        StringBuilder str = new StringBuilder();
        str.Append(string.Format(" SELECT  * FROM  dbo.ActivityBinGoNum  WHERE OpenId=@OpenId and AdminHotelid=@AdminHotelid and MenberId=0 "));
        List<SqlParam> ilistStr = new List<SqlParam>();
        ilistStr.Add(new SqlParam("@OpenId", OpenId.ToString()));
        ilistStr.Add(new SqlParam("@AdminHotelid", AdminHotelid.ToString()));
        str.Append("  ORDER BY ID ASC");
        DataTable dstr = DataFactory.SqlDataBase().GetDataTableBySQL(str, ilistStr.ToArray());
        if (dstr != null && dstr.Rows.Count > 0)
        {
            string menbername = "";
            string hylx = "";
            string phone = "";
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT jybj,CarId,lsh,hylx,xm,sjhm  FROM hy_hyzlxxb WHERE lsh='{0}' ", MenberId);
            DataTable hy = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);
            if (hy != null && hy.Rows.Count > 0)
            {
                menbername = hy.Rows[0]["xm"].ToString();
                hylx = hy.Rows[0]["hylx"].ToString();
                phone = hy.Rows[0]["sjhm"].ToString();
            }

            for (int i = 0; i < dstr.Rows.Count; i++)
            {
                string return_val = "0";
                string ItemId = dstr.Rows[i]["ItemId"].ToString();
                string ItemVal = dstr.Rows[i]["ItemVal"].ToString();
                if (ItemId.ToString() == "1")//领取卡券
                {
                    string sqlcp = string.Format(@"select * from V_XXZSYHQ where CID=@ID and AdminHotelid=@AdminHotelid ");
                    SqlParam[] parmAdd3 = new SqlParam[] { 
                                    new SqlParam("@ID", ItemVal),
                                    new SqlParam("@AdminHotelid", AdminHotelid)};
                    DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);
                    if (dts != null && dts.Rows.Count > 0)
                    {
                        return_val = AddActivityCardCoupons(MenberId.ToString(), OpenId.ToString(), AdminHotelid, "摇一摇活动优惠券", dts, "16");
                    }
                }
                else if (ItemId.ToString() == "2") //领取积分
                {

                    string Integral = ItemVal.ToString();
                    string zmsm = "摇一摇活动";
                    string bz = "摇一摇赠送积分";
                    AddIntegral(MenberId, phone, hylx, zmsm, Integral.ToString(), bz, AdminHotelid);

                    string url = "http://www.zidinn.com/Members/Mypoints.aspx?AdminHotelid=" + AdminHotelid;
                    string Remarks = "摇一摇活动赠送" + Integral + "积分";
                    AddIntegralNotice(AdminHotelid, "0", "", OpenId, "积分变动", "0", Integral.ToString(), url, Remarks);
                    return_val = "1";

                }
                else if (ItemId.ToString() == "3") //领取小礼品
                {
                    return_val = "1";
                }
                //else if (ItemId.ToString() == "4")//领取现金
                //{
                //    return_val = "1";
                //}
                //改变领取状态以及领取时间
                if (return_val == "1")
                {
                    Hashtable ht = new Hashtable();
                    ht["MenberId"] = MenberId;
                    ht["MenberName"] = menbername;
                    ht["Phone"] = phone;
                    if (ItemId.ToString() != "3")
                    {
                        ht["GetPrizeTime"] = DateTime.Now;
                        ht["State"] = 1;
                    }
                    int Ok = DataFactory.SqlDataBase().UpdateByHashtable("ActivityBinGoNum", "ID", dstr.Rows[i]["ID"].ToString(), ht);
                }
            }

            //更新参与记录的用户名昵称
            string sqlcj = string.Format(@"update ShakeRecord set MenberId='{0}',MenberName='{1}',Phone='{2}' where OpenId='{3}' and AdminHotelid='{4}' and MenberId=0 ", MenberId, menbername, phone, OpenId, AdminHotelid);
            DataTable isok = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcj));
        }
    }
    #endregion


    #region 发送短信
    /// <summary>
    /// 发送短信接口
    /// </summary>
    /// <param name="url"></param>
    /// <param name="param"></param>
    /// <returns></returns>

    public static string Create_BusinessNumber()
    {
        string strreturn = "";
        string date = System.DateTime.Now.ToString("yyyyMMdd");
        StringBuilder sb = new StringBuilder();
        sb.Append(" declare @MaxOrderNo int ");
        sb.Append(" select @MaxOrderNo= max(cast(substring(OrderNumber,len(OrderNumber)-2,3) as int)) ");
        sb.AppendFormat(" from [BookOrder] where OrderNumber like '{0}%' ", date);
        sb.Append(" if(@MaxOrderNo IS NULL) ");
        sb.Append("   SELECT 1 AS ProductNo ");
        sb.Append(" ELSE ");
        sb.Append("   SELECT (@MaxOrderNo +1) AS ProjecttNo ");
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
        if (dt.Rows[0][0].ToString().Length != 3)
        {
            for (int i = 0; i < 3 - dt.Rows[0][0].ToString().Length; i++)
            {
                strreturn += "0";
            }
        }
        strreturn += dt.Rows[0][0].ToString();
        return date + strreturn;
    }
    #endregion

    #region 发送短信
    /// <summary>
    /// 发送短信接口
    /// </summary>
    /// <param name="url"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static string SendRequest(string url, string param)
    {
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] data = encoding.GetBytes(param);
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;
        Stream sm = request.GetRequestStream();
        sm.Write(data, 0, data.Length);
        sm.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        if (response.StatusCode != HttpStatusCode.OK)
        {
            //请求失败
            return "";
        }

        StreamReader myreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        string responseText = myreader.ReadToEnd();
        return responseText;
    }

    /// <summary>
    /// 转换格式
    /// </summary>
    /// <param name="instring"></param>
    /// <returns></returns>
    public static string EncodeConver(string instring)
    {
        return HttpUtility.UrlEncode(instring, Encoding.UTF8);
    }

    /// <summary>
    /// 短信入口
    /// </summary>
    /// <returns></returns>
    public static bool SendSms(string Phone, string Number, int num, string AdminHotelid)
    {
        string content = "";
        string HotelName = "";
        string SendMoney = "0.08";
        string sql_name = string.Format(@"SELECT Name from Hotel_Admin where AdminHotelid='{0}'", AdminHotelid);
        DataTable ds_name = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql_name));
        if (ds_name != null && ds_name.Rows.Count > 0)
        {
            HotelName = ds_name.Rows[0]["Name"].ToString();

        }
        string sql = string.Format(@"SELECT ID , Userid ,  Account ,   Password , URL ,  OrganizationID , Name , SingleMoney , AdminHotelid from SmsParameter where AdminHotelid='{0}'", AdminHotelid);
        DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
        if (ds != null && ds.Rows.Count > 0)
        {
            SendMoney = ds.Rows[0]["SingleMoney"].ToString();
            if (num == 1)
            {
                content = "您正在" + HotelName + "进行免费注册验证，验证码：" + Number + "，请输入验证码完成注册。【" + ds.Rows[0]["name"] + "】";
                if (AdminHotelid == "1")
                {
                    content = "您正在" + HotelName + "进行登录，验证码：" + Number + "，请输入验证码完成登录。【" + ds.Rows[0]["name"] + "】";
                }
            }
            else if (num == 2)
            {
                content = "你在" + HotelName + "进行了修改手机绑定，验证码：" + Number + "，请输入验证码完成修改。【" + ds.Rows[0]["name"] + "】";
            }
            else if (num == 3)
            {
                content = "您正在" + HotelName + "进行免费注册验证，验证码：" + Number + "，请输入验证码完成注册。【" + ds.Rows[0]["name"] + "】";
            }
            else if (num == 4)
            {
                content = Number;
            }
            else if (num == 5)
            {
                content = "您正在智订云进行了忘记密码，验证码：" + Number + "，请输入验证码完成修改。【智订云】";
            }
            string param = "action=send&userid=" + ds.Rows[0]["Userid"] + "&account=" + ds.Rows[0]["account"] + "&password=" + ds.Rows[0]["password"] + "&mobile=" + Phone + "&content=" + EncodeConver(content);
            string url = ds.Rows[0]["url"].ToString();
            string result = SendRequest(url, param);
            SafeXmlDocument xmlDoc = new SafeXmlDocument();
            try
            {
                xmlDoc.LoadXml(result);
                //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                XmlNode root = xmlDoc.SelectSingleNode("//returnsms");//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                if (root != null)
                {
                    string RetureState = (root.SelectSingleNode("returnstatus")).InnerText;
                    string ErrorDescribe = (root.SelectSingleNode("message")).InnerText;
                    string RetureBalance = root.SelectSingleNode("remainpoint").InnerText;
                    string SequenceId = root.SelectSingleNode("taskID").InnerText;
                    string SuccessCounts = root.SelectSingleNode("successCounts").InnerText;
                    if (int.Parse(SuccessCounts) > 0)//发送成功添加记录到短信记录表
                    {
                        Hashtable ht = new Hashtable();
                        ht["MessageType"] = 0;
                        ht["Number"] = "DX" + System.DateTime.Now.ToString("yyMMddHHmmss") + Number;
                        ht["Code"] = Number;//
                        ht["ReceiveType"] = 3;//接收对象类型
                        ht["ReceiveObject"] = "个人（验证码）";//接收对象
                        ht["SendNum"] = 1;//发送短信数量
                        ht["SendUser"] = "智订云";
                        ht["SendContent"] = content;//发送内容
                        ht["SendType"] = 0;//发送类型（0、即时 1、实时）
                        ht["SendMoney"] = SendMoney;
                        ht["DeductionType"] = 1;//扣费类型(0赠送扣除 1营销费扣除 2费用不足够抵扣)
                        ht["SingleMoney"] = SendMoney;//单条短信费用
                        ht["MulTiple"] = 1;//短信倍数
                        ht["State"] = 1;//状态（1审核中、2部分成功、3发送失败、4发送成功）
                        ht["RetureState"] = RetureState;
                        ht["ErrorDescribe"] = ErrorDescribe;
                        ht["RetureBalance"] = RetureBalance;
                        ht["SequenceId"] = SequenceId;
                        ht["SuccessCounts"] = SuccessCounts;
                        ht["PhoneSubmit"] = Phone;
                        ht["HotelName"] = HotelName;
                        ht["AdminHotelid"] = AdminHotelid;
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendRecord", "ID", "", ht);
                        return true;
                    }
                    else
                    {//失败记录
                        Hashtable ht = new Hashtable();
                        ht["MessageType"] = 0;
                        ht["Number"] = "DX" + System.DateTime.Now.ToString("yyMMddHHmmss") + Number;
                        ht["Code"] = Number;//
                        ht["ReceiveType"] = 3;//接收对象类型
                        ht["ReceiveObject"] = "个人（验证码）";//接收对象
                        ht["SendNum"] = 1;//发送短信数量
                        ht["SendUser"] = "智订云";
                        ht["SendContent"] = content;//发送内容
                        ht["SendType"] = 0;//发送类型（0、即时 1、实时）
                        ht["SendMoney"] = SendMoney;
                        ht["DeductionType"] = 1;//扣费类型(0赠送扣除 1营销费扣除 2费用不足够抵扣)
                        ht["SingleMoney"] = SendMoney;//单条短信费用
                        ht["MulTiple"] = 1;//短信倍数
                        ht["State"] = 3;//状态（1审核中、2部分成功、3发送失败、4发送成功）
                        ht["PhoneSubmit"] = Phone;
                        ht["HotelName"] = HotelName;
                        ht["AdminHotelid"] = AdminHotelid;
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendRecord", "ID", "", ht);
                        return false;
                    }
                }
                else
                {

                    Console.WriteLine("the node  is not existed");
                    return false;
                }
            }
            catch (Exception e)
            {
                //显示错误信息  
                Console.WriteLine(e.Message);
                return false;
            }

        }
        else
        {
            // 解析 Result  
            return false;
        }

    }


    public static bool SendSms1(string Phone, string Number, string p1, string i, string p3, int num, string name, string AdminHotelid)
    {
        string content = "";
        string sql = string.Format(@"SELECT ID , Userid ,  Account ,   Password , URL ,  OrganizationID , Name , AdminHotelid from SmsParameter where AdminHotelid='{0}'", AdminHotelid);
        DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
        if (ds != null && ds.Rows.Count > 0)
        {
            if (num == 1)//推送给管理员的
            {
                content = "你好，有客户在官网上预订了客房！订单编号：" + Number + "。入住日期：" + p1 + "，" + i + "晚" + p3 + "间，请登录系统确认。【" + ds.Rows[0]["name"] + "】";
            }

            else if (num == 2)//推送给用户的
            {
                content = "欢迎您预订" + name + "，我们万分期待您到店入住！订单编号：" + Number + "。入住日期：" + p1 + "，" + i + "晚" + p3 + "间。【" + ds.Rows[0]["name"] + "】";
            }
            string param = "action=send&userid=" + ds.Rows[0]["Userid"] + "&account=" + ds.Rows[0]["account"] + "&password=" + ds.Rows[0]["password"] + "&mobile=" + Phone + "&content=" + EncodeConver(content);
            string url = ds.Rows[0]["url"].ToString();
            string result = SendRequest(url, param);

            //XmlDocument xmlDoc = new XmlDocument();
            SafeXmlDocument xmlDoc = new SafeXmlDocument();

            try
            {
                xmlDoc.LoadXml(result);
                //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                XmlNode root = xmlDoc.SelectSingleNode("//returnsms");//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                if (root != null)
                {
                    string Returnstatus = (root.SelectSingleNode("returnstatus")).InnerText;
                    string Message = (root.SelectSingleNode("message")).InnerText;
                    string Remainpoint = root.SelectSingleNode("remainpoint").InnerText;
                    string SuccessCounts = root.SelectSingleNode("successCounts").InnerText;
                    if (SuccessCounts == "1")//发送成功添加记录到短信记录表
                    {
                        int Smscount = 0;
                        string[] str1 = Phone.Split(',');
                        for (int s = 0; s < str1.Length; s++)
                        {
                            Smscount = Smscount + 1;
                        }
                        Hashtable ht = new Hashtable();
                        ht["Phone"] = Phone;
                        ht["Type"] = num;
                        ht["Smscount"] = Smscount;
                        ht["AddTime"] = DateTime.Now;
                        ht["Code"] = Number;
                        ht["State"] = 1;
                        ht["Content"] = content;
                        ht["AdminHotelid"] = AdminHotelid;
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SMSRecord", "ID", "", ht);
                        return true;
                    }
                    else
                    {//失败记录
                        int Smscount = 0;
                        string[] str1 = Phone.Split(',');
                        for (int s = 0; s < str1.Length; s++)
                        {
                            Smscount = Smscount + 1;
                        }
                        Hashtable ht = new Hashtable();
                        ht["Phone"] = Phone;
                        ht["Type"] = num;
                        ht["Smscount"] = Smscount;
                        ht["AddTime"] = DateTime.Now;
                        ht["State"] = 0;
                        ht["Content"] = content;
                        ht["AdminHotelid"] = AdminHotelid;
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SMSRecord", "ID", "", ht);
                        return false;
                    }
                }
                else
                {

                    Console.WriteLine("the node  is not existed");
                    return false;
                }
            }
            catch (Exception e)
            {
                //显示错误信息  
                Console.WriteLine(e.Message);
                return false;
            }

        }
        else
        {
            // 解析 Result  
            return false;
        }

    }

    public static bool SendSms2(string Phone, string Number, int num, string AdminHotelid)
    {
        string content = "";
        string sql = string.Format(@"SELECT ID , Userid ,  Account ,   Password , URL ,  OrganizationID , Name , AdminHotelid from SmsParameter where AdminHotelid='{0}'", AdminHotelid);
        DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
        if (ds != null && ds.Rows.Count > 0)
        {
            if (num == 1)
            {
                content = "你好，有客户在官网上完成了订单支付，订单编号：" + Number + "，请登录系统查看。【" + ds.Rows[0]["name"] + "】";
            }

            else if (num == 2)
            {
                content = "您的订单已支付成功，订单编号：" + Number + "。【" + ds.Rows[0]["name"] + "】";
            }

            string param = "action=send&userid=" + ds.Rows[0]["Userid"] + "&account=" + ds.Rows[0]["account"] + "&password=" + ds.Rows[0]["password"] + "&mobile=" + Phone + "&content=" + EncodeConver(content);
            string url = ds.Rows[0]["url"].ToString();
            string result = SendRequest(url, param);
            // XmlDocument xmlDoc = new XmlDocument();
            SafeXmlDocument xmlDoc = new SafeXmlDocument();

            try
            {
                xmlDoc.LoadXml(result);
                //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                XmlNode root = xmlDoc.SelectSingleNode("//returnsms");//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                if (root != null)
                {
                    string Returnstatus = (root.SelectSingleNode("returnstatus")).InnerText;
                    string Message = (root.SelectSingleNode("message")).InnerText;
                    string Remainpoint = root.SelectSingleNode("remainpoint").InnerText;
                    string SuccessCounts = root.SelectSingleNode("successCounts").InnerText;
                    if (SuccessCounts == "1")//发送成功添加记录到短信记录表
                    {
                        int Smscount = 0;
                        string[] str1 = Phone.Split(',');
                        for (int i = 0; i < str1.Length; i++)
                        {
                            Smscount = Smscount + 1;
                        }
                        Hashtable ht = new Hashtable();
                        ht["Phone"] = Phone;
                        ht["Type"] = num;
                        ht["Smscount"] = Smscount;
                        ht["AddTime"] = DateTime.Now;
                        ht["Code"] = Number;
                        ht["State"] = 1;
                        ht["Content"] = content;
                        ht["AdminHotelid"] = AdminHotelid;
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SMSRecord", "ID", "", ht);
                        return true;
                    }
                    else
                    {//失败记录 
                        int Smscount = 0;
                        string[] str1 = Phone.Split(',');
                        for (int i = 0; i < str1.Length; i++)
                        {
                            Smscount = Smscount + 1;
                        }
                        Hashtable ht = new Hashtable();
                        ht["Phone"] = Phone;
                        ht["Type"] = num;
                        ht["Smscount"] = Smscount;
                        ht["AddTime"] = DateTime.Now;
                        ht["State"] = 0;
                        ht["Content"] = content;
                        ht["AdminHotelid"] = AdminHotelid;
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SMSRecord", "ID", "", ht);
                        return false;
                    }
                }
                else
                {

                    Console.WriteLine("the node  is not existed");
                    return false;
                }
            }
            catch (Exception e)
            {
                //显示错误信息  
                Console.WriteLine(e.Message);
                return false;
            }

        }
        else
        {
            // 解析 Result  
            return false;
        }

    }

    public static bool SendSms3(string Phone, string Number, int money, int num, string AdminHotelid)
    {
        string content = "";
        string sql = string.Format(@"SELECT ID , Userid ,  Account ,   Password , URL ,  OrganizationID , Name , AdminHotelid from SmsParameter where AdminHotelid='{0}'", AdminHotelid);
        DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
        if (ds != null && ds.Rows.Count > 0)
        {
            if (num == 1)
            {
                content = "你好，有客户在微网上完成了会员卡升级，卡号：" + Number + "，请登录系统查看。【" + ds.Rows[0]["name"] + "】";
            }
            else if (num == 2)
            {
                content = "你好，有客户在微网上完成了会员卡充值，卡号：" + Number + "，充值金额：" + money + "，请登录系统查看。【" + ds.Rows[0]["name"] + "】";
            }
            string param = "action=send&userid=" + ds.Rows[0]["Userid"] + "&account=" + ds.Rows[0]["account"] + "&password=" + ds.Rows[0]["password"] + "&mobile=" + Phone + "&content=" + EncodeConver(content);
            string url = ds.Rows[0]["url"].ToString();
            string result = SendRequest(url, param);

            //XmlDocument xmlDoc = new XmlDocument();
            SafeXmlDocument xmlDoc = new SafeXmlDocument();

            try
            {
                xmlDoc.LoadXml(result);
                //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                XmlNode root = xmlDoc.SelectSingleNode("//returnsms");//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                if (root != null)
                {
                    string Returnstatus = (root.SelectSingleNode("returnstatus")).InnerText;
                    string Message = (root.SelectSingleNode("message")).InnerText;
                    string Remainpoint = root.SelectSingleNode("remainpoint").InnerText;
                    string SuccessCounts = root.SelectSingleNode("successCounts").InnerText;
                    if (SuccessCounts == "1")//发送成功添加记录到短信记录表
                    {
                        int Smscount = 0;
                        string[] str1 = Phone.Split(',');
                        for (int i = 0; i < str1.Length; i++)
                        {
                            Smscount = Smscount + 1;
                        }
                        Hashtable ht = new Hashtable();
                        ht["Phone"] = Phone;
                        ht["Type"] = num;
                        ht["Smscount"] = Smscount;
                        ht["AddTime"] = DateTime.Now;
                        ht["Code"] = Number;
                        ht["State"] = 1;
                        ht["Content"] = content;
                        ht["AdminHotelid"] = AdminHotelid;
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SMSRecord", "ID", "", ht);
                        return true;
                    }
                    else
                    {//失败记录
                        int Smscount = 0;
                        string[] str1 = Phone.Split(',');
                        for (int i = 0; i < str1.Length; i++)
                        {
                            Smscount = Smscount + 1;
                        }
                        Hashtable ht = new Hashtable();
                        ht["Phone"] = Phone;
                        ht["Type"] = num;
                        ht["Smscount"] = Smscount;
                        ht["AddTime"] = DateTime.Now;
                        ht["State"] = 0;
                        ht["Content"] = content;
                        ht["AdminHotelid"] = AdminHotelid;
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SMSRecord", "ID", "", ht);
                        return false;
                    }
                }
                else
                {

                    Console.WriteLine("the node  is not existed");
                    return false;
                }
            }
            catch (Exception e)
            {
                //显示错误信息  
                Console.WriteLine(e.Message);
                return false;
            }

            // 解析 Result  

        }
        else
        {
            // 解析 Result  
            return false;
        }

    }



    #endregion

    #region *** 发送短信模块 ***

    /// <summary>
    /// 查询消息模板报备
    /// </summary>
    /// <returns></returns>
    public static void MessageTemplateQuery(string AdminHotelid, string Hotelid)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT  * from SmsParameter where AdminHotelid=@AdminHotelid ");
        SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
        DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (ds != null && ds.Rows.Count > 0)
        {
            string TemplateURL = ds.Rows[0]["TemplateURL"].ToString();
            string MarketingTemplateURL = ds.Rows[0]["MarketingTemplateURL"].ToString();
            string Account = ds.Rows[0]["Account"].ToString();
            string Password = ds.Rows[0]["Password"].ToString();
            string MarketingAccount = ds.Rows[0]["MarketingAccount"].ToString();
            string MarketingPassword = ds.Rows[0]["MarketingPassword"].ToString();
            //读取报备查询状态
            StringBuilder str = new StringBuilder();
            str.Append("SELECT  * from SendMode where AdminHotelid=@AdminHotelid and HotelId=@HotelId and State<>1 ");
            SqlParam[] paramstr = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid), new SqlParam("HotelId", Hotelid) };
            DataTable dstr = DataFactory.SqlDataBase().GetDataTableBySQL(str, paramstr);
            if (dstr != null && dstr.Rows.Count > 0)
            {
                for (int k = 0; k < dstr.Rows.Count; k++)
                {
                    if (dstr.Rows[k]["TemplateId"] != null && dstr.Rows[k]["TemplateId"].ToString() != "")
                    {
                        string ModeId = dstr.Rows[k]["ID"].ToString();
                        string TemplateId = dstr.Rows[k]["TemplateId"].ToString();
                        string MessageType = dstr.Rows[k]["MessageType"].ToString();
                        //http://dx.ipyy.net/freeTemplate.aspx?action=query&account=qq&password=qq&templateid=123
                        string url = "";
                        string sendparam = "";
                        if (MessageType == "0")
                        {
                            url = ds.Rows[0]["TemplateURL"].ToString();
                            sendparam = "action=query&userid=&account=" + Account + "&password=" + Password + "&templateid=" + TemplateId;
                        }
                        else
                        {
                            url = ds.Rows[0]["MarketingTemplateURL"].ToString();
                            sendparam = "action=query&userid=&account=" + MarketingAccount + "&password=" + MarketingPassword + "&templateid=" + TemplateId;
                        }
                        string result = SendRequest(url, sendparam);
                        try
                        {
                            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(result);
                            string Status = jsonObj["Status"].ToString();//查询状态
                            string StatusCode = jsonObj["StatusCode"].ToString();
                            string Description = jsonObj["Description"].ToString();
                            string State = dstr.Rows[k]["State"].ToString();
                            if (jsonObj["TemplateList"] != null)
                            {
                                JArray TemplateList = JArray.Parse(jsonObj["TemplateList"].ToString());
                                JObject TemplateVal = JObject.Parse(TemplateList[0].ToString());
                                State = TemplateVal["Status"].ToString();
                                //更新模板表状态
                                Hashtable ht = new Hashtable();
                                ht["Status"] = Status;
                                ht["StatusCode"] = StatusCode;
                                ht["Description"] = Description;
                                ht["State"] = State;
                                bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendMode", "ID", ModeId, ht);
                            }

                        }
                        catch (Exception e)
                        {
                            //显示错误信息  
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }

    }



    /// <summary>
    /// 添加消息模板报备
    /// </summary>
    /// <returns></returns>
    public static bool MessageTemplateAdd(string MessageType, string MessageContent, string Remarks, string UserId, string UserName, string AdminHotelid, string Hotelid, string ModelId)
    {
        bool Reture = false;
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT  * from SmsParameter where AdminHotelid=@AdminHotelid ");
        SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
        DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (ds != null && ds.Rows.Count > 0)
        {

            string Account = ds.Rows[0]["Account"].ToString();
            string Password = ds.Rows[0]["Password"].ToString();
            string MarketingAccount = ds.Rows[0]["MarketingAccount"].ToString();
            string MarketingPassword = ds.Rows[0]["MarketingPassword"].ToString();
            //http://dx.ipyy.net/freeTemplate.aspx?action=Add&account=qq&password=qq&template=验证码@【华信】
            string url = "";
            string sendparam = "";
            if (MessageType == "0")
            {
                url = ds.Rows[0]["TemplateURL"].ToString();
                sendparam = "action=Add&userid=&account=" + Account + "&password=" + Password + "&template=" + EncodeConver(MessageContent);
            }
            else
            {
                url = ds.Rows[0]["MarketingTemplateURL"].ToString();
                sendparam = "action=Add&userid=&account=" + MarketingAccount + "&password=" + MarketingPassword + "&template=" + EncodeConver(MessageContent);
            }
            string result = SendRequest(url, sendparam);
            try
            {
                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(result);
                string Status = jsonObj["Status"].ToString();//查询状态
                string StatusCode = jsonObj["StatusCode"].ToString();
                string Description = jsonObj["Description"].ToString();
                string TemplateId = jsonObj["TemplateId"].ToString();
                Hashtable ht = new Hashtable();
                ht["MessageType"] = MessageType;
                ht["MessageContent"] = MessageContent;
                ht["SendTime"] = DateTime.Now;
                ht["Remarks"] = Remarks;
                ht["UserId"] = UserId;
                ht["UserName"] = UserName;
                ht["Status"] = Status;
                ht["StatusCode"] = StatusCode;
                ht["Description"] = Description;
                ht["TemplateId"] = TemplateId;
                ht["Hotelid"] = Hotelid;
                ht["AdminHotelid"] = AdminHotelid;
                bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendMode", "ID", ModelId, ht);
                if (IsOk == true)
                {
                    CommonMethod.Base_Log("短信管理", "SendMode", "hotelid:" + Hotelid, "添加短信模板信息", "添加短信模板信息");//操作记录
                    Reture = true;
                }

            }
            catch (Exception e)
            {
                //显示错误信息  
                Console.WriteLine(e.Message);
            }
        }
        return Reture;
    }



    /// <summary>
    ///更新消息模板报备
    /// </summary>
    /// <returns></returns>
    public static bool MessageTemplateUpdate(string MessageType, string MessageContent, string Remarks, string UserId, string UserName, string AdminHotelid, string Hotelid, string ModelId)
    {
        bool Reture = false;
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT  * from SmsParameter where AdminHotelid=@AdminHotelid ");
        SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
        DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (ds != null && ds.Rows.Count > 0)
        {

            string Account = ds.Rows[0]["Account"].ToString();
            string Password = ds.Rows[0]["Password"].ToString();
            string MarketingAccount = ds.Rows[0]["MarketingAccount"].ToString();
            string MarketingPassword = ds.Rows[0]["MarketingPassword"].ToString();

            StringBuilder str = new StringBuilder();
            str.Append("SELECT  * from SendMode where AdminHotelid=@AdminHotelid and HotelId=@HotelId and ID=@ModelId");
            SqlParam[] paramstr = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid), new SqlParam("HotelId", Hotelid), new SqlParam("ModelId", ModelId) };
            DataTable dstr = DataFactory.SqlDataBase().GetDataTableBySQL(str, paramstr);
            if (dstr != null && dstr.Rows.Count > 0)
            {
                if (dstr.Rows[0]["TemplateId"] != null && dstr.Rows[0]["TemplateId"].ToString() != "")
                {
                    string TemplateId = dstr.Rows[0]["TemplateId"].ToString();
                    //http://dx.ipyy.net/freeTemplate.aspx?action=update&account=qq&password=qq&templateid=123& template=123@【华信】
                    string sendparam = "";
                    string url = "";
                    if (MessageType == "0")
                    {
                        url = ds.Rows[0]["TemplateURL"].ToString();
                        sendparam = "action=update&userid=&account=" + Account + "&password=" + Password + "&templateid=" + TemplateId + "&template=" + EncodeConver(MessageContent);
                    }
                    else
                    {
                        url = ds.Rows[0]["MarketingTemplateURL"].ToString();
                        sendparam = "action=update&userid=&account=" + MarketingAccount + "&password=" + MarketingPassword + "&templateid=" + TemplateId + "&template=" + EncodeConver(MessageContent);
                    }
                    string result = SendRequest(url, sendparam);
                    try
                    {
                        JObject jsonObj = (JObject)JsonConvert.DeserializeObject(result);
                        string Status = jsonObj["Status"].ToString();//查询状态
                        string StatusCode = jsonObj["StatusCode"].ToString();
                        string Description = jsonObj["Description"].ToString();
                        if (Status == "Success")
                        {
                            //更新模板表状态
                            Hashtable ht = new Hashtable();
                            ht["MessageType"] = MessageType;
                            ht["MessageContent"] = MessageContent;
                            ht["SendTime"] = DateTime.Now;
                            ht["Remarks"] = Remarks;
                            ht["UserId"] = UserId;
                            ht["UserName"] = UserName;
                            ht["Status"] = Status;
                            ht["StatusCode"] = StatusCode;
                            ht["Description"] = Description;
                            ht["State"] = 0;
                            ht["Hotelid"] = Hotelid;
                            ht["AdminHotelid"] = AdminHotelid;
                            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendMode", "ID", ModelId, ht);
                            if (IsOk == true)
                            {
                                CommonMethod.Base_Log("短信管理", "SendMode", "hotelid:" + Hotelid, "修改短信模板信息", "修改短信模板信息");//操作记录
                                Reture = true;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        //显示错误信息  
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
        return Reture;
    }



    /// <summary>
    ///删除消息模板报备
    /// </summary>
    /// <returns></returns>
    public static bool MessageTemplateDelete(string AdminHotelid, string Hotelid, string ModelId)
    {
        bool Reture = false;
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT  * from SmsParameter where AdminHotelid=@AdminHotelid ");
        SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
        DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (ds != null && ds.Rows.Count > 0)
        {

            string Account = ds.Rows[0]["Account"].ToString();
            string Password = ds.Rows[0]["Password"].ToString();
            string MarketingAccount = ds.Rows[0]["MarketingAccount"].ToString();
            string MarketingPassword = ds.Rows[0]["MarketingPassword"].ToString();

            StringBuilder str = new StringBuilder();
            str.Append("SELECT  * from SendMode where AdminHotelid=@AdminHotelid and HotelId=@HotelId and ID=@ModelId");
            SqlParam[] paramstr = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid), new SqlParam("HotelId", Hotelid), new SqlParam("ModelId", ModelId) };
            DataTable dstr = DataFactory.SqlDataBase().GetDataTableBySQL(str, paramstr);
            if (dstr != null && dstr.Rows.Count > 0)
            {
                if (dstr.Rows[0]["TemplateId"] != null && dstr.Rows[0]["TemplateId"].ToString() != "")
                {
                    string TemplateId = dstr.Rows[0]["TemplateId"].ToString();
                    string MessageType = dstr.Rows[0]["MessageType"].ToString();
                    //http://dx.ipyy.net/freeTemplate.aspx?action=del&account=qq&password=qq&templateid=123
                    string url = "";
                    string sendparam = "";
                    if (MessageType == "0")
                    {
                        url = ds.Rows[0]["TemplateURL"].ToString();
                        sendparam = "action=del&userid=&account=" + Account + "&password=" + Password + "&templateid=" + TemplateId;
                    }
                    else
                    {
                        url = ds.Rows[0]["MarketingTemplateURL"].ToString();
                        sendparam = "action=del&userid=&account=" + MarketingAccount + "&password=" + MarketingPassword + "&templateid=" + TemplateId;
                    }
                    string result = SendRequest(url, sendparam);
                    try
                    {
                        JObject jsonObj = (JObject)JsonConvert.DeserializeObject(result);
                        string Status = jsonObj["Status"].ToString();//查询状态
                        string StatusCode = jsonObj["StatusCode"].ToString();
                        string Description = jsonObj["Description"].ToString();
                        if (Status == "Success")
                        {
                            int IsOk = DataFactory.SqlDataBase().DeleteData("SendMode", "ID", ModelId);
                            if (IsOk > 0)
                            {
                                CommonMethod.Base_Log("短信管理", "SendMode", "hotelid:" + Hotelid, "删除短信模板信息", "删除短信模板信息");//操作记录
                                Reture = true;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        //显示错误信息  
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
        return Reture;
    }


    /// <summary>
    /// 发送短信
    /// </summary>
    /// <returns></returns>
    public static bool MessageSend(string MessageType, string Phone, string SendContent, string SendRecordId, string SendTime, string AdminHotelid)
    {
        string HotelName = "";
        StringBuilder sb_name = new StringBuilder();
        sb_name.Append("SELECT Name from Hotel_Admin where AdminHotelid=@AdminHotelid ");
        SqlParam[] param_name = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
        DataTable ds_name = DataFactory.SqlDataBase().GetDataTableBySQL(sb_name, param_name);
        if (ds_name != null && ds_name.Rows.Count > 0)
        {
            HotelName = ds_name.Rows[0]["Name"].ToString();
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT * from SmsParameter where AdminHotelid=@AdminHotelid ");
        SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
        DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (ds != null && ds.Rows.Count > 0)
        {

            string url = "";
            string Account = ds.Rows[0]["Account"].ToString();
            string Password = ds.Rows[0]["Password"].ToString();
            string MarketingAccount = ds.Rows[0]["MarketingAccount"].ToString();
            string MarketingPassword = ds.Rows[0]["MarketingPassword"].ToString();

            string sendparam = "";
            if (MessageType == "0")
            {
                url = ds.Rows[0]["URL"].ToString();
                sendparam = "action=send&userid=&account=" + Account + "&password=" + Password + "&mobile=" + Phone + "&content=" + EncodeConver(SendContent) + "&sendTime=" + SendTime;
            }
            else
            {
                url = ds.Rows[0]["MarketingURL"].ToString();
                sendparam = "action=send&userid=&account=" + MarketingAccount + "&password=" + MarketingPassword + "&mobile=" + Phone + "&content=" + EncodeConver(SendContent) + "&sendTime=" + SendTime;
            }
            string result = SendRequest(url, sendparam);
            SafeXmlDocument xmlDoc = new SafeXmlDocument();
            try
            {
                xmlDoc.LoadXml(result);
                //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                XmlNode root = xmlDoc.SelectSingleNode("//returnsms");//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                if (root != null)
                {
                    string RetureState = (root.SelectSingleNode("returnstatus")).InnerText;
                    string ErrorDescribe = (root.SelectSingleNode("message")).InnerText;
                    string RetureBalance = root.SelectSingleNode("remainpoint").InnerText;
                    string SequenceId = root.SelectSingleNode("taskID").InnerText;
                    string SuccessCounts = root.SelectSingleNode("successCounts").InnerText;
                    if (int.Parse(SuccessCounts) > 0)//发送成功添加记录到短信记录表
                    {
                        Hashtable ht = new Hashtable();
                        ht["RetureState"] = RetureState;
                        ht["ErrorDescribe"] = ErrorDescribe;
                        ht["RetureBalance"] = RetureBalance;
                        ht["SequenceId"] = SequenceId;
                        ht["SuccessCounts"] = SuccessCounts;
                        ht["PhoneSubmit"] = Phone;
                        ht["State"] = 1;
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendRecord", "ID", SendRecordId.ToString(), ht);
                        return true;
                    }
                    else
                    {   //失败记录
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("the node  is not existed");
                    return false;
                }
            }
            catch (Exception e)
            {
                //显示错误信息  
                Console.WriteLine(e.Message);
                return false;
            }

        }
        else
        {
            // 解析 Result  
            return false;
        }

    }


    /// <summary>
    /// 短信查询   
    /// </summary>
    public static void CheckMessageSend()
    {
        StringBuilder str = new StringBuilder();
        str.Append(string.Format(" SELECT  * FROM  dbo.SendRecord  WHERE DeleteMark=1 and  State=1 and SendBatch=0 "));
        //str.Append(string.Format(" SELECT  * FROM  dbo.SendRecord  WHERE id=1520 "));
        DataTable dstr = DataFactory.SqlDataBase().GetDataTableBySQL(str);
        if (dstr != null && dstr.Rows.Count > 0)
        {

            for (int k = 0; k < dstr.Rows.Count; k++)
            {
                string AdminHotelid = dstr.Rows[k]["AdminHotelid"].ToString();
                string TaskId = dstr.Rows[k]["SequenceId"].ToString();
                string MessageType = dstr.Rows[k]["MessageType"].ToString();
                Log.Info("定时执行短信查询TaskID", "TaskID" + TaskId);
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * from SmsParameter where AdminHotelid=@AdminHotelid ");
                SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (ds != null && ds.Rows.Count > 0)
                {
                    string url = "";
                    string Account = ds.Rows[0]["Account"].ToString();
                    string Password = ds.Rows[0]["Password"].ToString();
                    string MarketingAccount = ds.Rows[0]["MarketingAccount"].ToString();
                    string MarketingPassword = ds.Rows[0]["MarketingPassword"].ToString();
                    string sendparam = "";
                    if (MessageType == "0")
                    {
                        url = ds.Rows[0]["QueryURL"].ToString();
                        sendparam = "action=query&userid=&account=" + Account + "&password=" + Password + "&taskid=" + TaskId;
                    }
                    else
                    {
                        url = ds.Rows[0]["MarketingQueryURL"].ToString();
                        sendparam = "action=query&userid=&account=" + MarketingAccount + "&password=" + MarketingPassword + "&taskid=" + TaskId;
                    }
                    string result = SendRequest(url, sendparam);
                    SafeXmlDocument xmlDoc = new SafeXmlDocument();
                    try
                    {
                        xmlDoc.LoadXml(result);
                        //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                        //XmlNode root = xmlDoc.SelectSingleNode("//returnsms");//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("returnsms").ChildNodes;
                        if (xmlNodeList != null)
                        {

                            string SendId = dstr.Rows[k]["ID"].ToString();// 发送记录表ID
                            string SingleMoney = dstr.Rows[k]["SingleMoney"].ToString();
                            string MulTiple = dstr.Rows[k]["MulTiple"].ToString();
                            string DeductionType = dstr.Rows[k]["DeductionType"].ToString();// 扣费类型(0赠送扣除 1营销费扣除)
                            int SendNum = int.Parse(dstr.Rows[k]["SendNum"].ToString());// 发送短信数量
                            string SendState = dstr.Rows[k]["State"].ToString();// 当前短信发送状态（1审核中、2部分成功、3发送失败、4发送成功）
                            string RetureMoney = "0";
                            string FailureReason = "";
                            string SendRetureVal = "0";
                            int SuccessNum = 0;
                            int FailureNum = 0;
                            string PhoneNumber = "";
                            string MobileNumber = "";
                            foreach (XmlNode list in xmlNodeList)
                            {
                                SendRetureVal = "1";
                                //状态报告----10：发送成功，20：发送失败
                                string MemberPhone = (list.SelectSingleNode("mobile")).InnerText;
                                string RetureState = (list.SelectSingleNode("status")).InnerText;
                                string ReceiveTime = (list.SelectSingleNode("receivetime")).InnerText;
                                string ErrorCode = (list.SelectSingleNode("errorcode")).InnerText;

                                if (int.Parse(RetureState) == 10)//发送成功添加记录到短信记录表
                                {
                                    PhoneNumber += MemberPhone + ",";
                                    SuccessNum += 1;
                                }
                                else if (int.Parse(RetureState) == 20)
                                {
                                    MobileNumber += MemberPhone + ",";
                                    FailureNum += 1;
                                }
                            }
                            if (SendRetureVal == "1")//判断接口是否有返回值
                            {
                                //SuccessNum = SuccessNum * CommonHelper.GetInt(MulTiple);
                                //FailureNum = FailureNum * CommonHelper.GetInt(MulTiple);
                                if (PhoneNumber != "")//发送成功号码
                                {
                                    PhoneNumber = PhoneNumber.Substring(0, PhoneNumber.Length - 1);
                                }
                                if (MobileNumber != "")//发送失败号码
                                {
                                    MobileNumber = MobileNumber.Substring(0, MobileNumber.Length - 1);
                                }
                                if (SendNum == SuccessNum)
                                {
                                    SendState = "4";//发送成功
                                }
                                else if (SendNum == FailureNum)
                                {
                                    SendState = "3";//发送失败
                                    FailureReason = "发送失败";
                                }
                                else if (SuccessNum > 0 && SuccessNum < SendNum)
                                {
                                    SendState = "2";//部分成功
                                }
                                Hashtable ht = new Hashtable();
                                ht["PhoneNumber"] = PhoneNumber;
                                ht["MobileNumber"] = MobileNumber;
                                ht["FailureReason"] = FailureReason;
                                ht["SuccessNum"] = SuccessNum;
                                ht["FailureNum"] = FailureNum;
                                if (SendState == "2" || SendState == "3")//部分成功或发送失败
                                {
                                    if (DeductionType == "0")
                                    {
                                        RetureMoney = "0";
                                    }
                                    else if (DeductionType == "1")
                                    {
                                        int FailSum = SendNum - SuccessNum;
                                        if (FailSum > 0)
                                        {
                                            RetureMoney = (Convert.ToDecimal(SingleMoney) * (Convert.ToDecimal(FailSum) * Convert.ToDecimal(MulTiple))).ToString();
                                        }
                                    }
                                }
                                ht["RetureMoney"] = RetureMoney.ToString();
                                ht["State"] = SendState;
                                bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendRecord", "ID", dstr.Rows[k]["ID"].ToString(), ht);
                            }
                        }
                        else
                        {
                            Console.WriteLine("the node  is not existed");

                        }
                    }
                    catch (Exception e)
                    {
                        //显示错误信息  
                        Console.WriteLine(e.Message);
                    }

                }
            }
        }

    }


    /// <summary>
    /// 批量执行发送短信
    /// </summary>
    /// <returns></returns>
    public static void MessageSendBatch()
    {
        StringBuilder Sql = new StringBuilder();
        Sql.Append("SELECT ID,AdminHotelid,MessageType,SendType,SendTime,SendContent,ReceiveType,BeginRow,EndRow from SendBatch where SendState=0 ");
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(Sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string SendBatchId = dt.Rows[i]["ID"].ToString();
                string AdminHotelid = dt.Rows[i]["AdminHotelid"].ToString();
                string MessageType = dt.Rows[i]["MessageType"].ToString();
                string SendTime = "";
                string SendType = dt.Rows[i]["SendType"].ToString();
                if (SendType == "1")
                {
                    SendTime = dt.Rows[i]["SendTime"].ToString();
                }
                string SendContent = dt.Rows[i]["SendContent"].ToString();
                string ReceiveType = dt.Rows[i]["ReceiveType"].ToString();//接收对象类型(0全部会员1会员等级2个人)
                string BeginRow = dt.Rows[i]["BeginRow"].ToString();//开始行数
                string EndRow = dt.Rows[i]["EndRow"].ToString();//结束行数
                //获取发送的会员
                string sql_member = "";
                if (ReceiveType == "0")
                {
                    sql_member = string.Format(@"select * from (select  row_number() over(order by lsh) as rowNum, sjhm from hy_hyzlxxb where 1=1 and sjhm is not null and  LEN([sjhm])=11) as MenberInfo where (rowNum >" + BeginRow + " and rowNum<=" + EndRow + ") ");
                }
                else if (ReceiveType == "1")
                {
                    string MemberGrade = dt.Rows[i]["MemberGrade"].ToString();//会员等级
                    sql_member = string.Format(@"select * from (select  row_number() over(order by lsh) as rowNum, sjhm from hy_hyzlxxb where 1=1 and sjhm is not null and  LEN([sjhm])=11 and hylx in(" + MemberGrade + ")) as MenberInfo where (rowNum >" + BeginRow + " and rowNum<=" + EndRow + ") ");
                }
                string MemberPhone = "";
                DataTable dt_member = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql_member));
                if (dt_member != null && dt_member.Rows.Count > 0)
                {
                    for (int k = 0; k < dt_member.Rows.Count; k++)
                    {
                        MemberPhone += dt_member.Rows[k]["sjhm"].ToString() + ",";
                    }
                }
                if (MemberPhone != "")
                {
                    MemberPhone = MemberPhone.Substring(0, MemberPhone.Length - 1);
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT Account,Password,URL,MarketingAccount,MarketingPassword,MarketingURL from SmsParameter where AdminHotelid=@AdminHotelid ");
                SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (ds != null && ds.Rows.Count > 0)
                {
                    string url = "";
                    string Account = ds.Rows[0]["Account"].ToString();
                    string Password = ds.Rows[0]["Password"].ToString();
                    string MarketingAccount = ds.Rows[0]["MarketingAccount"].ToString();
                    string MarketingPassword = ds.Rows[0]["MarketingPassword"].ToString();

                    string sendparam = "";
                    if (MessageType == "0")
                    {
                        url = ds.Rows[0]["URL"].ToString();
                        sendparam = "action=send&userid=&account=" + Account + "&password=" + Password + "&mobile=" + MemberPhone + "&content=" + EncodeConver(SendContent) + "&sendTime=" + SendTime;
                    }
                    else
                    {
                        url = ds.Rows[0]["MarketingURL"].ToString();
                        sendparam = "action=send&userid=&account=" + MarketingAccount + "&password=" + MarketingPassword + "&mobile=" + MemberPhone + "&content=" + EncodeConver(SendContent) + "&sendTime=" + SendTime;
                    }
                    string result = SendRequest(url, sendparam);
                    SafeXmlDocument xmlDoc = new SafeXmlDocument();
                    try
                    {
                        xmlDoc.LoadXml(result);
                        //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                        XmlNode root = xmlDoc.SelectSingleNode("//returnsms");//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                        if (root != null)
                        {
                            string RetureState = (root.SelectSingleNode("returnstatus")).InnerText;
                            string ErrorDescribe = (root.SelectSingleNode("message")).InnerText;
                            string RetureBalance = root.SelectSingleNode("remainpoint").InnerText;
                            string SequenceId = root.SelectSingleNode("taskID").InnerText;
                            string SuccessCounts = root.SelectSingleNode("successCounts").InnerText;
                            if (int.Parse(SuccessCounts) > 0)//发送成功添加记录到短信记录表
                            {
                                Hashtable ht = new Hashtable();
                                ht["RetureState"] = RetureState;
                                ht["ErrorDescribe"] = ErrorDescribe;
                                ht["RetureBalance"] = RetureBalance;
                                ht["SequenceId"] = SequenceId;
                                ht["SuccessCounts"] = SuccessCounts;
                                ht["PhoneSubmit"] = MemberPhone;
                                ht["SendState"] = 1;
                                bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendBatch", "ID", SendBatchId.ToString(), ht);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //显示错误信息  
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }


    }


    /// <summary>
    /// 批量短信查询
    /// </summary>
    public static void CheckMessageSendBatch()
    {
        StringBuilder str = new StringBuilder();
        str.Append(string.Format(" SELECT  * FROM  dbo.SendBatch  WHERE  SendState=1 and QueryState=0 "));
        DataTable dstr = DataFactory.SqlDataBase().GetDataTableBySQL(str);
        if (dstr != null && dstr.Rows.Count > 0)
        {

            for (int k = 0; k < dstr.Rows.Count; k++)
            {
                string AdminHotelid = dstr.Rows[k]["AdminHotelid"].ToString();
                string TaskId = dstr.Rows[k]["SequenceId"].ToString();
                string MessageType = dstr.Rows[k]["MessageType"].ToString();
                Log.Info("定时执行短信查询TaskID", "TaskID" + TaskId);
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * from SmsParameter where AdminHotelid=@AdminHotelid ");
                SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (ds != null && ds.Rows.Count > 0)
                {
                    string url = "";
                    string Account = ds.Rows[0]["Account"].ToString();
                    string Password = ds.Rows[0]["Password"].ToString();
                    string MarketingAccount = ds.Rows[0]["MarketingAccount"].ToString();
                    string MarketingPassword = ds.Rows[0]["MarketingPassword"].ToString();
                    string sendparam = "";
                    if (MessageType == "0")
                    {
                        url = ds.Rows[0]["QueryURL"].ToString();
                        sendparam = "action=query&userid=&account=" + Account + "&password=" + Password + "&taskid=" + TaskId;
                    }
                    else
                    {
                        url = ds.Rows[0]["MarketingQueryURL"].ToString();
                        sendparam = "action=query&userid=&account=" + MarketingAccount + "&password=" + MarketingPassword + "&taskid=" + TaskId;
                    }
                    string result = SendRequest(url, sendparam);
                    SafeXmlDocument xmlDoc = new SafeXmlDocument();
                    try
                    {
                        xmlDoc.LoadXml(result);
                        //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                        //XmlNode root = xmlDoc.SelectSingleNode("//returnsms");//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("returnsms").ChildNodes;
                        if (xmlNodeList != null)
                        {

                            string SendId = dstr.Rows[k]["ID"].ToString();// 发送记录表ID
                            string SingleMoney = dstr.Rows[k]["SingleMoney"].ToString();
                            string MulTiple = dstr.Rows[k]["MulTiple"].ToString();
                            string DeductionType = dstr.Rows[k]["DeductionType"].ToString();// 扣费类型(0赠送扣除 1营销费扣除)
                            int SendNum = int.Parse(dstr.Rows[k]["SendNum"].ToString());// 发送短信数量
                            string SendState = dstr.Rows[k]["State"].ToString();// 当前短信发送状态（1审核中、2部分成功、3发送失败、4发送成功）
                            string RetureMoney = "0";
                            string FailureReason = "";
                            string SendRetureVal = "0";
                            int SuccessNum = 0;
                            int FailureNum = 0;
                            string PhoneNumber = "";
                            string MobileNumber = "";
                            foreach (XmlNode list in xmlNodeList)
                            {
                                SendRetureVal = "1";
                                //状态报告----10：发送成功，20：发送失败
                                string MemberPhone = (list.SelectSingleNode("mobile")).InnerText;
                                string RetureState = (list.SelectSingleNode("status")).InnerText;
                                string ReceiveTime = (list.SelectSingleNode("receivetime")).InnerText;
                                string ErrorCode = (list.SelectSingleNode("errorcode")).InnerText;

                                if (int.Parse(RetureState) == 10)//发送成功添加记录到短信记录表
                                {
                                    PhoneNumber += MemberPhone + ",";
                                    SuccessNum += 1;
                                }
                                else if (int.Parse(RetureState) == 20)
                                {
                                    MobileNumber += MemberPhone + ",";
                                    FailureNum += 1;
                                }
                            }
                            if (SendRetureVal == "1")//判断接口是否有返回值
                            {
                                //SuccessNum = SuccessNum * CommonHelper.GetInt(MulTiple);
                                //FailureNum = FailureNum * CommonHelper.GetInt(MulTiple);
                                if (PhoneNumber != "")//发送成功号码
                                {
                                    PhoneNumber = PhoneNumber.Substring(0, PhoneNumber.Length - 1);
                                }
                                if (MobileNumber != "")//发送失败号码
                                {
                                    MobileNumber = MobileNumber.Substring(0, MobileNumber.Length - 1);
                                }
                                if (SendNum == SuccessNum)
                                {
                                    SendState = "4";//发送成功
                                }
                                else if (SendNum == FailureNum)
                                {
                                    SendState = "3";//发送失败
                                    FailureReason = "发送失败";
                                }
                                else if (SuccessNum > 0 && SuccessNum < SendNum)
                                {
                                    SendState = "2";//部分成功
                                }
                                Hashtable ht = new Hashtable();
                                ht["PhoneNumber"] = PhoneNumber;
                                ht["MobileNumber"] = MobileNumber;
                                ht["FailureReason"] = FailureReason;
                                ht["SuccessNum"] = SuccessNum;
                                ht["FailureNum"] = FailureNum;
                                if (SendState == "2" || SendState == "3")//部分成功或发送失败
                                {
                                    if (DeductionType == "0")
                                    {
                                        RetureMoney = "0";
                                    }
                                    else if (DeductionType == "1")
                                    {
                                        int FailSum = SendNum - SuccessNum;
                                        if (FailSum > 0)
                                        {
                                            RetureMoney = (Convert.ToDecimal(SingleMoney) * (Convert.ToDecimal(FailSum) * Convert.ToDecimal(MulTiple))).ToString();
                                        }
                                    }
                                }
                                ht["RetureMoney"] = RetureMoney.ToString();
                                ht["State"] = SendState;
                                ht["QueryState"] = 1;
                                bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendBatch", "ID", dstr.Rows[k]["ID"].ToString(), ht);
                            }
                        }
                        else
                        {
                            Console.WriteLine("the node  is not existed");

                        }
                    }
                    catch (Exception e)
                    {
                        //显示错误信息  
                        Console.WriteLine(e.Message);
                    }

                }
            }
        }

    }


    /// <summary>
    /// 统计批量发送情况
    /// </summary>
    public static void CheckStatisticsBatch()
    {
        StringBuilder str = new StringBuilder();
        str.Append(string.Format(" SELECT  * FROM  dbo.SendRecord  WHERE  SendBatch=1 and BatchQuery=0 "));
        DataTable dstr = DataFactory.SqlDataBase().GetDataTableBySQL(str);
        if (dstr != null && dstr.Rows.Count > 0)
        {

            for (int k = 0; k < dstr.Rows.Count; k++)
            {
                string SendRecordId = dstr.Rows[k]["ID"].ToString();
                string BatchNum = dstr.Rows[k]["BatchNum"].ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT PhoneNumber,SuccessNum,MobileNumber,FailureNum from SendBatch where SendRecordId=@SendRecordId and SendState=1 and QueryState=1 ");
                SqlParam[] param = new SqlParam[] { new SqlParam("SendRecordId", SendRecordId) };
                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
                if (ds != null && ds.Rows.Count > 0)
                {
                    string SendBatchNum = ds.Rows.Count.ToString();
                    if (BatchNum == SendBatchNum)//统计发送情况
                    {
                        string SendId = dstr.Rows[k]["ID"].ToString();// 发送记录表ID
                        string SingleMoney = dstr.Rows[k]["SingleMoney"].ToString();
                        string MulTiple = dstr.Rows[k]["MulTiple"].ToString();
                        string DeductionType = dstr.Rows[k]["DeductionType"].ToString();// 扣费类型(0赠送扣除 1营销费扣除)
                        int SendNum = int.Parse(dstr.Rows[k]["SendNum"].ToString());// 发送短信数量
                        string SendState = dstr.Rows[k]["State"].ToString();// 当前短信发送状态（1审核中、2部分成功、3发送失败、4发送成功）

                        string RetureMoney = "0";
                        string SendRetureVal = "0";
                        int SuccessNum = 0;
                        int FailureNum = 0;
                        string PhoneNumber = "";
                        string MobileNumber = "";
                        for (int i = 0; i < ds.Rows.Count; i++)
                        {
                            SendRetureVal = "1";

                            //成功手机号码
                            PhoneNumber += ds.Rows[i]["PhoneNumber"].ToString() + ",";
                            SuccessNum += int.Parse(ds.Rows[i]["SuccessNum"].ToString());

                            //失败手机号码
                            MobileNumber += ds.Rows[i]["MobileNumber"].ToString() + ",";
                            FailureNum += int.Parse(ds.Rows[i]["FailureNum"].ToString());
                        }

                        if (SendRetureVal == "1")//判断接口是否有返回值
                        {
                            //SuccessNum = SuccessNum * CommonHelper.GetInt(MulTiple);
                            //FailureNum = FailureNum * CommonHelper.GetInt(MulTiple);
                            if (PhoneNumber != "")//发送成功号码
                            {
                                PhoneNumber = PhoneNumber.Substring(0, PhoneNumber.Length - 1);
                            }
                            if (MobileNumber != "")//发送失败号码
                            {
                                MobileNumber = MobileNumber.Substring(0, MobileNumber.Length - 1);
                            }
                            if (SendNum == SuccessNum)
                            {
                                SendState = "4";//发送成功
                            }
                            else if (SendNum == FailureNum)
                            {
                                SendState = "3";//发送失败
                            }
                            else if (SuccessNum > 0 && SuccessNum < SendNum)
                            {
                                SendState = "2";//部分成功
                            }
                            Hashtable ht = new Hashtable();
                            ht["PhoneNumber"] = PhoneNumber;
                            ht["MobileNumber"] = MobileNumber;
                            ht["SuccessNum"] = SuccessNum;
                            ht["FailureNum"] = FailureNum;
                            if (SendState == "2" || SendState == "3")//部分成功或发送失败
                            {
                                if (DeductionType == "0")
                                {
                                    RetureMoney = "0";
                                }
                                else if (DeductionType == "1")
                                {
                                    int FailSum = SendNum - SuccessNum;
                                    if (FailSum > 0)
                                    {
                                        RetureMoney = (Convert.ToDecimal(SingleMoney) * (Convert.ToDecimal(FailSum) * Convert.ToDecimal(MulTiple))).ToString();
                                    }
                                }
                            }
                            ht["RetureMoney"] = RetureMoney.ToString();
                            ht["State"] = SendState;
                            ht["BatchQuery"] = 1;
                            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendRecord", "ID", dstr.Rows[k]["ID"].ToString(), ht);
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// 所有酒店会员生日 推送祝福信息
    /// </summary>
    public static void QueryBirthDay()
    {
        //查询所有酒店
        StringBuilder sqlStr = new StringBuilder();
        sqlStr.Append(string.Format("  SELECT  * FROM Set_Birth  WHERE IsTodaySend=0 Order by ID Desc "));
        DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(sqlStr);
        if (dts != null && dts.Rows.Count > 0)
        {
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                //时间判断
                string CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm"); //当前时间
                string BeginTime = DateTime.Now.ToString("yyyy-MM-dd") + " " + dts.Rows[i]["BeginTime"].ToString(); //开始时间
                string EndTime = DateTime.Now.ToString("yyyy-MM-dd") + " " + dts.Rows[i]["EndTime"].ToString();//结束时间
                if (Convert.ToDateTime(CurrentTime) > Convert.ToDateTime(BeginTime) && Convert.ToDateTime(CurrentTime) < Convert.ToDateTime(EndTime))
                {
                    string AdminHotelid = dts.Rows[i]["AdminHotelid"].ToString();
                    string OpenType = dts.Rows[i]["OpenType"].ToString();
                    if (OpenType.ToString() == "0")//生日祝福 是否开启
                    {
                        string SendMoney = dts.Rows[i]["SendMoney"].ToString();
                        string IsTemplate = dts.Rows[i]["IsTemplate"].ToString();
                        string TemplateId = dts.Rows[i]["TemplateId"].ToString();
                        string SingleMoney = dts.Rows[i]["SingleMoney"].ToString();
                        string MulTiple = dts.Rows[i]["MulTiple"].ToString();
                        string HotelId = dts.Rows[i]["HotelId"].ToString();
                        string HotelName = dts.Rows[i]["HotelName"].ToString();
                        //微信信息
                        string WeChatContent = "";
                        if (dts.Rows[i]["WeChatContent"] != null && dts.Rows[i]["WeChatContent"].ToString() != "")
                        {
                            WeChatContent = dts.Rows[i]["WeChatContent"].ToString() + "【" + dts.Rows[i]["HotelName"].ToString() + "】";
                        }
                        //短信信息
                        string MessageContent = "";
                        if (dts.Rows[i]["MessageContent"] != null && dts.Rows[i]["MessageContent"].ToString() != "")
                        {
                            MessageContent = dts.Rows[i]["MessageContent"].ToString();
                        }
                        string MessageType = dts.Rows[i]["MessageType"].ToString();
                        //0生日当天1一周前2生日当月
                        string SendType = dts.Rows[i]["SendType"].ToString();
                        string SendTime = "";
                        string newTime = DateTime.Now.ToString("yyyy-MM-dd"); //当前时间
                        string SendMonth = "0";//判断当月是否已经发送
                        if (SendType == "1")
                        {
                            //1一周前
                            SendTime = Convert.ToDateTime(newTime).AddDays(-7).ToString("yyyy-MM-dd");
                        }
                        else if (SendType == "2")
                        {
                            //2生日当月
                            DateTime MonthDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);//2019-03-01
                            SendTime = MonthDay.ToString("yyyy-MM-dd");
                            if (MonthDay == Convert.ToDateTime(newTime))//2019-02-20
                            {
                                SendMonth = "0";
                            }
                            else
                            {
                                SendMonth = "1";
                            }
                        }
                        else
                        {
                            //0生日当天
                            SendTime = newTime;
                        }
                        //查询所有酒店所在数据库的会员表
                        int SendNum = 0;
                        StringBuilder sql = new StringBuilder();
                        if (SendType == "2")
                        {
                            sql.AppendFormat("select xm,krsr,sjhm,carid from hy_hyzlxxb where month(krsr) = month('" + SendTime + "') ");
                        }
                        else
                        {
                            sql.AppendFormat("select xm,krsr,sjhm,carid from hy_hyzlxxb where month(krsr) = month('" + SendTime + "') and day(krsr) = day('" + SendTime + "') ");
                        }
                        DataTable dt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (SendMonth == "0")//判断当月是否已经发送
                            {
                                string MemberPhone = "";
                                for (int k = 0; k < dt.Rows.Count; k++)
                                {
                                    //微信信息
                                    if (WeChatContent != "")
                                    {
                                        if (dt.Rows[k]["carid"] != null && dt.Rows[k]["carid"].ToString() != "")
                                        {
                                            string fhz = TemplateMessage.Sentexts(dt.Rows[k]["carid"].ToString(), WeChatContent, AdminHotelid);//推送模板消息给 预订人
                                        }
                                    }
                                    //手机号码
                                    if (dt.Rows[k]["sjhm"] != null && dt.Rows[k]["sjhm"].ToString() != "" && dt.Rows[k]["sjhm"].ToString().Length == 11)//判断手机号码是否正确
                                    {
                                        SendNum += 1;
                                        MemberPhone += dt.Rows[k]["sjhm"].ToString() + ",";
                                    }
                                }

                                string SendRecordMoney = (Convert.ToDouble(SendMoney) * Convert.ToDouble(SendNum)).ToString();
                                string SureSendNum = (Convert.ToDouble(MulTiple) * Convert.ToDouble(SendNum)).ToString();
                                //短信信息
                                if (dts.Rows[i]["IsSend"].ToString() == "0")//判断是否勾选发送短信
                                {
                                    if (MessageContent != "")
                                    {
                                        if (MemberPhone != "")
                                        {
                                            MemberPhone = MemberPhone.Substring(0, MemberPhone.Length - 1);
                                            //添加发送短信记录
                                            Hashtable ht = new Hashtable();
                                            ht["MessageType"] = MessageType;
                                            ht["Number"] = "DX" + System.DateTime.Now.ToString("yyMMddHHmmss");
                                            ht["ReceiveType"] = 4;//接收对象类型
                                            ht["ReceiveObject"] = "生日祝福短信";//接收对象
                                            ht["SendNum"] = SureSendNum;
                                            ht["SendContent"] = MessageContent;
                                            ht["SendType"] = 0;//发送类型（0、即时 1、实时）
                                            ht["SendMoney"] = SendRecordMoney;
                                            ht["DeductionType"] = 1;//扣费类型(0赠送扣除 1营销费扣除 2费用不足够抵扣)
                                            ht["IsTemplate"] = IsTemplate;
                                            ht["TemplateId"] = TemplateId;
                                            ht["SingleMoney"] = SingleMoney;
                                            ht["MulTiple"] = MulTiple;
                                            ht["State"] = 0;
                                            ht["HotelId"] = HotelId;
                                            ht["HotelName"] = HotelName;
                                            ht["AdminHotelid"] = AdminHotelid;
                                            int k = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("SendRecord", ht);
                                            if (k > 0)
                                            {
                                                bool send = CommonMethod.MessageSend(MessageType, MemberPhone, MessageContent, k.ToString(), "", AdminHotelid);
                                            }
                                        }
                                    }
                                }

                                //更新表状态
                                Hashtable hs = new Hashtable();
                                hs["IsTodaySend"] = 1;
                                bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Set_Birth", "ID", dts.Rows[i]["ID"].ToString(), hs);

                            }
                        }
                    }
                }

            }
        }
    }



    /// <summary>
    /// 帮助中心发送短信
    /// </summary>
    /// <returns></returns>
    public static bool HelpSend(string MessageType, string Phone, string SendContent, string SendRecordId, string SendTime, string AdminHotelid)
    {
        string HotelName = "";
        StringBuilder sb_name = new StringBuilder();
        sb_name.Append("SELECT Name from Hotel_Admin where AdminHotelid=@AdminHotelid ");
        SqlParam[] param_name = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
        DataTable ds_name = DataFactory.SqlDataBase().GetDataTableBySQL(sb_name, param_name);
        if (ds_name != null && ds_name.Rows.Count > 0)
        {
            HotelName = ds_name.Rows[0]["Name"].ToString();
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT * from SmsParameter where AdminHotelid=@AdminHotelid ");
        SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
        DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (ds != null && ds.Rows.Count > 0)
        {

            string url = "";
            string Account = ds.Rows[0]["Account"].ToString();
            string Password = ds.Rows[0]["Password"].ToString();
            string MarketingAccount = ds.Rows[0]["MarketingAccount"].ToString();
            string MarketingPassword = ds.Rows[0]["MarketingPassword"].ToString();

            string sendparam = "";
            if (MessageType == "0")
            {
                url = ds.Rows[0]["URL"].ToString();
                sendparam = "action=send&userid=&account=" + Account + "&password=" + Password + "&mobile=" + Phone + "&content=" + EncodeConver(SendContent) + "&sendTime=" + SendTime;
            }
            else
            {
                url = ds.Rows[0]["MarketingURL"].ToString();
                sendparam = "action=send&userid=&account=" + MarketingAccount + "&password=" + MarketingPassword + "&mobile=" + Phone + "&content=" + EncodeConver(SendContent) + "&sendTime=" + SendTime;
            }
            string result = SendRequest(url, sendparam);
            SafeXmlDocument xmlDoc = new SafeXmlDocument();
            try
            {
                xmlDoc.LoadXml(result);
                //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                XmlNode root = xmlDoc.SelectSingleNode("//returnsms");//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                if (root != null)
                {
                    string RetureState = (root.SelectSingleNode("returnstatus")).InnerText;
                    string ErrorDescribe = (root.SelectSingleNode("message")).InnerText;
                    string RetureBalance = root.SelectSingleNode("remainpoint").InnerText;
                    string SequenceId = root.SelectSingleNode("taskID").InnerText;
                    string SuccessCounts = root.SelectSingleNode("successCounts").InnerText;
                    if (int.Parse(SuccessCounts) > 0)//发送成功添加记录到短信记录表
                    {
                        Hashtable ht = new Hashtable();
                        ht["SendSuccess"] = 1;
                        bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("FeedBackInfo", "ID", SendRecordId.ToString(), ht);
                        return true;
                    }
                    else
                    {   //失败记录
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("the node  is not existed");
                    return false;
                }
            }
            catch (Exception e)
            {
                //显示错误信息  
                Console.WriteLine(e.Message);
                return false;
            }

        }
        else
        {
            // 解析 Result  
            return false;
        }

    }


    #endregion

    #region *** 获取默认会员code ***

    public static void CopyVipInfo(string AdminHotelid)
    {
        if (!DataFactory.CheckSqlIsOpen(AdminHotelid, 0))
        {
            return;
        }
        StringBuilder sql = new StringBuilder();
        sql.Append(@"
        SELECT  *
        FROM    hy_hylxbmb
        WHERE   AdminHotelid = @AdminHotelid
        ORDER BY sort DESC");

        SqlParam[] param = new SqlParam[] { 
            new SqlParam("@AdminHotelid", AdminHotelid)
        };
        DataTable dt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql, param);
        if (dt == null)
        {
            return;
        }
        DataFactory.SqlDataBase().DeleteData("hy_hylxbmb", "AdminHotelid", AdminHotelid);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Hashtable ht = new Hashtable();
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Rows[i][dt.Columns[j].ColumnName] != null && dt.Rows[i][dt.Columns[j].ColumnName].ToString() != "")
                {
                    ht.Add(dt.Columns[j].ColumnName, dt.Rows[i][dt.Columns[j].ColumnName]);
                }
            }
            DataFactory.SqlDataBase().InsertByHashtable("hy_hylxbmb", ht);
        }
    }

    /// <summary>
    /// 获取会员等级
    /// </summary>
    /// <returns></returns>
    public static DataTable GetVip(string AdminHotelid)
    {
        DataTable dt = null;
        StringBuilder sql = new StringBuilder();
        sql.Append(@"
            SELECT  REPLACE(hylxcode, ' ', '') AS code ,
                    hylxname AS LevelName,sort
            FROM    hy_hylxbmb
            WHERE   AdminHotelid = @AdminHotelid
            ORDER BY sort DESC");

        SqlParam[] param = new SqlParam[] { 
            new SqlParam("@AdminHotelid", AdminHotelid)
        };
        bool bl = DataFactory.CheckSqlIsOpen(AdminHotelid, 0);
        if (bl)
        {
            dt = DataFactory.SqlDataBase(AdminHotelid, 0).GetDataTableBySQL(sql, param);
        }
        else
        {
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
        }

        return dt;
    }

    public static DataTable GetVip(string AdminHotelid, out bool bl)
    {
        DataTable dt = null;
        StringBuilder sql = new StringBuilder();
        sql.Append(@"
            SELECT  REPLACE(hylxcode, ' ', '') AS code ,
                    hylxname AS LevelName,sort
            FROM    hy_hylxbmb
            WHERE   AdminHotelid = @AdminHotelid
            ORDER BY sort DESC");

        SqlParam[] param = new SqlParam[] { 
           new SqlParam("@AdminHotelid", AdminHotelid)
        };

        bl = DataFactory.CheckSqlIsOpen(AdminHotelid, 0);
        if (bl)
        {
            dt = DataFactory.SqlDataBase(AdminHotelid, 0).GetDataTableBySQL(sql, param);
        }
        else
        {
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
        }
        return dt;
    }

    /// <summary>
    /// 根据条件获取会员等级
    /// </summary>
    /// <returns></returns>
    public static DataTable GetVip(string AdminHotelid, string where)
    {
        DataTable dt = null;

        StringBuilder sql = new StringBuilder();
        sql.AppendFormat(@"
            SELECT  REPLACE(hylxcode, ' ', '') AS hylxcode ,
                    hylxname AS hylxname
            FROM    hy_hylxbmb
            WHERE   AdminHotelid = @AdminHotelid 
            AND {0} 
            ORDER BY sort DESC", where);

        SqlParam[] param = new SqlParam[] { 
            new SqlParam("@AdminHotelid", AdminHotelid)
        };

        bool bl = DataFactory.CheckSqlIsOpen(AdminHotelid, 0);
        if (bl)
        {
            dt = DataFactory.SqlDataBase(AdminHotelid, 0).GetDataTableBySQL(sql, param);
        }
        else
        {
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
        }


        return dt;
    }

    public static DataTable GetVip(string AdminHotelid, string where, out bool bl)
    {
        DataTable dt = null;
        StringBuilder sql = new StringBuilder();
        sql.AppendFormat(@"
            SELECT  REPLACE(hylxcode, ' ', '') AS hylxcode ,
                    hylxname AS hylxname
            FROM    hy_hylxbmb
            WHERE   AdminHotelid = @AdminHotelid 
            AND {0} 
            ORDER BY sort DESC", where);

        SqlParam[] param = new SqlParam[] { 
            new SqlParam("@AdminHotelid", AdminHotelid)
        };

        bl = DataFactory.CheckSqlIsOpen(AdminHotelid, 0);
        if (bl)
        {
            dt = DataFactory.SqlDataBase(AdminHotelid, 0).GetDataTableBySQL(sql, param);
        }
        else
        {
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
        }
        return dt;
    }


    /// <summary>
    /// 获取默认会员code
    /// </summary>
    /// <returns></returns>
    public static string GetVipCode(string adminHotelId)
    {
        string vipCode = "";
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
            SELECT TOP 1
                    REPLACE(hylxcode, ' ', '') AS code ,
                    hylxname AS LevelName
            FROM    hy_hylxbmb
            WHERE   AdminHotelid = @AdminHotelid
            ORDER BY sort DESC");
        SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminHotelId)};
        DataTable dtVip = null;

        bool bl = DataFactory.CheckSqlIsOpen(adminHotelId, 0);
        if (bl)
        {
            dtVip = DataFactory.SqlDataBase(adminHotelId, 0).GetDataTableBySQL(sb, param);
        }
        else
        {
            dtVip = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        }
        if (dtVip != null && dtVip.Rows.Count > 0)
        {
            vipCode = dtVip.Rows[0]["code"].ToString();
        }
        return vipCode;
    }

    public static string GetVipCode(string adminHotelId, out bool bl)
    {
        string vipCode = "";
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
            SELECT TOP 1
                    REPLACE(hylxcode, ' ', '') AS code ,
                    hylxname AS LevelName
            FROM    hy_hylxbmb
            WHERE   AdminHotelid = @AdminHotelid
            ORDER BY sort DESC");
        SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminHotelId)};
        DataTable dtVip = null;

        bl = DataFactory.CheckSqlIsOpen(adminHotelId, 0);
        if (bl)
        {
            dtVip = DataFactory.SqlDataBase(adminHotelId, 0).GetDataTableBySQL(sb, param);
        }
        else
        {
            dtVip = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        }
        if (dtVip != null && dtVip.Rows.Count > 0)
        {
            vipCode = dtVip.Rows[0]["code"].ToString();
        }
        return vipCode;
    }

    /// <summary>
    /// 获取会员信息
    /// </summary>
    /// <param name="AdminHotelid">集团id</param>
    /// <param name="columns">查询 列</param>
    /// <param name="where">条件</param>
    /// <param name="bl">是否正常连接国光</param>
    /// <returns></returns>
    public static DataTable GetVipInfo(string AdminHotelid, string openid)
    {
        DataTable dt = null;
        StringBuilder sql = new StringBuilder();
        sql.Append(@"
        SELECT  a.* ,
                ISNULL(b.hylxname, '微会员') hylxname
        FROM    dbo.hy_hyzlxxb a
                LEFT JOIN hy_hylxbmb b ON a.hylx = b.hylxcode AND b.Adminhotelid = a.AdminHotelid
        WHERE   a.Carid = @openid
        ");
        List<SqlParam> lsp = new List<SqlParam>();
        lsp.Add(new SqlParam("@openid", openid));
        bool bl = DataFactory.CheckSqlIsOpen(AdminHotelid, 0);
        if (bl)
        {
            SqlParam[] param = lsp.ToArray();
            dt = DataFactory.SqlDataBase(AdminHotelid, 0).GetDataTableBySQL(sql, param);
        }
        else
        {
            sql.Append(" AND a.AdminHotelid = @AdminHotelid ");
            lsp.Add(new SqlParam("@AdminHotelid", AdminHotelid));
            SqlParam[] param = lsp.ToArray();
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
        }
        return dt;
    }

    /// <summary>
    /// 根据条件获取会员
    /// </summary>
    /// <returns></returns>
    public static DataTable GetHY(string AdminHotelid, string where, out bool bl)
    {
        DataTable dt = null;

        StringBuilder sql = new StringBuilder();
        sql.AppendFormat(@"
            SELECT  *
            FROM    hy_hyzlxxb
            WHERE   
            {0} 
            ", where);


        bl = DataFactory.CheckSqlIsOpen(AdminHotelid, 0);

        if (bl)
        {
            dt = DataFactory.SqlDataBase(AdminHotelid, 0).GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                tbhy(dt, AdminHotelid);
            }
        }
        else
        {
            sql.AppendFormat(" and AdminHotelid='{0}' ", AdminHotelid);
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
        }
        return dt;
    }

    //同步国光会员
    public static void tbhy(DataTable dt, string AdminHotelid)
    {
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["lsh"].ToString() != "")
            {
                Hashtable ht = new Hashtable();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][dt.Columns[j].ColumnName] != null && dt.Rows[i][dt.Columns[j].ColumnName].ToString() != "")
                    {
                        ht.Add(dt.Columns[j].ColumnName, dt.Rows[i][dt.Columns[j].ColumnName]);
                    }
                }
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@"
                                SELECT  id
                                FROM    hy_hyzlxxb
                                WHERE   
                                {0} 
                                ", " lsh=" + dt.Rows[i]["lsh"] + " and AdminHotelid='" + AdminHotelid + "' ");

                DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
                if (ds != null && ds.Rows.Count > 0)
                {
                    DataFactory.SqlDataBase().UpdateByHashtable("hy_hyzlxxb", "ID", ds.Rows[0][0].ToString(), ht);
                }
                else
                {
                    DataFactory.SqlDataBase().InsertByHashtable("hy_hyzlxxb", ht);
                }
            }
        }
    }

    /// <summary>
    /// 获取系统日期
    /// </summary>
    /// <returns></returns>
    public static string GetSystemTime(string AdminHotelid)
    {
        int BusinessHours = 6;

        HttpContext rq = HttpContext.Current;
        string dtSystemTime = "SystemTime" + AdminHotelid;
        if (rq.Session[dtSystemTime] != null)
        {
            BusinessHours = CommonHelper.GetInt(rq.Session[dtSystemTime]);
        }
        else
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"SELECT BusinessHours FROM Hotel_Admin WHERE AdminHotelid = @AdminHotelid");
            SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", AdminHotelid)};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                BusinessHours = CommonHelper.GetInt(dt.Rows[0][0]);
                rq.Session[dtSystemTime] = BusinessHours;
            }
        }
        string SystemTime = DateTime.Now.AddHours(-BusinessHours).ToString("yyyy-MM-dd");
        return SystemTime;
    }

    /// <summary>
    /// 获取最近一条订单姓名
    /// </summary>
    /// <returns></returns>
    public static void GetOrderName(string openid, out string od_phone, out string od_name)
    {
        od_phone = "";
        od_name = "";
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        SELECT TOP 1
                Name ,
                Mobile
        FROM    dbo.Reservation
        WHERE   openid = @openid
                AND Mobile NOT IN ( SELECT  User_Account
                                    FROM    dbo.Base_UserInfo
                                    WHERE   AdminHotelid = Reservation.AdminHotelid )
        ORDER BY AddTime DESC
        ");
        SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@openid", openid)};
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (dt != null && dt.Rows.Count > 0)
        {
            od_phone = dt.Rows[0]["Mobile"].ToString();
            od_name = dt.Rows[0]["Name"].ToString();
        }
    }
    #endregion

    #region *** 添加积分 ***
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lsh">国光会员id</param>
    /// <param name="phone">手机号码</param>
    /// <param name="hylb">会员类别</param>
    /// <param name="zmsm">账目说明</param>
    /// <param name="Integral">积分数</param>
    /// <param name="bz">备注</param>
    /// <param name="AdminHotelid">集团酒店id</param>
    public static void AddIntegral(string lsh, string phone, string hylb, string zmsm, string Integral, string bz, string AdminHotelid)
    {
        Hashtable hs = new Hashtable();
        hs["lsh"] = lsh;
        hs["kh"] = phone;
        hs["hylb"] = hylb;
        hs["bmcode"] = "99";
        hs["zmsm"] = zmsm;
        hs["jf"] = Integral;
        hs["jzflag"] = "F";
        hs["bz"] = bz;
        hs["czrq"] = DateTime.Now;
        hs["czygh"] = 999999;
        hs["czbc"] = 1;
        hs["AdminHotelid"] = AdminHotelid;
        DataFactory.SqlDataBase(AdminHotelid).InsertByHashtable("hy_hyxfjlb", hs);
    }
    //积分 变动通知
    public static void AddIntegralNotice(string AdminHotelId, string HotelId, string HotelName, string OpenId, string NoticeType, string Moneys, string Integral, string Url, string Remarks)
    {
        Hashtable htAC = new Hashtable();
        htAC["AdminHotelId"] = AdminHotelId;
        htAC["HotelId"] = HotelId;
        htAC["HotelName"] = HotelName;
        htAC["OpenId"] = OpenId;
        htAC["NoticeType"] = NoticeType;
        htAC["Moneys"] = Moneys;
        htAC["Integral"] = Integral;
        htAC["Url"] = Url;
        htAC["Remarks"] = Remarks;
        DataFactory.SqlDataBase().InsertByHashtable("AccountChangeNotice", htAC);
    }
    #endregion

    #region 会员升级
    /// <summary>
    /// 会员升级
    /// </summary>
    public static void UpdateUpgrade(string AdminHotelid, string openid, double xfmoney = 0, int czmoney = 0)
    {
        bool bl = DataFactory.CheckSqlIsOpen(AdminHotelid, 0);
        StringBuilder sqls = new StringBuilder();
        sqls.Append("SELECT xx.lsh,xx.hylx,jb.jb,jb.mrscm,jb.sort FROM hy_hyzlxxb xx,hy_hylxbmb jb WHERE jb.hylxcode=xx.hylx AND xx.CarId='" + openid + "' AND xx.AdminHotelid='" + AdminHotelid + "' ");

        if (bl)//国光数据库连接成功
        {
            DataTable hy = DataFactory.SqlDataBase(AdminHotelid, 0).GetDataTableBySQL(sqls);//会员信息
            if (hy != null && hy.Rows.Count > 0)
            {
                int lsh = CommonHelper.GetInt(hy.Rows[0]["lsh"]);//国光会员ID
                int hyjb = CommonHelper.GetInt(hy.Rows[0]["sort"]);//当前会员级别
                #region//积分升级
                try
                {
                    StringBuilder sql_jf = new StringBuilder();
                    sql_jf.Append("SELECT ISNULL(SUM(jf),0) FROM dbo.hy_hyxfjlb a LEFT JOIN hy_hyzlxxb b ON(a.lsh = b.lsh)  WHERE  b.CarId= @CarId and b.AdminHotelid=@AdminHotelid ");
                    SqlParam[] param_jf = new SqlParam[] {
                                     new SqlParam("@CarId",openid),
            new SqlParam("@AdminHotelid",AdminHotelid)};

                    DataTable jf_dt = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql_jf, param_jf); //累积积分
                    if (jf_dt != null && jf_dt.Rows.Count > 0)
                    {
                        int jf = CommonHelper.GetInt(jf_dt.Rows[0][0]);

                        StringBuilder sql_Upgrade = new StringBuilder();
                        sql_Upgrade.Append("SELECT TOP 1 ID,jb FROM Set_Upgrade WHERE AdminHotelid='" + AdminHotelid + "' AND isjfsjgy=1 AND jfsjgy<=" + jf + " AND jfsjgy>0  ORDER BY jfsjgy DESC ");
                        DataTable ud = DataFactory.SqlDataBase().GetDataTableBySQL(sql_Upgrade);
                        if (ud != null && ud.Rows.Count > 0)
                        {
                            if (Upgrade(AdminHotelid, openid, hyjb, ud.Rows[0]["jb"].ToString()) > 0)
                            {
                                //升级完成
                                Log.Info("积分升级成功", openid + "原等级sort=" + hyjb);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Info("积分升级失败", ex.Message);
                }
                #endregion
                #region//消费升级
                try
                {
                    StringBuilder sql_xfsj = new StringBuilder();
                    sql_xfsj.Append("SELECT ISNULL(SUM(Monery),0) Monery  FROM V_finance WHERE MemberId=" + lsh + " AND Type=1 AND AdminHotelid='" + AdminHotelid + "'");
                    DataTable xf = DataFactory.SqlDataBase().GetDataTableBySQL(sql_xfsj);
                    if (xf != null && CommonHelper.GetInt(xf.Rows[0][0]) > 0)
                    {
                        StringBuilder sql_xf = new StringBuilder();
                        sql_xf.Append("SELECT TOP 1 ID,jb FROM Set_Upgrade WHERE AdminHotelid='" + AdminHotelid + "' AND isxfsjgy=1 AND xfsjgy<=" + CommonHelper.GetInt(xf.Rows[0][0]) + "  ORDER BY xfsjgy DESC ");
                        DataTable ud = DataFactory.SqlDataBase().GetDataTableBySQL(sql_xf);
                        if (ud.Rows.Count > 0)
                        {
                            if (Upgrade(AdminHotelid, openid, hyjb, ud.Rows[0]["jb"].ToString()) > 0)
                            {
                                //升级完成
                                Log.Info("累积消费升级成功", openid + "原等级sort=" + hyjb);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Info("累积消费升级失败", ex.Message);
                }
                #endregion
                #region//充值升级 finance 财务记录表

                try
                {
                    StringBuilder sql_czsj = new StringBuilder();
                    sql_czsj.Append("SELECT ISNULL(SUM(Monery),0) Monery  FROM V_finance WHERE MemberId=" + lsh + " AND Type=4 AND AdminHotelid='" + AdminHotelid + "'");
                    DataTable cz = DataFactory.SqlDataBase().GetDataTableBySQL(sql_czsj);
                    if (cz != null && CommonHelper.GetInt(cz.Rows[0][0]) > 0)
                    {
                        StringBuilder sql_cz = new StringBuilder();
                        sql_cz.Append("SELECT TOP 1 ID,jb FROM Set_Upgrade WHERE AdminHotelid='" + AdminHotelid + "' AND isczsjgy=1 AND czsjgy<=" + CommonHelper.GetInt(cz.Rows[0][0]) + "  ORDER BY czsjgy DESC ");
                        DataTable ud = DataFactory.SqlDataBase().GetDataTableBySQL(sql_cz);
                        if (ud != null && ud.Rows.Count > 0)
                        {
                            if (Upgrade(AdminHotelid, openid, hyjb, ud.Rows[0]["jb"].ToString()) > 0)
                            {
                                //升级完成
                                Log.Info("累积充值升级成功", openid + "原等级sort=" + hyjb);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Info("累积充值升级失败", ex.Message);
                }
                #endregion
                #region//单次消费升级
                try
                {
                    if (xfmoney != 0)
                    {
                        StringBuilder sql_cz = new StringBuilder();
                        sql_cz.Append("SELECT TOP 1 ID,jb FROM Set_Upgrade WHERE AdminHotelid='" + AdminHotelid + "' AND isdcxfsjgy=1 AND dcxfsjgy<=" + CommonHelper.GetInt(xfmoney) + "  ORDER BY dcxfsjgy DESC ");
                        DataTable ud = DataFactory.SqlDataBase().GetDataTableBySQL(sql_cz);
                        if (ud != null && ud.Rows.Count > 0)
                        {
                            if (Upgrade(AdminHotelid, openid, hyjb, ud.Rows[0]["jb"].ToString()) > 0)
                            {
                                //升级完成
                                Log.Info("单次消费升级成功", openid + "原等级sort=" + hyjb);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Info("单次消费升级失败", ex.Message);
                }
                #endregion
                #region//单次充值升级
                try
                {
                    if (czmoney != 0)
                    {
                        StringBuilder sql_cz = new StringBuilder();
                        sql_cz.Append("SELECT TOP 1 ID,jb FROM Set_Upgrade WHERE AdminHotelid='" + AdminHotelid + "' AND isdcczsjgy=1 AND dcczsjgy<=" + CommonHelper.GetInt(czmoney) + "  ORDER BY dcczsjgy DESC ");
                        DataTable ud = DataFactory.SqlDataBase().GetDataTableBySQL(sql_cz);
                        if (ud != null && ud.Rows.Count > 0)
                        {
                            if (Upgrade(AdminHotelid, openid, hyjb, ud.Rows[0]["jb"].ToString()) > 0)
                            {
                                //升级完成
                                Log.Info("单次充值升级成功", openid + "原等级sort=" + hyjb);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Info("单次充值升级失败", ex.Message);
                }
                #endregion
            }
        }
    }

    public static int Upgrade(string AdminHotelid, string CarId, int hyjb, string hylx)
    {
        int i = 0;
        string mb = string.Format(@"SELECT  hylxcode,jb,mrscm,sort  FROM dbo.hy_hylxbmb WHERE sort < {0} and AdminHotelid='{1}' ORDER BY sort DESC ", hyjb, AdminHotelid);
        DataTable dsmb = DataFactory.SqlDataBase(AdminHotelid, 0).GetDataTableBySQL(new StringBuilder(mb));
        if (dsmb != null && dsmb.Rows.Count > 0)
        {
            for (int x = 0; x < dsmb.Rows.Count; x++)
            {
                if (dsmb.Rows[x]["hylxcode"].ToString().Trim() == hylx.Trim())
                {
                    string scmcode = dsmb.Rows[0]["mrscm"].ToString();
                    //微网自定义等级 1是最高级别 如果本身的会员级别大于充值的那么就更新会员级别
                    Hashtable hshy = new Hashtable();
                    hshy["hylx"] = hylx;
                    hshy["scmcode"] = scmcode;
                    i = DataFactory.SqlDataBase(AdminHotelid, 0).UpdateByHashtable("hy_hyzlxxb", "CarId", CarId, hshy);
                }
            }


        }
        return i;
    }



    #endregion

    #region 赠送优惠券
    /// <summary>
    /// 赠送优惠券
    /// </summary>
    /// <param name="AdminHotelid"></param>
    /// <param name="lsh"></param>
    /// <param name="type">注册赠送 单次消费赠送 单次充值赠送 评论赠送 指定赠送 累计消费赠送 累计充值赠送 逗号隔开</param>
    public static void GiveCoupon(string AdminHotelid, string lsh, string type, int je = 0, string ID = "", string ordernum = "")
    {
        try
        {
            string hydj = Gethydj(lsh, AdminHotelid);
            type = "'" + type.Replace("，", ",").Replace(",", "','") + "'";
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * from PreferentialType WHERE IsElectronics=0 and TypeName IN({0})", type);

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sqlcp = string.Format(@"select * from V_XXZSYHQ where TypeName=@TypeName and AdminHotelid=@AdminHotelid ");
                    if (hydj != "") sqlcp += " AND ',' + ishy  LIKE '%," + hydj + ",%' ";
                    SqlParam[] parmAdd3 = new SqlParam[] { 
                                     new SqlParam("@TypeName", dt.Rows[i]["TypeName"].ToString()),
                                     new SqlParam("@AdminHotelid", AdminHotelid)};
                    DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);

                    if (dt.Rows[i]["ID"].ToString() == "2")
                    {
                        //单次消费赠送
                        if (ordernum != "")
                        {
                            StringBuilder sqlzskq = new StringBuilder();
                            sqlzskq.AppendFormat("SELECT r.OrderNum,g.Is_zskq,r.HotelId FROM dbo.Reservation r LEFT JOIN dbo.Guestroom g ON r.RoomId=g.ID WHERE r.OrderNum='{0}' AND g.Is_zskq=1", ordernum);
                            DataTable dtzskq = DataFactory.SqlDataBase().GetDataTableBySQL(sqlzskq);
                            if (dtzskq != null && dtzskq.Rows.Count > 0) //该房型是否参与赠送卡券
                            {
                                sqlcp += "  and FirstMoney<=" + je;
                                sqlcp += "  and (Open_Hotel='0' or ','+Open_Hotel+',' Like '%," + dtzskq.Rows[0]["HotelId"] + ",%' )";
                                dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);
                                AddCardCoupons(lsh, AdminHotelid, "单次消费满" + je + "赠送", dts, dt.Rows[i]["ID"].ToString());
                            }
                        }

                    }
                    else if (dt.Rows[i]["ID"].ToString() == "3")
                    {
                        //单次充值赠送
                        if (ID != "")
                        {
                            sqlcp += "  and SecondMoney<=" + je;
                            sqlcp += "  and CID in (" + ID + ") ";
                            dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);
                            AddCardCoupons(lsh, AdminHotelid, "单次充值满" + je + "赠送", dts, dt.Rows[i]["ID"].ToString());
                        }
                    }
                    else if (dt.Rows[i]["ID"].ToString() == "11")
                    {
                        //累计消费赠送
                        StringBuilder sql_xfsj = new StringBuilder();
                        sql_xfsj.Append("SELECT ISNULL(SUM(Monery),0) Monery  FROM V_finance WHERE MemberId=" + lsh + " AND Type=1 AND AdminHotelid='" + AdminHotelid + "'");
                        DataTable xf = DataFactory.SqlDataBase().GetDataTableBySQL(sql_xfsj);
                        if (xf != null && CommonHelper.GetInt(xf.Rows[0][0]) > 0)
                        {
                            je = CommonHelper.GetInt(xf.Rows[0][0]);
                            sqlcp += "  and FirstMoney<=" + je;
                            dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);
                            AddCardCoupons(lsh, AdminHotelid, "累计消费满" + je + "赠送", dts, dt.Rows[i]["ID"].ToString());
                        }
                    }
                    else if (dt.Rows[i]["ID"].ToString() == "12")
                    {
                        //累计充值赠送
                        StringBuilder sql_czsj = new StringBuilder();
                        sql_czsj.Append("SELECT ISNULL(SUM(Monery),0) Monery  FROM V_finance WHERE MemberId=" + lsh + " AND Type=4 AND AdminHotelid='" + AdminHotelid + "'");
                        DataTable cz = DataFactory.SqlDataBase().GetDataTableBySQL(sql_czsj);
                        if (cz != null && CommonHelper.GetInt(cz.Rows[0][0]) > 0)
                        {
                            je = CommonHelper.GetInt(cz.Rows[0][0]);
                            sqlcp += "  and SecondMoney<=" + je;
                            dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);
                            AddCardCoupons(lsh, AdminHotelid, "累计充值满" + je + "赠送", dts, dt.Rows[i]["ID"].ToString());
                        }
                    }
                    else
                    {
                        AddCardCoupons(lsh, AdminHotelid, dt.Rows[i]["TypeName"].ToString(), dts, dt.Rows[i]["ID"].ToString());
                    }
                }
            }

        }
        catch
        {
        }
    }
    //电子券
    public static void GiveCoupons(string AdminHotelid, string lsh, string type, int je = 0, string ID = "", string ordernum = "")
    {
        try
        {
            string hydj = Gethydj(lsh, AdminHotelid);
            type = "'" + type.Replace("，", ",").Replace(",", "','") + "'";
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * from PreferentialType WHERE IsElectronics=1 and TypeName IN({0})", type);

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sqlcp = string.Format(@"select * from V_XXZSYHQs where TypeName=@TypeName and AdminHotelid=@AdminHotelid ");
                    if (hydj != "") sqlcp += " AND ',' + ishy  LIKE '%," + hydj + ",%' ";
                    SqlParam[] parmAdd3 = new SqlParam[] { 
                                     new SqlParam("@TypeName", dt.Rows[i]["TypeName"].ToString()),
                                     new SqlParam("@AdminHotelid", AdminHotelid)};
                    DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);

                    if (dt.Rows[i]["ID"].ToString() == "2")
                    {
                        //单次消费赠送
                        if (ordernum != "")
                        {
                            StringBuilder sqlzskq = new StringBuilder();
                            sqlzskq.AppendFormat("SELECT r.OrderNum,g.Is_zskq FROM dbo.Reservation r LEFT JOIN dbo.Guestroom g ON r.RoomId=g.ID WHERE r.OrderNum='{0}' AND g.Is_zskq=1", ordernum);
                            DataTable dtzskq = DataFactory.SqlDataBase().GetDataTableBySQL(sqlzskq);
                            if (dtzskq != null && dtzskq.Rows.Count > 0) //该房型是否参与赠送卡券
                            {
                                sqlcp += "  and FirstMoney<=" + je;
                                dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);
                                AddCardCoupons(lsh, AdminHotelid, "单次消费满" + je + "赠送", dts, dt.Rows[i]["ID"].ToString());
                            }
                        }

                    }
                    else if (dt.Rows[i]["ID"].ToString() == "3")
                    {
                        //单次充值赠送
                        if (ID != "")
                        {
                            sqlcp += "  and SecondMoney<=" + je;
                            sqlcp += "  and CID in (" + ID + ") ";
                            dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);
                            AddCardCoupons(lsh, AdminHotelid, "单次充值满" + je + "赠送", dts, dt.Rows[i]["ID"].ToString());
                        }
                    }
                    else if (dt.Rows[i]["ID"].ToString() == "11")
                    {
                        //累计消费赠送
                        StringBuilder sql_xfsj = new StringBuilder();
                        sql_xfsj.Append("SELECT ISNULL(SUM(Monery),0) Monery  FROM V_finance WHERE MemberId=" + lsh + " AND Type=1 AND AdminHotelid='" + AdminHotelid + "'");
                        DataTable xf = DataFactory.SqlDataBase().GetDataTableBySQL(sql_xfsj);
                        if (xf != null && CommonHelper.GetInt(xf.Rows[0][0]) > 0)
                        {
                            je = CommonHelper.GetInt(xf.Rows[0][0]);
                            sqlcp += "  and FirstMoney<=" + je;
                            dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);
                            AddCardCoupons(lsh, AdminHotelid, "累计消费满" + je + "赠送", dts, dt.Rows[i]["ID"].ToString());
                        }
                    }
                    else if (dt.Rows[i]["ID"].ToString() == "12")
                    {
                        //累计充值赠送
                        StringBuilder sql_czsj = new StringBuilder();
                        sql_czsj.Append("SELECT ISNULL(SUM(Monery),0) Monery  FROM V_finance WHERE MemberId=" + lsh + " AND Type=4 AND AdminHotelid='" + AdminHotelid + "'");
                        DataTable cz = DataFactory.SqlDataBase().GetDataTableBySQL(sql_czsj);
                        if (cz != null && CommonHelper.GetInt(cz.Rows[0][0]) > 0)
                        {
                            je = CommonHelper.GetInt(cz.Rows[0][0]);
                            sqlcp += "  and SecondMoney<=" + je;
                            dts = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sqlcp), parmAdd3);
                            AddCardCoupons(lsh, AdminHotelid, "累计充值满" + je + "赠送", dts, dt.Rows[i]["ID"].ToString());
                        }
                    }
                    else
                    {
                        AddCardCoupons(lsh, AdminHotelid, dt.Rows[i]["TypeName"].ToString(), dts, dt.Rows[i]["ID"].ToString());
                    }
                }
            }

        }
        catch
        {
        }
    }

    public static void AddCardCoupons(string lsh, string AdminHotelid, string type, DataTable dt, string PreferentialType)
    {
        //获取会员信息
        string jybj = "";
        string openid = "";
        string Is_Day_ok = "";
        StringBuilder sql = new StringBuilder();
        sql.AppendFormat("SELECT jybj,CarId  FROM hy_hyzlxxb WHERE lsh='{0}' ", lsh);
        DataTable hy = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);
        if (hy != null && hy.Rows.Count > 0)
        {
            jybj = hy.Rows[0]["jybj"].ToString();
            openid = hy.Rows[0]["CarId"].ToString();
        }


        //赠送卡券
        Hashtable hff = new Hashtable();

        if (dt != null && dt.Rows.Count > 0)
        {

            hff["openid"] = openid;
            hff["AdminHotelid"] = AdminHotelid;
            hff["Type"] = type;
            hff["isDelete"] = true;
            hff["isReceive"] = false;
            hff["PreferentialType"] = PreferentialType;

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (dt.Rows[j]["hytype"].ToString() == "1")
                {
                    if (jybj == "T")
                    {
                        continue;
                    }
                }

                if (dt.Rows[j]["hytype"].ToString() == "2")
                {
                    if (jybj != "T")
                    {
                        continue;
                    }
                }
                hff["ServiceRange"] = dt.Rows[j]["ServiceRange"];
                hff["Membergrade"] = dt.Rows[j]["Membergrade"];
                hff["yxrq"] = dt.Rows[j]["yxrq"];
                hff["jjr"] = dt.Rows[j]["jjr"];
                hff["Ishy"] = dt.Rows[j]["Ishy"];
                hff["hytype"] = dt.Rows[j]["hytype"];
                Is_Day_ok = dt.Rows[j]["Is_Day_ok"].ToString();
                int EffectiveType = CommonHelper.GetInt(dt.Rows[j]["EffectiveType"]);
                int EffectiveDay = CommonHelper.GetInt(dt.Rows[j]["EffectiveDay"]);
                hff["Is_Day_ok"] = CommonHelper.GetInt(Is_Day_ok);
                hff["EffectiveType"] = EffectiveType;
                hff["EffectiveDay"] = EffectiveDay;
                hff["UsedMin"] = dt.Rows[j]["UsedMin"];
                DateTime BiginTime = DateTime.Parse(dt.Rows[j]["BiginTime"].ToString());
                DateTime EndinTime = DateTime.Parse(dt.Rows[j]["endinTime"].ToString());
                if (EffectiveType == 2 && EffectiveDay != 0)
                {
                    BiginTime = DateTime.Now;
                    EndinTime = DateTime.Now.AddDays(EffectiveDay);
                }

                if (CommonHelper.GetInt(Is_Day_ok) == 0)
                {
                    BiginTime = BiginTime.AddDays(1);
                }
                hff["CouponID"] = CommonHelper.GetInt(dt.Rows[j]["CID"]);
                hff["Clientid"] = lsh;
                hff["Par"] = CommonHelper.GetInt(dt.Rows[j]["Par"]);
                hff["biginTime"] = BiginTime;
                hff["endinTime"] = EndinTime;
                if (CommonHelper.GetInt(dt.Rows[j]["Num"]) > 1)
                {
                    for (int n = 0; n < CommonHelper.GetInt(dt.Rows[j]["Num"]); n++)
                    {

                        DataFactory.SqlDataBase().InsertByHashtable("ClientCoupon", hff);
                    }
                }
                else
                {
                    DataFactory.SqlDataBase().InsertByHashtable("ClientCoupon", hff);
                }
            }
        }
    }

    private static int GetCardCoupons(string lsh, string AdminHotelid, string Coupons)
    {
        int x = 0;
        StringBuilder sql = new StringBuilder();
        sql.AppendFormat("");

        return x;
    }



    private static string Gethydj(string lsh, string AdminHotelid)
    {
        string hydj = "";
        StringBuilder sql = new StringBuilder();
        sql.AppendFormat("SELECT xx.lsh,xx.hylx,jb.jb,jb.mrscm,jb.sort FROM hy_hyzlxxb xx,hy_hylxbmb jb WHERE jb.hylxcode=xx.hylx AND xx.lsh='" + lsh + "' AND xx.AdminHotelid='" + AdminHotelid + "' ");
        DataTable hy = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(sql);//会员信息
        if (hy != null && hy.Rows.Count > 0)
        {
            hydj = hy.Rows[0]["hylx"].ToString().Trim();
        }

        return hydj;
    }
    #endregion

    #region ***扫酒店二维码***
    /// <summary>
    /// 添加扫码记录 判断是永久还是临时
    /// </summary>
    /// <param name="AdminHotelid"></param>
    /// <param name="openid"></param>
    /// <param name="fjPhone"></param>
    /// <returns></returns>
    public static bool HotelSource(string AdminHotelid, string openid, string Hotelid, out bool isVip)
    {

        isVip = false;
        bool bl = false;

        string sql_hotel = @"SELECT ID,AdminHotelid FROM Hotel WHERE AdminHotelid = @AdminHotelid  AND ID=@ID";
        SqlParam[] parm_hotel = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid",AdminHotelid),
                                     new SqlParam("@ID",Hotelid)};
        DataTable dt_hotel = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql_hotel), parm_hotel);

        if (dt_hotel != null && dt_hotel.Rows.Count > 0)
        {

            string HotelId = dt_hotel.Rows[0]["ID"].ToString();
            //判断是否是会员
            string sql_hyxx = @"select lsh,xm,sjhm from hy_hyzlxxb where CarId=@CarId and AdminHotelid=@AdminHotelid";
            SqlParam[] parm_hyxx = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", AdminHotelid),
                                     new SqlParam("@CarId",openid)};
            DataTable dt_hyxx = DataFactory.SqlDataBase(AdminHotelid).GetDataTableBySQL(new StringBuilder(sql_hyxx), parm_hyxx);
            if (dt_hyxx != null && dt_hyxx.Rows.Count > 0)
            {
                //临时关系
                Hashtable Temporaryht = new Hashtable();
                Temporaryht["name"] = dt_hyxx.Rows[0]["xm"];
                Temporaryht["Phone"] = dt_hyxx.Rows[0]["sjhm"];
                Temporaryht["openid"] = openid;
                Temporaryht["AdminHotelid"] = AdminHotelid;
                Temporaryht["TGType"] = "2";
                //Temporaryht["TGMember"] = userid;
                //Temporaryht["fjPhone"] = User_Account;
                Temporaryht["Source"] = "扫酒店二维码";
                Temporaryht["HotelId"] = HotelId;
                Temporaryht["AddTime"] = DateTime.Now;
                int w = DataFactory.SqlDataBase().InsertByHashtable("MemberSource", Temporaryht);
                isVip = true;
            }
            else
            {
                //永久关系
                Hashtable Temporaryht = new Hashtable();
                Temporaryht["openid"] = openid;
                Temporaryht["AdminHotelid"] = AdminHotelid;
                Temporaryht["HotelId"] = HotelId;
                Temporaryht["TGType"] = "1";
                //Temporaryht["TGMember"] = userid;
                //Temporaryht["fjPhone"] = fjPhone;
                Temporaryht["Source"] = "扫酒店二维码";
                Temporaryht["AddTime"] = DateTime.Now;
                int w = DataFactory.SqlDataBase().InsertByHashtable("MemberSource", Temporaryht);
                isVip = false;
            }
        }
        return bl;
    }

    #endregion

    #region ***绑定推广员工***

    /// <summary>
    /// 添加扫码记录 判断是永久还是临时
    /// </summary>
    /// <param name="AdminHotelid"></param>
    /// <param name="openid"></param>
    /// <param name="fjPhone"></param>
    /// <returns></returns>
    public static void MemberSource(string AdminHotelid, string openid, string fjPhone, bool isVip)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        SELECT  a.User_ID ,
                a.User_Account ,
                a.HotelId ,
                ISNULL(a.openid, '') openid ,
                b.name HotelName
        FROM    Base_UserInfo a
                LEFT JOIN dbo.Hotel b ON a.hotelid = b.ID
        WHERE   a.AdminHotelid = @AdminHotelid
                AND ( a.User_Account = @User_Account
                      OR a.User_ID = @User_ID
                    )
        ");
        SqlParam[] parm_user = new SqlParam[] { 
            new SqlParam("@AdminHotelid",AdminHotelid),
            new SqlParam("@User_ID",fjPhone),
            new SqlParam("@User_Account",fjPhone)
        };
        DataTable dt_user = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm_user);

        if (dt_user != null && dt_user.Rows.Count > 0)
        {
            string userid = dt_user.Rows[0]["User_ID"].ToString();
            string User_Account = dt_user.Rows[0]["User_Account"].ToString();
            string HotelId = dt_user.Rows[0]["HotelId"].ToString();
            string userOpenid = dt_user.Rows[0]["openid"].ToString();
            string HotelName = dt_user.Rows[0]["HotelName"].ToString();

            Hashtable Temporaryht = new Hashtable();
            Temporaryht["name"] = "";
            Temporaryht["Phone"] = "";
            Temporaryht["openid"] = openid;
            Temporaryht["AdminHotelid"] = AdminHotelid;
            Temporaryht["HotelId"] = HotelId;
            Temporaryht["HotelName"] = HotelName;
            Temporaryht["TGType"] = "2";
            Temporaryht["TGMember"] = userid;
            Temporaryht["fjPhone"] = User_Account;
            Temporaryht["Source"] = "扫推广码";
            Temporaryht["AddTime"] = DateTime.Now;
            int w = DataFactory.SqlDataBase().InsertByHashtable("MemberSource", Temporaryht);
            if (w > 0)
            {
                if (userOpenid != "")
                {
                    //扫码成功 发送员工通知
                    RemindStaff(HotelId, userid, userOpenid, openid, isVip);
                }
            }
        }
    }

    public static void D_MemberSource(string AdminHotelid, string openid, string fjPhone, bool isVip)
    {
        Log.Info("分销记录：", fjPhone);
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
            SELECT  a.User_ID ,
                    a.User_Account ,
                    a.HotelId ,
                    ISNULL(a.openid, '') openid ,
                    b.name HotelName
            FROM    Distributor_UserInfo a
                    LEFT JOIN dbo.Hotel b ON a.hotelid = b.ID
            WHERE   a.AdminHotelid = @AdminHotelid
                    AND ( a.User_Account = @User_Account
                          OR a.User_ID = @User_ID
                        )
            ");
        SqlParam[] parm_user = new SqlParam[] { 
                new SqlParam("@AdminHotelid",AdminHotelid),
                new SqlParam("@User_ID",fjPhone),
                new SqlParam("@User_Account",fjPhone)
            };
        DataTable dt_user = DataFactory.SqlDataBase().GetDataTableBySQL(sb, parm_user);

        if (dt_user != null && dt_user.Rows.Count > 0)
        {
            string userid = dt_user.Rows[0]["User_ID"].ToString();
            string User_Account = dt_user.Rows[0]["User_Account"].ToString();
            string HotelId = dt_user.Rows[0]["HotelId"].ToString();
            string userOpenid = dt_user.Rows[0]["openid"].ToString();
            string HotelName = dt_user.Rows[0]["HotelName"].ToString();

            Hashtable Temporaryht = new Hashtable();
            Temporaryht["name"] = "";
            Temporaryht["Phone"] = "";
            Temporaryht["openid"] = openid;
            Temporaryht["AdminHotelid"] = AdminHotelid;
            Temporaryht["HotelId"] = HotelId;
            Temporaryht["HotelName"] = HotelName;
            Temporaryht["TGType"] = "3";
            Temporaryht["TGMember"] = userid;
            Temporaryht["fjPhone"] = User_Account;
            Temporaryht["Source"] = "扫推广码";
            Temporaryht["AddTime"] = DateTime.Now;
            int w = DataFactory.SqlDataBase().InsertByHashtable("Distributor_MemberSource", Temporaryht);
            if (w > 0)
            {
                if (userOpenid != "")
                {
                    //扫码成功 发送员工通知
                    //RemindStaff(HotelId, userid, userOpenid, openid, isVip);
                }
            }
        }
    }

    /// <summary>
    /// 扫码成功 发送员工通知
    /// </summary>
    /// <param name="hotelId"></param>
    /// <param name="userid"></param>
    /// <param name="userOpenid"></param>
    /// <param name="openid"></param>
    /// <param name="isVip"></param>
    private static void RemindStaff(string hotelId, string userid, string userOpenid, string openid, bool isVip)
    {
        bool isOthers = false;

        //查询客人第一次扫描的员工id
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        SELECT TOP 1 TGMember
        FROM    MemberSource
        WHERE   HotelId = @HotelId
                AND openid = @openid
                AND LEN(TGMember) > 4
        ORDER BY AddTime ASC
        ");
        SqlParam[] param = new SqlParam[] { 
            new SqlParam("@HotelId",hotelId),
            new SqlParam("@openid",openid)
        };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (dt != null && dt.Rows.Count > 0)
        {
            if (userid != dt.Rows[0]["TGMember"].ToString())
            {
                isOthers = true;
            }
        }

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> csInfo = new Dictionary<string, object>();

        csInfo.Add("touser", userOpenid);//微信id
        csInfo.Add("template_id", ApplicationHelper.GetAdminTemplateId("关注成功通知"));//推送模板id
        string url = "";
        string title = "恭喜您又有一位客人扫描您的推广二维码！";
        string remark = "";

        if (isVip)//客人是国光会员
        {
            if (isOthers)//之前已经扫过他人推广码
            {
                remark = "客人已与他人绑定关系，客人本次微网消费您将获得奖金";
            }
            else//未扫过他人推广码
            {
                remark = "客人已和您绑定永久关系";
            }
        }
        else//还未注册未会员
        {
            if (isOthers)//之前已经扫过他人推广码
            {
                remark = "客人已与他人绑定关系，客人本次微网消费您将获得奖金";
            }
            else//未扫过他人推广码
            {
                //remark = "客人还未注册，请提醒客人注册会员，才能与您绑定永久关系";
                remark = "客人还未注册，请提醒客人注册会员，才能查看订单详情及获得积分";
            }
        }

        csInfo.Add("url", url);//点击跳转地址

        #region ****** 参数信息 ******

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("first", new Dictionary<string, object>
                    {
                        { "value", title},
                        { "color", "#000" }
                    });
        //
        data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value", "******"},
                        { "color", "#000" }
                    });
        //
        data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value", DateTime.Now.ToString("yyyy-MM-dd HH:mm")},
                        { "color", "#000" }
                    });

        data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", remark},
                        { "color", "#000" }
                    });
        #endregion
        csInfo.Add("data", data);
        string menuInfo = serializer.Serialize(csInfo);
        string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken("1"));
        TemplateMessage.PostWebRequest(postUrl, menuInfo);
    }




    /// <summary>
    /// 绑定推广下级会员
    /// </summary>
    /// <param name="AdminHotelid">集团酒店id</param>
    /// <param name="openid"></param>
    /// <param name="lsh">国光会员id</param>
    public static void SetSalesFans(string AdminHotelid, string openid, string lsh, string xm)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        SELECT TOP 1
                TGMember ,
                fjPhone
        FROM    MemberSource a
                LEFT JOIN Base_UserInfo b ON b.User_ID = a.TGMember
        WHERE   a.AdminHotelid = @AdminHotelid
                AND a.openid = @openid
                AND b.DeleteMark = 1
        ORDER BY a.AddTime ASC
        ");
        SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@openid", openid),
                                     new SqlParam("@AdminHotelid", AdminHotelid)};
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);

        if (dt != null && dt.Rows.Count > 0)
        {
            string userid = dt.Rows[0]["TGMember"].ToString();
            string fjPhone = dt.Rows[0]["fjPhone"].ToString();
            //插入粉丝表
            Hashtable Fht = new Hashtable();
            Fht["sjhm"] = fjPhone;
            Fht["fans_ID"] = lsh;
            Fht["Parent_ID"] = userid;
            Fht["addtime"] = DateTime.Now;
            Fht["type"] = "0";
            Fht["openid"] = openid;
            Fht["AdminHotelid"] = AdminHotelid;
            Fht["hy_from"] = "微信";
            int s = DataFactory.SqlDataBase().InsertByHashtable("Sales_fans", Fht);
            if (s > 0)
            {
                //注册推送拉新奖金
                SendTextMessage(AdminHotelid, openid, userid, xm, "1", "", 1, 1);
            }

        }

        ////注册推送福利
        //TemplateMessage.SenImage(openid, AdminHotelid);
        //注册推送充值消息
        SendTextMessage(AdminHotelid, openid, "", xm, "6", "", 1, 1);


    }

    private static bool CheckFans(string openid, string lsh)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT ID FROM dbo.Sales_fans WHERE openid = @openid");
        SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@openid", openid)};
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (dt != null && dt.Rows.Count > 0)
        {
            Hashtable ht = new Hashtable();
            ht["fans_ID"] = lsh;
            DataFactory.SqlDataBase().UpdateByHashtable("Sales_fans", "ID", dt.Rows[0][0].ToString(), ht);
            return false;
        }
        return true;
    }


    public static void SendTextMessage(string adminhotelid, string openid, string userid, string xm, string typeid, string topupmoney, int number, int days)
    {
        string url = ConfigHelper.GetAppSettings("Url");
        //酒店名称
        string HotelName = "";
        StringBuilder sb_Name = new StringBuilder();
        sb_Name.Append("SELECT Name from Hotel_Admin where AdminHotelid= @AdminHotelid  ");
        SqlParam[] param_Name = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid)};
        DataTable dt_Name = DataFactory.SqlDataBase().GetDataTableBySQL(sb_Name, param_Name);
        if (dt_Name != null && dt_Name.Rows.Count > 0)
        {
            HotelName = dt_Name.Rows[0]["Name"].ToString();
        }
        //获取固定奖金或者比例
        string obtainmoney = GetMarket(adminhotelid, typeid, topupmoney, number, days);
        int dbmn = CommonHelper.GetInt(obtainmoney);
        string Useropenid = "";
        string context = "";
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT openid FROM Base_UserInfo WHERE User_ID = @userid  ");
        SqlParam[] param = new SqlParam[] { 
                                     new SqlParam("@userid", userid)};
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (dt != null && dt.Rows.Count > 0)
        {
            Useropenid = dt.Rows[0]["openid"].ToString();
        }
        if (typeid.ToString() == "1" && dbmn > 0)
        {
            context = "客户[" + xm + "]已经注册成为" + HotelName + "会员，待[" + xm + "]入住后，您将获得" + obtainmoney + "元的拉新奖金";
            TemplateMessage.Sentext(Useropenid, context, "1");
        }
        else if (typeid.ToString() == "2" && dbmn > 0)
        {
            context = "客户[" + xm + "]已经注册成为" + HotelName + "会员，并成功办理了入住，您已获得" + obtainmoney + "元的拉新奖金，快快点击“<a href='" + url + "/Melt/Sale_bonus.aspx'>提现</a>”吧";
            TemplateMessage.Sentext(Useropenid, context, "1");
        }
        else if (typeid.ToString() == "3" && dbmn > 0)
        {
            context = "客户[" + xm + "]已经预订了" + HotelName + "客房，待[" + xm + "]退房后，您将获得" + obtainmoney + "元的售房奖金";
            TemplateMessage.Sentext(Useropenid, context, "1");
        }
        else if (typeid.ToString() == "4" && dbmn > 0)
        {
            context = "客户“" + xm + "”预订了" + HotelName + "客房，并成功办理了退房，您已获得" + obtainmoney + "元的售房奖金，快快点击“<a href='" + url + "/Melt/Sale_bonus.aspx'>提现</a>”吧";
            TemplateMessage.Sentext(Useropenid, context, "1");
        }
        else if (typeid.ToString() == "5" && dbmn > 0)
        {
            context = " 客户[" + xm + "]已经为" + HotelName + "会员卡充值了" + topupmoney + "元，您已获得" + obtainmoney + "元的充值奖金，快快点击“<a href='" + url + "/Melt/Sale_bonus.aspx'>提现</a>”吧";
            TemplateMessage.Sentext(Useropenid, context, "1");
        }
        else if (typeid.ToString() == "6" && dbmn > 0)
        {
            string recharge = url + "/Vipcard/MemCart.aspx?AdminHotelid=" + adminhotelid;
            context = "尊贵的会员，请点击“<a href='" + GetReservation(adminhotelid.ToString()) + "'>订房</a>”进行客房预订吧！";
            TemplateMessage.Sentext(openid, context, adminhotelid);
        }

    }

    #region 推送消息给智订云管理员
    /// <summary>
    /// 推送消息给智订云管理员及员工
    /// </summary>
    /// <param name="orderNumber"></param>
    public static void SendMessage_ZDY(string orderNumber, bool adminSynchro = false)
    {
        string url = ConfigHelper.GetAppSettings("Url");
        StringBuilder sb_Name = new StringBuilder();
        sb_Name.Append(@"
        SELECT  a.Name ,
                b.name HotelName ,
                a.RoomType roomName ,
                a.Number ,
                a.BeginTime ,
                a.EndTime ,
                ISNULL(c.User_Name, '') User_Name ,
                ISNULL(c.openid, '') User_Openid ,
                a.PayType ,
                a.TomePrice ,
                a.ServicingMoney,
                a.AgentServicingMoney,
                a.EstimateMoney ,
                a.RegisterMoney ,
                a.P_EstimateMoney + a.P_RegisterMoney PublicMoney,
                a.AdminHotelid ,
                a.HotelId ,
                a.openid ,
                a.IsScan ,
                a.AddTime,
                ISNULL(a.XX_SKYDDB, '') XX_SKYDDB ,
                ISNULL(a.FristOrderNum,'') FristOrderNum,
                ContinueState,
                a.D_Money,
                a.D_AdminMoney,
                d.User_Name AS fxname,
                d.openid AS fxopenid
        FROM    dbo.Reservation a
                LEFT JOIN dbo.Hotel b ON a.HotelId = b.ID
                LEFT JOIN dbo.Base_UserInfo c ON a.StaffId = c.User_ID
                LEFT JOIN dbo.Distributor_UserInfo d ON a.DistributorId=d.User_ID 
        WHERE   a.OrderNum = @OrderNum
        ");
        SqlParam[] param_Name = new SqlParam[] { 
                                     new SqlParam("@OrderNum", orderNumber)};
        DataTable dt_Name = DataFactory.SqlDataBase().GetDataTableBySQL(sb_Name, param_Name);

        if (dt_Name != null && dt_Name.Rows.Count > 0)
        {
            string Name = dt_Name.Rows[0]["Name"].ToString();//客人
            string HotelName = dt_Name.Rows[0]["HotelName"].ToString();//酒店名称
            string roomName = dt_Name.Rows[0]["roomName"].ToString();//房间名称
            string Number = dt_Name.Rows[0]["Number"].ToString();//房间数
            string rn = roomName + "[" + Number + "间]";
            string BeginTime = CommonHelper.GetDateTime(dt_Name.Rows[0]["BeginTime"]).ToString("yyyy-MM-dd");//预抵日期
            string EndTime = CommonHelper.GetDateTime(dt_Name.Rows[0]["EndTime"]).ToString("yyyy-MM-dd");//预离日期
            string User_Name = dt_Name.Rows[0]["User_Name"].ToString();//员工姓名
            string user_openid = dt_Name.Rows[0]["User_Openid"].ToString();//员工的Openid
            string PayType = dt_Name.Rows[0]["PayType"].ToString();//支付类型
            string TomePrice = dt_Name.Rows[0]["TomePrice"].ToString();//支付房费
            string ServicingMoney = dt_Name.Rows[0]["ServicingMoney"].ToString();//维护费
            double AgentServicingMoney = CommonHelper.GetDouble(dt_Name.Rows[0]["AgentServicingMoney"]);//代理费
            string EstimateMoney = dt_Name.Rows[0]["EstimateMoney"].ToString();//奖金
            double RegisterMoney = CommonHelper.GetDouble(dt_Name.Rows[0]["RegisterMoney"]);//拉新奖金
            double PublicMoney = CommonHelper.GetDouble(dt_Name.Rows[0]["PublicMoney"]);//公共奖金

            string CreateTime = Convert.ToDateTime(dt_Name.Rows[0]["AddTime"]).ToString("yyyy-MM-dd HH:mm:ss");//时间
            string AdminHotelid = dt_Name.Rows[0]["AdminHotelid"].ToString();//集团ID
            string Hotelid = dt_Name.Rows[0]["HotelId"].ToString();//酒店ID

            int ContinueState = CommonHelper.GetInt(dt_Name.Rows[0]["ContinueState"]);//是否续住
            string FristOrderNum = dt_Name.Rows[0]["FristOrderNum"].ToString();//续住首单编号
            string openid = dt_Name.Rows[0]["openid"].ToString();//客人微信id

            string D_openid = dt_Name.Rows[0]["fxopenid"].ToString();//分销的Openid
            string D_Money = dt_Name.Rows[0]["D_Money"].ToString();//分销奖金

            int isc = CommonHelper.GetInt(dt_Name.Rows[0]["IsScan"]);
            string isScan = (isc == 1) ? "扫码" : (isc == 3) ? "异常" : "自主";//是否半小时内扫码下单 0否, 1是
            bool isSynchro = (dt_Name.Rows[0]["XX_SKYDDB"].ToString() == "" || dt_Name.Rows[0]["XX_SKYDDB"].ToString() == "0");//未同步至国光系统

            switch (PayType)
            {
                case "1":
                    TomePrice += "[到店付款][未付款]";
                    RegisterMoney = 0;
                    break;
                case "2":
                    TomePrice += "[积分兑换]";
                    break;
                case "3":
                    TomePrice += "[会员卡支付]";
                    break;
                case "4":
                    TomePrice += "[微信支付]";
                    break;
                case "6":
                    TomePrice += "[使用预售券]";
                    break;
                default:
                    break;
            }
            #region 智订云以及酒店管理员管理员

            string WeekVal = TemplateMessage.GetWeek();//代表星期几
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"
            SELECT  CAST(HotelData AS VARCHAR(50)) HotelData ,
                    OpenId
            FROM    Notice
            WHERE   DeleteMark = 1
                    AND LEN(OpenId) > 4
                    AND AdminHotelid = '{0}'
                    AND Hotelid = '{1}'
                    AND ',' + NoticeVal + ',' LIKE '%,1,%' --客房预订通知
                    AND ',' + WeekVal + ',' LIKE '%,{2},%'--是否在星期内
                    AND ',' + PeriodTimeVal + ',' LIKE '%,{3},%'--是否在时间段内
            UNION  ALL
            SELECT  'admin' ,
                    b.openid
            FROM    Set_WeChat a
                    LEFT JOIN dbo.Base_UserInfo b ON b.User_Account = a.Phone
            WHERE   b.AdminHotelid='1004613' AND LEN(b.openid) > 4 
                    AND 1 = ( SELECT  TOP 1  1
                              FROM      dbo.Hotel_Admin
                              WHERE     StatisticsType = 10
                                        AND AdminHotelid = '{0}'
                            )
            GROUP BY b.openid
            ", AdminHotelid, Hotelid, WeekVal, DateTime.Now.Hour);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            //推送消息给智订云以及酒店管理员管理员
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GetArrangeRoomInfo(dt.Rows[i]["HotelData"].ToString(), dt.Rows[i]["openid"].ToString(), orderNumber, Name, HotelName, rn, BeginTime, EndTime, User_Name, TomePrice, CreateTime, ServicingMoney, AgentServicingMoney, EstimateMoney, ContinueState, FristOrderNum, isScan, PayType, isSynchro, adminSynchro, RegisterMoney, PublicMoney);
                }
            }

            //推送消息给员工
            if (user_openid.Length > 4 && CommonHelper.GetDouble(EstimateMoney) > 0)
            {
                GetArrangeRoomInfo("员工", user_openid, orderNumber, Name, HotelName, rn, BeginTime, EndTime, User_Name, TomePrice, CreateTime, ServicingMoney, AgentServicingMoney, EstimateMoney, ContinueState, FristOrderNum, isScan, PayType, isSynchro, adminSynchro, RegisterMoney, PublicMoney);
            }

            try
            {

                //推送消息给分销员
                if (D_openid.Length > 4 && CommonHelper.GetDouble(D_Money) > 0)
                {
                    GetArrangeRoomInfo("分销", D_openid, orderNumber, Name, HotelName, rn, BeginTime, EndTime, User_Name, TomePrice, CreateTime, ServicingMoney, AgentServicingMoney, D_Money, ContinueState, FristOrderNum, isScan, PayType, isSynchro, adminSynchro, RegisterMoney, PublicMoney);
                }

                //代理
                if (AgentServicingMoney > 0)
                {
                    StringBuilder sqls = new StringBuilder();
                    sqls.AppendFormat(@"
            SELECT tb.*,u.openid,u.zdy_openid from  (SELECT 'agent' as HotelData, (CASE Agencylevel WHEN 1 THEN '推荐人/业务代表' WHEN 2 THEN '一级代理' WHEN 3 THEN '二级代理' WHEN 4 THEN '内部员工' ELSE '' END) AS Agencylevel,User_Name,h.Consumption,h.OrderNumber,a.User_ID 
        FROM Agent_UserInfo a,Hotel_Account_Agent h
         WHERE a.User_ID=h.agentid  AND h.typenum=9) tb,Agent_UserInfo u
         WHERE (tb.User_ID=u.Admin_agent OR tb.User_ID=u.User_ID) AND u.IsPush=1 AND tb.OrderNumber='{0}' ", orderNumber);
                    DataTable dts = DataFactory.SqlDataBase().GetDataTableBySQL(sqls);

                    //推送消息给代理
                    if (dts != null && dts.Rows.Count > 0)
                    {
                        for (int i = 0; i < dts.Rows.Count; i++)
                        {
                            GetArrangeRoomInfo(dts.Rows[i]["HotelData"].ToString(), dts.Rows[i]["zdy_openid"].ToString(), orderNumber, Name, HotelName, User_Name, TomePrice, CreateTime, ServicingMoney, EstimateMoney, ContinueState, FristOrderNum, isScan, PayType);
                        }
                    }
                }

            }
            catch { }

            #endregion
        }
    }

    public static void GetArrangeRoomInfo(string type, string openid, string orderNumber, string Name, string HotelName, string roomName, string BeginTime, string EndTime, string User_Name, string TomePrice, string CreateTime, string ServicingMoney, double AgentServicingMoney, string EstimateMoney, int ContinueState, string FristOrderNum, string isScan, string PayType, bool isSynchro, bool adminSynchro, double RegisterMoney, double PublicMoney)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> csInfo = new Dictionary<string, object>();

        csInfo.Add("touser", openid);//微信id
        csInfo.Add("template_id", ApplicationHelper.GetAdminTemplateId("新订单通知"));//推送模板id
        string url = "";
        string yxt = "预订了";
        if (ContinueState == 1)
        {
            yxt = "续住了";
        }
        string title = "客人[" + ReplaceWithSpecialChar(Name) + "]" + isScan + yxt + "[" + HotelName + "]的客房";
        string title_color = "#000";
        if (isScan == "异常")
        {
            title_color = "#CC0000";
            title = "客人[" + ReplaceWithSpecialChar(Name) + "]" + yxt + "[" + HotelName + "]的客房[扫码失败]";
        }
        string remark = "";
        if (type == "员工")//员工
        {
            url = "http://www.zidinn.com/Melt/Sale_bonus.aspx";
            if (RegisterMoney > 0)
            {
                remark = "恭喜您获得了" + RegisterMoney + "元拉新奖金，";
            }
            remark += "待[" + ReplaceWithSpecialChar(Name) + "]退房后，您将获得" + EstimateMoney + "元售房奖金";
        }
        else if (type == "admin")//智订云超级管理员
        {
            if (CommonHelper.GetDouble(ServicingMoney) == 0)
            {
                return;
            }

            url = "http://www.zidinn.com/Melt/Sale_data.aspx";
            if (string.IsNullOrEmpty(User_Name))
            {
                remark = "智订云获得" + ServicingMoney + "元维护费，客人自主关注预订";
                string agent = GetAgent(orderNumber);//代理信息
                remark += agent;
            }
            else
            {
                if (PayType == "1")
                {
                    remark = "智订云预计获得" + ServicingMoney + "元维护费，员工[" + User_Name + "]预计获得" + EstimateMoney + "元售房奖金";
                    if (PublicMoney > 0)
                    {
                        remark += "，预计获得" + PublicMoney + "元公共奖金";
                    }
                    if (AgentServicingMoney > 0)
                    {
                        remark += "，代理预计获得" + AgentServicingMoney + "元分成";
                    }
                }
                else
                {
                    remark = "智订云获得" + ServicingMoney + "元维护费，员工[" + User_Name + "]预计获得" + EstimateMoney + "元售房奖金";
                    if (RegisterMoney > 0)
                    {
                        remark = "智订云获得" + ServicingMoney + "元维护费，员工[" + User_Name + "]获得" + RegisterMoney + "元拉新奖金，预计获得" + EstimateMoney + "元售房奖金";
                    }
                    if (PublicMoney > 0)
                    {
                        remark += "，预计获得" + PublicMoney + "元公共奖金";
                    }
                    string agent = GetAgent(orderNumber);//代理信息
                    remark += agent;
                }
            }
        }
        else if (type == "分销")
        {
            url = "http://www.zidinn.com/Melt/Distributor/Distributor_bonus.aspx";
            remark = "待[" + ReplaceWithSpecialChar(Name) + "]退房后，您将获得" + EstimateMoney + "元售房奖金";
        }
        else//酒店管理员
        {
            url = "http://www.zidinn.com/Melt/Hotel/HotelData.aspx";
            if (PublicMoney > 0)
            {
                remark = "预计获得" + PublicMoney + "元公共奖金，";
            }
            if (type == "0")//没有数据统计权限
            {
                url = "";
            }
            if (string.IsNullOrEmpty(User_Name))
            {
                remark += "客人自主关注预订";
            }
            else
            {
                remark += "员工[" + User_Name + "]预计获得" + EstimateMoney + "元售房奖金";
            }
            //remark = "有最新订单，请及时处理";
        }
        string order_color = "#000";
        if (isSynchro)
        {
            orderNumber += "[未同步]";
            order_color = "#CC0000";
        }
        if (adminSynchro)
        {
            orderNumber += "[已手动同步]";
            order_color = "#00B000";
        }
        csInfo.Add("url", url);//点击跳转地址

        #region ****** 参数信息 ******

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("first", new Dictionary<string, object>
                    {
                        { "value", title},
                        { "color", title_color }
                    });
        //
        data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value", orderNumber},
                        { "color", order_color }
                    });
        //
        data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value", HotelName},
                        { "color", "#000" }
                    });
        //
        data.Add("keyword3", new Dictionary<string, object>
                    {
                        { "value", roomName},
                        { "color", "#000" }
                    });
        //
        data.Add("keyword4", new Dictionary<string, object>
                    {
                        { "value", BeginTime + "至" + EndTime},
                        { "color", "#000" }
                    });
        //
        data.Add("keyword5", new Dictionary<string, object>
                    {
                        { "value", TomePrice},
                        { "color", "#000" }
                    });
        //
        data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", remark},
                        { "color", "#000" }
                    });
        #endregion
        csInfo.Add("data", data);
        string menuInfo = serializer.Serialize(csInfo);
        string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken("1"));
        TemplateMessage.PostWebRequest(postUrl, menuInfo);
    }

    public static void GetArrangeRoomInfo(string type, string openid, string orderNumber, string Name, string HotelName, string User_Name, string TomePrice, string CreateTime, string ServicingMoney, string EstimateMoney, int ContinueState, string FristOrderNum, string isScan, string PayType)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> csInfo = new Dictionary<string, object>();

        csInfo.Add("touser", openid);//微信id
        csInfo.Add("template_id", "FQdt92czUD5qcjGk3DdWH2zU4eotrRj9VGfNy5TXTEc");//推送模板id
        string url = "";
        string yxt = "预订了";
        string agent = GetAgent(orderNumber, openid, PayType);//代理信息
        if (ContinueState == 1)
        {
            yxt = "续住了";
            orderNumber = FristOrderNum;
        }
        string title = "客人[" + ReplaceWithSpecialChar(Name) + "]" + isScan + yxt + "[" + HotelName + "]的客房";
        string remark = "";
        if (type == "agent")//代理 
        {
            url = "http://www.zidinn.com/Melt/Sale_Agency_Data.aspx";

            if (string.IsNullOrEmpty(User_Name))
            {
                remark = agent + "，客人自主关注预订";
            }
            else
            {
                remark = agent + "，员工[" + User_Name + "]获得" + EstimateMoney + "元售房奖金";
            }
        }

        csInfo.Add("url", url);//点击跳转地址

        #region ****** 参数信息 ******

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("first", new Dictionary<string, object>
                    {
                        { "value", title},
                        { "color", "#000" }
                    });
        //
        data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value",orderNumber},
                        { "color", "#000" }
                    });
        //
        data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value", CreateTime},
                        { "color", "#000" }
                    });
        //
        data.Add("keyword3", new Dictionary<string, object>
                    {
                        { "value", TomePrice},
                        { "color", "#000" }
                    });
        //

        data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", remark},
                        { "color", "#000" }
                    });
        #endregion
        csInfo.Add("data", data);
        string menuInfo = serializer.Serialize(csInfo);
        string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken("1010086"));
        TemplateMessage.PostWebRequest(postUrl, menuInfo);
    }

    public static string GetAgent(string order)
    {
        string txt = "";
        StringBuilder sql = new StringBuilder();
        sql.AppendFormat("SELECT  (CASE Agencylevel WHEN 1 THEN '推荐人/业务代表' WHEN 2 THEN '一级代理' WHEN 3 THEN '二级代理' WHEN 4 THEN '内部员工' ELSE '' END) AS Agencylevel,User_Name,h.Consumption FROM Agent_UserInfo a,Hotel_Account_Agent h WHERE a.User_ID=h.agentid AND h.orderNumber='{0}' ", order);
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt += "，" + dt.Rows[i]["Agencylevel"] + "[" + dt.Rows[i]["User_Name"] + "]获得" + dt.Rows[i]["Consumption"] + "元分成";
            }
        }
        return txt;
    }


    public static string GetAgent(string order, string openid, string PayType)
    {
        string txt = "";
        StringBuilder sql = new StringBuilder();
        sql.AppendFormat(@"SELECT tb.*,u.openid,u.zdy_openid from  (SELECT 'agent' as HotelData, (CASE Agencylevel WHEN 1 THEN '推荐人/业务代表' WHEN 2 THEN '一级代理' WHEN 3 THEN '二级代理' WHEN 4 THEN '内部员工' ELSE '' END) AS Agencylevel,User_Name,h.Consumption,h.OrderNumber,a.User_ID 
        FROM Agent_UserInfo a,Hotel_Account_Agent h
         WHERE a.User_ID=h.agentid  AND h.typenum=9) tb,Agent_UserInfo u
         WHERE (tb.User_ID=u.Admin_agent OR tb.User_ID=u.User_ID) AND u.IsPush=1 AND tb.OrderNumber='{0}' AND u.zdy_openid='{1}' ", order, openid);
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
        if (dt != null && dt.Rows.Count > 0)
        {

            txt = dt.Rows[0]["Agencylevel"] + "[" + dt.Rows[0]["User_Name"] + "]" + (PayType == "1" ? "预计" : "") + "获得" + dt.Rows[0]["Consumption"] + "元分成";

        }
        return txt;
    }
    #endregion



    #region 预售券 推送消息给智订云管理员
    /// <summary>
    /// 推送消息给智订云管理员及员工
    /// </summary>
    /// <param name="orderNumber"></param>
    public static void TaoPiaoMeg_ZDY(string orderNumber)
    {
        string url = ConfigHelper.GetAppSettings("Url");
        StringBuilder sb_Name = new StringBuilder();
        sb_Name.Append(@"
        SELECT  b.name HotelName ,
                a.Name ,
                a.Number ,
                a.StartDate ,
                a.EndDate ,
                a.PayType ,
                a.PayMoney ,
                a.ServicingMoney ,
                a.AdminHotelid ,
                a.HotelId ,
                a.openid ,
                a.AddTime ,
                a.StaffBonus ,
                ISNULL(c.User_Name, '') User_Name ,
                ISNULL(c.openid, '') User_Openid ,
                a.MemberId ,
                a.MemberBonus ,
                ISNULL(d.openid, '') Member_Openid ,
                ISNULL(d.Phone, '') MemberPhone
        FROM    dbo.TP_Order a
                LEFT JOIN dbo.Hotel b ON a.HotelId = b.ID
                LEFT JOIN dbo.Base_UserInfo c ON a.StaffId = c.User_ID
                LEFT JOIN dbo.MemberInfo d ON a.MemberId = d.MemberId
        WHERE   a.OrderNum = @OrderNum
        ");
        SqlParam[] param_Name = new SqlParam[] { 
                                     new SqlParam("@OrderNum", orderNumber)};
        DataTable dt_Name = DataFactory.SqlDataBase().GetDataTableBySQL(sb_Name, param_Name);

        if (dt_Name != null && dt_Name.Rows.Count > 0)
        {
            string AdminHotelid = dt_Name.Rows[0]["AdminHotelid"].ToString();//集团ID
            string Hotelid = dt_Name.Rows[0]["HotelId"].ToString();//酒店ID

            string Name = dt_Name.Rows[0]["Name"].ToString();//客人
            string HotelName = dt_Name.Rows[0]["HotelName"].ToString();//酒店名称
            string StartDate = CommonHelper.GetDateTime(dt_Name.Rows[0]["StartDate"]).ToString("yyyy-MM-dd");//预抵日期
            string EndDate = CommonHelper.GetDateTime(dt_Name.Rows[0]["EndDate"]).ToString("yyyy-MM-dd");//预离日期
            string User_Name = dt_Name.Rows[0]["User_Name"].ToString();//员工姓名
            string user_openid = dt_Name.Rows[0]["User_Openid"].ToString();//员工的Openid
            string PayType = dt_Name.Rows[0]["PayType"].ToString();//支付类型
            string TomePrice = dt_Name.Rows[0]["PayMoney"].ToString();//支付房费
            TomePrice += "[微信支付]";
            string ServicingMoney = dt_Name.Rows[0]["ServicingMoney"].ToString();//维护费
            string StaffBonus = dt_Name.Rows[0]["StaffBonus"].ToString();//员工奖金
            string MemberBonus = dt_Name.Rows[0]["MemberBonus"].ToString();//会员奖金
            string MemberPhone = dt_Name.Rows[0]["MemberPhone"].ToString();//会员手机
            string Member_Openid = dt_Name.Rows[0]["Member_Openid"].ToString();//会员Openid

            string CreateTime = Convert.ToDateTime(dt_Name.Rows[0]["AddTime"]).ToString("yyyy-MM-dd HH:mm:ss");//时间


            #region 智订云以及酒店管理员管理员

            string WeekVal = TemplateMessage.GetWeek();//代表星期几
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"
            SELECT  CAST(HotelData AS VARCHAR(50)) HotelData ,
                    OpenId
            FROM    Notice
            WHERE   DeleteMark = 1
                    AND LEN(OpenId) > 4
                    AND AdminHotelid = '{0}'
                    AND Hotelid = '{1}'
                    AND ',' + NoticeVal + ',' LIKE '%,1,%' --客房预订通知
                    AND ',' + WeekVal + ',' LIKE '%,{2},%'--是否在星期内
                    AND ',' + PeriodTimeVal + ',' LIKE '%,{3},%'--是否在时间段内
            UNION  ALL
            SELECT  'admin' ,
                    b.openid
            FROM    Set_WeChat a
                    LEFT JOIN dbo.Base_UserInfo b ON b.User_Account = a.Phone
            WHERE   b.AdminHotelid='1004613' AND LEN(b.openid) > 4 
                    AND 1 = ( SELECT  TOP 1  1
                              FROM      dbo.Hotel_Admin
                              WHERE     StatisticsType = 10
                                        AND AdminHotelid = '{0}'
                            )
            GROUP BY b.openid
            ", AdminHotelid, Hotelid, WeekVal, DateTime.Now.Hour);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            //推送消息给智订云以及酒店管理员管理员
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TaoPiaoMeg_ZDY(dt.Rows[i]["HotelData"].ToString(), dt.Rows[i]["openid"].ToString(), orderNumber, Name, HotelName, StartDate, EndDate, User_Name, TomePrice, CreateTime, ServicingMoney, StaffBonus, MemberBonus, MemberPhone);
                }
            }

            //推送消息给员工
            if (user_openid.Length > 4 && CommonHelper.GetDouble(StaffBonus) > 0)
            {
                TaoPiaoMeg_ZDY("员工", user_openid, orderNumber, Name, HotelName, StartDate, EndDate, User_Name, TomePrice, CreateTime, ServicingMoney, StaffBonus, MemberBonus, MemberPhone);
            }
            //推送消息给 会员
            if (Member_Openid.Length > 4 && CommonHelper.GetDouble(MemberBonus) > 0)
            {
                TaoPiaoMeg_ZDY("会员", Member_Openid, orderNumber, Name, HotelName, StartDate, EndDate, User_Name, TomePrice, CreateTime, ServicingMoney, StaffBonus, MemberBonus, MemberPhone, AdminHotelid);
            }

            #endregion
        }
    }

    public static void TaoPiaoMeg_ZDY(string type, string openid, string orderNumber, string Name, string HotelName, string StartDate, string EndDate, string User_Name, string TomePrice, string CreateTime, string ServicingMoney, string StaffBonus, string MemberBonus, string MemberPhone, string adminHotelid = "")
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> csInfo = new Dictionary<string, object>();
        string url = "";
        string remark = "";

        csInfo.Add("touser", openid);//微信id
        string title = "客人[" + ReplaceWithSpecialChar(Name) + "]购买了[" + HotelName + "]的预售券";

        string template_id = ApplicationHelper.GetAdminTemplateId("通用新订单通知");
        string token = TemplateMessage.GetAccessToken("1");
        if (type == "会员")
        {
            url = "http://www.zidinn.com/Members/Commision/MarketingCenter.aspx?AdminHotelid=" + adminHotelid;
            remark = "恭喜您获得" + MemberBonus + "元推广奖金";
            //全员推广 会员消息发送到各酒店
            template_id = TemplateMessage.GetTemplateid("新订单通知", adminHotelid);
            token = TemplateMessage.GetAccessToken(adminHotelid);
        }
        else if (type == "员工")//员工
        {
            url = "http://www.zidinn.com/Melt/Sale_bonus.aspx";
            remark = "恭喜您获得" + StaffBonus + "元推广奖金";
        }
        else if (type == "admin")//智订云超级管理员
        {
            if (CommonHelper.GetDouble(ServicingMoney) == 0)
            {
                return;
            }
            url = "http://www.zidinn.com/Melt/Sale_data.aspx";
            remark = "智订云获得" + ServicingMoney + "元维护费，客人自主关注购买";

            if (!string.IsNullOrEmpty(User_Name))
            {
                remark = "智订云获得" + ServicingMoney + "元维护费，员工[" + User_Name + "]获得" + StaffBonus + "元推广奖金";
            }
            else if (!string.IsNullOrEmpty(MemberPhone))
            {
                remark = "智订云获得" + ServicingMoney + "元维护费，会员[" + MemberPhone + "]获得" + MemberBonus + "元推广奖金";
            }

            string agent = GetAgent(orderNumber);//代理信息
            remark += agent;
        }
        else//酒店管理员
        {
            url = "http://www.zidinn.com/Melt/Hotel/HotelData.aspx";
            if (type == "0")//没有数据统计权限
            {
                url = "";
            }
            remark = "客人自主关注购买";
            if (!string.IsNullOrEmpty(User_Name))
            {
                remark = "员工[" + User_Name + "]获得" + StaffBonus + "元推广奖金";
            }
            else if (!string.IsNullOrEmpty(MemberPhone))
            {
                remark = "会员[" + MemberPhone + "]获得" + MemberBonus + "元推广奖金";
            }
        }

        csInfo.Add("template_id", template_id);//推送模板id
        csInfo.Add("url", url);//点击跳转地址

        #region ****** 参数信息 ******

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("first", new Dictionary<string, object>
                    {
                        { "value", title},
                        { "color", "#000" }
                    });
        //酒店名称
        data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value", HotelName},
                        { "color", "#000" }
                    });
        //订单类型
        data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value", "预售券订单"},
                        { "color", "#000" }
                    });
        //订单编号
        data.Add("keyword3", new Dictionary<string, object>
                    {
                        { "value", orderNumber},
                        { "color", "#000" }
                    });
        //下单时间
        data.Add("keyword4", new Dictionary<string, object>
                    {
                        { "value",CreateTime},
                        { "color", "#000" }
                    });

        remark = "订单金额：" + TomePrice + "\n" + remark;

        data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", remark},
                        { "color", "#000" }
                    });
        #endregion
        csInfo.Add("data", data);
        string menuInfo = serializer.Serialize(csInfo);
        string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        postUrl = string.Format(postUrl, token);
        TemplateMessage.PostWebRequest(postUrl, menuInfo);
    }

    #endregion



    /// <summary>
    /// 门店推送消息给智订云管理员及员工
    /// </summary>
    /// <param name="orderNumber"></param>
    public static void SendMessage_MDZDY(string orderNumber)
    {
        string url = ConfigHelper.GetAppSettings("Url");
        StringBuilder sb_Name = new StringBuilder();
        sb_Name.Append(@"
        SELECT  a.Name ,
                b.name HotelName ,
                a.PayPrice ,
                a.AdminHotelid ,
                a.HotelId ,
                a.AddTime
        FROM    dbo.ServiceOrder a
                LEFT JOIN dbo.Hotel b ON a.HotelId = b.ID
        WHERE   a.OrderNum = @OrderNum
        ");
        SqlParam[] param_Name = new SqlParam[] { 
                                     new SqlParam("@OrderNum", orderNumber)};
        DataTable dt_Name = DataFactory.SqlDataBase().GetDataTableBySQL(sb_Name, param_Name);

        if (dt_Name != null && dt_Name.Rows.Count > 0)
        {
            string Name = dt_Name.Rows[0]["Name"].ToString();//客人
            string HotelName = dt_Name.Rows[0]["HotelName"].ToString();//酒店名称
            string PayPrice = dt_Name.Rows[0]["PayPrice"].ToString();//支付订单费用
            string CreateTime = Convert.ToDateTime(dt_Name.Rows[0]["AddTime"]).ToString("yyyy-MM-dd HH:mm:ss");//时间
            string AdminHotelid = dt_Name.Rows[0]["AdminHotelid"].ToString();//集团ID
            string Hotelid = dt_Name.Rows[0]["HotelId"].ToString();//酒店ID

            //判断类型
            string NoticeVal = "3";//代表门店通知值
            //判断星期
            string WeekVal = TemplateMessage.GetWeek();//代表星期几
            //判断时间段
            string PeriodTimeVal = TemplateMessage.GetPeriodTime();//代表时间段

            StringBuilder sql_Notice = new StringBuilder();
            sql_Notice.Append(@"
            SELECT  OpenId
            FROM    Notice
            WHERE   DeleteMark = 1
                    AND LEN(OpenId) > 4
                    AND HotelData = 1
                    AND AdminHotelid = @AdminHotelid  
                    AND  ','+HotelListId+',' LIKE '%,'+'@Hotelid'+',%'
                    AND  ','+NoticeVal+',' LIKE '%,'+'@NoticeVal'+',%'
                    AND  ','+WeekVal+',' LIKE '%,'+'@WeekVal'+',%'
                    AND  ','+PeriodTimeVal+',' LIKE '%,'+'@PeriodTimeVal'+',%'
            GROUP BY  OpenId
            ");
            SqlParam[] param_Notice = new SqlParam[] { 
                                    new SqlParam("@AdminHotelid", AdminHotelid),
                                    new SqlParam("@Hotelid", Hotelid),
                                    new SqlParam("@NoticeVal", NoticeVal),
                                    new SqlParam("@WeekVal", WeekVal),
                                    new SqlParam("@PeriodTimeVal", PeriodTimeVal)};
            DataTable dt_Notice = DataFactory.SqlDataBase().GetDataTableBySQL(sql_Notice, param_Notice);
            //推送消息给智订云以及酒店管理员管理员
            if (dt_Notice != null && dt_Notice.Rows.Count > 0)
            {
                for (int j = 0; j < dt_Notice.Rows.Count; j++)
                {
                    GetArrangeStoresInfo("1", dt_Notice.Rows[j]["OpenId"].ToString(), orderNumber, Name, HotelName, "", PayPrice, CreateTime, "", "");
                }
            }
        }
    }


    #region 门店推送消息给智订云管理员
    public static void GetArrangeStoresInfo(string type, string openid, string orderNumber, string Name, string HotelName, string User_Name, string PayPrice, string CreateTime, string ServicingMoney, string EstimateMoney)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> csInfo = new Dictionary<string, object>();

        csInfo.Add("touser", openid);//微信id
        csInfo.Add("template_id", "U6J5A_my1oqZVJtCN7-jYr4phxIqRpZzrIUP0f6MMVI");//推送模板id
        string url = "";
        string title = "客人[" + ReplaceWithSpecialChar(Name) + "]预订了[" + HotelName + "]的服务订单";
        string remark = "";

        csInfo.Add("url", url);//点击跳转地址

        #region ****** 参数信息 ******

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("first", new Dictionary<string, object>
                    {
                        { "value", title},
                        { "color", "#000" }
                    });
        //
        data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value",orderNumber},
                        { "color", "#000" }
                    });
        //
        data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value", CreateTime},
                        { "color", "#000" }
                    });
        //
        data.Add("keyword3", new Dictionary<string, object>
                    {
                        { "value", PayPrice},
                        { "color", "#000" }
                    });
        //

        data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", remark},
                        { "color", "#000" }
                    });
        #endregion
        csInfo.Add("data", data);
        string menuInfo = serializer.Serialize(csInfo);
        string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken("1"));
        TemplateMessage.PostWebRequest(postUrl, menuInfo);
    }
    #endregion



    #region 会员充值推送消息给智订云管理员及员工


    /// <summary>
    /// 会员充值消息 
    /// </summary>
    /// <param name="orderNumber"></param>
    public static void Message_CZ(string orderNumber)
    {
        string url = ConfigHelper.GetAppSettings("Url");
        StringBuilder sb_Name = new StringBuilder();
        sb_Name.Append(@"
        SELECT TOP 1
                a.AdminHotelid ,
                a.hotelid ,
                a.Number ,
                a.Name ,
                a.Phone ,
                a.Monery ,
                a.OpenId ,
                ISNULL(b.name, c.Name) HotelName ,
                ISNULL(b.phone, '') HotelPhone
        FROM    dbo.Finance a
                LEFT JOIN dbo.Hotel b ON a.hotelid = b.ID
                LEFT JOIN dbo.Hotel_Admin c ON a.AdminHotelid = c.AdminHotelid
        WHERE   Number = @OrderNum
        ");
        SqlParam[] param_Name = new SqlParam[] { 
                                     new SqlParam("@OrderNum", orderNumber)};
        DataTable dt_Name = DataFactory.SqlDataBase().GetDataTableBySQL(sb_Name, param_Name);

        if (dt_Name != null && dt_Name.Rows.Count > 0)
        {
            string AdminHotelid = dt_Name.Rows[0]["AdminHotelid"].ToString();//集团ID
            string Phone = dt_Name.Rows[0]["Phone"].ToString();//客人手机
            string PayPrice = dt_Name.Rows[0]["Monery"].ToString();//充值金额
            string OpenId = dt_Name.Rows[0]["OpenId"].ToString();//
            string HotelName = dt_Name.Rows[0]["HotelName"].ToString();//
            string HotelPhone = dt_Name.Rows[0]["HotelPhone"].ToString();//
            //OpenId = "obLkL09siLikrPgW7pPJJ2E8PS0g";//测试
            Message_CZ(AdminHotelid, OpenId, orderNumber, Phone, PayPrice, HotelName, HotelPhone);
        }
    }


    public static void Message_CZ(string adminHotelid, string openid, string orderNumber, string phone, string PayPrice, string hotelName, string hotelPhone)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> csInfo = new Dictionary<string, object>();

        csInfo.Add("touser", openid);//微信id
        csInfo.Add("template_id", TemplateMessage.GetTemplateid("充值成功通知", adminHotelid));//会员充值成功模板id
        string url = "";

        string title = "您好，您的帐户充值成功\n订单编号：" + orderNumber + "\n酒店名称：" + hotelName;
        string remark = "如有疑问，请致电" + hotelPhone + "联系我们。";
        csInfo.Add("url", url);//点击跳转地址

        #region ****** 参数信息 ******

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("first", new Dictionary<string, object>
                    {
                        { "value", title},
                        { "color", "#000" }
                    });
        //充值帐户
        data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value", phone},
                        { "color",  "#000"  }
                    });
        //充值金额
        data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value", PayPrice},
                        { "color", "#000" }
                    });
        //充值时间
        data.Add("keyword3", new Dictionary<string, object>
                    {
                        { "value", DateTime.Now.ToString("yyyy-MM-dd HH:mm")},
                        { "color", "#000" }
                    });
        data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", remark},
                        { "color", "#000" }
                    });
        #endregion
        csInfo.Add("data", data);
        string menuInfo = serializer.Serialize(csInfo);
        string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken(adminHotelid));
        TemplateMessage.PostWebRequest(postUrl, menuInfo);
    }


    /// <summary>
    /// 会员充值推送消息给智订云管理员及员工
    /// </summary>
    /// <param name="orderNumber"></param>
    public static void SendMessage_HYCZZDY(string orderNumber)
    {
        string url = ConfigHelper.GetAppSettings("Url");
        StringBuilder sb_Name = new StringBuilder();
        sb_Name.Append(@"
        SELECT  a.AdminHotelid ,
                a.hotelid ,
                a.Number ,
                a.Name ,
                a.Phone ,
                a.Monery ,
                b.Name HotelName ,
                ISNULL(c.Consumption, 0) ServicingMoney ,
                ISNULL(d.ObtainMonery, 0) EstimateMoney ,
                ISNULL(e.User_Name, '') User_Name ,
                ISNULL(e.openid, '') User_Openid
        FROM    dbo.Finance a
                LEFT JOIN dbo.Hotel_Admin b ON a.AdminHotelid = b.AdminHotelid
                LEFT JOIN dbo.Hotel_Account c ON a.Number = c.OrderNumber
                                                 AND c.TypeNum = 6
                LEFT JOIN GeneralizeCommision d ON a.Number = d.OrderNumber
                LEFT JOIN dbo.Base_UserInfo e ON d.UserId = e.User_ID
        WHERE   a.Number = @OrderNum And a.Type='4'
        ");
        SqlParam[] param_Name = new SqlParam[] { 
                                     new SqlParam("@OrderNum", orderNumber)};
        DataTable dt_Name = DataFactory.SqlDataBase().GetDataTableBySQL(sb_Name, param_Name);

        if (dt_Name != null && dt_Name.Rows.Count > 0)
        {
            string Name = dt_Name.Rows[0]["Name"].ToString();//客人
            string Phone = dt_Name.Rows[0]["Phone"].ToString();//客人手机
            string HotelName = dt_Name.Rows[0]["HotelName"].ToString();//酒店名称
            string User_Name = dt_Name.Rows[0]["User_Name"].ToString();//员工姓名
            string user_openid = dt_Name.Rows[0]["User_Openid"].ToString();//员工的Openid
            string PayPrice = dt_Name.Rows[0]["Monery"].ToString();//充值金额
            string ServicingMoney = dt_Name.Rows[0]["ServicingMoney"].ToString();//维护费
            string EstimateMoney = dt_Name.Rows[0]["EstimateMoney"].ToString();//奖金
            string AdminHotelid = dt_Name.Rows[0]["AdminHotelid"].ToString();//集团ID
            string Hotelid = dt_Name.Rows[0]["hotelid"].ToString();//酒店ID

            #region 智订云以及酒店管理员管理员

            string WeekVal = TemplateMessage.GetWeek();//代表星期几
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"
            SELECT  CAST(HotelData AS VARCHAR(50)) HotelData ,
                    OpenId
            FROM    Notice
            WHERE   DeleteMark = 1
                    AND LEN(OpenId) > 4
                    AND AdminHotelid = '{0}'
                    AND Hotelid = '{1}'
                    AND ',' + NoticeVal + ',' LIKE '%,4,%' --会员充值通知
                    AND ',' + WeekVal + ',' LIKE '%,{2},%'--是否在星期内
                    AND ',' + PeriodTimeVal + ',' LIKE '%,{3},%'--是否在时间段内
            UNION  ALL
            SELECT  'admin' ,
                    b.openid
            FROM    Set_WeChat a
                    LEFT JOIN dbo.Base_UserInfo b ON b.User_Account = a.Phone
            WHERE   b.AdminHotelid='1004613' AND LEN(b.openid) > 4 
        
            GROUP BY b.openid
            ", AdminHotelid, Hotelid, WeekVal, DateTime.Now.Hour);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);

            //推送消息给智订云以及酒店管理员管理员
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GetArrangeRechargeInfo(dt.Rows[i]["HotelData"].ToString(), dt.Rows[i]["openid"].ToString(), orderNumber, Name, Phone, HotelName, User_Name, PayPrice, ServicingMoney, EstimateMoney);
                }
            }

            //GetArrangeRechargeInfo("admin", "oQQLEw6xNcDk06dkGLaik190N0-o", orderNumber, Name, Phone, HotelName, User_Name, PayPrice, ServicingMoney, EstimateMoney);//测试

            //推送消息给员工
            if (user_openid.Length > 4 && CommonHelper.GetDouble(EstimateMoney) > 0)
            {
                GetArrangeRechargeInfo("员工", user_openid, orderNumber, Name, Phone, HotelName, User_Name, PayPrice, ServicingMoney, EstimateMoney);
            }
            #endregion
        }
    }


    public static void GetArrangeRechargeInfo(string type, string openid, string orderNumber, string Name, string phone, string HotelName, string User_Name, string PayPrice, string ServicingMoney, string EstimateMoney)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> csInfo = new Dictionary<string, object>();

        csInfo.Add("touser", openid);//微信id
        csInfo.Add("template_id", ApplicationHelper.GetAdminTemplateId("充值成功通知"));//会员充值成功模板id
        string url = "";

        string title = "[" + HotelName + "]的会员[" + ReplaceWithSpecialChar(Name) + "]充值成功\n订单编号：" + orderNumber;

        string remark = "";
        if (type == "员工")//员工
        {
            url = "http://www.zidinn.com/Melt/Sale_bonus.aspx";
            remark = "恭喜你您获得" + EstimateMoney + "元充值奖金";
        }
        else if (type == "admin")//智订云超级管理员
        {
            if (CommonHelper.GetDouble(ServicingMoney) == 0)
            {
                return;
            }
            url = "http://www.zidinn.com/Melt/Sale_data.aspx";

            if (string.IsNullOrEmpty(User_Name))
            {
                remark = "智订云获得" + ServicingMoney + "元维护费，客人自主关注预订";
            }
            else
            {
                remark = "智订云获得" + ServicingMoney + "元维护费，员工[" + User_Name + "]预计获得" + EstimateMoney + "元售房奖金";
            }

            string agent = GetAgent(orderNumber);//代理信息
            remark += agent;

        }
        else//酒店管理员
        {
            url = "http://www.zidinn.com/Melt/Hotel/HotelData.aspx";
            if (type == "0")//没有数据统计权限
            {
                url = "";
            }
            remark = "";
        }

        csInfo.Add("url", url);//点击跳转地址

        #region ****** 参数信息 ******

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("first", new Dictionary<string, object>
                    {
                        { "value", title},
                        { "color", "#000" }
                    });
        //充值帐户
        data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value", phone},
                        { "color",  "#000"  }
                    });
        //充值金额
        data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value", PayPrice},
                        { "color", "#000" }
                    });
        //充值时间
        data.Add("keyword3", new Dictionary<string, object>
                    {
                        { "value", DateTime.Now.ToString("yyyy-MM-dd HH:mm")},
                        { "color", "#000" }
                    });
        data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", remark},
                        { "color", "#000" }
                    });
        #endregion
        csInfo.Add("data", data);
        string menuInfo = serializer.Serialize(csInfo);
        string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken("1"));
        TemplateMessage.PostWebRequest(postUrl, menuInfo);
    }

    #endregion


    #region  到付通知
    /// <summary>
    /// 到付通知
    /// </summary>
    /// <param name="orderNumber"></param>
    public static void Message_DFTZ(string orderNumber)
    {
        string url = ConfigHelper.GetAppSettings("Url");
        StringBuilder sb_Name = new StringBuilder();
        sb_Name.Append(@"
        SELECT TOP 1
                a.AdminHotelid ,
                a.hotelid ,
                a.RoomType ,
                a.Number ,
                a.BeginTime ,
                a.EndTime ,
                a.Name ,
                a.TomePrice ,
                a.OpenId ,
                ISNULL(b.name, '') HotelName ,
                ISNULL(b.phone, '') HotelPhone
        FROM    dbo.Reservation a
                LEFT JOIN dbo.Hotel b ON a.hotelid = b.ID
        WHERE   OrderNum = @OrderNum
        ");
        SqlParam[] param_Name = new SqlParam[] { 
            new SqlParam("@OrderNum", orderNumber)
        };
        DataTable dt_Name = DataFactory.SqlDataBase().GetDataTableBySQL(sb_Name, param_Name);

        if (dt_Name != null && dt_Name.Rows.Count > 0)
        {
            string AdminHotelid = dt_Name.Rows[0]["AdminHotelid"].ToString();//集团ID
            int hotelid = CommonHelper.GetInt(dt_Name.Rows[0]["hotelid"].ToString());//酒店ID
            string TomePrice = dt_Name.Rows[0]["TomePrice"].ToString();//金额
            string OpenId = dt_Name.Rows[0]["OpenId"].ToString();//
            string HotelName = dt_Name.Rows[0]["HotelName"].ToString();//
            string HotelPhone = dt_Name.Rows[0]["HotelPhone"].ToString();//
            string RoomType = dt_Name.Rows[0]["RoomType"].ToString();//
            string Number = dt_Name.Rows[0]["Number"].ToString();//
            string BeginTime = CommonHelper.GetDateTime(dt_Name.Rows[0]["BeginTime"]).ToString("yyyy-MM-dd");//
            string EndTime = CommonHelper.GetDateTime(dt_Name.Rows[0]["EndTime"]).ToString("yyyy-MM-dd");//
            string roomName = RoomType + "[" + Number + "间]";
            Message_DFTZ(AdminHotelid, hotelid, OpenId, orderNumber, TomePrice, roomName, BeginTime, EndTime, HotelPhone);
        }
    }


    public static void Message_DFTZ(string adminHotelid, int hotelid, string openid, string orderNumber, string PayPrice, string roomName, string BeginTime, string EndTime, string hotelPhone)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> csInfo = new Dictionary<string, object>();
        csInfo.Add("touser", openid);//微信id
        csInfo.Add("template_id", TemplateMessage.GetTemplateid("到店付款通知", adminHotelid));//会员充值成功模板id
        string url = "http://" + WxPayConfig.redirect_uri(adminHotelid, hotelid) + "/Reservation/OrderWith.aspx?AdminHotelid=" + adminHotelid + "&Id=" + orderNumber;

        string title = "您的订单已提交成功，点击查看详情\n客房名称：" + roomName + "\n入离日期：" + BeginTime + "至" + EndTime;
        string remark = "如有疑问，请致电" + hotelPhone + "联系我们。";
        csInfo.Add("url", url);//点击跳转地址

        #region ****** 参数信息 ******

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("first", new Dictionary<string, object>
                    {
                        { "value", title},
                        { "color", "#000" }
                    });
        //单号
        data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value", orderNumber},
                        { "color",  "#000"  }
                    });
        //下单时间
        data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value",DateTime.Now.ToString("yyyy-MM-dd HH:mm") },
                        { "color", "#000" }
                    });
        //消费金额
        data.Add("keyword3", new Dictionary<string, object>
                    {
                        { "value", PayPrice},
                        { "color", "#000" }
                    });
        //支付方式
        data.Add("keyword4", new Dictionary<string, object>
                    {
                        { "value","到店付款"},
                        { "color", "#000" }
                    });
        data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", remark},
                        { "color", "#000" }
                    });
        #endregion
        csInfo.Add("data", data);
        string menuInfo = serializer.Serialize(csInfo);
        string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken(adminHotelid));
        TemplateMessage.PostWebRequest(postUrl, menuInfo);
    }

    #endregion


    #region  预售券通知
    /// <summary>
    /// 到付通知
    /// </summary>
    /// <param name="orderNumber"></param>
    public static void Message_TaoPiao(string orderNumber)
    {
        string url = ConfigHelper.GetAppSettings("Url");
        StringBuilder sb_Name = new StringBuilder();
        sb_Name.Append(@"
        SELECT TOP 1
                a.AdminHotelid ,
                a.hotelid ,
                a.Number ,
                a.Name ,
                a.PayMoney ,
                a.PayType ,
                c.payname ,
                a.OpenId ,
                ISNULL(b.name, '') HotelName ,
                ISNULL(b.phone, '') HotelPhone
        FROM    dbo.TP_Order a
                LEFT JOIN dbo.Hotel b ON a.hotelid = b.ID
                LEFT JOIN dbo.paytypes c ON a.PayType = c.orderpaytype
        WHERE   OrderNum = @OrderNum
        ");
        SqlParam[] param_Name = new SqlParam[] { 
            new SqlParam("@OrderNum", orderNumber)
        };
        DataTable dt_Name = DataFactory.SqlDataBase().GetDataTableBySQL(sb_Name, param_Name);

        if (dt_Name != null && dt_Name.Rows.Count > 0)
        {
            string AdminHotelid = dt_Name.Rows[0]["AdminHotelid"].ToString();//集团ID
            int hotelid = CommonHelper.GetInt(dt_Name.Rows[0]["hotelid"].ToString());//酒店ID
            string PayMoney = dt_Name.Rows[0]["PayMoney"].ToString();//金额
            string OpenId = dt_Name.Rows[0]["OpenId"].ToString();//
            string HotelName = dt_Name.Rows[0]["HotelName"].ToString();//
            string HotelPhone = dt_Name.Rows[0]["HotelPhone"].ToString();//
            string Number = dt_Name.Rows[0]["Number"].ToString();//
            string PayType = dt_Name.Rows[0]["payname"].ToString();//
            Message_TaoPiao(AdminHotelid, hotelid, OpenId, orderNumber, PayType, PayMoney, HotelPhone);
        }
    }


    public static void Message_TaoPiao(string adminHotelid, int hotelid, string openid, string orderNumber, string PayType, string PayPrice, string hotelPhone)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> csInfo = new Dictionary<string, object>();
        csInfo.Add("touser", openid);//微信id
        csInfo.Add("template_id", TemplateMessage.GetTemplateid("到店付款通知", adminHotelid));//订单提交成功通知
        string url = "http://" + WxPayConfig.redirect_uri(adminHotelid, hotelid) + "/Members/Ticket/TicketDetail.aspx?AdminHotelId=" + adminHotelid + "&OrderNum=" + orderNumber;

        string title = "您的订单已提交成功";
        string remark = "如有疑问，请致电" + hotelPhone + "联系我们。";
        csInfo.Add("url", url);//点击跳转地址

        #region ****** 参数信息 ******

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("first", new Dictionary<string, object>
                    {
                        { "value", title},
                        { "color", "#000" }
                    });
        //单号
        data.Add("keyword1", new Dictionary<string, object>
                    {
                        { "value", orderNumber},
                        { "color",  "#000"  }
                    });
        //下单时间
        data.Add("keyword2", new Dictionary<string, object>
                    {
                        { "value",DateTime.Now.ToString("yyyy-MM-dd HH:mm") },
                        { "color", "#000" }
                    });
        //消费金额
        data.Add("keyword3", new Dictionary<string, object>
                    {
                        { "value", PayPrice},
                        { "color", "#000" }
                    });
        //支付方式
        data.Add("keyword4", new Dictionary<string, object>
                    {
                        { "value",PayType},
                        { "color", "#000" }
                    });
        data.Add("remark", new Dictionary<string, object>
                    {
                        { "value", remark},
                        { "color", "#000" }
                    });
        #endregion
        csInfo.Add("data", data);
        string menuInfo = serializer.Serialize(csInfo);
        string postUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        postUrl = string.Format(postUrl, TemplateMessage.GetAccessToken(adminHotelid));
        TemplateMessage.PostWebRequest(postUrl, menuInfo);
    }

    #endregion



    public static string GetMarket(string adminhotelid, string typeid, string topupmoney, int number, int days)
    {
        string obtainmoney = "";
        StringBuilder sb_Market = new StringBuilder();
        sb_Market.Append("SELECT * FROM MarketingConfigure WHERE AdminHotelid = @AdminHotelid  ");
        SqlParam[] param_Market = new SqlParam[] { 
                                     new SqlParam("@AdminHotelid", adminhotelid)};
        DataTable dt_Market = DataFactory.SqlDataBase().GetDataTableBySQL(sb_Market, param_Market);
        if (dt_Market != null && dt_Market.Rows.Count > 0)
        {
            if (typeid.ToString() == "1" || typeid.ToString() == "2")
            {
                obtainmoney = dt_Market.Rows[0]["RegisterMoney"].ToString();
            }
            else if (typeid.ToString() == "3" || typeid.ToString() == "4")
            {
                if (dt_Market.Rows[0]["CheckIn"].ToString() == "1")//固定
                {
                    double cm = Convert.ToDouble(dt_Market.Rows[0]["CheckInMoney"]) * number * days;
                    obtainmoney = cm.ToString("0.00");
                }
                else
                {
                    obtainmoney = (Convert.ToDouble(dt_Market.Rows[0]["CheckInProportion"]) / 100 * Convert.ToDouble(topupmoney)).ToString("0.00");
                }
            }
            else if (typeid.ToString() == "5")
            {
                if (dt_Market.Rows[0]["Recharge"].ToString() == "1")//固定
                {
                    obtainmoney = Convert.ToDouble(dt_Market.Rows[0]["RechargeMoney"]).ToString("0.00");
                }
                else
                {
                    obtainmoney = (Convert.ToDouble(dt_Market.Rows[0]["RechargeProportion"]) / 100 * Convert.ToDouble(topupmoney)).ToString("0.00");
                }
            }
        }
        return obtainmoney;
    }


    public static string GetReservation(string AdminHotelid)
    {
        string url = ConfigHelper.GetAppSettings("Url");
        string Reservation = "";
        string sql_hotel = string.Format(@"SELECT type FROM dbo.Hotel_Admin WHERE AdminHotelid='{0}'", AdminHotelid);
        DataTable dt_hotel = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql_hotel));
        if (dt_hotel != null && dt_hotel.Rows.Count > 0)
        {
            if (dt_hotel.Rows[0]["type"].ToString() == "0")//单体酒店
            {
                Reservation = url + "/Reservation/HotelDetails.aspx?AdminHotelid=" + AdminHotelid.ToString() + "&hotelid=" + Hotelid(AdminHotelid.ToString());
            }
            else//集团酒店
            {
                Reservation = url + "/Reservation/HotelList.aspx?AdminHotelid=" + AdminHotelid.ToString();
            }
        }
        return Reservation;
    }

    public static string Hotelid(string AdminHotelid)
    {
        string Hotelid = "";
        string sql = string.Format(@"SELECT hotelid FROM dbo.V_Hotel WHERE AdminHotelid='{0}'", AdminHotelid);
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
        if (dt != null && dt.Rows.Count > 0)
        {
            Hotelid = dt.Rows[0]["hotelid"].ToString();
        }
        return Hotelid;
    }



    #endregion

    #region *** 活动赠送优惠券 ***
    public static string AddActivityCardCoupons(string lsh, string openid, string AdminHotelid, string type, DataTable dt, string PreferentialType)
    {
        string retureval = "0";
        //获取会员信息
        string Is_Day_ok = "";
        //活动赠送卡券
        Hashtable hff = new Hashtable();

        if (dt != null && dt.Rows.Count > 0)
        {
            hff["openid"] = openid;
            hff["AdminHotelid"] = AdminHotelid;
            hff["Type"] = type;
            hff["isDelete"] = true;
            hff["isReceive"] = false;
            hff["PreferentialType"] = PreferentialType;

            hff["ServiceRange"] = dt.Rows[0]["ServiceRange"];
            hff["Membergrade"] = dt.Rows[0]["Membergrade"];
            hff["yxrq"] = dt.Rows[0]["yxrq"];
            hff["jjr"] = dt.Rows[0]["jjr"];
            hff["Ishy"] = dt.Rows[0]["Ishy"];
            hff["hytype"] = dt.Rows[0]["hytype"];
            Is_Day_ok = dt.Rows[0]["Is_Day_ok"].ToString();
            int EffectiveType = CommonHelper.GetInt(dt.Rows[0]["EffectiveType"]);
            int EffectiveDay = CommonHelper.GetInt(dt.Rows[0]["EffectiveDay"]);
            hff["Is_Day_ok"] = CommonHelper.GetInt(Is_Day_ok);
            hff["EffectiveType"] = EffectiveType;
            hff["EffectiveDay"] = EffectiveDay;
            hff["UsedMin"] = dt.Rows[0]["UsedMin"];
            DateTime BiginTime = DateTime.Parse(dt.Rows[0]["BiginTime"].ToString());
            DateTime EndinTime = DateTime.Parse(dt.Rows[0]["endinTime"].ToString());
            if (EffectiveType == 2 && EffectiveDay != 0)
            {
                BiginTime = DateTime.Now;
                EndinTime = DateTime.Now.AddDays(EffectiveDay);
            }

            if (CommonHelper.GetInt(Is_Day_ok) == 0)
            {
                BiginTime = BiginTime.AddDays(1);
            }
            hff["CouponID"] = CommonHelper.GetInt(dt.Rows[0]["CID"]);
            hff["Clientid"] = lsh;
            hff["Par"] = CommonHelper.GetInt(dt.Rows[0]["Par"]);
            hff["biginTime"] = BiginTime;
            hff["endinTime"] = EndinTime;
            int isok = DataFactory.SqlDataBase().InsertByHashtable("ClientCoupon", hff);
            if (isok > 0)
            {
                retureval = "1";
            }
        }
        return retureval;
    }
    #endregion

    internal static void BindCredentials(HtmlSelect sel)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT * FROM Credentials_Type");
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
        sel.DataSource = dt;
        sel.DataTextField = "Credentials_Name";
        sel.DataValueField = "Credentials_Name";
        sel.DataBind();
        sel.Items.Insert(0, new ListItem("证件类型", ""));
    }

    #region 操作日志

    /// <summary>
    /// 操作日志
    /// </summary>
    /// <param name="type">操作事件</param>
    /// <param name="tableName">操作说明</param>
    /// <param name="number">字段:编号</param>
    /// <param name="remarks">备注</param>
    public static void Base_Log(string type, string tableName, string number, string explain, string remarks)
    {
        Hashtable ht_log = new Hashtable();
        ht_log["type"] = type;
        ht_log["Tables"] = tableName;
        ht_log["number"] = number;
        ht_log["SysTable"] = explain;
        ht_log["Remarks"] = remarks;
        ht_log["User_Account"] = RequestSession.GetSessionUser().IsAdmin.ToString() == "2" ? RequestSession.GetSessionUser().UserName.ToString() : RequestSession.GetSessionUser().UserAccount.ToString();
        ht_log["Base_UserInfo_ID"] = RequestSession.GetSessionUser().UserId.ToString();
        ht_log["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
        ht_log["Hotelid"] = RequestSession.GetSessionUser().Hotelid.ToString();

        HttpContext rq = HttpContext.Current;
        IPScanerHelper objScan = new IPScanerHelper();
        objScan.DataPath = rq.Server.MapPath("/Themes/IPScaner/QQWry.Dat");
        string ip = RequestHelper.GetIP();
        objScan.IP = ip;
        string address = objScan.IPLocation();

        ht_log["User_Ip"] = ip;
        ht_log["User_Address"] = address;

        DataFactory.SqlDataBase().InsertByHashtable("Base_Blog", ht_log);
    }

    //管理员操作日志
    public static void Base_Admin_Log(string AdminHotelid, string Hotelid, string type, string tableName, string number, string explain, string remarks)
    {
        Hashtable ht_log = new Hashtable();
        ht_log["type"] = type;
        ht_log["Tables"] = tableName;
        ht_log["number"] = number;
        ht_log["SysTable"] = explain;
        ht_log["Remarks"] = remarks;
        ht_log["User_Account"] = "管理员";
        ht_log["Base_UserInfo_ID"] = "";
        ht_log["AdminHotelid"] = AdminHotelid;
        ht_log["Hotelid"] = Hotelid;


        HttpContext rq = HttpContext.Current;
        IPScanerHelper objScan = new IPScanerHelper();
        objScan.DataPath = rq.Server.MapPath("/Themes/IPScaner/QQWry.Dat");
        string ip = RequestHelper.GetIP();
        objScan.IP = ip;
        string address = objScan.IPLocation();

        ht_log["User_Ip"] = ip;
        ht_log["User_Address"] = address;

        DataFactory.SqlDataBase().InsertByHashtable("Base_Blog", ht_log);
    }

    /// <summary>
    /// 押金 操作日志
    /// </summary>
    /// <param name="OrderId"></param>
    /// <param name="remarks"></param>
    public static void CashPledge_Log(string OrderId, string remarks)
    {
        Hashtable ht_log = new Hashtable();
        ht_log["AdminHotelId"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
        ht_log["HotelId"] = RequestSession.GetSessionUser().Hotelid.ToString();
        ht_log["UserName"] = RequestSession.GetSessionUser().UserName.ToString();
        ht_log["UserId"] = RequestSession.GetSessionUser().UserId.ToString();
        ht_log["OrderId"] = OrderId;
        ht_log["Remarks"] = remarks;
        DataFactory.SqlDataBase().InsertByHashtable("CashPledge_Log", ht_log);
    }


    #endregion


    #region

    /// <summary>
    /// 操作日志
    /// </summary>
    /// <param name="type">操作事件</param>
    /// <param name="tableName">操作说明</param>
    /// <param name="number">字段:编号</param>
    /// <param name="remarks">备注</param>
    public static void RoomCommodity_Log(string type, string tableName, string number, string explain, string remarks)
    {
        //Hashtable ht_log = new Hashtable();
        //ht_log["type"] = type;
        //ht_log["Tables"] = tableName;
        //ht_log["number"] = number;
        //ht_log["SysTable"] = explain;
        //ht_log["Remarks"] = remarks;
        //ht_log["User_Account"] = RequestSession.GetSessionUser().UserAccount.ToString();
        //ht_log["Base_UserInfo_ID"] = RequestSession.GetSessionUser().UserId.ToString();
        //ht_log["AdminHotelid"] = RequestSession.GetSessionUser().AdminHotelid.ToString();
        //ht_log["Hotelid"] = RequestSession.GetSessionUser().Hotelid.ToString();


        //HttpContext rq = HttpContext.Current;
        //IPScanerHelper objScan = new IPScanerHelper();
        //objScan.DataPath = rq.Server.MapPath("/Themes/IPScaner/QQWry.Dat");
        //string ip = RequestHelper.GetIP();
        //objScan.IP = ip;
        //string address = objScan.IPLocation();

        //ht_log["User_Ip"] = ip;
        //ht_log["User_Address"] = address;

        //DataFactory.SqlDataBase().InsertByHashtable("RoomCommodityLog", ht_log);
    }
    #endregion

    /// <summary>
    /// 更新订单表、订单详情表
    /// </summary>
    public static void UPDATE_Food_Order(string SoId, string ordernum, string TableCode, string OpenId, string RestaurantId, string RestaurantName, string AdminHotelid, string Hotelid)
    {


        StringBuilder sb_wx = new StringBuilder();
        sb_wx.Append(string.Format("select th,chk,xh,csbm,csmc,dj,sl,je,fwfje,zkje,zsje,kw  from view_krxfb_wx where 1=1 "));
        List<SqlParam> listStr_wx = new List<SqlParam>();
        if (TableCode != "")
        {
            sb_wx.Append(" And th = @TableCode");
            listStr_wx.Add(new SqlParam("@TableCode", TableCode));
        }
        DataTable dt_wx = DataFactory.SqlDataBase(AdminHotelid, Hotelid).GetDataTableBySQL(sb_wx, listStr_wx.ToArray());
        if (dt_wx != null && dt_wx.Rows.Count > 0)
        {
            for (int i = 0; i < dt_wx.Rows.Count; i++)
            {
                Hashtable hs = new Hashtable();
                hs["SoId"] = SoId;
                hs["OrderNum"] = ordernum;
                hs["TableCode"] = TableCode;//台号
                if (dt_wx.Rows[i]["chk"] != null && dt_wx.Rows[i]["chk"].ToString() != "")
                {
                    hs["BillNumber"] = dt_wx.Rows[i]["chk"].ToString();//账单号
                }
                if (dt_wx.Rows[i]["xh"] != null && dt_wx.Rows[i]["xh"].ToString() != "")
                {
                    hs["SerialNumber"] = dt_wx.Rows[i]["xh"].ToString();//序号
                }
                if (dt_wx.Rows[i]["csmc"] != null && dt_wx.Rows[i]["csmc"].ToString() != "")
                {
                    hs["FoodName"] = dt_wx.Rows[i]["csmc"].ToString();
                }
                if (dt_wx.Rows[i]["csbm"] != null && dt_wx.Rows[i]["csbm"].ToString() != "")
                {
                    hs["spcode"] = dt_wx.Rows[i]["csbm"].ToString();
                }
                if (dt_wx.Rows[i]["dj"] != null && dt_wx.Rows[i]["dj"].ToString() != "")
                {
                    hs["SinglePrice"] = Convert.ToDouble(dt_wx.Rows[i]["dj"]);//菜式单价
                }
                if (dt_wx.Rows[i]["sl"] != null && dt_wx.Rows[i]["sl"].ToString() != "")
                {
                    hs["Number"] = Convert.ToInt32(dt_wx.Rows[i]["sl"]).ToString();//数量
                }
                if (dt_wx.Rows[i]["je"] != null && dt_wx.Rows[i]["je"].ToString() != "")
                {
                    hs["Price"] = Convert.ToDouble(dt_wx.Rows[i]["je"]);//合计金额
                }
                if (dt_wx.Rows[i]["fwfje"] != null && dt_wx.Rows[i]["fwfje"].ToString() != "")
                {
                    hs["ServiceFee"] = Convert.ToDouble(dt_wx.Rows[i]["fwfje"]);//服务费金额
                }
                if (dt_wx.Rows[i]["zkje"] != null && dt_wx.Rows[i]["zkje"].ToString() != "")
                {
                    hs["DiscountFee"] = Convert.ToDouble(dt_wx.Rows[i]["zkje"]);//折扣金额
                }
                if (dt_wx.Rows[i]["zsje"] != null && dt_wx.Rows[i]["zsje"].ToString() != "")
                {
                    hs["SettlementFee"] = Convert.ToDouble(dt_wx.Rows[i]["zsje"]);//结算金额
                }
                if (dt_wx.Rows[i]["kw"] != null && dt_wx.Rows[i]["kw"].ToString() != "")
                {
                    hs["TasteName"] = dt_wx.Rows[i]["kw"].ToString();//口味
                }
                hs["OpenId"] = OpenId;
                hs["RestaurantId"] = RestaurantId;
                hs["RestaurantName"] = RestaurantName;
                hs["AdminHotelid"] = AdminHotelid;
                hs["Hotelid"] = Hotelid;
                int isok = DataFactory.SqlDataBase().InsertByHashtable("FoodOrderdetails", hs);
            }
            //删除当前餐厅餐桌所有的扫码记录
            string sqldelfood = string.Format(@"delete  FoodSource  where RestaurantId='{0}' and TableCode='{1}'", RestaurantId, TableCode);
            DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(sqldelfood));
        }

    }


    /// <summary>
    /// 更新点餐数量
    /// </summary>
    public static void UPDATE_Food_Num(string OrderNum)
    {
        StringBuilder str = new StringBuilder();
        str.Append(string.Format(" SELECT  * FROM  dbo.FoodOrderdetails  WHERE OrderNum=@OrderNum "));
        List<SqlParam> ilistStr = new List<SqlParam>();
        ilistStr.Add(new SqlParam("@OrderNum", OrderNum.ToString()));
        str.Append("  ORDER BY ID ASC");
        DataTable dstr = DataFactory.SqlDataBase().GetDataTableBySQL(str, ilistStr.ToArray());
        if (dstr != null && dstr.Rows.Count > 0)
        {
            for (int k = 0; k < dstr.Rows.Count; k++)
            {
                StringBuilder strs = new StringBuilder();
                strs.Append(string.Format(" SELECT  * FROM  dbo.FoodInfo  WHERE ID=@ID "));
                List<SqlParam> ilistStrs = new List<SqlParam>();
                ilistStrs.Add(new SqlParam("@ID", dstr.Rows[k]["FoodId"].ToString()));
                strs.Append("  ORDER BY ID ASC");
                DataTable dstrs = DataFactory.SqlDataBase().GetDataTableBySQL(strs, ilistStrs.ToArray());
                if (dstrs != null && dstrs.Rows.Count > 0)
                {
                    int CrestorNum = CommonHelper.GetInt(dstrs.Rows[0]["CrestorNum"].ToString()) - CommonHelper.GetInt(dstr.Rows[k]["Number"].ToString());
                    int HasbookedNum = CommonHelper.GetInt(dstrs.Rows[0]["HasbookedNum"].ToString()) + CommonHelper.GetInt(dstr.Rows[k]["Number"].ToString());
                    //int Remainingnum = CrestorNum - HasbookedNum;
                    Hashtable hs = new Hashtable();
                    hs["CrestorNum"] = CrestorNum;
                    hs["HasbookedNum"] = HasbookedNum;
                    hs["Remainingnum"] = CrestorNum;
                    DataFactory.SqlDataBase().UpdateByHashtable("FoodInfo", "ID", dstrs.Rows[0]["ID"].ToString(), hs);
                }

            }
        }

    }


    /// <summary>
    /// 更新商品数量
    /// </summary>
    public static void UPDATE_Product_Num(string OrderNum)
    {
        StringBuilder str = new StringBuilder();
        str.Append(string.Format(" SELECT  * FROM  dbo.ProductOrderDetail  WHERE OrderNum=@OrderNum "));
        List<SqlParam> ilistStr = new List<SqlParam>();
        ilistStr.Add(new SqlParam("@OrderNum", OrderNum.ToString()));
        str.Append("  ORDER BY ID ASC");
        DataTable dstr = DataFactory.SqlDataBase().GetDataTableBySQL(str, ilistStr.ToArray());
        if (dstr != null && dstr.Rows.Count > 0)
        {
            for (int k = 0; k < dstr.Rows.Count; k++)
            {
                StringBuilder strs = new StringBuilder();
                strs.Append(string.Format(" SELECT  * FROM  dbo.Product  WHERE ID=@ID "));
                List<SqlParam> ilistStrs = new List<SqlParam>();
                ilistStrs.Add(new SqlParam("@ID", dstr.Rows[k]["ProductId"].ToString()));
                strs.Append("  ORDER BY ID ASC");
                DataTable dstrs = DataFactory.SqlDataBase().GetDataTableBySQL(strs, ilistStrs.ToArray());
                if (dstrs != null && dstrs.Rows.Count > 0)
                {
                    int CrestorNum = CommonHelper.GetInt(dstrs.Rows[0]["Number"].ToString()) - CommonHelper.GetInt(dstr.Rows[k]["Number"].ToString());
                    int SalesNum = CommonHelper.GetInt(dstrs.Rows[0]["SalesNum"].ToString()) + CommonHelper.GetInt(dstr.Rows[k]["Number"].ToString());
                    int SalesMoney = CommonHelper.GetInt(dstrs.Rows[0]["SalesMoney"].ToString()) + CommonHelper.GetInt(dstr.Rows[k]["Price"].ToString());
                    int SalesIntegral = CommonHelper.GetInt(dstrs.Rows[0]["SalesIntegral"].ToString()) + CommonHelper.GetInt(dstr.Rows[k]["Integral"].ToString());
                    Hashtable hs = new Hashtable();
                    hs["Number"] = CrestorNum;//更新库存数量
                    if (CrestorNum < 10)
                    {
                        hs["ProductState"] = 1;//产品状态
                    }
                    else if (CrestorNum < 0)
                    {
                        hs["ProductState"] = 2;//产品状态
                    }
                    hs["SalesNum"] = SalesNum;//销售量
                    hs["SalesMoney"] = SalesMoney;//销售总金额
                    hs["SalesIntegral"] = SalesIntegral;//销售总积分
                    DataFactory.SqlDataBase().UpdateByHashtable("Product", "ID", dstrs.Rows[0]["ID"].ToString(), hs);
                }

            }
        }

    }


    /// <summary>
    /// 更新出库数量
    /// </summary>
    public static void UPDATE_ProductOutBill_Num(string OrderNum)
    {
        string Number = Create_OutBillNum();
        int ProOutId = 0;
        StringBuilder str = new StringBuilder();
        str.Append(string.Format(" SELECT  * FROM  dbo.ProductOrder  WHERE OrderNum=@OrderNum "));
        List<SqlParam> ilistStr = new List<SqlParam>();
        ilistStr.Add(new SqlParam("@OrderNum", OrderNum.ToString()));
        str.Append("  ORDER BY ID ASC");
        DataTable dstr = DataFactory.SqlDataBase().GetDataTableBySQL(str, ilistStr.ToArray());
        if (dstr != null && dstr.Rows.Count > 0)
        {
            //插入ProductOutBill(库存总表)
            Hashtable ht = new Hashtable();
            ht["Number"] = Number;
            ht["OrderNum"] = OrderNum;
            if (dstr.Rows[0]["TotalPrice"] != null && dstr.Rows[0]["TotalPrice"].ToString() != "")
            {
                ht["TotalPrice"] = dstr.Rows[0]["TotalPrice"].ToString();
            }
            if (dstr.Rows[0]["PayPrice"] != null && dstr.Rows[0]["PayPrice"].ToString() != "")
            {
                ht["PayPrice"] = dstr.Rows[0]["PayPrice"].ToString();
            }
            if (dstr.Rows[0]["zip"] != null && dstr.Rows[0]["zip"].ToString() != "")
            {
                ht["Integral"] = dstr.Rows[0]["zip"].ToString();
            }
            ht["UserID"] = dstr.Rows[0]["MemberId"].ToString();
            if (dstr.Rows[0]["Remark"] != null && dstr.Rows[0]["Remark"].ToString() != "")
            {
                ht["Remark"] = dstr.Rows[0]["Remark"].ToString();
            }
            ht["SellMode"] = dstr.Rows[0]["PayType"].ToString();
            ht["AdminHotelid"] = dstr.Rows[0]["AdminHotelid"].ToString();
            ht["HotelId"] = dstr.Rows[0]["Hotelid"].ToString();
            ProOutId = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("ProductOutBill", ht);

        }

        StringBuilder strs = new StringBuilder();
        strs.Append(string.Format(" SELECT  * FROM  dbo.ProductOrderDetail  WHERE OrderNum=@OrderNum "));
        List<SqlParam> ilistStrs = new List<SqlParam>();
        ilistStrs.Add(new SqlParam("@OrderNum", OrderNum.ToString()));
        strs.Append("  ORDER BY ID ASC");
        DataTable dstrs = DataFactory.SqlDataBase().GetDataTableBySQL(strs, ilistStrs.ToArray());
        if (dstrs != null && dstrs.Rows.Count > 0)
        {

            for (int k = 0; k < dstrs.Rows.Count; k++)
            {
                StringBuilder stres = new StringBuilder();
                stres.Append(string.Format(" SELECT  * FROM  dbo.Product  WHERE ID=@ID "));
                List<SqlParam> ilistStres = new List<SqlParam>();
                ilistStres.Add(new SqlParam("@ID", dstrs.Rows[k]["ProductId"].ToString()));
                stres.Append("  ORDER BY ID ASC");
                DataTable dstres = DataFactory.SqlDataBase().GetDataTableBySQL(stres, ilistStres.ToArray());
                if (dstres != null && dstres.Rows.Count > 0)
                {
                    //插入ProductOutDetail(库存明细表)
                    int ProDetailId = 0;
                    Hashtable hs = new Hashtable();
                    hs["Number"] = Number;
                    hs["OrderNum"] = OrderNum;
                    hs["ProductID"] = dstres.Rows[0]["ID"].ToString();
                    hs["ProductName"] = dstres.Rows[0]["Name"].ToString();
                    if (dstres.Rows[0]["Mode"] != null && dstres.Rows[0]["Mode"].ToString() != "")
                    {
                        hs["ProductMode"] = dstres.Rows[0]["Mode"].ToString();
                    }
                    if (dstres.Rows[0]["SerialNumber"] != null && dstres.Rows[0]["SerialNumber"].ToString() != "")
                    {
                        hs["ProductSerialNumber"] = dstres.Rows[0]["SerialNumber"].ToString();
                    }
                    if (dstres.Rows[0]["BarCode"] != null && dstres.Rows[0]["BarCode"].ToString() != "")
                    {
                        hs["ProductBarCode"] = dstres.Rows[0]["BarCode"].ToString();
                    }

                    if (dstres.Rows[0]["ProKindId"] != null && dstres.Rows[0]["ProKindId"].ToString() != "")
                    {
                        hs["ProductProKindId"] = dstres.Rows[0]["ProKindId"].ToString();
                    }
                    if (dstres.Rows[0]["ProKindName"] != null && dstres.Rows[0]["ProKindName"].ToString() != "")
                    {
                        hs["ProductProKindName"] = dstres.Rows[0]["ProKindName"].ToString();
                    }
                    if (dstres.Rows[0]["ProKindNames"] != null && dstres.Rows[0]["ProKindNames"].ToString() != "")
                    {
                        hs["ProductProKindNames"] = dstres.Rows[0]["ProKindNames"].ToString();
                    }
                    hs["ProductOutBillID"] = ProOutId;

                    hs["OutletsPrice"] = Convert.ToInt32(dstrs.Rows[k]["Number"].ToString()) * Convert.ToDouble(dstres.Rows[0]["OutletsPrice"].ToString());
                    hs["WeChatPrice"] = dstrs.Rows[k]["Price"].ToString();
                    hs["Integral"] = dstrs.Rows[k]["Integral"].ToString();
                    hs["Amout"] = dstrs.Rows[k]["Number"].ToString();

                    if (dstres.Rows[0]["Unit"] != null && dstres.Rows[0]["Unit"].ToString() != "")
                    {
                        hs["Unit"] = dstres.Rows[0]["Unit"].ToString();
                    }
                    hs["AdminHotelid"] = dstres.Rows[0]["AdminHotelid"].ToString();
                    hs["HotelId"] = dstres.Rows[0]["Hotelid"].ToString();
                    ProDetailId = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("ProductOutDetail", hs);
                }

            }
        }

    }



    /// <summary>
    /// 生成入库存编号
    /// </summary>
    /// <returns></returns>
    public static string Create_InBillNum()
    {
        string strreturn = "";
        string date = "RK" + System.DateTime.Now.ToString("yyyyMMdd");
        StringBuilder sb = new StringBuilder();
        sb.Append(" declare @MaxOrderNo int  ");
        sb.Append(" select @MaxOrderNo= max(cast(substring(Number,len(Number)-3,4) as int)) ");
        sb.AppendFormat(" from [ProductInBill] where Number like '{0}%' ", date);
        sb.Append(" if(@MaxOrderNo IS NULL) ");
        sb.Append("   SELECT 1 AS ProductNo ");
        sb.Append(" ELSE ");
        sb.Append("   SELECT (@MaxOrderNo +1) AS ProjecttNo ");
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0][0].ToString().Length != 4)
            {
                for (int i = 0; i < 4 - dt.Rows[0][0].ToString().Length; i++)
                {
                    strreturn += "0";
                }
            }
            strreturn += dt.Rows[0][0].ToString();
        }
        else
        {
            strreturn += "0001";
        }

        return date + strreturn;
    }
    /// <summary>
    /// 生成出库存编号
    /// </summary>
    /// <returns></returns>
    public static string Create_OutBillNum()
    {
        string strreturn = "";
        string date = "CK" + System.DateTime.Now.ToString("yyyyMMdd");
        StringBuilder sb = new StringBuilder();
        sb.Append(" declare @MaxOrderNo int  ");
        sb.Append(" select @MaxOrderNo= max(cast(substring(Number,len(Number)-3,4) as int)) ");
        sb.AppendFormat(" from [ProductOutDetail] where Number like '{0}%' ", date);
        sb.Append(" if(@MaxOrderNo IS NULL) ");
        sb.Append("   SELECT 1 AS ProductNo ");
        sb.Append(" ELSE ");
        sb.Append("   SELECT (@MaxOrderNo +1) AS ProjecttNo ");
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0][0].ToString().Length != 4)
            {
                for (int i = 0; i < 4 - dt.Rows[0][0].ToString().Length; i++)
                {
                    strreturn += "0";
                }
            }
            strreturn += dt.Rows[0][0].ToString();
        }
        else
        {
            strreturn += "0001";
        }

        return date + strreturn;
    }

    /// <summary>
    /// 将传入的字符串中间部分字符替换成特殊字符
    /// </summary>
    /// <param name="value">需要替换的字符串</param>
    /// <param name="startLen">前保留长度</param>
    /// <param name="endLen">尾保留长度</param>
    /// <param name="replaceChar">特殊字符</param>
    /// <returns>被特殊字符替换的字符串</returns>
    private static string ReplaceWithSpecialChar(string value, int startLen = 1, int endLen = 0, char specialChar = '※')
    {
        try
        {
            int lenth = value.Length - startLen - endLen;

            string replaceStr = value.Substring(startLen, lenth);

            string specialStr = "";

            for (int i = 0; i < replaceStr.Length; i++)
            {
                specialStr += specialChar;
            }
            if (replaceStr != "")
            {
                value = value.Replace(replaceStr, specialStr);
            }
        }
        catch (Exception)
        {

        }

        return value;
    }
}