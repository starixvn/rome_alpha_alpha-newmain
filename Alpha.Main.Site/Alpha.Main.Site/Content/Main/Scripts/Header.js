var headerManager;

var HeaderManager = function () {
    var self = this;
    self.enums = {
        ErrorCode: {
            AccessDenied: 1
        }
    };

    self.init = function () {
        self.runClock();

        self.initElements();
        self.bindEvents();
    };

    self.initElements = function () {
        var language = jQuery(".list-language ." + _resources.language + " > a").html();
        jQuery(".language-text").html(language);
    };
    self.bindEvents = function () {
        jQuery(".changelanguage-link").click(function () {
            self.changeLanguage($(this).attr("data-value"));
        });
    };

    self.runClock = function () {
        _resources.sec++;
        if (_resources.sec == 60) {
            _resources.sec = 0;
            _resources.min++;
        }
        if (_resources.min == 60) {
            _resources.min = 0;
            _resources.hrs++;
        }
        if (_resources.hrs == 24) {
            _resources.hrs = 0;
            _resources.sec = 0;
            _resources.min = 0;
            _resources.day++;
        }
        if (_resources.min < 10) {
            strmin = "0" + _resources.min;
        } else {
            strmin = _resources.min;
        }
        if (_resources.sec < 10) {
            strsec = "0" + _resources.sec;
        } else {
            strsec = _resources.sec;
        }

        if (!self.isDate(_resources.year, _resources.month, _resources.day)) {
            //to next _resources.month
            _resources.month++;
            _resources.day = 1;
        }
        if (!self.isDate(_resources.year, _resources.month, _resources.day)) {
            //to next _resources.year
            _resources.year++;
            _resources.month = 1;
            _resources.day = 1;
        }

        if (_resources.hrs >= 12) {
            stra = _resources.PM;
        } else {
            stra = _resources.AM;
        }
        if (_resources.hrs >= 12) {
            strhrs = _resources.hrs - 12;
        } else {
            strhrs = _resources.hrs;
        }

        var months = eval(_resources.months);

        var str = strhrs + ":" + strmin + ":" + strsec + " " + stra + " " +
            months[_resources.month - 1] + " " + _resources.day + ", " + _resources.year + " GMT-4";
        $("#clock").html(str);

        setTimeout(self.runClock, 1000);
    };
    self.isDate = function (TDY, TDM, TDD) {
        // input 3 string
        var BegDate = new Date(parseFloat(TDY) + "/" + TDM + "/" + TDD);
        if (BegDate.getYear() < 200) var TBegYear = BegDate.getYear() + 1900
        else var TBegYear = BegDate.getYear()
        if (TBegYear != parseFloat(TDY) || BegDate.getMonth() + 1 != parseFloat(TDM) || BegDate.getDate() != parseFloat(TDD)) return false;
        else return true;
    }
    self.changeLanguage = function (lang) {
        var url = top.baseMainApp + 'Home/ChangeLanguage';
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

var headerManager = new HeaderManager();
jQuery(document).ready(function () {
    headerManager.init();
});