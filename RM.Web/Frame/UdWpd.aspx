<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UdWpd.aspx.cs" Inherits="RM.Web.Frame.UdWpd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>找回密码 - 智订云酒店营销系统</title>
    <link href="/Themes/Styles/login.css" rel="stylesheet" type="text/css" />
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <link href="/Themes/Images/Login/logo.png" rel="icon" />
    <script src="../App_Themes/default/js/SolvePlaceholder.js" type="text/javascript"></script>
    <style>
 
    </style>
    <script type="text/javascript">

        $(function () {

            $("#next_Submit").click(function () {
                if ($("#regCodeBtn").val() == "" || ($("#regCodeBtn").val() != $("#hidcod").val())) {
                    alert("请输入正确的验证码");
                    return;
                }
                window.location.href = "UdWpd2.aspx?txtPhone=" + $("#phone").val();
            });

        })

        function GetRandomNum(Min, Max) {
            var Range = Max - Min;
            var Rand = Math.random();
            return (Min + Math.round(Rand * Range));
        }

        function getCode(o) {
            if ($("#phone").val() != "") {
                var re = /^1\d{10}$/;
                if (re.test($("#phone").val())) {
                    time1(o);
                } else {
                    alert("手机号格式错误！");
                }

            }
            else {
                alert('请输入手机号码');
            }
        }
        var wait1 = 60;
        var num1 = 0;
        function time1(o) {
            if (wait1 == 60) {
                num1++;
                var mob = document.getElementById('phone').value;
                var cpd = GetRandomNum(1000, 10000);

                //发送短信
                var parm = 'action=updatepwd&cpd=' + cpd + '&phone=' + mob;
                $.ajax({
                    url: "/members/registered.ashx",
                    data: {
                        action: "updatepwdss",
                        cpd: cpd,
                        phone: $("#phone").val(),
                        type: "updatepwd",
                        AdminHotelid: "1"
                    },
                    type: "GET",
                    dataType: "JSON",
                    success: function (rs) {
                        if (parseInt(rs) == 1) {
                            alert("发送成功");
                            $("#SetPhone").text("发送成功");
                            $("#SetPhone").show();
                            $("#hidcod").val(cpd);
                            $("#hidPhone").val(mob);
                            document.getElementById("hfyzm").value = rs;

                        } else if (parseInt(rs) == 2) {
                            alert("发送失败");
                            $("#SetPhone").text("发送失败");
                            $("#SetPhone").show();
                        } else if (parseInt(rs) == 3) {
                            alert("手机号" + $("#phone").val()+"不存在");
                            $("#SetPhone").text("发送失败");
                            $("#SetPhone").show();
                        }
                    }
                });



            }
            if (wait1 == 0) {
                o.style.color = "#fff";
                o.removeAttribute("disabled");
                o.style.backgroundColor = "#CC0000";
                o.value = "获取验证码";
                wait1 = 60;
            } else {
                o.style.color = "#999";
                o.setAttribute("disabled", true);
                o.style.backgroundColor = "#eaeaea";
                o.value = "重新发送(" + wait1 + ")";
                wait1--;
                setTimeout(function () {
                    time1(o)
                }, 1000);
            }
            setTimeout(function () {
                $("#SetPhone").hide();
            }, 3000);
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="hidcod" />
    <div class="login">
        <!---找回密码--->
        <div class="boxLogin boxpassword">
            <div class="login_head">
                <table>
                    <tr>
                        <td>
                            <img src="/Themes/Images/Login/adminL.png" />
                        </td>
                        <td class="nn">
                            <span id="ds" style="font-size: 16px; font-family: 微软雅黑;"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <dl>
                <dd>
                    <div class="s1  user_name">
                        <input type="text" id="phone" placeholder="手机号" class="txt" />
                        <input type="button" name="name" value="获取验证码" class="btn sign" onclick="getCode(this);"
                            style="width: 75px" />
                        <span id="errorMsg0" class="errorMsg"></span>
                    </div>
                </dd>
                <dd>
                    <div class="s2 pwd">
                        <input type="text" onpaste="return false;" id="regCodeBtn" placeholder="请输入验证码" class="txt"
                            onpaste="return false;" />&nbsp;<span id="errorMsg1" class="errorMsg"></span>
                    </div>
                </dd>
                
                <input id="File1" type="hidden" />
                <dd>
                    <div class="load">
                        <img src="../Themes/Images/Login/loading.gif" /></div>
                </dd>
            </dl>
            <div class="s8">
                <input id="next_Submit" type="button" class="sign" value="下一步" />
            </div>
            <p>
                温馨提示：如果忘记手机号码，请联系客服人员</p>
        </div>
        <div class="copyright">
            <p id="cp">
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
