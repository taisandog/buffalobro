var _currentDialog = null; //当前弹出的框

function artShow_ShowIFrameDialog(url, title, width, heigth, isDialog, closeHandle) {

    _currentDialog = art.dialog.open(url, { title: title, width: width, height: heigth, lock: isDialog,
        closeFn: closeHandle
    });
}
//关闭当前对话框
function artShow_CloseCurDialog() {
    if (_currentDialog != null) {
        _currentDialog.close();
    }
}
//关闭当前对话框
function artShow_AlertDialog(title, content, width, heigth, isDialog) {
    _currentDialog = art.dialog(
        {
            width: width,
            heigth: heigth,
            title: title,
            content: content,
            lock: isDialog
        }
        );
}
//关闭当前对话框
function artShow_AlertDialog(title, content, width, heigth, isDialog) {
    _currentDialog = art.dialog(
        {
            width: width,
            heigth: heigth,
            title: title,
            content: content,
            lock: isDialog
        }
        );
}
//关闭当前对话框
function artShow_AlertDialog(title, content, width, heigth, isDialog) {
    _currentDialog = art.dialog(
        {

            title: title,
            content: content,
            width: width,
            heigth: heigth,
            lock: isDialog
        }
        );
}

function artShow_ShowYesNo(title, content, width, heigth, isDialog, okHandle, cancelHandle) {
    _currentDialog = art.dialog(
        {
            title: title,
            content: content,
            width: width,
            heigth: heigth,
            lock: isDialog,
            ok: okHandle,
            cancel: cancelHandle
        });
}