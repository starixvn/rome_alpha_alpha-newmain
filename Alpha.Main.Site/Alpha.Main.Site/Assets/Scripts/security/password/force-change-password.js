'use strict';

var forceChangePassword = {
    initialize: function () {
        securityHelper.initLanguage();
        $('#pwdForm').submit(function (e) {
            forceChangePassword.changeSecurityCode();
            e.preventDefault();
        });

        $("#btn-back").bind("click", function () {
            securityHelper.signOut();
        });

        $('#txtOldPwd').focus();
    },
    changeSecurityCode: function () {
        $.blockUI();
        var $txtOldPassword = $('#txtOldPwd');
        var $txtPassword = $('#txtNewPwd');
        var $txtConfirmPassword = $('#txtConfirmPwd');
        if (!forceChangePassword.validateInputSecurityCode($txtOldPassword, $txtPassword, $txtConfirmPassword)) {
            $.unblockUI();
            return;
        }

        var oldPassword = $txtOldPassword.val();
        oldPassword = securityHelper.hashSecurityCode(oldPassword);
        var password = $txtPassword.val();
        var hashKey = $('#hashkey').val();
        password = password.ec(hashKey).hc();
        var confirmPassword = $txtConfirmPassword.val();
        confirmPassword = confirmPassword.ec(hashKey).hc();
        $.ajax({
            type: 'POST',
            url: 'ForceChangePassword',
            data: { currentPassword: oldPassword, newPassword: password},
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
                    $("#txtOldPwd").addClass('error');
                    $("#txtOldPwd-error").empty().html(data.ErrorMessage);
                    $txtOldPassword.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        })
    },
    validateInputSecurityCode: function ($txtOldPassword, $txtPassword, $txtConfirmPassword) {
        forceChangePassword.resetOriginalInputStatus();
        var oldPassword = $txtOldPassword.val();
        var password = $txtPassword.val();
        var confirmPassword = $txtConfirmPassword.val();
        if (oldPassword.length === 0) {
            $("#txtOldPwd").addClass('error');
            $("#txtOldPwd-error").empty().html(passwordResources.pleaseEnterOldPassword);
            $txtOldPassword.focus();
            return false;
        }
        else if (password.length === 0) {
            $("#txtNewPwd").addClass('error');
            $("#txtNewPwd-error").empty().html(passwordResources.passwordCannotEmpty).append(passwordResources.rules);
            $txtPassword.focus();
            return false;
        }
        else if (password != confirmPassword) {
            $("#txtConfirmPwd").addClass('error');
            $("#txtConfirmPwd-error").empty().html(passwordResources.confirmPasswordNotCorret);
            $txtConfirmPassword.focus();
            return false;
        }
        else if (oldPassword === password) {
            $("#txtConfirmPwd").addClass('error');
            $("#txtConfirmPwd-error").empty().html(passwordResources.newPassMustbeDiffOldPass);
            $txtPassword.focus();
            return false;
        }

        return true;
    },
    resetOriginalInputStatus: function () {
        $("#txtOldPwd").removeClass('error');
        $("#txtOldPwd-error").empty();
        $("#txtNewPwd").removeClass('error');
        $("#txtNewPwd-error").empty();
        $("#txtConfirmPwd").removeClass('error');
        $("#txtConfirmPwd-error").empty();
    }
}

$(function () {
    forceChangePassword.initialize();
});