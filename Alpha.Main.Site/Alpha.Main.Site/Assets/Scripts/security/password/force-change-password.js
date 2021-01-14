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
        $('.skip-force-change').click(function () {
            forceChangePassword.setSkipChangePasswordStorage("true");
            $('.force-change-password-container').addClass('hidden');
            $('.skip-force-change-container').removeClass('hidden');
        });
        $('#skip-text').keyup(function () {
            if ($('#skip-text').val().toUpperCase() === 'OK') {
                $('#btn-submit-skip').removeAttr("disabled");
            }
            else {
                $('#btn-submit-skip').attr('disabled', 'disabled');
            }
        });
        $('#btn-back-skip').click(function () {
            forceChangePassword.setSkipChangePasswordStorage("false");
            $('.force-change-password-container').removeClass('hidden');
            $('.skip-force-change-container').addClass('hidden');
        });
        $('#btn-submit-skip').click(function () {
            localStorage.removeItem('isSkipChangePassword');
            $('#SkipForceChangePasswordForm').submit();
        });
        forceChangePassword.setProfilePictureStorage();
        forceChangePassword.setSkipForceChangeContainerHeight();
        forceChangePassword.setSkipFormStatus();
    },
    setSkipForceChangeContainerHeight: function () {
        var containerHeight = $(".force-change-password-container").height();
        $(".skip-force-change-container").height(containerHeight);
    },
    setProfilePictureStorage: function () {
        var profilePicture = $('#logoProfileImg');
        localStorage.setItem('actorProfilePicture', JSON.stringify({
            ImageId: profilePicture.attr('profile-image-id'),
            Image: profilePicture.attr('src'),
            IsDefaultAvatar: profilePicture.attr('default-avatar')
        }));
    },
    setSkipChangePasswordStorage: function (isSkip) {
        localStorage.setItem('isSkipChangePassword', isSkip);
    },
    getSkipChangePasswordStorage: function () {
        return localStorage.getItem("isSkipChangePassword") === "true";
    },
    setSkipFormStatus: function () {
        if (forceChangePassword.getSkipChangePasswordStorage()) {
            $('.force-change-password-container').addClass('hidden');
            $('.skip-force-change-container').removeClass('hidden');
        }
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
        var oldPassword = $txtOldPassword.val().encrypt();
        var password = $txtPassword.val().encrypt();
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
                else if (data.Code === 101) {
                    $txtOldPassword.addClass('error').focus();
                    $("#txtOldPwd-error").empty().html(data.Message);
                }
                else {
                    $txtPassword.addClass('error').focus();
                    $("#txtNewPwd-error").empty().html(data.Message);
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        });
    },
    validateInputSecurityCode: function ($txtOldPassword, $txtPassword, $txtConfirmPassword) {
        forceChangePassword.resetOriginalInputStatus();
        var oldPassword = $txtOldPassword.val();
        var password = $txtPassword.val();
        var confirmPassword = $txtConfirmPassword.val();
        var oldPasswordError = $("#txtOldPwd-error");
        var newPasswordError = $("#txtNewPwd-error");
        var confirmPasswordError = $("#txtConfirmPwd-error");
        if (oldPassword.length === 0) {
            $txtOldPassword.addClass('error');
            oldPasswordError.html(passwordResources.pleaseEnterOldPassword);
            $txtOldPassword.focus();
            return false;
        }
        if (password.length === 0) {
            $txtPassword.addClass('error');
            newPasswordError.html(passwordResources.passwordCannotEmpty).append(passwordResources.rules);
            $txtPassword.focus();
            return false;
        }
        if (!securityHelper.isValidPassword(password)) {
            $txtPassword.focus();
            newPasswordError.html(passwordResources.passwordDoesNotMeetMinimunRequirement).append(passwordResources.rules);
            return false;
        }
        if (password != confirmPassword) {
            $txtConfirmPassword.addClass('error');
            confirmPasswordError.html(passwordResources.confirmPasswordNotCorret);
            $txtConfirmPassword.focus();
            return false;
        }
        if (oldPassword === password) {
            $txtConfirmPassword.addClass('error');
            confirmPasswordError.html(passwordResources.newPassMustbeDiffOldPass);
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
};
$(function () {
    forceChangePassword.initialize();
});
//# sourceMappingURL=force-change-password.js.map