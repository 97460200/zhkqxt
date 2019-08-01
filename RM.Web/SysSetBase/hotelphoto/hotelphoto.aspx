<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hotelphoto.aspx.cs" Inherits="RM.Web.SysSetBase.hotelphoto.hotelphoto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>酒店图片</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <link href="jquery.dad.css" rel="stylesheet" type="text/css" />
    <script src="jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="jquery.dad.min.js" type="text/javascript"></script>
    <style type="text/css">
        .item
        {
            float: left;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            if ($("#HotelTree").is(':visible')) {
                $(".sharebottombtn").css("padding-left", "220px");
            }
            else {
                $(".sharebottombtn").css("padding-left", "20px");
            }

            //左边导航
            $('.gmkfNav').on('click', 'b', function () {
                $(this).siblings('ul').slideToggle(120);
                $(this).parents('dd').toggleClass('down');
            });
            $('.gmkfNav ul').on('click', 'li', function () {
                $(this).addClass("active").siblings().removeClass("active");
                $("#Hdhoteladmin").val($(this).attr("AdminHotelId"));
                $("#hdHotelId").val($(this).attr("HotelId"));
                hotelphoto();
                $('.demo').dad({
                    draggable: '.dads-children img'
                });
                //加载时所有按钮不可见
                $(".hotelphotolistb").find("span").hide();
            });

            hotelphoto();

            $('.demo').dad({
                draggable: '.dads-children img'
            });
            //加载时所有按钮不可见
            $(".hotelphotolistb").find("span").hide();

            $('.hotelphoto').on('click', '.gb', function (ev) {
                ev = ev || window.event;
                ev.stopPropagation();
                if (!confirm('您确认要删除当前图片吗？')) {
                    return false;
                }
                $(this).parent().remove();
                $('.demo').dad({
                    draggable: '.dads-children img'
                });
            });
        })

        //判断最多上传5张图片
        function add(tr) {
            var imglength = $(tr).next().next().find("div").length;
            if (parseInt(imglength) > 4) {
                showTipsMsg("最多上传5张", 3000, 3);
                return false;
            }
            else {
                $(tr).next().click();
            }
        }

        //鼠标移入当前层显示删除按钮
        function toOver(tr) {
            $(tr).find("span").show();
        }
        //鼠标移出当前层显示删除按钮
        function toOut(tr) {
            $(tr).find("span").hide();
        }

        function hotelphoto() {

            var HotelId = $("#hdHotelId").val();
            //加载酒店图片
            $.ajax({
                url: "photo.ashx",
                data: {
                    Menu: "GetHotelPicList",
                    HotelId: $("#hdHotelId").val()
                },
                type: "GET",
                datatype: "JSON",
                async: false,
                success: function (data) {
                    if (data == "") {
                        $("#HotelPicture").empty();
                        var html = "";
                        html += "<div class='hotelphotolistb clearfix' onmouseover='toOver(this)' onmouseout='toOut(this)'>";
                        html += " <div class='cfae4ce clearfix'>";

                        html += "<div class='add' onclick='add(this)'><i class='icon-plus'></i></div>";
                        html += "<input type='file' id='fileElem' name='fileElem'  accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs' onchange='handleFiles(this)' style='display: none' />";
                        html += "<div pid='" + HotelId + "' typeid='9'  class='addlist demo clearfix' >";
                        html += "</div>";
                        html += "</div>";
                        html += "</div>";
                        $("#HotelPicture").append(html);
                        return;
                    }

                    var json = eval("(" + data + ")");
                    $("#HotelPicture").empty();
                    var html = "";
                    html += "<div class='hotelphotolistb clearfix' onmouseover='toOver(this)' onmouseout='toOut(this)'>";
                    html += " <div class='cfae4ce clearfix'>";

                    html += "<div class='add' onclick='add(this)'><i class='icon-plus'></i></div>";
                    html += "<input type='file' id='fileElem' name='fileElem'  accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs' onchange='handleFiles(this)' style='display: none' />";
                    html += "<div pid='" + HotelId + "' typeid='9'  class='addlist demo clearfix' >";

                    for (var i = 0; i < json.length; i++) {
                        if (json[i].IMGFILE != undefined && json[i].IMGFILE != "") {
                            html += "<div imgname='" + json[i].IMGFILE + "'><img src='../../upload/photo/SN" + json[i].IMGFILE + "' alt='Alternate Text' /> <span class='gb'><i class='icon-close'></i></span> </div>";
                        }
                    }
                    html += "</div>";
                    html += "</div>";
                    html += "</div>";
                    $("#HotelPicture").append(html);

                }
            });

            //加载房型图片
            $.ajax({
                url: "photo.ashx",
                data: {
                    Menu: "GetRoomPicList",
                    HotelId: $("#hdHotelId").val()
                },
                type: "GET",
                datatype: "JSON",
                async: false,
                success: function (data) {
                    if (data == "") {
                        $("#RoomPicture").empty();
                        return;
                    }

                    var json = eval("(" + data + ")");
                    $("#RoomPicture").empty();
                    var html = "";
                    for (var i = 0; i < json.length; i++) {
                        html += "<div class='hotelphotolistb clearfix' onmouseover='toOver(this)' onmouseout='toOut(this)'><div class='fx' >" + json[i].NAME + "</div>";
                        html += " <div class='cfae4ce clearfix'>";
                        var file = "file" + i;
                        html += "<div class='add' onclick='add(this)'><i class='icon-plus'></i></div>";
                        html += "<input type='file' id='" + file + "' name='fileElem'  accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs' onchange='handleFiles(this)' style='display: none' />";
                        html += "<div pid='" + json[i].ID + "' typeid='7'  class='addlist demo clearfix' >";
                        if (json[i].pic != undefined && json[i].pic != "") {
                            for (var s = 0; s < json[i].pic.length; s++) {
                                html += "<div imgname='" + json[i].pic[s].IMGFILE + "'><img src='../../upload/photo/SN" + json[i].pic[s].IMGFILE + "' alt='Alternate Text' /><span class='gb'><i class='icon-close'></i></span> </div>";
                            }
                        }
                        html += "</div>";
                        html += "</div>";
                        html += "</div>";
                    }
                    $("#RoomPicture").append(html);
                }
            });

            //加载营业点图片
            $.ajax({
                url: "photo.ashx",
                data: {
                    Menu: "GetBusinessPicList",
                    HotelId: $("#hdHotelId").val()
                },
                type: "GET",
                datatype: "JSON",
                async: false,
                success: function (data) {
                    if (data == "") {
                        $("#BusinessPicture").empty();
                        return;
                    }
                    var json = eval("(" + data + ")");
                    $("#BusinessPicture").empty();
                    var html = "";
                    for (var i = 0; i < json.length; i++) {
                        html += "<div class='hotelphotolistb clearfix' onmouseover='toOver(this)' onmouseout='toOut(this)'><div class='fx' >" + json[i].TYPENAME + "-" + json[i].BUSINESSNAME + "</div>";
                        html += " <div class='cfae4ce clearfix'>";
                        var file = "files" + i;
                        html += "<div class='add'  onclick='add(this)'><i class='icon-plus'></i></div>";
                        html += "<input type='file' id='" + file + "' name='fileElem'  accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs' onchange='handleFiles(this)' style='display: none' />";
                        html += "<div  pid='" + json[i].ID + "' typeid='17'  class='addlist demo clearfix' >";
                        if (json[i].pic != undefined && json[i].pic != "") {
                            for (var s = 0; s < json[i].pic.length; s++) {
                                html += "<div imgname='" + json[i].pic[s].IMGFILE + "'><img src='../../upload/photo/SN" + json[i].pic[s].IMGFILE + "' alt='Alternate Text' /><span class='gb'><i class='icon-close'></i></span> </div>";
                            }
                        }
                        html += "</div>";
                        html += "</div>";
                        html += "</div>";
                    }
                    $("#BusinessPicture").append(html);
                }
            })

            //加载微商城图片
            $.ajax({
                url: "photo.ashx",
                data: {
                    Menu: "GetMallPicList",
                    HotelId: $("#hdHotelId").val()
                },
                type: "GET",
                datatype: "JSON",
                async: false,
                success: function (data) {
                    if (data == "") {
                        $("#MallPicture").empty();
                        var html = "";
                        html += "<div class='hotelphotolistb clearfix' onmouseover='toOver(this)' onmouseout='toOut(this)'>";
                        html += " <div class='cfae4ce clearfix'>";

                        html += "<div class='add' onclick='add(this)'><i class='icon-plus'></i></div>";
                        html += "<input type='file' id='fileElem' name='fileElem'  accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs' onchange='handleFiles(this)' style='display: none' />";
                        html += "<div pid='" + HotelId + "' typeid='26'  class='addlist demo clearfix' >";
                        html += "</div>";
                        html += "</div>";
                        html += "</div>";
                        $("#MallPicture").append(html);
                        return;
                    }

                    var json = eval("(" + data + ")");
                    $("#MallPicture").empty();
                    var html = "";
                    html += "<div class='hotelphotolistb clearfix' onmouseover='toOver(this)' onmouseout='toOut(this)'>";
                    html += " <div class='cfae4ce clearfix'>";

                    html += "<div class='add' onclick='add(this)'><i class='icon-plus'></i></div>";
                    html += "<input type='file' id='fileElem' name='fileElem'  accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs' onchange='handleFiles(this)' style='display: none' />";
                    html += "<div pid='" + HotelId + "' typeid='26'  class='addlist demo clearfix' >";

                    for (var i = 0; i < json.length; i++) {
                        if (json[i].IMGFILE != undefined && json[i].IMGFILE != "") {
                            html += "<div imgname='" + json[i].IMGFILE + "'><img src='../../upload/photo/SN" + json[i].IMGFILE + "' alt='Alternate Text' /> <span class='gb'><i class='icon-close'></i></span> </div>";
                        }
                    }
                    html += "</div>";
                    html += "</div>";
                    html += "</div>";
                    $("#MallPicture").append(html);

                }
            });


        }
    </script>
    <script type="text/javascript">
        //上传图片
        function handleFiles(obj) {
            var files = obj.files;
            var img_name = files[0].name;
            if (!/.(gif|jpg|jpeg|png|GIF|JPG|png)$/.test(img_name)) {
                showTipsMsg("图片类型必须是.gif,jpeg,jpg,png中的一种！", 3000, 5);
                return false;
            }
            var file = (files[0].size / 1024).toFixed(2); // 将字节/1024，取KB值
            var filesize = 800; //限制大小 KB
            var optimize = 0;
            if (file > 20000) {
                showTipsMsg("提交失败,不能上传超过20M的图片！", 3000, 3);
                return;
            }
            if (file > filesize) { //当上传图片的大小超过3MB的时候，提示图片大小不符合！
                if (confirm("上传图片超过800KB，修改后再上传或者系统自动优化，是否系统自动优化？")) {
                    optimize = 1;
                } else {
                    return false;
                }
                //                showConfirmMsg("上传图片超过800kb，修改后再上传或者系统自动优化，是否系统自动优化？", function (r) {
                //                    if (r) {
                //                        optimize = 1;
                //                    } else {
                //                        return false;
                //                    }
                //                });
            }
            var img = new Image();
            var src = "";
            if (window.URL) {
                //保存图片
                var formData = new FormData();
                formData.append("type", "add");
                formData.append("path", "photo");
                formData.append("optimize", optimize); //是否系统自动优化
                formData.append("fileElem", files[0]);
                if (files[0] != null) {
                    $.ajax({
                        url: "../../Ajax/UploadImg1.ashx",
                        data: formData,
                        type: "post",
                        dataType: "text",
                        enctype: 'multipart/form-data',
                        contentType: false,
                        processData: false,
                        async: false,
                        success: function (response) {
                            src = response;
                        }
                    })
                    img.src = window.URL.createObjectURL(files[0]); //创建一个object URL，并不是你的本地路径
                    var newspan = "<div  imgname=" + src + "><img src='" + img.src + "' alt='Alternate Text' /><span class='gb' ><i class='icon-close'></i></span></div>";
                    $(obj).next().append(newspan);
                    $('.demo').dad({
                        draggable: '.dads-children img'
                    });
                }
            }
        }

        //保存图片
        function check() {
            var hfImage = "";
            var HotelId = $("#hdHotelId").val();
            for (var k = 0; k < $(".demo").length; k++) {
                var Image = "";
                for (var y = 0; y < $(".demo").eq(k).find("div").length; y++) {
                    Image += $(".demo").eq(k).find("div").eq(y).attr("imgname") + ",";
                }
                hfImage += $(".demo").eq(k).attr("pid") + "," + $(".demo").eq(k).attr("typeid") + "!" + Image + "|";
            }
            $("#hfImage").val(hfImage);

            if (!confirm('您确认要提交此操作吗？')) {
                return false;
            }
            //?Menu=AddPhoto&hfImage=" + hfImage + "&hdHotelId=" + HotelId
            $.post("photo.ashx", { Menu: "AddPhoto", hfImage: hfImage, hdHotelId: HotelId }, function (data) {
                if (data == "ok") {
                    showTipsMsg("提交成功！", 3000, 3);

                }
                else {
                    showTipsMsg("提交失败！", 3000, 3);
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <input runat="server" type="hidden" id="htHotelTree" value="true" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>酒店管理</span> &gt; </span><span>酒店图片</span></span>
        </div>
    </div>
    <div class="gtall gmkf clearfix">
        <div id="HotelTree" runat="server" class="gmkfNav">
            <dl>
                <%=hotelTreeHtml.ToString()%>
            </dl>
        </div>
        <div class="shareright" style="overflow-y: auto;">
            <div class="hotelphoto">
                <div class="hotelphotolist">
                    <div class="hotelphotolistt">
                        <span>酒店信息图片</span><p>
                            建议上传图片最大为800KB，尺寸为4:3，最多上传5张，第1张图片同时作为缩略图和轮换图，拖动图片可排序</p>
                    </div>
                    <div id="HotelPicture">
                    </div>
                </div>
                <div class="hotelphotolist">
                    <div class="hotelphotolistt">
                        <span>房型图片</span><p>
                            建议上传图片最大为800KB，尺寸为4:3，每个房型最多上传5张图片，第1张图片同时作为缩略图和轮换图，拖动图片可排序</p>
                    </div>
                    <div id="RoomPicture">
                    </div>
                </div>
                <div class="hotelphotolist">
                    <div class="hotelphotolistt">
                        <span>营业点图片</span><p>
                            建议上传图片最大为800KB，尺寸为4:3，每个营业点最多上传5张图片，第1张图片同时作为缩略图和轮换图，拖动图片可排序</p>
                    </div>
                    <div id="BusinessPicture">
                    </div>
                </div>

                  <div class="hotelphotolist">
                    <div class="hotelphotolistt">
                        <span>微商城主图</span><p>
                            建议上传图片最大为800KB，尺寸为4:3，最多上传5张，第1张图片同时作为缩略图和轮换图，拖动图片可排序</p>
                    </div>
                    <div id="MallPicture">
                    </div>
                </div>

                <div style="height: 70px;">
                </div>
                <div class="sharebottombtn" style="background-color: #F3F3F3; position: fixed; bottom: 0;
                    left: 0; width: 100%; padding-left: 20px;">
                    <a onclick="check()">保存</a>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfImage" runat="server" />
    </form>
</body>
</html>
