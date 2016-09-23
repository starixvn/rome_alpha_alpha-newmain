'use strict';

var verifySecurityCode = {
    initialize: function () {
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

        $('#txtSecCode').focus();
    },
    verify: function () {
        $.blockUI();
        var $messageContainer = $('#error-message');
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
                    securityHelper.signOut();
                }
                else {
                    securityHelper.showFailMessage($messageContainer, data.ErrorMessage);
                    $txtSecurityCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        })
    },
    checkInputParameters: function ($txtSecurityCode) {
        var message = '';
        var secCode = $txtSecurityCode.val();
        if (secCode.length === 0) {
            message = securityCodeResources.inform;
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

$(function () {
    verifySecurityCode.initialize();
});
