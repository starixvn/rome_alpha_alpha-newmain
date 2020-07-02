//// do not use below code in new-main

//// Set value for old main => every share variables should be listed here
top.ph = alpha.user.passwordHash;
top.isNewSite = true;
top.errCodes = [];
top.errMsgs = [];
top.userNames = [];

top.baseUrl = alpha.urls.oldMainRootPath;
top.exTotalBetsForecastRootPath = alpha.urls.exTotalBetsForecastRootPath;
top.mainRootPath = alpha.urls.mainRootPath;
top.isEnableOtp = alpha.user.isEnableOtp;
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
        if (!oldMainUltilities.isIE()) {
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
    SignOut: function () {
        var index = window;
        while (index != index.parent) {
            index = index.parent;
        }

        var isInternal = index.$("#isInternal").val();
        var signOutUrl = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx';

        if (isInternal.toString() !== "0") {
            $.getJSON(signOutUrl);
        }
        else {
            index.location = signOutUrl;
        }

        if (isInternal.toString() !== "0") {
            setTimeout(index.close(), 1000);
        }
    },
    updatePopupTitleAfterLoad: function (popUp, iframe) {
        var title = popUp.find(".popup-title").html() || '';
        var iframe_doc = iframe.contents();

        var iframeTitle = iframe_doc.find("title").text();
        if (title == '' && iframeTitle != 'Home') {
            popUp.find(".popup-title").html(iframeTitle);
        }
    }
};

age.CreateMaskDiv();

