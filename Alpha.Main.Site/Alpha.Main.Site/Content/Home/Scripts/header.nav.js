var headerNavigationManager;

$(document).ready(function () {
    headerNavigationManager.init();
});

var HeaderNavigationManager = function () {
    var self = this;
    self.data = {
        clock: {
            year: 0,
            month: 0,
            day: 0,
            hour: 0,
            minute: 0,
            second: 0,
            labelAM: '',
            labelPM: '',
            months: []
        },
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
        var currentLanguage = $("#language").attr("data-currentlanguage");
        var language = $(".list-language ." + currentLanguage + " > a").html();
        $(".language-selector-container").html(language);
    };
    self.initClock = function () {
        var clock = $("#clock");
        self.data.clock.year = clock.attr("data-year");
        self.data.clock.month = clock.attr("data-month");
        self.data.clock.day = clock.attr("data-day");
        self.data.clock.hour = clock.attr("data-hour");
        self.data.clock.minute = clock.attr("data-minute");
        self.data.clock.second = clock.attr("data-second");
        self.data.clock.labelAM = clock.attr("data-label-am");
        self.data.clock.labelPM = clock.attr("data-label-pm");
        self.data.clock.labelMonths = JSON.parse(clock.attr("data-label-months").replace(/'/g, '"'));
    };
    self.runClock = function () {
        self.data.clock.second++;
        if (self.data.clock.second == 60) {
            self.data.clock.second = 0;
            self.data.clock.minute++;
        }

        if (self.data.clock.minute == 60) {
            self.data.clock.minute = 0;
            self.data.clock.hour++;
        }

        if (self.data.clock.hour == 24) {
            self.data.clock.hour = 0;
            self.data.clock.second = 0;
            self.data.clock.minute = 0;
            self.data.clock.day++;
        }

        strmin = self.data.clock.minute < 10 ? "0" + self.data.clock.minute : self.data.clock.minute;
        strsec = self.data.clock.second < 10 ? "0" + self.data.clock.second : self.data.clock.second;

        if (!_.isDate(new Date(parseFloat(self.data.clock.year) + "/" + self.data.clock.month + "/" + self.data.clock.day))) {
            //to next self.data.clock.month
            self.data.clock.month++;
            self.data.clock.day = 1;
        }

        if (!_.isDate(new Date(parseFloat(self.data.clock.year) + "/" + self.data.clock.month + "/" + self.data.clock.day))) {
            //to next self.data.clock.year
            self.data.clock.year++;
            self.data.clock.month = 1;
            self.data.clock.day = 1;
        }

        stra = self.data.clock.hour >= 12 ? self.data.clock.labelPM : self.data.clock.labelAM;
        strhrs = self.data.clock.hour >= 12 ? self.data.clock.hour - 12 : self.data.clock.hour;

        var str = strhrs + ":" + strmin + ":" + strsec + " " + stra + " " +
            self.data.clock.labelMonths[self.data.clock.month - 1] + " " + self.data.clock.day + ", " + self.data.clock.year + " GMT-4";
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
            indexPageManager.changeMenu("/" + top.oldMainRootPath + '_Reports/PortalPage/PortalPage.aspx');
        });
        $(".icon-swap-horizontal").click(function () {
            top.location = "/" + top.oldMainRootPath + "Index.aspx";
        });
        $(".old-view-text").click(function () {
            top.location = "/" + top.oldMainRootPath + "Index.aspx";
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
        var url = "/" + top.mainRootPath + 'Home/ChangeLanguage';
        jQuery.ajax({
            type: "POST",
            url: url,
            data: { Language: lang },
            responseType: "json",
            success: function (result) {
                var resultCode = result.ResultCode;
                if (resultCode == 0) {
                    // Remove www so that subdomain can override LANGUAGE value
                    var domain = window.location.hostname.replace(/([a-zA-Z0-9]+.)/, '').replace('www.', '');
                    setCookie('LANGUAGE', lang, 30, domain);
                    self.reloadPage();
                } else {
                    if (resultCode == self.enums.ErrorCode.AccessDenied) {
                        indexPageManager.signOut();
                    }
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