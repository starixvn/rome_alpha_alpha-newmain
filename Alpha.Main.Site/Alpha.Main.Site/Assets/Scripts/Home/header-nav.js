/// <reference path="../core.js" />
/// <reference path="~/contents/libs/moment/moment-with-locales.min.js" />
/// <reference path="index-page.js" />

'use strict';

var headerNav = {
    domElements: {
        clock: null,
        languageCode: null,
        language: null,
        languageSelectorContainer: null,

        iconLogOut: null,
        iconCollapse: null,
        iconHome: null,
        swapView: null,
        changeLanguageLink: null,

        sideBar: null,
        sideBarContent: null,
        profileImage: null,

        headerDropdownMenu: null,
        btnHeaderMenu: null
    },

    resources: {
        labelAM: '',
        labelPM: '',
        months: []
    },

    languageCode: null,
    momentTime: null,
    hideHeaderDropdownMenuTimeout: null,

    urls: {
        home: alpha.urls.oldMainRootPath + '_Reports/PortalPage/PortalPage.aspx',
        oldMainIndex: alpha.urls.oldMainRootPath + "Index.aspx",
        changeLanguage: alpha.urls.mainRootPath + 'Language/Change'
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
        headerNav.domElements.clock = $('#clock');
        var clock = headerNav.domElements.clock;
        headerNav.resources.labelAM = clock.data("label-am");
        headerNav.resources.labelPM = clock.data("label-pm");
        headerNav.resources.labelMonths = JSON.parse(clock.data("label-months").replace(/'/g, '"'));

        var clock = headerNav.domElements.clock;
        headerNav.momentTime = moment({
            year: clock.attr("data-year"),
            month: clock.attr("data-month") - 1,
            day: clock.attr("data-day"),
            hour: clock.attr("data-hour"),
            minute: clock.attr("data-minute"),
            second: clock.attr("data-second"),
            milliseconds: 0
        });
    },
    runClock: function () {
        headerNav.momentTime.add(1, 'seconds');

        // Moment use hour from 1->12
        // var str = headerNav.momentTime.lang('zh').format("hh:mm:ss A MMM DD, YYYY") + " GMT-4";

        var hour = headerNav.momentTime.get("hour");
        var year = headerNav.momentTime.get("year");
        var month = headerNav.momentTime.get("month");
        var day = headerNav.momentTime.get("day");

        // Expected: hour from 0->11
        var strhrs = hour >= 12 ? hour - 12 : hour;
        var stra = hour >= 12 ? headerNav.resources.labelPM : headerNav.resources.labelAM;
        var str = strhrs + ":" + headerNav.momentTime.format("mm:ss") + " " + stra + " " +
            headerNav.resources.labelMonths[month] + " " + day + ", " + year + " GMT-4";

        headerNav.domElements.clock.html(str);
        setTimeout(headerNav.runClock, 1000);
    },
    cachedDomElements: function () {
        headerNav.domElements.languageCode = $("#language-code");
        headerNav.domElements.language = $("#language");
        headerNav.domElements.languageSelectorContainer = $(".language-selector-container");

        headerNav.domElements.iconLogOut = $(".icon-logout");
        headerNav.domElements.iconCollapse = $('.icon-collapse');
        headerNav.domElements.iconHome = $(".icon-home");
        headerNav.domElements.swapView = $(".icon-swap-horizontal, .old-view-text");
        headerNav.domElements.changeLanguageLink = $(".changelanguage-link");

        headerNav.domElements.sideBar = $('.sidebar');
        headerNav.domElements.sideBarContent = $('.content-wrapper');
        headerNav.domElements.profileImage = $("#logoProfileImg");

        headerNav.domElements.headerDropdownMenu = $('#header-dropdown-menu');
        headerNav.domElements.btnHeaderMenu = $('#btn-header-menu');
    },

    initLanguage: function () {
        headerNav.languageCode = headerNav.domElements.languageCode.val();

        var currentLanguage = headerNav.domElements.language.data("currentlanguage");
        var language = $(".list-language ." + currentLanguage + " > a").html();

        headerNav.domElements.languageSelectorContainer.html(language);
    },

    registerEvents: function () {
        headerNav.domElements.iconLogOut.click(alpha.core.signOut);

        headerNav.domElements.iconCollapse.click(function () {
            headerNav.collapseLeftPanel($(this));
        });

        headerNav.domElements.iconHome.click(function () {
            index.changeMenu(headerNav.urls.home);
        });

        headerNav.domElements.swapView.click(function () {
            top.location = headerNav.urls.oldMainIndex;
        });

        headerNav.domElements.changeLanguageLink.click(function () {
            headerNav.changeLanguage($(this).data("value"));
        });

        headerNav.domElements.headerDropdownMenu
            .off('mouseenter.headerNav')
            .on('mouseenter.headerNav', function () {
                clearTimeout(headerNav.hideHeaderDropdownMenuTimeout);
            })
            .off('mouseleave.headerNav')
            .on('mouseleave.headerNav', function () {
                headerNav.hideHeaderDropdownMenuTimeout = setTimeout(headerNav.hideHeaderMenu, 5000);
            });

        headerNav.domElements.btnHeaderMenu
            .off('click.headerNav')
            .on('click.headerNav', function () {
                headerNav.hideHeaderDropdownMenuTimeout = setTimeout(headerNav.hideHeaderMenu, 5000);
            });

        $(document).click(function () {
            headerNav.hideHeaderMenu();
        });

        headerNav.domElements.btnHeaderMenu
            .off('click.headerNav')
            .on('click.headerNav', function () {
                headerNav.hideHeaderDropdownMenuTimeout = setTimeout(headerNav.hideHeaderMenu, 5000);
            });
    },

    hideHeaderMenu: function () {
        if (headerNav.domElements.headerDropdownMenu.parent().hasClass('open')) {
            headerNav.domElements.headerDropdownMenu.dropdown('toggle');
        }
    },

    collapseLeftPanel: function (collapseButton) {
        var sidebar = headerNav.domElements.sideBar;
        var content = headerNav.domElements.sideBarContent;

        var isCollapse = sidebar.hasClass('collapsed');
        var activeSubMenu = $('.menu-item.active .sub-menu');

        if (isCollapse) {
            activeSubMenu.css('display', 'block');
            sidebar.removeClass('collapsed');
            content.removeClass('expanded');
            headerNav.domElements.profileImage.removeClass("collapse-image");
        } else {
            activeSubMenu.css('display', 'none');
            sidebar.addClass('collapsed');
            content.addClass('expanded');
            headerNav.domElements.profileImage.addClass("collapse-image");
        }
    },
    changeLanguage: function (lang) {
        jQuery.ajax({
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
