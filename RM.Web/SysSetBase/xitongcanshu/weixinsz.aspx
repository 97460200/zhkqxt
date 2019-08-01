<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="weixinsz.aspx.cs" Inherits="RM.Web.SysSetBase.xitongcanshu.weixinsz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微信设置</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-left:25px;margin-top:10px;">
        <strong style=" display:none"><b>公众号关注设置</b></strong>
        <p style="padding-bottom:10px;display:none" >
            客户访问微网或提交订单时是否需要先关注公众号
        </p>
        <div class="shareleixing" style="float:none;display:none">
            <div class="radio clearfix">
                <label class="checked">访问微网先关注公众号</label><label class="">提交订单先关注公众号</label>
            </div>
        </div>
        <strong style="display:none;"><b>公众号关注设置</b></strong>
        <%--<p style="padding-bottom:10px;">
            可在此次查看微信公众号的基本信息
        </p>--%>
        <div class="fsl" style="float: left;margin-right: 30px;font-weight: bold;margin-top: 12px;">
                    公众号基本信息
                </div>
        <dl class="addevaluate weixinsz">
            <dd>
                <small>微信名称</small>
                <div id="name" runat="server" >
                    未设置
                </div>
            </dd>
            <dd>
                <small>主体信息</small>
                <div id="subject" runat="server" >
                    未设置
                </div>
            </dd>
            <dd>
                <small>功能介绍</small>
                <div id="introduce" runat="server" >
                    未设置
                </div>
            </dd>
            <dd>
                <small>公众号管理员</small>
                <div id="adminname" runat="server" >
                    未设置
                </div>
            </dd>
            <dd>
                <small>认证到期时间</small>
                <div>
                    <span id="starttime" runat="server" >未设置</span> - <span id="endtime" runat="server">未设置</span> <i class="icon-help"></i>
                </div>
            </dd>
            <dd>
                <small>公众号logo</small>
                <div class="gzhlogo">
                    <img src="" alt="logo" id="logo" runat="server" />
                </div>
            </dd>
            <dd>
                <small>公众号二维码</small>
                <div class="gzhewm">
                    <img src="" alt="二维码"  id="ewm" runat="server"/><br/>
                    <a href="#" class="btn" style="margin-top:10px; display:none" >下载</a>
                </div>

            </dd>
            
        </dl>
    </div>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
        });
    </script>
</body>
</html>
