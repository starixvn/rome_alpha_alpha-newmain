$(document).ready(function () {
    verifySecurityCode.initialize();
})
var verifySecurityCode = {
    initialize: function () {
        $('#divVerifySecurityBlock').removeClass('hidden');
        $('#SC').keypress(function (e) {
            if (e.which == 13) {
                verifySecurityCode.verify();
                e.preventDefault();
            }
        });
        $('#btn-submit').click(function (e) {
            verifySecurityCode.verify();
            e.preventDefault();
        });
        $('#txtSecCode').focus();

        //$('#simulateDone').click(function (e) {
        //    var preUrl = top.popupSecurityCode.iframeAttributes.preUrl;
        //    top[preUrl]();
        //    top.popupSecurityCode.close();
        //    e.preventDefault();
        //});
    },
    verify: function () {
        lockUI();
        var $messageContainer = $('#errorSection');
        var $txtSecCode = $('#SC');
        var message = verifySecurityCode.checkInputParameters($txtSecCode);
        if (message.length !== 0) {
            showMessage($messageContainer, message, 1);
            unlockUI();
            return;
        }

        var ph = parent.top.ph;
        var secCode = $txtSecCode.val();
        secCode = hashSecurityCode(secCode);
        $.ajax({
            type: 'POST',
            url: 'VerifySecurityCode',
            data: { securityCode: secCode },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                var errorCode = data.ErrorCode;
                if (errorCode === 0) { // OK
                    redirectToHomePage(200);
                }
                else if (errorCode === 2) { // account is closed
                    alert(data.ErrorMessage);
                    top.age.SignOut();
                }
                else {
                    showMessage($messageContainer, data.ErrorMessage, data.ErrorCode);
                    $txtSecCode.focus();
                }
            },
            complete: function (jqXHR, textStatus) {
                unlockUI();
            }
        })
    },
    checkInputParameters: function ($txtSecCode) {
        var message = '';
        var secCode = $txtSecCode.val();
        if (secCode.length === 0) {
            message = securityCodeResource.informSecCode;
            $txtSecCode.focus();
        }
        return message;
    }
}