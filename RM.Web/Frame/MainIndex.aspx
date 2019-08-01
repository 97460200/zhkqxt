<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainIndex.aspx.cs" Inherits="RM.Web.Frame.MainIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>智订云酒店营销系统</title>
    <link href="/Themes/Styles/accordion.css" rel="stylesheet" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js"></script>
    <link href="/Themes/Scripts/artDialog/skins/blue.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js"></script>
    <script src="MainIndex.js"></script>
    <link href="/zidinn/images/logoico.png" rel="icon" />
    <script type="text/javascript">
        /**初始化**/
        $(document).ready(function () {
            //document.onselectstart = function () { return false; }
            $(document).bind("contextmenu", function () {
                return false;
            });
            GetMenu();
            Loading(true);
            iframeresize();
            AddTabMenu('ContentPannel', '/Frame/HomeIndex.aspx', '系统首页', '', 'false');
        });


        //菜单
        var V_JSON = "";
        function GetMenu() {
            var parm = 'action=Menu';
            $("#htmlMenuPanel").empty();
            getAjax('UrlLogin.ashx', parm, function (rs) {
                try {
                    V_JSON = rs;
                    var json = eval("(" + V_JSON + ")");
                    $("#TopMenu").empty();
                    var tmHtml = "<ul>";
                    for (var i = 0; i < json.MENU.length; i++) {
                        var menu = json.MENU[i];
                        if (menu.PARENTID == 0) {
                            tmHtml += "<li><a id='topMenu" + menu.MENU_ID + "' mid='" + menu.MENU_ID + "' class='li" + menu.SORTCODE + "' title='" + menu.MENU_NAME + "'  onclick=\"GetSeedMenu('" + menu.MENU_ID + "')\"></a></li>";
                        }
                    }

                    tmHtml += "</ul>";
                    $("#TopMenu").html(tmHtml);
                    GetSeedMenu($("#TopMenu").find(".li1").attr("mid"));
                } catch (e) {
                }
            });
        }

        //子菜单
        function GetSeedMenu(menu_id) {
            var topli = $("#topMenu" + menu_id);
            // $("#MenuTitle").html($(topli).attr("title")).parent().attr("class", $(topli).attr("class"));
            $(topli).parent().siblings().find("a").removeClass("this");
            $(topli).addClass("this");
            $("#htmlMenuPanel").empty();
            var json = eval("(" + V_JSON + ")");
            var menuHtml = "<div class='menu'>";
            var j = 0;
            for (var i = 0; i < json.MENU.length; i++) {
                var menu = json.MENU[i];
                if (menu.PARENTID == menu_id) {
                    if (menu.NAVIGATEURL.length > 10) {
                        menuHtml += "<ul id='" + menu.MENU_ID + "'>";
                        menuHtml += "<li ><a class=" + menu.CLASS + "  id='mla" + menu.MENU_ID + "' mid='" + menu.MENU_ID + "' pid='" + menu_id + "' mn='" + menu.MENU_NAME + "' url='" + menu.NAVIGATEURL + "' onclick=\"atm(this,'" + menu.CLASS + "')\">" + menu.MENU_NAME + "</a></li>";
                        menuHtml += "</ul>";
                    } else {
                        menuHtml += "<ul id='" + menu.MENU_ID + "'>";
                        var m3 = GetSeedMenu2(menu_id, menu.MENU_ID);
                        //var css = (m3 != "") ? (j == 0) ? "class='active1'" : "class='active'" : "";

                        var css = (m3 != "") ? (j == 0) ? " active" : " active" : "";
                        menuHtml += "<li ><a \class='" + menu.CLASS + css + "'\   onclick=\"MenuList(this,'" + menu.CLASS + "')\" >" + menu.MENU_NAME + "</a>";
                        menuHtml += m3;
                        menuHtml += "</li></ul>";
                    }
                    j++;
                }
            }
            menuHtml += "</div>";
            $("#htmlMenuPanel").append(menuHtml).find(".active1").next().show();
        }



        //子菜单二
        function GetSeedMenu2(pid, menu_id) {
            var json = eval("(" + V_JSON + ")");
            var mliHtml = "";
            for (var i = 0; i < json.MENU.length; i++) {
                var menu = json.MENU[i];
                if (menu.PARENTID == menu_id) {
                    mliHtml += "<li><a class=" + menu.CLASS + " id='mla" + menu.MENU_ID + "' mid='" + menu.MENU_ID + "' pid='" + pid + "' mn='" + menu.MENU_NAME + "' url='" + menu.NAVIGATEURL + "'  onclick=\"atm(this,'" + menu.CLASS + "')\">" + menu.MENU_NAME + "</a></li>";
                }
            }
            if (mliHtml != "") {
                mliHtml = "<ul style='display: none;'>" + mliHtml + "</ul>";
            }
            return mliHtml;
        }

        function MainHome(mn) {
            $("#TopMenu .li1").click();
            $("a[mn='" + mn + "']").click();
        }

        function atm(t, o) {
            AddTabMenu($(t).attr("mid"), $(t).attr("url"), $(t).attr("mn"), $(t).attr("pid"), 'true');
            //            $(t).parent().parent().find("a.this").attr("class", o);
            //            $(t).attr("class", "this " + o);
            var tabid = $(t).attr("mid");
            if (tabid.length == 36) {
                var mla = $("#mla" + tabid);
                if (mla.length > 0) {
                    if (mla.is(':hidden')) {
                        $(mla).parent().parent().parent().find(">a").click();
                    }
                    $(mla).parents("div:eq(0)").find("a.this").attr("class", "");
                    mla.attr("class", "this " + o);
                }
            }
        }

        function MenuList(event, o) {
            var oo = o;
            var lia = $(event);

            //            $(".menu > ul > li").each(function () {
            //                //                if ($(this).find("ul").length > 0) {
            //                //                    $(this).find(">a").not(lia).attr(oo + " active1");
            //                //                } else {
            //                //                    $(this).find("a").removeClass("active");
            //                //                }
            //            });

            //            lia.parent().parent().find("ul").hide();
            //            lia.parents("div").find(".active").next().hide();
            // $(".menu > ul > li > ul").hide();
            if (lia.attr("class") == oo + " active") {
                lia.attr("class", oo + " active1");
                lia.next().show();
            }
            else {
                lia.attr("class", oo + " active");
                lia.next().hide();
            }
        }
        function editInfo() {
            var url = "/RMBase/SysUserAdmin/UserInfo_Edit.aspx";
            top.openDialog(url, 'UserInfo_Edit', '修改用户信息', 550, 270, 50, 50);
        }

        function GetHelp() {
            var url = "/RMBase/SysHelp/Helperindex.aspx";
            //top.openDialog(url, 'UserInfo_Form', '帮助中心', 950, 500, 50, 50);
            window.open(url);
        }
    </script>
