'use strict'; 

var forceChangePassword = {
    initialize: function () {
        securityHelper.initLanguage();
        $('#pwdForm').submit(function (e) {
            e.preventDefault();
            forceChangePassword.changePassword();
        });

        $("#btn-back").bind("click", function () {
            securityHelper.signOut();
        });

        $('#txtOldPwd').val("").focus();

        $("#txtNewPwd").val("").keyup(function () {
            securityHelper.validatePassword(this);
        });

        $("#txtConfirmPwd").val("").keyup(function () {
            securityHelper.validatePassword(this);
        });

        $("#showPasswordchk").click(function () {
            var isChecked = $(this).is(":checked");
            isChecked ? securityHelper.showHint() : securityHelper.hideHint();
        });
    },
    changePassword: function () {
        $.blockUI();
        var $txtOldPassword = $('#txtOldPwd');
        var $txtPassword = $('#txtNewPwd');
        var $txtConfirmPassword = $('#txtConfirmPwd');
        if (!forceChangePassword.validateInputSecurityCode($txtOldPassword, $txtPassword, $txtConfirmPassword)) {
            $.unblockUI();
            return;
        }

        var hashKey = $('#hashkey').val();
        var oldPassword = $txtOldPassword.val().ec(hashKey).hc();
        var password = $txtPassword.val().ec(hashKey).hc();
        $.ajax({
            type: 'POST',
            url: 'ForceChangePassword',
            data: { currentPassword: oldPassword, newPassword: password },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.Code === 0) { // OK
                    securityHelper.redirectToHomePage(2000);
                }
                else if (data.Code === 2) { // account is closed
                    alert(data.Message);
                    securityHelper.signOut();
                }
                else {
                    $txtPassword.addClass('error').focus();
                    $("#txtNewPwd-error").empty().html(data.Message);
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

        if (password.length === 0) {
            $("#txtNewPwd").addClass('error');
            $("#txtNewPwd-error").empty().html(passwordResources.passwordCannotEmpty).append(passwordResources.rules);
            $txtPassword.focus();
            return false;
        }

        if (!securityHelper.isValidPassword(password)) {
            $txtPassword.focus();
            $("#txtNewPwd-error").empty().html(passwordResources.passwordDoesNotMeetMinimunRequirement).append(passwordResources.rules);
            return false;
        }

        if (password != confirmPassword) {
            $("#txtConfirmPwd").addClass('error');
            $("#txtConfirmPwd-error").empty().html(passwordResources.confirmPasswordNotCorret);
            $txtConfirmPassword.focus();
            return false;
        }

        if (oldPassword === password) {
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