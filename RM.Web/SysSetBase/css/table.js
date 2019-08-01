
define(['jquery'], function ($) {
    return function (TableDivClass) {
        var TabId = $(TableDivClass);
        var borderColor = TabId.find('td').css('border-color');
        if (TabId.find('table').height() > TabId.height()) {
            TabId.find('table').css('margin-bottom', '-1px');
            TabId.find('td').css('border-bottom-width', 0);

            //生成标题栏的宽高
            var html = TabId.html();
            TableDivClass = TableDivClass.replace(/[\.\#]/g, '');
            console.log(TableDivClass);
            TabId.before('<div class=\"' + TableDivClass + '-Title\">' + html + '</div>');
            var tableHeader = $('.' + TableDivClass + '-Title');
            var resize = function () {
                TabId.css({
                    'border-bottom': '1px solid ' + borderColor,
                    'margin-top': -TabId.find('thead').height() - 1
                });
                tableHeader.css({
                    'width': TabId.width(),
                    'height': TabId.find('thead').height() + 1,
                    'overflow': 'hidden',
                    'position': 'relative',
                    "margin-left": TabId.css("margin-left"),
                    'z-index': '10'
                });
                if (TabId.find('table').width() != tableHeader.find('table').width()) {
                    tableHeader.find('table').width(TabId.find('table').width());
                }
            };
            $(window).on('resize', resize);
            resize();
        };
    };
});


