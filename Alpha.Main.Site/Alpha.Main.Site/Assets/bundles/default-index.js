var alpha = {
    urls: {
        oldMainRootPath: top.document.getElementById('oldMainRootPath') != null
            ? top.document.getElementById('oldMainRootPath').value
            : "not found",
        mainRootPath: top.document.getElementById('mainRootPath') != null
            ? top.document.getElementById('mainRootPath').value
            : "not found",
        accountInfoRootPath: top.document.getElementById('accountInfoRootPath') != null
            ? top.document.getElementById('accountInfoRootPath').value
            : "not found",
        memberInfoRootPath: top.document.getElementById('memberInfoRootPath') != null
            ? top.document.getElementById('memberInfoRootPath').value
            : "not found",
        howToUseRootPath: top.document.getElementById('howToUseRootPath') != null
            ? top.document.getElementById('howToUseRootPath').value
            : "not found",
        exTotalBetsForecastRootPath: top.document.getElementById('exTotalBetsForecastRootPath') != null
            ? top.document.getElementById('exTotalBetsForecastRootPath').value
            : "not found",
        messageRootPath: top.document.getElementById('messageRootPath') != null
            ? top.document.getElementById('messageRootPath').value
            : "not found"
    },
    user: {
        onUserId: document.getElementById('onUserId') != null
            ? document.getElementById('onUserId').value
            : "not found",
        passwordHash: document.getElementById('passwordHash') != null
            ? document.getElementById('passwordHash').value
            : "not found",
        subPageAccess: document.getElementById('subPageAccess') != null
            ? document.getElementById('subPageAccess').value
            : "not found",
        level: document.getElementById('userLevel') != null
            ? document.getElementById('userLevel').value
            : "not found",
        isSubAccount: document.getElementById('isSubAccount') != null
            ? (document.getElementById('isSubAccount').value.toLowerCase() === "true")
            : "not found",
        isShowIntroduction: document.getElementById('isShowIntroduction') != null
            ? (document.getElementById('isShowIntroduction').value.toLowerCase() === "true")
            : "not found",
        isEnableOtp: document.getElementById('isEnabledOtp') != null
            ? (document.getElementById('isEnabledOtp').value.toLowerCase() === "true")
            : false
    },
    core: {
        convertReponseToJson: function (reponse) {
            return eval("(" + reponse + ")");
        },
        signOut: function () {
            var index = window;
            while (index != index.parent) {
                index = index.parent;
            }
            var isInternal = index.$("#isInternal").val();
            var signOutUrl = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx';
            if (isInternal.toString() !== "0") {
                $.ajax(signOutUrl, {
                    success: function () {
                        index.close();
                    }
                });
            }
            else {
                index.location = signOutUrl;
            }
        },
        isIe: function () {
            return /(MSIE|Trident\/|Edge\/)/i.test(navigator.userAgent);
        },
        IsIe8OrLower: function () {
            var userAgent = window.navigator.userAgent.toLowerCase();
            return (userAgent.indexOf('msie') != -1)
                ? (parseInt(userAgent.split('msie')[1]) < 9)
                : false;
        },
        IsAndroid: function () {
            return /Android/i.test(navigator.userAgent);
        }
    },
    isScreenGreater1024: false,
    isScreenLower768: false,
    isScreenGreater768AndLower1024: false,
    isScreenLower1024: false,
    isMobile: false,
    detectResponsiveInformation: function () {
        if (alpha.core.isIe()) {
            //// Work arround because in IE, bootstrap modal dont delegate focusin event to others.
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        }
        var currentWindowWidth = window.screen.width;
        if (currentWindowWidth < 768) {
            alpha.isScreenLower768 = true;
        }
        else if (currentWindowWidth <= 1024) {
            alpha.isScreenGreater768AndLower1024 = true;
        }
        else {
            alpha.isScreenGreater1024 = true;
        }
        alpha.isMobile = isMobile.any;
    },
    localStorage: {
        get: function (key) {
            return store.enabled ? store.get(key) : $.cookie(key);
        },
        set: function (key, value) {
            store.enabled ? store.set(key, value) : $.cookie(key, value, alpha.localStorage.getCookieExpireOptions());
        },
        check: function (key) {
            return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
        },
        getCookieExpireOptions: function () {
            var cookieExpire = { expires: 365, path: window.location.pathname };
            if (alpha.core.isIe()) {
                cookieExpire = { expires: 365 };
            }
            return cookieExpire;
        },
    },
    showMessage: function ($target, result) {
        $target.empty();
        if (result.isSuccessful) {
            $target.html("<div class='msg msg-success'>" + result.message + "</div>");
            return;
        }
        if (result.isWarning) {
            $target.html("<div class='msg msg-warning'>" + result.message + "</div>");
            return;
        }
        if (result.isFailed) {
            $target.html("<div class='msg msg-error'>" + result.message + "</div>");
            return;
        }
    },
    key: {
        instruction: 'Instruction',
        mobileInstruction: 'MobileInstruction'
    },
    messageType: {
        Normal: 0,
        Personal: 1,
        Special: 2,
        System: 3
    }
};
$(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var antiXSRFToken = document.getElementsByName("__RequestVerificationToken");
            if (antiXSRFToken.length > 0) {
                xhr.setRequestHeader("__RequestVerificationToken", antiXSRFToken[0].value);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status === 401) {
                alert('Oops... you are here because of a system error, please notify the administrators.');
                alpha.core.signOut();
            }
        }
    });
});
var Result = /** @class */ (function () {
    function Result(data) {
        this.code = data.Code;
        this.message = data.Message || "";
        this.isFailed = this.code !== 0;
        this.isSuccessful = this.code === 0;
        this.isWarning = data.Code === Result.warningCode;
    }
    Result.warningCode = -1000;
    return Result;
}());
//# sourceMappingURL=core.js.map
/// <reference path="menu-bar.ts" />
if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (prefix) {
        return this.slice(0, prefix.length) == prefix;
    };
}
'use strict';
/*global alpha, briefProfile */
var indexManager = {
    currentTab: null,
    currentUrlCookieName: 'DFFKDHFJKLDHFJDH',
    requiredNetworkSpeedCalculation: false,
    showOldCustomerList: false,
    domElements: {
        mainFrame: null,
        partialContents: null,
        content: null,
        accountInfoContent: null,
        currentURLCookieName: null,
        pagePopup: null
    },
    urls: {
        reCheckinUrl: alpha.urls.oldMainRootPath + '_Authorization/Handlers/ReCheckin.ashx'
    },
    init: function () {
        alpha.detectResponsiveInformation();
        if (!alpha.core.IsIe8OrLower()) {
            $.material.init();
        }
        indexManager.cachedDomElements();
        indexManager.loadPartialViews();
        indexManager.registerEvents();
        indexManager.setCurrentUrl();
        if (indexManager.requiredNetworkSpeedCalculation) {
            setTimeout(indexManager.calculateUserConnectionSpeed, 8000);
        }
    },
    registerEvents: function () {
        indexManager.domElements.pagePopup.tinyDraggable();
        setTimeout(indexManager.reCheckin, 30000);
    },
    cachedDomElements: function () {
        indexManager.domElements.partialContents = $('.partial-content');
        indexManager.domElements.content = $('#content');
        indexManager.domElements.accountInfoContent = $('#accountinfo-content');
        indexManager.domElements.currentURLCookieName = $('#currentURLCookieName');
        indexManager.domElements.pagePopup = $('#page-popup');
        indexManager.currentUrlCookieName = indexManager.domElements.currentURLCookieName.val();
        indexManager.requiredNetworkSpeedCalculation = $('#requiredNetworkSpeedCalculation').val() == '1';
        indexManager.showOldCustomerList = $('#showOldCustomerList').val() == '1';
    },
    changeMenu: function (url) {
        indexManager.domElements.mainFrame.attr('src', url);
    },
    reCheckin: function () {
        // Post with return result in JSON format
        $.cookie('ssl', location.href.indexOf('https://') === 0, { expires: 30, path: '/' });
        $.ajax({
            type: "POST",
            url: indexManager.urls.reCheckinUrl,
            data: { onid: alpha.user.onUserId },
            success: function (result) {
                try {
                    result = alpha.core.convertReponseToJson(result);
                    var errCode = result.errCode;
                    var errMsg = result.errMsg;
                    if (errCode !== 0) {
                        if (errMsg) {
                            alert(errMsg);
                        }
                        else {
                            alert('Oops... you are here because of a system error, please notify the administrators.');
                        }
                        alpha.core.signOut();
                    }
                }
                catch (e) {
                    if (typeof (result) === "string"
                        && result.indexOf("Oops... you are here because of a system error") > 0) {
                        alpha.core.signOut();
                        alert('Oops... you are here because of a system error, please notify the administrators.');
                    }
                }
            },
            error: function (xhr) {
                // If internal server error
                if (xhr.status === 500) {
                    alpha.core.signOut();
                    alert('Oops... you are here because of a system error, please notify the administrators.');
                }
            },
            complete: function () {
                setTimeout(indexManager.reCheckin, 30000);
            }
        });
    },
    loadPartialViews: function () {
        indexManager.domElements.partialContents.each(function (index, partialContent) {
            var $partialContent = $(partialContent), url = $partialContent.data('url');
            if (url && url.length > 0) {
                $partialContent.load(url + '?' + Math.random(), function () {
                    if ($partialContent.hasClass('customer-profile')) {
                        briefProfile.init();
                    }
                });
            }
        });
    },
    cacheCurrentUrl: function () {
        var currentUrl = document.getElementById("main").contentWindow.location.href;
        $.cookie(indexManager.currentUrlCookieName, currentUrl, { expires: 1, path: '/' });
    },
    setCurrentUrl: function () {
        var currentUrl = indexManager.domElements.content.data('default-url');
        if (indexManager.domElements.mainFrame
            && indexManager.domElements.mainFrame.length > 0) {
            indexManager.domElements.mainFrame.attr('src', currentUrl);
        }
        else {
            indexManager.domElements.content.append('<iframe frameborder="0" id="main" src="' + currentUrl + '" width="100%" height="100%" scrolling="auto" name="main" marginwidth="0" marginheight="0" allowtransparency="true" class="iframe-content"></iframe>');
            indexManager.domElements.mainFrame = $('#main');
            indexManager.domElements.mainFrame
                .on('load.newmain', function () {
                var iframeWindow = indexManager.domElements.mainFrame[0].contentWindow || indexManager.domElements.mainFrame[0].contentDocument;
                indexManager.closeAllOpenDropdowns(iframeWindow);
                indexManager.closeAllPopup();
                indexManager.cacheCurrentUrl();
                $(iframeWindow.document).on('click touchend', function () {
                    menuBar.collapseSideBar();
                });
                if (alpha.isMobile) {
                    $(iframeWindow.document).find(':focus').blur();
                }
            });
            indexManager.domElements.accountInfoContent.load(function () {
                var iframeWindow = indexManager.domElements.accountInfoContent[0].contentWindow ||
                    indexManager.domElements.accountInfoContent[0].contentDocument;
                indexManager.closeAllOpenDropdowns(iframeWindow);
            });
        }
    },
    closeAllOpenDropdowns: function (iframeWindow) {
        var closeDropdownSubMenu = function () {
            $('.dropdown-submenu').find('.dropdown-menu').hide();
        };
        $(document).on('click touchend', closeDropdownSubMenu);
        $('#btn-header-menu').on('click', closeDropdownSubMenu);
        $(iframeWindow.document).on('click touchend', function () {
            $('.dropdown-toggle').each(function (index, dropdown) {
                var $dropdown = $(dropdown);
                if ($dropdown.parent().hasClass('open')) {
                    $dropdown.dropdown('toggle');
                }
            });
            closeDropdownSubMenu();
        });
    },
    closeAllPopup: function () {
        var prevUrl = $.cookie(indexManager.currentUrlCookieName) || "";
        var currentUrl = document.getElementById("main").contentWindow.location.href || "";
        var prevUrlWithoutQueryString = prevUrl.split("?")[0];
        var currentUrlWithoutQueryString = currentUrl.split("?")[0];
        var isCloseAllPopup = !this.isSameUrl(prevUrlWithoutQueryString, currentUrlWithoutQueryString);
        if (isCloseAllPopup) {
            popupManager.closeAllPopup();
        }
    },
    isSameUrl: function (url1, url2) {
        url1 = url1.toLowerCase();
        url2 = url2.toLowerCase();
        if (url1 === url2) {
            return true;
        }
        var minLength = Math.min(url1.length, url2.length);
        var diff1 = url1.substring(minLength);
        diff1 = diff1.startsWith("/") ? diff1.substring(1) : diff1;
        diff1 = diff1 || "index";
        var diff2 = url2.substring(minLength);
        diff2 = diff2.startsWith("/") ? diff2.substring(1) : diff2;
        diff2 = diff2 || "index";
        return url1.substring(0, minLength) == url2.substring(0, minLength) && diff1 === "index" && diff2 === "index";
    },
    calculateUserConnectionSpeed: function () {
        var iframe = document.getElementById('connection-speed');
        if (typeof (iframe) == 'undefined'
            || iframe == null) {
            iframe = document.createElement('iframe');
            iframe.id = 'connection-speed';
            iframe.style.display = 'none';
            iframe.src = alpha.urls.mainRootPath + "NetworkSpeed/Index";
            document.body.appendChild(iframe);
        }
    }
};
$(function () {
    indexManager.init();
});
//# sourceMappingURL=index-manager.js.map
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
            $.ajax(signOutUrl, {
                success: function () {
                    index.close();
                }
            });
        }
        else {
            index.location = signOutUrl;
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

/// <reference path="../core.ts" />
/// <reference path="index-manager.ts" />
'use strict';
var headerNav = {
    domElements: {
        isDisableToCheckNewMessage: false,
        clock: null,
        languageCode: null,
        language: null,
        languageSelectorContainer: null,
        iconLogOut: null,
        iconHome: null,
        iconQRCodeTop: null,
        numberNewMessage: null,
        iconNewMessage: null,
        swapView: null,
        changeLanguageLink: null,
        iconSearch: null,
        linkLogout: null,
        languageDropdownSubMenu: null,
        sideBar: null,
        sideBarContent: null,
        profileImage: null,
        headerNavElement: null
    },
    messageType: {
        Normal: 0,
        Personal: 1,
        Special: 2,
        System: 3
    },
    maximunNewMessage: {
        value: 99,
        text: "99+"
    },
    resources: {
        labelAM: '',
        labelPM: '',
        months: [],
        labelMonths: null
    },
    languageCode: null,
    clockTime: null,
    hideHeaderDropdownMenuTimeout: null,
    urls: {
        home: alpha.urls.mainRootPath + "Dashboard/Index",
        qrCodeTop: alpha.urls.memberInfoRootPath + "QRCodeMemberSignIn",
        messages: alpha.urls.messageRootPath + "MessageHistory/Index",
        announcements: alpha.urls.messageRootPath + "Announcement/Index",
        changeLanguage: alpha.urls.mainRootPath + "Language/Change",
        queryUserName: alpha.urls.oldMainRootPath + "_AccountInfo/Handlers/QueryUserName.ashx",
        checkNewMessage: alpha.urls.messageRootPath + "NewMessage/GetNumberOfNewAnnouncement",
        banner: alpha.urls.mainRootPath + "Banner/Get?type="
    },
    unreadMessage: {
        Normal: 0,
        Special: 0,
        System: 0,
        Personal: 0,
        Total: 0
    },
    init: function () {
        headerNav.initClock();
        headerNav.runClock();
        headerNav.cachedDomElements();
        headerNav.setBannerBackgroundImage();
        headerNav.initLanguage();
        headerNav.registerEvents();
    },
    setBannerBackgroundImage: function () {
        var type = headerNav.detectBannerTypeHandler();
        $.ajax({
            type: "GET",
            url: headerNav.urls.banner + type,
            success: function (result) {
                headerNav.getBannerSuccessullyHandler(result, type === 2);
            }
        });
    },
    detectBannerTypeHandler: function () {
        alpha.isScreenGreater1024 = false;
        alpha.isScreenLower768 = false;
        alpha.isScreenGreater768AndLower1024 = false;
        alpha.isScreenLower1024 = false;
        alpha.isMobile = false;
        alpha.detectResponsiveInformation();
        // Desktop: 1, Mobile: 2
        return alpha.isScreenLower768 ? 2 : 1;
    },
    getBannerSuccessullyHandler: function (result, isMobile) {
        if (result.ImageContent) {
            headerNav.domElements.headerNavElement
                .addClass("m-banner")
                .css("background-image", "url(" + result.ImageContent + ")");
            if (isMobile && result.MobileBackgroundColor) {
                headerNav.domElements.headerNavElement.css("background-color", result.MobileBackgroundColor);
            }
        }
    },
    initClock: function () {
        // to keep the clock difference least to the actual time,
        // we must run this first of all
        headerNav.domElements.clock = $("#clock");
        headerNav.resources.labelAM = "AM";
        headerNav.resources.labelPM = "PM";
        headerNav.resources.labelMonths = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var clock = headerNav.domElements.clock;
        headerNav.clockTime = new Date(clock.attr("data-year"), clock.attr("data-month") - 1, clock.attr("data-day"), clock.attr("data-hour"), clock.attr("data-minute"), clock.attr("data-second"), 0);
    },
    runClock: function () {
        headerNav.clockTime.setSeconds(headerNav.clockTime.getSeconds() + 1);
        var strTime = headerNav.formatClockTime(headerNav.clockTime);
        headerNav.domElements.clock.html(strTime);
        setTimeout(headerNav.runClock, 1000);
    },
    formatClockTime: function (clockTime) {
        var year = clockTime.getFullYear();
        var month = clockTime.getMonth();
        var day = clockTime.getDate();
        var hour = clockTime.getHours();
        var minute = clockTime.getMinutes();
        var second = clockTime.getSeconds();
        // Expected: hour from 0->11
        var strHours = hour >= 12 ? hour - 12 : hour;
        var strMinute = minute >= 10 ? minute : "0" + minute;
        var strSecond = second >= 10 ? second : "0" + second;
        var stra = hour >= 12 ? headerNav.resources.labelPM : headerNav.resources.labelAM;
        var str = strHours + ":" + strMinute + ":" + strSecond + " " + stra + " " +
            headerNav.resources.labelMonths[month] + " " + day + ", " + year + " GMT-4";
        return str;
    },
    cachedDomElements: function () {
        headerNav.domElements.isDisableToCheckNewMessage = $("#disableMessages").val() == "1";
        headerNav.domElements.languageCode = $("#language-code");
        headerNav.domElements.language = $("#language");
        headerNav.domElements.languageSelectorContainer = $(".language-selector-container");
        headerNav.domElements.iconLogOut = $(".icon-logout");
        headerNav.domElements.iconHome = $(".icon-home");
        headerNav.domElements.iconQRCodeTop = $(".icon-qr-code-top");
        headerNav.domElements.iconNewMessage = $(".icon-bell");
        headerNav.domElements.numberNewMessage = $("#numberOfNewMessage");
        headerNav.domElements.changeLanguageLink = $(".changelanguage-link");
        // For mobile
        headerNav.domElements.iconSearch = $(".icon-account-search");
        headerNav.domElements.linkLogout = $(".link-logout");
        headerNav.domElements.languageDropdownSubMenu = $('.dropdown-submenu.language');
        headerNav.domElements.sideBar = $("#side-bar");
        headerNav.domElements.sideBarContent = $(".content-wrapper");
        headerNav.domElements.profileImage = $("#logoProfileImg");
        headerNav.domElements.headerNavElement = $("#header-nav");
    },
    initLanguage: function () {
        headerNav.languageCode = headerNav.domElements.languageCode.val();
        var currentLanguage = headerNav.domElements.language.data("currentlanguage");
        var language = $(".list-language ." + currentLanguage + " > a").html();
        headerNav.domElements.languageSelectorContainer.html(language);
    },
    markMessageAsRead: function (type) {
        if (type == headerNav.messageType.Normal) {
            headerNav.unreadMessage.Total = headerNav.unreadMessage.Total - headerNav.unreadMessage.Normal;
            headerNav.unreadMessage.Normal = 0;
        }
        else if (type == headerNav.messageType.Personal) {
            headerNav.unreadMessage.Total = headerNav.unreadMessage.Total - headerNav.unreadMessage.Personal;
            headerNav.unreadMessage.Personal = 0;
        }
        else if (type == headerNav.messageType.System) {
            headerNav.unreadMessage.Total = headerNav.unreadMessage.Total - headerNav.unreadMessage.System;
            headerNav.unreadMessage.System = 0;
        }
        else {
            headerNav.unreadMessage.Total = headerNav.unreadMessage.Total - headerNav.unreadMessage.Special;
            headerNav.unreadMessage.Special = 0;
        }
    },
    buildIconNewMessageByType: function (type) {
        headerNav.markMessageAsRead(type);
        if (headerNav.unreadMessage.Total > 0) {
            headerNav.domElements.numberNewMessage.text(headerNav.unreadMessage.Total > headerNav.maximunNewMessage.value ? headerNav.maximunNewMessage.text : headerNav.unreadMessage.Total);
        }
        else {
            headerNav.domElements.numberNewMessage.text("");
            headerNav.domElements.numberNewMessage.removeClass("badge-title");
        }
    },
    buildIconNewMessage: function () {
        var firstUrl = headerNav.popFirstNewMessage();
        if (headerNav.unreadMessage.Total > 0) {
            headerNav.domElements.numberNewMessage.text(headerNav.unreadMessage.Total > headerNav.maximunNewMessage.value ? headerNav.maximunNewMessage.text : headerNav.unreadMessage.Total);
        }
        else {
            headerNav.domElements.numberNewMessage.text("");
            headerNav.domElements.numberNewMessage.removeClass("badge-title");
        }
        indexManager.changeMenu(firstUrl);
    },
    checkNewMessage: function () {
        if (!headerNav.domElements.isDisableToCheckNewMessage) {
            $.ajax({
                type: "POST",
                url: headerNav.urls.checkNewMessage,
                success: function (result) {
                    if (result !== undefined) {
                        headerNav.unreadMessage = result;
                        if (result.Total > 0) {
                            headerNav.domElements.numberNewMessage.text(result.Total > headerNav.maximunNewMessage.value ? headerNav.maximunNewMessage.text : result.Total);
                            headerNav.domElements.numberNewMessage.addClass("badge-title");
                        }
                        else {
                            headerNav.domElements.numberNewMessage.text("");
                            headerNav.domElements.numberNewMessage.removeClass("badge-title");
                        }
                    }
                    setTimeout(headerNav.checkNewMessage, 2 * 60 * 1000);
                }
            });
        }
    },
    popFirstNewMessage: function () {
        if (headerNav.unreadMessage.Special > 0) {
            headerNav.markMessageAsRead(headerNav.messageType.Special);
            return headerNav.urls.messages + "?messageType=" + headerNav.messageType.Special;
        }
        else if (headerNav.unreadMessage.System > 0) {
            headerNav.markMessageAsRead(headerNav.messageType.System);
            return headerNav.urls.announcements;
        }
        else if (headerNav.unreadMessage.Normal > 0) {
            headerNav.markMessageAsRead(headerNav.messageType.Normal);
            return headerNav.urls.messages + "?messageType=" + headerNav.messageType.Normal;
        }
        headerNav.markMessageAsRead(headerNav.messageType.Personal);
        return headerNav.urls.messages + "?messageType=" + headerNav.messageType.Personal;
    },
    registerEvents: function () {
        headerNav.domElements.iconLogOut.on("click", alpha.core.signOut);
        headerNav.domElements.iconHome.on("click", function () {
            indexManager.changeMenu(headerNav.urls.home);
        });
        headerNav.domElements.iconQRCodeTop.on("click", function () {
            indexManager.changeMenu(headerNav.urls.qrCodeTop);
        });
        headerNav.domElements.changeLanguageLink.on('click touchend', function (event) {
            headerNav.changeLanguage($(event.target).data("value"));
        });
        // Logout link for mobile
        headerNav.domElements.linkLogout.on("click", alpha.core.signOut);
        // Quick search component for mobile
        headerNav.domElements.iconSearch.on('click', function () {
            var topSearchbar = $('.top-search-bar'), topSearchInput = $('.top-search-input'), iconClose = $('.icon-close'), searchBarToggleClass = 'search-bar-toggle';
            topSearchbar.addClass(searchBarToggleClass);
            topSearchInput.val('');
            headerNav.domElements.sideBarContent.append("<div class='blockUI'></div>");
            var onCloseSearchBar = function () {
                topSearchbar.removeClass(searchBarToggleClass);
                headerNav.domElements.sideBarContent.find(".blockUI").remove();
            };
            var getSuggestions = function (query, process) {
                return $.get(headerNav.urls.queryUserName, { query: query }, function (data) {
                    var suggestions = alpha.core.convertReponseToJson(data).suggestions;
                    if (typeof suggestions !== 'undefined' && suggestions !== null && suggestions.length !== 0) {
                        var sugestionItems = suggestions.map(function (item) {
                            return item[0] + (item[3] ? " (" + item[3] + ")" : "");
                        });
                        return process(sugestionItems);
                    }
                    return process(["Not found"]);
                });
            };
            var afterSelectSuggestionItem = function (item) {
                onCloseSearchBar();
                menuBar.collapseLeftPanel();
                menuBar.activateAccountInfoTab(null, item.replace(/\(\S+?\)/g, "").trim());
            };
            iconClose.on('click', onCloseSearchBar);
            $(".blockUI").on('click', onCloseSearchBar);
            var typeaheadOptions = {
                minLength: 3,
                items: 10,
                name: 'userNames',
                autoSelect: false,
                delay: 200,
                source: getSuggestions,
                afterSelect: afterSelectSuggestionItem
            };
            topSearchInput.typeahead(typeaheadOptions);
        });
        // Language show options for mobile
        headerNav.domElements.languageDropdownSubMenu.on('click', function (e) {
            headerNav.domElements.languageDropdownSubMenu.find('.list-language').toggle();
            e.stopPropagation();
        });
        headerNav.domElements.iconNewMessage.on("click", headerNav.buildIconNewMessage);
        headerNav.domElements.numberNewMessage.on("click", headerNav.buildIconNewMessage);
        headerNav.checkNewMessage();
    },
    changeLanguage: function (lang) {
        $.ajax({
            type: "POST",
            url: headerNav.urls.changeLanguage,
            data: { languageCode: lang },
            success: function (result) {
                if (result !== undefined) {
                    // Remove www so that subdomain can override LANGUAGE value
                    var domain = window.location.hostname.replace(/([a-zA-Z0-9]+.)/, '').replace("www.", '');
                    $.cookie("LANGUAGE", lang, { expires: 30, path: domain });
                    headerNav.reloadPage();
                }
            }
        });
    },
    reloadPage: function () {
        top.location.href = headerNav.removeBookmarksInUrl(top.location.href);
    },
    setProfilePictureOnNav: function (profilePicure) {
        $('#userPicture').attr('src', profilePicure);
        $('#profile-avatar-image').attr('src', profilePicure);
    },
    removeBookmarksInUrl: function (url) {
        if (typeof url !== 'string') {
            return null;
        }
        var i = url.indexOf("#");
        if (i === -1) {
            return url;
        }
        return url.substr(0, i);
    }
};
$(function () {
    headerNav.init();
});
$(window).on("orientationchange", function () {
    headerNav.setBannerBackgroundImage();
});
//# sourceMappingURL=header-nav.js.map
var HeaderMessage = /** @class */ (function () {
    function HeaderMessage($) {
        var _this = this;
        this.publicMessageId = 0;
        this.privateMessageId = 0;
        this.scrollerLeft = 0;
        this.scrollerWidth = 0;
        this.maskWidth = 0;
        this.domElements = {
            privateMessage: null,
            publicMessage: null,
            moveBetsMessage: null,
            marqueeMessage: null,
            hotMessages: null,
            userMessageModal: null,
            currentHotMessage: null,
            scroller: null,
            contentMessage: null,
            showMoreContainer: null,
            showLessContainer: null
        };
        this.urls = {
            mainAppPrivateMessageForm: alpha.urls.messageRootPath + 'MessageHistory/Index?messageType=1',
            mainAppPublicMessageForm: alpha.urls.messageRootPath + 'MessageHistory/Index?messageType=0',
            mainAppSpecialMessageForm: alpha.urls.messageRootPath + 'MessageHistory/Index?messageType=2',
            getMessages: alpha.urls.messageRootPath + 'MarqueeMessage/Index'
        };
        this.init = function () {
            _this.cacheElements();
            _this.registerEvents();
            _this.getMessages();
        };
        this.viewLiveMessage = function (event) {
            var messageType = parseInt(_this.$(event.currentTarget).attr('messageType'));
            headerNav.buildIconNewMessageByType(messageType);
            if (messageType == alpha.messageType.Normal) {
                indexManager.changeMenu(_this.urls.mainAppPublicMessageForm);
            }
            else if (messageType == alpha.messageType.Personal) {
                indexManager.changeMenu(_this.urls.mainAppPrivateMessageForm);
            }
            else if (messageType == alpha.messageType.Special) {
                indexManager.changeMenu(_this.urls.mainAppSpecialMessageForm);
            }
        };
        this.viewMoveBetsMessage = function (event) {
            event.stopPropagation();
        };
        this.resetHotMessageElements = function (showFullMessage) {
            var minMessageHeight = 40;
            _this.domElements.currentHotMessage.css('min-height', minMessageHeight);
            if (showFullMessage) {
                _this.domElements.showMoreContainer.addClass("hide");
                _this.domElements.currentHotMessage.removeClass("brief-content");
                _this.domElements.currentHotMessage.css('height', '');
            }
            else {
                _this.domElements.showLessContainer.addClass("hide");
                _this.domElements.currentHotMessage.removeClass("brief-content");
            }
        };
        this.setHotMessageElementStyleByMessageHeight = function (showFullMessage, messageHeight, maxMessageHeight) {
            if (showFullMessage) {
                if (messageHeight <= maxMessageHeight) {
                    _this.domElements.showLessContainer.addClass("hide");
                }
                else {
                    _this.domElements.showLessContainer.removeClass("hide");
                }
            }
            else {
                if (messageHeight <= maxMessageHeight) {
                    _this.domElements.showMoreContainer.addClass("hide");
                }
                else {
                    _this.domElements.currentHotMessage.addClass("brief-content").height(maxMessageHeight);
                    _this.domElements.showMoreContainer.removeClass("hide");
                }
            }
        };
        this.calculateHotMessageHeight = function (showFullMessage, maxMessageHeight) {
            var bodyBorderHeight = 2;
            var bodyPaddingHeight = 5 + bodyBorderHeight;
            var messageHeight = _this.domElements.currentHotMessage.height();
            _this.setHotMessageElementStyleByMessageHeight(showFullMessage, messageHeight, maxMessageHeight);
            if (maxMessageHeight < messageHeight) {
                messageHeight = showFullMessage ?
                    messageHeight + _this.domElements.showLessContainer.height() :
                    maxMessageHeight + _this.domElements.showMoreContainer.height();
            }
            messageHeight += bodyPaddingHeight;
            return messageHeight;
        };
        this.calculateHotMessagePopupHeight = function (showFullMessage) {
            _this.resetHotMessageElements(showFullMessage);
            var maxMessageHeight = 53;
            var messageHeight = _this.calculateHotMessageHeight(showFullMessage, maxMessageHeight);
            var headerHeight = 10;
            var popUpHeight = headerHeight + messageHeight;
            _this.domElements.userMessageModal.height(popUpHeight);
        };
        this.showMoreHotMessage = function () {
            _this.calculateHotMessagePopupHeight(true);
        };
        this.showLessHotMessage = function () {
            _this.calculateHotMessagePopupHeight(false);
        };
        this.getMessages = function () {
            _this.$.ajax({
                type: "POST",
                url: _this.urls.getMessages,
                success: _this.onSuccessGetMessages
            });
        };
        this.onSuccessGetMessages = function (rawResults) {
            var results = rawResults, movebetsMessage = results.MoveBetsInformation.Content || "";
            _this.showPublicMessage(results);
            _this.domElements.moveBetsMessage.html(movebetsMessage);
            _this.showPrivateMessage(results);
            _this.applyScroller();
            setTimeout(_this.getMessages, 1 * 60 * 1000);
        };
        this.showPublicMessage = function (results) {
            var publicMessage = results.PublicMessage ? results.PublicMessage : 0;
            // Process Public Message: show new public message and its link to message form
            var url = _this.urls.mainAppPublicMessageForm;
            if (publicMessage.Id
                && publicMessage.Id !== _this.publicMessageId) {
                _this.domElements.publicMessage
                    .html(publicMessage.Content + '. ')
                    .attr('messageType', publicMessage.MessageType);
                _this.publicMessageId = publicMessage.Id;
                if (_this.publicMessageId !== 0) {
                    _this.appendMessageToPopup(publicMessage.Content);
                }
            }
            else {
                var spanMoveBets = _this.$("#span-movebets");
                if (spanMoveBets.length) {
                    spanMoveBets.remove();
                }
            }
        };
        this.showPrivateMessage = function (results) {
            var privateMessage = results.PrivateMessage ? results.PrivateMessage : 0;
            // Process Private Message: show new private message
            var url = _this.urls.mainAppPrivateMessageForm;
            if (privateMessage.Id
                && _this.privateMessageId !== privateMessage.Id) {
                //// Append Private Message to Marquee with green color format.
                _this.domElements.privateMessage.html("<font id='private-message-content' color=\"green\">Private message: " +
                    privateMessage.Content +
                    "</font>")
                    .attr('messageType', privateMessage.MessageType);
                if (_this.privateMessageId !== 0) {
                    _this.appendMessageToPopup(privateMessage.Content);
                }
            }
        };
        this.appendMessageToPopup = function (message) {
            _this.domElements.contentMessage.append('<p class="not-shown-message"></p>');
            _this.domElements.contentMessage.find("p:last").html(message);
        };
        this.applyScroller = function () {
            if (_this.scrollerLeft) {
                return;
            }
            _this.scrollerLeft = 100;
            _this.scrollerWidth = _this.domElements.scroller.width();
            _this.scroller = _this.domElements.scroller[0];
            _this.privateMsg = _this.domElements.privateMessage[0];
            _this.publicMsg = _this.domElements.publicMessage[0];
            _this.privateMsg.onmouseover = _this.publicMsg.onmouseover = _this.domElements.moveBetsMessage[0].onmouseover = function () {
                _this.pauseMarquee();
            };
            _this.privateMsg.onmouseout = _this.publicMsg.onmouseout = _this.domElements.moveBetsMessage[0].onmouseout = function () {
                _this.playMarquee();
            };
            var textNews = _this.domElements.marqueeMessage[0];
            _this.maskWidth = textNews.offsetWidth;
            _this.scrollMessage();
        };
        this.playMarquee = function () {
            if (_this.scrollerPaused) {
                _this.scrollerPaused = false;
                _this.scrollMessageTimeout = setTimeout(_this.scrollMessage, 20);
            }
            else {
                _this.scrollerPaused = false;
            }
        };
        this.pauseMarquee = function () {
            clearTimeout(_this.scrollMessageTimeout);
            _this.scrollMessageTimeout = 0;
            _this.scrollerPaused = true;
        };
        this.scrollMessage = function () {
            if (_this.scrollerPaused) {
                return;
            }
            if (_this.scrollerLeft + _this.scrollerWidth < -5) {
                _this.scrollerLeft = _this.maskWidth + 5;
                _this.scrollerWidth = _this.domElements.scroller.width();
            }
            _this.scroller.style.left = (_this.scrollerLeft--) + 'px';
            _this.scrollMessageTimeout = setTimeout(_this.scrollMessage, 20);
        };
        this.cacheElements = function () {
            _this.domElements.privateMessage = _this.$('#private-message');
            _this.domElements.publicMessage = _this.$('#public-message');
            _this.domElements.moveBetsMessage = _this.$('#movebets-message');
            _this.domElements.marqueeMessage = _this.$('#marqueeMessage');
            _this.domElements.hotMessages = _this.$('#userMessageModalBody');
            _this.domElements.contentMessage = _this.$('#msg-content');
            _this.domElements.userMessageModal = _this.$('#userMessageModal');
            _this.domElements.currentHotMessage = _this.domElements.hotMessages.find('p.current-hot-message');
            _this.domElements.scroller = _this.$('#scroller');
            _this.domElements.showMoreContainer = _this.$(".showmore-container");
            _this.domElements.showLessContainer = _this.$(".showless-container");
        };
        this.registerEvents = function () {
            _this.domElements.privateMessage.on('click', _this.viewLiveMessage);
            _this.domElements.publicMessage.on('click', _this.viewLiveMessage);
            _this.domElements.moveBetsMessage.on('click', _this.viewMoveBetsMessage);
            _this.domElements.showMoreContainer.on('click', _this.showMoreHotMessage);
            _this.domElements.showLessContainer.on('click', _this.showLessHotMessage);
        };
        this.$ = $;
    }
    return HeaderMessage;
}());
$(function () {
    if (alpha.isScreenLower768) {
        return;
    }
    var headerMessage = new HeaderMessage($);
    headerMessage.init();
});
//# sourceMappingURL=header-msg.js.map
var DataProccessingNotification = /** @class */ (function () {
    function DataProccessingNotification($) {
        var _this = this;
        this.progressBoxCollapseTimeout = 5000; //5s
        this.getMessageTimeout = 60000; //1min
        this.getMoveBetMessageUrl = alpha.urls.messageRootPath + "MarqueeMessage/MoveBet";
        this.getMessage = function () {
            _this.$.ajax({
                type: "GET",
                url: _this.getMoveBetMessageUrl,
                success: _this.onGetMessageSuccess
            });
        };
        this.onGetMessageSuccess = function (data) {
            if (data && data.Content) {
                _this.$dataNotificationModal.find(".modal-body").html(data.Content);
                if (_this.$progressBox.hasClass("hide")) {
                    _this.showProgressBox();
                }
            }
            else {
                _this.$progressBox.addClass("hide");
            }
            setTimeout(_this.getMessage, _this.getMessageTimeout);
        };
        this.onProgressBoxClick = function () {
            _this.$dataNotificationModal.modal("show");
        };
        this.$ = $;
    }
    DataProccessingNotification.prototype.init = function () {
        this.cacheDOMElements();
        this.registerEvents();
        this.getMessage();
    };
    DataProccessingNotification.prototype.cacheDOMElements = function () {
        this.$progressBox = this.$("#progress-box");
        this.$dataNotificationModal = this.$("#data-processing-popup");
    };
    DataProccessingNotification.prototype.registerEvents = function () {
        var _this = this;
        this.$progressBox.on("touchstart", function () {
            _this.startPosition = _this.$progressBox.position();
        });
        this.$progressBox.on("touchend", function (event) {
            event.preventDefault();
            var endPosition = _this.$progressBox.position();
            if (Math.abs(_this.startPosition.top - endPosition.top) < 3
                && Math.abs(_this.startPosition.left - endPosition.left) < 3) {
                _this.onProgressBoxClick();
            }
        });
    };
    DataProccessingNotification.prototype.showProgressBox = function () {
        var _this = this;
        this.$progressBox
            .removeAttr("style")
            .removeClass("collapsed")
            .removeClass("hide")
            .draggable({ disabled: true });
        setTimeout(function () {
            _this.$progressBox
                .addClass("collapsed")
                .draggable({ disabled: false, containment: "#content-wrapper" });
        }, this.progressBoxCollapseTimeout);
    };
    return DataProccessingNotification;
}());
$(document).ready(function () {
    if (alpha.isScreenLower768) {
        var dataProcessingNotification = new DataProccessingNotification($);
        dataProcessingNotification.init();
    }
});
//# sourceMappingURL=data-proccessing-notification.js.map
var UMMessage = /** @class */ (function () {
    function UMMessage($) {
        this.umMessageData = [];
        this.getUMMessagesTimeout = 1000 * 60; // 1 minute
        this.getUMMessagesUrl = alpha.urls.mainRootPath + "UMMessage/Get";
        this.doNotShowAgainKey = "UMMessageDoNotShowAgain_";
        this.userId = "";
        this.$ = $;
    }
    UMMessage.prototype.init = function () {
        this.cacheDOMElements();
        this.registerEvents();
        this.getUMMessages();
    };
    UMMessage.prototype.cacheDOMElements = function () {
        this.$umMessageBox = this.$("#um-message-box");
        this.$umMessagePopup = this.$("#um-message-popup");
        this.$umMessagePopupBody = this.$umMessagePopup.find("#um-message-popup-body");
        var subUserId = this.$("#subUserId").val();
        this.userId = subUserId === "0" ? this.$("#userId").val() : subUserId;
    };
    UMMessage.prototype.registerEvents = function () {
        var _this = this;
        var startPosition;
        this.$umMessageBox.on("mousedown touchstart", function () {
            startPosition = _this.$umMessageBox.position();
        });
        this.$umMessageBox.on("mouseup touchend", function (event) {
            event.preventDefault();
            var endPosition = _this.$umMessageBox.position();
            if (Math.abs(startPosition.top - endPosition.top) < 3
                && Math.abs(startPosition.left - endPosition.left) < 3) {
                _this.$umMessagePopup.modal("show");
            }
        });
        this.$umMessageBox.draggable({ disabled: false, containment: "body" });
        var chkDontShowAgain = this.$umMessagePopup.find("#chb-um");
        chkDontShowAgain.change(function () {
            _this.setDontShowAgainLocalStorage(chkDontShowAgain.is(":checked"));
        });
        this.$umMessagePopup.on("show.bs.modal", function () {
            chkDontShowAgain.prop("checked", _this.getDontShowAgainLocalStorage());
        });
    };
    UMMessage.prototype.getUMMessages = function () {
        var self = this;
        this.$.get(this.getUMMessagesUrl, function (data) {
            self.getUMMessagesSuccessfully(data);
        }).always(function () {
            setTimeout(function () { self.getUMMessages(); }, self.getUMMessagesTimeout);
        });
    };
    UMMessage.prototype.getUMMessagesSuccessfully = function (data) {
        if (data.length) {
            var umMessage = this.extractUMMessage(data);
            this.$umMessagePopupBody.html(umMessage.message);
            if (umMessage.hasNewMessage) {
                this.getDontShowAgainLocalStorage() || this.$umMessagePopup.modal("show");
                this.$umMessageBox.removeClass("hide");
            }
        }
        else {
            this.umMessageData.length = 0;
            this.$umMessagePopupBody.html("");
            this.$umMessagePopup.modal("hide");
            this.$umMessageBox.addClass("hide");
        }
    };
    UMMessage.prototype.extractUMMessage = function (data) {
        var idArray = [];
        var messageArray = [];
        var hasNewMessage = false;
        for (var i = 0, max = data.length; i < max; i++) {
            var datum = data[i];
            idArray.push(datum.Id);
            messageArray.push('<li class="um-message-item">- ');
            messageArray.push(datum.Message);
            messageArray.push("</li>");
            if (this.umMessageData.indexOf(datum.Id) === -1) {
                hasNewMessage = true;
            }
        }
        this.umMessageData = idArray;
        return {
            hasNewMessage: hasNewMessage,
            message: messageArray.join("")
        };
    };
    UMMessage.prototype.setDontShowAgainLocalStorage = function (value) {
        alpha.localStorage.set(this.doNotShowAgainKey + this.userId, value ? "1" : "0");
    };
    UMMessage.prototype.getDontShowAgainLocalStorage = function () {
        return alpha.localStorage.get(this.doNotShowAgainKey + this.userId) === "1";
    };
    return UMMessage;
}());
$(function () {
    new UMMessage($).init();
});
//# sourceMappingURL=um-message.js.map
/// <reference path="../core.ts" />
var menuBar = {
    domElements: {
        subMenuItems: null,
        menuOpeners: null,
        subMenus: null,
        menuItems: null,
        sideBar: null,
        accountInfoTab: null,
        accountInfoContent: null,
        redirectItems: null,
        extraIcon: null,
        header: null,
        userIcon: null,
        mainTab: null,
        mainContent: null,
        mainMenuTab: null,
        mainMenuHeader: null,
        accountInfo: null,
        accountInfoHeader: null,
        hamburgerIcon: null,
        profileImage: null,
        announcementItems: null,
        closeIconOnMobile: null
    },
    urls: {
        accountInfo: alpha.urls.accountInfoRootPath + 'SearchAccount'
    },
    init: function () {
        menuBar.storeElements();
        menuBar.bindEvents();
        menuBar.initSideBar();
    },
    storeElements: function () {
        menuBar.domElements.subMenuItems = $('.sub-menu-item');
        menuBar.domElements.menuOpeners = $('.menu-opener');
        menuBar.domElements.subMenus = $('.sub-menu');
        menuBar.domElements.menuItems = $('.menu-item');
        menuBar.domElements.sideBar = $('#side-bar');
        menuBar.domElements.accountInfoContent = $('.accinfo-content');
        menuBar.domElements.redirectItems = $('.redirect-item');
        menuBar.domElements.extraIcon = $('.extra-box');
        menuBar.domElements.header = $('#header');
        menuBar.domElements.userIcon = $('#user-icon');
        menuBar.domElements.mainTab = $('#main-tab');
        menuBar.domElements.mainContent = $('#content-wrapper');
        menuBar.domElements.mainMenuTab = $('#main-menu-tab');
        menuBar.domElements.accountInfoTab = $('#account-info-tab');
        menuBar.domElements.accountInfoHeader = $('#account-info-header');
        menuBar.domElements.mainMenuHeader = $('#main-menu-header');
        menuBar.domElements.hamburgerIcon = $('#hamburger-icon');
        menuBar.domElements.profileImage = $('#logoProfileImg');
        menuBar.domElements.announcementItems = $("#menu-ANNOUNCEMENTS li a");
        menuBar.domElements.closeIconOnMobile = $('#closeIconOnMobile');
    },
    initSideBar: function () {
        var sidebar = menuBar.domElements.sideBar, mainContent = menuBar.domElements.mainContent;
        var activeSubMenu = $('.menu-item.active .sub-menu');
        if (alpha.isScreenLower768) {
            //mainContent.addClass('expanded-mobile');
            sidebar.addClass('hidden-mobile');
            return;
        }
        if (alpha.isScreenGreater768AndLower1024) {
            menuBar.collapseLeftPanel();
            return;
        }
        if (alpha.isScreenGreater1024) {
            activeSubMenu.show();
            sidebar.removeClass('collapsed').removeClass('collapsed-smallsize');
            mainContent.removeClass('expanded');
        }
    },
    isSideBarCollapsed: function () {
        return menuBar.domElements.sideBar.hasClass('collapsed');
    },
    bindEvents: function () {
        menuBar.domElements.menuOpeners.click(function (e) {
            var $this = $(this), sideBar = menuBar.domElements.sideBar, isCollapsed = sideBar.hasClass('focusout')
                && sideBar.hasClass('collapsed-smallsize')
                && !sideBar.hasClass('docked'), isActive = $this.hasClass('active');
            menuBar.expandSideBar();
            if (isCollapsed && isActive) {
                menuBar.activateMainMenuTab();
            }
            else {
                menuBar.toggleMainMenu($this);
            }
        });
        menuBar.domElements.subMenuItems.click(function () {
            menuBar.activateSubMenuItem($(this));
        });
        menuBar.domElements.mainMenuHeader.click(menuBar.activateMainMenuTab);
        menuBar.domElements.accountInfoHeader.click(menuBar.activateAccountInfoTab);
        menuBar.domElements.userIcon.click(menuBar.activateAccountInfoTab);
        menuBar.domElements.redirectItems.click(menuBar.collapseSideBar);
        //menuBar.domElements.header.click(menuBar.collapseSideBar);
        menuBar.domElements.extraIcon.click(menuBar.expandSideBar);
        menuBar.domElements.hamburgerIcon.click(function (e) {
            menuBar.collapseLeftPanel();
            if (alpha.isScreenLower768) {
                e.stopPropagation();
            }
        });
        menuBar.registerMouseHoverForDesktopBrowsers();
        menuBar.domElements.announcementItems.click(function () {
            menuBar.clickAnnouncementItem($(this));
        });
        menuBar.domElements.closeIconOnMobile.click(menuBar.collapseSideBar);
    },
    clickAnnouncementItem: function (menu) {
        var url = menu.attr("href");
        var parameters = url.split('?');
        if (parameters.length == 2) {
            var messageType = parameters[1].split('=')[1];
            headerNav.buildIconNewMessageByType(messageType);
        }
        else {
            headerNav.buildIconNewMessageByType(headerNav.messageType.System);
        }
    },
    registerMouseHoverForDesktopBrowsers: function () {
        if (!alpha.isMobile) {
            menuBar.domElements.sideBar
                .off('mouseenter.menubar')
                .on('mouseenter.menubar', function (e) {
                setTimeout(menuBar.expandSideBar, 50);
            });
            menuBar.domElements.sideBar
                .off('mouseleave.menubar')
                .on('mouseleave.menubar', menuBar.collapseSideBar);
        }
    },
    collapseLeftPanel: function () {
        var sidebar = menuBar.domElements.sideBar, content = menuBar.domElements.mainContent, header = menuBar.domElements.header, mainContent = menuBar.domElements.mainContent, closeIconOnMobile = menuBar.domElements.closeIconOnMobile, hamburgerIcon = menuBar.domElements.hamburgerIcon;
        if (alpha.isScreenLower768) {
            if (sidebar.hasClass('hidden-mobile')) {
                content.addClass('expanded-mobile');
                sidebar.removeClass('hidden-mobile').addClass('visible-mobile');
                header.addClass('expanded-header-mobile');
                hamburgerIcon.hide();
                closeIconOnMobile.addClass('active');
            }
            else {
                content.removeClass('expanded-mobile');
                sidebar.addClass('hidden-mobile collapse-mobile').removeClass('visible-mobile');
                header.removeClass('expanded-header-mobile');
            }
        }
        else {
            var collapsedClass = 'collapsed', expandedClass = 'expanded', collapseImageClass = 'collapse-image', dockedClass = 'docked', isCollapsed = sidebar.hasClass(collapsedClass), isHidenMobile = sidebar.hasClass('hidden-mobile'), activeSubMenu = $('.menu-item.active .sub-menu'), profileImage = menuBar.domElements.profileImage;
            if (isCollapsed) {
                activeSubMenu.show();
                sidebar.addClass(dockedClass).removeClass(collapsedClass);
                content.removeClass(expandedClass);
                profileImage.removeClass(collapseImageClass);
                //// When expanding, check current active tab, and active it
                if (menuBar.domElements.accountInfoHeader.hasClass('active')) {
                    menuBar.domElements.mainMenuTab.hide();
                    menuBar.domElements.accountInfoTab.show();
                }
                else {
                    menuBar.domElements.mainMenuTab.show();
                    menuBar.domElements.accountInfoTab.hide();
                }
            }
            else {
                activeSubMenu.hide();
                sidebar.removeClass(dockedClass).addClass(collapsedClass);
                content.addClass(expandedClass);
                profileImage.addClass(collapseImageClass);
                menuBar.domElements.mainMenuTab.show();
                menuBar.domElements.accountInfoTab.hide();
            }
        }
        menuBar.positionPTTooltip();
    },
    activateAccountInfoTab: function (event, userName) {
        if (userName === void 0) { userName = ''; }
        menuBar.domElements.accountInfoHeader.addClass('active');
        menuBar.domElements.mainMenuHeader.removeClass('active');
        menuBar.domElements.mainMenuTab.hide();
        menuBar.domElements.accountInfoTab.show();
        var accInfoContent = menuBar.domElements.accountInfoContent, accountInfoUrl = accInfoContent.attr('src');
        if (!accountInfoUrl) {
            accInfoContent.attr('src', menuBar.urls.accountInfo + '?mobileView=' + alpha.isScreenLower768);
        }
        if (userName !== '' || typeof userName === 'undefined') {
            accInfoContent.attr('src', menuBar.urls.accountInfo + '?mobileView=' + alpha.isScreenLower768 + '&SearchAccountName=' + userName);
        }
        // To work on IE, use plain-old javascript onload event
        accInfoContent[0].onload = function () {
            var navigationHeight = $('#navigation').height();
            var mainMenuHeight = $('.main-tab').height();
            var accountInfoIframeHeight = navigationHeight - mainMenuHeight;
            menuBar.domElements.accountInfoTab.css("height", accountInfoIframeHeight);
            if (alpha.isMobile) {
                $('#accountinfo-content').css("height", accountInfoIframeHeight - 20);
                menuBar.domElements.accountInfoContent.addClass('small-device');
            }
            else {
                $('#accountinfo-content').css("height", accountInfoIframeHeight - 5);
            }
        };
    },
    activateSubMenuItem: function (subMenu) {
        menuBar.domElements.subMenuItems.removeClass('active');
        subMenu.addClass('active');
        if (!alpha.core.isIe()) {
            $('#main').contents().find("head").append($("#AssetCommonCssLink").val());
            $('#main').contents().find('body').empty().block();
        }
        $('#main').attr('src', subMenu.data("url")).attr("target", "main");
    },
    expandSideBar: function () {
        var sideBar = menuBar.domElements.sideBar;
        if (sideBar.hasClass('collapsed')) {
            sideBar.addClass('hovered').removeClass('focusout');
            //// Show all active sub menus
            var activeSubMenus = $('.menu-item.active .sub-menu');
            activeSubMenus.css('display', 'block');
            //// When expanding, if active tab is account info, then hide menu bar
            if (menuBar.domElements.accountInfoHeader.hasClass('active')) {
                menuBar.domElements.mainMenuTab.hide();
                menuBar.domElements.accountInfoTab.show();
            }
        }
    },
    collapseSideBar: function () {
        var sideBar = menuBar.domElements.sideBar, closeIconOnMobile = menuBar.domElements.closeIconOnMobile, hamburgerIcon = menuBar.domElements.hamburgerIcon;
        if (alpha.isScreenLower768 && sideBar.hasClass('visible-mobile')) {
            menuBar.domElements.mainContent.removeClass('expanded-mobile');
            menuBar.domElements.header.removeClass('expanded-header-mobile');
            sideBar.addClass('hidden-mobile').removeClass('visible-mobile');
            closeIconOnMobile.removeClass('active');
            hamburgerIcon.show();
        }
        else {
            if (sideBar.hasClass('docked')) {
                return;
            }
            if (sideBar.hasClass('collapsed')) {
                sideBar.removeClass('hovered').addClass('focusout');
                //// Hide all expanded sub menus
                var activeSubMenus = $('.menu-item.active .sub-menu');
                activeSubMenus.css('display', 'none');
                //// Show all menu bar icon
                menuBar.domElements.mainMenuTab.show();
                menuBar.domElements.accountInfoTab.hide();
            }
        }
    },
    activateMainMenuTab: function () {
        //// Set header tab state, and hide account infor tab
        menuBar.domElements.accountInfoHeader.removeClass('active');
        menuBar.domElements.accountInfoTab.hide();
        menuBar.domElements.mainMenuHeader.addClass('active');
        menuBar.domElements.mainMenuTab.show();
    },
    toggleMainMenu: function (menuOpener) {
        menuBar.activateMainMenuTab();
        var mainMenu = menuOpener.parent(), isVisible = menuOpener.hasClass('active');
        //// Hide all active menus
        menuBar.domElements.menuItems.removeClass('active');
        menuBar.domElements.menuOpeners.removeClass('active');
        menuBar.domElements.subMenus.slideUp('fast');
        var subMenuContainer = menuOpener.next();
        if (isVisible) {
            //// Unset active for selected menu, when collapse
            mainMenu.removeClass('active');
            menuOpener.removeClass('active');
            //// Incase of has sub menu, then collapse sub menus
            if (subMenuContainer.is('ul')) {
                subMenuContainer.slideUp('fast');
            }
        }
        else {
            //// Expand selected menu
            mainMenu.addClass('active');
            menuOpener.addClass('active');
            //// expand all sub menu
            if (subMenuContainer.is('ul')) {
                subMenuContainer.slideDown('fast');
            }
        }
    },
    positionPTTooltip: function () {
        // tabPT tool tip
        setTimeout(function () {
            var tabPT = $('#toolTipSettingMinPT');
            if (tabPT && tabPT.length) {
                var tabPTLeft = $('.sidebar').width() + 20;
                tabPT.css('left', tabPTLeft);
            }
        }, 400);
    }
};
$(function () {
    menuBar.init();
});
//# sourceMappingURL=menu-bar.js.map
/// <reference path="../core.ts" />
'use strict';
//declare var $: any;
var briefProfile = {
    maxCountShowTooltip: 4,
    isShowEditProfilePictureTooltip: false,
    isShowNickNameProfileTooltip: false,
    autoCloseToolTipTime: 20000,
    domElements: {
        toolTipBox: null,
        profileTooltip: null,
        changePictureLink: null,
        profileContent: null,
        profilePicture: null,
        adminAndSubAcc: null
    },
    countNickNameToolTipCookieName: null,
    init: function () {
        briefProfile.cachedDomElements();
        briefProfile.checkAndSetShowEditProfilePictureTooltip();
        briefProfile.checkAndShowNickNameTooltip();
        briefProfile.setProfilePictureStorage();
        briefProfile.setActorNameStorage();
        briefProfile.registerEvents();
    },
    checkAndSetShowEditProfilePictureTooltip: function () {
        briefProfile.isShowEditProfilePictureTooltip = briefProfile.domElements.profileContent.data('is-show-profile-picture-tooltip');
    },
    checkAndShowNickNameTooltip: function () {
        var countNickNameToolTip = briefProfile.getCountNickNameToolTip();
        if (briefProfile.domElements.profileContent.data('nick-name').length === 0
            && countNickNameToolTip < briefProfile.maxCountShowTooltip) {
            briefProfile.isShowNickNameProfileTooltip = true;
        }
        briefProfile.setCountNickNameToolTip(++countNickNameToolTip);
        briefProfile.showTooltipIntroNickname();
    },
    getCountNickNameToolTip: function () {
        briefProfile.countNickNameToolTipCookieName = briefProfile.domElements.profileContent.data('introduce-nick-name-tooltip-cookie-name');
        var countNickNameTooltip = $.cookie(briefProfile.countNickNameToolTipCookieName);
        return parseInt(countNickNameTooltip || 0);
    },
    setCountNickNameToolTip: function (value) {
        $.cookie(briefProfile.countNickNameToolTipCookieName, value, { expires: 30, path: '/' });
    },
    setProfilePictureStorage: function () {
        localStorage.setItem('actorProfilePicture', JSON.stringify({
            ImageId: briefProfile.domElements.profilePicture.attr('profile-image-id'),
            Image: briefProfile.domElements.profilePicture.attr('src'),
            IsDefaultAvatar: briefProfile.domElements.profilePicture.attr('default-avatar')
        }));
    },
    setActorNameStorage: function () {
        var actorNameStorage = localStorage.getItem('actorName');
        var actorName = $('#Username').val();
        var IsOldActor = actorName === actorNameStorage;
        localStorage.setItem('IsOldActor', JSON.stringify(IsOldActor));
        if (!IsOldActor) {
            localStorage.setItem('actorName', actorName);
        }
    },
    registerEvents: function () {
        briefProfile.domElements.profileTooltip
            .off('click')
            .on('click', briefProfile.hideProfileTooltip);
        briefProfile.domElements.toolTipBox
            .off('click')
            .on('click', briefProfile.hideTooltipIntroNickname);
        briefProfile.domElements.profilePicture.click(function () {
            menuBar.collapseSideBar();
        });
        if (briefProfile.domElements.adminAndSubAcc.length > 0) {
            briefProfile.domElements.adminAndSubAcc.tooltip();
        }
        $(".nickname-box").tooltip();
    },
    cachedDomElements: function () {
        briefProfile.domElements.toolTipBox = $('#tooltip-box');
        briefProfile.domElements.profileTooltip = $('#profileTooltip');
        briefProfile.domElements.changePictureLink = $('#changePictureLink');
        briefProfile.domElements.profileContent = $('#profile-content');
        briefProfile.domElements.profilePicture = $("#logoProfileImg");
        briefProfile.domElements.adminAndSubAcc = $("#adminAndSubAcc");
    },
    hideTooltipIntroNickname: function () {
        if (briefProfile.domElements.toolTipBox.length > 0) {
            briefProfile.domElements.toolTipBox.remove();
        }
        briefProfile.showTooltipProfilePicture();
    },
    hideProfileTooltip: function () {
        if (briefProfile.domElements.profileTooltip.length > 0) {
            briefProfile.domElements.profileTooltip.remove();
        }
    },
    hideAllTooltips: function () {
        if (briefProfile.domElements.toolTipBox.length > 0) {
            briefProfile.domElements.toolTipBox.remove();
        }
        briefProfile.hideProfileTooltip();
    },
    showTooltipProfilePicture: function () {
        if (briefProfile.domElements.profileTooltip.length > 0
            && briefProfile.isShowEditProfilePictureTooltip) {
            briefProfile.domElements.profileTooltip.show();
            setTimeout(function () {
                briefProfile.hideProfileTooltip();
            }, 20000);
        }
    },
    showTooltipIntroNickname: function () {
        if (briefProfile.domElements.toolTipBox.length > 0
            && briefProfile.isShowNickNameProfileTooltip) {
            briefProfile.domElements.toolTipBox.show();
            setTimeout(function () {
                briefProfile.hideTooltipIntroNickname();
            }, 20000);
        }
        else {
            briefProfile.showTooltipProfilePicture();
        }
    }
};
//# sourceMappingURL=brief-profile.js.map
var PopupAnnouncement = /** @class */ (function () {
    function PopupAnnouncement($) {
        var _this = this;
        this.url = alpha.urls.messageRootPath + 'Announcement/Popup';
        this.init = function () {
            var iframe = _this.$('.announcement-modal iframe');
            iframe.attr("src", _this.url);
        };
        this.$ = $;
    }
    return PopupAnnouncement;
}());
$(document).ready(function () {
    if ($("#disableMessages").val() == "1") {
        return;
    }
    var announcement = new PopupAnnouncement($);
    announcement.init();
});
//# sourceMappingURL=popup-announcement.js.map