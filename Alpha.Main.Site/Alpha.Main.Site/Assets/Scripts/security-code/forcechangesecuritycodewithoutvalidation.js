$(document).ready(function () {
    forcechangesecuritycodewithoutvalidation.initialize();
})
var forcechangesecuritycodewithoutvalidation = {
    initialize: function () {
        var $form = $('#frmForceChangeSecurityCodeWithoutValidation');
        $form.submit(function (e) {
            forcechangesecuritycodewithoutvalidation.changeSecurityCode();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            $('#spnNewPwdIcon').removeClass();
            $('#spnConfirmSecCodeIcon').removeClass();
        });
        $('#txtSecCode').focus();
    },
    changeSecurityCode: function () {
        lockUI();
        var $messageContainer = $('#errorSection');
        var $txtSecCode = $('#txtSecCode');
        var $txtConfirmSecCode = $('#txtConfirmSecCode');
        var message = forcechangesecuritycodewithoutvalidation.checkInputParameters($txtSecCode, $txtConfirmSecCode);
        if (message.length !== 0) {
            showMessage($messageContainer, message, 1);
            unlockUI();
            return;
        }

        var ph = parent.top.ph;
        var secCode = $txtSecCode.val();
        secCode = secCode.ec(ph).hc();
        var confirmSecCode = $txtConfirmSecCode.val();
        confirmSecCode = confirmSecCode.ec(ph).hc();
        $.ajax({
            type: 'POST',
            url: 'ForceChangeSecurityCodeWithoutValidation',
            data: { securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                showMessage($messageContainer, data.ErrorMessage, data.ErrorCode);
                if (data.ErrorCode === 0) {
                    redirectToHomePage(2000);
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
    checkInputParameters: function ($txtSecCode, $txtConfirmSecCode) {
        var message = '';
        var secCode = $txtSecCode.val();
        var confirmSecCode = $txtConfirmSecCode.val();
        if (secCode.length === 0) {
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

        return message;
    }
}