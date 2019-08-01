<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hotelsetting03.aspx.cs" Inherits="RM.Web.SysSetBase.superAdmin.hotelsetting03" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>酒店管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="hotelsetting">
            <div class="hotelname">
                柏丽酒店
            </div>
            <div class="sharetabs">
                <ul class="clearfix">
                    <li><a href="###">营销设置</a> </li>
                    <li><a href=="###">微信设置</a> </li>
                    <li><a href=="###">短信设置</a> </li>
                    <li class="act"><a href=="###">功能设置</a> </li>
                    <li><a href=="###">系统对接</a> </li>
                </ul>
            </div>

            <dl class="addevaluate weixinshezhi duanxinshezhi">
                <div class="d1">
                    设置是否启用以下功能
                </div>
                <dd>
                    <div class="checkbox">
                        <label class="checked">会员卡功能</label>
                        <label class="checked">会员卡充值</label>
                        <label>会员卡升级</label>
                        <label>分销功能</label>
                        <label>抽奖（活动）功能</label>
                        <label>手动修改会员卡等级</label>
                        <label>积分功能</label>
                        <label>手动增减积分</label>
                        <label>评价功能</label>
                        <label>卡券功能</label>
                    </div>
                </dd>
                <div class="membtn">
                    <a class="button buttonActive" onclick="Submit()">保存</a>
                </div>
            </dl>
        </div>
    </form>
</body>
</html>
