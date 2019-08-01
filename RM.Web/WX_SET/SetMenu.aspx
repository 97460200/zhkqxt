<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetMenu.aspx.cs" Inherits="RM.Web.WX_SET.SetMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Themes/js/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ButtonJson() {
            var button1 = GetButton("button1");
            var button2 = GetButton("button2");
            var button3 = GetButton("button3");
            $("#hdMenu").val("{" + button1 + "," + button2 + "," + button3 + "}");
        }

        function GetButton(btcs) {
            var button = "";
            $(".sub_button ." + btcs + " .name").each(function () {
                if ($(this).val() != "") {
                    var obj_btn = new Object();
                    obj_btn.type = "view";
                    obj_btn.name = $(this).val();
                    obj_btn.url = $(this).parent().find(".url").val();
                    var jsonDate = JSON.stringify(obj_btn);
                    if (button == "") {
                        button = jsonDate;
                    } else {
                        button = button + "," + jsonDate;
                    }
                }
            });

            if (button == "") {
                var btn1 = new Object();
                btn1.type = "view";
                btn1.name = $("#" + btcs + "_name").val();
                btn1.url = $("#" + btcs + "_url").val();
                var btn1_json = JSON.stringify(btn1);
                button = '"button":[' + btn1_json + ']';
            } else {
                button = '"button":[{"name":"' + $("#" + btcs + "_name").val() + '","sub_button":[' + button + ']}]';
            }
            return button;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdMenu" />
    <input runat="server" type="hidden" id="hdAdminHotelid" />
    <div onclick="ButtonJson()">
        <span>酒店</span> <span>A</span>
    </div>
    <div id="b1">
    </div>
    <div id="b2">
    </div>
    <div id="b3">
    </div>
    <div>
        <table>
            <tr>
                <td>
                    菜单名称/连接
                </td>
                <td>
                    菜单名称/连接
                </td>
                <td>
                    菜单名称/连接
                </td>
            </tr>
            <tr class="sub_button">
                <td class="button1">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button2">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button3">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
            </tr>
            <tr class="sub_button">
                <td class="button1">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button2">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button3">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
            </tr>
            <tr class="sub_button">
                <td class="button1">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button2">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button3">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
            </tr>
            <tr class="sub_button">
                <td class="button1">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button2">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button3">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
            </tr>
            <tr class="sub_button">
                <td class="button1">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button2">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button3">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
            </tr>
            <tr class="sub_button">
                <td class="button1">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button2">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
                <td class="button3">
                    <input class="name" value="" />
                    <input class="url" value="" />
                </td>
            </tr>
            <tr class="button">
                <td class="button1">
                    <input id="button1_name" class="name" value="酒店预订" />
                    <input id="button1_url" class="url" value="" />
                </td>
                <td class="button2">
                    <input id="button2_name" class="name" value="会员尊享" />
                    <input id="button2_url" class="url" value="" />
                </td>
                <td class="button3">
                    <input id="button3_name" class="name" value="更多服务" />
                    <input id="button3_url" class="url" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Button ID="btnCreateMenu" runat="server" Text="创建菜单" OnClick="btnCreateMenu_Click"
        OnClientClick="ButtonJson()" />
    <asp:Label ID="lblResult" runat="server" Text="结果"></asp:Label>
    </form>
</body>
</html>
