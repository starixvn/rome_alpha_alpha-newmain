/// <reference path="../core.js" />

'use strict';
/*global alpha, briefProfile*/

var index = {
    currentTab: null,
    currentPageCookieName: 'DFFKDHFJKLDHFJDH',

    domElements: {
        mainFrame: null,
        partialContents: null,
        content: null,
        accountInfoContent: null
    },

    urls: {
        reCheckinUrl: alpha.urls.oldMainRootPath + '_Authorization/Handlers/ReCheckin.ashx'
    },

    init: function () {
        $.material.init();
        index.cachedDomElements();
        index.loadPartialViews();
        index.registerEvents();

        index.setCurrentUrl();
        setTimeout(index.cacheCurrentUrl, 1000);
    },

    registerEvents: function () {
        setTimeout(index.reCheckin, 30000);
    },

    cachedDomElements: function () {
        index.domElements.partialContents = $('.partial-content');
        index.domElements.content = $('#content');
        index.domElements.accountInfoContent = $('#accountinfo-content');
    },

    changeMenu: function (url) {
        index.domElements.mainFrame.attr('src', url);
    },

    reCheckin: function () {
        // Post with return result in JSON format
        $.cookie('ssl', location.href.indexOf('https://') === 0, { expires: 30, path: '/' });

        $.ajax({
            type: "POST",
            url: index.urls.reCheckinUrl,
            data: { onid: alpha.user.onUserId },
            responseType: "json",
            success: function (result) {
                result = alpha.core.convertReponseToJson(result);
                var errCode = result.errCode;
                if (errCode === 0) {
                    setTimeout(index.reCheckin, 30000);
                } else {
                    var errMsg = result.errMsg;
                    if (errMsg) {
                        alert(errMsg);
                        alpha.core.signOut();
                    } else {
                        alert('Could not contact the server. Please check your network connection.');
                    }
                }
            }
        });
    },

    loadPartialViews: function () {
        index.domElements.partialContents.each(function (index, partialContent) {
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
        var currentUrl = document.getElementById("main").contentWindow.location.href;

        $.cookie(index.currentPageCookieName, currentUrl, { expires: 1, path: '/' });
    },

    setCurrentUrl: function () {
        var currentUrl = index.domElements.content.data('default-url');

        if (index.domElements.mainFrame
            && index.domElements.mainFrame.length > 0) {
            index.domElements.mainFrame.attr('src', currentUrl);
        }
        else {
            index.domElements.content.append('<iframe frameborder="0" id="main" src="' + currentUrl + '" width="100%" height="100%" scrolling="auto" name="main" marginwidth="0" marginheight="0" allowtransparency="true" class="iframe-content"></iframe>');
            index.domElements.mainFrame = $('#main');
            index.domElements.mainFrame.load(function () {
                var iframeWindow = index.domElements.mainFrame[0].contentWindow || index.domElements.mainFrame[0].contentDocument;
                index.closeAllOpenDropdowns(iframeWindow);
            });

            index.domElements.accountInfoContent.load(function () {
                var iframeWindow = index.domElements.accountInfoContent[0].contentWindow || index.domElements.accountInfoContent[0].contentDocument;
                index.closeAllOpenDropdowns(iframeWindow);
            });
        }
    },

    closeAllOpenDropdowns: function (iframeWindow) {
        $(iframeWindow.document).click(function () {
            $('.dropdown-toggle').each(function (index, dropdown) {
                var $dropdown = $(dropdown);
                if ($dropdown.parent().hasClass('open')) {
                    $dropdown.dropdown('toggle');
                }
            });
        });
    },
    saveCurrentUrl: function (currentUrl) {
        $.cookie(index.currentPageCookieName, currentUrl, { expires: 1, path: '/' });
    }
};

$(function () {
    index.init();
});
