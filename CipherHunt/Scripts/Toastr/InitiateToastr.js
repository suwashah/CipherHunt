// <reference path="toastr.min.js" />
//------------TOASTR-------------//
function toastify(type, msg, title, position, showclosebutton) {
    if (position === null || position === '') {
        toastr.options.positionClass = 'toast-bottom-right';
    }
    else {
        toastr.options.positionClass = position;
    }
    toastr.options.closeButton = showclosebutton;
    toastr.options.timeOut = '5000';
    toastr.options.progressBar = false;
    switch (type) {
        case 'success': toastr.success(msg, title);
            break;
        case 'info': toastr.info(msg, title);
            break;
        case 'warning': toastr.warning(msg, title);
            break;
        case 'error': toastr.error(msg, title);
            break;
    }   
    //toastr.clear();
}
function toastrMessage(type, msg, title) {
    toastr.options.positionClass = 'toast-bottom-right';
    toastr.options.closeButton = true;
    toastr.options.timeOut = '5000';
    toastr.options.progressBar = false;
    switch (type) {
        case 'success': toastr.success(msg, title);
            break;
        case 'info': toastr.info(msg, title);
            break;
        case 'warning': toastr.warning(msg, title);
            break;
        case 'error': toastr.error(msg, title);
            break;
    }
    //toastr.clear();
}