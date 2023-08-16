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
    hidePopupCookies: 'HidePopup',

    requiredNetworkSpeedCalculation: false,
    showOldCustomerList: false,
    reportDomain: "",
    recheckInIntervalMiliseconds: 30000,
    isInternal: true,

    domElements: {
        mainFrame: null,
        partialContents: null,
        content: null,
        accountInfoContent: null,
        currentURLCookieName: null,
        pagePopup: null,
        feedbackFunction: null
    },

    urls: {
        reCheckinUrl: alpha.urls.oldMainRootPath + '_Authorization/Handlers/ReCheckin.ashx',
        feedback: alpha.urls.messageRootPath + 'Feedback/Index',
        checkExistFeedback: alpha.urls.messageRootPath + 'Feedback/CheckExist?id=',
        feedbackFunction: alpha.urls.messageRootPath + 'Feedback/Function?id=',
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

        indexManager.initFeedbackGlobal();
        indexManager.initNewFeatureHighlightPopup();

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

            if (e.data.event_id === "pplivecasino-result") {
                var left = popupManager.leftPosition();
                var top = popupManager.topPosition() + 100;
                var option = "toolbar=no,scrollbars=no,resizable=yes,top=" + top + ",left=" + left + ",width=835,height=400,location=no";
                var resultPopup = window.open("", "_blank", option);
                resultPopup.document.title = "PP Live Casino Result";
                $.ajax({
                    type: "POST",
                    url: location.origin.split(':')[0] + "://" + location.hostname + '/' + alpha.urls.resultRootPath + e.data.url,
                    success: function (data) {
                        resultPopup.location.href = data.Result;
                    },
                    error: function () {
                        resultPopup.location.href = alpha.urls.oldMainRootPath + 'Error.aspx';
                    }
                });
            }

            if (e.data.event_id === "winlossdetailnew-feedback") {
                indexManager.initFeedbackFunction(e.data.feedback_id, undefined, undefined);
            }
        }, false);
    },

    cachedDomElements: function () {
        indexManager.isInternal = $('#isInternal').val() === "1";
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
        indexManager.domElements.feedbackFunction = $("#feedback-function");
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

            if (!indexManager.isInternal) {
                var mainFrame = <HTMLIFrameElement>document.getElementById("main");
                var mainFrameDocument = mainFrame.contentWindow || mainFrame.contentDocument;
                var currentUrl = mainFrameDocument && mainFrameDocument.location.href || mainFrame.src;
                localStorage.setItem(indexManager.currentUrlCookieName, currentUrl);
            }
        } catch (e) { }
    },

    setCurrentUrl: function () {
        const defaultUrl = indexManager.domElements.content.data('default-url');
        let currentUrl = indexManager.isInternal
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
                    indexManager.domElements.feedbackFunction.addClass("hide");
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

    updateCurrentUrl: function (queryString) {
        if (!indexManager.isInternal) {
            const currentUrl = localStorage.getItem(indexManager.currentUrlCookieName);
            if (currentUrl) {
                const currentUrlArray = currentUrl.split("?");
                localStorage.setItem(indexManager.currentUrlCookieName, currentUrlArray[0] + "?" + queryString);
            }
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

    initFeedbackGlobal: function () {
        $("#feedback-box").click(function () {
            popupManager.open(indexManager.urls.feedback, undefined, 440, 380, null, null, null, null, null, null, { customClass: 'feedback-modal' });
        });
    },

    initFeedbackFunction: function (id, width, height) {
        const hideFeedbackFunctionSessionKey = indexManager.getCustomerCookiesName('HideFeedbackFunction') + "_" + id;
        if (sessionStorage.getItem(hideFeedbackFunctionSessionKey) === "1") {
            return;
        }

        $.get(indexManager.urls.checkExistFeedback + id, function (result) {
            if (!result.IsExist) {
                //indexManager.calulateFeedbackFunctionPosition();

                const feedbackBox = indexManager.domElements.feedbackFunction.find(".feedback-box");
                feedbackBox.find(".txt-feedback").text(result.FunctionName || "Feedback");
                feedbackBox
                    .off()
                    .click(function () {
                        popupManager.open(indexManager.urls.feedbackFunction + id, undefined, width || 440, height || 380, null, null, null, null, null, null, { customClass: "feedback-modal" });
                    });
                indexManager.domElements.feedbackFunction.find(".icon-close-feedback")
                    .off()
                    .click(function () {
                        sessionStorage.setItem(hideFeedbackFunctionSessionKey, "1");
                        indexManager.domElements.feedbackFunction.addClass("hide");
                    });
                indexManager.domElements.feedbackFunction.removeClass("hide");
                //$(window).off("resize").resize(indexManager.calulateFeedbackFunctionPosition);
            }
        });
    },

    //calulateFeedbackFunctionPosition() {
    //    indexManager.domElements.mainFrame.height() >= indexManager.domElements.mainFrame.contents().height()
    //        && indexManager.domElements.feedbackFunction.css({ right: -20 });

    //    indexManager.domElements.feedbackFunction.css({
    //        top: $(window).height() - 100
    //    });
    //},

    initNewFeatureHighlightPopup: function () {
        const lmenu = $("#menu-CHATBOT");
        if (lmenu.length === 0) {
            return;
        }

        const featureName = "AbnormalLogin";

        const cookiesName = indexManager.hidePopupCookies + featureName;
        const hidePopup = localStorage.getItem(indexManager.getCustomerCookiesName(cookiesName));

        if (hidePopup !== "1") {
            popupManager.closeAllPopup();
            const box = $("#boxHighlightPopupNewFeature");

            box.css({ top: lmenu.position().top + 130 });
            box.removeClass("hide");

            box.find(".icon-close").click(() => box.addClass("hide"));

            box.find("#chkHideHighlightPopupNewFeature").click(function () {
                localStorage.setItem(indexManager.getCustomerCookiesName(cookiesName), "1");
                box.addClass("hide");
            });

            const newFeatureLink = box.find("#linkHighlightPopupCheckOut");
            newFeatureLink.click(() => box.addClass("hide"));

            $(window).scroll(() => box.addClass("hide"));

            $("#main-menu-tab")["mCustomScrollbar"]({
                callbacks: {
                    onScrollStart: () => box.addClass("hide")
                }
            });

            // indexManager.replaceCookieslessURL(newFeatureLink);
        }
    },

    hideFloatingFeedbackIcon: function () {
        indexManager.domElements.feedbackFunction.addClass("hide");
    },

    removeCookies: function () {
        localStorage.removeItem(indexManager.currentUrlCookieName);
    },

    getCustomerCookiesName: function (key) {
        return `${key}_${$("#CustId").val()}_${$("#SubAccId").val()}`;
    },

    replaceCookieslessURL: function (newFeatureLink) {
        //// Rebuild URL for Reports/Bet Lists incase of internal
        if (newFeatureLink.attr("href").indexOf("(S(") === -1) {
            return;
        }

        let retryTimes = 0;
        let rebuildNewFeatureLinkInterval = setInterval(function () {
            if (menuBar.domElements.sessionId
                && menuBar.domElements.reportSessionId
                && newFeatureLink.attr("href").indexOf(menuBar.domElements.reportSessionId) === -1) {
                newFeatureLink.attr("href", newFeatureLink.attr("href").replace(menuBar.domElements.sessionId, menuBar.domElements.reportSessionId))
                clearInterval(rebuildNewFeatureLinkInterval);
            }

            retryTimes++;
            retryTimes >= 10 && clearInterval(rebuildNewFeatureLinkInterval);
        }, 1000);
    },
};

$(function () {
    indexManager.init();
});
