<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kfsssz.aspx.cs" Inherits="RM.Web.SysSetBase.xitongcanshu.kfsssz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客房设施设置</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
        <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="tools_bar btnbartitle btnbartitlenr" style="display:block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt; </span><span>系统设置</span> &gt; <span>客房设置</span>
        </div>
    </div>
    <div class="memr">
        <div class="mrNav">
            <a>客房展示设置</a><a class="active">客房设施设置</a>
        </div>
        <div class="ptb8">
            <div class="w120" style="display: none">
                <select name="ddlType" id="ddlType">
                </select>
            </div>
            <div class="sharesearch">
                <input name="txtSearch" type="text" id="txtSearch" placeholder="请输入关键字...">
                <i class="icon-search" onclick="ListGrid(false);"></i>
            </div>
            <div class="wdyhd" style="padding-right: 12px;">
                <div class="r">
                    <span onclick="add()">添加</span><span onclick="edit()">修改</span><span onclick="Delete()">删除</span>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
