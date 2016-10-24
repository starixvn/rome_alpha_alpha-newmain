/// <reference path="../../core.ts" />

class SetupOtp {
    $: any;

    $otpForm: any;
    $txtOtp: any;
    $registerPanel: any;
    $removeSecretKeyPanel: any;
    $btnRemoveOtp: any;

    constructor($: any) {
        this.$ = $;
    }

    init = () => {
        this.cachedDomElements();
        this.registerEvents();
    }

    cachedDomElements = () => {
        this.$otpForm = this.$("#otp-form");
        this.$txtOtp = this.$("#txtOtp");
        this.$registerPanel = this.$("#register-panel");
        this.$removeSecretKeyPanel = this.$("#remove-secret-key-panel");
        this.$btnRemoveOtp = this.$("#btn-remove-otp");
    }

    registerEvents = () => {
        
    }

    setUpEvent = (e: any) => {
        e.preventDefault();

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "Otp/SetUp",
            data: { opt: this.$txtOtp.val() },
            dataType: "json",
            success: function (result: any) {
                if (result.Code == 0) {

                }
            },
            complete: function () {
                this.$.unblockUI();
            }
        });
    }

    onSuccess = (result: any) => {
        if (result.Code == 0) {
            this.$registerPanel.hide();
            this.$removeSecretKeyPanel.show();
        }
    }

    removeOtp = (result: any) => {
        
    }
}

var setUpOtp = new SetupOtp($);

$(function () {
    setUpOtp.init();
});