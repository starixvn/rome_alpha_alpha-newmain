/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />
/// <reference path="security-code-resources.ts" />
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
        $("#txtSecCode").keyup(function () {
            securityHelper.validateSecurityCode(this);
        });
        $("#showSCchk").click(function () {
            var isChecked = $(this).is(":checked");
            isChecked ? securityHelper.showHint() : securityHelper.hideHint();
        });
        if (top.hasPopupOpen()) {
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
                if (code === 0) {
                    var redirectUrl = verifySecurityCode.getUrlVars()['redirectUrl'];
                    if (redirectUrl) {
                        window.location.href = decodeURIComponent(redirectUrl);
                        return;
                    }
                    if (top.securityCodePopup && top.securityCodePopup.iframeAttributes.preUrl) {
                        var preUrl = top.securityCodePopup.iframeAttributes.preUrl;
                        top[preUrl]();
                        top.securityCodePopup.close();
                        return;
                    }
                    if (top.ageWnd && top.ageWnd.iframeAttributes.preUrl) {
                        var preUrl = top.ageWnd.iframeAttributes.preUrl;
                        top[preUrl]();
                        return;
                    }
                    securityHelper.redirectToHomePage(200);
                }
                else if (code === 2) {
                    alert(data.Messsage);
                    securityHelper.signOut();
                }
                else {
                    securityHelper.showFailMessage($messageContainer, data.Messsage);
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
$(function () {
    verifySecurityCode.initialize();
});
//# sourceMappingURL=verify-securitycode.js.map