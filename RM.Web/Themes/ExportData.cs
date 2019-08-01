using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM.Web.Themes
{
    public class ExportData
    {
        
        #region 构造函数
       public ExportData()
       {
           //
           // TODO: 在此处添加构造函数逻辑
           //
       }
       #endregion

       #region 导出页面或web控件方法
       /**//// <summary>
       /// 将Web控件或页面信息导出(不带文件名参数)
       /// </summary>
       /// <param name="source">控件实例</param>        
       /// <param name="DocumentType">导出类型:Excel或Word</param>
       public void ExportControl(System.Web.UI.Control source, string DocumentType)
       {
           //设置Http的头信息,编码格式
           if (DocumentType == "Excel")
           {
               //Excel            
               HttpContext.Current.Response.AppendHeader("Content-Disposition","attachment;filename="+ HttpUtility.UrlEncode("下载文件.xls",System.Text.Encoding.UTF8));
               HttpContext.Current.Response.ContentType = "application/ms-excel";
           }

           else if (DocumentType == "Word")
           {
               //Word
               HttpContext.Current.Response.AppendHeader("Content-Disposition","attachment;filename="+ HttpUtility.UrlEncode("下载文件.doc",System.Text.Encoding.UTF8));
               HttpContext.Current.Response.ContentType = "application/ms-word";
           }

           HttpContext.Current.Response.Charset = "UTF-8";   
           HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8; 

           //关闭控件的视图状态
           source.Page.EnableViewState =false;    

           //初始化HtmlWriter
           System.IO.StringWriter writer = new System.IO.StringWriter() ;
           System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(writer);
           source.RenderControl(htmlWriter); 

           //输出
           HttpContext.Current.Response.Write(writer.ToString());
       }

       /**//// <summary>
       /// 将Web控件或页面信息导出(带文件名参数)
       /// </summary>
       /// <param name="source">控件实例</param>        
       /// <param name="DocumentType">导出类型:Excel或Word</param>
       /// <param name="filename">保存文件名</param>
       public void ExportControl(System.Web.UI.Control source, string DocumentType, string filename)
       {
           //设置Http的头信息,编码格式
           if (DocumentType == "Excel")
           {
               //Excel            
               HttpContext.Current.Response.AppendHeader("Content-Disposition","attachment;filename="+ HttpUtility.UrlEncode(filename+".xls",System.Text.Encoding.UTF8));
               HttpContext.Current.Response.ContentType = "application/ms-excel";            
           }

           else if (DocumentType == "Word")
           {
               //Word
               HttpContext.Current.Response.AppendHeader("Content-Disposition","attachment;filename="+ HttpUtility.UrlEncode(filename+".doc",System.Text.Encoding.UTF8));
               HttpContext.Current.Response.ContentType = "application/ms-word";
           }

           HttpContext.Current.Response.Charset = "UTF-8";   
           HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8; 

           //关闭控件的视图状态
           source.Page.EnableViewState =false;    

           //初始化HtmlWriter
           System.IO.StringWriter writer = new System.IO.StringWriter() ;
           System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(writer);
           source.RenderControl(htmlWriter); 

           //输出
           HttpContext.Current.Response.Write(writer.ToString());
       }
       #endregion


        #region 调用说明
        //方法ExportControl(System.Web.UI.Control source, string DocumentType,string filename)中
        //第一个参数source表示导出的页面或控件名,当为datagrid或dataList控件时，在导出Excel/word文件时，必须把控件的分页、排序等属性去除并重新绑定，
        //第二个参数DocumentType表示导出的文件类型word或excel
        //第三个参数filename表示需要导出的文件所取的文件名
        //调用方法：
        //ExportData export=new ExportData();
        //export.ExportControl(this, "Word","testfilename");//当为this时表示当前页面
        //这是将整个页面导出为Word,并命名为testfilename
        #endregion
    }
}