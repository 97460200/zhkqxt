<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="setcard.aspx.cs" Inherits="RM.Web.SysSetBase.sales.setcard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>设置展牌</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>              <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ke-container
        {
            width:370px !important;
            border-right:1px solid #CCCCCC !important;
            }
        .ke-toolbar {
	        display: none !important;
        }
    </style>

        <link href="../../kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../kindeditor/plugins/code/prettify.css" />
    <script charset="utf-8" src="../../kindeditor/kindeditor.js"></script>
    <script charset="utf-8" src="../../kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" src="../../kindeditor/plugins/code/prettify.js"></script>
    <script>
        KindEditor.ready(function (K) {
            var editor1 = K.create('#txtExtension', {
                cssPath: '../../kindeditor/plugins/code/prettify.css',
                uploadJson: '../../kindeditor/asp.net/upload_json.ashx',
                fileManagerJson: '../../kindeditor/asp.net/file_manager_json.ashx',
                allowFileManager: false,
                allowImageRemote: false,
                afterCreate: function () {
                    var self = this;
                    K.ctrl(document, 13, function () {
                        self.sync();
                        K('form[name=example]')[0].submit();
                    });
                    K.ctrl(self.edit.doc, 13, function () {
                        self.sync();
                        K('form[name=example]')[0].submit();
                    });
                }
            });
            prettyPrint();
        });
    </script>


    <script type="text/javascript">

            function inputPath(fu) {

                if (fu != '') {
                    document.getElementById('photo').style.display = "block";
                    document.getElementById('imgPicture').style.display = "block";
                    //document.getElementById('imgFristPicture').style.display = "block";
                    var input2 = document.getElementById("a"); // 获取 input 对象 input2
                    var src = document.getElementById('fuPicture').files[0];
                    document.getElementById('imgPicture').src = window.URL.createObjectURL(src);
                    //document.getElementById('imgFristPicture').src = window.URL.createObjectURL(src);
                    document.getElementById('hfImage').value = fu.value;
                    input2.value = fu.value;
                }
                else {
                    document.getElementById('imgPicture').src = "";
                    //document.getElementById('imgFristPicture').src = "";
                    document.getElementById('photo').style.display = "none";
                    document.getElementById('imgPicture').style.display = "none";
                    //document.getElementById('imgFristPicture').style.display = "none";
                    document.getElementById('hfImage').value = "";
                }
            }
            //保存图片
            function CheckValid() {

                if ($('#hfImage').val() == '') {
                    showTipsMsg("请上传酒店LOGO！", 3000, 3);
                    return false;
                }

                if ($('#txtHotelNameCode').val() == '') {
                    showTipsMsg("请输入酒店名称！", 3000, 3);
                    $('#txtHotelNameCode').focus();
                    return false;
                }

          
             
            }

        </script>
</head>
<body>
    <form id="form1" runat="server">
        <input runat="server" type="hidden" id="Hdhoteladmin" />
    <div class="clearfix" style="width:740px;height:400px;">
    
        <dl class="adifoli addcard">
            <dd>
                <small style="font-family:”Microsoft YaHei”">LOGO</small>
                <div class="addlogo" onclick="$('input[id=fuPicture]').click()">
                    +
                </div>
                <div class="addlogo01"  id="photo" runat="server" style="display: none">
                    <img   id="imgPicture" runat="server" src="../img/ewm.png" alt="Alternate Text" />
                </div>
                  <asp:FileUpload ID="fuPicture" onchange="inputPath(this)" runat="server" Style="border: none;
                    display: none; width: 57px;" />
                 <asp:HiddenField ID="hfImage" runat="server" />
            </dd>

            <dd>
                <small>酒店名称</small>
                <div class="bt">
                  <input type="text" id="txtHotelNameCode" runat="server" />

                </div>
            </dd>

            <dd>
                <small>简介</small>
                <div class="bt">
                    <textarea name="Extension" wrap="hard" id="txtExtension"  runat="server" onkeyup="onkeyAdvert(this.value)" cols="30" rows="10" placeholder="" style="width:380px;"></textarea>
                </div>
            </dd>
        </dl>
        
        <div class="zpyl">
            <div class="d1">
                展牌预览
            </div>
            <div class="d2">
                <div class="d21">
                    <img src="../img/ewm.png" id="HotelLogo" runat="server"  alt="Alternate Text" />
                </div>

                <div class="">
                    <asp:Label ID="lblHotelNameCode" runat="server"></asp:Label>  
                </div>

                <div class="d22">
                    <img  id="HotelCode"  runat="server"  src="../img/sewaewm.jpg" alt="Alternate Text" />
                    <img id="imgFristPicture" runat="server"  src="../img/ewm.png"  alt="Alternate Text" class="ewmzx"/>
                </div>
                <div class="d23" id="lblAdvertising" runat="server">
                    通过扫描维码<br />                    关注“XX酒店”微信公众号<br />                    首次入住立减40元<br />                  （另送早餐）   
                </div>
        </div>
        </div>
    </div>
    
    <div class="adifoliBtn">
        <div style="float: right;">
            <asp:Button ID="btnSumit" runat="server" Text="保存并预览"  OnClientClick="return CheckValid()" OnClick="btnSumit_Click" />
            <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();"><span>关 闭</span></a>
        </div>
    </div>
    </form>
</body>
</html>
