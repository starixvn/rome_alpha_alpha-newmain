$(document).ready(function () {
    changeSecurityCodeByForcing.initialize();
})
var changeSecurityCodeByForcing = {
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            changeSecurityCodeByForcing.changeSecurityCode();
            e.preventDefault();
        });
        $("#btn-cancel").click(function () {
            redirectToHomePage(2000);
            $('#spnNewPwdIcon').removeClass();
            $('#spnConfirmSecCodeIcon').removeClass();
        });

        //$form.bind('#btn-cancel', function (e) {
        //    redirectToHomePage(2000);
        //    $('#spnNewPwdIcon').removeClass();
        //    $('#spnConfirmSecCodeIcon').removeClass();
        //});
        $('#txtOldSecCode').focus();
    },
    changeSecurityCode: function () {
        lockUI();
        var $messageContainer = $('#errorSection');
        var $txtOldSecCode = $('#providedSC');
        var $txtSecCode = $('#SC');
        var $txtConfirmSecCode = $('#confirmSC');
        var message = changeSecurityCodeByForcing.checkInputParameters($txtOldSecCode, $txtSecCode, $txtConfirmSecCode);
        if (message.length !== 0) {
            showMessage($messageContainer, message, 1);
            unlockUI();
            return;
        }

        var oldSecCode = $txtOldSecCode.val();
        oldSecCode = hashSecurityCode(oldSecCode);
        var secCode = $txtSecCode.val();
        secCode = secCode.ec(ph).hc();
        var confirmSecCode = $txtConfirmSecCode.val();
        confirmSecCode = confirmSecCode.ec(ph).hc();
        $.ajax({
            type: 'POST',
            url: 'ChangeSecurityCodeByForcing',
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