/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />

'use strict'; 

var changePassword = {
    domElements: {
        form: null,
        securityCodeNav: null,
        txtOldPwd: null,
        txtNewPwd: null,
        txtConfirmPwd: null,
        showPasswordchk: null,
        container: null,
        hashKey: null,
        statusMessage: null,
        statusConatiner: null,
    },

    cachedDomElements: function () {
        changePassword.domElements.form = $(".form-SC");
        changePassword.domElements.securityCodeNav = $("#securityCodeNav");
        changePassword.domElements.txtOldPwd = $("#txtOldPwd");
        changePassword.domElements.txtNewPwd = $("#txtNewPwd");
        changePassword.domElements.txtConfirmPwd = $("#txtConfirmPwd");
        changePassword.domElements.showPasswordchk = $("#showPasswordchk");
        changePassword.domElements.container = $("#container");
        changePassword.domElements.hashKey = $("#hashkey");
        changePassword.domElements.statusMessage = $("#status-message");
        changePassword.domElements.statusConatiner = $("#status-message-content");
    },

    init: function () {
        changePassword.cachedDomElements();
        changePassword.registerEvents();

        if (top.hasPopupOpen()) {
            changePassword.domElements.container.addClass("has-popup");
        }
    },

    registerEvents: function () {
        changePassword.domElements.form
            .submit(function (e) {
                e.preventDefault();
                changePassword.change();
            }).bind("reset", function (e) {
                changePassword.resetOriginalInputStatus();
            });

        changePassword.domElements.securityCodeNav.click(function () {
            changePassword.gotoSecurityCodePage();
        });

        changePassword.domElements.txtOldPwd
            .val("")
            .focus();

        changePassword.domElements.txtNewPwd
            .val("")
            .keyup(function () {
                securityHelper.validatePassword(this);
            });

        changePassword.domElements.txtConfirmPwd
            .val("")
            .keyup(function () {
                securityHelper.validatePassword(this);
            });

        changePassword.domElements.showPasswordchk
            .click(function () {
                var isChecked = $(this).is(":checked");
                isChecked ? securityHelper.showHint() : securityHelper.hideHint();
            });
    },

    change: function () {
        var $txtOldPassword = changePassword.domElements.txtOldPwd;
        var $txtPassword = changePassword.domElements.txtNewPwd;
        var $txtConfirmPassword = changePassword.domElements.txtConfirmPwd;
        var $messageContainer = changePassword.domElements.statusConatiner;

        changePassword.domElements.statusMessage.removeClass("display-none");

        var errorMessage = changePassword.checkInputParameters($txtOldPassword, $txtPassword, $txtConfirmPassword);
        if (errorMessage.length > 0) {
            securityHelper.showFailMessage($messageContainer, errorMessage);
            return;
        }

        var hashKey = changePassword.domElements.hashKey.val();
        var oldPassword = $txtOldPassword.val().ec(hashKey).hc();
        var password = $txtPassword.val().ec(hashKey).hc();
        var confirmPassword = $txtConfirmPassword.val().ec(hashKey).hc();

        $.blockUI();
        $.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath +  "Password/ChangePassword",
            data: { currentPassword: oldPassword, newPassword: password },
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                if (data.Code === 0) { // OK
                    securityHelper.showSuccessfullyMessage($messageContainer, passwordResources.changePasswordSuccessfully);
                    setTimeout(changePassword.refreshPage, 3000);

                    return;
                }

                if (data.Code === 2) { // account is closed
                    alert(data.Message);
                    securityHelper.signOut();

                    return;
                }

                securityHelper.showFailMessage($messageContainer, data.Message);
                $txtOldPassword.focus();
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        })
    },

    checkInputParameters: function ($txtOldPassword, $txtPassword, $txtConfirmPassword) {
        var oldPassword = $txtOldPassword.val();
        if (oldPassword.length === 0) {
            $txtOldPassword.focus();
            return passwordResources.pleaseEnterOldPassword;
        }

        var password = $txtPassword.val();
        if (password.length === 0) {
            $txtPassword.focus();
            return passwordResources.passwordCannotEmpty;
        }

        if (!securityHelper.isValidPassword(password)) {
            $txtPassword.focus();
            return passwordResources.passwordDoesNotMeetMinimunRequirement;
        }

        var confirmPassword = $txtConfirmPassword.val();
        if (password != confirmPassword) {
            $txtConfirmPassword.focus();
            return passwordResources.confirmPasswordNotCorret;
        }

        if (oldPassword === password) {
            $txtPassword.focus();
            return passwordResources.newPassMustbeDiffOldPass;
        }

        return "";
    },

    resetOriginalInputStatus: function () {
        changePassword.domElements.txtOldPwd.val("");
        changePassword.domElements.txtNewPwd.val("");
        changePassword.domElements.txtConfirmPwd.val("");
    },

    gotoSecurityCodePage: function () {
        window.location.href = alpha.urls.mainRootPath + "SecurityCode/ChangeUserOwnSecurityCode";
    },

    refreshPage: function () {
        window.location.href = alpha.urls.mainRootPath + "Password/ChangePassword";
    }
}

$(function () {
    changePassword.init();
});