<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xghydj.aspx.cs" Inherits="RM.Web.SysSetBase.xitongcanshu.xghydj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改会员等级</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/js/button.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="text" id="hid" runat="server" style="display: none" />
    <dl class="addevaluate xghydj" style="height: 400px">
        <dd>
            <small>国光会员等级</small>
            <div>
                <asp:DropDownList ID="DDLgghy" runat="server" Style="width: 216px;">
                </asp:DropDownList>
            </div>
        </dd>
        <dd>
            <small>会员等级名称</small>
            <div>
                <asp:TextBox ID="hyname" runat="server" MaxLength="30" Style="width: 216px;"></asp:TextBox>
            </div>
        </dd>
        <dd>
            <small>会员等级级别</small>
            <div>
                <asp:DropDownList ID="hyjb" runat="server" Style="width: 216px;">
                    <asp:ListItem Text="一级" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="二级" Value="2"></asp:ListItem>
                    <asp:ListItem Text="三级" Value="3"></asp:ListItem>
                    <asp:ListItem Text="四级" Value="4"></asp:ListItem>
                    <asp:ListItem Text="五级" Value="5"></asp:ListItem>
                    <asp:ListItem Text="六级" Value="6"></asp:ListItem>
                    <asp:ListItem Text="七级" Value="7"></asp:ListItem>
                </asp:DropDownList>
                <span>会员卡级别中一级为最高级别</span>
            </div>
        </dd>
        <dd style="display: none;">
            <small>会员卡价格</small>
            <div>
                <asp:TextBox ID="hykje" runat="server" Width="100px" MaxLength="5" onkeyup="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
            </div>
        </dd>
        <dd>
            <small>会员卡背景</small>
            <div>
                <a href="#" class="btn" onclick="$('input[id=fuPicture]').click()">上传 </a>
                <asp:FileUpload ID="fuPicture" onchange="inputPath(this)" runat="server" Style="border: none;
                    display: none" />
                <asp:HiddenField ID="hfImage" runat="server" />
                <div class="hykbj clearfix">
                    <div class="hykbjl">
                        <%--<img src="../img/ewm.png">--%>
                        <img id="imgPicture" runat="server" src="../img/ewm.png" />
                    </div>
                    <div class="hykbjc">
                        <span>预览</span>
                    </div>
                    <div class="hykbjr">
                        <img src="../img/ewm.png" id="imgPicture1" runat="server">
                    </div>
                    <div class="hykbja">
                        <div class="arrow-left arrow-box tb">
                            <b class="left"><i class="left-arrow1"></i><i class="left-arrow2"></i></b>
                        </div>
                    </div>
                </div>
                <p>
                    建议上传580*320px图片
                </p>
            </div>
        </dd>
        <dd>
            <small>说明</small>
            <div>
                <textarea id="sm" runat="server"></textarea>
            </div>
        </dd>
    </dl>
    <div class="adifoliBtn">
        <div style="float: right;">
            <asp:Button ID="btnSubmit" runat="server" Text="编辑" class="button_input_come" OnClick="btnSubmit_Click"
                Style="margin-right: 5px;" />
            <asp:Button ID="btnCancel" runat="server" Text="重置" OnClientClick=" return clck()" />
        </div>
    </div>
    </form>
    <script>


        function inputPath(fu) {

            if (fu != '') {
                //document.getElementById('imgPicture').style.display = "block";
                var input2 = document.getElementById("a"); // 获取 input 对象 input2
                var src = document.getElementById('fuPicture').files[0];
                document.getElementById('imgPicture').src = window.URL.createObjectURL(src);
                document.getElementById('imgPicture1').src = window.URL.createObjectURL(src);
                document.getElementById('hfImage').value = fu.value;
                input2.value = fu.value;
            }
            else {
                document.getElementById('imgPicture').src = "";
                //document.getElementById('imgPicture').style.display = "none";
                document.getElementById('hfImage').value = "";
            }
        }
    </script>
</body>
</html>
