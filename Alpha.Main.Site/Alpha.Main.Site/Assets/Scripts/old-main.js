//// do not use below code in new-main

//// Set value for old main => every share variables should be listed here
top.ph = alpha.user.passwordHash;
top.isNewSite = true;
top.errCodes = [];
top.baseUrl = alpha.urls.oldMainRootPath;
top.mainRootPath = alpha.urls.mainRootPath;
top.IsInternalWebVPN = $("#isInternalWebVPN");
top.AGE = top.AGE || {};
top.AGE.PERMISSIONS = alpha.user.subPageAccess;

//// Functions for Site-old only
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1, c.length);
        }

        if (c.indexOf(nameEQ) == 0) {
            return c.substring(nameEQ.length, c.length);
        }
    }

    return null;
}

function setCookie(name, value, days, domain) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else {
        var expires = "";
    }

    document.cookie = name + "=" + value + expires + "; path=/;" + ((typeof domain != 'undefined') ? ("domain=" + domain) : String.Empty);
}

window.age = window.age || {
    OpenPopUp: function (url, title, width, height, position) {
        title = title || '';
        var popH = height || 360;
        var popW = width || 480;
        var left = position ? position.left : 200;
        var top = position ? position.top : 200;

        var popUp = $("#page-popup");
        popUp.css({
            width: popW,
            height: popH,
            left: left,
            top: top
        });

        popUp.removeClass("hidden");
        popUp.find(".popup-title").html(title);
        popUp.find(".ex-modal").css("height", popH + "px");
        popUp.find(".modal-content .modal-body").css('padding', '8px 14px');

        var headerHeight = 40;
        var contentBorder = 2;
        var bodyHeightPadding = 38;
        popUp.find(".modal-content .modal-body").css("height", (popH - headerHeight - contentBorder) + "px");

        var iframe = popUp.find("#popup-iframe");

        var padding = 15 * 2;
        iframe.css({
            width: popW - padding,
            height: popH - headerHeight - contentBorder - bodyHeightPadding,
            left: left,
            top: top
        });
        iframe.attr("src", url);
        popUp.modal({ show: true, backdrop: false });
        popUp.find(".icon-close").unbind("click").click(function () {
            popUp.addClass("hidden");
        });

        iframe.load(function () {
            var title = popUp.find(".popup-title").html() || '';
            var iframe_doc = iframe.contents();

            var iframeTitle = iframe_doc.find("title").text();
            if (title == '' && iframeTitle != 'Home') {
                popUp.find(".popup-title").html(iframeTitle);
            }
        });
    },
    ClosePopUp: function () {
        var popUp = $("#page-popup");
        popUp.addClass("hidden");
    },
    Lock: function (withIcon) {
        age.ShowMaskDiv();
        age.locked = true;
    },
    UnLock: function () {
        age.HideMaskDiv();
        age.locked = false;
    },
    GetBaseUrl: function () {
        return top.baseUrl;
    },
    HideMaskDiv: function () {
        if (!age.divMaskLoading) {
            return;
        }

        age.divMaskLoading.style.display = 'none';
    },
    ShowMaskDiv: function () {
        if (!age.divMaskLoading) {
            return;
        }

        age.CalculateViewport();
        age.divMaskLoading.style.width = age.viewport.width - 2 + 'px';
        age.divMaskLoading.style.height = age.viewport.height - 2 + 'px';
        age.divMaskLoading.firstChild.style.left = Math.floor(age.viewport.width / 2) + 'px';
        age.divMaskLoading.firstChild.style.top = document.documentElement.scrollTop + 'px';
        age.divMaskLoading.className = 'mask-loading-div';
        age.divMaskLoading.style.display = 'block';
    },
    CreateMaskDiv: function () {
        this.divMaskLoading = age.createElement("div");
        var div1 = age.createElement("div");
        div1.style.width = '100px';
        div1.style.height = '100px';
        div1.style.position = 'relative';
        this.divMaskLoading.style.display = 'none';
        this.divMaskLoading.style.top = '0px';
        this.divMaskLoading.style.left = '0px';
        this.divMaskLoading.style.filter = 'alpha(opacity=50)';
        this.divMaskLoading.appendChild(div1);
        document.body.appendChild(this.divMaskLoading);
    },
    createElement: function (type, parent) {
        var el = null;
        if (document.createElementNS) {
            // use the XHTML namespace; IE won't normally get here unless
            // _they_ "fix" the DOM2 implementation.
            el = document.createElementNS("http://www.w3.org/1999/xhtml", type);
        } else {
            el = document.createElement(type);
        }

        if (typeof parent != "undefined") {
            parent.appendChild(el);
        }

        return el;
    },
    CalculateViewport: function () {
        age.viewport = { width: 0, height: 0 };
        if (!age.isIE()) {
            // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
            if (typeof document.documentElement != 'undefined' &&
                typeof document.documentElement.scrollWidth != 'undefined' &&
                document.documentElement.scrollWidth != 0) {
                age.viewport.width = document.documentElement.scrollWidth;
                age.viewport.height = document.documentElement.scrollHeight;
            }

            age.viewport.width = Math.max(age.viewport.width, Math.max(document.body.scrollWidth, document.body.clientWidth));
            age.viewport.height = Math.max(age.viewport.height, Math.max(document.body.scrollHeight, document.body.clientHeight));
        } else {
            // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
            if (typeof document.documentElement != 'undefined' &&
                typeof document.documentElement.clientWidth != 'undefined' &&
                document.documentElement.clientWidth != 0) {
                age.viewport.width = document.documentElement.clientWidth;
                age.viewport.height = document.documentElement.clientHeight;
            }

            age.viewport.width = Math.max(document.body.scrollWidth, age.viewport.width);
            age.viewport.height = Math.max(document.body.scrollHeight, age.viewport.height);
        }
    },
    isIE: function () {
        return (/msie/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent));
    }
};

