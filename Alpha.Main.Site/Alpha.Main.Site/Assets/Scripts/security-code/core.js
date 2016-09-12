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
showMessage = function ($container, message, type) {
    var oldClassName = $container.prop('class');
    if (type === 0) { // success
        if (oldClassName !== 'successSection') {
            var $msg_1_1 = $('#msg_1_1');
            $msg_1_1.removeClass();
            $msg_1_1.addClass('succ_1_1');
            var $msg_1_2 = $('#msg_1_2');
            $msg_1_2.removeClass();
            $msg_1_2.addClass('succ_1_2');
            var $msg_1_3 = $('#msg_1_3');
            $msg_1_3.removeClass();
            $msg_1_3.addClass('succ_1_3');
            var $msg_2_1 = $('#msg_2_1');
            $msg_2_1.removeClass();
            $msg_2_1.addClass('succ_2_1');
            var $msg_2_2 = $('#msg_2_2');
            $msg_2_2.removeClass();
            $msg_2_2.addClass('succ_2_2');
            var $msg_3_1 = $('#msg_3_1');
            $msg_3_1.removeClass();
            $msg_3_1.addClass('succ_3_1');
            var $msg_3_2 = $('#msg_3_2');
            $msg_3_2.removeClass();
            $msg_3_2.addClass('succ_3_2');
            var $msg_3_3 = $('#msg_3_3');
            $msg_3_3.removeClass();
            $msg_3_3.addClass('succ_3_3');

            $container.removeClass();
            $container.addClass('successSection');
        }
    }
    else {
        if (oldClassName !== 'errorSection') {
            var $msg_1_1 = $('#msg_1_1');
            $msg_1_1.removeClass();
            $msg_1_1.addClass('emsg_1_1');
            var $msg_1_2 = $('#msg_1_2');
            $msg_1_2.removeClass();
            $msg_1_2.addClass('emsg_1_2');
            var $msg_1_3 = $('#msg_1_3');
            $msg_1_3.removeClass();
            $msg_1_3.addClass('emsg_1_3');
            var $msg_2_1 = $('#msg_2_1');
            $msg_2_1.removeClass();
            $msg_2_1.addClass('emsg_2_1');
            var $msg_2_2 = $('#msg_2_2');
            $msg_2_2.removeClass();
            $msg_2_2.addClass('emsg_2_2');
            var $msg_3_1 = $('#msg_3_1');
            $msg_3_1.removeClass();
            $msg_3_1.addClass('emsg_3_1');
            var $msg_3_2 = $('#msg_3_2');
            $msg_3_2.removeClass();
            $msg_3_2.addClass('emsg_3_2');
            var $msg_3_3 = $('#msg_3_3');
            $msg_3_3.removeClass();
            $msg_3_3.addClass('emsg_3_3');

            $container.removeClass();
            $container.addClass('errorSection');
        }
    }
    $container.html(message);
};
isValidSecurityCode = function (secCode) {
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
};
validateSecurityCode = function (container, errorContainerName) {
    var $container = $(container);
    var $errorContainer = $('#' + errorContainerName);
    var errorContainerClassName = $errorContainer.prop('class');

    var securityCode = $container.val();
    var validSC = isValidSecurityCode(securityCode);
    if (validSC && errorContainerClassName !== 'tick') {
        $errorContainer.removeClass();
        $errorContainer.addClass('tick');
    }
    else if (!validSC && errorContainerClassName !== 'crossError') {
        $errorContainer.removeClass();
        $errorContainer.addClass('crossError');
    }
};
hashSecurityCode = function (securityCode) {
    var salt = window.securitySalt;
    var token = window.securityCodeToken
    if (!salt || !token) {
        return '';
    }

    var sha1SC = Sha1.hash(securityCode + salt);
    var sha256SC = Sha256.hash(sha1SC + token);
    return sha256SC;
};
redirectToHomePage = function (timeout) {
    setTimeout(gotoHomePage, timeout);
};
lockUI = function () {
    try { parent.age.Lock(false) } catch (err) { };
};
unlockUI = function () {
    try { parent.age.UnLock() } catch (err) { };
};
getBaseUrl = function () {
    var url = top.baseUrl;
    if (!url) {
        var urlArray = window.location.href.split('/');
        var protocol = urlArray[0];
        var host = urlArray[2];
        url = protocol + '//' + host + '/';
    }

    return url;
};
gotoHomePage = function () {
    var wnd = window.parent;
    while (wnd != wnd.parent) // Jump out
    {
        wnd = wnd.parent;
    }

    var baseUrl = getBaseUrl();
    if (top.mainRootPath) {
        wnd.location = top.mainRootPath + 'Default';
    } else {
        wnd.location = baseUrl + 'Index.aspx';
    }
};
/*refresh = function () {
    var wnd = window.parent;
    while (wnd != wnd.parent) // Jump out
    {
        wnd = wnd.parent;
    }
    wnd.location = wnd.location.href;
};*/

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