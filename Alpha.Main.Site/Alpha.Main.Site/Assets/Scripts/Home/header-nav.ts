/// <reference path="../core.ts" />
/// <reference path="index-manager.ts" />

'use strict';

declare var LiveChatWidget: any;

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
        liveChat: null,
        switchMode: null
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
    clockTimeInterval: null,

    urls: {
        home: alpha.urls.mainRootPath + "Dashboard/Index2",
        qrCodeTop: alpha.urls.memberInfoRootPath + "QRCodeMemberSignIn",
        messages: alpha.urls.messageRootPath + "MessageHistory/Index",
        announcements: alpha.urls.messageRootPath + "Announcement/Index",
        changeTheme: alpha.urls.mainRootPath + "Theme/Change",
        changeLanguage: alpha.urls.mainRootPath + "Language/Change",
        queryUserName: alpha.urls.oldMainRootPath + "_AccountInfo/Handlers/QueryUserName.ashx",
        checkNewMessage: alpha.urls.messageRootPath + "NewMessage/GetNumberOfNewAnnouncement",
        banner: alpha.urls.mainRootPath + "Banner/Get?type=",
        serverTime: alpha.urls.mainRootPath + "Dashboard/GetServerTime"
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
        headerNav.clockTimeInterval = setInterval(headerNav.runClock, 1000);

        headerNav.cachedDomElements();
        headerNav.initLanguage();
        headerNav.registerEvents();

        $(".open-link").unbind().bind('click', event => { alpha.viewDataURL(event); });
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
    },
    getServerTime: function (onUserId = "-1") {
        $.get(headerNav.urls.serverTime + "?onUserId=" + onUserId, function (data) {
            if (data && data.signOut) {
                alpha.core.signOut();

                return;
            }

            headerNav.clockTimeInterval && clearInterval(headerNav.clockTimeInterval);
            headerNav.clockTime = new Date(
                data.Year,
                data.Month - 1,
                data.Day,
                data.Hour,
                data.Minute,
                data.Second,
                0);
            headerNav.clockTimeInterval = setInterval(headerNav.runClock, 1000);
        });
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

        headerNav.domElements.liveChat = $("#liveChat");
        headerNav.domElements.switchMode = $(".switch-mode");
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
        } else if (type == headerNav.messageType.Personal) {
            headerNav.unreadMessage.Total = headerNav.unreadMessage.Total - headerNav.unreadMessage.Personal;
            headerNav.unreadMessage.Personal = 0;
        } else if (type == headerNav.messageType.System) {
            headerNav.unreadMessage.Total = headerNav.unreadMessage.Total - headerNav.unreadMessage.System;
            headerNav.unreadMessage.System = 0;
        } else {
            headerNav.unreadMessage.Total = headerNav.unreadMessage.Total - headerNav.unreadMessage.Special;
            headerNav.unreadMessage.Special = 0;
        }
    },

    buildIconNewMessageByType: function (type) {
        headerNav.markMessageAsRead(type);

        if (headerNav.unreadMessage.Total > 0) {
            headerNav.domElements.numberNewMessage.text(headerNav.unreadMessage.Total > headerNav.maximunNewMessage.value ? headerNav.maximunNewMessage.text : headerNav.unreadMessage.Total);
        } else {
            headerNav.domElements.numberNewMessage.text("");
            headerNav.domElements.numberNewMessage.removeClass("badge-title");
        }
    },

    buildIconNewMessage: function () {
        var firstUrl = headerNav.popFirstNewMessage();
        if (headerNav.unreadMessage.Total > 0) {
            headerNav.domElements.numberNewMessage.text(headerNav.unreadMessage.Total > headerNav.maximunNewMessage.value ? headerNav.maximunNewMessage.text : headerNav.unreadMessage.Total);
        } else {
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
                        } else {
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
        } else if (headerNav.unreadMessage.System > 0) {
            headerNav.markMessageAsRead(headerNav.messageType.System);

            return headerNav.urls.announcements;
        } else if (headerNav.unreadMessage.Normal > 0) {
            headerNav.markMessageAsRead(headerNav.messageType.Normal);

            return headerNav.urls.messages + "?messageType=" + headerNav.messageType.Normal;
        }

        headerNav.markMessageAsRead(headerNav.messageType.Personal);

        return headerNav.urls.messages + "?messageType=" + headerNav.messageType.Personal;
    },

    registerEvents: function () {
        headerNav.domElements.iconLogOut.on("click", alpha.core.signOut2);

        headerNav.domElements.iconHome.on("click", () => {
            indexManager.changeMenu(headerNav.urls.home);
        });

        headerNav.domElements.iconQRCodeTop.on("click", () => {
            indexManager.changeMenu(headerNav.urls.qrCodeTop);
        });

        headerNav.domElements.switchMode.on('click', headerNav.changeTheme);

        headerNav.domElements.changeLanguageLink.on('click touchend', (event) => {
            headerNav.changeLanguage($(event.target).data("value"), false);
        });

        // Logout link for mobile
        headerNav.domElements.linkLogout.on("click", alpha.core.signOut2);

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
                                return item[0] + (item[3] ? " (" + item[3] + ")" : "");
                            });

                            return process(sugestionItems);
                        }

                        return process(["Not found"]);
                    });
            };

            const afterSelectSuggestionItem = (item) => {
                onCloseSearchBar();
                menuBar.collapseLeftPanel();
                menuBar.activateAccountInfoTab(null, item.replace(/\(\S+?\)/g, "").trim());
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

        // Language show options for mobile
        headerNav.domElements.languageDropdownSubMenu.on('click', (e) => {
            headerNav.domElements.languageDropdownSubMenu.find('.list-language').toggle();
            e.stopPropagation();
        });

        headerNav.domElements.iconNewMessage.on("click", headerNav.buildIconNewMessage);
        headerNav.domElements.numberNewMessage.on("click", headerNav.buildIconNewMessage);

        headerNav.checkNewMessage();

        headerNav.domElements.liveChat.one("click", function () {
            headerNav.showLiveChat();
        });
    },

    changeLanguage: function (lang, isDisplayLanguage) {
        $.ajax({
            type: "POST",
            url: headerNav.urls.changeLanguage,
            data: { languageCode: lang, displayLanguage: isDisplayLanguage },
            success: function (result) {
                if (result !== undefined) {
                    headerNav.reloadPage();
                }
            }
        });
    },

    changeTheme: function () {
        const isDarkMode = $(this).hasClass("dark-mode");
        $.ajax({
            type: "POST",
            url: headerNav.urls.changeTheme,
            data: { isDarkMode: !isDarkMode },
            success: function (result) {
                result && headerNav.reloadPage();
            }
        });
    },

    reloadPage: function () {
        top.location.href = headerNav.removeBookmarksInUrl(top.location.href);
    },

    setProfilePictureOnNav: function (profilePicure) {
        $('#userPicture').attr('src', profilePicure);
        $('#profile-avatar-image').attr('src', profilePicure);
        $("#logoProfileImg").attr('src', profilePicure);
        briefProfile.setProfilePictureStorage();
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
    },

    showLiveChat: () => {
        LiveChatWidget.init();
        LiveChatWidget.call("set_customer_name", $("#UsernameShowOnLiveChat").val());
        LiveChatWidget.call("maximize");

        LiveChatWidget.on("visibility_changed", function (data) {
            data.visibility === "minimized" && LiveChatWidget.call("hide");
        });

        headerNav.domElements.liveChat.on("click", function () {
            LiveChatWidget.call("maximize");
        });
    }
};

$(function () {
    headerNav.init();
    window.onfocus = function () {
        headerNav.getServerTime(alpha.user.onUserId || "-1");
    };

    indexManager.domElements.mainFrame.on('load.newmain', function () {
        headerNav.getServerTime(alpha.user.onUserId || "-1");
    });

    try {
        document.addEventListener("visibilitychange", function () {
            !document.hidden && headerNav.getServerTime(alpha.user.onUserId || "-1");
        });
    } catch (e) {
    }
});