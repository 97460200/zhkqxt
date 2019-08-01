var left = {
    currKey: -1,   //记录li的索引值
    subKey: -1,    //记录a 的索引值
    F_isSm: function (bool) {
        bool ? $('#sm').show() : $('#sm').hide();
        bool ? $('#submenuContxt').hide() : $('#submenuContxt').show();
    },
    F_val: function (val, obj) {                                       //赋值
        $('#nemuName').html(val);
        $('#newTitle').val(val);

        if (typeof obj !== 'undefined') {
            $('#sendText').val(obj.text); //赋值富文本
            $('#Url').val(obj.url);       //赋值链接
        }
    },
    F_addLi: function () {                                     //添加及编辑<li>              this指向li
        left.currKey = $(this).index();
        left.subKey = -1;
        if ($(this).attr('class') == 'icon-plus') {
            //left
            if ($('.phfoNav li').length > 2) $(this).hide();
            $('.phfoNav li').removeClass('active');
            $(this).before('<li class="active"><b>菜单名称</b><div class="subNav"><a class="icon-plus" title="最多添加5个子菜单名称"></a></div></li>');

            //right
            left.F_val('菜单名称');
            left.F_isSm(true);

            DataList.push({ menu: '菜单名称', sub: [] })
        } else {
            //left
            $(this).addClass('active').siblings().removeClass('active');

            //right
            left.F_val($(this).find('b').html());
            left.F_isSm(true);
        };
        $('.survey').fadeIn(50);
        $('.phBtn').show();
    },
    F_SubNav: function (ev) {                                  //添加及编辑<li>里面的<a>       this指向a
        var ev = ev || window.event;
        ev.stopPropagation();
        left.subKey = $(this).index();
        var index = $(this).parents('li').index();
        if ($(this).attr('class') == 'icon-plus') {
            if ($('.phfoNav li').eq(index).find('a').length > 4) $(this).hide();
            $(this).parents('.subNav').prepend('<a data-url="" data-send="">子菜单名称</a>');

            DataList[index].sub.unshift({ name: '子菜单名称' });
        } else {
            //left
            $('.subNav a').removeClass('curr');
            $(this).addClass('curr');

            //right
            left.F_val($(this).html(), { text: $(this).attr('data-send'), url: $(this).attr('data-url') });
            left.F_isSm(false);
        }
    },
    F_init: function (arr) {                                     //初始化左边导航内容
        var html = '';
        window.DataList = arr;
        for (var i = 0; i < arr.length; i++) {
            html += '<li><b>' + arr[i].menu + '</b><div class="subNav">';
            for (var j = 0; j < arr[i].sub.length; j++) {
                html += '<a data-url="' + arr[i].sub[j].url + '" data-send="' + arr[i].sub[j].send + '">' + arr[i].sub[j].name + '</a>';
            };
            arr[i].sub.length < 5 ? html += '<a class="icon-plus" title="最多添加5个子菜单名称"></a>' : html += '<a class="icon-plus" style="display: none" title="最多添加5个子菜单名称"></a>';
            html += '</div></li>';
        }
        arr.length < 3 ? html += '<li class="icon-plus" title="最多添加3个菜单名称"></li>' : html += '<li class="icon-plus" style="display: none" title="最多添加3个菜单名称"></li>';
        $('.phfoNav').append(html);
    }
};





var right = {
    F_tip: function (text) {                                     //提示函数
        $('#SetUseName').html(text).show();
        setTimeout(function () { $('#SetUseName').html('').hide(); }, 2000);
    },
    F_changeNav: function () {                                   //菜单内容栏目tab切换
        var index = $(this).index();
        $(this).addClass('active').siblings().removeClass('active');
        $('#showList li').eq(index).show().siblings().hide();
    },
    F_del: function (str) {                                           //进入删除弹窗
        if (typeof str === 'undefined' || str === 'sure') {
            $('.delAlert').fadeOut(100);
            $('body').css('overflow', '');
            if (str === 'sure') right.F_sureDel();
        } else {
            $('#Dete').fadeIn(100);
            $('body').css('overflow', 'hidden');
        }
    },
    F_sureDel: function () {                                          //弹窗确定 - 删除导航
        $('.survey').fadeOut(100);
        $('.phBtn').hide();
        if (left.subKey != -1) {
            $('.phfoNav li').removeClass('active').eq(left.currKey).find('a').removeClass('curr').eq(left.subKey).remove();
            $('.phfoNav li').eq(left.currKey).find('a').length < 6 ? $('.phfoNav li').eq(left.currKey).find('a.icon-plus').show() : $('.phfoNav li').eq(left.currKey).find('a.icon-plus').hide();
            DataList[left.currKey].sub.splice(left.subKey, 1);
        } else {
            $('.phfoNav li').eq(left.currKey).remove();
            $('.phfoNav li').length < 4 ? $('.phfoNav li.icon-plus').show() : $('.phfoNav li.icon-plus').hide();
            console.log(left.currKey);
            DataList.splice(left.currKey, 1);
        }
        console.log(DataList);
    },
    F_eval: function (val, obj) {                                       //赋值
        if (val == '') {
            right.F_tip('导航标题不能为空');
            return;
        }

        right.F_tip('保存并发布成功');
        if (left.subKey != -1) {
            $('.phfoNav li').eq(left.currKey).find('a').eq(left.subKey).html(val);
            DataList[left.currKey].sub[left.subKey].name = val;
        } else {
            $('.phfoNav li').eq(left.currKey).find('b').html(val);
            DataList[left.currKey].menu = val;
        };
        $('#nemuName').html(val)

        console.log(DataList);
        if (typeof obj !== 'undefined' && left.subKey != -1) {
            DataList[left.currKey].sub[left.subKey].send = obj.text;
            DataList[left.currKey].sub[left.subKey].url = obj.url;
            $('.phfoNav li').eq(left.currKey).find('a').eq(left.subKey).attr('data-send', obj.text);
            $('.phfoNav li').eq(left.currKey).find('a').eq(left.subKey).attr('data-url', obj.url);
        }
    },
    F_sure: function (ev) {                                       //确定保存
        if (typeof ev !== 'undefined' && ev.which != 13) return;
        var $title = $('#newTitle').val();
        var $sendText = $('#sendText').val();
        var $Url = $('#Url').val();
        right.F_eval($title, { text: $sendText, url: $Url });
    }
}



$('.phfoNav').on('click', 'li', left.F_addLi);
$('.phfoNav').on('click', 'a', left.F_SubNav);
$('#exchange').on('click', 'a', right.F_changeNav);
document.onkeyup = right.F_sure;

