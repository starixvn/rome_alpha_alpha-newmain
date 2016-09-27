'use strict';

var changeUserOwnSecurityCode = {
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            changeUserOwnSecurityCode.change();
            e.preventDefault();
        });

        $form.bind('reset', function (e) {
            changeUserOwnSecurityCode.resetOriginalInputStatus();
        });

        $('#passwordNav').click(function () {
            changeUserOwnSecurityCode.gotoPasswordPage()
        });

        $('#txtOldSecCode').focus();

        $("#txtSecCode").keyup(function () {
            securityHelper.validateSecurityCode(this);
        });

        $("#txtConfirmSecCode").keyup(function () {
            securityHelper.validateSecurityCode(this);
        });

        $("#showSCchk").click(function () {
            var isChecked = $(this).is(":checked");
            isChecked ? securityHelper.showHint() : securityHelper.hideHint();
        });
    },
    change: function () {
        $.blockUI();
        var $container = $('#status-message');
        var $messageContainer = $('#status-message-content');
        $container.removeClass('display-none');
        var $txtOldSecurityCode = $('#txtOldSecCode');
        var $txtSecurityCode = $('#txtSecCode');
        var $txtConfirmSecurityCode = $('#txtConfirmSecCode');
        var message = changeUserOwnSecurityCode.checkInputParameters($txtOldSecurityCode, $txtSecurityCode, $txtConfirmSecurityCode);
        if (message.length !== 0) {
            securityHelper.showFailMessage($messageContainer, message);
            $.unblockUI();
            return;
        }

        var hashKey = $('#hashkey').val();
        var oldSecCode = $txtOldSecurityCode.val();
        oldSecCode = securityHelper.hashSecurityCode(oldSecCode);
        var secCode = $txtSecurityCode.val();
        secCode = secCode.ec(hashKey).hc();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        confirmSecCode = confirmSecCode.ec(hashKey).hc();
        $.ajax({
            type: 'POST',
            url: 'ChangeUserOwnSecurityCode',
            data: { oldSecurityCode: oldSecCode, securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorCode === 0) { // OK
                    securityHelper.showSuccessfullyMessage($messageContainer, securityCodeResources.securityCodeChangeSucessfully);
                    setTimeout(changeUserOwnSecurityCode.refreshPage, 5000);
                }
                else if (data.ErrorCode === 2) { // account is closed
                    alert(data.ErrorMessage);
                    securityHelper.signOut();
                }
                else {
                    securityHelper.showFailMessage($messageContainer, data.ErrorMessage);
                    $txtOldSecurityCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        })
    },
    checkInputParameters: function ($txtOldSecurityCode, $txtSecurityCode, $txtConfirmSecurityCode) {
        var oldSecCode = $txtOldSecurityCode.val();
        var secCode = $txtSecurityCode.val();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        if (oldSecCode.length === 0) {
            $txtOldSecurityCode.focus();
            return securityCodeResources.oldSecurityCodeEmpty;
        }
        else if (secCode.length === 0) {
            $txtSecurityCode.focus();
            return securityCodeResources.securityCodeEmpty;
        }
        else if (!securityHelper.isValidSecurityCode(secCode)) {
            $txtSecurityCode.focus();
            return securityCodeResources.securityCodeWrongRule;
        }
        else if (secCode != confirmSecCode) {
            $txtConfirmSecurityCode.focus();
            return securityCodeResources.errorSecurityCodeConfirm;
        }
        else if (oldSecCode === secCode) {
            $txtSecurityCode.focus();
            return securityCodeResources.sameSecurityCode;
        }

        return '';
    },
    resetOriginalInputStatus: function () {
        $("#txtOldSecCode").empty();
        $("#txtSecCode").empty();
        $("#txtConfirmSecCode").empty();
    },
    gotoPasswordPage: function () {
        window.location = $("#mainRootPath").val() + 'Password/ChangePassword';
    },
    refreshPage: function () {
        window.location = $("#mainRootPath").val() + 'SecurityCode/ChangeUserOwnSecurityCode';
    }
}

$(function () {
    changeUserOwnSecurityCode.initialize();
});