<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="houseRule.aspx.cs" Inherits="RM.Web.SysSetBase.houseState.houseRule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>售房规则</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <style type="text/css">
        .rul
        {
            height: calc(100% - 92px);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>酒店管理</span> &gt; </span><span>售房规则</span></span>
        </div>
    </div>
    <div class="gtall gmkf clearfix">
        <div class="gmkfNav" id="HotelTree" runat="server">
            <dl>
                <%=hotelTreeHtml.ToString()%>
            </dl>
        </div>
        <div class="gmkfStion" style="padding: 0">
            <div class="ptb8">
                <div class="w120">
                    <select id="roomlist">
                        <option value="0">全部房型</option>
                    </select>
                </div>
                <div class="sharesearch">
                    <input name="txtSearch" id="txtSearch" type="text" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid();"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="add()">添加</span>
                    </div>
                </div>
            </div>
            <div class="rul pq-cont" style="width: auto;">
                <table>
                    <thead>
                        <tr>
                            <th>
                                房型
                            </th>
                            <th>
                                规则名称
                            </th>
                            <th style="display: none;">
                                销售规则
                            </th>
                            <th>
                                早餐份数
                            </th>
                            <th style=" display:none;">
                                立减
                            </th>
                            <th style="display: none;">
                                付款方式
                            </th>
                            <th style="display: none;">
                                适用于
                            </th>
                            <th>
                                内容
                            </th>
                            <th>
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody id="rulelist">
                        <%--<tr>
                            <td rowspan="3">
                                高级标间
                            </td>
                            <td>
                                无早
                            </td>
                            <td>
                                无
                            </td>
                            <td>
                                无早
                            </td>
                            <td>
                                预付
                            </td>
                            <td>
                            </td>
                            <td>
                            <div class="caozuo"><span class="icon-edit4" title="编辑" onclick="edit(this)">编辑</span><span class="icon-lbx" title="删除" onclick="delete(this)">删除</span></div>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                有早
                            </td>
                            <td>
                                提前1天预订
                            </td>
                            <td>
                                单早
                            </td>
                            <td>
                                预付
                            </td>
                            <td>
                            </td>
                            <td>
                                修改 删除
                            </td>
                        </tr>
                        <tr>
                            <td>
                                有早
                            </td>
                            <td>
                                提前2晚及以上
                            </td>
                            <td>
                                双早
                            </td>
                            <td>
                                预付、现付
                            </td>
                            <td>
                            </td>
                            <td>
                                修改 删除
                            </td>
                        </tr>--%>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <table style="display: none">
        <tbody id="hdRowTrHtml">
            <tr dataid="{@DataId}">
                <td rowspan="{@rowspan}">
                    {@RoomName}
                </td>
                <td>
                    {@RuleName}
                </td>
                <td style="display: none;">
                    {@RuleSales}
                </td>
                <td>
                    {@breakfast}
                </td>
                <td style="display: none;">
                    {@Discount}
                </td>
                <td style="display: none;">
                    {@paytype}
                </td>
                <td style="display: none;">
                    {@viptype}
                </td>
                <td>
                    {@bz}
                </td>
                <td>
                    <div class="caozuo">
                        <span class="icon-edit4" title="修改" onclick="edit(this)">修改</span><span class="icon-lbx"
                            title="删除" onclick="del(this)">删除</span></div>
                </td>
            </tr>
        </tbody>
        <tbody id="hdTrHtml">
            <tr dataid="{@DataId}">
                <td>
                    {@RuleName}
                </td>
                <td style="display: none;">
                    {@RuleSales}
                </td>
                <td>
                    {@breakfast}
                </td>
                <td style="display: none;">
                    {@Discount}
                </td>
                <td style="display: none;">
                    {@paytype}
                </td>
                <td style="display: none;">
                    {@viptype}
                </td>
                <td>
                    {@bz}
                </td>
                <td>
                    <div class="caozuo">
                        <span class="icon-edit4" title="修改" onclick="edit(this)">修改</span><span class="icon-lbx"
                            title="删除" onclick="del(this)">删除</span></div>
                </td>
            </tr>
        </tbody>
    </table>
    <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <input runat="server" type="hidden" id="htHotelTree" value="true" />
    </form>
    <script type="text/javascript">
        $('.gmkfNav').on('click', 'b', function () {
            $(this).siblings('ul').slideToggle(120);
            $(this).parents('dd').toggleClass('down');
        });

        $("#HotelTree li").on("click", function () {
            var hotelid = $(this).attr("hotelid");
            $("#hdHotelId").val(hotelid);
            kflist();
            ListGrid();
        });
    </script>
    <script type="text/javascript">

        $(function () {
            kflist();
            ListGrid();
            $("#roomlist").change(function () {
                ListGrid();
            });
        });

        function kflist() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetroomList",
                    hotelid: $("#hdHotelId").val(),
                    adminhotelid: $("#Hdhoteladmin").val()
                },
                type: "POST",
                dataType: "text",
                success: function (data) {
                    var list = "<option value='0'>全部房型</option>";
                    if (data == "") {
                        $("#roomlist").html(list);
                        return;
                    }
                    var json = eval("(" + data + ")");

                    for (var i = 0; i < json.length; i++) {
                        list += "<option value='" + json[i].ID + "'>" + json[i].NAME + "</option>";
                    }
                    $("#roomlist").html(list);
                }
            });
        }


        function ListGrid() {
            var kfid = $("#roomlist").val();
            var txtSearch = $.trim($("#txtSearch").val());
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "houseRuleList",
                    hotelid: $("#hdHotelId").val(),
                    kfid: kfid,
                    txtSearch: txtSearch
                },
                type: "POST",
                dataType: "JSON",
                success: function (data) {
                   
                    $("#rulelist").html("");
                    debugger;
                    if (data == null || data == "") {
                        return;
                    }
                    var hideRowTr = $("#hdRowTrHtml").html();
                    var hideTr = $("#hdTrHtml").html();
                    for (var i = 0; i < data.length; i++) {
                        var dti = data[i];
                        var lists = dti.list;
                        for (var j = 0; j < lists.length; j++) {
                            var copytr = hideTr;
                            if (j == 0) {
                                copytr = hideRowTr;
                                copytr = copytr.replace("{@rowspan}", lists.length);
                                copytr = copytr.replace("{@RoomName}", dti.Name);
                            }
                            copytr = copytr.replace("{@DataId}", lists[j].ID);
                            copytr = copytr.replace("{@RuleName}", lists[j].Rule_Name);
                            copytr = copytr.replace("{@RuleSales}", lists[j].Sales);
                            copytr = copytr.replace("{@breakfast}", lists[j].Breakfast);
                            copytr = copytr.replace("{@Discount}", lists[j].Discount);
                            copytr = copytr.replace("{@paytype}", lists[j].Pay);
                            copytr = copytr.replace("{@viptype}", lists[j].Vip_Val);
                            copytr = copytr.replace("{@bz}", lists[j].Remarks); 
                            //copytr = copytr.replace("", lh.);
                            $("#rulelist").append(copytr);
                            //alert(copytr);
                        }
                    }
                    
                }
            });
        }

        //添加
        function add() {
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                showTipsMsg('请先选择所属酒店！', '5000', '5');
                return false;
            }
            var url = "/SysSetBase/houseState/houseRuleAdd.aspx?HotelId=" + HotelId;
            top.openDialog(url, 'UserInfo_Info', '添加售房规则', 470, 370, 50, 50);
        }

        function edit(e) {
            var key = $(e).parents("tr:eq(0)").attr("DataId");
            var url = "/SysSetBase/houseState/houseRuleAdd.aspx?key=" + key;
            top.openDialog(url, 'UserInfo_Info', '修改售房规则', 470, 370, 50, 50);
        }

        function del(e) {
            var key = $(e).parents("tr:eq(0)").attr("DataId");
            var delparm = 'action=DeleteP&module=售房规则&tableName=Room_Rule&pkName=ID&pkVal=' + key;
            delConfig('/Ajax/Common_Ajax.ashx', delparm, "ListGrid();");
        }
    </script>
</body>
</html>
