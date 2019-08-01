<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="setPrice.aspx.cs" Inherits="RM.Web.SysSetBase.houseState.setPrice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>批量设置房价</title>
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
    <input runat="server" type="hidden" id="hdJFState" value="1" />
    <div class="gtall gmkf clearfix">
        <div class="gmkfNav" id="HotelTree" runat="server">
            <dl>
                <%=hotelTreeHtml.ToString()%>
            </dl>
        </div>
        <div class="gmkfStion" style="padding-left: 20px; overflow: auto;">
            <strong class="hasbtn"><b>批量设置房价</b><a href="roomstate.aspx">返回</a></strong>
            <div class="plsz">
                <i class="text">日期范围</i>
                <div class="date">
                    <input runat="server" id="txtStartTime" type="text" value="2018-01-01" />
                    <input runat="server" id="txtEndTime" type="text" value="2018-02-01" />
                </div>
            </div>
            <div class="hpce">
                <table>
                    <thead id="vipTitle">
                        <asp:Repeater runat="server" ID="rptVip">
                            <HeaderTemplate>
                                <tr>
                                    <th rowspan="2" style="width: 150px;">
                                        <label id="allChecked">
                                            房型
                                        </label>
                                    </th>
                                    <th rowspan="2" style="width: 150px;">
                                        售房规则
                                    </th>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <th class="vip" colspan="2" hid="<%# Eval("code") %>" dataval="<%# Eval("LevelName")%>">
                                    <%# Eval("LevelName")%>
                                </th>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tr>
                                <tr>
                                    <%# GetFooterTr()%>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
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
        $(function () {
            if ($("#hdJFState").val() != "1") {
                $(".jf").hide();
            }
            ListGrid();
        });

        $('.gmkfNav').on('click', 'b', function () {
            $(this).siblings('ul').slideToggle(120);
            $(this).parents('dd').toggleClass('down');
        });

        $("#HotelTree li").on("click", function () {
            var hotelid = $(this).attr("hotelid");
            $("#hdHotelId").val(hotelid);
            //kflist();
            ListGrid();
        });
        $('#rulelist').on('click', 'label', function () {
            $(this).hasClass('active') ? $(this).removeClass('active') : $(this).addClass('active');
            if ($(this).attr("roomname") == "true") {
                $("#rulelist tr[group='" + $(this).attr("roomid") + "']").find("label").attr("class", $(this).attr("class"));
            }
        })


        $('#rulelist').on('keypress', 'input', function (e) {
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        });

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

        function ListGrid() {
            var jfstate = "";
            if ($("#hdJFState").val() != "1") {
                jfstate = " style='display:none;'";
            }
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
                            var copytr = "<tr Group='" + dti.Id + "'>";
                            if (j == 0) {
                                copytr += "<td rowspan='" + lists.length + "'>";
                                copytr += "<label roomid='" + dti.Id + "' roomname='true'>";
                                copytr += dti.Name;
                                copytr += "</label></td>";
                            }

                            copytr += "<td>";
                            copytr += "<label Rule='true'>";
                            copytr += lists[j].Rule_Name;
                            copytr += "</label></td>";
                            var RoomId = dti.Id;
                            var RuleId = lists[j].ID;
                            $(vips).each(function () {
                                var vip_code = $(this).attr("hid");
                                var group = RuleId + "_" + vip_code;
                                //平日价
                                copytr += "<td>";
                                copytr += "<div class='pf'><input type='text' maxlength='6' Group='" + group + "' RoomId='" + RoomId + "' RuleId='" + RuleId + "' vip_code='" + vip_code + "' name='prj' />元</div>";
                                copytr += "<div class='pf jf' " + jfstate + "><input type='text' maxlength='6' Group='" + group + "' RoomId='" + RoomId + "' RuleId='" + RuleId + "' vip_code='" + vip_code + "' name='prj_jf' />分</div>";
                                copytr += "</td>";
                                //    周末价                            
                                copytr += "<td>";
                                copytr += "<div class='pf'><input type='text' maxlength='6' Group='" + group + "' RoomId='" + RoomId + "' RuleId='" + RuleId + "' vip_code='" + vip_code + "' name='zmj' />元</div>";
                                copytr += "<div class='pf jf' " + jfstate + "><input type='text' maxlength='6' Group='" + group + "' RoomId='" + RoomId + "' RuleId='" + RuleId + "' vip_code='" + vip_code + "' name='zmj_jf' />分</div>";
                                copytr += "</td>";
                            });
                            copytr += "</tr>";
                            roomlist += copytr;
                        }
                        $("#rulelist").append(roomlist);
                    }
                }
            });
        }

        function SaveInfo() {
            var arr = [];
            var objInfo = [];
            $("#rulelist label[Rule='true'].active").each(function () {
                $(this).parents("tr:eq(0)").find("input").each(function () {
                    var group = $(this).attr("group");
                    var roomid = $(this).attr("roomid");
                    var ruleid = $(this).attr("ruleid");
                    var vip_code = $(this).attr("vip_code");
                    var name = $(this).attr("name");
                    var val = $(this).val();
                    if (val == "") {
                        val = "0";
                    }
                    var ag = $.inArray(group, arr);
                    if (ag >= 0) {
                        var obj = objInfo[ag];
                        obj[name] = val;
                    } else {
                        arr.push(group);
                        var obj = new Object();
                        obj.RoomId = roomid;
                        obj.RuleId = ruleid;
                        obj.Vip_Code = vip_code;
                        obj[name] = val;
                        objInfo.push(obj);
                    }
                });
            });
            var jsonDate = JSON.stringify(objInfo);
            $.post("../Ajax/SysAjax.ashx", {
                Menu: "SetRulePrice",
                Hotelid: $("#hdHotelId").val(),
                JsonDate: jsonDate,
                StartDate: $("#txtStartTime").val(),
                EndDate: $("#txtEndTime").val()
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
