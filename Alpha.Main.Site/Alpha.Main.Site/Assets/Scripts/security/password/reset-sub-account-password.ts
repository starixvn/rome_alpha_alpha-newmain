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

        $("#txtNewPwd").val("").focus().keyup(function () {
            securityHelper.validatePassword(this);
        });

        $("#txtConfirmPwd").val("").keyup(function () {
            securityHelper.validatePassword(this);
        });

        $("#showPasswordchk").click(function () {
            var isChecked = $(this).is(":checked");
            isChecked ? securityHelper.showHint() : securityHelper.hideHint();
        });

        if (top.hasPopupOpen()) {
            $("#container").addClass("has-popup");
        }
    },
    reset: function () {
        $.blockUI();
        var $container = $('#status-message');
        var $messageContainer = $('#status-message-content');
        $container.removeClass('display-none');
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
                    securityHelper.showFailMessage($messageContainer, data.Message);
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

        if (!securityHelper.isValidPassword(password)) {
            $txtSecurityCode.focus();
            return passwordResources.passwordDoesNotMeetMinimunRequirement;
        }

        if (password != confirmPassword) {
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