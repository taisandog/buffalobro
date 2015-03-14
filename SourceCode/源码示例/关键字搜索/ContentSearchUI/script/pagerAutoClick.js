function onPage(clientId,e)
{
    var eve=window.event||e;
    var keycode=eve.keyCode;
    if (keycode==13)
    {
	    var btn=document.getElementById(clientId);
        
        if(window.event)
        {
            eve.returnValue=false; //cancel Enter action
            btn.click();
	    }
        else
        {
            eve.preventDefault();
            var e = document.createEvent("MouseEvents"); 
            e.initEvent("click", true, true); 
            btn.dispatchEvent(e);
        }
        return false;
    }
    return true;
}
