/// <reference path="menu-bar.ts" />
declare var popupManager: any;
declare interface String {
    startsWith(text: string, position: string): boolean;
}

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
    hideFeedbackCookies: 'HideFeedback',
    hideFeedbackPopupCookies: 'HideFeedbackPopup',
    requiredNetworkSpeedCalculation: false,
    showOldCustomerList: false,
    reportDomain: "",
    recheckInIntervalMiliseconds: 30000,

    domElements: {
        mainFrame: null,
        partialContents: null,
        content: null,
        accountInfoContent: null,
        currentURLCookieName: null,
        pagePopup: null
    },

    urls: {
        reCheckinUrl: alpha.urls.oldMainRootPath + '_Authorization/Handlers/ReCheckin.ashx',
        feedback: alpha.urls.messageRootPath + 'Feedback/Index',
        checkExistFeedback: alpha.urls.messageRootPath + 'Feedback/CheckExist',
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
        indexManager.initFeedback();

        if (indexManager.requiredNetworkSpeedCalculation) {
            setTimeout(indexManager.calculateUserConnectionSpeed, 8000);
        }
    },

    registerEvents: function () {
        indexManager.domElements.pagePopup.tinyDraggable();
        setTimeout(indexManager.reCheckin, indexManager.recheckInIntervalMiliseconds);

        const eventMethod = window.addEventListener ? "addEventListener" : "attachEvent";
        const eventer = window[eventMethod];
        const messageEvent = eventMethod === "attachEvent" ? "onmessage" : "message";
        eventer(messageEvent, function (e) {
            if (e.data.event_id === "daily-statement"
                || e.data.event_id === "detail-statement") {
                popupManager.open(e.data.url, e.data.title, 900);
            }

            if (e.data.event_id === "betlist-result") {
                popupManager.openMultiple(e.data.url, e.data.title, e.data.width, e.data.height, undefined, undefined, undefined, undefined, undefined, undefined, e.data.options);
            }
        }, false);
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
        indexManager.reportDomain = $('#reportDomain').val();
        indexManager.recheckInIntervalMiliseconds = $("#RecheckinIntervalMiliseconds").val();
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
            success: (result) => {
                try {
                    result = alpha.core.convertReponseToJson(result);
                    var errCode = result.errCode;
                    var errMsg = result.errMsg;
                    if (errCode !== 0) {
                        alpha.core.signOut();
                        if (errMsg) {
                            alert(errMsg);
                        }
                    }
                }
                catch (e) {
                }
            },
            complete: () => {
                setTimeout(indexManager.reCheckin, indexManager.recheckInIntervalMiliseconds);
            }
        });
    },

    loadPartialViews: function () {
        indexManager.domElements.partialContents.each(function (index, partialContent) {
            var $partialContent = $(partialContent),
                url = $partialContent.data('url');
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
        try {
            $('.modal[id^="page-popup-"]').remove();

            if ($("#isInternal").val() === "0") {
                var mainFrame = <HTMLIFrameElement>document.getElementById("main");
                var mainFrameDocument = mainFrame.contentWindow || mainFrame.contentDocument;
                var currentUrl = mainFrameDocument && mainFrameDocument.location.href || mainFrame.src;
                localStorage.setItem(indexManager.currentUrlCookieName, currentUrl);
            }
        } catch (e) { }
    },

    setCurrentUrl: function () {
        const defaultUrl = indexManager.domElements.content.data('default-url');
        let currentUrl = $("#isInternal").val() === "1"
            ? defaultUrl
            : localStorage.getItem(indexManager.currentUrlCookieName) || defaultUrl;
        currentUrl = indexManager.isIgnoreRedirectUrl(currentUrl)
            ? defaultUrl
            : indexManager.replaceIsDarkModeQueryStringValue(currentUrl);

        if (currentUrl.indexOf(headerNav.urls.home) !== -1) {
            currentUrl = currentUrl.replace("Index2", "Index");
        }

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

                    indexManager.cacheCurrentUrl();
                    try {
                        $(iframeWindow.document).on('click touchend',
                            () => {
                                menuBar.collapseSideBar();
                            });

                        if (alpha.isMobile) {
                            $(iframeWindow.document).find(':focus').blur();
                        }
                    } catch (e) { }
                });

            indexManager.domElements.accountInfoContent.load(function () {
                var iframeWindow = indexManager.domElements.accountInfoContent[0].contentWindow ||
                    indexManager.domElements.accountInfoContent[0].contentDocument;
                indexManager.closeAllOpenDropdowns(iframeWindow);
            });
        }
    },

    closeAllOpenDropdowns: function (iframeWindow) {
        const closeDropdownSubMenu = () => {
            $('.dropdown-submenu').find('.dropdown-menu').hide();
        }

        $(document).on('click touchend', closeDropdownSubMenu);

        $('#btn-header-menu').on('click', closeDropdownSubMenu);

        try {
            $(iframeWindow.document).on('click touchend', () => {
                $('.dropdown-toggle').each((index, dropdown) => {
                    var $dropdown = $(dropdown);

                    if ($dropdown.parent().hasClass('open')) {
                        $dropdown.dropdown('toggle');
                    }
                });

                closeDropdownSubMenu();
            });
        } catch (e) { }
    },

    closeAllPopup: function () {
        try {
            var prevUrl = localStorage.getItem(indexManager.currentUrlCookieName) || "";
            var currentUrl = (<HTMLIFrameElement>document.getElementById("main")).contentWindow.location.href || "";

            var prevUrlWithoutQueryString = prevUrl.split("?")[0];
            var currentUrlWithoutQueryString = currentUrl.split("?")[0];

            var isCloseAllPopup = !this.isSameUrl(prevUrlWithoutQueryString, currentUrlWithoutQueryString);

            if (isCloseAllPopup) {
                popupManager.closeAllPopup();
            }
        } catch (e) { }
    },

    isSameUrl: function (url1, url2) {
        url1 = this.removeHashFromTheEndOfURL(url1.toLowerCase());
        url2 = this.removeHashFromTheEndOfURL(url2.toLowerCase());
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

    removeHashFromTheEndOfURL: function (url: string) {
        const indexOfHash = url.length - 1;

        return url[indexOfHash] === "#"
            ? url.substring(0, indexOfHash)
            : url;
    },

    calculateUserConnectionSpeed: function () {
        var iframe = <HTMLIFrameElement>document.getElementById('connection-speed');

        if (typeof (iframe) == 'undefined'
            || iframe == null) {
            iframe = <HTMLIFrameElement>document.createElement('iframe');
            iframe.id = 'connection-speed';
            iframe.style.display = 'none';
            iframe.src = alpha.urls.mainRootPath + "NetworkSpeed/Index";
            document.body.appendChild(iframe);
        }
    },

    isIgnoreRedirectUrl: function (currentUrl: string): boolean {
        const ignoreUrls = ['error.aspx', 'signin.aspx', 'securitycode', 'dashboard', 'partialum', 'auth', 'kickout', 'signout'];
        const ignoreUrlsLength = ignoreUrls.length;
        const lowerCurrentUrl = currentUrl.toLowerCase();

        for (let i = 0; i < ignoreUrlsLength; i++) {
            if (lowerCurrentUrl.indexOf(ignoreUrls[i]) > -1) {
                return true;
            }
        }

        return false;
    },

    replaceIsDarkModeQueryStringValue: function (url: string) {
        return indexManager.isDCSUrl(url)
            ? url.replace(/(isdarkmode=)[^\&]+/i, '$1' + $('#isDarkMode').val())
            : url;
    },

    isDCSUrl: function (url: string) {
        return url.indexOf('dcs/viewcustomersetting') > -1;
    },

    initFeedback: function () {
        $(".txt-feedback, .feedback-box .icon-feedback, #btn-feedback").click(function () { indexManager.openFeedback(); });

        var hideFeedbackPopup = localStorage.getItem(indexManager.getCustomerCookiesName(indexManager.hideFeedbackPopupCookies));
        if (hideFeedbackPopup !== '1') {
            $(".advertisement-modal").remove();
            $(".um-message-popup").remove();

            popupManager.closeAllPopup();

            $(".feedback-box").addClass('show-popup');
            $("#btn-hide-feedback-popup").click(function () {
                localStorage.setItem(indexManager.getCustomerCookiesName(indexManager.hideFeedbackPopupCookies), "1");
                $(".feedback-box").removeClass('show-popup');
            });

            $(window).click(function () {
                $(".feedback-box").removeClass('show-popup');
            });
        }

        $("#main").load(function () {
            if ($("#main").height() >= $('#main').contents().height()) {
                $(".div-feedback").css({ right: -20 });
            }
        });

        var hideFeedback = localStorage.getItem(indexManager.getCustomerCookiesName(indexManager.hideFeedbackCookies));

        if (hideFeedback === '1') {
            $(".div-feedback").remove();
            return;
        }

        $.ajax({
            type: "POST",
            url: indexManager.urls.checkExistFeedback,
            data: {},
            success: (result) => {
                if (result) {
                    return;
                }

                $(".div-feedback").css({
                    top: $(window).height() - 100
                });

                $(".div-feedback").removeClass('hide');
                $(".icon-close-feedback").click(function () {
                    localStorage.setItem(indexManager.getCustomerCookiesName(indexManager.hideFeedbackCookies), "1");
                    $(".div-feedback").addClass('hide');
                });
            },
            complete: () => { }
        });
    },
    openFeedback: function () {
        popupManager.open(indexManager.urls.feedback, $("#FeedbackTitle").val(), 680, 550, null, null, null, null, null, null, { customClass: 'feedback-modal' });
    },

    hideFloatingFeedbackIcon: function () {
        $(".div-feedback").remove();
    },

    removeCookies: function () {
        localStorage.removeItem(indexManager.currentUrlCookieName);
        localStorage.removeItem(indexManager.getCustomerCookiesName(indexManager.hideFeedbackCookies));
        localStorage.removeItem(indexManager.getCustomerCookiesName(indexManager.hideFeedbackPopupCookies));
    },
    getCustomerCookiesName: function (key) {
        return `${key}_${$("#CustId").val()}_${$("#SubAccId").val()}`;
    }
};

$(function () {
    indexManager.init();
});
