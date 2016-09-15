$(document).ready(function () {
    resetsecuritycode.initialize();
})
var resetsecuritycode = {
    initialize: function () {
        var $form = $('.form-SC');
        $form.submit(function (e) {
            resetsecuritycode.reset();
            e.preventDefault();
        });
        $form.bind('reset', function (e) {
            $('#spnNewPwdIcon').removeClass();
            $('#spnConfirmSecCodeIcon').removeClass();
        });
        $('#txtSecCode').focus();

        var hasPopUp = false;
        var popUp;
        if (top.isNewSite) {
            popUp = top.$('.modal[id^="page-popup"]');
            var popUpLength = popUp.length;
            hasPopUp = !top.$(popUp[popUpLength - 1]).hasClass('hidden');
        } else {
            popUp = parent.document.getElementsByClassName('AGEWnd');
            hasPopUp = popUp && popUp.length;
        }

        if (hasPopUp) {
            var titles = $("#header_main");
            titles.addClass('display-none');
            $('.tbl-security .subTitle').addClass('display-none');
        }
    },
    reset: function () {
        lockUI();
        var $messageContainer = $('#errorSection');
        var $txtSecCode = $('#SC');
        var $txtConfirmSecCode = $('#confirmSC');
        var message = resetsecuritycode.checkInputParameters($txtSecCode, $txtConfirmSecCode);
        if (message.length !== 0) {
            $('#error-wrap').removeClass('display-none');
            showMessage($messageContainer, message, 1);
            unlockUI();
            return;
        }

        var secCode = $txtSecCode.val();
        secCode = secCode.ec(ph).hc();
        var confirmSecCode = $txtConfirmSecCode.val();
        confirmSecCode = confirmSecCode.ec(ph).hc();
        $.ajax({
            type: 'POST',
            url: 'ResetSecurityCode',
            data: { custId: window.custid, subAccId: window.subaccid, securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                $('#error-wrap').removeClass('display-none');
                showMessage($messageContainer, data.ErrorMessage, data.ErrorCode);
                $txtSecCode.focus();

                window.top.reloadPopUpSetting(3000);
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