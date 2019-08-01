<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cancel.aspx.cs" Inherits="RM.Web.SysSetBase.guestroomorder.cancel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>取消订单</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="clearfix">
        <dl class="addevaluate cancelorder">
            <dd>
                <small>取消订单原因</small>
                <div>
                    <select>
                        <option value="value">请选择</option>
                    </select>
                </div>
            </dd>
            <dd>
                <small>详细说明</small>
                <div>
                    <textarea>content</textarea>
                </div>
            </dd>
            <dd>
                <small>发送通知</small>
                <div class="checkbox">
                    <span style="float:left;margin-right:10px;">取消订单原因发送给预订人</span><label class="checked">短信</label><label class="checked">微信</label>
                </div>
            </dd>
        </dl>
    </form>
</body>
</html>
