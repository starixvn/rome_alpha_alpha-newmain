/// <reference path="../core.js" />

'use strict';
/*global alpha, briefProfile*/

var indexPage = {
    currentTab: null,

    domElements: {
        mainFrame: null,
        partialContents: null
    },

    init: function () {
        window.document.title = alpha.urls.mainRootPath.substr(1, alpha.urls.mainRootPath.length - 2);
        $.material.init();

        indexPage.cachedDomElements();
        indexPage.loadPartialViews();
        indexPage.registerEvents();
    },

    registerEvents: function () {
        setTimeout(indexPage.reCheckin, 30000);
    },

    cachedDomElements: function () {
        indexPage.domElements.mainFrame = $('#main');
        indexPage.domElements.partialContents = $('.partial-content');
    },

    changeMenu: function (url) {
        indexPage.domElements.mainFrame.attr('src', url);
    },

    reCheckin: function () {
        // Post with return result in JSON format
        $.cookie('ssl', location.href.indexOf('https://') === 0, { expires: 30, path: '/' });

        var url = alpha.urls.oldMainRootPath + '_Authorization/Handlers/ReCheckin.ashx';

        $.ajax({
            type: "POST",
            url: url,
            data: { onid: alpha.user.onUserId },
            responseType: "json",
            success: function (result) {
                result = alpha.core.convertReponseToJson(result);
                var errCode = result.errCode;
                if (errCode === 0) {
                    setTimeout(indexPage.reCheckin, 30000);
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
        indexPage.domElements.partialContents.each(function (index, item) {
            var url = $(item).data('url');
            if (url && url.length > 0) {
                $(item).load(url + '?' + Math.random(), function () {
                    if ($(item).hasClass('customer-profile')) {
                        briefProfile.init();
                    }
                });
            }
        });
    }
};

$(function () {
    indexPage.init();
});
