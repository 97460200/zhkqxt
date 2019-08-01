<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectsize.aspx.cs" Inherits="RM.Web.SysSetBase.sales.selectsize" %>

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
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="selectsize">
        <table border="0" cellpadding="0" cellspacing="0">
            <thead>
                <tr>
                    <th>
                        二维码边长（cm）
                    </th>
                    <th>
                        建议扫描距离（米）
                    </th>
                    <th>
                        下载链接
                    </th>
                </tr>
            </thead>
            <tr>
                <td>
                    8cm
                </td>
                <td>
                    0.5m
                </td>
                <td>
                    <i class="icon-cloadown"></i>
                </td>
            </tr>
            <tr>
                <td>
                    12cm
                </td>
                <td>
                    0.8m
                </td>
                <td>
                    <i class="icon-cloadown"></i>
                </td>
            </tr>
            <tr>
                <td>
                    15m
                </td>
                <td>
                    1cm
                </td>
                <td>
                    <i class="icon-cloadown"></i>
                </td>
            </tr>
            <tr>
                <td>
                    30cm
                </td>
                <td>
                    1.5m
                </td>
                <td>
                    <i class="icon-cloadown"></i>
                </td>
            </tr>
            <tr>
                <td>
                    50cm
                </td>
                <td>
                    2.5m
                </td>
                <td>
                    <i class="icon-cloadown"></i>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
