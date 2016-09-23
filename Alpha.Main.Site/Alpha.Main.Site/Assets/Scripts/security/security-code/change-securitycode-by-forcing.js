'use strict';

var changeSecurityCodeByForcing = {
    initialize: function () {
        securityHelper.initLanguage();
        $('.form-SC').submit(function (e) {
            changeSecurityCodeByForcing.changeSecurityCode();
            e.preventDefault();
        });

        $("#btn-back").bind("click", function () {
            securityHelper.signOut();
        });

        $('#txtOldSecCode').focus();
    },
    changeSecurityCode: function () {
        $.blockUI();
        var $txtOldSecurityCode = $('#txtOldSecCode');
        var $txtSecurityCode = $('#txtSecCode');
        var $txtConfirmSecurityCode = $('#txtConfirmSecCode');
        if (!changeSecurityCodeByForcing.validateInputSecurityCode($txtOldSecurityCode, $txtSecurityCode, $txtConfirmSecurityCode)) {
            $.unblockUI();
            return;
        }

        var oldSecCode = $txtOldSecurityCode.val();
        oldSecCode = securityHelper.hashSecurityCode(oldSecCode);
        var secCode = $txtSecurityCode.val();
        var hashKey = $('#hashkey').val();
        secCode = secCode.ec(hashKey).hc();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        confirmSecCode = confirmSecCode.ec(hashKey).hc();
        $.ajax({
            type: 'POST',
            url: 'ChangeSecurityCodeByForcing',
            data: { oldSecurityCode: oldSecCode, securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorCode === 0) { // OK
                    securityHelper.redirectToHomePage(2000);
                }
                else if (data.ErrorCode === 2) { // account is closed
                    alert(data.ErrorMessage);
                    securityHelper.signOut();
                }
                else {
                    $("#txtOldSecCode").addClass('error');
                    $("#txtOldSecCode-error").empty().html(data.ErrorMessage);
                    $txtOldSecurityCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        })
    },
    validateInputSecurityCode: function ($txtOldSecurityCode, $txtSecurityCode, $txtConfirmSecurityCode) {
        changeSecurityCodeByForcing.resetOriginalInputStatus();
        var oldSecCode = $txtOldSecurityCode.val();
        var secCode = $txtSecurityCode.val();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        if (oldSecCode.length === 0) {
            $("#txtOldSecCode").addClass('error');
            $("#txtOldSecCode-error").empty().html(securityCodeResources.oldSecurityCodeEmpty);
            $txtOldSecurityCode.focus();
            return false;
        }
        else if (secCode.length === 0) {
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
        else if (oldSecCode === secCode) {
            $("#txtConfirmSecCode").addClass('error');
            $("#txtConfirmSecCode-error").empty().html(securityCodeResources.sameSecurityCode);
            $txtSecurityCode.focus();
            return false;
        }

        return true;
    },
    resetOriginalInputStatus: function () {
        $("#txtOldSecCode").removeClass('error');
        $("#txtOldSecCode-error").empty();
        $("#txtSecCode").removeClass('error');
        $("#txtSecCode-error").empty();
        $("#txtConfirmSecCode").removeClass('error');
        $("#txtConfirmSecCode-error").empty();
    }
}

$(function () {
    changeSecurityCodeByForcing.initialize();
});