age.CreateMaskDiv();

window.ageWnd = window.ageWnd || {
    open: function (url, title, left, top, width, height, callBack) {
        title = title || '';
        var popH = height || 360;

        var oldSiteBodyWidthPadding = 34;
        var popW = (width + oldSiteBodyWidthPadding) || 480;
        var left = left || 200;
        var top = top || 200;

        var popUp = $("#page-popup").clone();
        popUp.css({
            width: popW,
            height: popH,
            left: left,
            top: top
        });

        popUp.appendTo("body");
        popUp.removeClass("hidden");
        popUp.find(".popup-title").html(title);
        popUp.find(".ex-modal").css("height", popH + "px");

        var headerHeight = 40;
        var contentBorder = 2;
        var bodyHeightPadding = 38;
        popUp.find(".modal-content .modal-body").css("height", (popH - headerHeight - contentBorder) + "px");

        var iframe = popUp.find("#popup-iframe");

        iframe.css({
            width: '100%',
            height: popH - headerHeight - contentBorder - bodyHeightPadding,
            left: left,
            top: top
        });

        iframe.attr("src", url);
        popUp.modal({ show: true, backdrop: false });
        popUp.find(".icon-close").unbind("click").click(function () {
            popUp.remove();
            if (callBack && typeof (callBack) == "function") {
                callBack();
            }
        });

        iframe.load(function () {
            var title = popUp.find(".popup-title").html() || '';
            var iframe_doc = iframe.contents();

            var iframeTitle = iframe_doc.find("title").text();
            if (title == '' && iframeTitle != 'Home') {
                popUp.find(".popup-title").html(iframeTitle);
            }
        });
    },
};

window.bootboxConfirm = function (message, handleElement, acceptCallback, cancelCallback) {
    bootbox.confirm(message, function (result) {
        if (result && acceptCallback && typeof (acceptCallback) == "function") {
            acceptCallback();
        } else if (cancelCallback && typeof (cancelCallback) == "function") {
            cancelCallback();
        }
    });

    if (handleElement) {
        var $handleElement = $(handleElement);
        var dialog = $(".bootbox-confirm .modal-dialog");
        var position = $handleElement.position();
        var checkboxLeft = 3;
        var checkboxBottom = 3;
        var startTop = 164;

        dialog.css({
            left: position.left + checkboxLeft,
            top: startTop + position.top + checkboxBottom
        });
    }
};

window.bootboxAlert = function (message, handleElement, callbackFunction) {
    bootbox.alert(message, function () {
        if (callbackFunction && typeof (callbackFunction) == "function") {
            callbackFunction();
        }
    });

    if (handleElement) {
        var $handleElement = $(handleElement);
        var dialog = $(".bootbox-alert .modal-dialog");
        var position = $handleElement.position();
        var checkboxLeft = 3;
        var checkboxBottom = 3;
        var startTop = 164;

        dialog.css({
            left: position.left + checkboxLeft,
            top: startTop + position.top + checkboxBottom
        });
    }
};

var AGEWnd = function (docContext) {
    return top.ageWnd;
};

function ShowSecCodePopup(preUrl) {

    preUrl = 'url=' + escape(preUrl);
    var url = age.GetBaseUrl() + '_MemberInfo/SecurityCode/CheckSecurityCode.aspx?' + preUrl;
    var popH = 190, popW = 370, verticalOffset = -75;
    var x = (screen.width / 2) - (popW / 2) + verticalOffset;
    var y = (screen.height / 2) - (popH / 2);

    function unLockChildWindow(w) {
        if (w) {
            if (w.age) {
                w.age.UnLock();
            }
            var c = w.frames.length;
            for (var i = 0; i < c; i++) {
                if (w.frames[i].age) {
                    unLockChildWindow(w.frames[i]);
                }
            }
        }
    }

    top.age.Lock();
    top.ageWnd.open(url, '', y, x, popW, popH, function () {
        var c = top.frames.length;
        for (var i = 0; i < c; i++) {
            var w = top.frames[i];
            unLockChildWindow(w);
        }

        top.age.UnLock();
        return true;
    });
}
