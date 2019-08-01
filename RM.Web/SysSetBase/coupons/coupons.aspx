<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="coupons.aspx.cs" Inherits="RM.Web.SysSetBase.coupons.coupons" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>信息</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <style>
        
span.zffs, span.ddly, span.ddzt{
	background-color: #fff;
	padding: 0 8px;
	height: 22px;
	line-height: 22px;
	text-align: center;
	display: inline-block;
	color: #fff;
}
 span.wxzf{
	background-color: #339900;
}
 span.jfzf{
	background-color: #33CCCC;
}
 span.hykzf{
	background-color: #FF9933;
}
 span.qtzf{
	background-color: #D6487E;
}

 span.ygxs{
	background-color: #3A87AD;
}
 span.gw{
	background-color: #FF9933;
}

 span.wqr{
	background-color: #CC0000;
}
 span.yqr{
	background-color: #3A87AD;
}
 span.yqx{
	background-color: #E7E7E7;
	color: #999;
}
 span.yrz{
	background-color: #339900;
}
    </style>
    <script type="text/javascript">
        $(function () {
            ListGrid();
            $("#ddlSearch").change(function () {
                ListGrid();
            });

        });
        function GetWhere() {
            var strwhere = "";
            var Search = "";
            var svl = $("#txtSearch").val();
            if ($.trim(svl) != '') {
                Search += "number≌" + svl + "|";
            }
            if (Search != "") {
                strwhere += "&Search=" + Search.substr(0, Search.length - 1);
            }
            return strwhere;
        }
        /**加载表格函数**/
        function ListGrid() {
            //url：请求地址
            var url = "GetCouponList.ashx?Menu=GetCoupon" + GetWhere();
            //colM：表头名称
            var colM = [
                { title: "checkbox", width: 60, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        return "<div onclick='CheckAllLine3(this)' class='xuanxuan' dateId='" + rowData[0] + "' name='checkbox'></div>";
                    }
                },
                { title: "卡券名称", width: 120, align: "center" },
                { title: "面值", width: 80, align: "center" },
                { title: "满额可用", width: 100, align: "center", render: function (ui) {
                    var rowData = ui.rowData;
                    return "满" + rowData[13] + "可用";
                } 
                },
                { title: "有效期", width: 180, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        if (rowData[9] * 1 == 1) {
                            return "永久";
                        } else if (rowData[9] * 1 == 2) {
                            return "时长（自领取之日" + rowData[12] + "天）";
                        } else {
                            return "" + rowData[3] + " - " + rowData[4] + "";
                        }
                    }
                },
                { title: "可用会员", width: 100, align: "center", render: function (ui) {
                    var rowData = ui.rowData;
                    return "" + rowData[10] + " ";
                }
                },
                { title: "获得方式", width: 100, align: "center", render: function (ui) {
                    var rowData = ui.rowData;
                    return "" + rowData[5] + " ";
                }
                },
