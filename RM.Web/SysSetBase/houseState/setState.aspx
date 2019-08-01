<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="setState.aspx.cs" Inherits="RM.Web.SysSetBase.houseState.setState" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>批量设置房态</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <input runat="server" type="hidden" id="htHotelTree" value="true" />
    <div class="gtall gmkf clearfix">
        <div class="gmkfNav" id="HotelTree" runat="server">
            <dl>
                <%=hotelTreeHtml.ToString()%>
            </dl>
        </div>
        <div class="gmkfStion" style="padding-left: 20px; overflow: auto;">
            <strong class="hasbtn"><b>批量设置房态</b><a href="roomstate.aspx">返回</a></strong>
            <div class="plsz">
                <i class="text">日期范围</i>
                <div class="date">
                    <input runat="server" id="txtStartTime" type="text" value="2018-01-01" />
                    <input runat="server" id="txtEndTime" type="text" value="2018-02-01" />
                </div>
            </div>
            <div class="plsz">
                <i class="text">有效日期</i>
                <div id="EffectiveDate" class="checkbox">
                    <label wk="Monday" class="checked">
                        周一</label>
                    <label wk="Tuesday" class="checked">
                        周二</label>
                    <label wk="Wednesday" class="checked">
                        周三</label>
                    <label wk="Thursday" class="checked">
                        周四</label>
                    <label wk="Friday" class="checked">
                        周五</label>
                    <label wk="Saturday" class="checked">
                        周六</label>
                    <label wk="Sunday" class="checked">
                        周日</label>
                </div>
            </div>
            <div class="plsz">
                <i class="text">房态</i>
                <div id="RoomState" class="radio">
                    <label rs="1" class="checked">
                        开启</label>
                    <label rs="0">
                        关闭</label>
                </div>
            </div>
            <div class="plsz">
                <i class="text"></i>
                <div class="much">
                    可订房
                    <input id="txtEffectiveNumber" type="text" />
                    间
                </div>
            </div>
            <div class="hpce">
                <table style="width: 400px;">
                    <thead id="vipTitle">
                        <tr>
                            <th>
                                <label id="allChecked">
                                    房型
                                </label>
                            </th>
                            <th>
                                售房规则
                            </th>
                        </tr>
                    </thead>
                    <tbody id="rulelist">
                    </tbody>
                </table>
            </div>
            <a class="button buttonActive" onclick="SaveInfo()">保存</a>
        </div>
    </div>
    </form>
    <script type="text/javascript">

        $('#EffectiveDate').on('click', 'label', function () {
            $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');
        })

        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked')
        })

        $('.gmkfNav').on('click', 'b', function () {
            $(this).siblings('ul').slideToggle(120);
            $(this).parents('dd').toggleClass('down');
        });

        $("#HotelTree li").on("click", function () {
            var hotelid = $(this).attr("hotelid");
            $("#hdHotelId").val(hotelid);
            ListGrid();
        });
        $('#rulelist').on('click', 'label', function () {
            $(this).hasClass('active') ? $(this).removeClass('active') : $(this).addClass('active');
            if ($(this).attr("roomname") == "true") {
                $("#rulelist tr[group='" + $(this).attr("roomid") + "']").find("label").attr("class", $(this).attr("class"));
            }
        })

        $("#allChecked").click(function () {
            if ($(this).attr('class') == "active") {
                $(this).removeClass('active');
                $('#rulelist label').removeClass('active');
            } else {
                $('#rulelist label').addClass('active');
                $(this).addClass('active');
            }
        });
    </script>
    <script type="text/javascript">
        $(function () {
            ListGrid();
        });
        function ListGrid() {
            var kfid = ""; //$("#roomlist").val();
            var txtSearch = ""; //$.trim($("#txtSearch").val());
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
                    if (data == null || data == "") {
                        return;
                    }
                    var vips = $("#vipTitle .vip");
                    for (var i = 0; i < data.length; i++) {
                        var dti = data[i];
                        var lists = dti.list;
                        var roomlist = "";
                        for (var j = 0; j < lists.length; j++) {
                            var RoomId = dti.Id;
                            var RuleId = lists[j].ID;
                            var copytr = "<tr Group='" + RoomId + "'>";
                            if (j == 0) {
                                copytr += "<td rowspan='" + lists.length + "'>";
                                copytr += "<label roomid='" + RoomId + "' roomname='true'>";
                                copytr += dti.Name;
                                copytr += "</label></td>";
                            }
                            copytr += "<td>";
                            copytr += "<label Rule='true' roomid = '" + RoomId + "' ruleid = '" + RuleId + "'>";
                            copytr += lists[j].Rule_Name;
                            copytr += "</label></td>";
                            copytr += "</tr>";
                            roomlist += copytr;
                        }
                        $("#rulelist").append(roomlist);
                    }
                }
            });
        }

        function SaveInfo() {
            var hid = $("#hdHotelId").val();
            if (hid == "-1" || hid == "") {
                showTipsMsg('请先选择酒店！', '5000', '3');
                return;
            }

            var objInfo = [];
            $("#rulelist label[Rule='true'].active").each(function () {
                var roomid = $(this).attr("roomid");
                var ruleid = $(this).attr("ruleid");
                var obj = new Object();
                obj.RoomId = roomid;
                obj.RuleId = ruleid;
                objInfo.push(obj);
            });
            var wk = [];
            $("#EffectiveDate .checked").each(function () {
                wk.push($(this).attr("wk"));
            });
            if (wk.length < 1) {
                showTipsMsg('请选择有效日期！', '5000', '3');
                return;
            }
            var rs = $("#RoomState .checked").attr("rs");
            var jsonDate = JSON.stringify(objInfo);
            $.post("../Ajax/SysAjax.ashx", {
                Menu: "SetRoomState",
                Hotelid: $("#hdHotelId").val(),
                JsonDate: jsonDate,
                StartDate: $("#txtStartTime").val(),
                EndDate: $("#txtEndTime").val(),
                Room_State: rs,
                EffectiveNumber: $("#txtEffectiveNumber").val(),
                EffectiveDate: wk.join(",")
            }, function (date) {
                if (date == "1") {
                    showTipsMsg('保存成功！', '5000', '5');
                    ListGrid();
                } else {
                    showTipsMsg('保存失败！', '5000', '3');
                }
            })
        }
    </script>
</body>
</html>
