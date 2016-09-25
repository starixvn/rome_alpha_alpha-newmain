'use strict';

var resetSubAccountPassword = {
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            resetSubAccountPassword.reset();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            resetSubAccountPassword.resetInputValue();
        });
        $('#txtNewPwd').focus();
    },
    reset: function () {
        $.blockUI();
        var $messageContainer = $('#error-message');
        $messageContainer.removeClass('display-none');
        var $txtSecurityCode = $('#txtNewPwd');
        var $txtConfirmSecurityCode = $('#txtConfirmPwd');
        var message = resetSubAccountPassword.checkInputParameters($txtSecurityCode, $txtConfirmSecurityCode);
        if (message.length !== 0) {
            securityHelper.showFailMessage($messageContainer, message);
            $.unblockUI();
            return;
        }

        var hashKey = $('#hashkey').val();
        var password = $txtSecurityCode.val();
        password = password.ec(hashKey).hc();
        var confirmPassword = $txtConfirmSecurityCode.val();
        confirmPassword = confirmPassword.ec(hashKey).hc();
        $.ajax({
            type: 'POST',
            url: 'ResetSubAccountPassword',
            data: { subAccountId: $('#subAccountId').val(), newPassword: password },
            success: function (data, textStatus, jqXHR) {
                if (data.Code !== 0) {
                    securityHelper.showFailMessage($messageContainer, data.Messsage);
                    $txtSecurityCode.focus();
                }
                else {
                    securityHelper.showSuccessfullyMessage($messageContainer, passwordResources.changePasswordSuccessfully);
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        })
    },
    checkInputParameters: function ($txtSecurityCode, $txtConfirmSecurityCode) {
        var password = $txtSecurityCode.val();
        var confirmPassword = $txtConfirmSecurityCode.val();
        if (password.length === 0) {
            $txtSecurityCode.focus();
            return passwordResources.passwordCannotEmpty;

        }
        else if (password != confirmPassword) {
            $txtConfirmSecurityCode.focus();
            return passwordResources.confirmPasswordNotCorret;
        }

        return '';
    },
    resetInputValue: function () {
        $("#txtNewPwd").empty();
        $("#txtConfirmPwd").empty();
    }
}

$(function () {
    resetSubAccountPassword.initialize();
});