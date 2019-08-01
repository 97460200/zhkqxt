﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

namespace RM.Common.DotNetUI
{
    /// <summary>
    /// 客户端提示信息帮助类
    /// </summary>
    public class ShowMsgHelper
    {
        /// <summary>
        /// 默认成功提示
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void Alert(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');", message));
        }

        /// <summary>
        /// 默认成功提示，关闭页面
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void OpenClose(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');OpenClose();", message));
        }

        /// <summary>
        /// 默认成功提示，关闭页面
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void AlertClose(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');window.parent.frames[Current_iframeID()].location.reload();OpenClose();", message));
        }

        /// <summary>
        /// 默认成功提示，刷新父窗口函数关闭页面
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void AlertMsg(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');window.parent.frames[Current_iframeID()].frames['target_right'].location.reload();OpenClose();", message));
        }
        /// <summary>
        /// 默认成功提示，刷新父窗口函数关闭页面
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void ParmAlertMsg(string message)
        {
            //ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');window.parent.frames[Current_iframeID()].location.reload();OpenClose();", message));
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');window.parent.frames[Current_iframeID()].frames['target_right'].location.reload();OpenClose();", message));
        }


        /// <summary>
        /// 默认成功提示，刷新父窗口函数关闭页面
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void ParmAlertMsgS(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');window.parent.frames[Current_iframeID()].location.reload();OpenClose();", message));

        }
        /// <summary>
        /// 默认成功提示，刷新父窗口指定函数 并关闭页面
        /// </summary>
        /// <param name="message">提示语</param>
        /// <param name="method">父级方法</param>
        public static void AlertReloadClose(string message, string method)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');window.parent.frames[Current_iframeID()].{1};OpenClose();", message, method));
        }

        /// <summary>
        /// 默认成功提示，刷新父窗口函数关闭页面
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void ParmAlertMsg(string dialogId, string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');window.parent.frames[Current_iframeID()].location.reload();OpenClose();", message));
        }
        /// <summary>
        /// 默认错误提示
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void Alert_Error(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','5000','5');", message));
        }
        /// <summary>
        /// 默认警告提示
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void Alert_Wern(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','3000','3');", message));
        }
        /// <summary>
        /// 提示警告信息
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void showFaceMsg(string message)
        {
            ExecuteScript(string.Format("showFaceMsg('{0}');", message));
        }
        /// <summary>
        /// 提示警告信息
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void showWarningMsg(string message)
        {
            ExecuteScript(string.Format("showWarningMsg('{0}');", message));
        }

        /// <summary>
        /// 后台调用JS函数
        /// </summary>
        /// <param name="obj"></param>
        public static void ShowScript(string strobj)
        {
            Page p = HttpContext.Current.Handler as Page;
            p.ClientScript.RegisterStartupScript(p.ClientScript.GetType(), "myscript", "<script>" + strobj + "</script>");
        }
        public static void ExecuteScript(string scriptBody)
        {
            string scriptKey = "Somekey";
            Page p = HttpContext.Current.Handler as Page;
            p.ClientScript.RegisterStartupScript(typeof(string), scriptKey, scriptBody, true);
        }
    }
}