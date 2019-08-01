<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jjrsz.aspx.cs" Inherits="RM.Web.SysSetBase.xitongcanshu.jjrsz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="display:none;">
        <div class="fangjiamoban" style="margin-right:0;">
            <div class="fangjiamobant clearfix">
                <span>房价模板</span><span><img src="/SysSetBase/img/mbadd.png" /></span>
            </div>
            <div class="fangjiamobanb">
                <ul>
                    <li class="clearfix">
                        <span>平日价模板A</span><span><img src="/SysSetBase/img/del10.png" /></span>
                    </li>
                    <li class="clearfix">
                        <span>平日价模板A</span><span><img src="/SysSetBase/img/del10.png" /></span>
                    </li>
                    <li class="clearfix">
                        <span>平日价模板A</span><span><img src="/SysSetBase/img/del10.png" /></span>
                    </li>
                    <li class="clearfix">
                        <span>平日价模板A</span><span><img src="/SysSetBase/img/del10.png" /></span>
                    </li>
                </ul>
            </div>
        </div>
        <div class="shareright">
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
    </div>
    <dl class="addevaluate" style="width:395px;height:90px;display:none;">
        <dd>
            <small>选择年份</small>
            <div>
                <select style="width:100%;">
                    <option value="">请选择</option>
                </select>
            </div>
        </dd>
    </dl>

    <dl class="addhousetypeInfo addjjr" style="width:430px;height:170px;">
        <dd>
            <small>节日名称</small>
            <div>
                <input type="text" name="name" value="" style="width:100%;"/>
            </div>
        </dd>
        <dd>
            <small>时间段</small>
            <div class="date">
                <input type="text" value="2018-04-12">
                <input type="text" value="2018-04-12"><span>共1天</span>
            </div>
        </dd>
        <dd>
            <small>积分是否可用</small>
            <div class="radio">
                <label class="checked">可用</label>
                <label>不可用</label>
            </div>
        </dd>
    </dl>
    </form>
</body>
</html>
