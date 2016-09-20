'use strict';

$(document).ready(function () {
    verifySecurityCode.initialize();
})

var verifySecurityCode = {
    resources: {
        inform: $('#inform-securitycode').val()
    },
    initialize: function () {
        $('#SC').keypress(function (e) {
            if (e.which == 13) {
                verifySecurityCode.verify();
                e.preventDefault();
            }
        });

        $('.form-SC').submit(function (e) {
            verifySecurityCode.verify();
            e.preventDefault();
        });

        $('#txtSecCode').focus();
    },
    verify: function () {
        securitycodeHelper.lockUI();
        var $messageContainer = $('#error-message');
        var $txtSecurityCode = $('#txtSecCode');
        var message = verifySecurityCode.checkInputParameters($txtSecurityCode);
        if (message.length !== 0) {
            $messageContainer.empty().html(message);
            securitycodeHelper.unlockUI();
            return;
        }

        var secCode = $txtSecurityCode.val();
        secCode = securitycodeHelper.hashSecurityCode(secCode);
        $.ajax({
            type: 'POST',
            url: 'VerifySecurityCode',
            data: { securityCode: secCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                var errorCode = data.ErrorCode;
                if (errorCode === 0) { // OK
                    var redirectUrl = verifySecurityCode.getUrlVars()['redirectUrl'];
                    if (redirectUrl) {
                        window.location.href = decodeURIComponent(redirectUrl);
                        return;
                    }
                    
                    if (top.popupSecurityCode && top.popupSecurityCode.iframeAttributes.preUrl) {
                        var preUrl = top.popupSecurityCode.iframeAttributes.preUrl;
                        top[preUrl]();
                        top.popupSecurityCode.close();
                        return;
                    }

                    redirectToHomePage(200);
                }
                else if (errorCode === 2) { // account is closed 
                    alert(data.ErrorMessage);
                    top.age.SignOut();
                }
                else {
                    $messageContainer.empty().html(data.ErrorMessage);
                    $txtSecurityCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                securitycodeHelper.unlockUI();
            }
        })
    },
    checkInputParameters: function ($txtSecurityCode) {
        var message = '';
        var secCode = $txtSecurityCode.val();
        if (secCode.length === 0) {
            message = verifySecurityCode.resources.inform;
            $txtSecurityCode.focus();
        }
        return message;
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