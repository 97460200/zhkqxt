
    //全选
    function SelectAllCheckboxes(spanChk)
    {
        var oItem = spanChk.children;   
        var theBox= (spanChk.type=="checkbox") ? spanChk : spanChk.children.item[0];  
        xState=theBox.checked;  
        elm=theBox.form.elements;  
        for(i=0;i<elm.length;i++)   
        if(elm[i].type=="checkbox" &&elm[i].id!=theBox.id)  
        {     
          if(elm[i].checked!=xState)    
            elm[i].click();
        }
    }
    
    //弹出层用于移动信息。
        function show()
        {
            var divRemark = document.getElementById("divRemark");
            var back = document.getElementById("back");
            divRemark.style.display="";

            divRemark.style.left=(document.documentElement.clientWidth/2 - divRemark.style.width)/2 - document.documentElement.scrollLeft/2+100;
            //divRemark.style.top = (document.documentElement.clientHeight - divRemark.style.height)/2 + document.documentElement.scrollTop;
            divRemark.style.top = 350-document.documentElement.scrollTop/2 + document.documentElement.scrollTop;
            
            back.style.width = divRemark.style.width;
            back.style.height = divRemark.style.height;
            back.style.top = divRemark.style.top;
            back.style.left = divRemark.style.left;
            back.style.display = "";
        }
        function closeDiv()
        {
            document.getElementById("divRemark").style.display="none";
            document.getElementById("back").style.display="none";
        }
        function check()
        {
            //栏目必填    
            for(var i=0; i<document.getElementById('ddlToPath').options.length; i++) 
            {
                if(document.getElementById('ddlToPath').options[i].selected) 
                {
                    if(i==0)
                    {
                       alert('请选择栏目类型!');
                       return false;
                    }
                    else
                    {
                        var text = (document.getElementById('ddlToPath').options[i].innerHTML).split("|—");
                        var text2;
                        if(document.getElementById('ddlToPath').options[i+1] != null)
                        {
                            text2 = (document.getElementById('ddlToPath').options[i+1].innerHTML).split("|—");
                            if(text.length < text2.length)
                            {
                                alert('不可选择该栏目类型！');
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }