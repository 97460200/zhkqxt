<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="memAdminInfo.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.memAdminInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>会员卡管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" runat="server" id="hdMemberId" />
    <input type="hidden" runat="server" id="hdOpenId" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>会员管理</span><span> &gt; </span><span>会员卡管理</span>
        </div>
    </div>
    <div class="gtall gmkf clearfix">
        <div class="gmkfNav" style="display: none">
            <dl>
                <dd class="">
                    <b>美思柏丽酒店</b>
                    <ul style="display: none;">
                        <li><span>鹤山前进南路店</span> </li>
                        <li><span>佛山文华北路店</span> </li>
                        <li><span>恩平鳌峰广场店</span> </li>
                        <li><span>鹤山中心店</span> </li>
                    </ul>
                </dd>
            </dl>
        </div>
        <div class="bonusrecord">
            <div class="bonusrecord01">
                <div class="xm" id="xm" runat="server">
                    万妮达
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="" style="display: none">修改</span> <span onclick="">返回</span>
                    </div>
                </div>
            </div>
            <div class="bonusrecord02">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <span>卡号</span> <em id="khs" runat="server">159475</em>
                        </td>
                        <td>
                            <span>微信昵称</span> <em id="name" runat="server">你好，明天</em>
                        </td>
                        <td>
                            <span>性别</span> <em id="lblSex" runat="server">女</em>
                        </td>
                        <td>
                            <span>创建时间</span> <em id="addtime" runat="server">2018-01-01 18:56</em>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>手机号码</span> <em id="sjhm" runat="server">159475</em>
                        </td>
                        <td>
                            <span>消费总额</span> <em id="ddze" runat="server">0</em>
                        </td>
                        <td>
                            <span>会员卡余额</span> <em id="hykye" runat="server">0</em>
                        </td>
                        <td>
                            <span>总积分</span> <em id="jf" runat="server">0</em>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="sharetabs">
                <ul class="clearfix">
                    <li class="act"><a target="iframe1" onfocus="this.blur()" onclick="jl2(this)" href="ReservationList.aspx?lsh=<%=ae %>"
                        id="A1">订单记录</a></li>
                    <li><a target="iframe1" onfocus="this.blur()" onclick="jl2(this)" href="memcard.aspx?lsh=<%=ae %>"
                        id="A2">会员卡记录</a></li>
                    <li><a target="iframe1" onfocus="this.blur()" onclick="jl2(this)" href="../../RMBase/SysMember/StoresOrderList.aspx?lsh=<%=ae %>"
                        id="A4">服务订单</a></li>
                    <li><a target="iframe1" onfocus="this.blur()" onclick="jl2(this)" href="IntegralList.aspx?lsh=<%=ae %>"
                        id="A3">积分记录</a></li>
                        <li><a target="iframe1" onfocus="this.blur()" onclick="jl2(this)" href="../../RMBase/SysMember/ClientCoupon.aspx?lsh=<%=ae %>"
                        id="A5">卡券记录</a></li>
                    <li><a runat="server" id="aMemberSource" target="iframe1" onfocus="this.blur()" onclick="jl2(this)"
                        href="MemberSource.aspx">扫码记录</a></li>
                    <li><a runat="server" id="aOperationLog" target="iframe1" onfocus="this.blur()" onclick="jl2(this)"
                        href="OperationLog.aspx">操作记录</a></li>
                       
                    <li><a  runat="server" target="iframe1" onfocus="this.blur()" onclick="jl2(this)"  id="aWithdraw">推广佣金记录</a></li>
            
            
                        

                </ul>
            </div>
            <iframe id="iframe1" runat="server" name="iframe1" width="100%" frameborder="0" marginheight="0"
                marginwidth="0" scrolling="no" style="height: calc(100% - 200px);"></iframe>
        </div>
    </div>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $('.clearfix').on('click', 'li', function () { $(this).addClass('act').siblings().removeClass('act') });
    </script>
</body>
</html>
