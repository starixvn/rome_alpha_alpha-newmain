$(document).ready(function () {
    changeSecurityCodeByForcing.initialize();
})
var changeSecurityCodeByForcing = {
    resources: {
        rules: "<i class='icon-information-outline icon-rules' data-toggle='modal' data-target='#sc'></i>",
        oldSecurityCodeEmpty: $('#alert-old-securitycode-empty').val(),
        securityCodeEmpty: $('#alert-securitycode-empty').val(),
        securityCodeWrongRule: $('#alert-securitycode-wrong-rule').val(),
        errorSecurityCodeConfirm: $('#alert-error-securitycode-confirm').val(),
        sameSecurityCode: $('#alert-same-securitycode').val(),
        securityCodeChangeSucessfully: $('#alert-securitycode-change-ok').val()
    },
    initialize: function () {
        $('.form-SC').submit(function (e) {
            changeSecurityCodeByForcing.changeSecurityCode();
            e.preventDefault();
        });

        $("#btn-cancel").bind("click", function () {
            redirectToHomePage(2000);
        });

        $('#providedSC').focus();
    },
    changeSecurityCode: function () {
        //lockUI();
        //var $messageContainer = $('#error-message');
        var $txtOldSecurityCode = $('#providedSC');
        var $txtSecurityCode = $('#SC');
        var $txtConfirmSecurityCode = $('#confirmSC');
        if (!changeSecurityCodeByForcing.validateInputSecurityCode($txtOldSecurityCode, $txtSecurityCode, $txtConfirmSecurityCode)) {
            return;
        }

        var oldSecCode = $txtOldSecurityCode.val();
        oldSecCode = securitycodeHelper.hashSecurityCode(oldSecCode);
        var secCode = $txtSecurityCode.val();
        var hashKey = $('#hashkey').val();
        secCode = secCode.ec(hashKey).hc();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        confirmSecCode = confirmSecCode.ec(hashKey).hc();
        $.ajax({
            type: 'POST',
            url: 'ChangeSecurityCodeByForcing',
            data: { oldSecurityCode: oldSecCode, securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorCode === 0) { // OK
                    securitycodeHelper.redirectToHomePage(2000);
                }
                else if (data.ErrorCode === 2) { // account is closed
                    alert(data.ErrorMessage);
                    top.age.SignOut();
                }
                else {
                    $("#providedSC").addClass('error');
                    $("#lblprovidedSC-error").empty().html(data.ErrorMessage);
                    $txtOldSecurityCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                //unlockUI();
            }
        })
    },
    validateInputSecurityCode: function ($txtOldSecurityCode, $txtSecurityCode, $txtConfirmSecurityCode) {
        changeSecurityCodeByForcing.resetOriginalInputStatus();
        var oldSecCode = $txtOldSecurityCode.val();
        var secCode = $txtSecurityCode.val();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        if (oldSecCode.length === 0) {
            $("#providedSC").addClass('error');
            $("#lblprovidedSC-error").empty().html(changeSecurityCodeByForcing.resources.oldSecurityCodeEmpty);
            $txtOldSecurityCode.focus();
            return false;
        }
        else if (secCode.length === 0) {
            $("#SC").addClass('error');
            $("#lblSC-error").empty().html(changeSecurityCodeByForcing.resources.securityCodeEmpty).append(changeSecurityCodeByForcing.resources.rules);
            $txtSecurityCode.focus();
            return false;
        }
        else if (!securitycodeHelper.isValidSecurityCode(secCode)) {
            $("#SC").addClass('error');
            $("#lblSC-error").empty().html(changeSecurityCodeByForcing.resources.securityCodeWrongRule).append(changeSecurityCodeByForcing.resources.rules);
            $txtSecurityCode.focus();
            return false;
        }
        else if (secCode != confirmSecCode) {
            $("#confirmSC").addClass('error');
            $("#lblconfirmSC-error").empty().html(changeSecurityCodeByForcing.resources.errorSecurityCodeConfirm);
            $txtConfirmSecurityCode.focus();
            return false;
        }
        else if (oldSecCode === secCode) {
            $("#confirmSC").addClass('error');
            $("#lblconfirmSC-error").empty().html(changeSecurityCodeByForcing.resources.sameSecurityCode);
            $txtSecurityCode.focus();
            return false;
        }

        return true;
    },
    resetOriginalInputStatus: function () {
        $("#providedSC").removeClass('error');
        $("#lblprovidedSC-error").empty();
        $("#SC").removeClass('error');
        $("#lblSC-error").empty();
        $("#confirmSC").removeClass('error');
        $("#lblconfirmSC-error").empty();
    }
}