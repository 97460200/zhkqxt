<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manfang.aspx.cs" Inherits="RM.Web.SysSetBase.GuestRoom.manfang" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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
</head>
<body>
    <form id="form1" runat="server">
    <div class="gtall gmmf clearfix">
        <%--<div class="gmmfNav">
            <dl>
                <dd>
                    <span>柏丽酒店 (佛山文华北路店)</span>
                </dd>
                <dd>
                    <span>金逸酒店</span>
                </dd>
            </dl>
        </div>--%>
        <div class="gmmfNav">
            <dl id="gmmfNav">
            </dl>
        </div>
        <div class="gmmfLsit">
            <div class="wdyhd" style="padding-right: 12px;">
                <div class="r">
                    <span onclick="add()">添加</span> <span onclick="edit()">修改</span> <span onclick="Delete()">
                        删除</span>
                </div>
            </div>
            <div class="mftable">
                <table class="ul" style="width: 800px;">
                    <thead>
                        <tr>
                            <th width="40">
                            </th>
                            <th width="40">
                                <i class="icon-radio6"></i>
                            </th>
                            <th>
                                客房名称
                            </th>
                            <th>
                                开始时间
                            </th>
                            <th>
                                结束时间
                            </th>
                            <th>
                                天数
                            </th>
                            <th width="80">
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody id="tb">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div style="display: none" id="hotelnameList">
        <dd hotelid="{@hotelid}">
            <span>{@hotelname} </span>
        </dd>
    </div>
    <table style="display: none">
        <tbody id="tr">
            <tr name="{@id}">
                <td>
                    {@sy}
                </td>
                <td>
                    <i class="icon-radio6"></i>
                </td>
                <td>
                    {@kfname}
                </td>
                <td>
                    {@kssj}
                </td>
                <td>
                    {@jssj}
                </td>
                <td>
                    {@ts}
                </td>
                <td>
                    <i class="icon-edit4" onclick="edits('{@ids}')"></i><i class="icon-lbx" onclick="Deletes('{@idd}')">
                    </i>
                </td>
            </tr>
        </tbody>
    </table>
    <input runat="server" id="adminhotelid" type="hidden" />
    <input runat="server" id="HotelID" type="hidden" />
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
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


        selected('.mftable');  //调用 勾选启动
    </script>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script src="/SysSetBase/css/ScrollBar.js" type="text/javascript"></script>
    <script>



        $(function () {
            hotel();
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
                    var hftr = $("#hotelnameList").html();
                    $("#gmmfNav").empty();
                    for (var i = 0; i < json.length; i++) {
                        var copytr = hftr;
                        copytr = copytr.replace("{@hotelid}", json[i].ID);
                        copytr = copytr.replace("{@hotelname}", json[i].NAME);


                        $("#gmmfNav").append(copytr);

                    }
                }
            })


        }


        ///客房点击事件
        $("#gmmfNav").on('click', "dd", function () {
            $("#gmmfNav dd").removeClass("active");
            $(this).addClass("active");

            $("#HotelID").val($(this).attr("hotelid"));
            //            alert($("#HotelID").val());

            getInfo($(this).attr("hotelid"));
        });

        function getInfo(HotelID) {


            $.post("manfang.ashx?action=getinfo&hotelid=" + HotelID + "", function (data) {
                var json = eval("(" + data + ")");
                var hftr = $("#tr").html();
                $("#tb").empty();

                for (var i = 0; i < json.Full_house.length; i++) {
                    var copytr = hftr;
                    copytr = copytr.replace("{@id}", json.Full_house[i].ID);
                    copytr = copytr.replace("{@ids}", json.Full_house[i].ID);
                    copytr = copytr.replace("{@idd}", json.Full_house[i].ID);
                    copytr = copytr.replace("{@sy}", i + 1);
                    copytr = copytr.replace("{@kfname}", json.Full_house[i].FESTIVALNAME);
                    copytr = copytr.replace("{@kssj}", json.Full_house[i].STARTTIME);
                    copytr = copytr.replace("{@jssj}", json.Full_house[i].ENDTIME);
                    copytr = copytr.replace("{@ts}", json.Full_house[i].NUMBER);
                    $("#tb").append(copytr);


                }


            });


        }

        //添加
        function add() {

            var mid = $("#HotelID").val();
            if (mid != null && mid != "") {
                var url = "/RMBase/SysCalendar/Full_FestivalAdd.aspx?mid=" + mid;
                top.art.dialog.open(url, {
                    id: 'FestivalAdded',
                    title: '客房满房设置 > 添加满房',
                    width: 350,
                    height: 250,
                    close: function () {
                        window.location.reload();
                    }
                }, false);
            } else {
                showTipsMsg("请选择酒店！", 2000, 5);
            }
        }

        //修改
        function edit() {

            var hid = getid();
            hid = hid.substring(0, hid.length - 1);
            if (hid.split(",").length > 1) {
                showTipsMsg("当前选择了多行！", 2000, 5);
                return false;
            }

            if (hid != "") {
                var url = "/RMBase/SysCalendar/Full_FestivalAdd.aspx?ID=" + hid;
                top.art.dialog.open(url, {
                    id: 'FestivalAdded',
                    title: '客房满房设置 > 编辑满房',
                    width: 350,
                    height: 250,
                    close: function () {
                        window.location.reload();
                    }
                }, false);


            } else {
                showTipsMsg("请选择编辑行！", 2000, 5);
            }

        }

        function edits(hid) {


            if (hid != "") {
                var url = "/RMBase/SysCalendar/Full_FestivalAdd.aspx?ID=" + hid;
                top.art.dialog.open(url, {
                    id: 'FestivalAdded',
                    title: '客房满房设置 > 编辑满房',
                    width: 350,
                    height: 250,
                    close: function () {
                        window.location.reload();
                    }
                }, false);


            } else {
                showTipsMsg("请选择编辑行！", 2000, 5);
            }

        }

        // 删除
        function Delete() {
            var hid = getid();
            hid = hid.substring(0, hid.length - 1);
            if (hid != "") {
                var parm = 'type=Delete&ID=' + hid;
                getAjax('/RMBase/SysCalendar/house.ashx', parm, function (rs) {
                    if (parseInt(rs) == 1) {
                        //重新刷新
                        showTipsMsg("删除成功！", 2000, 4);
                        window.location.reload();
                    }
                });
            } else {
                showTipsMsg("请选择删除行！", 2000, 5);
            }
        }
        function Deletes(hid) {

            if (hid != "") {
                var parm = 'type=Delete&ID=' + hid;
                getAjax('/RMBase/SysCalendar/house.ashx', parm, function (rs) {
                    if (parseInt(rs) == 1) {
                        //重新刷新
                        showTipsMsg("删除成功！", 2000, 4);
                        window.location.reload();
                    }
                });
            } else {
                showTipsMsg("请选择删除行！", 2000, 5);
            }
        }

        function getid() {
            var id = "";
            $("#tab").find(".active").each(function () {
                id += $(this).attr("name") + ",";
            });
            return id;
        }

    </script>
</body>
</html>
