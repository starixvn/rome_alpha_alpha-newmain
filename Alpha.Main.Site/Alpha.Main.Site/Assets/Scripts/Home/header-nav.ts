/// <reference path="../core.ts" />
/// <reference path="index-manager.ts" />

'use strict';

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
        iconSearch: null,
        linkLogout: null,
        iconBell: null,
        iconMessage: null,
        languageDropdownSubMenu: null,

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
        home: alpha.urls.mainRootPath + "Dashboard/Index",
        messages: alpha.urls.mainRootPath + "Message/Index",
        changeLanguage: alpha.urls.mainRootPath + "Language/Change",
        queryUserName: alpha.urls.oldMainRootPath + "_AccountInfo/Handlers/QueryUserName.ashx"
    },

    init: function () {
        headerNav.initClock();
        headerNav.runClock();

        headerNav.cachedDomElements();
        headerNav.initLanguage();
        headerNav.registerEvents();
    },

    initClock: function () {
        // to keep the clock difference least to the actual time,
        // we must run this first of all
        headerNav.domElements.clock = $("#clock");
        headerNav.resources.labelAM = "AM";
        headerNav.resources.labelPM = "PM";
        headerNav.resources.labelMonths = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

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
        var strMinute = minute >= 10 ? minute : "0" + minute;
        var strSecond = second >= 10 ? second : "0" + second;
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
        // For mobile
        headerNav.domElements.iconSearch = $(".icon-account-search");
        headerNav.domElements.linkLogout = $(".link-logout");
        headerNav.domElements.iconBell = $(".icon-bell");
        headerNav.domElements.iconMessage = $(".icon-message");
        headerNav.domElements.languageDropdownSubMenu = $('.dropdown-submenu.language');

        headerNav.domElements.sideBar = $("#side-bar");
        headerNav.domElements.sideBarContent = $(".content-wrapper");
        headerNav.domElements.profileImage = $("#logoProfileImg");
    },

    initLanguage: function () {
        headerNav.languageCode = headerNav.domElements.languageCode.val();

        var currentLanguage = headerNav.domElements.language.data("currentlanguage");
        var language = $(".list-language ." + currentLanguage + " > a").html();

        headerNav.domElements.languageSelectorContainer.html(language);
    },

    registerEvents: function () {
        headerNav.domElements.iconLogOut.on("click", alpha.core.signOut);

        headerNav.domElements.iconHome.on("click", () => {
            indexManager.changeMenu(headerNav.urls.home);
        });

        headerNav.domElements.changeLanguageLink.on('click touchend', (event) => {
            headerNav.changeLanguage($(event.target).data("value"));
        });

        // Logout link for mobile
        headerNav.domElements.linkLogout.on("click", alpha.core.signOut);

        headerNav.domElements.iconMessage.on("click", () => {
            indexManager.changeMenu(headerNav.urls.messages);

            $('.icon-message').find('.circle-red').hide();
        });

        // Quick search component for mobile
        headerNav.domElements.iconSearch.on('click', () => {
            let topSearchbar = $('.top-search-bar'),
                topSearchInput = $('.top-search-input'),
                iconClose = $('.icon-close'),
                searchBarToggleClass = 'search-bar-toggle';

            topSearchbar.addClass(searchBarToggleClass);
            topSearchInput.val('');
            headerNav.domElements.sideBarContent.append("<div class='blockUI'></div>");

            const onCloseSearchBar = () => {
                topSearchbar.removeClass(searchBarToggleClass);
                headerNav.domElements.sideBarContent.find(".blockUI").remove();
            };

            const getSuggestions = (query, process) => {
                return $.get(
                    headerNav.urls.queryUserName,
                    { query: query },
                    (data) => {
                        let suggestions = alpha.core.convertReponseToJson(data).suggestions;

                        if (typeof suggestions !== 'undefined' && suggestions !== null && suggestions.length !== 0) {
                            let sugestionItems = suggestions.map((item) => {
                                return item[0];
                            });

                            return process(sugestionItems);
                        }

                        return process(["Not found"]);
                    });
            };

            const afterSelectSuggestionItem = (item) => {
                onCloseSearchBar();
                menuBar.collapseLeftPanel();
                menuBar.activateAccountInfoTab(null, item);
            }

            iconClose.on('click', onCloseSearchBar);

            $(".blockUI").on('click', onCloseSearchBar);

            const typeaheadOptions: TypeaheadOptions = {
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

        // Annoucement popup for mobile
        headerNav.domElements.iconBell.on('click', () => {
            let counter = new PopupAnnouncement.AnnouncementCounter($);
            const announcementMobile = new PopupAnnouncement.Mobile($, counter);
            announcementMobile.startLoading();
        });

        // Language show options for mobile
        headerNav.domElements.languageDropdownSubMenu.on('click', (e) => {
            headerNav.domElements.languageDropdownSubMenu.find('.list-language').toggle();
            e.stopPropagation();
        });
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