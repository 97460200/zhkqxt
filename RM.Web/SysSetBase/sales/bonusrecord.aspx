<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bonusrecord.aspx.cs" Inherits="RM.Web.SysSetBase.sales.bonusrecord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>奖励记录</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
        <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <style type="text/css">
        .sharetabs li a
        {
            display: block;
        }
    </style>
    <script src="../css/ScrollBar.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            //左边导航
            $('.gmkfNav').on('click', 'b', function () {
                $(this).siblings('ul').slideToggle(120);
                $(this).parents('dd').toggleClass('down');
            });
            $('.gmkfNav ul').on('click', 'li', function () {
                $(this).addClass("active").siblings().removeClass("active");
                $("#Hdhoteladmin").val($(this).attr("AdminHotelId"));
                $("#hdHotelId").val($(this).attr("HotelId"));

                location.href = "MemberList.aspx";
            });
        });
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="Hdhoteladmin" />
    <input runat="server" type="hidden" id="hdHotelId" />
    <input runat="server" type="hidden" id="htHotelTree" value="true" />
    <input runat="server" type="hidden" id="hdMenus" />
    <input runat="server" type="hidden" id="hdMemberid" />
    <asp:HiddenField ID="hdUser_ID" runat="server" />
    
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>营销管理</span> &gt; </span><span>销售管理</span></span>
        </div>
    </div>
    <div class="gtall gmkf clearfix">
        <div class="gmkfNav" id="HotelTree" runat="server">
            <dl>
                <%=hotelTreeHtml.ToString()%>
            </dl>
        </div>
        <div class="bonusrecord">
            <div class="bonusrecord01">
                <div class="xm">
                    <asp:Literal ID="lblName" runat="server"></asp:Literal>[<asp:Literal ID="lblPhone"
                        runat="server"></asp:Literal>]
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <%--<span onclick="">修改</span> --%><span onclick="window.history.go(-1)">返回</span>
                    </div>
                </div>
            </div>
            <div class="bonusrecord02">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <span>微信昵称</span> <em>
                                <asp:Literal ID="lblWXName" runat="server"></asp:Literal></em>
                        </td>
                        <td>
                            <span>角色</span> <em>
                                <asp:Literal ID="lblRolse" runat="server"></asp:Literal></em>
                        </td>
                        <%--  <td>
                            <span>部门</span> <em>
                                <asp:Literal ID="lblRolse" runat="server"></asp:Literal></em>
                        </td>--%>
                        <td>
                            <span>创建时间</span> <em>
                                <asp:Literal ID="lblAddTime" runat="server"></asp:Literal></em>
                        </td>
                        <td>
                            <span></span><em></em>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>累计销售额</span> <em>
                                <asp:Literal ID="lblSales_Total" runat="server"></asp:Literal></em>
                        </td>
                        <td>
                            <span>累计奖金</span> <em>
                                <asp:Literal ID="lblBonus_Total" runat="server"></asp:Literal></em>
                        </td>
                        <td>
                            <span>累计提现</span> <em>
                                <asp:Literal ID="lblWithdrawCash" runat="server"></asp:Literal></em>
                        </td>
                        <td>
                            <span>可提奖金</span> <em>
                                <asp:Literal ID="lblExtractableMoney" runat="server"></asp:Literal></em>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>累计注册客户</span> <em>
                                <asp:Literal ID="lblRegister_Total" runat="server"></asp:Literal></em>
                        </td>
                        <td>
                            <span>累计入住客户</span> <em>
                                <asp:Literal ID="lblCheckIn_Total" runat="server"></asp:Literal></em>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="sharetabs">
                <ul class="clearfix">
                    <li class="act"><a target="iframe1" onclick="jl2(this)" onfocus="this.blur()" href="OrderRecord.aspx?UserId=<%=hdUser_ID.Value%>"
                        class="selected">奖金记录</a> </li>
                    <li><a target="iframe1" onfocus="this.blur()" onclick="jl2(this)" href="TiXianList.aspx?UserId=<%=hdUser_ID.Value%>"
                        id="hrefBase">提现记录</a> </li>
                    <li><a target="iframe1" onfocus="this.blur()" onclick="jl2(this)" href="DaiLiKeHu.aspx?UserId=<%=hdUser_ID.Value%>"
                        id="A1">带来客户</a> </li>
                </ul>
            </div>
   

  
                            <iframe id="iframe1" runat="server" name="iframe1" width="100%" frameborder="0"
                                marginheight="0" marginwidth="0" scrolling="no" style="height:calc(100% - 233px);"></iframe>
                            <script language="javascript">
                                var url = location.search; //获取url中"?"符后的字串
                            
                                var str = "";
                                if (url.indexOf("?") != -1) {
                                    var str = url.substr(1);
                                    strs = str.split("&");
                                    for (var i = 0; i < strs.length; i++) {
                                        str = (strs[i].split("=")[1]);
                                    }
                                }
                                var ifa = document.all("iframe1");
                                var UserId = $("#hdUser_ID").val();
                              
                                ifa.src = "OrderRecord.aspx?UserId=" + UserId;
                            </script>
                 

        </div>

        
    <script type="text/javascript">


        $('.sharetabs').on('click', 'li', function () {
            var index = $(this).index();
            $(this).addClass('act').siblings().removeClass('act');

            $('.rge').eq(index).show().siblings('.rge').hide();
        })

    </script>
    </form>
</body>
</html>
