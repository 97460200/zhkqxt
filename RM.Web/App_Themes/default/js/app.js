/**
 * Created by Sole on 2015/6/4.
 */
function $(id){
    return typeof id=="string"?document.getElementById(id):id;
}

window.onload = function(){
    var titleName = $("tab-title").getElementsByTagName("li");
    var tabConten = $("tab-content").getElementsByTagName("div");
    if(titleName.length != tabConten.length){
        return;
    }
    for(var i = 0;i<titleName.length;i++){
        titleName[i].id = i;
        titleName[i].onmouseover = function(){
            for(var j = 0;j<titleName.length;j++){
                titleName[j].className = "";  //ȥ������������ʾ
                tabConten[j].style.display = "none";  //�������������������ʾ
            }
            this.className="select";
            tabConten[this.id].style.display = "block";
        }
    }
}