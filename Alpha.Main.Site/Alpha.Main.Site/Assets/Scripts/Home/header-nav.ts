/// <reference path="../core.ts" />
/// <reference path="index.ts" />


//'use strict';

var headerNav = {
    domElements: {
        clock: null,
        languageCode: null,
        language: null,
        languageSelectorContainer: null,

        iconLogOut: null,
        iconHome: null,
        swapView: null,
        changeLanguageLink: null,

        sideBar: null,
        sideBarContent: null,
        profileImage: null
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
        home: alpha.urls.oldMainRootPath + '_Reports/PortalPage/PortalPage.aspx',
        changeLanguage: alpha.urls.mainRootPath + 'Language/Change'
    },

    init: function () {
        headerNav.initClock();
        headerNav.runClock();

        headerNav.cachedDomElements();
        headerNav.initLanguage();
        headerNav.registerEvents();

        $('[data-toggle="tooltip"]').tooltip();
    },

    initClock: function () {
        // to keep the clock difference least to the actual time,
        // we must run this first of all 
        headerNav.domElements.clock = $('#clock');
        var clock = headerNav.domElements.clock;
        headerNav.resources.labelAM = 'AM';
        headerNav.resources.labelPM = 'PM';
        headerNav.resources.labelMonths = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

        var clock = headerNav.domElements.clock;
        headerNav.clockTime = new Date(
            clock.attr("data-year"),
            clock.attr("data-month") - 1,
            clock.attr("data-day"),
            clock.attr("data-hour"),
            clock.attr("data-minute"),
            clock.attr("data-second"),
            0);
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
        var strMinute = minute >= 10 ? minute : '0' + minute;
        var strSecond = second >= 10 ? second : '0' + second;
        var stra = hour >= 12 ? headerNav.resources.labelPM : headerNav.resources.labelAM;

        var str = strHours + ":" + strMinute + ":" + strSecond + " " + stra + " " +
            headerNav.resources.labelMonths[month] + " " + day + ", " + year + " GMT-4";
        return str;
    },
    cachedDomElements: function () {
        headerNav.domElements.languageCode = $("#language-code");
        headerNav.domElements.language = $("#language");
        headerNav.domElements.languageSelectorContainer = $(".language-selector-container");

        headerNav.domElements.iconLogOut = $(".icon-logout");
        headerNav.domElements.iconHome = $(".icon-home");
        headerNav.domElements.changeLanguageLink = $(".changelanguage-link");

        headerNav.domElements.sideBar = $('#side-bar');
        headerNav.domElements.sideBarContent = $('.content-wrapper');
        headerNav.domElements.profileImage = $("#logoProfileImg");
    },

    initLanguage: function () {
        headerNav.languageCode = headerNav.domElements.languageCode.val();

        var currentLanguage = headerNav.domElements.language.data("currentlanguage");
        var language = $(".list-language ." + currentLanguage + " > a").html();

        headerNav.domElements.languageSelectorContainer.html(language);
    },

    registerEvents: function () {

        headerNav.domElements.iconLogOut.on('click touchend', alpha.core.signOut);

        headerNav.domElements.iconHome.click(function () {
            index.changeMenu(headerNav.urls.home);
        });

        headerNav.domElements.changeLanguageLink.click(function () {
            headerNav.changeLanguage($(this).data("value"));
        });
    },

    changeLanguage: function (lang) {
        $.ajax({
            type: "POST",
            url: headerNav.urls.changeLanguage,
            data: { languageCode: lang },
            responseType: "json",
            success: function (result) {
                if (result !== undefined) {
                    // Remove www so that subdomain can override LANGUAGE value
                    var domain = window.location.hostname.replace(/([a-zA-Z0-9]+.)/, '').replace('www.', '');
                    $.cookie('LANGUAGE', lang, { expires: 30, path: domain });
                    headerNav.reloadPage();
                }
            }
        });
    },
    reloadPage: function () {
        top.location = headerNav.removeBookmarksInUrl(top.location.href);
    },
    removeBookmarksInUrl: function (url) {
        if (typeof url != 'string') {
            return null;
        }

        var i = url.indexOf('#');
        if (i == -1) {
            return url;
        }

        return url.substr(0, i);
    }
};

$(function () {
    headerNav.init();
});
