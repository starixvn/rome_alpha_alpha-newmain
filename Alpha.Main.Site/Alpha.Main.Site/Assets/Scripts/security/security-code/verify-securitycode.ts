/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />
/// <reference path="security-code-resources.ts" />

'use strict';
var verifySecurityCode = {
    initialize: function () {
        const actorProfilePicture = JSON.parse(localStorage.getItem('actorProfilePicture'));
        $('.profile-avatar').attr("src", actorProfilePicture.Image);

        const $txtSecCode = $('#txtSecCode');
        $txtSecCode.keypress(function (e) {
            if (e.which == 13) {
                verifySecurityCode.verify();
                e.preventDefault();
            }
        });

        $('.form-SC').submit(function (e) {
            verifySecurityCode.verify();
            e.preventDefault();
        });

        $txtSecCode.val("");

        $txtSecCode.keyup(function () {
            securityHelper.validateSecurityCode(this);
        });

        $("#showSCchk").click(function () {
            var isChecked = $(this).is(":checked");
            isChecked ? $txtSecCode.addClass("show-hint") : $txtSecCode.removeClass("show-hint");
        });

        if (top.hasPopupOpen()) {
            top.setSecurityCodePopupTitle($("#title_header").text());
            $("#container").addClass("has-popup");
            $("#verify-security-code-container").addClass("has-popup");
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
        })
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
}

$(function () {
    verifySecurityCode.initialize();
});
