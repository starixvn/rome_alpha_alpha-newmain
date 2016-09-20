$(document).ready(function () {
    changeSecurityCodeFirstLogin.initialize();
})
var changeSecurityCodeFirstLogin = {
    resources: {
        rules: "<i class='icon-information-outline icon-rules' data-toggle='modal' data-target='#sc'></i>",
        securityCodeEmpty: $('#alert-securitycode-empty').val(),
        securityCodeWrongRule: $('#alert-securitycode-wrong-rule').val(),
        errorSecurityCodeConfirm: $('#alert-error-securitycode-confirm').val(),
    },
    initialize: function () {
        $('.form-SC').submit(function (e) {
            changeSecurityCodeFirstLogin.changeSecurityCode();
            e.preventDefault();
        });
        $("#btn-back").bind("click", function () {
            redirectToHomePage(2000);
        });

        $('#txtSecCode').focus();
    },
    changeSecurityCode: function () {
        securitycodeHelper.lockUI();
        var $txtSecurityCode = $('#txtSecCode');
        var $txtConfirmSecurityCode = $('#txtConfirmSecCode');
        if (!changeSecurityCodeFirstLogin.validateInputSecurityCode($txtSecurityCode, $txtConfirmSecurityCode)) {
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
            url: 'ChangeSecurityCodeFirstLogin',
            data: { securityCode: secCode, confirmSecurityCode: confirmSecCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorCode === 0) {
                    securitycodeHelper.redirectToHomePage(2000);
                }
                else {
                    $("#txtSecCode").addClass('error');
                    $("#txtSecCode-error").empty().html(data.ErrorMessage);
                    $txtSecurityCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                securitycodeHelper.unlockUI();
            }
        })
    },
    validateInputSecurityCode: function ($txtSecurityCode, $txtConfirmSecurityCode) {
        changeSecurityCodeFirstLogin.resetOriginalInputStatus();
        var secCode = $txtSecurityCode.val();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        if (secCode.length === 0) {
            $("#txtSecCode").addClass('error');
            $("#txtSecCode-error").empty().html(changeSecurityCodeFirstLogin.resources.securityCodeEmpty).append(changeSecurityCodeFirstLogin.resources.rules);
            $txtSecurityCode.focus();
            return false;
        }
        else if (!securitycodeHelper.isValidSecurityCode(secCode)) {
            $("#txtSecCode").addClass('error');
            $("#txtSecCode-error").empty().html(changeSecurityCodeFirstLogin.resources.securityCodeWrongRule).append(changeSecurityCodeFirstLogin.resources.rules);
            $txtSecurityCode.focus();
            return false;
        }
        else if (secCode != confirmSecCode) {
            $("#txtConfirmSecCode").addClass('error');
            $("#txtConfirmSecCode-error").empty().html(changeSecurityCodeFirstLogin.resources.errorSecurityCodeConfirm);
            $txtConfirmSecurityCode.focus();
            return false;
        }

        return true;
    },
    resetOriginalInputStatus: function () {
        $("#txtSecCode").removeClass('error');
        $("#txtSecCode-error").empty();
        $("#txtConfirmSecCode").removeClass('error');
        $("#txtConfirmSecCode-error").empty();
    }
}