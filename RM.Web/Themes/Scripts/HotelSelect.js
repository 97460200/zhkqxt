$(document).ready(function () {
    var hs = $(".HotelSelect");
    if (hs.length > 0) {
        var hsHtml = "<input type='text' class='txtHotelName' value='全部酒店' onclick='ChangeText()' placeholder='全部酒店'  />";
        hsHtml += "<div class='selectkuang' style='display: none'><ul id='searchkuang'></ul></div>";
        $(".HotelSelect").html(hsHtml);
        GetInfo();

        $('.txtHotelName').on('input propertychange', function () {
            var txtSearch = $(this).val();
            if (txtSearch != "") {
                $("#searchkuang li").hide();
                $("#searchkuang li:contains(" + txtSearch + ")").show();
            } else {
                $("#searchkuang li").show();
            }
        });
    }
})

function ChangeText() {
    $('.txtHotelName').val("");
    $(".selectkuang").show();
    $("#searchkuang li").show();
}

//$("p:contains(intro)").css("background-color","#B2E0FF");
function GetInfo() {
    $.ajax({
        url: "/API/GetInfo.ashx",
        data: {
            action: "GetHotelList"
        },
        type: "POST",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data == null) {
                $(".selectkuang").hide();
                return;
            }
            $("#searchkuang").empty();
            var html = "";
            for (var i = 0; i < data.data.length; i++) {
                var hotel = data.data[i];
                html += "<li onclick=\"ChangeHotel('" + hotel.Id + "','" + hotel.HotelName + "')\" >" + hotel.HotelName + "</li>";
            }
            $("#searchkuang").append(html);
        }
    })
}

function ChangeHotel(id, name) {
    $("#hdHotelId").val(id);
    $(".txtHotelName").val(name);
    $(".selectkuang").hide();
}