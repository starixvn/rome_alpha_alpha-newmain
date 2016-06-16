//// do not use below code in new-main

//// Set value for old main => every share variables should be listed here
top.ph = alpha.user.passwordHash;
top.isNewSite = true;
top.errCodes = [];
top.errMsgs = [];
top.userNames = [];

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
    calculateWidthHeight: function (width, height, position) {
        var maxWidth = window.top.window.innerWidth;
        var maxHeight = window.top.window.innerHeight - (position ? position.top : 0) - 60;
        width = width > maxWidth ? maxWidth : width;
        height = height > maxHeight ? maxHeight : height;

        var minHeight = 250;
        minHeight = height < minHeight ? height : minHeight;
        if (height < minHeight) {
            if (minHeight < height) {
                height = minHeight;
            } else {
                height = height;
            }
        }

        return {
            width: width,
            height: height
        };
    },
    OpenPopUp: function (url, title, width, height, position) {
        var widthHeight = window.age.calculateWidthHeight(width, height, position);
        width = widthHeight.width;
        height = widthHeight.height;

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
        popUp.find(".modal-content .modal-body").css('overflow', 'hidden');

        var headerHeight = 40;
        var contentBorder = 2;
        popUp.find(".modal-content .modal-body").css("height", (popH - headerHeight - contentBorder) + "px");

        var iframe = popUp.find("#popup-iframe");

        var padding = 15 * 2;
        var verticalPadding = 8 * 2;
        iframe.css({
            width: popW - padding,
            height: popH - headerHeight - contentBorder - verticalPadding,
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
    },
    SignOut: function () {
        var index = window;
        while (index != index.parent) {
            index = index.parent;
        }

        index.location = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx';

        if (index.location.href.indexOf('https://imb.') == 0 ||
            index.location.href.indexOf('https://int.') == 0) {
            index.close();
        }
    }
};

age.CreateMaskDiv();

var AgeWnd = function () {
    return {
        popUp: [],
        callback: [],

        calculateWidthHeight: function (width, height, position) {
            var maxWidth = window.top.window.innerWidth;
            var maxHeight = window.top.window.innerHeight - (position ? position.top : 0) - 60;
            width = width > maxWidth ? maxWidth : width;
            height = height > maxHeight ? maxHeight : height;

            var minHeight = 250;
            minHeight = height < minHeight ? height : minHeight;
            if (height < minHeight) {
                if (minHeight < height) {
                    height = minHeight;
                } else {
                    height = height;
                }
            }

            return {
                width: width,
                height: height
            };
        },
        open: function (url, title, left, popUpTop, width, height, callBack) {
            popUpTop = popUpTop || 200;

            var widthHeight = this.calculateWidthHeight(width, height, { top: popUpTop });
            width = widthHeight.width;
            height = widthHeight.height;

            title = title || '';
            var popH = height || 360;
            popH = popH + 50;

            var oldSiteBodyWidthPadding = 34;
            var popW = (width + oldSiteBodyWidthPadding) || 480;
            left = left || 200;

            var popUp = this.popUp.length >= 1 ? this.popUp.pop() : $("#page-popup").clone();

            popUp.attr("id", "page-popup-" + Math.random().toString(36).substring(7));

            // Calculate body scrolltop to fix issue: focus popup make body scroll to popup position (on device)
            var scrollTop = oldMainUltilities.isSmallDevice() ? document.body.scrollTop : 0;

            popUp.css({
                width: popW,
                height: popH,
                left: left,
                top: popUpTop + scrollTop
            });

            popUp.appendTo("body");
            popUp.removeClass("hidden");
            popUp.find(".popup-title").html(title);
            popUp.find(".ex-modal").css("height", popH + "px");

            var headerHeight = 40;
            var contentBorder = 2;
            var bodyHeightPadding = 18;
            popUp.find(".modal-content .modal-body").css("height", (popH - headerHeight - contentBorder) + "px");

            var iframe = popUp.find("#popup-iframe");
            var iframeHeight = popH - headerHeight - contentBorder - bodyHeightPadding;
            ageWnd.setIframeInfo(iframe, popUp, iframeHeight, iframeHeight, left, popUpTop);
            if (url) {
                iframe.attr("src", url);
            } else {
                ageWnd.copyTargetForm(popUp);
                iframe = popUp.find('.modal-body').find('#popup-iframe');
                ageWnd.setIframeInfo(iframe, popUp, iframeHeight, iframeHeight, left, popUpTop);
            }

            setTimeout(function () {
                ageWnd.openPopUpAfterSetIframeSrc(popUp, callBack);
            }, 100);
        },
        openPopUpAfterSetIframeSrc: function (popUp, callBack) {
            popUp.modal({ show: true, backdrop: false });
            popUp.find(".icon-close").unbind("click").click(function () {
                popUp.find('.modal-body #target').remove();
                popUp.remove();
                if (callBack && typeof (callBack) == "function") {
                    callBack();
                }
            });

            popUp.tinyDraggable();

            // This will make elements on popup lose focus
            // Issue: click any point on popup trigger open keyboard/dropdown (default focus elements)
            popUp.blur();
            this.popUp.push(popUp);
            this.callback.push();
        },
        close: function () {
            var popUp = this.popUp.pop();
            popUp.find('.modal-body #target').remove();
            popUp.remove();

            var callBack = this.callback.pop();
            if (callBack && typeof (callBack) == "function") {
                callBack();
            }
        },
        setRect: function (left, top, width, height) {
        },
        createInstance: function () {
            return new AgeWnd();
        },

        copyTargetForm: function (popUp) {
            var oldTarget = top.main.document.getElementById("target");
            var newTarget = $.clone(oldTarget);

            popUp.find('.modal-body').find('#popup-iframe').remove();
            var iframeId = 'edit-setting-frame-' + Math.random().toString(36).substring(7);
            popUp.find('.modal-body').append('<iframe id="popup-iframe" frameborder="0" allowtransparency="yes" marginheight="0" marginwidth="0" scrolling="auto" name="' + iframeId + '"></iframe>');

            popUp.find('.modal-body').append(newTarget);
            newTarget.target = iframeId;
            newTarget.submit();
        },
        setIframeInfo: function (iframe, popUp, iframeHeight, left, top) {
            iframe.css({
                width: '100%',
                height: iframeHeight,
                left: left,
                top: top
            });

            iframe.load(function () {
                var title = popUp.find(".popup-title").html() || '';
                var iframe_doc = iframe.contents();

                var iframeTitle = iframe_doc.find("title").text();
                if (title == '' && iframeTitle != 'Home') {
                    popUp.find(".popup-title").html(iframeTitle);
                }
            });
        }
    }
};

window.ageWnd = window.ageWnd || new AgeWnd();

window.bootboxConfirm = function (message, handleElement, acceptCallback, cancelCallback, popUpOptions) {
    bootbox.confirm(message, function (result) {
        if (result && acceptCallback && typeof (acceptCallback) == "function") {
            acceptCallback();
        } else if (cancelCallback && typeof (cancelCallback) == "function") {
            cancelCallback();
        }
    });

    if (handleElement) {
        var dialog = $(".bootbox-confirm .modal-dialog");
        var position = menuBarManager.getPopupEditStatusPosition(handleElement, popUpOptions);
        dialog.css({
            left: position.left,
            top: position.top
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
        var dialog = $(".bootbox-alert .modal-dialog");
        var position = menuBarManager.getPopupEditStatusPosition(handleElement);

        dialog.css({
            left: position.left,
            top: position.top
        });
    }
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

//Javascript code snippet to create a div dynamically in Javascript.
function CreateDiv(id, html, className, left, top, position) {

    var newdiv = document.createElement('div');
    newdiv.setAttribute('id', id);

    if (position != "") {
        newdiv.style.position = position;
    }

    if (left != "") {
        newdiv.style.left = left;
    }
    if (top != "") {
        newdiv.style.top = top;
    }

    if (className != "") {
        newdiv.className = className;
    }

    if (html != "") {
        newdiv.innerHTML = html;
    }

    document.body.appendChild(newdiv);
}

function RemoveDiv(idNode) {
    var self = document.getElementById(idNode);
    if (self != null) {
        self.parentNode.removeChild(self);
    }
}

var menuBarManager = {
    getCustInfoScrollTop: function () {
        var isSmallDevice = oldMainUltilities.isSmallDevice();
        if (isSmallDevice) {
            return -top.$('.accountinfo-container').scrollTop();
        }

        return parseFloat(top.$('#mCSB_2_container').css('top').replace(/px/g, ''));
    },
    getMainFrameScrollTop: function () {
        var scrollTop = top.main.scrollY;
        if (scrollTop == undefined) {
            scrollTop = document.documentElement ?
                document.documentElement.scrollTop :
                document.body.scrollTop;
        }

        scrollTop = -scrollTop;

        return scrollTop;
    },


    getPopupEditStatusPosition_CustomerList: function (handleElement) {
        var handleElementPosition = oldMainUltilities.getOffset(handleElement);

        var startTop = 0;
        var scrollTop = menuBarManager.getMainFrameScrollTop();

        var popUpTop = 0;
        var left = 0;
        startTop = 146;
        popUpTop = startTop + handleElementPosition.top + scrollTop;
        left = handleElementPosition.left - 200 + top.$('.sidebar').width();

        var position = {
            left: left,
            top: popUpTop
        };

        var windowHeight = window.top.window.innerHeight;
        var maxTop = windowHeight - 200;
        if (position.top >= maxTop) {
            position.top = maxTop;
        }

        return position;
    },
    getPopupEditStatusPosition_LeftPanel: function (handleElement) {
        var handleElementPosition = oldMainUltilities.getOffset(handleElement);

        var startTop = 0;
        var scrollTop = menuBarManager.getCustInfoScrollTop();

        var popUpTop = 0;
        var left = 0;

        startTop = 164;
        var checkboxLeft = 3;
        var checkboxBottom = 3;
        popUpTop = startTop + handleElementPosition.top + checkboxBottom + scrollTop;
        left = handleElementPosition.left;

        var position = {
            left: left,
            top: popUpTop
        };

        var windowHeight = window.top.window.innerHeight;
        var maxTop = windowHeight - 200;
        if (position.top >= maxTop) {
            position.top = maxTop;
        }

        return position;
    },

    getPopupEditStatusPosition: function (handleElement, popUpOptions) {
        if (!handleElement) {
            return null;
        }

        if (popUpOptions && popUpOptions.type == 'DoubleComm_CustomerList') {
            return menuBarManager.getPopupEditStatusPosition_DoubleComm(handleElement);
        }

        if (handleElement.id == 'Popup') {
            return menuBarManager.getPopupEditStatusPosition_CustomerList(handleElement);
        }

        var position = menuBarManager.getPopupEditStatusPosition_LeftPanel(handleElement);
        if (popUpOptions && popUpOptions.type == 'TransferCondition') {
            position.left += top.$('.sidebar').width() + 3;
        }

        return position;
    },

    getPopupEditStatusPosition_DoubleComm: function (handleElement) {
        var handleElementPosition = oldMainUltilities.getOffset(handleElement);

        var startTop = 0;
        var scrollTop = menuBarManager.getMainFrameScrollTop();

        var popUpTop = 0;
        var left = 0;
        startTop = 111;
        popUpTop = startTop + handleElementPosition.top + scrollTop;
        left = handleElementPosition.left - 150 + top.$('.sidebar').width();

        var position = {
            left: left,
            top: popUpTop
        };

        var windowHeight = window.top.window.innerHeight;
        var maxTop = windowHeight - 200;
        if (position.top >= maxTop) {
            position.top = maxTop;
        }

        return position;
    }
};

var oldMainUltilities = {
    isSmallDevice: function () {
        var isSmallDevice = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
        return isSmallDevice;
    },
    getOffset: function (el) {
        var _x = 0;
        var _y = 0;
        while (el && !isNaN(el.offsetLeft) && !isNaN(el.offsetTop)) {
            if (el.style.display == 'none') {
                _x += parseFloat(el.style.left.replace(/px/g, '')) - el.scrollLeft;
                _y += parseFloat(el.style.top.replace(/px/g, '')) - el.scrollTop;
            } else {
                _x += el.offsetLeft - el.scrollLeft;
                _y += el.offsetTop - el.scrollTop;
            }

            el = el.offsetParent;
        }

        return { top: _y, left: _x };
    }
};