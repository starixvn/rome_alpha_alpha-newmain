var headerNavigationManager;

$(document).ready(function () {
    headerNavigationManager.init();
});

var HeaderNavigationManager = function () {
    var self = this;
    self.data = {
        clock: {
            labelAM: '',
            labelPM: '',
            months: [],
            momentTime: null
        },
        languageCode: 'en'
    };
    self.enums = {
        ErrorCode: {
            AccessDenied: 1
        }
    };

    self.init = function () {
        self.initClock();
        self.runClock();

        self.initElements();
        self.bindEvents();
    };

    self.initElements = function () {
        self.data.languageCode = $("#language-code").val();
        var currentLanguage = $("#language").attr("data-currentlanguage");
        var language = $(".list-language ." + currentLanguage + " > a").html();
        $(".language-selector-container").html(language);
    };
    self.initClock = function () {
        var clock = $("#clock");

        self.data.clock.labelAM = clock.attr("data-label-am");
        self.data.clock.labelPM = clock.attr("data-label-pm");
        self.data.clock.labelMonths = JSON.parse(clock.attr("data-label-months").replace(/'/g, '"'));

        var time = new Date(self.data.clock.year, self.data.clock.month - 1, self.data.clock.day,
            self.data.clock.hour, self.data.clock.minute, self.data.clock.second);
        self.data.clock.momentTime = moment({
            year: clock.attr("data-year"),
            month: clock.attr("data-month") - 1,
            day: clock.attr("data-day"),
            hour: clock.attr("data-hour"),
            minute: clock.attr("data-minute"),
            second: clock.attr("data-second"),
            milliseconds: 0
        });
    };
    self.runClock = function () {
        self.data.clock.momentTime.add(1, 'seconds');

        // Moment use hour from 1->12
        // var str = self.data.clock.momentTime.lang('zh').format("hh:mm:ss A MMM DD, YYYY") + " GMT-4";

        var hour = self.data.clock.momentTime.get("hour");
        var year = self.data.clock.momentTime.get("year");
        var month = self.data.clock.momentTime.get("month");
        var day = self.data.clock.momentTime.get("day");

        // Expected: hour from 0->11   
        var strhrs = hour >= 12 ? hour - 12 : hour;
        var stra = hour >= 12 ? self.data.clock.labelPM : self.data.clock.labelAM;
        var str = strhrs + ":" + self.data.clock.momentTime.format("mm:ss") + " " + stra + " " +
            self.data.clock.labelMonths[month] + " " + day + ", " + year + " GMT-4";

        $("#clock").html(str);
        setTimeout(self.runClock, 1000);
    };

    self.bindEvents = function () {
        $(".icon-logout").click(function () {
            indexPageManager.signOut();
        });
        $('.icon-collapse').click(function () {
            self.collapseLeftPanel($(this));
        });
        $(".icon-home").click(function () {
            indexPageManager.changeMenu(top.oldMainRootPath + '_Reports/PortalPage/PortalPage.aspx');
        });
        $(".icon-swap-horizontal").click(function () {
            top.location = top.oldMainRootPath + "Index.aspx";
        });
        $(".old-view-text").click(function () {
            top.location = top.oldMainRootPath + "Index.aspx";
        });
        $(".changelanguage-link").click(function () {
            self.changeLanguage($(this).attr("data-value"));
        });
    };
    self.collapseLeftPanel = function (collapseButton) {
        var sidebar = $('.sidebar');
        var content = $('.content-wrapper');

        var isCollapse = sidebar.hasClass('collapsed');

        var activeSubMenu = $('.menu-item.active .sub-menu');
        if (isCollapse) {
            activeSubMenu.css('display', 'block');
            sidebar.removeClass('collapsed');
            content.removeClass('expanded');
            $("#logoProfileImg").removeClass("collapse-image");
        } else {
            activeSubMenu.css('display', 'none');
            sidebar.addClass('collapsed');
            content.addClass('expanded');
            $("#logoProfileImg").addClass("collapse-image");
        }
    };
    self.closeListSetting = function () {
        self.activeSettingList();
    };
    self.activeSettingList = function () {
        $('.top-right-menu').parent().toggleClass("active");
    };
    self.changeLanguage = function (lang) {
        var url = top.mainRootPath + 'Language/Change';
        jQuery.ajax({
            type: "POST",
            url: url,
            data: { languageCode: lang },
            responseType: "json",
            success: function (result) {              
                if (result !== undefined) {
                    // Remove www so that subdomain can override LANGUAGE value
                    var domain = window.location.hostname.replace(/([a-zA-Z0-9]+.)/, '').replace('www.', '');
                    setCookie('LANGUAGE', lang, 30, domain);
                    self.reloadPage();
                }
            },
            error: function (jqXhr, event, ajaxSettings) {
            }
        });
    };
    self.reloadPage = function () {
        top.location = self.removeBookmarksInUrl(top.location.href);
    };
    self.removeBookmarksInUrl = function (url) {
        if (typeof url != 'string') {
            return null;
        }

        var i = url.indexOf('#');
        if (i == -1) {
            return url;
        }

        return url.substr(0, i);
    };
};

var headerNavigationManager = new HeaderNavigationManager();