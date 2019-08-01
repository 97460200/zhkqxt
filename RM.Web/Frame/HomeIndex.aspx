<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeIndex.aspx.cs" Inherits="RM.Web.Frame.HomeIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统首页</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            if ($("#divAffair").html().trim() == "") {
                $("#divAffair").hide();
            }
        });


        /**修改用户信息**/
        function editInfo() {
            var url = "/RMBase/SysUserAdmin/UserInfo_Edit.aspx";
            top.openDialog(url, 'UserInfo_Edit', '修改用户信息', 550, 270, 50, 50);
        }
        /**修改密码**/
        function editpwd() {
            var url = "/RMBase/SysUserAdmin/UpdateUserPwd.aspx";
            top.openDialog(url, 'UpdateUserPwd', '修改登录密码', 400, 270, 50, 50);
        }
        //新增快捷功能
        function add_HomeShortcut() {
            var url = "/RMBase/SysPersonal/HomeShortcut_Form.aspx";
            top.openDialog(url, 'Menu_Form', '首页快捷功能信息 - 添加', 450, 300, 50, 50);
        }
        //快捷功能点击事件
        function ClickShortcut(url, title, Target) {
            top.NavMenu(url, title);
        }
        function MainHome(mn) {
            window.top.MainHome(mn);
        }
    </script>
    <style type="text/css">
        .shortcuticons
        {
            float: left;
            border: solid 1px #fff;
            width: 62px;
            height: 55px;
            margin: 5px;
            padding: 5px;
            cursor: pointer;
            vertical-align: middle;
            text-align: center;
            word-break: keep-all;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }
        .shortcuticons:hover
        {
            color: #FFF;
            border: solid 1px #3399dd;
            background: #2288cc;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#33bbee', endColorstr='#2288cc');
            background: linear-gradient(top, #33bbee, #2288cc);
            background: -moz-linear-gradient(top, #33bbee, #2288cc);
            background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#33bbee), to(#2288cc));
            text-shadow: -1px -1px 1px #1c6a9e;
            width: 62px;
            height: 55px;
            font-weight: bold;
        }
        #Content
        {
            margin: 0px auto;
            height: 100%;
            overflow: hidden;
            background: url(/Themes/Images/bodybg.jpg) no-repeat right bottom;
        }
        .weather
        {
            padding-top: 0px;
        }
        .weather .cross-small dd .wea-more
        {
            display: none;
        }
        .weather .cross-small dd .item a:hover
        {
            color: #29689A !important;
            text-decoration: none !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle btnbartitlenr">
        <div>
            当前位置：<a>系统首页</a>
        </div>
    </div>
    <div class="div-body xthome">
        <div class="home_left">
            <div class="top boxshadow mode">
                <dl>
                    <dt style="display: none;"><a class="baoa">
                        <img src="../../Themes/Images/people.png" id="glimg">
                    </a></dt>
                    <dd>
                        Welcome &nbsp;&nbsp;<span id="Title" runat="server" class="xg"></span></dd>
                    <dd id="name" class="name">
                        <a class="baoa" onclick="editInfo()"><span runat="server" id="spTopUserName" style="margin-right: 0px;">
                            指挥处 - admin</span> </a>
                    </dd>
                    <dd class="last">
                        <span id="Email" runat="server" class="xg" style="display: ">woque@163.com</span>&nbsp;&nbsp;<span
                            id="Theme" runat="server" style="display: " class="xg"></span>&nbsp;&nbsp;<span class="xg"
                                style="display: ;">[<a onclick="editInfo()">个人信息</a>]</span><span class="xg">[<a
                                    onclick="editpwd()">修改密码</a>]</span></dd>
                    <asp:LinkButton ID="lbClearApplication" runat="server" OnClick="lbClearApplication_Click">.</asp:LinkButton>
                </dl>
            </div>
            <div id="divAffair" class="affair mode">
            </div>
        </div>
        <div class="home_right">
            <div class="weather">
                <h2>
                    <img class="icon" src="../Themes/Images/weather_icon.png" />天气情况
                </h2>
                <iframe allowtransparency="true" frameborder="0" width="290" height="96" scrolling="no"
                    src="http://tianqi.2345.com/plugin/widget/index.htm?s=1&z=1&t=0&v=0&d=2&bd=0&k=&f=&q=1&e=1&a=1&c=54511&w=290&h=96&align=center">
                </iframe>
            </div>
        </div>
        <div class="notice">
        </div>
    </div>
    </form>
</body>
</html>
