$(document).ready(function () {
    forcechangesecuritycode.initialize();
})
var forcechangesecuritycode = {
    initialize: function () {
        var $form = $('#frmForceChangeSecurityCode');
        $form.submit(function (e) {
            forcechangesecuritycode.changeSecurityCode();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            $('#spnNewPwdIcon').removeClass();
            $('#spnConfirmSecCodeIcon').removeClass();
        });
        $('#txtOldSecCode').focus();
    },
    changeSecurityCode: function () {
        lockUI();
        var $messageContainer = $('#errorSection');
        var $txtOldSecCode = $('#txtOldSecCode');
        var $txtSecCode = $('#txtSecCode');
        var $txtConfirmSecCode = $('#txtConfirmSecCode');
        var message = forcechangesecuritycode.checkInputParameters($txtOldSecCode, $txtSecCode, $txtConfirmSecCode);
        if (message.length !== 0) {
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
            url: 'ForceChangeSecurityCodeByUpline',
            data: { oldSecurityCode: oldSecCode, securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                var errorCode = data.ErrorCode;
                showMessage($messageContainer, data.ErrorMessage, data.ErrorCode);
                if (errorCode === 0) { // OK
                    redirectToHomePage(2000);
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
    }
}