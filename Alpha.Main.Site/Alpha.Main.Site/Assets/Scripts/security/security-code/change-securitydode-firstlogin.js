'use strict';

var changeSecurityCodeFirstLogin = {
    initialize: function () {
        securityHelper.initLanguage();
        $('.form-SC').submit(function (e) {
            changeSecurityCodeFirstLogin.changeSecurityCode();
            e.preventDefault();
        });
        $("#btn-back").bind("click", function () {
            securityHelper.signOut();
        });

        $('#txtSecCode').focus();
    },
    changeSecurityCode: function () {
        $.blockUI();
        var $txtSecurityCode = $('#txtSecCode');
        var $txtConfirmSecurityCode = $('#txtConfirmSecCode');
        if (!changeSecurityCodeFirstLogin.validateInputSecurityCode($txtSecurityCode, $txtConfirmSecurityCode)) {
            $.unblockUI();
            return;
        }

        var hashKey = $('#hashkey').val();
        var secCode = $txtSecurityCode.val();
        secCode = secCode.ec(hashKey).hc();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        confirmSecCode = confirmSecCode.ec(hashKey).hc();
        $.ajax({
            type: 'POST',
            url: 'ChangeSecurityCodeFirstLogin',
            data: { securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorCode === 0) {
                    securityHelper.redirectToHomePage(2000);
                }
                else {
                    $("#txtSecCode").addClass('error');
                    $("#txtSecCode-error").empty().html(data.ErrorMessage);
                    $txtSecurityCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        })
    },
    validateInputSecurityCode: function ($txtSecurityCode, $txtConfirmSecurityCode) {
        changeSecurityCodeFirstLogin.resetOriginalInputStatus();
        var secCode = $txtSecurityCode.val();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        if (secCode.length === 0) {
            $("#txtSecCode").addClass('error');
            $("#txtSecCode-error").empty().html(securityCodeResources.securityCodeEmpty).append(securityCodeResources.rules);
            $txtSecurityCode.focus();
            return false;
        }
        else if (!securityHelper.isValidSecurityCode(secCode)) {
            $("#txtSecCode").addClass('error');
            $("#txtSecCode-error").empty().html(securityCodeResources.securityCodeWrongRule).append(securityCodeResources.rules);
            $txtSecurityCode.focus();
            return false;
        }
        else if (secCode != confirmSecCode) {
            $("#txtConfirmSecCode").addClass('error');
            $("#txtConfirmSecCode-error").empty().html(securityCodeResources.errorSecurityCodeConfirm);
            $txtConfirmSecurityCode.focus();
            return false;
        }

        return true;
    },
    resetOriginalInputStatus: function () {
        $("#txtSecCode").removeClass('error');
        $("#txtSecCode-error").empty();
        $("#txtConfirmSecCode").removeClass('error');
        $("#txtConfirmSecCode-error").empty();
    }
}

$(function () {
    changeSecurityCodeFirstLogin.initialize();
});