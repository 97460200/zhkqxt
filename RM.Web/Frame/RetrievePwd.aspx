<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RetrievePwd.aspx.cs" Inherits="RM.Web.Frame.RetrievePwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>登录 - 智订云酒店营销系统</title>
    <meta name="viewport" content="width=device-width, user-scalable=no" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        function sms_code() {
            var phone = $("#txtPhone").val();
            if (phone == "") {
                $("#txtPhone").focus();
                return false;
            }
            var parm = 'action=sms_code&num=5&phone=' + phone;
            getAjax('Frame.ashx', parm, function (rs) {
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

        function retrieve_pwd() {
            var phone = $("#txtPhone").val();
            var code = $("#txtCode").val();
            var pwd = $("#new_pwd").val();
            var pwds = $("#con_pwd").val();
            if (phone == "") {
                $("#txtPhone").focus();
                return false;
            } else if (code == "") {
                $("#txtCode").focus();
                return false;
            }
            if (pwd.length < 6) {
                alert('密码长度不能小于6位！');
                $("#new_pwd").focus();
                return false;
            }
            if (pwd != pwds) {
                alert('二次密码输入不一致！');
                return false;
            }
            var parm = 'action=retrieve_pwd&phone=' + phone + '&code=' + code + '&pwd=' + pwd;
            getAjax('Frame.ashx', parm, function (rs) {
                if (parseInt(rs) == 1) {
                    window.location.href = 'login.aspx';
                } else if (parseInt(rs) == 10011) {
                    alert('验证码错误或超时！');
                    $("#txtCode").focus();
                    return false;
                } else if (parseInt(rs) == 10012) {
                    alert('手机号不存在！！');
                    $("#txtPhone").focus();
                    return false;
                } else {
                    alert('服务器连接不上,联系管理员！');
                    return false;
                }
            });
        }
    </script>
</head>
<body class="newlogin">
    <form id="form1" runat="server">
    <div class="newloginc retrievepwd">
        <div class="newloginc-top">
            <p class="p1">
                <img src="/SysSetBase/img/zdy.png" />
            </p>
            <p class="p2">
                降低获单成本，提高服务品质
            </p>
        </div>
        <div class="newloginc-center czmm">
            <div class="navtab clearfix">
                <div class="d1">
                    重置密码
                </div>
                <a href="login.aspx">返回 </a>
            </div>
            <div class="biaodan ">
                <div class="d1 clearfix">
                    <label>
                        手机号</label>
                    <input type="text" id="txtPhone" value="" autocomplete="off" />
                </div>
                <div class="d1 yzm clearfix">
                    <label>
                        验证码</label>
                    <input type="text" id="txtCode" value="" class="i1" autocomplete="off" />
                    <input id="getCode" type="button" value="获取验证码" class="i2" onclick="sms_code()" />
                    <input id="syCode" type="button" value="剩余60S" class="i2 i3" style="display: none;" />
                </div>
                <div class="d1 clearfix">
                    <label>
                        新密码</label>
                    <input type="password" id="new_pwd" value="" autocomplete="off" />
                </div>
                <div class="d1 clearfix">
                    <label>
                        确认密码</label>
                    <input type="password" id="con_pwd" value="" autocomplete="off" />
                </div>
                <a onclick="retrieve_pwd()" class="login">确 定 </a>
            </div>
        </div>
        <div class="newloginc-bottom">
            <div class="ewm clearfix">
                <div class="ewml">
                    <img src="/SysSetBase/img/zdyshpt.png" />
                    <p>
                        智订云商户平台
                    </p>
                </div>
                <div class="ewml">
                    <img src="/SysSetBase/img/zdygzh.png" />
                    <p>
                        智订云公众号
                    </p>
                </div>
            </div>
            <div class="bah">
                © 2003 - 2018 SEWA&nbsp;&nbsp;&nbsp;粤ICP备44036001号
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $("input").on("input", function () {
            var txtPhone = $("#txtPhone").val();
            var txtCode = $("#txtCode").val();
            var new_pwd = $("#new_pwd").val();
            var con_pwd = $("#con_pwd").val();
            if (txtPhone != '' && txtCode != '' && new_pwd != '' && con_pwd != '') {
                $(".login").addClass("act");
            } else {
                $(".login").removeClass("act");
            }
        });

        $("input").on("input", function () {
            var txtCode = $("#txtCode").val();
            if (txtCode != '') {
                $("#getCode").addClass("act");
            } else {
                $("#getCode").removeClass("act");
            }
        });
    </script>
</body>
</html>
