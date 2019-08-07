var _artShow_currentDialog = null; //当前弹出的框

function artShow_ShowIFrameDialog(url, title, width, heigth, isDialog, closeHandle) {

    _artShow_currentDialog = art.dialog.open(url, { title: title, width: width, height: heigth, lock: isDialog,
        closeFn: function (here) {
            return closeHandle && closeHandle.call(this, here);
        }
    });
}
//关闭当前对话框
function artShow_CloseCurDialog() {
    if (_artShow_currentDialog != null) {
        _artShow_currentDialog.close();
    }
}
//关闭当前对话框
function artShow_AlertDialog(title, content, width, height, isDialog, icon, okHandle) {

    _artShow_currentDialog = art.dialog(
        {
            title: title,
            content: content,
            width: width,
            height: height,
            lock: isDialog,
            icon: icon,
            ok: function (here) {
                return okHandle && okHandle.call(this, here);
            }

        }
        );
}

/**
* 确认
* @param	{String}	消息内容
* @param	{Function}	确定按钮回调函数
* @param	{Function}	取消按钮回调函数
*/
function artShow_Confirm(title, content, width, height, yes, no) {
    _artShow_currentDialog = art.dialog({
        id: 'Confirm',
        zIndex: _zIndex(),
        icon: 'question',
        fixed: true,
        lock: true,
        opacity: .1,
        title: title,
        content: content,
        width: width,
        height: height,
        ok: function (here) {
            return yes.call(this, here);
        },
        cancel: function (here) {
            return no && no.call(this, here);
        }
    });

};

/**
* 提问
* @param	{String}	提问内容
* @param	{Function}	回调函数. 接收参数：输入值
* @param	{String}	默认值
*/
function artShow_Prompt(title, content, width, height, yes, value) {
    value = value || '';
    var input;

    _artShow_currentDialog = ({
        id: 'Prompt',
        zIndex: _zIndex(),
        icon: 'question',
        fixed: true,
        lock: true,
        opacity: .1,
        content: [
			'<div style="margin-bottom:5px;font-size:12px">',
				content,
			'</div>',
			'<div>',
				'<input value="',
					value,
				'" style="width:18em;padding:6px 4px" />',
			'</div>'
			].join(''),
        init: function () {
            input = this.DOM.content.find('input')[0];
            input.select();
            input.focus();
        },
        ok: function (here) {
            return yes && yes.call(this, input.value, here);
        },
        cancel: true
    });
};