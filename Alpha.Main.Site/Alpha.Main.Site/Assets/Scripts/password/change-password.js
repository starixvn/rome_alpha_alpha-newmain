$(document).ready(function () {
    changeUserOwnPassword.initialize();
})
var changeUserOwnPassword = {
    resources: {
        pleaseEnterOldPassword: $('#please-enter-oldPassword').val(),
        passwordCannotEmpty: $('#password-cannot-empty').val(),
        confirmPasswordNotCorret: $('#not-correct-confirm-password').val(),
        changePasswordSuccessfully: $('#password-change-successfully').val(),
        newPassMustbeDiffOldPass: $('#newPassword-must-be-diff-old').val()
    },
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            changeUserOwnPassword.change();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            changeUserOwnPassword.resetOriginalInputStatus();
        });
        $('#txtOldPwd').focus();
        $('#securityCodeNav').click(function () {
            changeUserOwnPassword.gotoSecurityCodePage()
        });
    },
    change: function () {
        $.blockUI();
        var $messageContainer = $('#error-message');
        $messageContainer.removeClass('display-none');
        var $txtOldPassword = $('#txtOldPwd');
        var $txtPassword = $('#txtNewPwd');
        var $txtConfirmPassword = $('#txtConfirmPwd');
        var message = changeUserOwnPassword.checkInputParameters($txtOldPassword, $txtPassword, $txtConfirmPassword);
        if (message.length !== 0) {
            changeUserOwnPassword.showFailMessage($messageContainer, message);
            $.unblockUI();
            return;
        }

        var hashKey = $('#hashkey').val();
        var oldPassword = $txtOldPassword.val();
        oldPassword = securitycodeHelper.hashSecurityCode(oldPassword);
        var password = $txtPassword.val();
        password = password.ec(hashKey).hc();
        var confirmPassword = $txtConfirmPassword.val();
        confirmPassword = confirmPassword.ec(hashKey).hc();
        $.ajax({
            type: 'POST',
            url: 'ChangePassword',
            data: { currentPassword: oldPassword, newPassword: password },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorCode === 0) { // OK
                    changeUserOwnPassword.showSuccessfullyMessage($messageContainer);
                    setTimeout(changeUserOwnPassword.refreshPage, 5000);
                }
                else if (data.ErrorCode === 2) { // account is closed
                    alert(data.ErrorMessage);
                    securitycodeHelper.signOut();
                }
                else {
                    changeUserOwnPassword.showFailMessage($messageContainer, data.ErrorMessage);
                    $txtOldPassword.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        })
    },
    checkInputParameters: function ($txtOldPassword, $txtPassword, $txtConfirmPassword) {
        var oldPassword = $txtOldPassword.val();
        var password = $txtPassword.val();
        var confirmPassword = $txtConfirmPassword.val();
        if (oldPassword.length === 0) {
            $txtOldPassword.focus();
            return changeUserOwnPassword.resources.pleaseEnterOldPassword;
        }
        else if (password.length === 0) {
            $txtPassword.focus();
            return changeUserOwnPassword.resources.passwordCannotEmpty;
        }
        else if (password != confirmPassword) {
            $txtConfirmPassword.focus();
            return changeUserOwnPassword.resources.confirmPasswordNotCorret;
        }
        else if (oldPassword === password) {
            $txtPassword.focus();
            return changeUserOwnPassword.resources.newPassMustbeDiffOldPass;
        }

        return '';
    },
    resetOriginalInputStatus: function () {
        $("#txtOldPwd").empty();
        $("#txtNewPwd").empty();
        $("#txtConfirmPwd").empty();
    },
    showSuccessfullyMessage: function ($messageContainer) {
        $messageContainer.removeClass('msg-error');
        $messageContainer.addClass('msg-success').empty().html(changeUserOwnPassword.resources.changePasswordSuccessfully);
    },
    showFailMessage: function ($messageContainer, message) {
        $messageContainer.removeClass('msg-success');
        $messageContainer.addClass('msg-error').empty().html(message);
    },
    gotoSecurityCodePage: function () {
        window.location = $("#mainRootPath").val() + 'SecurityCode/changeUserOwnPassword';
    },
    refreshPage: function () {
        window.location = $("#mainRootPath").val() + 'Password/ChangePassword';
    }
}