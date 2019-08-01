<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuestRoom.aspx.cs" Inherits="RM.Web.SysSetBase.GuestRoom.GuestRoom" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客房设置</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="gtrm">
        <div class="gmNav" id="nav">
            <a url="kefang.aspx" class="active">客房设置</a><a url="manfang.aspx">满房设置</a><a url="jri.aspx">节假日设置</a>
        </div>
        <iframe id="gtIfe" frameborder="0" width="100%" height="100%" src="kefang.aspx" scrolling="no">
        </iframe>
    </div>
    <div class="Mask" id="msk">
    </div>
    </form>
    <script type="text/javascript">
        nav.onclick = function (ev) {
            if (ev.target.className == 'active' || ev.target.className == 'gmNav') return;

            var gtIfe = document.getElementById('gtIfe');
            var A = this.getElementsByTagName('a');
            for (var i = 0; i < A.length; i++) { A[i].className = ''; }
            ev.target.className = 'active';
            gtIfe.setAttribute('src', ev.target.getAttribute('url'));
        };
        var zz = document.getElementById('msk');
        var msk = function () {
            zz.style.display = zz.style.display == 'block' ? 'none': 'block';
        }
    </script>
</body>
</html>
