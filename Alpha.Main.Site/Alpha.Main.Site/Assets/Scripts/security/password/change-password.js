'use strict';
var changeUserOwnPassword = {
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            e.preventDefault();
            changeUserOwnPassword.change();
        });
        $form.bind('reset', function (e) {
            changeUserOwnPassword.resetOriginalInputStatus();
        });
        $('#securityCodeNav').click(function () {
            changeUserOwnPassword.gotoSecurityCodePage();
        });
        $('#txtOldPwd').focus();
        $("#txtNewPwd").keyup(function () {
            securityHelper.validatePassword(this);
        });
        $("#txtConfirmPwd").keyup(function () {
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
    change: function () {
        $.blockUI();
        var $container = $('#status-message');
        var $messageContainer = $('#status-message-content');
        $container.removeClass('display-none');
        var $txtOldPassword = $('#txtOldPwd');
        var $txtPassword = $('#txtNewPwd');
        var $txtConfirmPassword = $('#txtConfirmPwd');
        var message = changeUserOwnPassword.checkInputParameters($txtOldPassword, $txtPassword, $txtConfirmPassword);
        if (message.length !== 0) {
            securityHelper.showFailMessage($messageContainer, message);
            $.unblockUI();
            return;
        }
        var hashKey = $('#hashkey').val();
        var oldPassword = $txtOldPassword.val();
        oldPassword = oldPassword.ec(hashKey).hc();
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
                if (data.Code === 0) {
                    securityHelper.showSuccessfullyMessage($messageContainer, passwordResources.changePasswordSuccessfully);
                    setTimeout(changeUserOwnPassword.refreshPage, 5000);
                }
                else if (data.Code === 2) {
                    alert(data.Messsage);
                    securityHelper.signOut();
                }
                else {
                    securityHelper.showFailMessage($messageContainer, data.Messsage);
                    $txtOldPassword.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        });
    },
    checkInputParameters: function ($txtOldPassword, $txtPassword, $txtConfirmPassword) {
        var oldPassword = $txtOldPassword.val();
        var password = $txtPassword.val();
        var confirmPassword = $txtConfirmPassword.val();
        if (oldPassword.length === 0) {
            $txtOldPassword.focus();
            return passwordResources.pleaseEnterOldPassword;
        }
        else if (password.length === 0) {
            $txtPassword.focus();
            return passwordResources.passwordCannotEmpty;
        }
        else if (password != confirmPassword) {
            $txtConfirmPassword.focus();
            return passwordResources.confirmPasswordNotCorret;
        }
        else if (oldPassword === password) {
            $txtPassword.focus();
            return passwordResources.newPassMustbeDiffOldPass;
        }
        return '';
    },
    resetOriginalInputStatus: function () {
        $("#txtOldPwd").empty();
        $("#txtNewPwd").empty();
        $("#txtConfirmPwd").empty();
    },
    gotoSecurityCodePage: function () {
        window.location.href = $("#mainRootPath").val() + 'SecurityCode/ChangeUserOwnSecurityCode';
    },
    refreshPage: function () {
        window.location.href = $("#mainRootPath").val() + 'Password/ChangePassword';
    }
};
$(function () {
    changeUserOwnPassword.initialize();
});
