<%@ Page Title="主页" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WxCallback._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <br />
    <input runat="server" id="txtHotelCode" type="text" />
    <div runat="server" id="dTest">
    </div>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var key = $("#MainContent_txtHotelCode").val();
            $.ajax({
                type: "POST",
                url: "/PMS/api.ashx?action=Submit&path=/PMS/Room/GetRoomPriceCode",
                data: { HotelCode: key },
                dataType: "JSON",
                success: function (data) {
                    var strData = JSON.stringify(data);
                    $("#MainContent_dTest").html(strData);
                },
                error: function (message) {
                    alert("提交数据失败！");
                }
            });
        });
    </script>
</asp:Content>
