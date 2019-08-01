<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="room_Distribution.aspx.cs"
    Inherits="RM.Web.SysSetBase.sell.room_Distribution" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script src="../../App_Themes/default/js/jquery-1.8.3.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="altconX">
            <div class="sllAlt">
                <dl class="sll_l" id="hotelList">
                </dl>
                <div class="sll_r">
                    <table>
                        <thead>
                            <tr>
                                <th>
                                    客房
                                </th>
                                <th>
                                    首次奖励
                                </th>
                                <th>
                                    多次奖励
                                </th>
                            </tr>
                        </thead>
                        <tbody id="roomList">
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="sllBtn">
                <a id="btnyes">保存</a>
            </div>
        </div>
    </div>
    <div style="display: none" id="hotelhtml">
        <dd hid="{@id}">
            <b>{@hotelname}</b>
        </dd>
    </div>
    <table style="display: none">
        <tbody id="roomhtml">
            <tr>
                <td>
                    {@kfname}
                </td>
                <td>
                    <input type="text" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')" value="{@first_internal}" />元
                </td>
                <td>
                    <input type="text" maxlength="5"  onkeyup="value=value.replace(/[^\d.]/g,'')" value="{@repeatedly_internal}" />元
                </td>
            </tr>
        </tbody>
    </table>
    <input runat="server" id="AdminHotelid" type="hidden" />
    <input id="hotelid" type="hidden" value="" />
    </form>
    <script>

        $(function () {
            hotellist();
        });

        $("#hotelList").on("click", "dd", function () {
            var hotelid = $(this).attr("hid");
            $("#hotelid").val(hotelid);
            roomlist(hotelid);
        });

        function hotellist() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "Gethotel",
                    adminhotelid: $("#AdminHotelid").val()
                },
                type: "GET",
                dataType: "text",
                async: false,
                success: function (data) {
                    if (data == "") {
                        return;
                    }
                    var json = eval("(" + data + ")");
                    var hftr = $("#hotelhtml").html();
                    $("#hotelList").empty();
                    for (var i = 0; i < json.length; i++) {
                        var copytr = hftr;
                        copytr = copytr.replace("{@id}", json[i].ID);
                        copytr = copytr.replace("{@hotelname}", json[i].NAME);
                        $("#hotelList").append(copytr);
                    }
                }
            });
        }

        function roomlist(hotelid) {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetroomList",
                    adminhotelid: $("#AdminHotelid").val(),
                    hotelid: hotelid
                },
                type: "GET",
                dataType: "text",
                async: false,
                success: function (data) {
                    if (data == "") {
                        $("#roomList").html("");
                        return;
                    }
                    var json = eval("(" + data + ")");
                    var hftr = $("#roomhtml").html();
                    $("#roomList").empty();
                    for (var i = 0; i < json.length; i++) {
                        var copytr = hftr;
                        copytr = copytr.replace("{@kfname}", json[i].NAME);
                        copytr = copytr.replace("{@first_internal}", json[i].first_internal);
                        copytr = copytr.replace("{@repeatedly_internal}", json[i].repeatedly_internal);

                        $("#roomList").append(copytr);
                    }
                }
            });
        }

        $("#btnyes").click(function () {
            var hotelid = $("#hotelid").val();
            if (hotelid == "") {
                alert("请选择酒店！");
                return;
            }
            
            var hdvals = "";

            $("#roomList input[type=text]").each(function (index) {
                var tVal = $(this).val() == "" ? "0" : $(this).val();
                if (index % 2 == 0) {
                    hdvals += tVal + ",";
                } else {
                    hdvals += tVal + "|";
                }
            });
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "save_disFJ",
                    adminhotelid: $("#AdminHotelid").val(),
                    hotelid: hotelid,
                    hdvals: hdvals
                },
                type: "GET",
                dataType: "text",
                async: false,
                success: function (data) {
                    if (data == "1") {
                        alert("保存成功！");
                    } else {
                        alert("保存失败！");
                    }
                }
            });
        });
    </script>
</body>
</html>
