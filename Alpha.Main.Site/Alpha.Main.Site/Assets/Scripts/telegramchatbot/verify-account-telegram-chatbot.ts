/// <reference path="../core.ts" />
"use strict";

class VerifyAccountTelegramChatbot extends BaseTelegramChatBot {
    $: JQueryStatic;

    // Verify Account
    $verificationSection: JQuery;
    $verificationForm: JQuery;
    $txtOTP: JQuery;
    $verificationMessage: JQuery;
    $btnVerify: JQuery;
    $btnResend: JQuery;

    // Captcha
    $captchaContainer: JQuery;
    $txtCaptcha: JQuery;
    $captcha: JQuery;
    $captchaRefreshIcon: JQuery;
    $modelImageElement: JQuery;
    $modelTokenParameterName: JQuery;
    $modelTokenElement: JQuery;
    $modelRefreshUrl: JQuery;

    isShowCaptcha: boolean;
    imageElementId: string;
    tokenElementId: string;

    resources = {
        //verificationCodeIsInvalidOrExpired: (<HTMLInputElement>document.getElementById('VerificationCodeIsInvalidOrExpired')).value,
        //verificationCodeIsRequired: (<HTMLInputElement>document.getElementById('VerificationCodeIsRequired')).value,
        //captchaIsRequired: (<HTMLInputElement>document.getElementById('CaptchaIsRequired')).value,
        //accountHasAlreadyBeenRegistered: (<HTMLInputElement>document.getElementById('AccountHasAlreadyBeenRegistered')).value
        OTPIsInvalidOrExpired: "OTP is incorrected, please try again",
        OTPIsRequired: "Please input OTP.",
        captchaIsRequired: "Captcha is required",
        accountHasAlreadyBeenRegistered: "test"
    }

    init = () => {
        this.isShowCaptcha = this.convertAttributeToBool(this.$("#IsShowCaptcha").val());
        this.cachedDOMElements();
        this.registerEvents();

        if (this.isShowCaptcha) {
            this.cachedCaptchaDOMElements();
            this.registerCaptchaEvents();
            this.$captcha.show();
        }
    }

    cachedDOMElements = () => {
        this.$verificationSection = this.$("#verification-section");
        this.$verificationForm = this.$("#verification-form");
        this.$txtOTP = this.$("#txtOTP");
        this.$verificationMessage = this.$("#verification-code-message");
        this.$captchaContainer = this.$("#captcha-container");
        this.$btnVerify = this.$("#btnVerify");
        this.$btnResend = this.$("#btnResend");
    }

    registerEvents = () => {
        this.$btnVerify.click(this.verifyOTPEvent);
        this.$btnResend.click(this.resendEvent);

        this.$txtOTP.keypress((event) => {
            if (event.keyCode === 13 && !this.$btnVerify.is(":disabled")) {
                this.$btnVerify.click();
            }
        });
    }

    initCaptcha = () => {
        this.cachedCaptchaDOMElements();
        this.registerCaptchaEvents();
        this.$captcha.show();
    }

    cachedCaptchaDOMElements = () => {
        var captchaId = this.$("#CaptchaId").val();
        this.$captcha = this.$("#" + captchaId);
        this.$captchaRefreshIcon = this.$("#captcha-refresh-icon");
        this.$txtCaptcha = this.$("#txtCaptcha");

        this.imageElementId = this.$("#ModelImageElementId").val();
        this.$modelImageElement = this.$("#" + this.imageElementId);
        this.$modelTokenParameterName = this.$("#ModelTokenParameterName");

        this.tokenElementId = this.$("#ModelTokenElementId").val();
        this.$modelTokenElement = this.$("#" + this.tokenElementId);
        this.$modelRefreshUrl = this.$("#ModelRefreshUrl");
    }

