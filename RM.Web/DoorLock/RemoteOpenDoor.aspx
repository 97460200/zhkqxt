<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemoteOpenDoor.aspx.cs" Inherits="RM.Web.DoorLock.RemoteOpenDoor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>房间开门</title>
    <meta content="width=device-width,initial-scale=1,user-scalable=no,viewport-fit=cover"
        name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link href="/App_Themes/default/css/Total.css" rel="stylesheet" type="text/css" />
    <link href="/App_Themes/default/css/regroupadd.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="RemoteOpenDoor">
        <div class="RemoteOpenDoort">
            
        </div>
        <div class="RemoteOpenDoorb">
            <div class="kmmm clearfix">
                <div class="kmmml">
                    <input type="text" name="name" value="" placeholder="请输入关键字..." />
                    <i class="mmxs"></i>
                </div>
                <div class="kmmmr act">
                    记住密码
                </div>
            </div>
            <div class="number">
                <ul class="clearfix">
                    <li>1</li>
                    <li>2</li>
                    <li>3</li>
                    <li>4</li>
                    <li>5</li>
                    <li>6</li>
                    <li>7</li>
                    <li>8</li>
                    <li>9</li>
                    <li>删除</li>
                    <li>0</li>
                    <li>确定</li>
                </ul>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
