<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="member.aspx.cs" Inherits="RM.Web.SysSetBase.member.member" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>会员设置</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <style type="text/css">
        .memr .memrList li th:nth-child(1)
        {
            width:40px;
            }
        .memr .memrList li th:nth-child(2)
        {
            width:80px;
            }
        .memr .memrList li th
        {
            width:120px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="tools_bar btnbartitle btnbartitlenr" style="display:block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt; </span><span>系统设置</span> &gt; <span>会员设置</span>
        </div>
    </div>
    <div class="memr">
        <div class="mrNav">
            <%--<a>会员等级设置</a><a  onclick="bind()" >会员权益设置</a>--%><a class="active" onclick="binds()">会员升级设置</a><a  href="../../RMBase/SysEMS/MessageSendBirthday.aspx">会员生日祝福设置</a>
        </div>
        <ul class="memrList">
        <li>
                <div class="upgrdtable" style="width: 960px;" >
                    <table class="ul" id="tab2" style="width:auto;float:left;">
                        
                        
                    </table>
                    <div class="radio clearfix" id="gnlx" style="padding-top:236px;">
                        <label class="checked">原价购买</label>
                        <label>差价购买</label>
                    </div>
                    <div class="zs">如最低级别会员设置了购买价格，则用户需支付相应金额才可完成注册</div>
                    <div id="sm">
                    <div class="sjxs clearfix" style=" display:">
                        <span>微会员购买升级形式/说明</span><input type="text" name="name" value="" />
                    </div>
                    </div>
                    <div class="sjxs clearfix" style=" display:none">
                        <span>金卡会员购买升级形式/说明</span><input type="text" name="name" value="" />
                    </div>
                    <div class="sjxs clearfix" style=" display:none">
                        <span>砖石会员购买升级形式/说明</span><input type="text" name="name" value="" />
                    </div>
                    <div class="membtn">
                        <a class="button buttonActive"  onclick="Submits()">保存</a>
                    </div>
                </div>
                
            </li>
        <li style="padding-left:0; display:none" >
            <%--<iframe id="Member3" width="100%" frameborder="0"
                                        marginheight="0" marginwidth="0" scrolling="no" runat="server" src="../../RMBase/SysParameter/Member3.aspx?HoleID=" style="height: calc(100% - 45px);"></iframe>--%>
        </li>
            
            <li style=" display:none">
                <div class="qytable" style="width: 600px;">
                    <table class="ul" id="tab1" >
                        
                    </table>
                    <div class="membtn">
                        <a class="button buttonActive" onclick="Submit()">保存</a>
                    </div>
                </div>
            </li>
            
        </ul>
    </div>
    <input type="text" id="AdminHotelid" runat="server"  style="display:none;"   />
    <input type="text" id="hgnlx" value="1" runat="server" style="display:none;"/>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script>
        bind(); binds();bindss();
        function bind() {
            $("#tab1").html("");
            var url = "member.ashx?action=gethtml";
            $.post(url, function (data) {
                $("#tab1").html(data);
                gets();
            });
        }

        function binds() {
            $("#tab2").html("");
            var url = "member.ashx?action=gethtmls";
            $.post(url, function (data) {
                $("#tab2").html(data);
                get();
            });
        }

        function bindss() {
            $("#sm").html("");
            var url = "member.ashx?action=gethtmlss";
            $.post(url, function (data) {
                $("#sm").html(data);
                getss();
            });
        }

        function Submit() {
            var qy = new Array();
            var i = 0;
            var jfbs = 0; var rzmyj = 0; var dhyjblsj = 0;
            var wwyjblsj = 0; var zfyjblsj = 0; var ystfsj = 0;
            $(".hylxname").each(function () {
                var jb = $(this).attr("jb");
                qy[i] = jb;

                $(".jfbs").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? jfbs = 1 : jfbs = 0; });
                $(".rzmyj").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + $("input[name='w" + jb + "']:checked").val(); $(this).parent().parent().hasClass('active') ? rzmyj = 1 : rzmyj = 0; });
                $(".dhyjblsj").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + $(this).val(); $(this).parent().parent().hasClass('active') ? dhyjblsj = 1 : dhyjblsj = 0; });
                $(".wwyjblsj").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + $(this).val(); $(this).parent().parent().hasClass('active') ? wwyjblsj = 1 : wwyjblsj = 0; });
                $(".zfyjblsj").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + $(this).val(); $(this).parent().parent().hasClass('active') ? zfyjblsj = 1 : zfyjblsj = 0; });
                $(".ystfsj").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + $(this).val(); $(this).parent().parent().hasClass('active') ? ystfsj = 1 : ystfsj = 0; });

                i++;

            });

            var url = "member.ashx?action=update"; var values = "";
            for (var j = 0; j < qy.length; j++) { values += qy[j] + "|"; }
            values = values.substring(0, values.length - 1);
            url += "&values=" + values;
            url += "&jfbs=" + jfbs;
            url += "&rzmyj=" + rzmyj;
            url += "&dhyjblsj=" + dhyjblsj;
            url += "&wwyjblsj=" + wwyjblsj;
            url += "&zfyjblsj=" + zfyjblsj;
            url += "&ystfsj=" + ystfsj;


            $.post(url, function (data) {
                showTipsMsg("保存成功！", 2000, 4);
                //alert("保存成功");
            });
        }




        function Submits() {
            var qy = new Array();
            var i = 0;
            var jfsjgy = 0; var dcxfsjgy = 0; var xfsjgy = 0;
            var czsjgy = 0; var gmsjgy = 0; var dcczsjgy = 0;
            
            $(".hylxnames").each(function () {
                var jb = $(this).attr("jb");
                qy[i] = jb;

                $(".jfsjgy").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? jfsjgy = 1 : jfsjgy = 0; });
                //$(".jfsjdy").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? jfsjdy = 1 : jfsjdy = 0; });

                $(".xfsjgy").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? xfsjgy = 1 : xfsjgy = 0; });
                $(".dcxfsjgy").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? dcxfsjgy = 1 : dcxfsjgy = 0; });
                $(".czsjgy").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? czsjgy = 1 : czsjgy = 0; });
                $(".dcczsjgy").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? dcczsjgy = 1 : dcczsjgy = 0; });
                $(".gmsjgy").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? gmsjgy = 1 : gmsjgy = 0; });
                $(".sm").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + $(this).val(); });
                i++;

            });

            var url = "member.ashx?action=updates"; var values = "";
            for (var j = 0; j < qy.length; j++) { values += qy[j] + "|"; }
            values = values.substring(0, values.length - 1);
            url += "&values=" + values;
            url += "&jfsjgy=" + jfsjgy;
            //url += "&jfsjdy=" + jfsjdy;
            url += "&dcxfsjgy=" + dcxfsjgy;
            url += "&xfsjgy=" + xfsjgy;
            url += "&czsjgy=" + czsjgy;
            url += "&dcczsjgy=" + dcczsjgy;
            url += "&gmsjgy=" + gmsjgy;
            url += "&gmlx=" + $("#hgnlx").val();

            //alert(url);
            $.post(url, function (data) {
                showTipsMsg("保存成功！", 2000, 4);

            });
        }


        function get() {
            var url = "member.ashx?action=getinfos";
            $.post(url, function (data) {
                var json = eval("(" + data + ")");


                for (var i = 0; i < json.Set_Upgrade.length; i++) {
                    $(".jfsjgy").each(function () { if ($(this).attr("jb") == json.Set_Upgrade[i].JB) $(this).val(json.Set_Upgrade[i].JFSJGY); json.Set_Upgrade[i].ISJFSJGY == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    //$(".jfsjdy").each(function () {if ($(this).attr("jb") == json.Set_Upgrade[i].JB) $(this).val(json.Set_Upgrade[i].JFSJDY); json.Set_Upgrade[i].ISJFSJDY == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".xfsjgy").each(function () { if ($(this).attr("jb") == json.Set_Upgrade[i].JB) $(this).val(json.Set_Upgrade[i].XFSJGY); json.Set_Upgrade[i].ISXFSJGY == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".dcxfsjgy").each(function () { if ($(this).attr("jb") == json.Set_Upgrade[i].JB) $(this).val(json.Set_Upgrade[i].DCXFSJGY); json.Set_Upgrade[i].ISDCXFSJGY == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".czsjgy").each(function () { if ($(this).attr("jb") == json.Set_Upgrade[i].JB) $(this).val(json.Set_Upgrade[i].CZSJGY); json.Set_Upgrade[i].ISCZSJGY == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".dcczsjgy").each(function () { if ($(this).attr("jb") == json.Set_Upgrade[i].JB) $(this).val(json.Set_Upgrade[i].DCCZSJGY); json.Set_Upgrade[i].ISDCCZSJGY == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".gmsjgy").each(function () { if ($(this).attr("jb") == json.Set_Upgrade[i].JB) $(this).val(json.Set_Upgrade[i].GMSJGY); json.Set_Upgrade[i].ISGMSJGY == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    

                }

            });

        }


        function gets() {
            var url = "member.ashx?action=getinfo";
            $.post(url, function (data) {
                var json = eval("(" + data + ")");

                for (var i = 0; i < json.Set_Privilege.length; i++) {
                    $(".jfbs").each(function () { if ($(this).attr("jb") == json.Set_Privilege[i].JB) $(this).val(json.Set_Privilege[i].JFBS); json.Set_Privilege[i].ISJFBS == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".rzmyj").each(function () { if ($(this).attr("jb") == json.Set_Privilege[i].JB) $(this).find(":radio[value='" + json.Set_Privilege[i].RZMYJ + "']"); json.Set_Privilege[i].ISRZMYJ == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".dhyjblsj").each(function () { if ($(this).attr("jb") == json.Set_Privilege[i].JB) $(this).val(json.Set_Privilege[i].DHYJBLSJ); json.Set_Privilege[i].ISDHYJBLSJ == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".wwyjblsj").each(function () { if ($(this).attr("jb") == json.Set_Privilege[i].JB) $(this).val(json.Set_Privilege[i].WWYJBLSJ); json.Set_Privilege[i].ISWWYJBLSJ == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".zfyjblsj").each(function () { if ($(this).attr("jb") == json.Set_Privilege[i].JB) $(this).val(json.Set_Privilege[i].ZFYJBLSJ); json.Set_Privilege[i].ISZFYJBLSJ == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".ystfsj").each(function () { if ($(this).attr("jb") == json.Set_Privilege[i].JB) $(this).val(json.Set_Privilege[i].YSTFSJ); json.Set_Privilege[i].ISYSTFSJ == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $("#hgmlx").val(json.Set_Privilege[i].GMLX);
                }

            });

        }


        function getss() {
            var url = "member.ashx?action=getinfos";
            $.post(url, function (data) {
                var json = eval("(" + data + ")");

                
                for (var i = 0; i < json.Set_Upgrade.length; i++) {
                   
                    $(".sm").each(function () { if ($(this).attr("jb") == json.Set_Upgrade[i].JB) $(this).val(json.Set_Upgrade[i].SM == "&nbsp;" ? "" : json.Set_Upgrade[i].SM); });

                }

            });

        }

    </script>
    <script type="text/javascript">
        var selected = function (Selector) {
            var htr = function () {
                $(this).parents('tr').hasClass('active') ? $(Selector).find('tr').removeClass('active') : $(Selector).find('tr').addClass('active');
            };
            var btr = function () {
                $(this).parents('tr').hasClass('active') ? $(this).parents('tr').removeClass('active') : $(this).parents('tr').addClass('active');
                isCheckAll() ? $(Selector).find('thead tr').removeClass('active') : $(Selector).find('thead tr').addClass('active');
            };
            var isCheckAll = function () {
                var otr = $(Selector).find('tbody tr');
                for (var i = 0; i < otr.length; i++) {
                    if (!otr.eq(i).hasClass('active')) return true;
                }
                return false;
            };
            $(Selector).on('click', 'thead .icon-radio6', htr);
            $(Selector).on('click', 'tbody .icon-radio6', btr);
        };
        selected('.qytable');
        selected('.upgrdtable');


//        $('.mrNav').on('click', 'a', function () {
//            $(this).addClass('active').siblings().removeClass('active');
//            $('.memrList li').eq($(this).index()).show().siblings().hide();
//        });

        $('#gnlx').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
            if ($.trim($(this).html()) == "原价购买") {
                $("#hgnlx").val(1);
                
            }
            if ($.trim($(this).html()) == "差价购买") {
                $("#hgnlx").val(2);
                
            }

        });


        $("#gnlx label").each(function () {
            var state = "";
            if ($("#hgnlx").val() == "1") {
                state = "原价购买";

            } else if ($("#hgnlx").val() == "2") {
                state = "差价购买";
               
            }

            if (state == $.trim($(this).html())) {
                $(this).addClass('checked').siblings().removeClass('checked');
            }
        });


    </script>
    
</body>
</html>
