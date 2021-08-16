"use strict";
interface ValidationResult {
    isValid: boolean;
    error: string;
}

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
        this.txtConfirmProtectionCode = $("#txt-confirmProtectionCode");

        this.resources = {
            requiredNewProtectionCode: this.$("#requiredNewProtectionCode").val(),
            minLengthNewProtectionCodeError: this.$("#minLengthNewProtectionCodeError").val(),
            requiredConfirmProtectionCode: this.$("#requiredConfirmProtectionCode").val(),
            notMatchedConfirmProtectionCode: this.$("#notMatchedConfirmProtectionCode").val(),
            containsSpaceOrUnicodeCharacterError: this.$("#containsSpaceOrUnicodeCharacterError").val()
        };
    }

    private bindEvents() {
        this.txtNewProtectionCode.keyup(_ => this.validateProtectionCode(this.txtNewProtectionCode));

        this.txtConfirmProtectionCode.keyup(_ => this.validateProtectionCode(this.txtConfirmProtectionCode));

        const chkShowProtectionCode = $("#chk-showProtectionCode");
        chkShowProtectionCode.change(_ => this.showHideProtectionCode(chkShowProtectionCode.is(":checked")));

        const frmProtectionCode = $("#frm-protectionCode");
        frmProtectionCode.submit(e => {
            e.preventDefault();
            this.updateProtectionCode(frmProtectionCode.attr("action"));
        });

        this.$("#btn-reset").click(_ => {
            this.showHideProtectionCode(false);
            securityHelper.removeValidationIcon(this.txtNewProtectionCode);
            securityHelper.removeValidationIcon(this.txtConfirmProtectionCode);
        });
    }

    private validateProtectionCode($textbox: JQuery) {
        this.checkValidProtectionCode($textbox.val()).isValid
            ? securityHelper.showSuccessfullyIcon($textbox)
            : securityHelper.showFailIcon($textbox);
    }

    private checkValidProtectionCode(protectionCode: string): ValidationResult {
        if (protectionCode.length < 6) {
            return {
                isValid: false,
                error: this.resources.minLengthNewProtectionCodeError
            };
        }

        const Space = 32;
        const MaxAnsiCode = 127;
        let charCode;

        for (let i = 0, max = protectionCode.length; i < max; i++) {
            charCode = protectionCode.charCodeAt(i);

            if (charCode === Space || charCode > 127) {
                return {
                    isValid: false,
                    error: this.resources.containsSpaceOrUnicodeCharacterError
                };
            }
        }

        return {
            isValid: true,
            error: undefined
        };
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
                    this.msg.parent().removeClass("hidden");
                    setTimeout(function () {
                        window.location.href = window.location.href;
                    }, 5000);
                }
            }).always(this.$.unblockUI);
        }
    }

    private validateFormData() {
        const newProtectionCode = this.txtNewProtectionCode.val();
        if (!newProtectionCode.length) {
            this.txtNewProtectionCode.focus();
            securityHelper.showFailMessage(this.msg, this.resources.requiredNewProtectionCode);
            return;
        }

        const validationResult = this.checkValidProtectionCode(newProtectionCode);
        if (!validationResult.isValid) {
            this.txtNewProtectionCode.focus();
            securityHelper.showFailMessage(this.msg, validationResult.error);
            return;
        }

        const confirmProtectionCode = this.txtConfirmProtectionCode.val();
        if (!confirmProtectionCode.length) {
            this.txtConfirmProtectionCode.focus();
            securityHelper.showFailMessage(this.msg, this.resources.requiredConfirmProtectionCode);
            return;
        }

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