var alpha = {
    urls: {
        oldMainRootPath: top.document.getElementById('oldMainRootPath') != null
            ? top.document.getElementById('oldMainRootPath').value
            : "not found",
        mainRootPath: top.document.getElementById('mainRootPath') != null
            ? top.document.getElementById('mainRootPath').value
            : "not found",
        accountInfoRootPath: top.document.getElementById('accountInfoRootPath') != null
            ? top.document.getElementById('accountInfoRootPath').value
            : "not found",
        memberInfoRootPath: top.document.getElementById('memberInfoRootPath') != null
            ? top.document.getElementById('memberInfoRootPath').value
            : "not found",
        howToUseRootPath: top.document.getElementById('howToUseRootPath') != null
            ? top.document.getElementById('howToUseRootPath').value
            : "not found",
        exTotalBetsForecastRootPath: top.document.getElementById('exTotalBetsForecastRootPath') != null
            ? top.document.getElementById('exTotalBetsForecastRootPath').value
            : "not found",
        messageRootPath: top.document.getElementById('messageRootPath') != null
            ? top.document.getElementById('messageRootPath').value
            : "not found"
    },
    user: {
        onUserId: document.getElementById('onUserId') != null
            ? document.getElementById('onUserId').value
            : "not found",
        passwordHash: document.getElementById('passwordHash') != null
            ? document.getElementById('passwordHash').value
            : "not found",
        subPageAccess: document.getElementById('subPageAccess') != null
            ? document.getElementById('subPageAccess').value
            : "not found",
        level: document.getElementById('userLevel') != null
            ? document.getElementById('userLevel').value
            : "not found",
        isSubAccount: document.getElementById('isSubAccount') != null
            ? (document.getElementById('isSubAccount').value.toLowerCase() === "true")
            : "not found",
        isShowIntroduction: document.getElementById('isShowIntroduction') != null
            ? (document.getElementById('isShowIntroduction').value.toLowerCase() === "true")
            : "not found",
        isEnableOtp: document.getElementById('isEnabledOtp') != null
            ? (document.getElementById('isEnabledOtp').value.toLowerCase() === "true")
            : false
    },
    core: {
        convertReponseToJson: function (reponse) {
            return eval("(" + reponse + ")");
        },
        signOut: function () {
            var index = window;
            while (index != index.parent) {
                index = index.parent;
            }
            var isInternal = index.$("#isInternal").val();
            var signOutUrl = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx';
            if (isInternal.toString() !== "0") {
                $.ajax(signOutUrl, {
                    success: function () {
                        index.close();
                    }
                });
            }
            else {
                index.location = signOutUrl;
            }
        },
        isIe: function () {
            return /(MSIE|Trident\/|Edge\/)/i.test(navigator.userAgent);
        },
        IsIe8OrLower: function () {
            var userAgent = window.navigator.userAgent.toLowerCase();
            return (userAgent.indexOf('msie') != -1)
                ? (parseInt(userAgent.split('msie')[1]) < 9)
                : false;
        },
        IsAndroid: function () {
            return /Android/i.test(navigator.userAgent);
        }
    },
    isScreenGreater1024: false,
    isScreenLower768: false,
    isScreenGreater768AndLower1024: false,
    isScreenLower1024: false,
    isMobile: false,
    detectResponsiveInformation: function () {
        if (alpha.core.isIe()) {
            //// Work arround because in IE, bootstrap modal dont delegate focusin event to others.
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        }
        var currentWindowWidth = window.screen.width;
        if (currentWindowWidth < 768) {
            alpha.isScreenLower768 = true;
        }
        else if (currentWindowWidth <= 1024) {
            alpha.isScreenGreater768AndLower1024 = true;
        }
        else {
            alpha.isScreenGreater1024 = true;
        }
        alpha.isMobile = isMobile.any;
    },
    localStorage: {
        get: function (key) {
            return store.enabled ? store.get(key) : $.cookie(key);
        },
        set: function (key, value) {
            store.enabled ? store.set(key, value) : $.cookie(key, value, alpha.localStorage.getCookieExpireOptions());
        },
        check: function (key) {
            return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
        },
        getCookieExpireOptions: function () {
            var cookieExpire = { expires: 365, path: window.location.pathname };
            if (alpha.core.isIe()) {
                cookieExpire = { expires: 365 };
            }
            return cookieExpire;
        },
    },
    showMessage: function ($target, result) {
        $target.empty();
        if (result.isSuccessful) {
            $target.html("<div class='msg msg-success'>" + result.message + "</div>");
            return;
        }
        if (result.isWarning) {
            $target.html("<div class='msg msg-warning'>" + result.message + "</div>");
            return;
        }
        if (result.isFailed) {
            $target.html("<div class='msg msg-error'>" + result.message + "</div>");
            return;
        }
    },
    key: {
        instruction: 'Instruction',
        mobileInstruction: 'MobileInstruction'
    },
    messageType: {
        Normal: 0,
        Personal: 1,
        Special: 2,
        System: 3
    }
};
$(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var antiXSRFToken = document.getElementsByName("__RequestVerificationToken");
            if (antiXSRFToken.length > 0) {
                xhr.setRequestHeader("__RequestVerificationToken", antiXSRFToken[0].value);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status === 401) {
                alert('Oops... you are here because of a system error, please notify the administrators.');
                alpha.core.signOut();
            }
        }
    });
});
var Result = /** @class */ (function () {
    function Result(data) {
        this.code = data.Code;
        this.message = data.Message || "";
        this.isFailed = this.code !== 0;
        this.isSuccessful = this.code === 0;
        this.isWarning = data.Code === Result.warningCode;
    }
    Result.warningCode = -1000;
    return Result;
}());
//# sourceMappingURL=core.js.map
'use strict';
var securityCodeResources = {
    rules: "<i class='icon-information-outline icon-rules' data-toggle='modal' data-target='#sc'></i>",
    oldSecurityCodeEmpty: document.getElementById('alert-old-securitycode-empty').value,
    securityCodeEmpty: document.getElementById('alert-securitycode-empty').value,
    securityCodeWrongRule: document.getElementById('alert-securitycode-wrong-rule').value,
    errorSecurityCodeConfirm: document.getElementById('alert-error-securitycode-confirm').value,
    sameSecurityCode: document.getElementById('alert-same-securitycode').value,
    securityCodeChangeSucessfully: document.getElementById('alert-securitycode-change-ok').value
};
//# sourceMappingURL=security-code-resources.js.map
/// <reference path="../core.ts" />
var securityHelper = {
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
    validateSecurityCode: function (container) {
        var $container = $(container);
        var securityCode = $container.val();
        var validSC = securityHelper.isValidSecurityCode(securityCode);
        validSC ? securityHelper.showSuccessfullyIcon($container) : securityHelper.showFailIcon($container);
    },
    validatePassword: function (container, isChangeMemberPassword) {
        if (isChangeMemberPassword === void 0) { isChangeMemberPassword = false; }
        var $container = $(container);
        var password = $container.val();
        var validPassword = securityHelper.isValidPassword(password, isChangeMemberPassword);
        validPassword ? securityHelper.showSuccessfullyIcon($container) : securityHelper.showFailIcon($container);
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
        setTimeout(securityHelper.gotoHomePage, timeout);
    },
    signOut: alpha.core.signOut,
    gotoHomePage: function () {
        window.location.href = alpha.urls.mainRootPath;
    },
    initLanguage: function () {
        $(".changelanguage-link").click(function () {
            securityHelper.changeLanguage($(this).data("value"));
        });
    },
    changeLanguage: function (lang) {
        $.ajax({
            type: "POST",
            url: $("#mainRootPath").val() + 'Language/Change',
            data: { languageCode: lang },
            success: function (result) {
                if (result !== undefined) {
                    window.location.reload();
                }
            }
        });
    },
    showSuccessfullyIcon: function ($container) {
        $container.removeClass('error');
        $container.addClass('valid');
    },
    showFailIcon: function ($container) {
        $container.removeClass('valid');
        $container.addClass('error');
    },
    showSuccessfullyMessage: function ($messageContainer, successfullyMessage) {
        $messageContainer.removeClass('msg-error');
        $messageContainer.addClass('msg-success').empty().html(successfullyMessage);
    },
    showFailMessage: function ($messageContainer, failMessage) {
        $($messageContainer).parent().removeClass('hidden');
        $messageContainer.removeClass('msg-success');
        $messageContainer.addClass('msg-error').empty().html(failMessage);
    },
    isValidPassword: function (password, isChangeMemberPassword) {
        if (isChangeMemberPassword === void 0) { isChangeMemberPassword = false; }
        var whiteSpace = /^\S*$/;
        var whiteSpaceCase = password.search(whiteSpace) != -1;
        if (isChangeMemberPassword) {
            var memberRegex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,10}$/;
            return (password.search(memberRegex) != -1) && whiteSpaceCase;
        }
        else {
            var regex1 = /^(?=.{8,100}$).*\d/i;
            var regex2 = /^(?=.{8,100}$).*[a-z]/;
            var regex3 = /^(?=.{8,100}$).*[A-Z]/;
            var regex4 = /^(?=.{8,100}$).*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]/i;
            var case1 = password.search(regex1) != -1 ? 1 : 0;
            var case2 = password.search(regex2) != -1 ? 1 : 0;
            var case3 = password.search(regex3) != -1 ? 1 : 0;
            var case4 = password.search(regex4) != -1 ? 1 : 0;
            return (case1 + case2 + case3 + case4 >= 3) && whiteSpaceCase;
        }
    },
    showHint: function () {
        $('input[type=password]').each(function () {
            $(this).removeAttr("type").prop('type', 'text');
        });
    },
    hideHint: function () {
        $('input[type=text]').each(function () {
            $(this).removeAttr("type").prop('type', 'password');
        });
    },
    showSC: function () {
        $('input[type=tel]').each(function () {
            $(this).addClass('show-hint');
        });
    },
    hideSC: function () {
        $('input[type=tel]').each(function () {
            $(this).removeClass('show-hint');
        });
    },
    afterVerifed: function () {
        if (top.hasPopupOpen()) {
            top.revertSecurityCodePopupTitle();
        }
        var redirectUrl = securityHelper.getUrlVars()['redirectUrl'];
        if (redirectUrl) {
            window.location.href = decodeURIComponent(redirectUrl);
            return;
        }
        if (top.securityCodePopup
            && top.securityCodePopup.iframeAttributes
            && top.securityCodePopup.iframeAttributes.preUrl) {
            var preUrl = top.securityCodePopup.iframeAttributes.preUrl;
            top[preUrl]();
            top.securityCodePopup.close();
            return;
        }
        if (top.ageWnd
            && top.ageWnd.iframeAttributes
            && top.ageWnd.iframeAttributes.preUrl) {
            var preUrl = top.ageWnd.iframeAttributes.preUrl;
            top[preUrl]();
            return;
        }
        if (top.productPopupUrl) {
            top[top.productPopupUrl]();
            return;
        }
        securityHelper.redirectToHomePage(200);
    },
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }
};
String.prototype.ec = function (key) {
    var pt = this;
    var s = new Array();
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
String.prototype.encrypt = function () {
    var plainText = this;
    plainText = plainText.toString();
    var secretKey = $('#hashkey').val();
    var passPhrase = 'a5s8d2e9c172';
    var key = CryptoJS.enc.Utf8.parse(passPhrase + secretKey);
    var iv = key;
    var pwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(plainText), key, {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });
    return pwd.toString();
};
//# sourceMappingURL=security-helper.js.map
/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />
/// <reference path="security-code-resources.ts" />
'use strict';
var verifySecurityCode = {
    initialize: function () {
        var actorProfilePicture = JSON.parse(localStorage.getItem('actorProfilePicture'));
        $('.profile-avatar').attr("src", actorProfilePicture.Image);
        $('#txtSecCode').keypress(function (e) {
            if (e.which == 13) {
                verifySecurityCode.verify();
                e.preventDefault();
            }
        });
        $('.form-SC').submit(function (e) {
            verifySecurityCode.verify();
            e.preventDefault();
        });
        $('#txtSecCode').val("").focus();
        $("#txtSecCode").keyup(function () {
            securityHelper.validateSecurityCode(this);
        });
        $("#showSCchk").click(function () {
            var isChecked = $(this).is(":checked");
            isChecked ? securityHelper.showHint() : securityHelper.hideHint();
        });
        if (top.hasPopupOpen()) {
            top.setSecurityCodePopupTitle($("#title_header").text());
            $("#container").addClass("has-popup");
            $("#title_header").remove();
        }
    },
    verify: function () {
        $.blockUI();
        var $container = $('#status-message');
        var $messageContainer = $('#status-message-content');
        $messageContainer.removeClass('msg-warning');
        var $txtSecurityCode = $('#txtSecCode');
        var message = verifySecurityCode.checkInputParameters($txtSecurityCode);
        if (message.length !== 0) {
            securityHelper.showFailMessage($messageContainer, message);
            $.unblockUI();
            return;
        }
        var secCode = $txtSecurityCode.val();
        secCode = securityHelper.hashSecurityCode(secCode);
        $.ajax({
            type: 'POST',
            url: 'VerifySecurityCode',
            data: { securityCode: secCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                var code = data.Code;
                if (code === 0) { // OK
                    $txtSecurityCode.attr("disabled", "disabled");
                    $("#btn-submit").remove();
                    if (top.hasPopupOpen()) {
                        top.revertSecurityCodePopupTitle();
                    }
                    securityHelper.afterVerifed();
                }
                else if (code === 2) { // account is closed 
                    alert(data.Message);
                    securityHelper.signOut();
                }
                else {
                    securityHelper.showFailMessage($messageContainer, data.Message);
                    $txtSecurityCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        });
    },
    checkInputParameters: function ($txtSecurityCode) {
        var message = '';
        var secCode = $txtSecurityCode.val();
        if (secCode.length === 0) {
            message = securityCodeResources.securityCodeEmpty;
            $txtSecurityCode.focus();
        }
        return message;
    }
};
$(function () {
    verifySecurityCode.initialize();
});
//# sourceMappingURL=verify-securitycode.js.map