﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cwls.aspx.cs" Inherits="RM.Web.SysSetBase.superAdmin.cwls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>财务流水</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .bonutext
        {
            float: left;
            line-height: 26px;
            margin-left: 10px;
        }
        .bonutext em{ color: #e00; margin:0 2px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="gtall gmkf clearfix">
        <div class="bonusrecord">
            <div class="bonusrecord03">
                <div class="w120">
                    <select>
                        <option value="">全部酒店</option>
                        <option value="金逸酒店">金逸酒店</option>
                    </select>
                </div>
                <div class="sharedate">
                    <input name="txtStartTime" type="text" value="2018-01-01">
                    <input name="txtEndTime" type="text" value="2018-02-01">
                </div>
                <div class="bonutext">
                    期间：<b>总收入(元)<em>200655.00</em>(系统维护费3000.00，支付手续费30000.00，短信通知费2000.00)</b>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span>导出</span>
                    </div>
                </div>
            </div>
            <table>
                <thead>
                    <tr>
                        <th>
                            阿斯
                        </th>
                        <th>
                            华发商都
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            䧃
                        </td>
                        <td>
                            欧冠
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
