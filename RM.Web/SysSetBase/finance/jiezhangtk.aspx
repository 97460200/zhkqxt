<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jiezhangtk.aspx.cs" Inherits="RM.Web.SysSetBase.finance.jiezhangtk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>结账弹框</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <dl class="addjiezhang">
        <dd>
            <small>付款码</small>
            <div>
                <input type="text" />
            </div>
        </dd>
        <dd>
            <small>操作人</small>
            <div>
                李鑫
            </div>
        </dd>
    </dl>
    <div class="sharebottombtn">
        <div class="fr">
            <input type="submit" name="btnSumit" value="提交">
            <input type="submit" name="btnSumit" value="重置">
        </div>
    </div>
    </form>
</body>
</html>
