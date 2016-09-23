var resetSubAccountPassword = {
    $messageContainer: $('#error-message'),
    $txtNewPassword: $('#txtNewPwd'),
    $txtConfirmNewPassword: $('#txtConfirmPwd'),
    resources: {
        passwordCannotEmpty: $('#password-cannot-empty').val(),
        changePasswordSuccessfully: $('#password-change-successfully').val(),
        confirmPasswordNotCorret: $('#not-correct-confirm-password').val(),
    },
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            resetSubAccountPassword.reset();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            resetSubAccountPassword.resetInputValue();
        });
        $txtNewPassword.focus();
    },
    reset: function () {
        $.blockUI();
        var $messageContainer = $('#error-message');
        $messageContainer.removeClass('display-none');
        var $txtSecurityCode = $('#txtNewPwd');
        var $txtConfirmSecurityCode = $('#txtConfirmPwd');
        var message = resetSubAccountPassword.checkInputParameters($txtSecurityCode, $txtConfirmSecurityCode);
        if (message.length !== 0) {
            resetSubAccountPassword.showFailMessage($messageContainer, message);
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
            data: { subAccId: $('#subaccid').val(), newPassword: password },
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorCode !== 0) {
                    resetSubAccountPassword.showFailMessage($messageContainer, data.ErrorMessage);
                    $txtSecurityCode.focus();
                }
                else {
                    resetSubAccountPassword.showSuccessfullyMessage($messageContainer);
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
            return resetSubAccountPassword.resources.passwordCannotEmpty;

        }
        else if (password != confirmPassword) {
            $txtConfirmSecurityCode.focus();
            return resetSubAccountPassword.resources.confirmPasswordNotCorret;
        }

        return '';
    },
    resetInputValue: function () {
        $("#txtNewPwd").empty();
        $("#txtConfirmPwd").empty();
    },
    showSuccessfullyMessage: function ($messageContainer) {
        $messageContainer.removeClass('msg-error');
        $messageContainer.addClass('msg-success').empty().html(resetSubAccountPassword.resources.securityCodeChangeSucessfully);
    },
    showFailMessage: function ($messageContainer, message) {
        $messageContainer.removeClass('msg-success');
        $messageContainer.addClass('msg-error').empty().html(message);
    }
}

$(function () {
    resetSubAccountPassword.initialize();
});