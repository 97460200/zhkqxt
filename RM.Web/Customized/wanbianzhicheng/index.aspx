<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="RM.Web.Customized.wanbianzhicheng.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <meta name="format-detection" content="telephone=no">
    <title>湾边之城酒店</title>
    <link href="/App_Themes/default/css/Total.css" rel="stylesheet" type="text/css">
    <link href="/App_Themes/default/css/regroup.css" rel="stylesheet" type="text/css">
    <link href="/App_Themes/default/css/bh.css" rel="stylesheet" type="text/css" />
    <link href="/App_Themes/default/css/swiper.min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .logo {
                width: 36%;
                margin: 0 auto;
                position: absolute;
                left: 32%;
                top: 5%;
                z-index: 999;
        }
        .wbzcnav {
                width: 66%;
                margin: 0 auto;
                position: absolute;
                left: 17%;
                top: 30%;
                z-index: 999;
        }
        .wbzcnav .a1{
                position: absolute;
                left: 34%;
                top: 5%;
                width: 32%;
                height: 30%;
        }
        .wbzcnav .a2{
                position: absolute;
                left: 2%;
                top: 35%;
                width: 32%;
                height: 30%;
        }
        .wbzcnav .a3{
                position: absolute;
                left: 34%;
                top: 65%;
                width: 32%;
                height: 30%;
        }
        .wbzcnav .a4{
                position: absolute;
                left: 65%;
                top: 35%;
                width: 32%;
                height: 30%;
        }
        .wbzcnav img{
                width: 100%;
        }
        .logo img{
            width: 100%;
        }
        .phone {
            position: absolute;
            bottom: 8%;
            width: 60%;
            left: 24%;
            color: #fff;
            z-index: 999;
        }
    </style>
</head>
<body  class="bg">
    <form id="form1" runat="server">
    <div class="banner">
        <div class="swiper-container">
            <div class="swiper-wrapper">
                <div class="swiper-slide"><img src="/App_Themes/default/images/wbzcimages/bg.png" alt="Alternate Text" /></div>
            </div>
        </div>
    </div>
    <div class="logo clearfix">
        <div class="">
            <img src="/App_Themes/default/images/wbzcimages/logo.png" alt="Alternate Text" />
        </div>
    </div>
    <div class="wbzcnav">
        <img src="/App_Themes/default/images/wbzcimages/nav.png" alt="Alternate Text" />
        <a href="http://www.zidinn.com/Reservation/HotelDetails.aspx?AdminHotelid=1010107&hotelid=156" class="a1"></a>
        <a href="http://www.zidinn.com/Reservation/HotelIntroduction.aspx?AdminHotelid=1010107&hotelid=156" class="a2"></a>
        <a href="http://www.zidinn.com/HotelCenter/Discount.aspx?AdminHotelid=1010107" class="a3"></a>
        <a href="http://www.zidinn.com/Members/MemberCantre.aspx?AdminHotelid=1010107" class="a4"></a>
    </div>
    <div class="phone clearfix">
        <div class="phone01">
            <img src="/App_Themes/default/images/wbzcimages/phone.png" alt="Alternate Text" />
        </div>
        <a style="display: block;" href="tel:400-8310-009">
            <div class="phone02">
                <p>
                    预订热线
                </p>
                <p>
                    400-8310-009
                </p>
            </div>
        </a>
    </div>
    </form>
</body>
</html>
