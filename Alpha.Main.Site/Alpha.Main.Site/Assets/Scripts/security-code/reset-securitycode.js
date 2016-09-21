$(document).ready(function () {
    resetsecuritycode.initialize();
})
var resetsecuritycode = {
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
            resetsecuritycode.reset();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            resetsecuritycode.resetInputValue();
        });
        $('#txtSecCode').focus();
    },
    reset: function () {
        securitycodeHelper.lockUI();
        var $messageContainer = $('#error-message');
        $messageContainer.removeClass('display-none');
        var $txtSecurityCode = $('#txtSecCode');
        var $txtConfirmSecurityCode = $('#txtConfirmSecCode');
        var message = resetsecuritycode.checkInputParameters($txtSecurityCode, $txtConfirmSecurityCode);
        if (message.length !== 0) {
            $messageContainer.empty().html(message);
            securitycodeHelper.unlockUI();
            return;
        }

        var hashKey = $('#hashkey').val();
        var secCode = $txtSecurityCode.val();
        secCode = secCode.ec(hashKey).hc();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        confirmSecCode = confirmSecCode.ec(hashKey).hc();
        $.ajax({
            type: 'POST',
            url: 'ResetSecurityCode',
            data: { custId: $('#custid').val(), subAccId: $('#subaccid').val(), securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorCode !== 0) {
                    resetsecuritycode.showFailMessage(data.ErrorMessage);
                    $txtSecurityCode.focus();
                }
                else
                {
                    resetsecuritycode.showSuccessfullyMessage();
                }
            },
            complete: function (jqXHR, textStatus) {
                securitycodeHelper.unlockUI();
            }
        })
    },
    checkInputParameters: function ($txtSecurityCode, $txtConfirmSecurityCode) {
        var secCode = $txtSecurityCode.val();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        if (secCode.length === 0) {
            $txtSecurityCode.focus();
           return resetsecuritycode.resources.securityCodeEmpty
           
        }
        else if (!securitycodeHelper.isValidSecurityCode(secCode)) {
            $txtSecurityCode.focus();
            return resetsecuritycode.resources.securityCodeWrongRule;
        }
        else if (secCode != confirmSecCode) {
            $txtConfirmSecurityCode.focus();
            return resetsecuritycode.resources.errorSecurityCodeConfirm;
        }

        return '';
    },
    resetInputValue: function()
    {
        $("#txtSecCode").empty();
        $("#txtConfirmSecCode").empty();
    },
    showSuccessfullyMessage: function(message)
    {
        $messageContainer.removeClass('msg-success');
        $messageContainer.addClass('msg-error').empty().html(message);
    },
    showFailMessage: function()
    {
        $messageContainer.removeClass('msg-error');
        $messageContainer.addClass('msg-success').empty().html(changeUserOwnSecurityCode.resources.securityCodeChangeSucessfully);
    }
}