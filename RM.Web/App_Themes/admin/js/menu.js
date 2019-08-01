
//选项菜单显示当前页效果
//function menu(){
//var parent_event;
//for(var j=0;j<3;j++)
//{
//    var ul=document.getElementById("ul_menu_"+j);
//    var lis=ul.getElementsByTagName("li");
//    for(var i=0;i<lis.length;i++)
//    {
//       var li_a=lis[i].getElementsByTagName("a");
//       li_a[0].onclick=function()
//       {
//         var t =  (arguments[0]!=null)?arguments[0].target : window.event.srcElement
//         if(parent_event!=null)
//            parent_event.className="menu_box_a";//鼠标离开样式
//         parent_event=t;
//         parent_event.className="menu_box_a_click";//点击样式
//       }
//       li_a[0].onmouseover=function()
//       {
//        var t =  (arguments[0]!=null)?arguments[0].target : window.event.srcElement
//         //if(parent_event!=null && t!=parent_event)
//         if(t.className!="menu_box_a_click")
//            t.className="menu_box_a_hover";//鼠标进入样式
//       }
//       li_a[0].onmouseout=function()
//       {
//          var t =  (arguments[0]!=null)?arguments[0].target : window.event.srcElement
////          if(parent_event!=null && t==parent_event)
////            parent_event.className="menu_box_a_click";//点击样式
////          else
////            t.className="menu_box_a";//鼠标离开样式
////            
////            if(parent_event!=null && t!=parent_event)
////              t.className="menu_box_a";//鼠标离开样式
//if(t.className!="menu_box_a_click")
//            t.className="menu_box_a";//鼠标进入样式
//       }
//       li_a[0].onfocus=function()
//       {
//          var t =  (arguments[0]!=null)?arguments[0].target : window.event.srcElement
//          t.blur();//点击后失去焦点
//       }
//    }
// } 
// } 

//选项菜单效果
//function setTab(name,cursel,n){
//for(i=1;i<=n;i++){
//var menu=document.getElementById(name+i);
//var con=document.getElementById("con_"+name+"_"+i);
//menu.className=i==cursel?"hover":"";
//con.style.display=i==cursel?"block":"none";
//}
//}

 //提交按钮样式
function input_over(control)
{
	control.className="button_input_over"
}
function input_out(control)
{
	control.className="button_input_out"
}

//menu选项菜单效果
function switchTag(tag,content)
{
////	alert(tag);
////	alert(content);
//	for(i=1; i <4; i++)
//	{
//		if ("tag"+i==tag)
//		{
//			document.getElementById(tag).getElementsByTagName("a")[0].className="selectli"+i;
//			document.getElementById(tag).getElementsByTagName("a")[0].getElementsByTagName("span")[0].className="selectspan"+i;
//		}else{
//			document.getElementById("tag"+i).getElementsByTagName("a")[0].className="";
//			document.getElementById("tag"+i).getElementsByTagName("a")[0].getElementsByTagName("span")[0].className="";
//		}
//		if ("content"+i==content)
//		{
//			document.getElementById(content).className="";
//		}else{
//			document.getElementById("content"+i).className="hidecontent";
//		}
//		document.getElementById("content").className=content;
//	}
}

