/// <reference path="../core.js" />

'use strict';
/*global alpha, briefProfile*/

var index = {
    currentTab: null,
    currentPageCookieName: 'DFFKDHFJKLDHFJDH',

    domElements: {
        mainFrame: null,
        partialContents: null,
        content: null
    },

    urls: {
        reCheckinUrl: alpha.urls.oldMainRootPath + '_Authorization/Handlers/ReCheckin.ashx'
    },

    init: function () {
        window.document.title = alpha.urls.mainRootPath.substr(1, alpha.urls.mainRootPath.length - 2);
        $.material.init();

        index.cachedDomElements();
        index.setCurrentUrl();
        index.loadPartialViews();
        index.registerEvents();

        setTimeout(index.cacheCurrentUrl, 1000);
    },

    registerEvents: function () {
        setTimeout(index.reCheckin, 30000);
    },

    cachedDomElements: function () {
        index.domElements.partialContents = $('.partial-content');
        index.domElements.content = $('#content');
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
        index.domElements.partialContents.each(function (index, item) {
            var url = $(item).data('url');
            if (url && url.length > 0) {
                $(item).load(url + '?' + Math.random(), function () {
                    if ($(item).hasClass('customer-profile')) {
                        briefProfile.init();
                    }
                });
            }
        });
    },

    cacheCurrentUrl: function () {
        var currentUrl = document.getElementById("main").contentWindow.location.href;
       
        $.cookie(index.currentPageCookieName, currentUrl, { expires: 1, path: '/' });

        setTimeout(index.cacheCurrentUrl, 1000);
    },

    setCurrentUrl: function () {
        var currentUrl = $.cookie(index.currentPageCookieName) || index.domElements.content.data('default-url');

        if (index.domElements.mainFrame
            && index.domElements.mainFrame.length > 0) {
            index.domElements.mainFrame.attr('src', currentUrl);
        }
        else {
            index.domElements.content.append('<iframe frameborder="0" id="main" src="' + currentUrl + '" width="100%" height="100%" scrolling="auto" name="main" marginwidth="0" marginheight="0" allowtransparency="true" class="iframe-content"></iframe>');
            index.domElements.mainFrame = $('#main');
        }
    }
};

$(function () {
    index.init();
});
