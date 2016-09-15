$(document).ready(function () {
    verifySecurityCode.initialize();
})
var verifySecurityCode = {
    initialize: function () {
        $('#divVerifySecurityBlock').removeClass('hidden');
        $('#SC').keypress(function (e) {
            if (e.which == 13) {
                verifySecurityCode.verify();
                e.preventDefault();
            }
        });
        var $form = $('.form-SC');
        $form.submit(function (e) {
            verifySecurityCode.verify();
            e.preventDefault();
        });
        $('#txtSecCode').focus();
    },
    verify: function () {
        lockUI();
        var $messageContainer = $('#errorSection');
        var $txtSecCode = $('#SC');
        var message = verifySecurityCode.checkInputParameters($txtSecCode);
        if (message.length !== 0) {
            showMessage($messageContainer, message, 1);
            unlockUI();
            return;
        }

        var secCode = $txtSecCode.val();
        secCode = hashSecurityCode(secCode);
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
                    showMessage($messageContainer, data.ErrorMessage, data.ErrorCode);
                    $txtSecCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                unlockUI();
            }
        })
    },
    checkInputParameters: function ($txtSecCode) {
        var message = '';
        var secCode = $txtSecCode.val();
        if (secCode.length === 0) {
            message = securityCodeResource.informSecCode;
            $txtSecCode.focus();
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