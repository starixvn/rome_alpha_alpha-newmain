/// <reference path="../core.ts" />

interface Window {
    hasPopupOpen(): boolean;
    securityCodePopup: any;
    setSecurityCodePopupTitle(securityCodeTitle): void;
    revertSecurityCodePopupTitle(): void;
    ageWnd: any;    
}

interface String {
    hc(): string;
    ec(text: string): string;
    encrypt(): string;
}

declare var Sha1: any;
declare var Sha256: any;
declare var CryptoJS: any;

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
        var $container = $(container)
        var securityCode = $container.val();
        var validSC = securityHelper.isValidSecurityCode(securityCode);
        validSC ? securityHelper.showSuccessfullyIcon($container) : securityHelper.showFailIcon($container);
    },
    validatePassword: function (container, isChangeMemberPassword = false) {
        var $container = $(container)
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
        $messageContainer.removeClass('msg-success');
        $messageContainer.addClass('msg-error').empty().html(failMessage);
    },
    isValidPassword: function (password, isChangeMemberPassword = false) {
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
        })
    },
    hideHint: function () {
        $('input[type=text]').each(function () {
            $(this).removeAttr("type").prop('type', 'password');
        })
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
            var preUrl = top.securityCodePopup.iframeAttributes.preUrl as string;
            top[preUrl]();
            top.securityCodePopup.close();
            return;
        }

        if (top.ageWnd
            && top.ageWnd.iframeAttributes
            && top.ageWnd.iframeAttributes.preUrl) {
            var preUrl = top.ageWnd.iframeAttributes.preUrl as string;
            top[preUrl]();
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
}

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

    var pwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(plainText), key,
    {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });

    return pwd.toString();
};