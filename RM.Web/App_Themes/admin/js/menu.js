
//ѡ��˵���ʾ��ǰҳЧ��
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
//            parent_event.className="menu_box_a";//����뿪��ʽ
//         parent_event=t;
//         parent_event.className="menu_box_a_click";//�����ʽ
//       }
//       li_a[0].onmouseover=function()
//       {
//        var t =  (arguments[0]!=null)?arguments[0].target : window.event.srcElement
//         //if(parent_event!=null && t!=parent_event)
//         if(t.className!="menu_box_a_click")
//            t.className="menu_box_a_hover";//��������ʽ
//       }
//       li_a[0].onmouseout=function()
//       {
//          var t =  (arguments[0]!=null)?arguments[0].target : window.event.srcElement
////          if(parent_event!=null && t==parent_event)
////            parent_event.className="menu_box_a_click";//�����ʽ
////          else
////            t.className="menu_box_a";//����뿪��ʽ
////            
////            if(parent_event!=null && t!=parent_event)
////              t.className="menu_box_a";//����뿪��ʽ
//if(t.className!="menu_box_a_click")
//            t.className="menu_box_a";//��������ʽ
//       }
//       li_a[0].onfocus=function()
//       {
//          var t =  (arguments[0]!=null)?arguments[0].target : window.event.srcElement
//          t.blur();//�����ʧȥ����
//       }
//    }
// } 
// } 

//ѡ��˵�Ч��
//function setTab(name,cursel,n){
//for(i=1;i<=n;i++){
//var menu=document.getElementById(name+i);
//var con=document.getElementById("con_"+name+"_"+i);
//menu.className=i==cursel?"hover":"";
//con.style.display=i==cursel?"block":"none";
//}
//}

 //�ύ��ť��ʽ
function input_over(control)
{
	control.className="button_input_over"
}
function input_out(control)
{
	control.className="button_input_out"
}

//menuѡ��˵�Ч��
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

