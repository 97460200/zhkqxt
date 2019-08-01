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
/// 酒店树 帮助类
/// </summary>
public static class HotelTreeHelper
{
    #region 酒店树
    /// <summary>
    /// 
    /// </summary>
    /// <param name="adminHotelid">集团酒店id</param>
    /// <param name="type">1酒店,2分店</param>
    /// <param name="blHotelTree">是否有多分店</param>
    /// <param name="HotelId">默认酒店ID</param>
    /// <returns></returns>
    public static string HotelTree(string adminHotelid, int type, out bool blHotelTree, out string HotelId)
    {
        string hotelids = RequestSession.GetSessionUser().HotelListId.ToString();
        HttpContext rq = HttpContext.Current;
        string treeSession = "HotelTree" + adminHotelid + "_" + type;
        string blSessionTree = "blHotelTree" + adminHotelid + "_" + type;
        string hidSession = "HotelIdSession" + adminHotelid + "_" + type;
        object ht = rq.Session[treeSession];
        if (ht != null && ht.ToString() != "")
        {
            blHotelTree = (bool)rq.Session[blSessionTree];
            HotelId = rq.Session[hidSession].ToString();
            return ht.ToString();
        }
        blHotelTree = false;
        HotelId = "-1";
        StringBuilder strHtml = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT id,name,AdminHotelid FROM  Hotel_Admin WHERE AdminHotelid= @AdminHotelid");
        SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", adminHotelid) };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string hotelName = dt.Rows[i]["name"].ToString();
                string ahid = dt.Rows[i]["AdminHotelid"].ToString();//预设可管理多个酒店用
                strHtml.Append("<dd class='down'>");
                strHtml.AppendFormat("<b AdminHotelId = '{0}' HotelId = '0'>{1}</b>", ahid, hotelName);
                //创建子节点
                strHtml.Append(GetTreeNode(ahid, type, hotelids, out blHotelTree, out HotelId));
                strHtml.Append("</dd>");
            }
        }
        if (strHtml.ToString() != "")
        {
            rq.Session[treeSession] = strHtml;
            rq.Session[blSessionTree] = blHotelTree;
            rq.Session[hidSession] = HotelId;
        }
        return strHtml.ToString();
    }


    /// <summary>
    /// 创建子节点
    /// </summary>
    /// <param name="parentID">父节点主键</param>
    /// <param name="dtMenu"></param>
    /// <returns></returns>
    public static string GetTreeNode(string adminHotelid, int type, string hotelids, out bool blHotelTree, out string HotelId)
    {
        blHotelTree = false;
        HotelId = "-1";
        StringBuilder sb = new StringBuilder();
        sb.Append("select id,name from Hotel where DeleteMark=1 and AdminHotelid = @AdminHotelid ");
        if (hotelids != "" && hotelids != "0")
        {
            sb.AppendFormat(" and id in({0})", hotelids);
        }
        if (type == 1 || type == 2)
        {
            sb.AppendFormat(" and type = '{0}' ", type);
        }
        sb.Append(" order by sort desc");
        SqlParam[] param = new SqlParam[] { new SqlParam("@AdminHotelid", adminHotelid) };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);

        StringBuilder sb_TreeNode = new StringBuilder();
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Rows.Count == 1)
            {
                blHotelTree = false;
                HotelId = dt.Rows[0]["id"].ToString();
            }
            else
            {
                blHotelTree = true;
            }
            sb_TreeNode.Append("<ul style='display:block;'>");
            if (type == 8)
            {

                StringBuilder sb_8 = new StringBuilder();
                sb_8.Append("SELECT  dlyy_gzh FROM dbo.Set_Association WHERE dlyy_gzh = 1 AND AdminHotelid = @AdminHotelid");
                SqlParam[] param_8 = new SqlParam[] { new SqlParam("@AdminHotelid", adminHotelid) };
                DataTable dt_8 = DataFactory.SqlDataBase().GetDataTableBySQL(sb_8, param_8);
                if (dt_8 != null && dt_8.Rows.Count > 0)
                {
                    sb_TreeNode.AppendFormat("<li AdminHotelId = '{0}' HotelId = '{1}'>", adminHotelid, 0);
                    sb_TreeNode.AppendFormat("<span>{0}</span>", "运营商户号");
                    sb_TreeNode.Append("</li>");
                }
            }
            foreach (DataRow drv in dt.Rows)
            {
                sb_TreeNode.AppendFormat("<li AdminHotelId = '{0}' HotelId = '{1}'>", adminHotelid, drv["id"]);
                sb_TreeNode.AppendFormat("<span>{0}</span>", drv["name"]);
                sb_TreeNode.Append("</li>");
            }
            sb_TreeNode.Append("</ul>");
        }
        return sb_TreeNode.ToString();
    }

    #endregion


    #region 酒店房型树
    public static string RoomTree(int type, out string HotelId)
    {
        HotelId = "-1";
        HttpContext rq = HttpContext.Current;
        object ht = rq.Session["RoomTree"];
        if (ht != null && ht.ToString() != "")
        {
            HotelId = rq.Session["Room_HotelId"].ToString();
            return ht.ToString();
        }

        SessionUser su = RequestSession.GetSessionUser();
        StringBuilder sbTree = new StringBuilder();
        if (su != null)
        {
            string adminHotelid = su.AdminHotelid.ToString();
            string hotelids = su.HotelListId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"
            SELECT  a.id ,
                    a.name ,
                    b.Name AdminHotelName
            FROM    Hotel a
                    LEFT JOIN Hotel_Admin b ON a.AdminHotelid = b.AdminHotelid
            WHERE   a.DeleteMark = 1 AND a.[TYPE] = {0}
                    AND a.AdminHotelid = @AdminHotelid ", type);
            if (hotelids != "")
            {
                sb.AppendFormat(" and a.id in({0})", hotelids);
            }
            sb.Append(" order by a.sort desc");
            SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", adminHotelid) };
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count == 1)
                {
                    #region 单体酒店
                    HotelId = dt.Rows[0]["id"].ToString();
                    string hotelName = dt.Rows[0]["name"].ToString();

                    sbTree.Append("<dd class='down'>");
                    sbTree.AppendFormat("<b class='Hotel' AdminHotelId = '{0}' HotelId = '{1}'>{2}</b>", adminHotelid, HotelId, hotelName);
                    //创建子节点
                    sbTree.Append(GetTreeNode(adminHotelid, HotelId, true));
                    sbTree.Append("</dd>");
                    #endregion
                }
                else
                {
                    #region 连锁酒店
                    string AdminHotelName = dt.Rows[0]["AdminHotelName"].ToString();
                    sbTree.Append("<dd class='down'>");
                    sbTree.AppendFormat("<b AdminHotelId = '{0}' HotelId = '0'>{1}</b>", adminHotelid, AdminHotelName);
                    //创建子节点
                    sbTree.Append(GetTreeNode(adminHotelid, dt));
                    sbTree.Append("</dd>");
                    #endregion
                }
            }
        }
        return sbTree.ToString();
    }

    /// <summary>
    /// 单体酒店-创建子节点
    /// </summary>
    /// <param name="parentID">父节点主键</param>
    /// <param name="dtMenu"></param>
    /// <returns></returns>
    public static string GetTreeNode(string adminHotelid, string HotelId, bool type)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT ID,Name FROM Guestroom WHERE HotelID = @HotelID ORDER BY Sort DESC");
        SqlParam[] param = new SqlParam[] { new SqlParam("HotelID", HotelId) };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        string style = " style='display:none;'";
        if (type)
        {
            style = " style='display:block;'";
        }
        StringBuilder sb_TreeNode = new StringBuilder();
        if (dt != null && dt.Rows.Count > 0)
        {
            sb_TreeNode.AppendFormat("<ul {0}>", style);
            foreach (DataRow drv in dt.Rows)
            {
                sb_TreeNode.AppendFormat("<li datatype='Room' AdminHotelId = '{0}' HotelId = '{1}' RoomId = '{2}'>", adminHotelid, HotelId, drv["id"]);
                sb_TreeNode.AppendFormat("<span>{0}</span>", drv["name"]);
                sb_TreeNode.Append("</li>");
            }
            sb_TreeNode.Append("</ul>");
        }
        return sb_TreeNode.ToString();
    }

    /// <summary>
    /// 连锁酒店-创建子节点
    /// </summary>
    /// <param name="parentID">父节点主键</param>
    /// <param name="dtMenu"></param>
    /// <returns></returns>
    public static string GetTreeNode(string adminHotelid, DataTable dt)
    {
        StringBuilder sb_TreeNode = new StringBuilder();
        sb_TreeNode.Append("<ul style='display:block;'>");
        foreach (DataRow drv in dt.Rows)
        {
            sb_TreeNode.AppendFormat("<li datatype='Hotel' class='li' AdminHotelId = '{0}' HotelId = '{1}'>", adminHotelid, drv["id"]);
            sb_TreeNode.Append("<dl><dd>");
            sb_TreeNode.AppendFormat("<b>{0}</b>", drv["name"]);
            sb_TreeNode.Append(GetTreeNode(adminHotelid, drv["id"].ToString(), false));
            sb_TreeNode.Append("</dd></dl>");
            sb_TreeNode.Append("</li>");
        }
        sb_TreeNode.Append("</ul>");

        return sb_TreeNode.ToString();
    }


    #endregion
    /// <summary>
    /// 获取积分开启状态,1：开启  2关闭
    /// </summary>
    /// <returns></returns>
    internal static string GetJFState()
    {
        string AdminHotelid = RequestSession.GetSessionUser().AdminHotelid.ToString();
        string jfState = "JFState" + AdminHotelid;
        HttpContext rq = HttpContext.Current;
        object ht = rq.Session[jfState];
        if (ht != null && ht.ToString() != "")
        {
            return ht.ToString();
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT isjf FROM jfMatter WHERE AdminHotelid = @AdminHotelid");
        SqlParam[] param = new SqlParam[] { new SqlParam("AdminHotelid", AdminHotelid) };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);
        string JFState = "0";
        if (dt != null && dt.Rows.Count > 0)
        {
            JFState = dt.Rows[0][0].ToString();
        }
        rq.Session[jfState] = JFState;
        return JFState;
    }

    #region  获取所有酒店列表
    internal static string AllHotelTree()
    {
        string hotelTreeHtml = "";
        HttpContext rq = HttpContext.Current;
        object ht = rq.Session["AllHotelTree"];
        if (ht != null && ht.ToString() != "")
        {
            hotelTreeHtml = rq.Session["AllHotelTree"].ToString();
            return ht.ToString();
        }
        StringBuilder sbTree = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT * FROM dbo.Hotel_Admin WHERE DeleteMark = 1 AND state = 1 ORDER BY AddTime ");
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb);

        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string adminHotelid = dt.Rows[i]["AdminHotelId"].ToString();
                string AdminHotelName = dt.Rows[i]["name"].ToString();
                sbTree.Append("<dd class='down'>");
                sbTree.AppendFormat("<b AdminHotelId = '{0}' HotelId = '0'>{1}</b>", adminHotelid, AdminHotelName);
                //创建子节点
                sbTree.Append(GetHotelTreeNode(adminHotelid, 1));
                sbTree.Append("</dd>");
            }
        }
        if (!string.IsNullOrEmpty(sbTree.ToString()))
        {
            rq.Session["AllHotelTree"] = sbTree.ToString();
        }
        return sbTree.ToString();
    }

    /// <summary>
    /// 加载酒店
    /// </summary>
    /// <param name="adminHotelid">集团id</param>
    /// <param name="type">类型 1酒店 , 2门店</param>
    /// <returns></returns>
    public static string GetHotelTreeNode(string adminHotelid, int type)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("select id,name from Hotel where DeleteMark = 1 and AdminHotelid = @AdminHotelid ");
        if (type == 1 || type == 2)
        {
            sb.AppendFormat(" and type = '{0}' ", type);
        }
        sb.Append(" order by sort desc");
        SqlParam[] param = new SqlParam[] { new SqlParam("@AdminHotelid", adminHotelid) };
        DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, param);

        StringBuilder sb_TreeNode = new StringBuilder();
        if (dt != null && dt.Rows.Count > 0)
        {
            sb_TreeNode.Append("<ul style='display:block;'>");
            foreach (DataRow drv in dt.Rows)
            {
                sb_TreeNode.AppendFormat("<li AdminHotelId = '{0}' HotelId = '{1}'>", adminHotelid, drv["id"]);
                sb_TreeNode.AppendFormat("<span>{0}</span>", drv["name"]);
                sb_TreeNode.Append("</li>");
            }
            sb_TreeNode.Append("</ul>");
        }
        return sb_TreeNode.ToString();
    }
    #endregion
}
