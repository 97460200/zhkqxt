<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UdWpd2.aspx.cs" Inherits="RM.Web.Frame.UdWpd2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>找回密码 - 智订云酒店营销系统</title>
    <link href="/Themes/Styles/login.css" rel="stylesheet" type="text/css" />
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="../App_Themes/default/js/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../App_Themes/default/js/SolvePlaceholder.js" type="text/javascript"></script>
    <link href="/Themes/Images/Login/logo.png" rel="icon" />
    <script>

        $(function () {

            $("#Okbut").click(function () {
                //alert($("#txtPwd").val().length);
                if ($("#txtPwd").val().length < 6) {
                    alert("请输入至少6位密码！");
                    $("#txtPwd").focus();
                    return false;
                }

                if ($("#txtRePwd").val() == "") {
                    alert("请重复输入密码！");
                    $("#txtRePwd").focus();
                    return false;
                }

                if ($("#txtPwd").val() != $("#txtRePwd").val()) {
                    alert("两次密码输入不一致！");
                    $("#txtPwd").attr("value", "");
                    $("#txtRePwd").attr("value", "");
                    $("#txtPwd").focus();
                    return false;
                }
            });

        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="login">
        <!---重新设置密码--->
        <div class="boxLogin setpassword">
            <div class="login_head">
                <table>
                    <tr>
                        <td>
                            <img src="/Themes/Images/Login/adminL.png" />
                        </td>
                        <td class="nn">
                            <span id="Span1" style="font-size: 16px; font-family: 微软雅黑;"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <dl>
                <dd>
                    <div class="s11">
                        <asp:TextBox ID="TextBox1" runat="server" style="display: none;" class="txt" MaxLength="16" placeholder="密码" ></asp:TextBox>
                        <asp:TextBox runat="server"  ID="txtPwd" class="txt" MaxLength="16" placeholder="密码"></asp:TextBox>
                        <%--<input type="password" runat="server" id="txtPwd" class="txt" style="width: 140px;
                            display: none" />--%>
                        <span id="Span2" class="errorMsg"></span>
                    </div>
                </dd>
                <dd>
                    <div class="s22">
                        <asp:TextBox ID="txtRePwd" runat="server" class="txt" AccessKey MaxLength="16" placeholder="确认密码"></asp:TextBox>
                        <input type="password" runat="server" id="TextBox2" class="txt" onpaste="return false;"
                            style="width: 222px; display: none" />&nbsp;<span id="Span3" class="errorMsg"></span>
                    </div>
                </dd>
                <input id="Hidden3" type="hidden" />
                <dd>
                    <div class="load">
                        <img src="../Themes/Images/Login/loading.gif" /></div>
                </dd>
            </dl>
            <div class="s88">
                <asp:Button ID="Okbut" runat="server" CssClass="sign" OnClick="lbtnSubmit_Click"
                    Text="确定" />
                <%--<input id="Okbut" type="button" class="sign" value="确定" />--%>
            </div>
        </div>
        <div class="copyright">
            <p id="P1">
                <!--© 2015 BOYE CAR CLUB-->
            </p>
            <p>
                <a class="sewa" href="http://www.sewa-power.com" target="_blank"></a>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
