/*
上传图片    
*/

var maxsize = 2 * 1024 * 1024; //2M  
var errMsg = "上传的图片不能超过2M！！！";
var tipMsg = "您的浏览器暂不支持计算上传文件的大小，确保上传文件不要超过2M，建议使用IE10及以上、FireFox(火狐)、Chrome(谷歌)浏览器。";
var browserCfg = {};
var ua = window.navigator.userAgent;
if (ua.indexOf("MSIE") >= 1) {
    browserCfg.ie = true;
} else if (ua.indexOf("Firefox") >= 1) {
    browserCfg.firefox = true;
} else if (ua.indexOf("Chrome") >= 1) {
    browserCfg.chrome = true;
}

//vFiles:上传file按钮对象
//showImg:要显示图片层的ID
//vWidth:图片的宽度
//vheight:图片的高度
window.URL = window.URL || window.webkitURL;
function UploadImg(vFiles, showImg, vWidth, vHeight) {

    var userAgent = navigator.userAgent; //取得浏览器的userAgent字符串
    var isIE = userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1; //判断是否IE浏览器
    var IE5 = IE55 = IE6 = IE7 = IE8 = IE9 = false;
    var reIE = new RegExp("MSIE (\\d+\\.\\d+);");
    reIE.test(userAgent);
    var fIEVersion = parseFloat(RegExp["$1"]);
    IE55 = fIEVersion == 5.5;
    IE6 = fIEVersion == 6.0;
    IE7 = fIEVersion == 7.0;
    IE8 = fIEVersion == 8.0;
    IE9 = fIEVersion == 9.0;

    if (IE5 || IE55 || IE6 || IE7 || IE8 || IE9) {
        alert(tipMsg);
        return false;
    }

    try {
        var obj_file = document.getElementById("fileUploadImg");
        if (obj_file.value == "") {
            alert("请先选择上传文件");
            return false;
        }
        var filesize = 0;
        if (browserCfg.firefox || browserCfg.chrome) {
            filesize = obj_file.files[0].size;
        } else if (browserCfg.ie) {
            var obj_img = document.getElementById('tempimg');
            obj_img.dynsrc = obj_file.value;
            filesize = obj_img.fileSize;
        } else {
            alert(tipMsg);
            return false;
        }
        if (filesize == -1) {
            alert(tipMsg);
            return false;
        } else if (filesize > maxsize) {
            alert(errMsg);
            return false;
        }
        //        else {
        //            //alert("文件大小符合要求");
        //            return true;
        //        }
    } catch (e) {
        alert(e);
        return false;
    }

    var files = vFiles.files;
    var dImages = $("#" + showImg);
    if (dImages.length < 1) {
        alert("未找到显示图层");
        return;
    }
    var img = new Image();
    var src = "";
    if (window.URL) {
        //保存图片
        var formData = new FormData();
        formData.append("type", "add");
        formData.append("path", "Images");
        formData.append("fileElem", files[0]);
        $.ajax({
            url: "/Ajax/UploadImg.ashx",
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
        });

        if (src == "") {
            alert("图片上传失败!可能图片过大,图片不能大于4M");
            return false;
        }
        if (src == "0") {
            alert("图片上传失败!图片格式为jpg、gif、bmp、png");
            return false;
        }
        if (src.length != 40) {
            alert("图片上传失败!图片格式为jpg、gif、bmp、png");
            return false;
        }
        var dHtml = "<div>";
        dHtml += "<img src='" + window.URL.createObjectURL(files[0]) + "' width='" + vWidth + "' height='" + vHeight + "' />";
        dHtml += "<input type='button' class='tpclose' value='" + src + "' />";
        dHtml += "<input type='hidden' value='" + src + "' />";
        dHtml += "</div>";
        dImages.append(dHtml).find("input[type=button]").click(function () {
            var t = $(this);
            //删除图片
            $.ajax({
                url: "/Ajax/UploadImg.ashx",
                data: {
                    type: "dele",
                    name: $(t).val(),
                    path: "Images"
                },
                type: "get",
                dataType: "text",
                success: function (response) {
                    if (response != null) {
                        //删除成功
                        $(t).parent("div").remove();
                    }
                }
            });
        });
        //结束
    }
}