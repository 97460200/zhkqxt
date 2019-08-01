<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SwitchHotel.aspx.cs" Inherits="RM.Web.Frame.SwitchHotel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function SwitchHotel(ua, up, ah) {
            debugger;
            $("#hdUser_Account").val(ua);
            $("#hdUser_Pwd").val(up);
            $("#hdAdminHotelid").val(ah);
            javascript: __doPostBack('lbtSwitch', '');
        }

        function DefaultHotel(uid) {
            debugger;
            $("#hdUserId").val(uid);
            javascript: __doPostBack('lbtDefault', '');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" runat="server" id="hdUserId" />
    <input type="hidden" runat="server" id="hdUser_Account" />
    <input type="hidden" runat="server" id="hdUser_Pwd" />
    <input type="hidden" runat="server" id="hdAdminHotelid" />
    <asp:LinkButton ID="lbtSwitch" runat="server" OnClick="lbtSwitch_Click" Style="display: none;">切换</asp:LinkButton>
    <asp:LinkButton ID="lbtDefault" runat="server" OnClick="lbtDefault_Click" Style="display: none;">设为默认</asp:LinkButton>
    <div class="switchhotel">
        <div class="switchhotel1">
            <p class="p1">
                请选择酒店
            </p>
            <p class="p2">
                下次登录会自动切换到默认酒店
            </p>
        </div>
        <div class="switchhotel2">
            <ul>
                <asp:Repeater runat="server" ID="rptAdminHotel">
                    <ItemTemplate>
                        <li class="clearfix">
                            <div class="divl">
                                <%# GetDefaultType(Eval("IsDefault").ToString(), Eval("AdminHotelid").ToString())%>
                                <em>
                                    <%# Eval("HotelName") %></em>
                            </div>
                            <div class="divr">
                                <%# GetButton(Eval("IsDefault").ToString(), Eval("User_Id").ToString(), Eval("User_Account").ToString(), Eval("User_Pwd").ToString(), Eval("AdminHotelid").ToString())%>
                               <%-- <a onclick="SwitchHotel('','','')">切换</a> <a onclick="DefaultHotel('')" class="w70 act">
                                    设为默认</a>--%>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
                <%--<li class="clearfix">
                    <div class="divl">
                        <i class="dq">当前</i><em>美思柏丽酒店（东门店）</em>
                    </div>
                    <div class="divr">
                        <a href="#" class="act">切换</a> <a href="#" class="w70">设为默认</a>
                    </div>
                </li>
                <li class="clearfix">
                    <div class="divl">
                        <em>美思柏丽酒店（东门店）</em>
                    </div>
                    <div class="divr">
                        <a href="#">切换</a> <a href="#" class="w70">设为默认</a>
                    </div>
                </li>
                <li class="clearfix">
                    <div class="divl">
                        <em>美思柏丽酒店（东门店）</em>
                    </div>
                    <div class="divr">
                        <a href="#">切换</a> <a href="#" class="w70">设为默认</a>
                    </div>
                </li>
                <li class="clearfix">
                    <div class="divl">
                        <em>美思柏丽酒店（东门店）</em>
                    </div>
                    <div class="divr">
                        <a href="#">切换</a> <a href="#" class="w70">设为默认</a>
                    </div>
                </li>--%>
            </ul>
        </div>
    </div>
    </form>
</body>
</html>
