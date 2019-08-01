<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="guanlifulileixing.aspx.cs" Inherits="RM.Web.SysSetBase.yingxiaoguanli.guanlifulileixing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>管理福利类型</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <dl class="addevaluate guanlifulileixing">
        <dd>
            <small>现金红包</small><div class="radio">
                <label class="checked" value='1' >开启</label>
                <label value='0'>关闭</label>
            </div>
        </dd>
        <dd>
            <small>小礼品</small><div class="radio">
                <label class="checked" value='1' >开启</label>
                <label value='0'>关闭</label>
            </div>
        </dd>
        <dd>
            <small>卡券</small><div class="radio">
                <label class="checked" value='1' >开启</label>
                <label value='0'>关闭</label>
            </div>
        </dd>
        <dd>
            <small>积分</small><div class="radio">
                <label class="checked" value='1' >开启</label>
                <label value='0'>关闭</label>
            </div>
        </dd>
    </dl>
    </form>
</body>
</html>
