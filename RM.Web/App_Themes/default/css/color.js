/*var standard = `
    mCol:#f90, mIcon:#f90,
    sCol_pinf:#f90, sCol_price:#f90, sCol_conpon:#f90,

    bCol_mainbg:#ffffff, bCol_cutLine:#eee, bCol_border:#f3f3f3, bCol_textDeep:#333333, bCol_textMid:#666666, bCol_textLight:#999999, 
    bCol_btnDisabled:#e3e3e3,bCol_btnDisabledIcon:#090,bCol_icon:#999, bCol_cpn:#ddd,
    
    bCol_memCard:#ff9900, bCol_textCard:#ffffff,bCol_bottombar:#ffffff,bCol_leftmenu:#ffffff,bCol_inputbox:#ffffff,bCol_fydxanbg:#ffffff,bCol_gobacktop:#ffffff,bCol_jrgwcbg:#ffffff,
    bCol_dbczlxztb:#ffffff,bCol_dbczlmrtb:#ffffff
`;

var whiteCol = `
    mCol:#006, mIcon:#509E4B,
    sCol_pinf:#09f, sCol_price:#cc0000, sCol_conpon:#f90,

    bCol_mainbg:#ffffff, bCol_cutLine:#eee, bCol_border:#f3f3f3, bCol_textDeep:#333333, bCol_textMid:#666666, bCol_textLight:#999999, 
    bCol_btnDisabled:#e3e3e3,bCol_btnDisabledIcon:#090,bCol_icon:#999, bCol_cpn:#ddd,
    
    bCol_memCard:#ff9900, bCol_textCard:#ffffff,bCol_bottombar:#ffffff,bCol_leftmenu:#ffffff,bCol_inputbox:#ffffff,bCol_fydxanbg:#ffffff,bCol_gobacktop:#ffffff,bCol_jrgwcbg:#ffffff,
    bCol_dbczlxztb:#ffffff,bCol_dbczlmrtb:#ffffff
`;

var blackCol = `
    mCol:#003866, mIcon:#fc0,
    sCol_pinf:#67BDE5, sCol_price:#ee0000, sCol_conpon:#f90,

    bCol_mainbg:#000000, bCol_cutLine:#454545, bCol_border:#333, bCol_textDeep:#d6d6d6, bCol_textMid:#999, bCol_textLight:#767676, 
    bCol_btnDisabled:#333,bCol_btnDisabledIcon:#090,bCol_icon:#767676, bCol_cpn:#ddd,
    
    bCol_memCard:#ff9900, bCol_textCard:#ffffff,bCol_bottombar:#ffffff,bCol_leftmenu:#ffffff,bCol_inputbox:#ffffff,bCol_fydxanbg:#ffffff,bCol_gobacktop:#ffffff,bCol_jrgwcbg:#ffffff,
    bCol_dbczlxztb:#ffffff,bCol_dbczlmrtb:#ffffff
`;*/


var hubei = `
    mCol:#B97039, mIcon:#B97039,
    sCol_pinf:#B97039, sCol_price:#B97039, sCol_conpon:#B97039,

    bCol_mainbg:#ffffff, bCol_cutLine:#eee, bCol_border:#f3f3f3, bCol_textDeep:#333333, bCol_textMid:#666666, bCol_textLight:#999999, 
    bCol_btnDisabled:#e3e3e3,bCol_btnDisabledIcon:#090,bCol_icon:#999, bCol_cpn:#ddd,
    
    bCol_memCard:#B97039, bCol_textCard:#ffffff,bCol_bottombar:#ffffff,bCol_leftmenu:#ffffff,bCol_inputbox:#ffffff,bCol_fydxanbg:#ffffff,bCol_gobacktop:#ffffff,bCol_jrgwcbg:#ffffff,
    bCol_dbczlxztb:#ffffff,bCol_dbczlmrtb:#ffffff,bCol_tbczlds:#ffffff,bCol_tbczlztys:#ffffff
`;




