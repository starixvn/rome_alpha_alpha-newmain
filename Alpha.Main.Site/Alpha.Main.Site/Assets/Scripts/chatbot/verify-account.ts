/// <reference path="../core.ts" />
"use strict";

class VerifyAccount extends BaseChatBot {
    $: JQueryStatic;

    // Verify Account
    $verificationSection: JQuery;
    $verificationForm: JQuery;
    $txtVerificationCode: JQuery;
    $verificationMessage: JQuery;
    $userId: JQuery;
    $messengerAccount: JQuery;
    $btnVerify: JQuery;
    $btnResend: JQuery;
    $countDown: JQuery;

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
    timer: number;

    resources = {
        verificationCodeIsInvalidOrExpired: (<HTMLInputElement>document.getElementById('VerificationCodeIsInvalidOrExpired')).value,
        verificationCodeIsRequired: (<HTMLInputElement>document.getElementById('VerificationCodeIsRequired')).value,
        captchaIsRequired: (<HTMLInputElement>document.getElementById('CaptchaIsRequired')).value
    }

    init = () => {
        this.cachedDOMElements();
        this.registerEvents();

        this.isShowCaptcha = this.convertAttributeToBool(this.$("#IsShowCaptcha").val());

        if (this.isShowCaptcha) {
            this.cachedCaptchaDOMElements();
            this.registerCaptchaEvents();
            this.$captcha.show();
        }

        this.countDownTimer();
    }

    cachedDOMElements = () => {
        this.$verificationSection = this.$("#verification-section");
        this.$verificationForm = this.$("#verification-form");
        this.$txtVerificationCode = this.$("#txtVerificationCode");
        this.$verificationMessage = this.$("#verification-code-message");
        this.$captchaContainer = this.$("#captcha-container");
        this.$userId = this.$("#UserId");
        this.$messengerAccount = this.$("#MessengerAccount");
        this.$btnVerify = this.$("#btnVerify");
        this.$btnResend = this.$("#btnResend");
        this.$countDown = this.$("#countdown");
    }

    registerEvents = () => {
        this.$btnVerify.click(this.verifyCodeEvent);
        this.$btnResend.click(this.resendEvent);

        this.$txtVerificationCode.keypress((event) => {
            if (event.keyCode === 13 && !this.$btnVerify.is(":disabled")) {
                this.$btnVerify.click();
            }
        });

        this.$txtCaptcha.keypress((event) => {
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
    }

    verifyCodeEvent = (e: any) => {
        e.preventDefault();

        var verificationCode = this.$txtVerificationCode.val();
        if (verificationCode.length === 0) {
            this.$txtVerificationCode.focus();
            alpha.showMessage(this.$verificationMessage, new Result({ Code: 1, Message: this.resources.verificationCodeIsRequired }));
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
            url: alpha.urls.mainRootPath + "ChatBot/VerifyVerificationCode",
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
                    alpha.showMessage(this.$verificationMessage, new Result({ Code: 1, Message: this.resources.verificationCodeIsInvalidOrExpired }));
                } else {
                    alpha.showMessage(this.$verificationMessage, new Result({ Code: result.Code, Message: result.Message }));
                }
            }

            return;
        }

        this.$captchaContainer.html(data);
        this.initCaptcha();
        alpha.showMessage(this.$verificationMessage, new Result({ Code: 1, Message: this.resources.verificationCodeIsInvalidOrExpired }))
    }

    resendEvent = (e: any) => {
        e.preventDefault();

        this.$.blockUI();
        var userId = this.$userId.val();
        var account = this.$messengerAccount.val();
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "ChatBot/ResendVerificationCode",
            data: { userId: userId, messengerUsername: account },
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
                clearInterval(this.timer);
                this.$countDown.text(180);
                this.$btnVerify.removeAttr("disabled");
                this.countDownTimer();
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
        var userId = this.$userId.val();
        var account = this.$messengerAccount.val();
        this.$.ajax({
            type: "GET",
            url: alpha.urls.mainRootPath + "ChatBot/Captcha",
            dataType: "text",
            success: (result) => {
                this.$captchaContainer.html(result);
                this.initCaptcha();
            },
            complete: this.onComplete,
        });
    }

    countDownTimer = () => {
        var sec = parseInt(this.$countDown.text());
        this.timer = setInterval(() => {
            if (sec == 0) {
                this.$btnVerify.attr("disabled", "disabled");
                clearInterval(this.timer);
            } else {
                this.$countDown.text(--sec);
            }
        }, 1000)
    }
}