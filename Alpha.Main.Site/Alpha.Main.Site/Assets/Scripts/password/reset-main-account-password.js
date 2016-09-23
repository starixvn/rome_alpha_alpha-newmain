var resetMainAccountPassword = {
    resources: {
        passwordCannotEmpty: $('#password-cannot-empty').val(),
        changePasswordSuccessfully: $('#password-change-successfully').val(),
        confirmPasswordNotCorret: $('#not-correct-confirm-password').val(),
    },
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            resetMainAccountPassword.reset();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            resetMainAccountPassword.resetInputValue();
        });
        $('#password').focus();
    },
    reset: function () {
        $.blockUI();
        var $messageContainer = $('#error-message');
        $messageContainer.removeClass('display-none');
        var $txtSecurityCode = $('#password');
        var $txtConfirmSecurityCode = $('#confirmPassword');
        var message = resetMainAccountPassword.checkInputParameters($txtSecurityCode, $txtConfirmSecurityCode);
        if (message.length !== 0) {
            resetMainAccountPassword.showFailMessage($messageContainer, message);
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
            url: 'ResetMainAccountPassword',
            data: { custId: $('#custid').val(), newPassword: password},
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorCode !== 0) {
                    resetMainAccountPassword.showFailMessage($messageContainer, data.ErrorMessage);
                    $txtSecurityCode.focus();
                }
                else
                {
                    resetMainAccountPassword.showSuccessfullyMessage($messageContainer);
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
            return resetMainAccountPassword.resources.passwordCannotEmpty;
        }
        else if (password != confirmPassword) {
            $txtConfirmSecurityCode.focus();
            return resetMainAccountPassword.resources.confirmPasswordNotCorret;
        }

        return '';
    },
    resetInputValue: function()
    {
        $("#password").empty();
        $("#confirmPassword").empty();
    },
    showSuccessfullyMessage: function ($messageContainer)
    {
        $messageContainer.removeClass('msg-error');
        $messageContainer.addClass('msg-success').empty().html(resetMainAccountPassword.resources.securityCodeChangeSucessfully);
    },
    showFailMessage: function ($messageContainer, message)
    {
        $messageContainer.removeClass('msg-success');
        $messageContainer.addClass('msg-error').empty().html(message);
    }
}

$(function () {
    resetMainAccountPassword.initialize();
});