//                { title: "同一会员赠送数量", width: 160, align: "center", render: function (ui) {
//                    var rowData = ui.rowData;
//                    return "" + rowData[11] + " ";
//                }
            //                },
                {title: "当天可用", width: 80, align: "center", render: function (ui) {
                     var rowData = ui.rowData;
                     return "" + rowData[15] + " ";
                }
                },
                { title: "是否启用", width: 80, align: "center", render: function (ui) {
                    var rowData = ui.rowData;
                    return "" + rowData[6] + " ";
                }
                },
                { title: "卡券状态", width: 80, align: "center", render: function (ui) {
                    var rowData = ui.rowData[8];
                    if (rowData == "未过期") return "<span class='ddzt yrz'>未过期</span>";
                    if (rowData == "已过期") return "<span class='ddzt yqx'>已过期</span>";
                    //return "" + rowData[6] + " ";
                }
                },
                { title: "添加时间", width: 100, align: "center", render: function (ui) {
                    var rowData = ui.rowData;
                    return "" + rowData[14] + " ";
                }
                },
                { title: "操作", editable: false, width: 80, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "<div class='caozuo'>";
                        px += "<span class='icon-edit4' title='' onclick='editById(" + rowData[0] + ")'>编辑</span>"
                        px += " </div>";
                        return px;
                    }
                }
            ];

            //sort：要显示字段,数组对应
                var sort = [
             "id",
                "id",
                "couponname",
                "par",
                "BiginTime",
                "EndinTime",
                "TypeName",
                "isEnable",
                "isend",
                "EffectiveType",
                "Ishy",
                "count",
                "EffectiveDay",
                "UsedMin",
                "addtime",
                "Is_Day_ok"
            ];
            PQLoadGrid("#grid_paging", url, colM, sort, 20, false);
            //                $("#grid_paging").pqGrid({
            //                    freezeCols: 7, //固定前面列
            //                    title: false
            //                });
            pqGridResize("#grid_paging", 0, 0);
            var kjheight = $("#grid_paging .pq-grid-header-table").width();
            if ($("#grid_paging").width() < kjheight) {
                pqGridResize("#grid_paging", -65, 0);
            }
            else {
                pqGridResize("#grid_paging", -45, 0);
            }
            $(window).bind({
                resize: function () {
                    if ($("#grid_paging").width() < kjheight) {
                        pqGridResize("#grid_paging", -65, 0);
                    }
                    else {
                        pqGridResize("#grid_paging", -45, 0);
                    }
                }
            })
        }
    </script>
    <script type="text/javascript">

        //添加
        function add() {
            var url = "/SysSetBase/coupons/couponadd.aspx";
            top.art.dialog.open(url, {
                id: 'MattersEdit',
                title: '卡券管理 > 添加卡券',
                width: 926,
                height: 550,
                close: function () {
                    ListGrid();
                }
            }, false);
        }
        //修改
        function edit() {
            var key = GetPqGridRowValue("#grid_paging", 0);
            if (IsEditdata(key)) {
                editById(key);
            }
        }

        function editById(key) {
            var url = "/SysSetBase/coupons/couponadd.aspx?ID=" + key;
            top.art.dialog.open(url, {
                id: 'MattersEdit',
                title: '卡券管理 > 修改卡券',
                width: 926,
                height: 550,
                close: function () {
                    ListGrid();
                }
            }, false);
        }

        // 删除
        function de() {
            var key = CheckboxValue();
            if (IsDelData(key)) {
                DeleteById(key)
            }
        }
        function DeleteById(key) {
            var delparm = 'action=ISDelete&module=优惠券管理系统&tableName=Coupon&pkName=ID&pkVal=' + key;
            delConfig('/Ajax/Common_Ajax.ashx', delparm);
        }

        function typegl() {
            window.location = "/SysSetBase/coupons/couponstypelist.aspx";
        }

   
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="tools_bar btnbartitle btnbartitlenr" style="display:block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt; </span><span>系统设置</span> &gt; </span><span>卡券设置</span></span>
        </div>
    </div>
    <div class="btnbarcontetn2"   style="display:none">
        <div class="niuniu">
            <ul>
                <li><a class="tz" onclick="add();"></a></li>
                <li><a class="xg" onclick="edit();"></a></li>
                <li><a class="sc" onclick="de();"></a></li>
                <li><a class="fh" href="javascript:history.back()"></a></li>
            </ul>
        </div>
    </div>

    <div class="ptb8">
        <div class="sharesearch">
        <asp:TextBox runat="server" ID="TextBox1" placeholder="请输入关键字..."></asp:TextBox>
            
            <i class="icon-search" onclick="ListGrid()"></i>
        </div>
        <div class="wdyhd" style="padding-right: 12px;">
            <div class="r">      
                <span onclick="typegl()" style=" display:none;">类型管理</span>      
                <span onclick="add()">添加</span>     
                <span onclick="edit()">编辑</span>     
                <span onclick="de();">删除</span>        
                <span><a href="javascript:history.back()">返回</a></span>     
            </div>

        </div>
    </div>


    <div id="grid_paging" style="margin-top: 1px;">
    </div>
    </form>
</body>
</html>
