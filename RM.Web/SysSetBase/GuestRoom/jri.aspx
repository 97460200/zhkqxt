<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jri.aspx.cs" Inherits="RM.Web.SysSetBase.GuestRoom.jri" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=8,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/App_Themes/admin/js/button.js"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script language="javascript" src="/WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="gtall gmjri" style="width: 820px;">
        <div class="wdyTle">
            <div class="wdyhd">
                <div class="r">
                    <span onclick="add();">添加</span> <span onclick="edit();">修改</span> <span onclick="Rm.F_delect();">删除</span>
                </div>
            </div>
        </div>
        <div class="wdytList">
            <div class="wdytLeft">
                <b>选择年份</b>
                <ul>
                    <%for (int i = 0; i < 4; i++)
                      {%>
                    <li onclick="GetTable(<%=DateTime.Now.Year+i %>)">
                        <%=DateTime.Now.Year + i%>
                    </li>
                    <% } %>
                    <%--<li>2018</li>
                    <li class="active">2017</li>
                    <li>2016</li>
                    <li>2015</li>--%>
                </ul>
            </div>
            <div class="wdytRight">
                <div class="wdytable">
                    <table class="ul" id="tab">
                        <thead>
                            <tr>
                                <th width="40">
                                    <i class="icon-radio6"></i>
                                </th>
                                <th>
                                    节假名称
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
                        <tbody>
                            <asp:Repeater ID="GV_mygvInfo" runat="server">
                                <ItemTemplate>
                                    <tr name='<%#Eval("ID") %>'>
                                        <td>
                                            <i class="icon-radio6"></i>
                                        </td>
                                        <td>
                                            <%#Eval("FestivalName")%>
                                        </td>
                                        <td>
                                            <%#Eval("StartTime","{0:yyyy-MM-dd}")%>
                                        </td>
                                        <td>
                                            <%#Eval("EndTime","{0:yyyy-MM-dd}")%>
                                        </td>
                                        <td>
                                            <%#Eval("Number")%>
                                        </td>
                                        <td>
                                            <i class="icon-edit4" onclick="edits('<%#Eval("ID") %>');"></i><i class="icon-lbx" onclick="Rm.F_delect('<%#Eval("ID") %>')"></i>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <!--弹窗-->
    <div class="Alert" id="add">
        <h2 onclick="Rm.F_alert();">
            编辑节假日 <i class="icon-boldclose"></i>
        </h2>
        <div class="altconX">
            <div id="divRemark" class="popup" style="width: 350px;">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="text-align: right">
                            节日名称
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" Width="200px" MaxLength="6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            开始日期
                        </td>
                        <td>
                            <asp:TextBox ID="txtBeginTime" Width="200px" runat="server" onfocus="new WdatePicker();show2()"
                                class="Wdate"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            截止日期
                        </td>
                        <td>
                            <asp:TextBox ID="txtEndTime" Width="200px" runat="server" onfocus="new WdatePicker();show2()"
                                class="Wdate"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            天数
                        </td>
                        <td>
                            <span id="jiwan" runat="server">0天</span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="altBtn">
            <a onclick="cz()">重置</a><a class="active" onclick="Rm.F_alert('sure');">提交</a>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        function checkInput() {
            var price = document.getElementById("txtName");
            if (price.value == "") {
                alert("请输入节日名称！");
                price.focus();
                return false;
            }
            var price1 = document.getElementById("txtBeginTime");
            if (price1.value == "") {
                alert("请输入开始时间！");
                price1.focus();
                return false;
            }
            var price2 = document.getElementById("txtEndTime");
            if (price2.value == "") {
                alert("请输入结束时间！");
                price2.focus();
                return false;
            }
            var startTime = new Date(Date.parse(price1.value.replace(/-/g, "/"))).getTime();
            var endTime = new Date(Date.parse(price2.value.replace(/-/g, "/"))).getTime();
            if (startTime >= endTime) {
                alert("开始日期不能大于截止日期");
                return false;
            }

            return true;
        }

        function show2() {
            var a = document.getElementById("txtBeginTime").value;
            var b = document.getElementById("txtEndTime").value;

            document.getElementById("jiwan").innerText = GetDateDiff(a, b) + "天";
        }
        function GetDateDiff(startDate, endDate) {
            var startTime = new Date(Date.parse(startDate.replace(/-/g, "/"))).getTime();
            var endTime = new Date(Date.parse(endDate.replace(/-/g, "/"))).getTime();
            var dates;
            if (endTime >= startTime) {
                dates = Math.abs((startTime - endTime)) / (1000 * 60 * 60 * 24);
            } else {
                return 0;
            }
            return dates;
        }

        function cz() {
            var price = document.getElementById("txtName");
            var price1 = document.getElementById("txtBeginTime");
            var price2 = document.getElementById("txtEndTime");
            price.value = "";
            price1.value = "";
            price2.value = "";
        }
        
    </script>
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
        selected('.wdytable');


        var type;
        var ids;
        var Rm = {
            checkyear: function () { $(this).addClass('active').siblings().removeClass('active') },

            F_alert: function (Selector) {
                if (typeof Selector === 'undefined' || Selector == 'sure') {
                    $('.Alert').fadeOut(120);
                    if (Selector == 'sure') {
                        if (type == "#add") {
                            this.F_add();
                        } else if (type == "#update") {

                            this.F_update(ids);
                        }
                    }
                } else if (Selector == "#add") {
                    type = Selector;
                    $("#add").fadeIn(120);
                    this.curr = Selector;

                } else if (Selector == "#update") {
                    type = Selector;
                    var hid = this.F_getid();
                    hid = hid.substring(0, hid.length - 1);
                    if (hid == "") {
                        alert("当前未选择任何一行");
                        return false;
                    } else if (hid.split(",").length > 1) {
                        alert("当前选择了多行");
                        return false;
                    } else {
                        ids = hid;
                        this.F_getinfo(hid);
                    }

                    $("#add").fadeIn(120);
                    this.curr = Selector;
                }
            },
            F_sure: function () {
                this.F_tip('当前选择器是' + this.curr);
            },
            F_tip: function (str) {
                $('.tip').html(str).fadeIn(120);
                setTimeout(function () { $('.tip').fadeOut(120); }, 2000);
            },
            F_add: function () {

                var price = document.getElementById("txtName");
                var price1 = document.getElementById("txtBeginTime");
                var price2 = document.getElementById("txtEndTime");

                if (checkInput()) {

                    $.post("jri.ashx?action=addjr&Name=" + price.value + "&BeginTime=" + price1.value + "&EndTime=" + price2.value, function (data) {

                        if (data == "ok") {
                            alert("添加成功!");
                            window.location.reload();
                        }
                        else {
                            alert("添加失败!");
                        }

                    });

                }
            },
            F_update: function (id) {

                var price = document.getElementById("txtName");
                var price1 = document.getElementById("txtBeginTime");
                var price2 = document.getElementById("txtEndTime");

                if (checkInput()) {

                    $.post("jri.ashx?action=updatejr&ID=" + id + "&Name=" + price.value + "&BeginTime=" + price1.value + "&EndTime=" + price2.value, function (data) {

                        if (data == "ok") {
                            alert("修改成功!");
                            window.location.reload();
                        }
                        else {
                            alert("修改失败!");
                        }

                    });

                }
            },
            F_getid: function () {
                var id = "";
                $("#tab").find(".active").each(function () {
                    id += $(this).attr("name") + ",";
                });
                return id;
            },
            F_getinfo: function (id) {
                var price = document.getElementById("txtName");
                var price1 = document.getElementById("txtBeginTime");
                var price2 = document.getElementById("txtEndTime");
                $.post("jri.ashx?action=getinfo&ID=" + id + "", function (data) {
                    var json = eval("(" + data + ")");
                    price.value = json.Holiday[0].FESTIVALNAME;
                    price1.value = formatDate(json.Holiday[0].STARTTIME);
                    price2.value = formatDate(json.Holiday[0].ENDTIME);
                    document.getElementById("jiwan").innerText = json.Holiday[0].NUMBER + "天";
                });
            },
            F_delect: function (id) {
                
                if (typeof id == "undefined") {
                    var hid = this.F_getid();
                    id = hid.substring(0, hid.length - 1);
                }
                var parm = 'type=Delete&ID=' + id;
                getAjax('/RMBase/SysCalendar/choice.ashx', parm, function (rs) {
                    if (parseInt(rs) == 1) {
                        //重新刷新
                        alert("删除成功！");
                        window.location.reload();
                    }
                });



            }

        }
        $('.wdytLeft').on('click', 'li', Rm.checkyear);



        //根据条件查询假日
        function GetTable(year) {

            var parm = 'type=Select&year=' + year;
            getAjax('/RMBase/SysCalendar/choice.ashx', parm, function (rs) {
                //清空所有行
                $("#tab tbody").html("");
                //重新加载

                var obj = eval(rs);
                var str = "";
                $(obj).each(function (index) {

                    var val = obj[index];
                    str += "<tr name='" + val.ID + "'>";
                    str += "<td><i class='icon-radio6'></i></td>";
                    str += " <td >" + val.FESTIVALNAME + "</td>";
                    str += " <td >" + formatDate(val.STARTTIME) + "</td>";
                    str += " <td >" + formatDate(val.ENDTIME) + "</td>";
                    str += " <td >" + val.NUMBER + "</td>";
                    str += "<td ><i class='icon-edit4' onclick='edits(" + val.ID + ")'></i><i class='icon-lbx'></i></td>";
                    str + "</tr>";
                });

                $("#tab tbody").append(str);
            });
        }
        function formatDate(date) {
            var d = new Date(date),
    month = '' + (d.getMonth() + 1),
    day = '' + d.getDate(),
    year = d.getFullYear();

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;

            return [year, month, day].join('-');
        }

        function updates(id) {
            ids = id;
            type = "#update";
            Rm.F_getinfo(ids);
            $("#add").fadeIn(120);
        }



        //添加
        function add() {
            var url = "/RMBase/SysCalendar/FestivalAdded.aspx";
            top.art.dialog.open(url, {
                id: 'FestivalAdded',
                title: '客房参数设置 > 添加节假日',
                width: 350,
                height: 250,
                close: function () {
                    window.location.reload();
                }
            }, false);

        }

        //修改
        function edit() {
            var hid = Rm.F_getid();
            hid = hid.substring(0, hid.length - 1);
            if (hid.split(",").length > 1) {
                showTipsMsg("当前选择了多行！", 2000, 5);
              
                return false;
            }
            if (hid != "") {
                var url = "/RMBase/SysCalendar/FestivalAdded.aspx?ID=" + hid;
                top.art.dialog.open(url, {
                    id: 'FestivalAdded',
                    title: '客房参数设置 > 编辑节假日',
                    width: 350,
                    height: 250,
                    close: function () {
                        window.location.reload();
                    }
                }, false);

            }
            else {
                showTipsMsg("请选择编辑行！", 2000, 5);
            }

        }

        function edits(hid) {
            
            if (hid != "") {
                var url = "/RMBase/SysCalendar/FestivalAdded.aspx?ID=" + hid;
                top.art.dialog.open(url, {
                    id: 'FestivalAdded',
                    title: '客房参数设置 > 编辑节假日',
                    width: 350,
                    height: 250,
                    close: function () {
                        window.location.reload();
                    }
                }, false);

            }
            else {
                showTipsMsg("请选择编辑行！", 2000, 5);
            }

        }

    </script>
</body>
</html>
