<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addcolu.aspx.cs" Inherits="RM.Web.SysSetBase.Busin.addcolu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>营业点管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
         <script src="../../App_Themes/default/js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/admin/js/button.js"></script>

     <script language="javascript" type="text/javascript">


         function check() {

             if ($('#txtTypeName').val() == '') {
                 showTipsMsg("请输入栏目名称！", 3000, 3);
                 $('#txtTypeName').focus();
                 return false;
             }

             return true;
         }

  
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <dl class="adifoli colu">
        <dd>
            <small>栏目名称</small>
            <div>
                <input type="text" id="txtTypeName" runat="server" style="width: 100%;" />
            </div>
        </dd>

        <dd>
            <small>说明</small>
            <div>
                <textarea cols="30"  id="txtInstructions" runat="server" rows="10"></textarea>
            </div>
        </dd>
    </dl>
    <div class="adifoliBtn">
     <asp:Button ID="btnSubmit" runat="server" Text="提交" OnClientClick="return check()"  OnClick="btnSubmit_Click" />

    </div>
    </form>
</body>
</html>