;(function(selector) {
    var colStr = '',
        // colJson = JSON.parse('{"' + document.querySelector(selector).value.replace(/\s+/g, '').toLowerCase().replace(/:#/g, '\":\"#').replace(/,/g, '\",\"') + '"}'),
        colJson = JSON.parse('{"' + hubei.replace(/\s+/g, '').toLowerCase().replace(/:#/g, '\":\"#').replace(/,/g, '\",\"') + '"}'),
        styleTag = document.createElement('style');
        var jsong = document.getElementById(selector).value;
        if (jsong != "" && jsong.length > 10) {
            colJson =  JSON.parse(jsong.toLowerCase());
        }
    colStr = `
        /*公共部分*/
        body{background-color: ${colJson.bcol_mainbg}; color: ${colJson.bcol_textdeep}}
        input,select,textarea{background-color: ${colJson.bcol_mainbg}; border-color: ${colJson.bcol_cutline} !important; color: ${colJson.bcol_textdeep}}
        ::-webkit-input-placeholder{color: ${colJson.bcol_textlight};}
        [bg]{background-color: ${colJson.bcol_mainbg}}
        [bgother]{background-color: ${colJson.bcol_border}}

        [textdeep]{color: ${colJson.bcol_textdeep}}
        [textmid]{color: ${colJson.bcol_textmid}}
        [textfleet]{color: ${colJson.bcol_textlight}}


        /*边线 & 分割线*/
        [border]{border-color: ${colJson.bcol_cutline} !important}
        [bgborder]{border-color: ${colJson.bcol_border} !important}
        [bgbafter]:after{background-color: ${colJson.bcol_cutline} !important}


        /*单选多选*/
        [radio] input:checked + label:before{color: ${colJson.micon} !important}
        [radio] .active:before{color: ${colJson.micon} !important}               /*字体图标高亮*/
        [radioOther] .active:before{color: ${colJson.mcol} !important}
        [radioOtherFont] .active, [radioOtherFont] .active:before{color: ${colJson.mcol} !important}

        [radioFont] .active,[radioFont] .this{color: ${colJson.mcol} !important}
        [radioFont] .active:after, [radioFont] .this:after{background-color: ${colJson.mcol} !important}




        /*主色调 & 高亮字色*/
        [mcol],[mcolbefore]:before{color: ${colJson.mcol} !important}
        [bgmcol],[bgmcolbefore]:before,[bgmcolafter]:after{background-color: ${colJson.mcol} !important}
        [mfont]{color: ${colJson.micon} !important}                          /*字体颜色高亮*/


        /*副色调 [价格,按钮][评分]*/
        [scol], [scolbefore]:before{color: ${colJson.scol_price} !important}
        [bgscol],[bgscolafter]:after{background-color: ${colJson.mcol} !important}
        [spf]{color: ${colJson.scol_pinf} !important}

        [btnfleet]{background-color: ${colJson.bcol_btndisabled} !important}
        [btnscolicon]{background-color: ${colJson.bcol_btndisabledicon} !important}
        [btnscolicon]:before{background-color: ${colJson.bcol_btndisabledicon} !important}
        [btnfleeticon]:before{background-color: ${colJson.bcol_textlight} !important}
        .button{background-color: ${colJson.bcol_btndisabled}}



        /*字体图标*/
        [miconbefore]:before{color: ${colJson.micon}}
        [bgmiconbefore]:before{background-color: ${colJson.micon}}
        [fleetbefore]:before, [fleetafter]:after{color: ${colJson.bcol_btndisabled}}


        /*卡券*/
        [conpon]:before{color: ${colJson.scol_conpon} !important}
        [bgconpon]{background-color: ${colJson.scol_conpon} !important}
        [bgfleetcpn]{background-color: ${colJson.bcol_cpn} !important}
        [bgcpnw]:before,[bgcpnw]:after{background-color: ${colJson.bcol_mainbg}; border-color: ${colJson.bcol_cutline} !important}

        /*会员卡*/
        [memCard]{background-color: ${colJson.bcol_memcard} !important}

        /*头部&底部*/
        #H_title,#H_list,.Head,.Head > a{background-color: ${colJson.bcol_tbczlds}}
        .Head{color: ${colJson.bcol_tbczlztys}}
        .Reserve .foot li a.rese,.orderlist .foot li a.orde,.profile .foot li a.prof,.discount .foot li a.conp,.member .foot li a.mine,.Reserve .foot li a.rese:before,.orderlist .foot li a.orde:before,.profile .foot li a.prof:before,.discount .foot li a.conp:before,.member .foot li a.mine:before{color: ${colJson.bcol_dbczlxztb}}
        .foot li a{color:${colJson.bcol_dbczlmrtb}}
        [bottombar]{background-color:${colJson.bcol_bottombar}}


        /*特殊*/
            /*日期插件*/
            .kalendae .k-title {background-color: ${colJson.bcol_border}}
            .kalendae .k-header,.kalendae .k-days span.k-in-month.k-active{background-color: ${colJson.bcol_mainbg}}
            .kalendae .k-caption, .kalendae .k-header span{color: ${colJson.bcol_textdeep}}
            .kalendae .k-days span.k-out-of-month{color: ${colJson.bcol_textlight}}
            .kalendae .k-days span.k-in-month.k-active{color:  ${colJson.bcol_textdeep}}
            .kalendae .k-days span.k-selected.k-active, .kalendae .k-days span.k-range{background-color: ${colJson.mcol} !important; color: #fff !important}

            /*预订成功*/
            .orderok .oroList li .df:before,.df1:before{background-color: ${colJson.mcol}}

            /*订单详情 进度*/
            .ordprog li:after{color: ${colJson.bcol_btndisabled};}
            .ordprog li i,.ordDtl .ordprog li i:after{background: ${colJson.bcol_btndisabled};}
            .ordprog li.active, .ordDtl .ordprog li.active:after{color: ${colJson.micon};}
            .ordDtl .ordprog li.active i, .ordDtl .ordprog li.active i:after{background: ${colJson.micon};}

            /*订单 评价*/
            .pulish .plhstar i{color: ${colJson.bcol_btndisabled}}
            .pulish .plhstar i.active{color: ${colJson.scol_price}}

            /*注册*/
            .Regis .reRead:before{border-color: ${colJson.bcol_btndisabled}; }
            .Regis .active:before{background-color: ${colJson.scol_price}}

            /*其他*/
            .zhoubianxinxi p.act{color: ${colJson.mcol} !important;border-color:${colJson.mcol} !important}

            /*开票*/
            .Billing .BillList li.active b{
                background-color: ${colJson.mcol};
                border-color: ${colJson.bcol_border} !important;
            }
            .Billing .shopPos li.active i:after{
                background-color: ${colJson.mcol};
            }
            /*点餐*/
            .Menu .SideList li.active a{
                color:${colJson.mcol} !important;
            }
            .Menu .ContList .ConText p.price em i.add:before, .Menu .ContList .ConText p.price em i.add:after,.Menu .ContList .ConText p.price em i.reduce:after,.OrderText .orderPrice p i.add:before, .OrderText .orderPrice p i.add:after{
                background-color:${colJson.mcol} !important;
            }
            .beizhu label.active{
                background-color: ${colJson.mcol};
                border-color: ${colJson.mcol} !important;
            }
            .guige .guigelist li .d1 div.act{
                background-color: ${colJson.mcol};
            }

            /*微商城*/
            .Productmall .Pmnav li.act01{
                color:${colJson.mcol};
                border-bottom:1px solid ${colJson.mcol};
            }

            /*商品管理*/
            .Roomnumberlist dl dd a.act{
                color:${colJson.mcol};
            }
            /*商品管理*/
            
            .ticketnav ul li.act .p2{
                color:${colJson.mcol};
            }
            .ticketnav ul li.act .p1{
                color:${colJson.mcol};
            }
    `;


    styleTag.innerHTML = colStr;
    document.getElementsByTagName('head')[0].appendChild(styleTag);
})('head_Custom_Color');