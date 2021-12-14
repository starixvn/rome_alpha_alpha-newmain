'use strict';

var resetMainAccountPassword = {
    isChangeMemberPassword: false,
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            resetMainAccountPassword.reset();
            e.preventDefault();
        });

        $form.bind('reset', function (e) {
            resetMainAccountPassword.resetInputValue();
        });

        $("#password").val("").focus().keyup(function () {
            securityHelper.validatePassword(this, resetMainAccountPassword.isChangeMemberPassword);
        });

        $("#confirmPassword").val("").keyup(function () {
            securityHelper.validatePassword(this, resetMainAccountPassword.isChangeMemberPassword);
        });

        $("#showPasswordchk").click(function () {
            var isChecked = $(this).is(":checked");
            isChecked ? securityHelper.showHint() : securityHelper.hideHint();
        });

        if (top.hasPopupOpen()) {
            $("#container").addClass("has-popup");
        }

        resetMainAccountPassword.isChangeMemberPassword = $("#isChangeMemberPassword").val() === "True";
    },
    reset: function () {
        $.blockUI();
        var $container = $('#status-message');
        var $messageContainer = $('#status-message-content');
        $container.removeClass('display-none');
        var $txtSecurityCode = $('#password');
        var $txtConfirmSecurityCode = $('#confirmPassword');
        var message = resetMainAccountPassword.checkInputParameters($txtSecurityCode, $txtConfirmSecurityCode);
        if (message.length !== 0) {
            securityHelper.showFailMessage($messageContainer, message);
            $.unblockUI();
            return;
        }

        var hashKey = $('#hashkey').val();
        var password = $txtSecurityCode.val();
        password = password.encrypt();
        var confirmPassword = $txtConfirmSecurityCode.val();
        confirmPassword = confirmPassword.encrypt();
        $.ajax({
            type: 'POST',
            url: 'ResetMainAccountPassword',
            data: { custId: $('#custId').val(), custRoleId: $('#custRoleId').val(), newPassword: password},
            dataType: 'json',
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

        if (!securityHelper.isValidPassword(password, resetMainAccountPassword.isChangeMemberPassword)) {
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
        securityHelper.hideHint();
        securityHelper.removeValidationIcon($("#password"));
        securityHelper.removeValidationIcon($("#confirmPassword"));
    }
}

$(function () {
    resetMainAccountPassword.initialize();
});