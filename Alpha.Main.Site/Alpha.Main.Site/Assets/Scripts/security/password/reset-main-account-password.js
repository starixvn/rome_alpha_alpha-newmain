'use strict';

var resetMainAccountPassword = {
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
        var isCasinoSync = $('#isCasinoSync').val() === 'True';
        var message = resetMainAccountPassword.checkInputParameters($txtSecurityCode, $txtConfirmSecurityCode);
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
            url: 'ResetMainAccountPassword',
            data: { custId: $('#custId').val(), custRole: $('#custRole').val(), newPassword: password, isCasinoSync: isCasinoSync },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.Code !== 0) {
                    securityHelper.showFailMessage($messageContainer, data.Messsage);
                    $txtSecurityCode.focus();
                }
                else
                {
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
    resetInputValue: function()
    {
        $("#password").empty();
        $("#confirmPassword").empty();
    }
}

$(function () {
    resetMainAccountPassword.initialize();
});