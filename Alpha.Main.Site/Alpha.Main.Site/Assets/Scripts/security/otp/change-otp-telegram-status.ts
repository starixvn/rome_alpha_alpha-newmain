/// <reference path="../../core.ts" />

class ChangeOtpTelegramStatus {
    $: JQueryStatic;
    topWindow: any;
    $btnUnsubscribe: JQuery;
    $cbkEnableOtp: JQuery;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cachedDOMElements();
        this.registerEvents();
    }

    resources = {
        unsubscribeWarning: (<HTMLInputElement>document.getElementById('UnsubscribeWarning')).value
    }

    cachedDOMElements = () => {
        this.$btnUnsubscribe = this.$("#btnUnsubscribe");
        this.$cbkEnableOtp = this.$(".cbkEnableOtp");
    }

    registerEvents = () => {
        this.$btnUnsubscribe.click(this.unsubscribeEvent);
        this.$cbkEnableOtp.click(this.updateFunction);
    }

    unsubscribeEvent = (e: any) => {
        e.preventDefault();
        this.topWindow.bootboxConfirm(this.resources.unsubscribeWarning, undefined, () => {
            const self = this;
            this.$.blockUI();
            this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "TelegramChatBot/Unsubscribe",
                dataType: "text",
                success: function (data, textStatus, jqXHR) {
                    parent.window.location.href = parent.window.location.href;
                },
                complete: function (jqXHR, textStatus) {
                    self.$.unblockUI();
                }
            });
        });
    }

    updateFunction = (e: JQueryEventObject) => {
        var checkbox = this.$(e.target);
        var functionId = checkbox.attr("data-functionId");
        var functionName = checkbox.attr("data-functionName");
        var type = checkbox.attr("data-type");
        var enabled = checkbox.is(":checked");
        this.$.blockUI();
        var self = this;
        this.$.ajax({
            type: "POST",
            url: alpha.urls.mainRootPath + "Otp/UpdateOtpFunction",
            data: { functionId, functionName, type, enabled },
            dataType: "json",
            success: (result) => {
                if (enabled) {
                    checkbox.prop('checked', true).attr('checked', 'checked');
                }
                else {
                    checkbox.prop('checked', false).removeAttr('checked');
                }

                setTimeout(() => {
                    parent.window.location.href = parent.window.location.href;
                }, 2000);
            },
            complete: function (jqXHR, textStatus) {
                self.$.unblockUI();
            }
        });
    }
}

$(function () {
    const changeOtpTelegramStatus = new ChangeOtpTelegramStatus($, window.top);
    changeOtpTelegramStatus.init();
});