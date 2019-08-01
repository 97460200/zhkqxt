<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="couponadd.aspx.cs" Inherits="RM.Web.SysSetBase.coupons.couponadd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>卡券设置</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <script src="/WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/admin/js/button.js"></script>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            hotel();

            if ($("#adminhotelid").val() != "1008337") {//八号定制需求
                $("#xlhylx").hide();
            }
        })

        function hotel() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetHotelList",
                    adminhotelid: $("#adminhotelid").val()
                },
                type: "GET",
                datatype: "JSON",
                async: false,
                success: function (data) {
                    if (data == "") {
                        return;
                    }
                    var json = eval("(" + data + ")");
                    $("#gmkfNav").empty();
                    var html = "";
                    for (var i = 0; i < json.length; i++) {
                        var copytr = "";
                        html += "<li class='down'><b class='icon-kg1' hotelid=" + json[i].ID + ">" + json[i].NAME + "</b>";
                        if (json[i].kf != undefined && json[i].kf != "") {
                            html += "<div class='sub' style='display: block;'>";
                            for (var s = 0; s < json[i].kf.length; s++) {
                                html += "<span><a  class='icon-kg2 kgClose2' GSID=" + json[i].kf[s].id + ">" + json[i].kf[s].name + "</a></span>";
                            }
                            html += "</div>";
                        }
                        html += "</li>";
                    }
                    $("#gmkfNav").append(html);
                }
            })


            //加载时在后台获取所有值再循环给样式
            var mystring = "<%=_sumhotel %>";
            var myarray = mystring.split(",");
            for (var i = 0; i < myarray.length; i++) {
                for (var k = 0; k < $(".icon-kg1").length; k++) {
                    if ($(".icon-kg1").eq(k).attr("hotelid") == myarray[i]) {

                        $(".icon-kg1").eq(k).addClass('kgClose');

                    }
                }

                for (var j = 0; j < $(".icon-kg2").length; j++) {
                    if ($(".icon-kg2").eq(j).attr("gsid") == myarray[i]) {

                        $(".icon-kg2").eq(j).addClass('kgClose2');
                    }
                    if (mystring.indexOf($(".icon-kg2").eq(j).attr("gsid")) == -1 && mystring!="") {
                        
                        $(".icon-kg2").eq(j).removeClass('kgClose2');
                    }
                }
            }
        }

    </script>
    <script type="text/javascript">

        $(function () {
            $(".icon-kg1").click(function () {
                //$(this).hasClass('kgClose') ? $(this).removeClass('kgClose') : $(this).addClass('kgClose');
                if ($(this).hasClass('kgClose') == true) {
                    //$(this).next(".sub").children("span").children("a").addClass("icon-kg2 kgClose2");
                } else if ($(this).hasClass('kgClose') == false) {
                    //$(this).next(".sub").children("span").children("a").removeClass("kgClose2");
                }
            })

            $(".icon-kg2").click(function () {
                $(this).hasClass('kgClose2') ? $(this).removeClass('kgClose2') : $(this).addClass('kgClose2');
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" id="adminhotelid" type="hidden" />
    <div class="clearfix" style="height:500px;overflow: auto;">
    <dl class="copnli copnli1" style="padding-left: 32px;padding-right: 0;">
    <dd class="inp">
            <small>卡券类型</small>
            <div>
                <asp:DropDownList ID="ddltype" runat="server" class="gets">
                </asp:DropDownList>
                
            </div>
        </dd>
        <dd class="inp">
            <small>卡券名称</small>
            <div>
                <input type="text" id="txtName" runat="server" />
            </div>
        </dd>
        <dd class="inp">
            <small>面值</small>
            <div>
                <p class="radio term" id="P1" style="display: none;">
                    <label class="checked">固定</label><label>随机</label>
                </p>
                <div>
                <input type="text" id="txtPar" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" />元
                </div>
                <div style="display: none;"> 
                    <input type="text" name="name" value="" style="width:97px;"/> - <input type="text" name="name" value="" style="width:97px;"/>元
                </div>
            </div>
        </dd>
        <dd class="inp">
            <small>满多少可用</small>
            <div>
                <input type="text" id="txtUsedMin" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" />元
            </div>
        </dd>
         
        <%--新加有效会员--%>
        <dd class="inp"  >
            <small>赠送会员</small>
            <div>
                <p class="checkbox" id="ishy"  style="display: block;">
                    <%=_yxMembergrade%>
                </p>
            </div>
        </dd>
        <dd class="inp">
                <small class="lh14" id="numsm">同一会员赠送</small>
                <div>
                    <input type="text" id="txtNum" runat="server" />张
                </div>
         </dd>
        <dd class="inp">
            <small>有效期</small>
            <div>
                <p class="radio term" id="term" style="display: block;">
                    <label class="checked">
                        永久</label><label>时长</label><label>时段</label>
                </p>
                <p>
                    领取之日起<input type="text" id="txtDay" onblur="if(this.value!='') alert('从领取当天算起'+this.value+'天有效');" runat="server" class="tian" />天
                </p>
                <p>
                    <input id="B_sj" type="text" runat="server" onfocus="WdatePicker()" class="date" />
                    ～
                    <input id="E_sj" type="text" runat="server" onfocus="WdatePicker()" class="date" />
                </p>
            </div>
        </dd>
        <%--新加有效日期--%>
        <dd class="inp"  >
            <small>有效日期</small>
            <div>
                <p class="checkbox" id="yxrq"  style="display: block;">
                    <label class="checked" val="0">周一</label>
                    <label class="checked" val="1">周二</label>
                    <label class="checked" val="2">周三</label>
                    <label class="checked" val="3">周四</label>
                    <label class="checked" val="4">周五</label>
                    <label class="checked" val="5">周六</label>
                    <label class="checked" val="6">周日</label>
                </p>
            </div>
        </dd>
        <dd>
            <small class="lh14">领取当天是否可用</small>
            <div id="Is_Day_ok" class="radio isopen">
                <label class="checked">
                    是</label><label>否</label>
            </div>
        </dd>
         <%--新加可用节假日--%>
        <dd class="inp" >
            <small>可用节假日</small>
            <div>
                <ul id="jjr" runat="server" >
                <%=_jjr%>
                    <%--<li class="clearfix">
                        <span style="float:left;">2018年</span>
                        <p class="checkbox cb2"  style="display: block;width:300px;float:left;padding-left:20px;">
                            <label class="checked">元旦</label>
                            <label>春节</label>
                            <label>清明节</label>
                            <label>劳动节</label>
                            <label>端午节</label>
                            <label>国庆节</label>
                            <label>中秋节</label>
                        </p>
                    </li>
                    <li  class="clearfix">
                        <span style="float:left;">2019年</span>
                        <p class="checkbox cb2"  style="display: block;width:300px;float:left;padding-left:20px;">
                            <label class="checked">元旦</label>
                            <label>春节</label>
                            <label>清明节</label>
                            <label>劳动节</label>
                            <label>端午节</label>
                            <label>国庆节</label>
                            <label>中秋节</label>
                        </p>
                    </li>--%>
                </ul> 
            </div>
        </dd>
        <%--新加有效会员--%>
        <dd class="inp"  id="xlhylx" runat="server" >
            <small>会员类型</small>
            <div>
                <p class="radio term" id="hytype" style="display: block;">
                    <label class="checked">
                        全部</label><label>老会员</label><label>新会员</label>
                </p>
            </div>
        </dd>

    </dl>
    <dl class="copnli copnli1">
        
        <dd class="inp">
            <small>获取方式</small>
            <div>
                <asp:DropDownList ID="DDLmode" runat="server" class="gets">
                </asp:DropDownList>
                <i class="sm" style=" color:Red;" id="hqfssm"></i>

                <!--指定赠送-->
                <p id="zdzss" runat="server" style="display: none ; padding: 4px 0;" class="radio mem">
                    <label class="checked">
                        全部会员</label><label>会员等级</label><label>个人</label>
                </p>
                <%--新指定赠送--%>
                <p id="zdzs" runat="server" style="display:;margin:10px 0 0 -58px;;">
                    赠送对象 <a href="#" class="btn" onclick="zskq()" style="margin-left:5px; display:">选择</a>
                    <span id="zsdx" style="margin:10px 0 0 58px;display:none">
                        <label id="hy">会员：全部会员</label><label id="cs">消费次数：3-6次</label><label id="jg">消费间隔：不限</label>
                    </span>
                </p>
                <!--会员等级-->
                <p id="hydj" runat="server" style="display: none;" class="checkbox">
                    <%=_Membergrade%>
                </p>
                <!--个人-->
                <p id="hykh" runat="server" style="display: none;margin-top: 10px;">
                    <i>会员手机号</i>
                    <input type="text" id="kh" runat="server" />
                    <input id="btnyes" type="button" value="搜索" class="btn"/>
                    <i class="sm">*例如：(13576457177,13576457277)添加多个中间用英文逗号分开</i>
                </p>
                <p id="hyxx" runat="server" style="display: none">
                    
                    <label id="hyxxtd">
                    </label>
                </p>
                <p class="hysjhtj" style="display:none">
                    <label>
                        <span>李静</span><span>13794897560</span>
                    </label>
                </p>
                <!-- 消费赠送-->
                <p id="xfje" runat="server" style="display: none;margin-top:10px;margin-left:-58px;">
                    消费金额
                    <input type="text" id="txtFirstMoney" runat="server" class="half"/>元以上
                    
                    <input type="text" style=" display:none;" id="txtSecondMoney" runat="server" class="half" />
                </p>
                <!-- 充值赠送-->
                <p id="czje" runat="server" style="display: none;margin-top:10px;margin-left:-58px;">
                    充值金额
                    <input type="text" id="txtcz" runat="server" class="half" />元以上
                    
                    
                </p>
                <!--赠送-->
                <p id="zssm" runat="server" style="display: none">
                    <i>赠送说明</i>
                    <input type="text" id="bzsss" value="指定赠送" runat="server" />
                </p>
                <p id="sfzs" runat="server" style="display: none" class="radio isopen">
                    <i>是否赠送</i>
                    <label class="checked">是</label><label >否</label>
                </p>
            </div>
        </dd>
        <dd class="inp" id="srzs" runat="server"  style="display: none" >
            <small>赠送时间</small>
            <div id="RadioSendTimes" class="checkbox" style=" display:none">
                <label class="checked" value="0">
                    生日当天
                </label>
                <label class value="1">
                    一周前
                </label>
                <label class value="2">
                    生日当月
                </label>
            </div>
            <div>
            <p class="radio" id="RadioSendTime" style="display: ;">
                    <label class="checked" value="0">
                    生日当天
                </label>
                <label value="1">
                    一周前
                </label>
                <label value="2">
                    生日当月
                </label>
                </p>
                </div>
        </dd>
        <dd class="inp" style="display:;">
                <small>总发行</small>
                <div>
                    <input type="text" id="txtTotal" runat="server"  />张
                </div>
         </dd>
         
        <dd class="inp">
            <small>说明</small>
            <div>
                <textarea cols="30" id="txtInstructions" runat="server" rows="10"></textarea>
            </div>
        </dd>

        <dd class="inp"  >
            <small>启用门店</small>
            <div>
                <p class="checkbox" id="Open_Hotel"  style="display: block;">
                    <%=_Open_Hotel%>
                </p>
            </div>
        </dd>

        <dd class="useList">
            <small>可用门店</small>
            <ol id="gmkfNav">
            </ol>
        </dd>


        <dd class="inp"  >
            <small>消费类型</small>
            <div>
                <p class="checkbox" id="ConsumptionType"  style="display: block;">
                    <label class="checked" val="0">微信支付</label>
                    <label class="checked" val="1">会员卡支付</label>
                    <label class="checked" val="2">到店付款</label>
                    <label class="checked" val="3">积分兑换</label>
                </p>
            </div>
        </dd>

        <dd>
            <small>是否启用</small>
            <div id="sfqy" class="radio isopen">
                <label class="checked">
                    是</label><label>否</label>
            </div>
        </dd>
    </dl>
    </div>

    <script type="text/javascript">
        $(function () {



            $("#zdzs label").each(function () {
                var zdzs = "";
                if ('<%=_zdzs %>' == "1") {
                    zdzs = "全部";
                } else if ('<%=_zdzs %>' == "2") {

                    zdzs = "会员等级";
                } else if ('<%=_zdzs %>' == "3") {
                    zdzs = "个人";
                }
                if (zdzs == $(this).html()) {
                    $(this).addClass('checked').siblings().removeClass('checked');
                }
            });



            $("#term label").each(function () {
                var Effective = "";

                if ('<%=_Effective %>' == "1") {
                    Effective = "永久";
                } else if ('<%=_Effective %>' == "2") {

                    Effective = "时长";
                } else if ('<%=_Effective %>' == "3") {
                    Effective = "时段";
                }
                if (Effective == $(this).html()) {
                    $(this).addClass('checked').siblings().removeClass('checked');
                    $(this).parents('div').find('p').eq($(this).index()).show().siblings().hide();
                    $(this).parent().show();
                }
            });


            $("#sfzs label").each(function () {
                var sfzs = "";
                if ('<%=_sfzs %>' == "1") {
                    sfzs = "是";
                } else if ('<%=_sfzs %>' == "2") {
                    sfzs = "否";
                }

                if (sfzs == $.trim($(this).html())) {
                    $(this).addClass('checked').siblings().removeClass('checked');
                }
            });


            $("#sfqy label").each(function () {
                var sfqy = "";
                if ('<%=_sfqy %>' == "1") {
                    sfqy = "是";
                } else if ('<%=_sfqy %>' == "2") {
                    sfqy = "否";
                }
                if (sfqy == $.trim($(this).html())) {

                    $(this).addClass('checked').siblings().removeClass('checked');
                }
            });
            $("#Is_Day_ok label").each(function () {
                var Is_Day_ok = "";
                if ('<%=_Is_Day_ok %>' == "1") {
                    Is_Day_ok = "是";
                } else if ('<%=_Is_Day_ok %>' == "0") {
                    Is_Day_ok = "否";
                }
                if (Is_Day_ok == $.trim($(this).html())) {

                    $(this).addClass('checked').siblings().removeClass('checked');
                }
            });
            $("#hytype label").each(function () {
                var hytype = "";
                if ('<%=_hytype %>' == "0") {
                    hytype = "全部";
                } else if ('<%=_hytype %>' == "1") {
                    hytype = "老会员";
                } else if ('<%=_hytype %>' == "2") {
                    hytype = "新会员";
                }
                if (hytype == $.trim($(this).html())) {

                    $(this).addClass('checked').siblings().removeClass('checked');
                }
            });

            $("#yxrq label").each(function () {
                var sfqy = "";
                var yxrq = $("#hyxrq").val().split(",");
                for (var i = 0; i < yxrq.length; i++) {

                    if ($(this).attr("val") * 1 == yxrq[i] * 1) {
                        
                        $(this).addClass('checked'); return;
                    } else {
                        
                        $(this).removeClass('checked');
                    }
                }
            });

            $("#ConsumptionType label").each(function () {
                var sfqy = "";
                var ConsumptionType = $("#HConsumptionType").val().split(",");
                for (var i = 0; i < ConsumptionType.length; i++) {

                    if ($(this).attr("val") * 1 == ConsumptionType[i] * 1) {

                        $(this).addClass('checked'); return;
                    } else {

                        $(this).removeClass('checked');
                    }
                }
            });

        });
    </script>
    <script type="text/javascript">
        //赋值
//        if ($("#hfEffectiveType").val()*1 == 1) {

//        } else if ($("#hfEffectiveType").val()*1 == 2) {
//            $('.copnli .term').each(function () {
//                if ($.trim($(this).html()) == '时长') {
//                    $(this).addClass('checked').siblings().removeClass('checked');
//                    $(this).parents('div').find('p').eq($(this).index()).show().siblings().hide();
//                    $(this).parent().show();
//                }
//            });
//        } else if ($("#hfEffectiveType").val()*1 == 3) {
//            $('.copnli .term').each(function () {
//                if ($.trim($(this).html()) == '时段') {
//                    $(this).addClass('checked').siblings().removeClass('checked');
//                    $(this).parents('div').find('p').eq($(this).index()).show().siblings().hide();
//                    $(this).parent().show();
//                }
//            });
//        }
        //有效期 单选
        $('.copnli .term').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
            $(this).parents('div').find('p').eq($(this).index()).show().siblings().hide();
            $(this).parent().show();

            if ($.trim($(this).html()) == '永久') {
                $("#hfEffectiveType").val(1);

            } else if ($.trim($(this).html()) == '时长') {
                $("#hfEffectiveType").val(2);

            } else if ($.trim($(this).html()) == '时段') {
                $("#hfEffectiveType").val(3);

            }

        });
        //赋值
        $('.copnli .gets').each(function () {
            if ($(this).val() == 2 || $(this).val() == 11)//消费赠送
            {
                $("#hqfssm").text("会员消费指定金额后即可获得卡券");
                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").show(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            }
            else if ($(this).val() == 12 || $(this).val() == 3)//充值赠送
            {
                $("#hqfssm").text("会员充值指定金额后即可获得卡券");
                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").show();
            }
            else if ($(this).val() == 10)//指定赠送
            {
                $("#srzs").hide();
                                $("#zdzs").show(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide();
//                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").show(); $("#hyxx").show();
//                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide();



//                if ($("#hfzdzs").val() == '1') {

//                    $("#zdzs").show(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
//                    $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
//                }
//                else if ($("#hfzdzs").val() == '2') {
//                    $("#zdzs").show(); $("#hydj").show(); $("#hykh").hide(); $("#hyxx").hide();
//                    $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
//                }
//                else if ($("#hfzdzs").val() == '3') {
//                    $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").show(); $("#hyxx").show();
//                    $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
//                }



            }
            else if ($(this).val() == 1)//注册赠送
            {
                $("#hqfssm").text("用户注册成为会员后即可获得卡券");

                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            }
            else if ($(this).val() == 5)//评论赠送
            {
                $("#hqfssm").text("会员评论并处理后即可获得卡券");

                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            }
            else if ($(this).val() == 14)//自主领取
            {
                
                $("#hqfssm").text("会员可在微网【优惠】>【领券中心】领取");
                $("#numsm").text("同一会员最多领取");

                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            } 
            else if ($(this).val() == 15)//生日赠送
            {
                
                $("#hqfssm").text("会员生日自动发放");
                $("#srzs").show();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            }
            else//其他类型不可见
            {
                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            }
        });

        //下拉列表
        $('.copnli .gets').on('click', function () {

            if ($(this).val() == 2 || $(this).val() == 11)//消费赠送
            {
                $("#hqfssm").text("会员消费指定金额后即可获得卡券");
                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").show(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            }
            else if ($(this).val() == 12 || $(this).val() == 3)//充值赠送
            {
                $("#hqfssm").text("会员充值指定金额后即可获得卡券");
                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").show();
            }
            else if ($(this).val() == 10)//指定赠送
            {
                $("#srzs").hide();
                                $("#zdzs").show(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide();


//                                if ($("#hfzdzs").val() == '1') {

//                                    $("#zdzs").show(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
//                                    $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
//                                }
//                                else if ($("#hfzdzs").val() == '2') {
//                                    $("#zdzs").show(); $("#hydj").show(); $("#hykh").hide(); $("#hyxx").hide();
//                                    $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
//                                }
//                                else if ($("#hfzdzs").val() == '3') {
//                                    $("#zdzs").show(); $("#hydj").hide(); $("#hykh").show(); $("#hyxx").show();
//                                    $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
//                                }

//                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").show(); $("#hyxx").show();
//                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide();



//                if ($("#hfzdzs").val() == '1') {

//                    $("#zdzs").show(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
//                    $("#xfje").hide(); $("#zssm").show(); $("#sfzs").show(); $("#czje").hide();
//                }
//                else if ($("#hfzdzs").val() == '2') {
//                    $("#zdzs").show(); $("#hydj").show(); $("#hykh").hide(); $("#hyxx").hide();
//                    $("#xfje").hide(); $("#zssm").show(); $("#sfzs").show(); $("#czje").hide();
//                }
//                else if ($("#hfzdzs").val() == '3') {
//                    $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").show(); $("#hyxx").show();
//                    $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
//                }

            }
            else if ($(this).val() == 1)//注册赠送
            {
                $("#hqfssm").text("用户注册成为会员后即可获得卡券");
                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            } 
            else if ($(this).val() == 5)//评论赠送
            {
                $("#hqfssm").text("会员评论并处理后即可获得卡券");
                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            }
            else if ($(this).val() == 14)//自主领取
            {
                
                $("#hqfssm").text("会员可在微网【优惠】>【领券中心】领取");
                $("#numsm").text("同一会员最多领取");
                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            }
            else if ($(this).val() == 15)//生日赠送
            {

                $("#hqfssm").text("会员生日自动发放");
                
                $("#srzs").show();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            }
            else//其他类型不可见
            {
                $("#srzs").hide();
                $("#zdzs").hide(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide(); $("#czje").hide();
            }

        });

        $('#RadioSendTime').on('click', 'label', function () {
            
            //$(this).addClass('checked').siblings().removeClass('checked');
            $("#hdSendType").val($(this).attr('value'));
            //alert($(this).html());
             $(this).addClass('checked');
            $(this).siblings().removeClass('checked');

        });



        

        //指定赠送 单选
        $('.copnli .mem').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');

            if ($.trim($(this).html()) == '全部') {

                $("#zdzs").show(); $("#hydj").hide(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide();
                $("#hfzdzs").val(1);
            }
            else if ($.trim($(this).html()) == '会员等级') {
                $("#zdzs").show(); $("#hydj").show(); $("#hykh").hide(); $("#hyxx").hide();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide();
                $("#hfzdzs").val(2);
            }
            else if ($.trim($(this).html()) == '个人') {
                $("#zdzs").show(); $("#hydj").hide(); $("#hykh").show(); $("#hyxx").show();
                $("#xfje").hide(); $("#zssm").hide(); $("#sfzs").hide();
                $("#hfzdzs").val(3);
            }


        })

        //会员等级 多选
        $('.copnli .checkbox').on('click', 'label', function () {
            $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');
        });
//        //可用日期
//        $('#yxrq').on('click', 'label', function () {
//            $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');
//        });

//        //节假日
//        $('#jjr').on('click', 'label', function () {
//            $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');
//        });

       

        //是否 单选
        $('#sfzs').on('click', 'label', function () {

            $(this).addClass('checked').siblings().removeClass('checked');
            if ($.trim($(this).html()) == "是") {
                $("#hfsfzs").val(1);
            } else if ($.trim($(this).html()) == "否") {
                $("#hfsfzs").val(0);
            }
        });
        $('#sfqy').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
            if ($.trim($(this).html()) == "是") {
                $("#hfsfqy").val(1);
            } else if ($.trim($(this).html()) == "否") {
                $("#hfsfqy").val(0);
            }

        });
        $('#Is_Day_ok').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
            if ($.trim($(this).html()) == "是") {
                $("#hfIs_Day_ok").val(1);
            } else if ($.trim($(this).html()) == "否") {
                $("#hfIs_Day_ok").val(0);
            }

        });
        $('#hytype').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
            if ($.trim($(this).html()) == "全部") {
                $("#hhytype").val(0);
            } else if ($.trim($(this).html()) == "老会员") {
                $("#hhytype").val(1);
            } else if ($.trim($(this).html()) == "新会员") {
                $("#hhytype").val(2);
            }

        });

        //二级列表
        $('.useList').on('click', 'b', function () {
            $(this).parent().toggleClass('down');
            $(this).siblings('.sub').slideToggle(140);
        })
    </script>
    <div class="copnliBtn">
        <asp:Button ID="btnSumit" runat="server" Text="提交" OnClientClick="return checkIput()"
            class="copnliBtn" OnClick="btnSumit_Click" />
    </div>
    <asp:HiddenField ID="hfdID" runat="server" Value="0" />
    <asp:HiddenField ID="hfzdzs" runat="server" Value="1" />
    <asp:HiddenField ID="rdlx" runat="server" Value="0" />
    <asp:HiddenField ID="qiyong" runat="server" Value="0" />
    <asp:HiddenField ID="hfsfzs" runat="server" Value="1" />
    <asp:HiddenField ID="hfsfqy" runat="server" Value="1" />
    <asp:HiddenField ID="hfIs_Day_ok" runat="server" Value="1" />
    <asp:HiddenField ID="hhytype" runat="server" Value="0" />
    <asp:HiddenField ID="hfEffectiveType" runat="server" Value="1" />
    <asp:HiddenField ID="labelhtml" runat="server" Value="0" />
    <asp:HiddenField ID="sumhotel" runat="server" Value="0" />
    <asp:HiddenField ID="hyxrq" runat="server" Value="" />
    <asp:HiddenField ID="hjjr" runat="server" Value="" />
    <asp:HiddenField ID="hishy" runat="server" Value="" />
    <asp:HiddenField ID="HOpen_Hotel" runat="server" Value="" />
    <asp:HiddenField ID="HConsumptionType" runat="server" Value="" />
    <input runat="server" type="hidden" id="hdSendType" value="0" /><%--会员生日赠送时间类型--%>
    <div id="tdhtml" style="display: none">
        <input runat="server" id="lsh" value="{@lsh}" type="hidden" />
        姓名：<asp:Label ID="Label1" runat="server">{@xm}</asp:Label>
        手机号码：<asp:Label ID="Label2" runat="server">{@sjhm}</asp:Label><br />
    </div>
    </form>
    <script type="text/javascript">

        $("#RadioSendTime label").each(function () {
            var SendTime = "";
            if ($("#hdSendType").val() == "0") {
                SendTime = "生日当天";
            } else if ($("#hdSendType").val() == "1") {
                SendTime = "一周前";
            } else if ($("#hdSendType").val() == "2") {
                SendTime = "生日当月";
            }
            if (SendTime == $.trim($(this).html())) {
                $(this).addClass('checked').siblings().removeClass('checked');
            }
        });

        $(function () {

            $("#btnyes").click(function () {
                if ($("#kh").val().replace(/\s+/g, "") == "") {
                    showTipsMsg("请输入卡号！", 3000, 3);
                }
                else {

                    $("#hyxxtd").html("");
                    $.ajax({
                        url: "selectkh.ashx",
                        data: {
                            kh: $("#kh").val()
                        },
                        type: "GET",
                        dataType: "JSON",
                        success: function (response) {
                           // alert(response);
                            if (response != "1") {
                                var obj = eval(response);

                                var str = "";
                                var hftr = $("#tdhtml").html();
                                $(obj).each(function (index) {
                                    var copytr = hftr;

                                    var val = obj[index];
                                    //alert(val.LSH);

                                    copytr = copytr.replace("{@lsh}", val.LSH);
                                    copytr = copytr.replace("{@xm}", val.XM);
                                    copytr = copytr.replace("{@sjhm}", val.SJHM);
                                    $("#hyxxtd").append(copytr);
                                });
                                $("#hyxx").show();
                            }
                            else {
                                showTipsMsg("卡号不存在！", 3000, 3);
                                $("#kh").focus();
                            }
                        }
                    });
                }

            })
        })


        function checkIput() {

            if ($("#txtName").val() == "") {
                alert('名称不能为空！');
                return false;
            }
            if ($("#txtPar").val() == "") {
                alert('面值不能为空！');
                return false;
            }
            if ($("#DDLmode").val() == "0") {
                alert("请选择获取方式！");
                return false;
            }

            if ($("#DDLmode").val() == "2") {

                if ($("#txtFirstMoney").val() == "") {
                    alert("请输入消费金额");
                    return false;
                }
//                if ($("#txtSecondMoney").val() == "") {
//                    alert("请输入消费金额");
//                    return false;
//                }

            }


            if ($("#hfEffectiveType").val() == "2") {

                if ($("#txtDay").val() == "") {
                    alert("请输入天数");
                    return false;
                }

            } else if ($("#hfEffectiveType").val() == "3") {
                if ($("#B_sj").val() == "") {
                    alert("请选择开始日期！");
                    return false;
                }
                //结束日期
                if ($("#E_sj").val() == "") {
                    alert("请选择结束日期！");
                    return false;
                }

                var s1 = $("#B_sj").val();
                var start = new Date(s1.replace("-", "/").replace("-", "/"));
                var s2 = $("#E_sj").val();
                var end = new Date(s2.replace("-", "/").replace("-", "/"));
                if (end < start) {

                    alert("开始日期大于结束日期！");
                    return false;
                }

            }


            //会员等级
            var labelhtml = "";
            for (var i = 0; i < $("#hydj").find(".checked").length; i++) {

                labelhtml += $("#hydj").find(".checked").eq(i).html() + ',';

            }
            $("#labelhtml").val(labelhtml);

            //有效日期
            var yxrq = "";
            for (var i = 0; i < $("#yxrq").find(".checked").length; i++) {

                yxrq += $("#yxrq").find(".checked").eq(i).attr("val") + ',';

            }
            $("#hyxrq").val(yxrq);


            //消费类型
            var ConsumptionType = "";
            for (var i = 0; i < $("#ConsumptionType").find(".checked").length; i++) {

                ConsumptionType += $("#ConsumptionType").find(".checked").eq(i).attr("val") + ',';

            }
            $("#HConsumptionType").val(ConsumptionType);

            //节假日
            var jjr = "";
            for (var i = 0; i < $("#jjr").find(".checked").length; i++) {

                jjr += $("#jjr").find(".checked").eq(i).attr("val") + ',';

            }
            $("#hjjr").val(jjr);

            //有效会员
            var ishy = "";
            for (var i = 0; i < $("#ishy").find(".checked").length; i++) {

                ishy += $("#ishy").find(".checked").eq(i).attr("val") + ',';

            }
            $("#hishy").val(ishy);

            //启用门店
            var isopen_hotel = "";
            for (var i = 0; i < $("#Open_Hotel").find(".checked").length; i++) {

                isopen_hotel += $("#Open_Hotel").find(".checked").eq(i).attr("val") + ',';

            }
            $("#HOpen_Hotel").val(isopen_hotel);

            //获取勾选所有样式值kgClose（hotelid）   kgClose2（gsid）
            var sumhotel = "";
            var hotelid = "";
            for (var i = 0; i < $(".kgClose").length; i++) {
                hotelid += $(".kgClose").eq(i).attr("hotelid") + ",";
                // alert($(".kgClose").eq(i).attr("hotelid"));
            }
            var roomid = "";
            for (var k = 0; k < $(".kgClose2").length; k++) {
                roomid += $(".kgClose2").eq(k).attr("gsid") + ",";
                //alert($(".kgClose2").eq(k).attr("gsid"));
            }
            sumhotel = hotelid + roomid;
            $("#sumhotel").val(sumhotel);
            return true;
        }


        function zskq() {
            var url = "/SysSetBase/coupons/couponappoint.aspx";
            top.art.dialog.open(url, {
                id: 'MattersEditzs',
                title: '指定赠送对象选择',
                width: 500,
                height: 318,
                close: function () {
                    //ListGrid();
                    //alert(111555);
                }
            }, false);
        }
    </script>
</body>
</html>
