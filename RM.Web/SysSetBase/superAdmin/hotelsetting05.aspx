<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hotelsetting05.aspx.cs" Inherits="RM.Web.SysSetBase.superAdmin.hotelsetting05" %>

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
        <dl class="addevaluate xiaoshoushezhi">
            <dd>
                <small>奖励规则</small>
                <div class="radio">
                    <label class="checked" >手动提现</label>
                    <label>线下核算</label><br>
                    <p>
                        单笔最小金额<input type="text" name="name" value=" " /><span>单笔最小金额默认为1元</span>
                    </p>
                    <p>
                        单笔单日限额<input type="text" name="name" value=" " /><span>单笔单日限额为2W</span>
                    </p>
                    <p>
                        每日最多提现<input type="text" name="name" value=" " /><span>每个用户每天最多可提现10次</span>
                    </p>
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
