<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RM.Web.Customized.MeiCheng.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>登录 - 美橙酒店营销系统</title>
    <meta name="viewport" content="width=device-width, user-scalable=no" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <link href="/zidinn/images/logoico.png" rel="icon">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        if (window.location.href.indexOf("localhost:") <= 0) { //判断是否是本地运行
            var targetProtocol = "https:";
            if (window.location.protocol != targetProtocol)
                window.location.href = targetProtocol + window.location.href.substring(window.location.protocol.length);
        }
        
    </script>
    <script type="text/javascript">
        //回车键
        document.onkeydown = function (e) {
            if (!e) e = window.event; //火狐中是 window.event
            if ((e.keyCode || e.which) == 13) {
                if ($("#mmdl_login").is(':visible')) {
                    mmdl_login();
                } else {
                    dxdl_login();
                }
            }
        }

        $(function () {
            $("#dLoginType").on("click", "div", function () {
                $(this).addClass("act").siblings().removeClass("act");
                $(".newloginc-center .biaodan").hide().eq($(this).index()).show();
            });
            $("#dRememberPwd").click(function () {
                $(this).attr("class", ($(this).attr("class") == "mdl act") ? "mdl" : "mdl act");
            });
            rememberPwd();
        });


        function rememberPwd() {
            var parm = "action=jizhumima";
            getAjax('/Frame/Frame.ashx', parm, function (rs) {
                if (rs != "0") {
                    var strs = new Array;
                    strs = rs.split("&");
                    $("#mmdl_Name").val(strs[0]);
                    $("#mmdl_Pwd").val(strs[1]);
                    $("#dRememberPwd").attr("class", "mdl act");
                } else {
                    $("#dRememberPwd").attr("class", "mdl");
                }
            });
        }

        function mmdl_login() {
            var name = $("#mmdl_Name").val();
            var pwd = $("#mmdl_Pwd").val();
            if (name == "") {
                $("#mmdl_Name").focus();
                return false;
            } else if (pwd == "") {
                $("#mmdl_Pwd").focus();
                return false;
            }
            var jizhu = "";
            if ($("#dRememberPwd").attr("class") == "mdl act") {
                jizhu = "1";
            } else {
                jizhu = "0";
            }
            var parm = 'action=login&user_Account=' + escape(name) + '&userPwd=' + escape(pwd) + '&jizhu=' + jizhu;
            getAjax('/Frame/Frame.ashx', parm, function (rs) {
                if (parseInt(rs) == 2) {
                    $("#txtUserName").focus();
                    //$("#errorMsg0").html("账户被锁,联系管理员");
                    //CheckingLogin(0);
                    return false;
                } else if (parseInt(rs) == 4) {
                    alert('账户或密码有错误！');
                    $("#mmdl_Name").focus();
                    //$("#errorMsg0").html("账户或密码有错误");
                    //CheckingLogin(0);
                    return false;
                } else if (parseInt(rs) == 6) {
                    $("#txtUserName").focus();
                    //$("#errorMsg0").html("该用户已经登录");
                    //CheckingLogin(0);
                    return false;
                } else if (parseInt(rs) == 3) {
                    window.location.href = 'MainDefault.aspx';
                } else {
                    //CheckingLogin(0);
                    alert('服务器连接不上,联系管理员！');
                    //window.location.href = window.location.href.replace('#', '');
                    return false;
                }
            });
        }
    </script>
    <script type="text/javascript">
        function sms_code() {
            var phone = $("#dxdl_Phone").val();
            if (phone == "") {
                $("#dxdl_Phone").focus();
                return false;
            }
            var parm = 'action=sms_code&num=1&phone=' + phone;
            getAjax('/Frame/Frame.ashx', parm, function (rs) {
                if (rs == "1") {
                    $("#getCode").hide();
                    $("#syCode").show();
                    code_time();
                } else {
                    alert('短信发送失败！');
                    return false;
                }
            });
        }

        var time_num = 60;
        function code_time() {
            $("#syCode").val("剩余" + time_num + "s");
            if (time_num > 0) {
                setTimeout(function () {
                    code_time();
                }, 1000);
                time_num--;
            } else {
                time_num = 60;
                $("#getCode").show();
                $("#syCode").hide();
            }
        }

        function dxdl_login() {
            var phone = $("#dxdl_Phone").val();
            var code = $("#dxdl_Code").val();
            if (phone == "") {
                $("#dxdl_Phone").focus();
                return false;
            } else if (code == "") {
                $("#dxdl_Code").focus();
                return false;
            }
            var parm = 'action=code_login&phone=' + phone + '&code=' + code;
            getAjax('/Frame/Frame.ashx', parm, function (rs) {
                if (parseInt(rs) == 1) {
                    window.location.href = 'MainDefault.aspx';
                } else if (parseInt(rs) == 10011) {
                    alert('验证码错误或超时！');
                    $("#dxdl_Code").focus();
                    return false;
                } else if (parseInt(rs) == 10012) {
                    alert('手机号不存在！！');
                    $("#dxdl_Phone").focus();
                    return false;
                } else {
                    alert('服务器连接不上,联系管理员！');
                    return false;
                }
            });
        }

        function retrieve_pwd() {
            window.location.href = '/Frame/RetrievePwd.aspx';
        }
    </script>
