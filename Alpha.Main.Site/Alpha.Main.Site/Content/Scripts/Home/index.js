/// <reference path="../core.js" />

'use strict';
/*global alpha, briefProfile*/

var index = {
    currentTab: null,
    currentPageCookieName: 'DFFKDHFJKLDHFJDH',

    domElements: {
        mainFrame: null,
        partialContents: null
    },

    urls: {
        reCheckinUrl: alpha.urls.oldMainRootPath + '_Authorization/Handlers/ReCheckin.ashx'
    },

    init: function () {
        window.document.title = alpha.urls.mainRootPath.substr(1, alpha.urls.mainRootPath.length - 2);
        $.material.init();

        index.cachedDomElements();
        index.setCurrentPage();
        index.loadPartialViews();
        index.registerEvents();

        setTimeout(index.cacheCurrentPage, 1000);
    },

    registerEvents: function () {
        setTimeout(index.reCheckin, 30000);
    },

    cachedDomElements: function () {
        index.domElements.mainFrame = $('#main');
        index.domElements.partialContents = $('.partial-content');
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

                    if ($(item).hasClass('navigation')) {
                        menuBar.init();
                    }
                });
            }
        });
    },

    cacheCurrentPage: function () {
        var currentUrl = document.getElementById("main").contentWindow.location.href;
       
        $.cookie(index.currentPageCookieName, currentUrl, { expires: 30, path: '/' });

        setTimeout(index.cacheCurrentPage, 1000);
    },

    setCurrentPage: function () {
        var currentPage = $.cookie(index.currentPageCookieName);

        if (currentPage) {
            index.domElements.mainFrame.attr('src', currentPage);
        }
        else {
            index.domElements.mainFrame.attr('src', index.domElements.mainFrame.data('default-url'));
        }
    }
};

$(function () {
    index.init();
});
