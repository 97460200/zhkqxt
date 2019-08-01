<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserTag.aspx.cs" Inherits="WxCallback.UserTag" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .AdminHotelId
        {
            display: inline-block;
            width: 80px;
        }
        .Name
        {
            display: inline-block;
            width: 320px;
        }
        .users
        {
            display: inline-block;
            width: 80px;
        }
        .tag_user
        {
            display: inline-block;
            width: 80px;
        }
    </style>
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function set_tag(AdminHotelId) {
            alert(AdminHotelId);
            $.post("/api/wx_api.ashx?action=set_tags_all&AdminHotelId=" + AdminHotelId, function () {

            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div runat="server" id="userIsTag">
    </div>
    </form>
</body>
</html>