var AgeWnd = function () {
    return {
        popUp: [],
        callback: [],
        id: null,
        iframeAttributes: null,
        calculateWidthHeight: function (width, height, position) {
            var realHeight = height;
            var maxWidth = window.top.window.innerWidth;

            var minHeight = 250;
            var maxHeight = window.top.window.innerHeight - (position ? position.top : 0) - 60;
            maxHeight = maxHeight < minHeight ? minHeight : maxHeight;

            width = width > maxWidth ? maxWidth : width;
            realHeight = realHeight > maxHeight ? maxHeight : realHeight;
            realHeight = realHeight < minHeight ? minHeight : realHeight;

            return {
                width: width,
                height: realHeight
            };
        },
        open: function (url, title, left, popUpTop, width, height, windowCss, titleCss, closeBtnCss, iframeAttributes, options) {
            options = options || {};
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

            var popupId = "page-popup-" + Math.random().toString(36).substring(7);
            popUp.attr("id", popupId);
            if (windowCss) {
                popUp.addClass(windowCss);
            }

            this.id = popupId;
            // Calculate body scrolltop to fix issue: focus popup make body scroll to popup position (on device)
            var scrollTop = oldMainUltilities.getScrollTop(window.document.body);
            var scrollLeft = oldMainUltilities.getScrollLeft(window.document.body);

            // ON iOS: there is scroll issues with popup. We must show popup with auto height
            var isAutoHeight = oldMainUltilities.isiOS();

            var popupCss = {
                width: popW,
                height: isAutoHeight ? 'auto' : popH,
                left: left + scrollLeft,
                top: popUpTop + scrollTop
            };

            if (options.resizable) {
                popupCss.overflow = "hidden";
            }

            if (isAutoHeight) {
                $.extend(popupCss, { 'overflow-y': 'visible', 'overflow-x': 'auto' });
            }

            popUp.css(popupCss);

            var iframe = popUp.find("#popup-iframe");
            iframe.attr("src", "about:blank");

            popUp.appendTo("body");
            popUp.removeClass("hidden");
            popUp.find(".popup-title").html(title);
            if (!isAutoHeight) {
                popUp.find(".ex-modal").css("height", popH + "px");
            }

            var headerHeight = 40;
            var contentBorder = 2;
            var bodyHeightPadding = 18;
            popUp.find(".modal-content .modal-body").css("height", isAutoHeight ? 'auto' : (popH - headerHeight - contentBorder) + "px");

            var iframeHeight = popH - headerHeight - contentBorder - bodyHeightPadding;
            ageWnd.setIframeInfo(iframe, popUp, iframeHeight, left, popUpTop, isAutoHeight);
            if (url) {
                iframe.attr("src", url);
            } else {
                ageWnd.copyTargetForm(popUp);
                iframe = popUp.find('.modal-body').find('#popup-iframe');
                ageWnd.setIframeInfo(iframe, popUp, iframeHeight, left, popUpTop, isAutoHeight);
            }

            if (iframeAttributes) {
                this.iframeAttributes = iframeAttributes;
                this.iframeAttributes.src = url;
            }

            ageWnd.openPopUpAfterSetIframeSrc(popUp, isAutoHeight, this, options);
        },
        openPopUpAfterSetIframeSrc: function (popUp, isAutoHeight, currentAgeWnd, options) {
            function getElementSize(element) {
                var style = element[0].style;
                return {
                    width: parseInt(style.width.substring(0, style.width.length - 2)),
                    height: parseInt(style.height.substring(0, style.height.length - 2))
                };
            }

            popUp.on('shown.bs.modal', function () {
                // transform property make objectFlash not show (MacauGames Result)
                popUp.find('.modal-dialog').css({
                    transform: 'none'
                });

                // This will make popup isn't cut its height when its height larger than browser height
                if (isAutoHeight) {
                    popUp.css({ display: 'table' });
                }
            });

            popUp.modal({ show: true, backdrop: false });

            if (options.resizable) {
                var prevPosition = {};
                var prevSize = {};

                popUp.resizable({
                    minHeight: 100,
                    minWidth: 100,
                    direction: ['right', 'bottom'],
                    start: function () {
                        prevPosition = popUp.offset();
                        prevSize = getElementSize(popUp);
                    },
                    resize: function () {
                        var size = getElementSize(popUp);

                        popUp.find(".ex-modal").css("height", size.height + "px");

                        var headerHeight = 40;
                        var contentBorder = 2;
                        var bodyHeightPadding = 18;
                        popUp.find(".modal-content .modal-body").css("height", (size.height - headerHeight - contentBorder) + "px");
                    },
                    stop: function () {
                        var size = getElementSize(popUp);

                        if (prevSize.width == size.width && prevSize.height == size.height) {
                            return;
                        }

                        prevSize = size;
                        popUp.offset(prevPosition);
                    }
                });
            }

            popUp.find(".icon-close").unbind("click").click(function () {
                popUp.find('.modal-body #target').remove();
                popUp.remove();

                if (currentAgeWnd.onClosing && typeof (currentAgeWnd.onClosing) == "function") {
                    currentAgeWnd.onClosing();
                }
            });

            popUp.tinyDraggable({
                exclude: "a.icon-information-outline.icon-info, button.icon.icon-close.close",
                resizeElement: options.resizable ? ".resizable-handle" : ""
            });

            currentAgeWnd.popUp.push(popUp);
        },
        close: function () {
            var popUp = this.popUp.pop();
            if (!popUp) {
                return;
            }
            popUp.find('.modal-body #target').remove();
            popUp.remove();

            if (this.onClosing && typeof (this.onClosing) == "function") {
                this.onClosing();
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
        setIframeInfo: function (iframe, popUp, iframeHeight, left, top, isAutoHeight) {
            iframe.css({
                width: '100%',
                height: '100%',
                left: left,
                top: top
            });

            iframe.load(function () {
                ageWnd.updatePopupTitleAfterLoad(popUp, iframe);

                if (oldMainUltilities.isSmallDevice()) {
                    // This will make elements on popup lose focus
                    // Issue: click any point on popup trigger open keyboard/dropdown (default focus elements)
                    iframe.contents().find(':focus').blur();
                }

                ageWnd.fixIOSHeight(iframe);
            });
        },
        updatePopupTitleAfterLoad: function (popUp, iframe) {
            var title = popUp.find(".popup-title").html() || '';
            var iframe_doc = iframe.contents();

            var iframeTitle = iframe_doc.find("title").text();
            if (title == '' && iframeTitle != 'Home') {
                popUp.find(".popup-title").html(iframeTitle);
            }
        },
        fixIOSHeight: function (iframe) {
            var isAutoHeight = oldMainUltilities.isiOS();
            if (!isAutoHeight) {
                return;
            }

            var fixHeightInterval = setInterval(function () {
                var iframe0 = iframe[0];
                var iframeDoc = iframe0.contentDocument || (iframe0.contentWindow ? iframe0.contentWindow.document : null);
                if (!iframeDoc) {
                    return;
                }

                var isReady = iframeDoc.readyState == 'complete' || iframeDoc.readyState == 'interactive';
                if (!isReady) {
                    return;
                }

                var iframeBody = iframe.contents().find('body');
                var bodyCss = {
                    height: 'auto',
                    position: "relative",
                    float: 'left'
                };

                iframeBody.css(bodyCss);

                if (iframeBody.height()) {
                    iframeBody.css(bodyCss);
                    var maxHeight = $(window).height() - 200;
                    var minHeight = iframeBody.height() + 100;

                    iframe.css({
                        "min-height": Math.min(minHeight, maxHeight) + "px"
                    });

                    iframeBody.css({
                        float: 'none'
                    });

                    clearInterval(fixHeightInterval);
                }
            }, 200);
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

    var dialog = $(".bootbox-confirm .modal-dialog");
    var position = getBootboxPostion(handleElement, popUpOptions, dialog);
    dialog.css({
        left: position.left,
        top: position.top
    });
};

window.bootboxAlert = function (message, handleElement, callbackFunction, popUpOptions) {
    bootbox.alert(message, function () {
        if (callbackFunction && typeof (callbackFunction) == "function") {
            callbackFunction();
        }
    });

    var dialog = $(".bootbox-alert .modal-dialog");
    var position = getBootboxPostion(handleElement, popUpOptions, dialog);

    dialog.css({
        left: position.left || 0,
        top: position.top || 0
    });
};

function getBootboxPostion(handleElement, popUpOptions, dialog) {
    if (popUpOptions && popUpOptions.position) {
        return popUpOptions.position;
    }

    if (handleElement) {
        return menuBarManager.getPopupEditStatusPosition(handleElement, popUpOptions);
    }

    var left = $(window).width() / 2 - dialog.width() / 2;
    return {
        left: left < 0 ? 0 : left,
        top: 100
    };
}

function ShowSecCodePopup(preUrl, callerAge) {
    //preUrl = 'url=' + escape(preUrl);
    //var url = age.GetBaseUrl() + '_MemberInfo/SecurityCode/CheckSecurityCode.aspx?' + preUrl;
    var url = top.mainRootPath + (alpha.user.isEnableOtp ? 'otp/verifyotp' : 'SecurityCode/VerifySecurityCode');
    var popH = 190, popW = 396, verticalOffset = -75;
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

    var popupSecurityCode = top.ageWnd.createInstance();
    popupSecurityCode.onClosing = function () {
        try {
            top.age.UnLock();

            if (callerAge) {
                callerAge.UnLock();
            }
        } catch (e) {
        }

        return true;
    };

    top.securityCodePopup = popupSecurityCode;

    top.popupManager.openWithHanlder(top.securityCodePopup, url, '', popW, popH, null, null, null, null, null, {
        preUrl: preUrl
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

    getPopupEditStatusPosition_CustomerList: function (handleElement) {
        var handleElementPosition = oldMainUltilities.getOffset(handleElement);

        var startTop = 0;

        var popUpTop = 0;
        var left = 0;
        startTop = 146;
        popUpTop = startTop + handleElementPosition.top;
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
        var popUpTop = 0;
        var left = 0;
        startTop = 111;
        popUpTop = startTop + handleElementPosition.top;
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
    isIE: function () {
        return (/(MSIE|Trident\/|Edge\/)/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent));
    },
    isSmallDevice: function () {
        var isSmallDevice = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
        return isSmallDevice;
    },
    isiOS: function () {
        var isiOS = /iPhone|iPad|iPod/i.test(navigator.userAgent);
        return isiOS;
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
    },
    getScrollLeft: function (element) {
        return element.pageXOffset || element.scrollX || element.scrollLeft || 0;
    },
    getScrollTop: function (element) {
        return element.pageYOffset || element.scrollY || element.scrollTop || 0;
    }
};

function reloadPopUpSetting(timeOut) {
    var popUp = top.ageWnd.popUp[top.ageWnd.popUp.length - 1];
    if (typeof (popUp) === 'undefined') {
        if (timeOut) {
            setTimeout(function () {
                window.top.main.location.reload(true);
            }, timeOut);
        }
        else {
            window.top.main.location.reload(true);
        }

        return;
    }
    var iframes = popUp.find('#popup-iframe');
    var target = popUp.find('.modal-body #target');

    if (timeOut) {
        setTimeout(function () {
            doReloadMainFrame();
        }, timeOut);
    } else {
        doReloadMainFrame();
    }

    function doReloadPopup() {
        if (iframes && iframes.length && iframes.attr('src')) {
            iframes.attr('src', iframes.attr('src'));
        } else if (target && target.length) {
            var isForcingModeElement = target.find("#isForcingMode");
            var isForcingModeCustomerListElement = top.main.$("isForcingMode");

            if (isForcingModeElement && isForcingModeCustomerListElement) {
                isForcingModeElement.val(isForcingModeCustomerListElement.value);
            }

            target.submit();
        }
    }

    function doReloadMainFrame() {
        var isCustomerList = top.main.$("arrayCustID")
            && top.main.$("arrayCustID").value
            && top.main.$("arrayCustID").value.length
            && top.main.$("arrayUserName")
            && top.main.$("arrayUserName").value
            && top.main.$("arrayUserName").value.length;

        if (isCustomerList) {
            var arrayCustId = top.main.$("arrayCustID").value;
            var arrayUserName = top.main.$("arrayUserName").value;
            //// Use load.oldmain here to do not override onload funtion on new main, please do not change the event name
            window.top.$('#main')
                .on('load.oldmain', function () {
                    top.main.$("arrayCustID").value = arrayCustId;
                    top.main.$("arrayUserName").value = arrayUserName;
                    doReloadPopup();

                    window.top.$('#main').off('load.oldmain');
                });
        } else {
            window.top.$('#main')
                .on('load.oldmain', function () {
                    doReloadPopup();
                    window.top.$('#main').off('load.oldmain');
                });
        }
        window.top.main.location.reload(true);
        window.top.clearPopupSubtitle(true);
    }
}

function hasPopupOpen() {
    var popUp = top.$('.modal[id^="page-popup"]');
    var popUpLength = popUp.length;
    var hasPopUp = !top.$(popUp[popUpLength - 1]).hasClass('hidden');

    return hasPopUp;
}

function setSecurityCodePopupTitle(securityCodeTitle) {
    var popupTitle = top.$('.modal[id^="page-popup"]').find('#popup-title');

    if (popupTitle.length > 0) {
        var previousTitle = popupTitle.length > 1 ? $(popupTitle[1]).text() : $(popupTitle[0]).text();
        popupTitle.text(securityCodeTitle);
        popupTitle.attr("data-previous-title", previousTitle);
    }
}

function revertSecurityCodePopupTitle() {
    var popupTitle = top.$('.modal[id^="page-popup"]').find('#popup-title');

    if (popupTitle.length > 0) {
        var previousTitle = popupTitle.attr("data-previous-title");
        var currentTitle = popupTitle.length > 1 ? $(popupTitle[1]).text() : $(popupTitle[0]).text();
        if (previousTitle != currentTitle) {
            popupTitle.text(previousTitle);
        }
        else {
            popupTitle.text("");
        }
    }
}

function clearPopupSubtitle(hasSubTitle) {
    var popUps = top.$('.modal[id^="page-popup"]');
    var popUpLength = popUps.length;
    var hasPopUp = !top.$(popUps[popUpLength - 1]).hasClass('hidden');

    if (!hasPopUp) {
        return;
    }

    var popUp = top.$(popUps[popUpLength - 1]);
    var iframes = popUp.find('#popup-iframe');
    var iframe = iframes && iframes.length ? iframes[0] : null;

    if (!iframe) {
        return;
    }

    var timer = setInterval(function () {
        var iframeDoc = iframe.contentDocument || (iframe.contentWindow ? iframe.contentWindow.document : null);
        if (!iframeDoc) {
            return;
        }

        var isReady = iframeDoc.readyState == 'complete' ||
            iframeDoc.readyState == 'interactive';
        if (!isReady) {
            return;
        }

        clearSubTitleAfterInframeLoad(iframe);
        clearInterval(timer);
    }, 200);

    top.ageWnd.fixIOSHeight($(iframe));

    iframe.onload = function () {
        clearSubTitleAfterInframeLoad(iframe);
        clearInterval(timer);
    };

    function clearSubTitleAfterInframeLoad(iframe) {
        var titles = iframe.contentWindow.document.getElementsByClassName('setting-header-agency');
        var header_main = titles && titles.length ? titles[0] : null;

        header_main = header_main || iframe.contentWindow.document.getElementById('header_main');
        header_main = header_main || iframe.contentWindow.document.getElementById('race-book-positiontaking-header');
        header_main = header_main || iframe.contentWindow.document.getElementById('title_header');
        header_main = header_main || iframe.contentWindow.document.getElementById('button_hr');

        if (header_main) {
            header_main.style.display = 'none';
        }

        if (hasSubTitle) {
            var subTitles = iframe.contentWindow.document.getElementsByClassName('subTitle');
            if (subTitles && subTitles.length) {
                subTitles[0].style.display = 'none';
            }
        }
    }
}

function closePopupAndReloadMainFrameAfterUpdateSuccess(timeOut, afterLoadMainFrameFunction) {
    var popUp = top.ageWnd.popUp[top.ageWnd.popUp.length - 1];
    var iframes = popUp.find('#popup-iframe');
    var iframeSrc = iframes && iframes.length && iframes.attr('src');

    function blurElementToScrollIframe() {
        top.main.$('txtUserName').blur();
    }

    function getScrollY(element) {
        return element.pageYOffset || element.scrollY || element.scrollTop || 0;
    }

    function storeAndRestoreFrameData(isCustomerList) {
        var mainFrame = top.main;
        var $main = top.$('#main');

        var scrollYMainFrame = getScrollY(mainFrame);
        var scrollYTopBody = getScrollY(top.document.body);

        var arrayCustId = '';
        var arrayUserName = '';

        if (isCustomerList) {
            arrayCustId = mainFrame.$("arrayCustID").value;
            arrayUserName = mainFrame.$("arrayUserName").value;
        };

        $main.on('load.oldmain', function () {
            if (isCustomerList) {
                mainFrame.$("arrayCustID").value = arrayCustId;
                mainFrame.$("arrayUserName").value = arrayUserName;
            }

            blurElementToScrollIframe();

            $main[0].contentWindow.scrollTo(0, scrollYMainFrame);
            top.window.scrollTo(0, scrollYTopBody);

            $main.off('load.oldmain');
        });
    }

    function doActionAfterTimeOut() {
        if (hasPopupOpen()) {
            top.ageWnd.close();
        }

        var mainFrame = top.main;
        var isCustomerList = mainFrame.document.getElementById("arrayCustID") &&
            mainFrame.document.getElementById("arrayCustID").value.length &&
            mainFrame.document.getElementById("arrayUserName") &&
            mainFrame.document.getElementById("arrayUserName").value.length;
        storeAndRestoreFrameData(isCustomerList);

        top.main.location.reload(true);

        window.top.clearPopupSubtitle(true);
    }

    if (timeOut) {
        setTimeout(function () {
            doActionAfterTimeOut();
        }, timeOut);
    } else {
        doActionAfterTimeOut();
    }
}

function reloadPage() {
    setTimeout("window.location.href = window.location.href", 2000);
}

var popupManager = {
    positionPadding: 5,
    leftPosition: function () {
        // top-left main frame
        var accountInfoWidth = top.$('.sidebar').width();

        return accountInfoWidth + top.popupManager.positionPadding;
    },

    topPosition: function () {
        // top-left main frame
        return 90 + top.popupManager.positionPadding;
    },

    // Choose most suitable screen resolution 1920 X 1080
    defaultHeight: 1000,

    defaultWidth: 480,
    openByRelativeUrl: function (url, title, width, height, windowCss, titleCss, closeBtnCss, left, top, iframeAttributes, options) {
        window.top.popupManager.open(age.GetBaseUrl() + url, title, width, height, windowCss, titleCss, closeBtnCss, left, top, iframeAttributes, options);
    },
    openMultipleByRelativeUrl: function (url, title, width, height, windowCss, titleCss, closeBtnCss, left, top, iframeAttributes, options) {
        window.top.popupManager.openMultiple(age.GetBaseUrl() + url, title, width, height, windowCss, titleCss, closeBtnCss, left, top, iframeAttributes, options);
    },
    open: function (url, title, width, height, windowCss, titleCss, closeBtnCss, left, top, iframeAttributes, options) {
        var handler = window.top.ageWnd;
        window.top.popupManager.openWithHanlder(handler, url, title, width, height, windowCss, titleCss, closeBtnCss, left, top, iframeAttributes, options);
    },
    openMultiple: function (url, title, width, height, windowCss, titleCss, closeBtnCss, left, top, iframeAttributes, options) {
        var handler = window.top.ageWnd.createInstance();
        window.top.popupManager.openWithHanlder(handler, url, title, width, height, windowCss, titleCss, closeBtnCss, left, top, iframeAttributes, options);
    },
    openWithHanlder: function (handler, url, title, width, height, windowCss, titleCss, closeBtnCss, left, top, iframeAttributes, options) {
        left = left || window.top.popupManager.leftPosition();
        top = top || window.top.popupManager.topPosition();

        width = width || window.top.popupManager.defaultWidth;
        height = height || window.top.popupManager.defaultHeight;
        title = title || '';

        handler.open(url, title, left, top, width, height, windowCss, titleCss, closeBtnCss, iframeAttributes, options);
    },
    close: function () {
        top.ageWnd.close();
    },
    closeAllPopup: function () {
        top.$('.modal[id^="page-popup-"]').remove();
    }
};