</head>
<body>
    <form method="post" action="MainDefault.aspx" id="form1">
    <div id="Container">
        <div id="Header">
            <div style="margin-top: 23px; margin-left: 10px" id="HeaderLogo" onclick="AddTabMenu('ContentPannel', '/Frame/HomeIndex.aspx', '系统首页', '', 'false');">
            </div>
            <div id="TopMenu" class="topMenu">
                <ul>
                    <li><a class="li1" title="办公"></a></li>
                </ul>
            </div>
            <div id="Headermenu">
                <div class="top">
                    <div class="right_title">
                        <ul>
                            <li ><a class="li3" onclick="GetHelp();">帮助</a></li>
                            <li style="background: none;"><a class="li4" onclick="IndexOuts()">退出</a></li>
                        </ul>
                    </div>
                    <div class="right_title" style="display: none;">
                        <ul>
                            <li style="background: none;"><a class="li5" onclick="editInfo()">欢迎您，<span runat="server"
                                id="spTopUserName"></span></a></li>
                        </ul>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="hotelName" runat="server" id="hotelName">
                智订云酒店营销系统
            </div>
        </div>
        <div id="Headerbotton">
            <div id="dww-menu" class="mod-tab">
                <div class="mod-hd" style="float: left">
                    <ul class="tab-nav" id="tabs_container">
                    </ul>
                </div>
            </div>
        </div>
        <div id="MainContent">
            <div class="navigation" id="navigation" style="padding-top: 0px;">
                <div class="box-title" style="font-weight: bold; background: #eee;">
                    <h2 class="li2" onclick="editInfo()">
                        <img src="../Themes/Images/people.png" width="22">
                        <span id="MenuTitle" runat="server"></span>
                    </h2>
                </div>
                <div class="leftbg">
                    <img src="/Themes/Images/leftbg.png" /></div>
                <div id="htmlMenuSelect" runat="Server" class="navSelect topline">
                </div>
                <div id="htmlMenuPanel" runat="Server" class="navPanelMini">
                    <div class="menu">
                        <%--人事管理--%>
                        <ul>
                            <li><a onclick="MenuList(this)" class="active1">人事信息</a>
                                <ul style="display: none;">
                                    <li><a onclick="AddTabMenu('rzgl', '/RMBase/SysPersonnel/PersonnemIndex.aspx?src=PersonnelList2', '入职管理', '', 'true');"
                                        class="this">入职管理</a></li>
                                    <li><a onclick="AddTabMenu('rygl', '/RMBase/SysUser/UserInfo_Form.aspx?key=1E0CA5B1252F1F6B1E0AC91BE7E7219E', '人员管理', '', 'true');">
                                        人员管理</a></li>
                                </ul>
                            </li>
                            <li id="rsbd"><a onclick="MenuList(this)" class="active">人事变动</a>
                                <ul style="display: none;">
                                    <li><a onclick="AddTabMenu('lzgl', '/RMBase/SysUser/UserInfo_List.aspx', '离职管理', '', 'true');">
                                        离职管理</a></li>
                                </ul>
                            </li>
                            <li id="Li1"><a onclick="MenuList(this)" class="active">人事变动</a>
                                <ul style="display: none;">
                                    <li><a onclick="AddTabMenu('lzgl', '/RMBase/SysMenu/Menu_List.aspx', '离职管理', '', 'true');">
                                        离职管理</a></li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <div id="ContentPannel">
            </div>
        </div>
    </div>
    <!--载进度条start-->
    <div id="loading" onclick="Loading(false);">
        <img src="../Themes/Images/loading.gif" style="padding-bottom: 2px; vertical-align: middle;" />&nbsp;正在处理，请稍待...&nbsp;
    </div>
    <div id="Toploading">
    </div>
    <!--载进度条end-->
    </form>
</body>
</html>