</head>
<body class="newlogin">
    <form id="form1" runat="server">
    <div class="newloginc">
        <div class="newloginc-top">
            <p class="p1">
                <a href="/" target="_blank">
                    <img src="/SysSetBase/img/mc.jpg" style="width:90px;"/></a>
            </p>
            <p class="p2">
                降低获单成本，提高服务品质
            </p>
        </div>
        <div class="newloginc-center">
            <div id="dLoginType" class="navtab clearfix">
                <div class="tab act">
                    密码登录
                </div>
                <div class="tab">
                    短信登录
                </div>
            </div>
            <div class="biaodan">
                <div class="phone">
                    <input id="mmdl_Name" type="text" value="" placeholder="手机/用户名" autocomplete="off" />
                </div>
                <div class="password">
                    <input id="mmdl_Pwd" type="password" value="" placeholder="密码" autocomplete="off" />
                </div>
                <a id="mmdl_login" onclick="mmdl_login()" class="login">登 录 </a>
                <div class="caozuo">
                    <div id="dRememberPwd" class="mdl act">
                        记住密码
                    </div>
                    <div onclick="retrieve_pwd()" class="wjmm">
                        忘记密码？
                    </div>
                </div>
            </div>
            <div class="biaodan dxdl" style="display: none;">
                <div class="phone">
                    <input id="dxdl_Phone" type="text" value="" placeholder="手机号码" autocomplete="off" />
                </div>
                <div class="password">
                    <input id="dxdl_Code" type="text" value="" placeholder="验证码" autocomplete="off" />
                    <input id="getCode" type="button" value="获取验证码" class="hqyzm" onclick="sms_code()" />
                    <input id="syCode" type="button" value="剩余60S" class="hqyzm hqyzmds" style="display: none;" />
                </div>
                <a onclick="dxdl_login()" class="login">登 录 </a>
                <div class="caozuo">
                    <div onclick="retrieve_pwd()" class="wjmm">
                        忘记密码？
                    </div>
                </div>
            </div>
        </div>
        <div class="newloginc-bottom">
            <div class="ewm clearfix" style="display:none;">
                <div class="ewml">
                    <img src="/SysSetBase/img/zdyshpt.png" />
                    <p>
                        美橙商户平台
                    </p>
                </div>
                <div class="ewml">
                    <img src="/SysSetBase/img/zdygzh.png" />
                    <p>
                        美橙公众号
                    </p>
                </div>
            </div>
            <div class="bah" style="display:none;">
                © 2003 - 2018 SEWA&nbsp;&nbsp;&nbsp; <a target="_blank" href="http://www.miitbeian.gov.cn/">
                    粤ICP备12091896号</a>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
