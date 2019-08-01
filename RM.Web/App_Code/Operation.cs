using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Operation 的摘要说明
/// </summary>
public class Operation
{
    public Operation()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public new void Fill(DataTable dt, int pageSize, GridView GV, Page page)
    {
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = dt.DefaultView;
        pds.AllowPaging = true;
        pds.PageSize = pageSize;

        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        ImageButton IB_first = ((ImageButton)(page.Form.FindControl("IB_first")));
        ImageButton IB_previous = ((ImageButton)(page.Form.FindControl("IB_previous")));
        ImageButton IB_next = ((ImageButton)(page.Form.FindControl("IB_next")));
        ImageButton IB_last = ((ImageButton)(page.Form.FindControl("IB_last")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        if (Convert.ToInt32(Lab_flag.Text) > pds.PageCount)
        {
            Lab_flag.Text = ((int)(Convert.ToInt32(Lab_flag.Text) - 1)).ToString();
        }
        int pageindex = Convert.ToInt32(Lab_flag.Text);
        pds.CurrentPageIndex = pageindex - 1;

        ((Label)(page.Form.FindControl("Lab_end"))).Text = pds.PageCount.ToString();
        ((Label)(page.Form.FindControl("Lab_count"))).Text = dt.Rows.Count.ToString();

        IB_first.Enabled = true;
        IB_previous.Enabled = true;
        IB_next.Enabled = true;
        IB_last.Enabled = true;

        if (!page.IsPostBack)
        {
            for (int i = 1; i <= pds.PageCount; i++)
            {
                DDL_page.Items.Add(i.ToString());
            }
        }

        if (pds.IsFirstPage)
        {
            IB_first.Enabled = false;
            IB_previous.Enabled = false;
        }
        if (pds.IsLastPage)
        {
            IB_next.Enabled = false;
            IB_last.Enabled = false;
        }

        //page.Response.Write(a.GetType().ToString());
        GV.DataSource = pds;
        GV.DataBind();
    }

    public new void Fill(DataSet ds, int pageSize, GridView GV, Page page)
    {
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = ds.Tables[0].DefaultView;
        pds.AllowPaging = true;
        pds.PageSize = pageSize;

        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        ImageButton IB_first = ((ImageButton)(page.Form.FindControl("IB_first")));
        ImageButton IB_previous = ((ImageButton)(page.Form.FindControl("IB_previous")));
        ImageButton IB_next = ((ImageButton)(page.Form.FindControl("IB_next")));
        ImageButton IB_last = ((ImageButton)(page.Form.FindControl("IB_last")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        if (Convert.ToInt32(Lab_flag.Text) > pds.PageCount)
        {
            Lab_flag.Text = ((int)(Convert.ToInt32(Lab_flag.Text) - 1)).ToString();
        }
        int pageindex = Convert.ToInt32(Lab_flag.Text);
        pds.CurrentPageIndex = pageindex - 1;

        ((Label)(page.Form.FindControl("Lab_end"))).Text = pds.PageCount.ToString();
        ((Label)(page.Form.FindControl("Lab_count"))).Text = ds.Tables[0].Rows.Count.ToString();

        IB_first.Enabled = true;
        IB_previous.Enabled = true;
        IB_next.Enabled = true;
        IB_last.Enabled = true;

        if (!page.IsPostBack)
        {
            for (int i = 1; i <= pds.PageCount; i++)
            {
                DDL_page.Items.Add(i.ToString());
            }
        }

        if (pds.IsFirstPage)
        {
            IB_first.Enabled = false;
            IB_previous.Enabled = false;
        }
        if (pds.IsLastPage)
        {
            IB_next.Enabled = false;
            IB_last.Enabled = false;
        }

        //page.Response.Write(a.GetType().ToString());
        GV.DataSource = pds;
        GV.DataBind();

    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public void QueryData(DataTable dt, int pageSize, GridView GV, Page page)
    {
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = dt.DefaultView;
        pds.AllowPaging = true;
        pds.PageSize = pageSize;

        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        ImageButton IB_first = ((ImageButton)(page.Form.FindControl("IB_first")));
        ImageButton IB_previous = ((ImageButton)(page.Form.FindControl("IB_previous")));
        ImageButton IB_next = ((ImageButton)(page.Form.FindControl("IB_next")));
        ImageButton IB_last = ((ImageButton)(page.Form.FindControl("IB_last")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        Lab_flag.Text = "1";
        int pageindex = Convert.ToInt32(Lab_flag.Text);
        pds.CurrentPageIndex = pageindex - 1;

        ((Label)(page.Form.FindControl("Lab_end"))).Text = pds.PageCount.ToString();
        ((Label)(page.Form.FindControl("Lab_count"))).Text = dt.Rows.Count.ToString();

        IB_first.Enabled = true;
        IB_previous.Enabled = true;
        IB_next.Enabled = true;
        IB_last.Enabled = true;

        DDL_page.Items.Clear();
        for (int i = 1; i <= pds.PageCount; i++)
        {
            DDL_page.Items.Add(i.ToString());
        }

        if (pds.IsFirstPage)
        {
            IB_first.Enabled = false;
            IB_previous.Enabled = false;
        }
        if (pds.IsLastPage)
        {
            IB_next.Enabled = false;
            IB_last.Enabled = false;
        }

        //page.Response.Write(a.GetType().ToString());
        GV.DataSource = pds;
        GV.DataBind();
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public new void QueryData(DataSet ds, int pageSize, GridView GV, Page page)
    {
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = ds.Tables[0].DefaultView;
        pds.AllowPaging = true;
        pds.PageSize = pageSize;

        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        ImageButton IB_first = ((ImageButton)(page.Form.FindControl("IB_first")));
        ImageButton IB_previous = ((ImageButton)(page.Form.FindControl("IB_previous")));
        ImageButton IB_next = ((ImageButton)(page.Form.FindControl("IB_next")));
        ImageButton IB_last = ((ImageButton)(page.Form.FindControl("IB_last")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        Lab_flag.Text = "1";
        int pageindex = Convert.ToInt32(Lab_flag.Text);
        pds.CurrentPageIndex = pageindex - 1;

        ((Label)(page.Form.FindControl("Lab_end"))).Text = pds.PageCount.ToString();
        ((Label)(page.Form.FindControl("Lab_count"))).Text = ds.Tables[0].Rows.Count.ToString();

        IB_first.Enabled = true;
        IB_previous.Enabled = true;
        IB_next.Enabled = true;
        IB_last.Enabled = true;

        DDL_page.Items.Clear();
        for (int i = 1; i <= pds.PageCount; i++)
        {
            DDL_page.Items.Add(i.ToString());
        }

        if (pds.IsFirstPage)
        {
            IB_first.Enabled = false;
            IB_previous.Enabled = false;
        }
        if (pds.IsLastPage)
        {
            IB_next.Enabled = false;
            IB_last.Enabled = false;
        }

        //page.Response.Write(a.GetType().ToString());
        GV.DataSource = pds;
        GV.DataBind();

    }

    /// <summary>
    /// 首页
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public void ClickFirst(DataTable dt, int pageSize, GridView GV, Page page)
    {
        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        int first = 1;
        Lab_flag.Text = first.ToString();
        DDL_page.SelectedValue = Lab_flag.Text;
        Fill(dt, pageSize, GV, page);
    }

    /// <summary>
    /// 首页
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public void ClickFirst(DataSet ds, int pageSize, GridView GV, Page page)
    {
        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        int first = 1;
        Lab_flag.Text = first.ToString();
        DDL_page.SelectedValue = Lab_flag.Text;
        Fill(ds, pageSize, GV, page);
    }

    /// <summary>
    /// 上一页
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public void ClickPrevious(DataTable dt, int pageSize, GridView GV, Page page)
    {
        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        Lab_flag.Text = Convert.ToString(Convert.ToInt32(Lab_flag.Text) - 1);
        DDL_page.SelectedValue = Lab_flag.Text;
        Fill(dt, pageSize, GV, page);
    }

    /// <summary>
    /// 上一页
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public void ClickPrevious(DataSet ds, int pageSize, GridView GV, Page page)
    {
        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        Lab_flag.Text = Convert.ToString(Convert.ToInt32(Lab_flag.Text) - 1);
        DDL_page.SelectedValue = Lab_flag.Text;
        Fill(ds, pageSize, GV, page);
    }

    /// <summary>
    /// 下一页
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public void ClickNext(DataTable dt, int pageSize, GridView GV, Page page)
    {
        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        Lab_flag.Text = Convert.ToString(Convert.ToInt32(Lab_flag.Text) + 1);
        DDL_page.SelectedValue = Lab_flag.Text;
        Fill(dt, pageSize, GV, page);
    }

    /// <summary>
    /// 下一页
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public void ClickNext(DataSet ds, int pageSize, GridView GV, Page page)
    {
        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        Lab_flag.Text = Convert.ToString(Convert.ToInt32(Lab_flag.Text) + 1);
        DDL_page.SelectedValue = Lab_flag.Text;
        Fill(ds, pageSize, GV, page);
    }

    /// <summary>
    /// 最后一页
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public void ClickLast(DataTable dt, int pageSize, GridView GV, Page page)
    {
        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        Label Lab_end = ((Label)(page.Form.FindControl("Lab_end")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        Lab_flag.Text = Lab_end.Text;
        DDL_page.SelectedValue = Lab_flag.Text;
        Fill(dt, pageSize, GV, page);
    }

    /// <summary>
    /// 最后一页
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public void ClickLast(DataSet ds, int pageSize, GridView GV, Page page)
    {
        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        Label Lab_end = ((Label)(page.Form.FindControl("Lab_end")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        Lab_flag.Text = Lab_end.Text;
        DDL_page.SelectedValue = Lab_flag.Text;
        Fill(ds, pageSize, GV, page);
    }

    /// <summary>
    /// 选择页
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public void ChangePage(DataTable dt, int pageSize, GridView GV, Page page)
    {
        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        Lab_flag.Text = DDL_page.SelectedItem.Text.ToString();
        Fill(dt, pageSize, GV, page);
    }

    /// <summary>
    /// 选择页
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="pageSize"></param>
    /// <param name="GV"></param>
    /// <param name="page"></param>
    public void ChangePage(DataSet ds, int pageSize, GridView GV, Page page)
    {
        Label Lab_flag = ((Label)(page.Form.FindControl("Lab_flag")));
        DropDownList DDL_page = ((DropDownList)(page.Form.FindControl("DDL_page")));

        Lab_flag.Text = DDL_page.SelectedItem.Text.ToString();
        Fill(ds, pageSize, GV, page);
    }
}
