$(document).ready(function () {
    changeUserOwnSecurityCode.initialize();
})
var changeUserOwnSecurityCode = {
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            changeUserOwnSecurityCode.change();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            $('#spnNewPwdIcon').removeClass();
            $('#spnConfirmSecCodeIcon').removeClass();
        });
        $('#txtOldSecCode').focus();
    },
    change: function () {
        lockUI();
        var $messageContainer = $('#errorSection');
        var $txtOldSecCode = $('#oldSC');
        var $txtSecCode = $('#SC');
        var $txtConfirmSecCode = $('#confirmSC');
        var message = changeUserOwnSecurityCode.checkInputParameters($txtOldSecCode, $txtSecCode, $txtConfirmSecCode);
        if (message.length !== 0) {
            $('#error-wrap').removeClass('display-none');
            showMessage($messageContainer, message, 1);
            unlockUI();
            return;
        }

        var ph = parent.top.ph;
        var oldSecCode = $txtOldSecCode.val();
        oldSecCode = hashSecurityCode(oldSecCode);
        var secCode = $txtSecCode.val();
        secCode = secCode.ec(ph).hc();
        var confirmSecCode = $txtConfirmSecCode.val();
        confirmSecCode = confirmSecCode.ec(ph).hc();
        $.ajax({
            type: 'POST',
            url: 'ChangeUserOwnSecurityCode',
            data: { oldSecurityCode: oldSecCode, securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                $('#error-wrap').removeClass('display-none');
                showMessage($messageContainer, data.ErrorMessage, data.ErrorCode);
                var errorCode = data.ErrorCode;
                if (errorCode === 0) { // OK
                    setTimeout(changesecuritycode.gotoSecurityCodePage, 5000);
                }
                else if (errorCode === 2) { // account is closed
                    alert(data.ErrorMessage);
                    top.age.SignOut();
                }
                else {
                    $txtSecCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                unlockUI();
            }
        })
    },
    checkInputParameters: function ($txtOldSecCode, $txtSecCode, $txtConfirmSecCode) {
        var message = '';
        var oldSecCode = $txtOldSecCode.val();
        var secCode = $txtSecCode.val();
        var confirmSecCode = $txtConfirmSecCode.val();
        if (oldSecCode.length === 0) {
            message = securityCodeResource.alertOldSecCodeEmpty;
            $txtOldSecCode.focus();
        }
        else if (secCode.length === 0) {
            message = securityCodeResource.alertSecCodeEmpty;
            $txtSecCode.focus();
        }
        else if (!isValidSecurityCode(secCode)) {
            message = securityCodeResource.alertSecCodeWrongRule;
            $txtSecCode.focus();
        }
        else if (secCode != confirmSecCode) {
            message = securityCodeResource.alertErrorSecCodeConfirm;
            $txtConfirmSecCode.focus();
        }
        else if (oldSecCode === secCode) {
            message = securityCodeResource.alertSameSecCode;
            $txtSecCode.focus();
        }

        return message;
    },
    gotoPwdPage: function () {
        if (top.menu && top.menu.ClearCurrentLeftMenu) {
            top.menu.ClearCurrentLeftMenu();
        }

        var pwdLink = '_MemberInfo/ChangePassword/ChangePassword.aspx';
        window.location = getBaseUrl() + pwdLink;
    },
    gotoSecurityCodePage: function () {
        if (top.menu && top.menu.ClearCurrentLeftMenu) {
            top.menu.ClearCurrentLeftMenu();
        }

        var scLink = 'Security/SecurityCode/ChangeSecurityCode';
        window.location = getBaseUrl() + scLink;
    }
}