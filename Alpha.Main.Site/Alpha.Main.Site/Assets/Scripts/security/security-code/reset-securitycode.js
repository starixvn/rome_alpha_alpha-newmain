'use strict';
var resetsecuritycode = {
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            resetsecuritycode.reset();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            resetsecuritycode.resetInputValue();
        });
        $("#txtSecCode").focus().keyup(function () {
            securityHelper.validateSecurityCode(this);
        });
        $("#txtConfirmSecCode").keyup(function () {
            securityHelper.validateSecurityCode(this);
        });
        $("#showSCchk").click(function () {
            var isChecked = $(this).is(":checked");
            isChecked ? securityHelper.showHint() : securityHelper.hideHint();
        });
        if (top.hasPopupOpen()) {
            $("#container").addClass("has-popup");
        }
    },
    reset: function () {
        $.blockUI();
        var $container = $('#status-message');
        var $messageContainer = $('#status-message-content');
        $container.removeClass('display-none');
        var $txtSecurityCode = $('#txtSecCode');
        var $txtConfirmSecurityCode = $('#txtConfirmSecCode');
        var message = resetsecuritycode.checkInputParameters($txtSecurityCode, $txtConfirmSecurityCode);
        if (message.length !== 0) {
            securityHelper.showFailMessage($messageContainer, message);
            $.unblockUI();
            return;
        }
        var hashKey = $('#hashkey').val();
        var secCode = $txtSecurityCode.val();
        secCode = secCode.ec(hashKey).hc();
        $.ajax({
            type: 'POST',
            url: 'ResetSecurityCode',
            data: { custId: $('#custid').val(), subAccId: $('#subaccid').val(), securityCode: secCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.Code !== 0) {
                    securityHelper.showFailMessage($messageContainer, data.Messsage);
                    $txtSecurityCode.focus();
                }
                else {
                    securityHelper.showSuccessfullyMessage($messageContainer, securityCodeResources.securityCodeChangeSucessfully);
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        });
    },
    checkInputParameters: function ($txtSecurityCode, $txtConfirmSecurityCode) {
        var secCode = $txtSecurityCode.val();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        if (secCode.length === 0) {
            $txtSecurityCode.focus();
            return securityCodeResources.securityCodeEmpty;
        }
        else if (!securityHelper.isValidSecurityCode(secCode)) {
            $txtSecurityCode.focus();
            return securityCodeResources.securityCodeWrongRule;
        }
        else if (secCode != confirmSecCode) {
            $txtConfirmSecurityCode.focus();
            return securityCodeResources.errorSecurityCodeConfirm;
        }
        return '';
    },
    resetInputValue: function () {
        $("#txtSecCode").empty();
        $("#txtConfirmSecCode").empty();
    }
};
$(function () {
    resetsecuritycode.initialize();
});
