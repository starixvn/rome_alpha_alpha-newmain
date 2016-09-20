var securitycodeHelper = {
    isValidSecurityCode: function (secCode) {
        var secCodeLength = secCode ? secCode.length : 0;
        if (secCodeLength != 6) { // check length
            return false;
        }
        // check digits and all of them are not the same
        var regex = /^([0-9])(?!\1+$)[0-9]+$/;
        var result = secCode.search(regex) != -1 ? true : false;
        if (result) { // check 'Forward Run' and 'Backward Run'
            var isForwardRun = true, isBackwardRun = true;
            var forward = 0, backward = 0;
            forward = backward = parseInt(secCode.charAt(0));
            for (var i = 1; i < secCodeLength; i++) {
                var current = parseInt(secCode.charAt(i));
                if (++forward != current) {
                    isForwardRun = false;
                }
                if (--backward != current) {
                    isBackwardRun = false;
                }
            }
            result = !(isForwardRun || isBackwardRun);
        }
        return result;
    },
    hashSecurityCode: function (securityCode) {
        var salt = $("#security-salt").val();
        var token = $("#securitycode-token").val();
        if (!salt || !token) {
            return '';
        }

        var sha1SC = Sha1.hash(securityCode + salt);
        var sha256SC = Sha256.hash(sha1SC + token);
        return sha256SC;
    },
    redirectToHomePage: function (timeout) {
        setTimeout(securitycodeHelper.gotoHomePage, timeout);
    },
    getBaseUrl: function () {
        var url = top.baseUrl;
        if (!url) {
            var urlArray = window.location.href.split('/');
            var protocol = urlArray[0];
            var host = urlArray[2];
            url = protocol + '//' + host + '/';
        }

        return url;
    },
    signOut: function () {
        window.location = securitycodeHelper.getBaseUrl() + $("#oldMainRootPath").val() + '_Authorization/Handlers/SignOut.ashx';
    },
    gotoHomePage: function () {
        window.location = securitycodeHelper.getBaseUrl() + $("#mainRootPath").val() + '/Default/Index';
    },
    initLanguage: function () {
        var languageCode = $("#language-code").val();
        var currentLanguage = $("#language").data("currentlanguage");
        var language = $(".list-language ." + currentLanguage + " > a").html();
        $(".language-selector-container").html(language);

        $(".changelanguage-link").click(function () {
            securitycodeHelper.changeLanguage($(this).data("value"));
        });
    },
    changeLanguage: function (lang) {
        $.ajax({
            type: "POST",
            url: securitycodeHelper.getBaseUrl() + $("#mainRootPath").val() + 'Language/Change',
            data: { languageCode: lang },
            responseType: "json",
            success: function (result) {
                if (result !== undefined) {
                    window.location.reload();
                }
            }
        });
    },
    lockUI: function () {
    },
    unlockUI: function () {
    }
}

String.prototype.ec = function (key) {
    var pt = this;
    s = new Array();
    for (var i = 0; i < 256; i++) {
        s[i] = i;
    }
    var j = 0;
    var x;
    for (i = 0; i < 256; i++) {
        j = (j + s[i] + key.charCodeAt(i % key.length)) % 256;
        x = s[i];
        s[i] = s[j];
        s[j] = x;
    }
    i = 0;
    j = 0;
    var ct = '';
    for (var y = 0; y < pt.length; y++) {
        i = (i + 1) % 256;
        j = (j + s[i]) % 256;
        x = s[i];
        s[i] = s[j];
        s[j] = x;
        ct += String.fromCharCode(pt.charCodeAt(y) ^ s[(s[i] + s[j]) % 256]);
    }
    return ct;
};

String.prototype.hc = function () {
    var b16_digits = '0123456789abcdef';
    var b16_map = new Array();
    for (var i = 0; i < 256; i++) {
        b16_map[i] = b16_digits.charAt(i >> 4) + b16_digits.charAt(i & 15);
    }
    var result = new Array();
    for (var i = 0; i < this.length; i++) {
        result[i] = b16_map[this.charCodeAt(i)];
    }
    return result.join('');
};

$(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var antiXSRFToken = document.getElementsByName("__RequestVerificationToken");
            if (antiXSRFToken.length > 0) {
                xhr.setRequestHeader("__RequestVerificationToken", antiXSRFToken[0].value);
            }
        }
    });
});