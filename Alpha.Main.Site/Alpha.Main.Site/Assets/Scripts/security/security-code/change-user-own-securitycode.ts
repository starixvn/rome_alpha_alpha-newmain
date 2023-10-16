'use strict';

var changeUserOwnSecurityCode = {
    topWindow: null,
    initialize: function () {
        changeUserOwnSecurityCode.topWindow = window.top;
        var $form = $('.form-SC');
        $form.submit(function (e) {
            changeUserOwnSecurityCode.change();
            e.preventDefault();
        });

        $form.bind('reset', function (e) {
            changeUserOwnSecurityCode.resetOriginalInputStatus();
        });

        $('#passwordNav').click(function () {
            changeUserOwnSecurityCode.gotoPasswordPage()
        });

        $('#txtOldSecCode').val("").focus();

        $("#txtSecCode").val("").keyup(function () {
            securityHelper.validateSecurityCode(this);
        });

        $("#txtConfirmSecCode").val("").keyup(function () {
            securityHelper.validateSecurityCode(this);
        });

        $("#showSCchk").click(function () {
            var isChecked = $(this).is(":checked");
            isChecked ? securityHelper.showHintSC() : securityHelper.hideHintSC();
        });

        $(".switch-on").click(changeUserOwnSecurityCode.handleSwitchButton);
    },
    change: function () {
        $.blockUI();
        var $container = $('#status-message');
        var $messageContainer = $('#status-message-content');
        $container.removeClass('display-none');
        var $txtOldSecurityCode = $('#txtOldSecCode');
        var $txtSecurityCode = $('#txtSecCode');
        var $txtConfirmSecurityCode = $('#txtConfirmSecCode');
        var message = changeUserOwnSecurityCode.checkInputParameters($txtOldSecurityCode, $txtSecurityCode, $txtConfirmSecurityCode);
        if (message.length !== 0) {
            securityHelper.showFailMessage($messageContainer, message);
            $.unblockUI();
            return;
        }

        var hashKey = $('#hashkey').val();
        var oldSecCode = $txtOldSecurityCode.val();
        oldSecCode = securityHelper.hashSecurityCode(oldSecCode);
        var secCode = $txtSecurityCode.val();
        secCode = secCode.encrypt();
        $.ajax({
            type: 'POST',
            url: 'ChangeUserOwnSecurityCode',
            data: { oldSecurityCode: oldSecCode, securityCode: secCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.Code === 0) { // OK
                    securityHelper.showSuccessfullyMessage($messageContainer, securityCodeResources.securityCodeChangeSucessfully);
                    setTimeout(changeUserOwnSecurityCode.refreshPage, 5000);
                }
                else if (data.Code === 2) { // account is closed
                    alert(data.Message);
                    securityHelper.signOut();
                }
                else {
                    securityHelper.showFailMessage($messageContainer, data.Message);
                    $txtOldSecurityCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                $.unblockUI();
            }
        })
    },
    checkInputParameters: function ($txtOldSecurityCode, $txtSecurityCode, $txtConfirmSecurityCode) {
        var oldSecCode = $txtOldSecurityCode.val();
        var secCode = $txtSecurityCode.val();
        var confirmSecCode = $txtConfirmSecurityCode.val();
        if (oldSecCode.length === 0) {
            $txtOldSecurityCode.focus();
            return securityCodeResources.oldSecurityCodeEmpty;
        }
        else if (secCode.length === 0) {
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
        else if (oldSecCode === secCode) {
            $txtSecurityCode.focus();
            return securityCodeResources.sameSecurityCode;
        }

        return '';
    },
    resetOriginalInputStatus: function () {
        securityHelper.hideHintSC();
        securityHelper.removeValidationIcon($("#txtSecCode"));
        securityHelper.removeValidationIcon($("#txtConfirmSecCode"));
    },
    gotoPasswordPage: function () {
        window.location.href = $("#mainRootPath").val() + 'Password/ChangePassword';
    },
    refreshPage: function () {
        window.location.href = $("#mainRootPath").val() + 'SecurityCode/ChangeUserOwnSecurityCode';
    },
    handleSwitchButton: function () {
        $.blockUI();
        const $element = $(this);
        const isOff = $element.hasClass("switch-off");
        const functionName = $element.data("function");
        $.post("UpdateSecurityCodeFunction", { function: functionName, isOn: isOff }, function (data) {
            if (data.IsOk) {
                if (changeUserOwnSecurityCode.topWindow.menuBar.domElements.reportDomain) {
                    setTimeout(() => {
                        (<HTMLIFrameElement>top.document.getElementById('reportFrame')).src = changeUserOwnSecurityCode.topWindow.menuBar.domElements.reportDomain + changeUserOwnSecurityCode.topWindow.menuBar.domElements.cookieless + '/Auth/SecurityCode' + functionName + '?s=' + isOff;
                    }, 2000);
                }

                setTimeout(() => {
                    if (isOff) {
                        $element
                            .removeClass("switch-off")
                            .text(securityCodeResources.switchLableOn);
                    } else {
                        $element
                            .addClass("switch-off")
                            .text(securityCodeResources.switchLableOff);
                    }
                }, 3000);
            } else {
                $("#status-message").removeClass("display-none");
                securityHelper.showFailMessage($("#status-message-content"), data.Message);
                $(window).scrollTop(0);
            }
        }).always($.unblockUI);
    }
}

$(function () {
    changeUserOwnSecurityCode.initialize();
});