    registerCaptchaEvents = () => {
        this.$captchaRefreshIcon.click(this.refreshCaptchaEvent);
        this.$modelImageElement.click(this.refreshCaptchaEvent);
        this.$txtCaptcha.keypress((event) => {
            if (event.keyCode === 13 && !this.$btnVerify.is(":disabled")) {
                this.$btnVerify.click();
            }
        });
    }

    verifyOTPEvent = (e: any) => {
        e.preventDefault();

        var OTP = this.$txtOTP.val();
        if (OTP.length === 0) {
            this.$txtOTP.focus();
            alpha.showMessage(this.$verificationMessage, new Result({ Code: 1, Message: this.resources.OTPIsRequired }));
            return;
        } else {
            this.$verificationMessage.html("");
        }

        if (this.isShowCaptcha) {
            if (this.$txtCaptcha.val() === "") {
                this.$txtCaptcha.focus();
                alpha.showMessage(this.$verificationMessage, new Result({ Code: 1, Message: this.resources.captchaIsRequired }));
                return;
            } else {
                this.$verificationMessage.html("");
            }
        }

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "TelegramChatBot/VerifyOTP",
            data: this.$verificationForm.serialize(),
            dataType: "text",
            success: this.onVerifySuccess,
            complete: this.onComplete,
        });
    }

    onVerifySuccess = (data: any) => {
        var result = this.safelyParseJSON(data);

        if (result) {
            if (result.IsOk) {
                parent.window.location.href = parent.window.location.href;
            } else {
                if (result.IsNotAuthorized) {
                    this.topWindow.bootboxAlert(result.Message);
                    return;
                }

                this.isShowCaptcha = this.convertAttributeToBool(this.$("#IsShowCaptcha").val());
                if (this.isShowCaptcha) {
                    this.reloadCaptcha();
                }

                if (result.IsVerificationCodeExpired || result.IsWrongVerificationCode) {
                    alpha.showMessage(this.$verificationMessage, new Result({ Code: 1, Message: this.resources.OTPIsInvalidOrExpired }));
                } else if (result.IsMessengerAccountHasBeenRegisteredByAnotherCustomer) {
                    alpha.showMessage(this.$verificationMessage, new Result({ Code: 1, Message: this.resources.accountHasAlreadyBeenRegistered }));
                } else {
                    alpha.showMessage(this.$verificationMessage, new Result({ Code: result.Code, Message: result.Message }));
                }
            }

            return;
        }

        this.$captchaContainer.html(data);
        this.initCaptcha();
        alpha.showMessage(this.$verificationMessage, new Result({ Code: 1, Message: this.resources.OTPIsInvalidOrExpired }))
    }

    resendEvent = (e: any) => {
        e.preventDefault();

        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "TelegramChatBot/ResendOTP",
            dataType: "json",
            success: this.onResendSuccess,
            complete: this.onComplete,
        });
    }

    onResendSuccess = (data: any) => {
        if (data) {
            if (data.IsNotAuthorized) {
                this.topWindow.bootboxAlert(data.Message);
                return;
            }

            if (data.IsOk) {
                this.$btnResend.attr("disabled", "disabled");
                var interval = setInterval(() => {
                    clearInterval(interval);
                    this.$btnResend.removeAttr("disabled");
                }, 2000)
            }
        }
    }

    refreshCaptchaEvent = (e: any) => {
        var self = this;
        var tokenParams = {};
        tokenParams[this.$modelTokenParameterName.val()] = this.$modelTokenElement.val();

        var url = this.$modelRefreshUrl.val();
        this.$.post(url, tokenParams)
            .always(function (result) {
                eval(result.responseText);
                self.$captcha.show();
            });

        return false;
    }

    reloadCaptcha = () => {
        this.$.blockUI();
        this.$.ajax({
            type: "GET",
            url: alpha.urls.mainRootPath + "TelegramChatBot/Captcha",
            dataType: "text",
            success: (result) => {
                this.$captchaContainer.html(result);
                this.initCaptcha();
            },
            complete: this.onComplete,
        });
    }
}