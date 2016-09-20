$(document).ready(function () {
    changeUserOwnSecurityCode.initialize();
})
var changeUserOwnSecurityCode = {
    resources: {
        oldSecurityCodeEmpty: $('#alert-old-securitycode-empty').val(),
        securityCodeEmpty: $('#alert-securitycode-empty').val(),
        securityCodeWrongRule: $('#alert-securitycode-wrong-rule').val(),
        errorSecurityCodeConfirm: $('#alert-error-securitycode-confirm').val(),
        sameSecurityCode: $('#alert-same-securitycode').val(),
        securityCodeChangeSucessfully: $('#alert-securitycode-change-ok').val()
    },
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            changeUserOwnSecurityCode.change();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            changeUserOwnSecurityCode.resetOriginalInputStatus();
        });
        $('#txtOldSecCode').focus();
    },
    change: function () {
        securitycodeHelper.lockUI();
        var $messageContainer = $('#error-message');
        var $txtOldSecurityCode = $('#txtOldSecCode');
        var $txtSecurityCode = $('#txtSecCode');
        var $txtConfirmSecurityCode = $('#txtConfirmSecCode');
        var message = changeUserOwnSecurityCode.checkInputParameters($txtOldSecurityCode, $txtSecurityCode, $txtConfirmSecurityCode);
        if (message.length !== 0) {
            $messageContainer.empty().html(message);
            securitycodeHelper.unlockUI();
            return;
        }

        var hashKey = $('#hashkey').val();
        var oldSecCode = $txtOldSecurityCode.val();
        oldSecCode = securitycodeHelper.hashSecurityCode(oldSecCode);
        var secCode = $txtSecurityCode.val();
        secCode = secCode.ec(hashKey).hc();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        confirmSecCode = confirmSecCode.ec(hashKey).hc();
        $.ajax({
            type: 'POST',
            url: 'ChangeUserOwnSecurityCode',
            data: { oldSecurityCode: oldSecCode, securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorCode === 0) { // OK
                    setTimeout(changeUserOwnSecurityCode.gotoSecurityCodePage(), 5000);
                }
                else if (data.ErrorCode === 2) { // account is closed
                    alert(data.ErrorMessage);
                    top.age.SignOut();
                }
                else {
                    $messageContainer.empty().html(data.ErrorMessage);
                    $txtOldSecurityCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                securitycodeHelper.unlockUI();
            }
        })
    },
    checkInputParameters: function ($txtOldSecurityCode, $txtSecurityCode, $txtConfirmSecurityCode) {
        var oldSecCode = $txtOldSecurityCode.val();
        var secCode = $txtSecurityCode.val();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        if (oldSecCode.length === 0) {
            $txtOldSecurityCode.focus();
            return changeUserOwnSecurityCode.resources.oldSecurityCodeEmpty;
        }
        else if (secCode.length === 0) {
            $txtSecurityCode.focus();
            return changeUserOwnSecurityCode.resources.securityCodeEmpty;
        }
        else if (!securitycodeHelper.isValidSecurityCode(secCode)) {
            $txtSecurityCode.focus();
            return changeUserOwnSecurityCode.resources.securityCodeWrongRule;
        }
        else if (secCode != confirmSecCode) {
            $txtConfirmSecurityCode.focus();
            return changeUserOwnSecurityCode.resources.errorSecurityCodeConfirm;
        }
        else if (oldSecCode === secCode) {
            $txtSecurityCode.focus();
            return changeUserOwnSecurityCode.resources.sameSecurityCode;
        }

        return '';
    },
    resetOriginalInputStatus: function () {
        $("#txtOldSecCode").empty();
        $("#txtSecCode").empty();
        $("#txtConfirmSecCode").empty();
    },
    gotoPwdPage: function () {
        if (top.menu && top.menu.ClearCurrentLeftMenu) {
            top.menu.ClearCurrentLeftMenu();
        }

        var pwdLink = '_MemberInfo/ChangePassword/ChangePassword.aspx';
        window.location = securitycodeHelper.getBaseUrl() + pwdLink;
    },
    gotoSecurityCodePage: function () {
        //if (top.menu && top.menu.ClearCurrentLeftMenu) {
        //    top.menu.ClearCurrentLeftMenu();
        //}

        var scLink = 'SecurityCode/ChangeUserOwnSecurityCode';
        window.location = securitycodeHelper.getBaseUrl() + 'SecurityCode/ChangeUserOwnSecurityCode';
    }
}