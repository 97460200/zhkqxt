<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberAdd.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.MemberAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加会员</title>
    <link href="../../SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="../../SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/admin/js/button.js"></script>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
        <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script type="text/javascript">
        function checkData() {

            if ($("#txtName").val() == "") {
                showTipsMsg('请输入真实姓名!', '5000', '5');
                return false;
            }
            if ($("#txtPhone").val() == "") {
                showTipsMsg('请输入手机号码!', '5000', '5');
                return false;
            }

            if ($('#ddlMemberLevel').val() == null) {
                showTipsMsg("请先添加会员级别！", '5000', '5');
                $('#ddlMemberLevel').focus();
                return false;
            }

            if ($('#ddlMemberLevel').val() == '') {
                showTipsMsg("请选择会员级别！", '5000', '5');
                $('#ddlMemberLevel').focus();
                return false;
            }

            if ($("#txtCreateTime").val() == "") {
                showTipsMsg('请选择创建时间!', '5000', '5');
                return false;
            }
        }
        </script>
</head>
<body>
    <form id="form1" runat="server">
    
    <dl class="adifoli" style="max-height: 320px;height: 320px;width: 417px;overflow: auto;">
        <dd>
            <small>真实姓名</small>
            <div>
               <input type="text" style="width:224px;" id="txtName" runat="server">
            </div>
        </dd>

        <dd>
            <small>手机号码</small>
            <div>
               <input type="text" style="width:224px;" id="txtPhone" runat="server">
            </div>
        </dd>

        <dd>
            <small>会员级别</small>
            <div>
                <asp:DropDownList ID="ddlMemberLevel" runat="server" Width="224px">
                </asp:DropDownList>
            </div>
        </dd>

        <dd>
            <small>创建时间</small>
            <div>
             <input id="txtCreateTime" runat="server"  type="text" placeholder="2018-01-01 06:00"
                        onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd HH:mm'})" style="width: 224px;" class="date"
                        autocomplete="off" />
            </div>
        </dd>

        <dd>
            <small>备注</small>
            <div>
                <textarea style="width:224px;height:70px;" id="txtNote" runat="server"></textarea>
            </div>
        </dd>
    </dl>

    <div class="adifoliBtn">
        <div style="float:right;">
            <asp:Button ID="btnSubmit" runat="server" Text="确定" OnClick="btnSubmit_Click"  OnClientClick="return checkData()" class="button" />
        </div>
    </div>
    </form>

</body>
</html>
