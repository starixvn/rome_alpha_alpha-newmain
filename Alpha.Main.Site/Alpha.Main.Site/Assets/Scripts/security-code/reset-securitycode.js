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
            //$('#spnNewPwdIcon').removeClass();
            //$('#spnConfirmSecCodeIcon').removeClass();
        });
        $('#txtSecCode').focus();

        //var hasPopUp = false;
        //var popUp;
        //if (top.isNewSite) {
        //    popUp = top.$('.modal[id^="page-popup"]');
        //    var popUpLength = popUp.length;
        //    hasPopUp = !top.$(popUp[popUpLength - 1]).hasClass('hidden');
        //} else {
        //    popUp = parent.document.getElementsByClassName('AGEWnd');
        //    hasPopUp = popUp && popUp.length;
        //}

        //if (hasPopUp) {
        //    var titles = $("#header_main");
        //    titles.addClass('display-none');
        //    $('.tbl-security .subTitle').addClass('display-none');
        //}
    },
    reset: function () {
        //lockUI();
        var $messageContainer = $('#error-message');
        var $txtSecurityCode = $('#SC');
        var $txtConfirmSecurityCode = $('#confirmSC');
        var message = resetsecuritycode.checkInputParameters($txtSecurityCode, $txtConfirmSecurityCode);
        if (message.length !== 0) {
            $messageContainer.empty().html(message);
            // unlockUI();
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
                    $messageContainer.empty().html(data.ErrorMessage);
                    $txtSecurityCode.focus();
                }
                
                //window.top.reloadPopUpSetting(3000);
            },
            complete: function (jqXHR, textStatus) {
                //unlockUI();
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
    }
}