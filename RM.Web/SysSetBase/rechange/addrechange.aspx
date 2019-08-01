<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addrechange.aspx.cs" Inherits="RM.Web.SysSetBase.rechange.addrechange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:350px;">
        <ul class="rgeAlt" style="width:530px;max-height: 350px;padding-left:10px;">

             <li><small>酒店/门店</small>
                <div>
                    <asp:DropDownList ID="ddlHotel" runat="server" style="width:204px;">
                    </asp:DropDownList> 
                </div>
            </li>


            <li class="inp"><small>充值金额</small>
                <div>
                    <asp:TextBox ID="czje" runat="server" MaxLength="5" onkeyup="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>元
                </div>
            </li>
            <li class="inp" style="padding-top: 0;"><small>赠送</small>
                <div class="zssz">
                    <%-- --%><span><label>金额</label><asp:TextBox ID="zsje" runat="server" Text="0" MaxLength="5"
                        onkeyup="this.value=this.value.replace(/\D/g,'')"></asp:TextBox><input type="hidden"
                            id="iszsmoneys" runat="server" value="0" />元</span>
                    <%-- --%><span style=" "><label>积分</label><asp:TextBox ID="zsjf" runat="server" Text="0" MaxLength="5"
                        onkeyup="this.value=this.value.replace(/\D/g,'')"></asp:TextBox><input type="hidden"
                            id="iszsjf" runat="server" value="0" /></span>
                    <%-- --%><span style=" "><label>卡券</label>
                        <asp:DropDownList ID="yhq" runat="server">
                        </asp:DropDownList>
                        <input type="hidden" id="iscouponid" runat="server" value="0" />
                        <input type="hidden" id="couponids" runat="server" value="" />
                        <a class="btn" onclick="add(this)">添加</a>
                        <%--<a class="setup">设置</a></span>
                        <span class="shanchu">
                            <input type="text" cid='' disabled name="name" value="" style="background-color:#F7F7F7;" />
                            <a class="btn" onclick="del(this)">删除</a>
                        </span>
                        <span class="shanchu">
                            <input type="text" disabled name="name" value="" style="background-color:#F7F7F7;" />
                            <a class="btn" onclick="del()">删除</a>
                        </span>
                        <span class="shanchu">
                            <input type="text" disabled name="name" value="" style="background-color:#F7F7F7;" />
                            <a class="btn" onclick="del()">删除</a>
                        </span>--%>
                    <%-- --%><span style=" display:none"><label>会员卡</label>
                        <asp:DropDownList ID="hyjb" runat="server">
                        </asp:DropDownList>
                        <input type="hidden" id="ishylxcode" runat="server" value="0" />
                    </span>
                </div>
            </li>
            <li><small>充值说明</small>
                <div>
                    <textarea cols="30" rows="10" id="txtInfo" runat="server"></textarea>
                </div>
            </li>
        </ul>
    </div>
    <div class="adifoliBtn">
        <div style="float:right;">
            <asp:Button ID="btnSubmit" runat="server" Text="修改" OnClick="btnSubmit_Click" class="button" />
        </div>
    </div>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(".zssz").find("label").each(function () {
            if ($(this).next().next().val() * 1 == 1) {
                $(this).addClass('checked')
            } else {
                $(this).removeClass('checked');
            }
        });

        $('.zssz').on('click', 'label', function () {
            $(this).hasClass('checked') ? $(this).removeClass('checked').next().next().val("0") : $(this).addClass('checked').next().next().val("1");
        });

        function add(obj) {
            //alert($("#couponids").val().indexOf($("#yhq").val() + ","));
            if ($("#couponids").val().indexOf($("#yhq").val() + ",") > 0) {
                alert('已添加该卡券');
                return;
           }

            var html = "<span class='shanchu'><input type='text' cid='" + $("#yhq").val() + "' disabled name='name' value='" + $("#yhq option:checked").text() + "' style='background-color:#F7F7F7;' /><a class='btn' onclick='del(this)'>删除</a></span>";
            $(obj).parent().after(html);
            $("#couponids").val($("#couponids").val() + $("#yhq").val() + ",");
            //alert($("#couponids").val());
        }

        function del(obj) {


            $(obj).parent().remove();

            $("#couponids").val($("#couponids").val().replace(($(obj).parent().find("input").eq(0).attr("cid")+","),""));

            //alert($("#couponids").val());
        }
        

    </script>
</body>
</html>
