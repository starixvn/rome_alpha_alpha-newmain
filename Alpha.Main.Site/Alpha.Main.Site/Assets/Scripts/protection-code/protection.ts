"use strict";
class ProtectionCode {
    $: JQueryStatic;
    msg: JQuery;
    txtNewProtectionCode: JQuery;
    txtConfirmProtectionCode: JQuery;
    resources: any;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    public init() {
        this.cacheElements();
        this.bindEvents();
        this.txtNewProtectionCode.focus();
    }

    private cacheElements() {
        this.msg = $("#msg");
        this.txtNewProtectionCode = $("#txt-newProtectionCode");
        this.txtConfirmProtectionCode = $("#txtConfirmProtectionCode");

        this.resources = {
            requiredNewProtectionCode: this.$("#requiredNewProtectionCode").val(),
            notMatchedConfirmProtectionCode: this.$("#notMatchedConfirmProtectionCode").val()
        };
    }

    private bindEvents() {
        this.txtNewProtectionCode.keyup(_ => this.validateProtectionCode(this.txtNewProtectionCode));

        this.txtConfirmProtectionCode.keyup(_ => this.validateProtectionCode(this.txtConfirmProtectionCode));

        const chkShowProtectionCode = $("#chkShowProtectionCode");
        chkShowProtectionCode.change(_ => this.showHideProtectionCode(chkShowProtectionCode.is(":checked")));

        const frmProtectionCode = $("#frmProtectionCode");
        frmProtectionCode.submit(e => {
            e.preventDefault();
            this.updateProtectionCode(frmProtectionCode.attr("action"));
        });
    }

    private validateProtectionCode($textbox: JQuery) {
        this.isValidProtectionCode($textbox.val())
            ? securityHelper.showSuccessfullyIcon($textbox)
            : securityHelper.showFailIcon($textbox);
    }

    private isValidProtectionCode(protectionCode: string) {
        return protectionCode.length >= 6;
    }

    private showHideProtectionCode(isShow: boolean) {
        if (isShow) {
            this.txtNewProtectionCode.addClass("show-hint");
            this.txtConfirmProtectionCode.addClass("show-hint");
        } else {
            this.txtNewProtectionCode.removeClass("show-hint");
            this.txtConfirmProtectionCode.removeClass("show-hint");
        }
    }

    private updateProtectionCode(url: string) {
        let formData = this.validateFormData();
        if (formData) {
            this.$.blockUI();
            formData.protectionCode = formData.protectionCode.encrypt();
            this.$.post(url, formData, result => {
                if (result.Code) {
                    securityHelper.showFailMessage(this.msg, result.Message);
                    this.txtNewProtectionCode.focus();
                } else {
                    securityHelper.showSuccessfullyMessage(this.msg, result.Message);
                    setTimeout(function () {
                        window.location.href = window.location.href;
                    }, 5000);
                }
            }).done(this.$.unblockUI);
        }
    }

    private validateFormData() {
        const newProtectionCode = this.txtNewProtectionCode.val();
        if (!this.isValidProtectionCode(newProtectionCode)) {
            this.txtNewProtectionCode.focus();
            securityHelper.showFailMessage(this.msg, this.resources.requiredNewProtectionCode);
            return;
        }

        const confirmProtectionCode = this.txtConfirmProtectionCode.val();
        if (confirmProtectionCode !== newProtectionCode) {
            this.txtConfirmProtectionCode.focus();
            securityHelper.showFailMessage(this.msg, this.resources.notMatchedConfirmProtectionCode);
            return;
        }

        return {
            protectionCode: newProtectionCode
        };
    }
}

$(function () {
    const protectionCode = new ProtectionCode($);
    protectionCode.init();
})