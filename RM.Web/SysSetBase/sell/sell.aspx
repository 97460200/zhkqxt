<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sell.aspx.cs" Inherits="RM.Web.SysSetBase.sell.sell" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>分销设置</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="sll">
        <b class="tx">设置会员分销方式、规则和奖励</b>
        <div class="setion">
            <div class="byfig clearfix">
                <span>分销功能</span>
                <label name="fx_function" hid="1">
                    开启</label>
                <label name="fx_function" hid="0" class="active">
                    关闭</label>
            </div>
            <ul class="sllList">
                <li><b>分销规则</b>
                    <div class="alyt">
                        <table>
                            <thead>
                                <tr>
                                    <th>
                                        注册
                                    </th>
                                    <th>
                                        评价
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <div>
                                            <label name="fx_internal_jf" hid="0" class="active">
                                                无</label>
                                            <label name="fx_internal_jf" hid="2">
                                                积分</label>
                                            <label name="fx_internal_jf" hid="3">
                                                卡券</label>
                                        </div>
                                        <div id="fx_internal_jf2_div" style="display: none">
                                            <div id="jf2_je_div" style="display: none">
                                                <input type="text" /><i>元</i>
                                            </div>
                                            <div id="jf2_jf_div" style="display: none">
                                                <input type="text" /><i>分</i>
                                            </div>
                                            <div id="jf2_kq_div" style="display: none">
                                                <select>
                                                    <option value="0">50元卡券</option>
                                                </select>
                                                <a>管理</a>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <label name="fx_internal_kq" hid="0" class="active">
                                                无</label>
                                            <label name="fx_internal_kq" hid="2">
                                                积分</label>
                                            <label name="fx_internal_kq" hid="3">
                                                卡券</label>
                                        </div>
                                        <div id="fx_internal_kq2_div" style="display: none">
                                            <div id="kq2_je_div" style="display: none">
                                                <input type="text" /><i>元</i>
                                            </div>
                                            <div id="kq2_jf_div" style="display: none">
                                                <input type="text" /><i>分</i>
                                            </div>
                                            <div id="kq2_kq_div" style="display: none">
                                                <select>
                                                    <option value="0">50元卡券</option>
                                                </select>
                                                <a>管理</a>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </li>
            </ul>
        </div>
        <div class="poBtn">
            <a class="button buttonActive" id="btnyes">保存</a>
        </div>
    </div>
    <div class="tip">
    </div>
    <%--判断是插入还是更新 --%>
    <input runat="server" type="hidden" id="isid" value="0" />
    <%--分销功能是否开启--%>
    <input runat="server" type="hidden" id="fx_function" value="0" />
    <input runat="server" type="hidden" id="AdminHotelid" value="0" />
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $('.sll').on('click', 'label', function () {
            var name = $(this).attr("name");
            var hid = $(this).attr("hid");
            $(this).addClass('active').siblings().removeClass('active');

            switch (name) {
                case "fx_function":
                    $("#fx_function").val(hid);
                    break;
                case "is_reward":
                    break;
                default:
                    break;
            }
        })

   
    
    </script>
    <script>
        $("#btnyes").click(function () {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "save_setfx",
                    isid: $("#isid").val(),
                    adminhotelid: $("#AdminHotelid").val(),
                    isopen: $("#fx_function").val()  //是否开启分销功能                
                },
                type: "POST",
                dataType: "text",
                async: false,
                success: function (data) {
                    if (data == "0") {
                        Tip("保存失败！");
                    } else {
                        Tip("保存成功！");
                    }
                }
            });
        });

        $(function () {
            show_fx();
        });

        function show_fx() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "Show_setfx",
                    adminhotelid: $("#AdminHotelid").val()
                },
                type: "POST",
                dataType: "text",
                async: false,
                success: function (data) {
                    if (data == "") {
                        return;
                    }
                    var json = eval("(" + data + ")");
                    for (var i = 0; i < json.length; i++) {
                        var id = json[i].ID;
                        $("#isid").val(id);
                        var isopen = json[i].ISOPEN;
                        $("#fx_function").val(isopen);
                        if (isopen == $("label[name=fx_function]").eq(0).attr("hid")) {
                            $("label[name=fx_function]").eq(0).addClass('active').siblings().removeClass('active');
                        }

                    }
                }
            });
        }
    </script>
</body>
</html>
