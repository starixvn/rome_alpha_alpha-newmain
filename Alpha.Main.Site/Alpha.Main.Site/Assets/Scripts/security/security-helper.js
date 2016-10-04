var securityHelper = {
    isValidSecurityCode: function (secCode) {
        var secCodeLength = secCode ? secCode.length : 0;
        if (secCodeLength != 6) {
            return false;
        }
        // check digits and all of them are not the same
        var regex = /^([0-9])(?!\1+$)[0-9]+$/;
        var result = secCode.search(regex) != -1 ? true : false;
        if (result) {
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
    validatePassword: function (container) {
        var $container = $(container);
        var password = $container.val();
        var validPassword = securityHelper.isValidPassword(password);
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
    signOut: function () {
        window.location.href = $("#oldMainRootPath").val() + '_Authorization/Handlers/SignOut.ashx';
    },
    gotoHomePage: function () {
        window.location.href = $("#mainRootPath").val();
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
            responseType: "json",
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
    isValidPassword: function (password) {
        var regex1 = /^(?=.{8,100}$).*\d/i;
        var regex2 = /^(?=.{8,100}$).*[a-z]/;
        var regex3 = /^(?=.{8,100}$).*[A-Z]/;
        var regex4 = /^(?=.{8,100}$).*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]/i;
        var whiteSpace = /^\S*$/;
        var case1 = password.search(regex1) != -1 ? 1 : 0;
        var case2 = password.search(regex2) != -1 ? 1 : 0;
        var case3 = password.search(regex3) != -1 ? 1 : 0;
        var case4 = password.search(regex4) != -1 ? 1 : 0;
        var case5 = password.search(whiteSpace) != -1;
        return (case1 + case2 + case3 + case4 >= 3) && case5;
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
